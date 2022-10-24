using Microsoft.AspNetCore.Http;
using MS.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Utilities
{
    public class CommonFunction: ICommonFunction
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CommonFunction(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User?.Claims?.First(x => x.Type == "id").Value;
            return userId;
        }
    }
}
