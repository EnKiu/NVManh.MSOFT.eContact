using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MS.ApplicationCore.Interfaces;
using MS.ApplicationCore.Services;
using MS.ApplicationCore.Utilities;
using MS.Infrastructure;
using MS.Infrastructure.Data;
using MS.Infrastructure.UnitOfWork;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MS.eContact.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //CommonConst.ServerFileUrl = Configuration["ServerFiles"];
            CommonConst.ServerFileUrl = String.Format("{0}/{1}", Configuration["ServerFiles"], Configuration["ServerFilePath"]);
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                }); ;

            SqlMapper.AddTypeHandler(new MySqlGuidTypeHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));
            services.AddCors();
            //services.AddDbContext<DataContex>(options =>
            //              options.UseMySql(
            //                  Configuration.GetConnectionString("DefaultConnection")));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MS.eContact.Web", Version = "v1" });
            });
            //services.AddScoped<IDbConnection>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(DapperRepository<>));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(DapperRepository<>));
            services.AddScoped(typeof(IAsyncService<>), typeof(BaseService<>));
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            services.AddScoped<IFileTransfer, FileTransfer>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IEventDetailService, EventDetailService>();
            services.AddScoped<IEventDetailRepository, EventDetailRepository>();
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
            app.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            //app.UseFileServer();
            //app.UseStaticFiles();
            //app.UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = new List<string> { "index.html" } });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
