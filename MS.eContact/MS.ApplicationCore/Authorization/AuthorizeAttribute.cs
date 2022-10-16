using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MS.ApplicationCore.DTOs;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Entities.Auth;
using MS.ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace MS.ApplicationCore.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<Enums.Role> _roles;

        public AuthorizeAttribute(params Enums.Role[] roles)
        {
            _roles = roles ?? new Enums.Role[] { };
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Bỏ qua việc xác thực thông tin ủy quyền nếu action được gán thuộc tính [AllowAnonymous]
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            var user = (UserInfoResponse)context.HttpContext.Items["User"];

            //var roles = (_roles.Any() && !_roles.Contains(user.Role));
            if (user == null)
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else
            {
                var userRoles = user.Roles.Where(r => _roles.Contains(r.RoleValue));
                if (userRoles == null || userRoles.Count() == 0)
                {
                    context.Result = new JsonResult(new { message = "Forbidden" }) { StatusCode = StatusCodes.Status403Forbidden };
                }
            }
        }
    }
}
