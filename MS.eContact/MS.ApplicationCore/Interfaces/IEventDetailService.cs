﻿using MS.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Interfaces
{
    public interface IEventDetailService:IBaseService<EventDetail>,IAsyncService<EventDetail>
    {
        
    }
}
