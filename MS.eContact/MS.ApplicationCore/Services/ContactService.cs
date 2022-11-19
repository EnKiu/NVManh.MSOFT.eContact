using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using MS.ApplicationCore.DTOs;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using MS.eContact.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Services
{
    public class ContactService : BaseService<Contact>, IContactService
    {
        private readonly IHubContext<NotificationHub> _notificationHub;
        public ContactService(IContactRepository repository, IUnitOfWork unitOfWork, IMapper mapper, IHubContext<NotificationHub> notificationHub) : base(repository, unitOfWork, mapper)
        {
            _notificationHub = notificationHub;
        }
        public async override Task<int> AddAsync(Contact entity)
        {
            var res = await base.AddAsync(entity);
            // Lấy thông tin tổng tiền mới:
            var classInfo = await UnitOfWork.Users.GetClassInfoById();
            await _notificationHub.Clients.All.SendAsync("UpdateClassInfo", classInfo);
            return res;
        }

        public async override Task<int> RemoveAsync(object key)
        {
            var res = await base.RemoveAsync(key);
            // Lấy thông tin tổng tiền mới:
            var classInfo = await UnitOfWork.Users.GetClassInfoById();
            await _notificationHub.Clients.All.SendAsync("UpdateClassInfo", classInfo);
            return res;
        }
    }
}
