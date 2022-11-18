﻿using AutoMapper;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Services
{
    public class OrganizationService : BaseService<Organization>,IOrganizationService
    {
        public OrganizationService(IRepository<Organization> repository, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, unitOfWork, mapper)
        {
        }
    }
}
