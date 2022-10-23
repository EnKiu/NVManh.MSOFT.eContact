using MS.ApplicationCore.MSEnums;
using MS.ApplicationCore.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.ApplicationCore.Entities
{
    public class BaseEntity
    {
        [NotMapQuery]
        public EntityState EntityState { get; set; }
    }
}
