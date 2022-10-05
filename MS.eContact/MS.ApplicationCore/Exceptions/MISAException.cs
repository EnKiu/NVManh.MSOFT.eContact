using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MS.ApplicationCore.Exceptions
{
    public class MISAException : Exception
    {
        private string Msg;
        IDictionary Errors;
        public HttpStatusCode StatusCode;

        public MISAException(HttpStatusCode statusCode, string msg)
        {
            Msg = msg;
            StatusCode = statusCode;
        }

        public MISAException(HttpStatusCode statusCode,Dictionary<string,List<string>> errors)
        {
            Errors = errors;
            StatusCode = statusCode;
        }

        public MISAException(string msg, List<string>? errors = null)
        {
            Msg = msg;
            Errors = new Dictionary<string, List<string>>();
            if (errors == null)
            {
                errors = new List<string>();
                errors.Add(msg);
            };
            Errors.Add("ValidErrors", errors);
        }

        public override string Message => this.Msg;
        public override IDictionary Data => this.Errors;
    }
}
