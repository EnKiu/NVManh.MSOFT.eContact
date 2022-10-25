using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MS.ApplicationCore.Exceptions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MS.ApplicationCore.Helpers;
using MS.ApplicationCore.Interface.Service;
using MS.ApplicationCore.Entities.Auth;

namespace MS.ApplicationCore.Authorization
{
    /// <summary>
    /// Middware xử lý đọc thông tin Token để trích xuất thông tin người dùng từ Token
    /// </summary>
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings, IConfiguration configuration)
        {
            _next = next;
            _appSettings = appSettings.Value;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
        {
            // Lấy Token từ header của request -> đã được gắn từ phía client:
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            // Nếu có Token thì trích xuất thông tin User từ Token: việc này thực hiện để xác thực thông tin
            if (token != null)
            {
                // Sử dụng try/ catch để loại trừ trường hợp khi đăng nhập/ đăng xuất mà có gửi 1 Token không hợp lệ:
                try
                {
                    var userId = jwtUtils.ValidateJwtToken(token);
                    if (userId != null)
                    {
                        context.Items.Add("userId", userId);
                        // Trích xuất thông tin người dùng, gắn nó vào context để sử dụng khi cần
                        var userInfo = await userService.GetUserInfoById(userId);
                        userInfo.HighestRole = userInfo.Roles.FirstOrDefault()?.RoleValue;
                        context.Items["User"] = userInfo;

                    }
                }
                catch(Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException)
                {
                    // Token hết hạn:
                    var requestUrl = context.Request.Path.Value?.ToLower();
                    if (!requestUrl.Contains("/account/login") && !requestUrl.Contains("account/register"))
                    {
                        throw new UnauthorizedException("Thông tin xác thực không hợp lệ hoặc đã hết hạn. Vui lòng đăng nhập lại!");
                    }

                }
                catch (Exception)
                {
                    //var allowAnonymous = authContext.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
                    var requestUrl = context.Request.Path.Value?.ToLower();
                    // Trừ đăng nhập/ đăng xuất ra còn lại chặn hết với trường hợp Token không hợp lệ hoặc bị hết hạn:
                    //if (!requestUrl.Contains("/authenticate/login") && !requestUrl.Contains("authenticate/register"))
                    //{
                    //    throw new UnauthorizedException("Thông tin xác thực không hợp lệ hoặc đã hết hạn. Vui lòng đăng nhập lại!");
                    //}
                }
                

            }

            // Bỏ qua nếu không gán Token:
            await _next(context);
        }
    }
}
