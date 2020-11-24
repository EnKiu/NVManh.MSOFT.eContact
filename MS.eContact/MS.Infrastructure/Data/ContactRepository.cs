﻿using MS.ApplicationCore.Entities;
using MS.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Infrastructure.Data
{
    public class ContactRepository:DapperRepository<Contact>
    {
        IUnitOfWork _unitOfWork = null;
        string _tableName = string.Empty;
        public ContactRepository(IUnitOfWork unitOfWork):base(unitOfWork)
        {
            _tableName = typeof(Contact).Name;
            _unitOfWork = unitOfWork;
        }
    }
}
