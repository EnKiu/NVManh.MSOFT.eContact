using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using MS.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Infrastructure.Data
{
    public class ExpenditureRepository : DapperRepository<Expenditure>,IExpenditureRepository
    {
        public ExpenditureRepository(MySqlDbContext sqlDbContext) : base(sqlDbContext)
        {
        }
    }
}
