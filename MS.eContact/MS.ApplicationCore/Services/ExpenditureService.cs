using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Exceptions;
using MS.ApplicationCore.Interfaces;
using MS.eContact.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Services
{
    public class ExpenditureService : BaseService<Expenditure>, IExpenditureService
    {
        private readonly IHubContext<NotificationHub> _notificationHub;
        public ExpenditureService(IRepository<Expenditure> repository, IUnitOfWork unitOfWork, IMapper mapper, IHubContext<NotificationHub> notificationHub) : base(repository, unitOfWork, mapper)
        {
            _notificationHub = notificationHub;
        }
        public override async Task<int> AddAsync(Expenditure entity)
        {
            // Kiểm tra xem khoản hiện tại đã được thêm hay chưa? - Chỉ sử dụng khi đóng khoản thu theo kế hoạch
            if (entity.ExpenditureType == MSEnums.ExpenditureType.INCREMENT_PLAN)
            {
                var hasExits = await UnitOfWork.Expenditures.CheckCurrentFundHasExits(entity);
                if (hasExits == true)
                {
                    throw new MISAException(System.Net.HttpStatusCode.Conflict, "Thành viên này đã đóng khoản này.");
                }
            }
            var res =  await base.AddAsync(entity);

            // Lấy thông tin tổng tiền mới:
            var classInfo = await UnitOfWork.Users.GetClassInfoById();
            await _notificationHub.Clients.All.SendAsync("UpdateClassInfo", classInfo);
            return res;
        }

        protected override void BeforeSave(Expenditure entity)
        {
            entity.ExpenditureDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)entity.ExpenditureDate, "SE Asia Standard Time");
        }

        public async override Task<int> UpdateAsync(Expenditure entity, object pks)
        {
            var res = await base.UpdateAsync(entity, pks);
            // Lấy thông tin tổng tiền mới:
            var classInfo = await UnitOfWork.Users.GetClassInfoById();
            await _notificationHub.Clients.All.SendAsync("UpdateClassInfo", classInfo);
            return res;
        }
    }
}
