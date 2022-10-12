using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using MS.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Infrastructure.Data
{
    public class ContactRepository:DapperRepository<Contact>
    {
        string _tableName = string.Empty;
        public ContactRepository(IUnitOfWork unitOfWork):base(unitOfWork)
        {
            _tableName = typeof(Contact).Name;
        }
    }
}
