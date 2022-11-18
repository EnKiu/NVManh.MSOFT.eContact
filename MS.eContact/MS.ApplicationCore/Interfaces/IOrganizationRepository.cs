using MS.ApplicationCore.DTOs;
using MS.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Interfaces
{
    public interface IOrganizationRepository:IRepository<Organization>
    {
        Task<IEnumerable<OrganizationRegister>> GetAll();
        Task<IEnumerable<ContactRegister>> GetContactsRegister(string organizationId);
    }
}
