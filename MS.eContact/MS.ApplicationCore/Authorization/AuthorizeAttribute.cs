using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MS.ApplicationCore.DTOs;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Entities.Auth;
using MS.ApplicationCore.MSEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        //private readonly IList<MSEnums.MSRole> _roles;
        private readonly MSEnums.MSRole _role;
        private readonly IList<Role> _roles;

        public AuthorizeAttribute()
        {

        }
        public AuthorizeAttribute(params Role[] roles)
        {
            _roles = roles ?? new Role[] { };
        }
        public AuthorizeAttribute(MSEnums.MSRole role)
        {
            _role = role;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Bỏ qua việc xác thực thông tin ủy quyền nếu action được gán thuộc tính [AllowAnonymous]
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            var user = (UserInfo)context.HttpContext.Items["User"];

            //var roles = (_roles.Any() && !_roles.Contains(user.Role));
            if (user == null)
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else
            {
                var hasNotPermission = (user.HighestRole == null || (_role!=0 && user.HighestRole > _role));
                // Chỉ quản lý hoặc người dùng thông thường được phép sửa thông tin liên hệ:
                var path = context.HttpContext.Request.Path.Value;
                var method = context.HttpContext.Request.Method;
                if (path == "/api/v1/contacts" && method == "PUT")
                {
                    var contactId = Guid.Parse(context.HttpContext.Request.Form["contactId"].First().ToString());
                    var currentUserId = user.ContactId;
                    if (contactId!= currentUserId && hasNotPermission)
                    {
                        context.Result = new JsonResult(new { message = "Bạn không được cấp quyền thực hiện chỉnh sửa liên hệ của người khác." }) { StatusCode = StatusCodes.Status403Forbidden };
                    }
                }
                else
                {
                    if (hasNotPermission)
                    {
                        context.Result = new JsonResult(new { message = "Bạn không có quyền thực hiện chức năng hoặc truy cập tài nguyên hiện tại." }) { StatusCode = StatusCodes.Status403Forbidden };
                    }
                }
            }
        }
    }
}
