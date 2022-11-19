using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Exceptions
{
    public class MSMySqlException:Exception
    {
        private string Msg;
        IDictionary Errors;
        public HttpStatusCode StatusCode;
        public MSMySqlException(HttpStatusCode statusCode, string msg)
        {
            Msg = msg;
            StatusCode = statusCode;
        }
        public override string Message => this.Msg;
        public override IDictionary Data => this.Errors;
    }
}
