using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MS.ApplicationCore.Authorization;
using MS.ApplicationCore.Helpers;
using MS.ApplicationCore.Interface.Service;
using MS.ApplicationCore.Interfaces;
using MS.ApplicationCore.Services;
using MS.ApplicationCore.Utilities;
using MS.eContact.Core;
using MS.eContact.Web.Middware;
using MS.Infrastructure;
using MS.Infrastructure.Data;
using MS.Infrastructure.UnitOfWork;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AutoMapper;

namespace MS.eContact.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public string _specificOrigins = "MSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //CommonConst.ServerFileUrl = Configuration["ServerFiles"];
            CommonConst.ServerFileUrl = String.Format("{0}/{1}", Configuration["ServerFiles"], Configuration["ServerFilePath"]);
            
            // Adding Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/NotificationHub")))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            
           

            //SqlMapper.AddTypeHandler(new MySqlGuidTypeHandler());
            //SqlMapper.AddTypeHandler(new MySqlGuidWithNullTypeHandler());
            //SqlMapper.RemoveTypeMap(typeof(Guid));
            //SqlMapper.RemoveTypeMap(typeof(Guid?));
            // Add services to the container.
            services.AddCors(options =>
            {
                options.AddPolicy(_specificOrigins,
                                      builder =>
                                      {
                                          builder.WithOrigins("http://localhost:5002",
                                                              "http://localhost:5005",
                                                              "http://a1.manhnv.net",
                                                              "http://wwww.a1.manhnv.net",
                                                              "https://a1.manhnv.net",
                                                              "https://wwww.a1.manhnv.net",
                                                              "http://a1api.nmanh.com",
                                                              "https://wwww.a1api.nmanh.com",
                                                              "https://a1api.nmanh.com")
                                                              .AllowAnyHeader()
                                                              .AllowAnyMethod()
                                                              .AllowCredentials();
                                      });
            });
            services
                .AddSignalR()
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                });
            services.AddApplication();
            services.AddControllers()
               .AddNewtonsoftJson(options =>
               {
                   options.SerializerSettings.ContractResolver = new DefaultContractResolver();
               });
            // Cấu hình liên quan đến Files:
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<FormOptions>(x =>
            {
                x.ValueCountLimit = int.MaxValue;
                x.ValueLengthLimit = 1024 * 1024 * 100;// int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            });

            services.Configure<KestrelServerOptions>(x =>
            {
                x.Limits.MaxRequestBufferSize = int.MaxValue;
                x.Limits.MaxRequestBodySize = int.MaxValue;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MS.eContact.Web", Version = "v1" });
            });
            //services.AddScoped<IDbConnection>();
           

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MS.eContact.Web v1"));
            }
            
            app.UseHttpsRedirection();

            // Authentication & Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // global cors policy
            app.UseCors(_specificOrigins);
            app.UseMiddleware<ErrorHandlerMiddleware>();

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();
            app.UseRouting();

            app.UseAuthorization();
            //app.UseFileServer();
            //app.UseStaticFiles();
            //app.UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = new List<string> { "index.html" } });
            app.UseWebSockets();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("NotificationHub");
                endpoints.MapControllers();
            });
        }
    }
}
