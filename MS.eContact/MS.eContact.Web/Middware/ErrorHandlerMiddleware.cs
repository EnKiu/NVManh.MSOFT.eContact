using Microsoft.AspNetCore.Http;
using MS.ApplicationCore.Exceptions;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Text.Json;

namespace MS.eContact.Web.Middware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case MISAException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case UnauthorizedException e:
                        // Unauthorized
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
#if DEBUG
                        Console.WriteLine(error.Message);
#endif
                        break;
                }
                var responseObject = new
                {
                    DevMsg = error.Message,
                    UserMsg = MS.ApplicationCore.Resource.Resource.Exception_Message_General,
                    errors = error.Data
                };
                var result = JsonSerializer.Serialize(responseObject);
                await response.WriteAsync(result);
            }
        }
    }
}
