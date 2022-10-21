using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.DTOs
{
    public class ResponseObject
    {
        public string DevMsg { get; set; }
        public string UserMsg { get; set; }
        public IDictionary errors { get; set; }
    }
}
