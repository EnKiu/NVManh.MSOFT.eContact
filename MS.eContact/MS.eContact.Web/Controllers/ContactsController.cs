using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS.eContact.Web.Controllers
{
    public class ContactsController : BaseController<Contact>
    {
        IRepository<Contact> _repository;
        public ContactsController(IRepository<Contact> repository): base(repository)
        {

        }
    }
}
