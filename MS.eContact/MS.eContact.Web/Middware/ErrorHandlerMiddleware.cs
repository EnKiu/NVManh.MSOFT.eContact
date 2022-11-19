using Microsoft.AspNetCore.Http;
using MS.ApplicationCore.Exceptions;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Text.Json;
using MS.ApplicationCore.DTOs;

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
                var responseObject = new ResponseObject()
                {
                    DevMsg = error.Message,
                    UserMsg = MS.ApplicationCore.Resource.Resource.Exception_Message_General,
                    errors = error.Data
                };
                switch (error)
                {
                    case WebException e:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        //FtpWebResponse newResponse = e.Response as FtpWebResponse;
                        //if (newResponse.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                        //{
                        //    responseObject.UserMsg = "Tệp hoặc thư mục không tồn tại trên server files";
                        //}
                        //else
                        //{
                        //    responseObject.UserMsg = "Không thể thực hiện xóa các tệp, vui lòng thử lại.";
                        //}
                        break;
                    case MISAException e:
                        // custom application error
                        if (e.StatusCode == HttpStatusCode.Conflict)
                        {
                            response.StatusCode = (int)HttpStatusCode.Conflict;
                        }
                        else
                        {
                            response.StatusCode = (int)HttpStatusCode.BadRequest;
                        }
                        break;
                    case MSMySqlException e:
                        // custom application error
                        response.StatusCode = (int)e.StatusCode;
                        responseObject.UserMsg = error.Message;
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

                var result = JsonSerializer.Serialize(responseObject);
                await response.WriteAsync(result);
            }
        }
    }
}
