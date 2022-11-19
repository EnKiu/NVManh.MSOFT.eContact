using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Exceptions;
using MS.ApplicationCore.Interfaces;
using MS.ApplicationCore.Utilities;
using MS.eContact.Core;
using System;
using System.Collections.Generic;
using System.Data;
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

        protected override async Task ValidateObject(Expenditure entity)
        {
            // Kiểm tra khi thu tiền theo kế hoạch
            if (entity.ExpenditureType == MSEnums.ExpenditureType.INCREMENT_PLAN)
            {
                // Nếu là thêm mới thì kiểm tra xem khoản hiện tại đã được thêm hay chưa ? -Chỉ sử dụng khi đóng khoản thu theo kế hoạch
                if (entity.EntityState == MSEnums.EntityState.ADD)
                {
                    var hasExits = await UnitOfWork.Expenditures.CheckCurrentFundHasExits(entity);
                    if (hasExits == true)
                    {
                        throw new MISAException(System.Net.HttpStatusCode.Conflict, "Thành viên này đã đóng khoản này.");
                    }

                }

                // Kiểm tra xem nếu đóng tiền theo kế hoạch thì đã có thông tin kế hoạch hay chưa?
                if (entity.ExpenditurePlanId == null)
                {
                    AddErrors("PlanId", "Kế hoạch thu không được để trống.");
                }

            }

            if (entity.ExpenditureType == MSEnums.ExpenditureType.REDURE_PLAN)
            {
                // Kiểm tra xem nếu đóng tiền theo kế hoạch thì đã có thông tin kế hoạch hay chưa?
                if (entity.ExpenditurePlanId == null)
                {
                    AddErrors("PlanId", "Kế hoạch chi không được để trống.");
                }
            }

            // Đã có thông tin người nộp tiền/chi tiền chưa?
            if (entity.ContactId == null)
            {
                AddErrors("ContactId", "Thông tin người nộp/chi tiền không được để trống.");
            }

            // Đã có thông tin người nộp tiền chưa?
            if (entity.Amount == null || entity.Amount < 0)
            {
                AddErrors("Amount", "Số tiền phải lớn hơn 0.");
            }

        }


        protected override async Task AfterSave(Expenditure entity)
        {
            // Lấy thông tin tổng tiền mới:
            var classInfo = await UnitOfWork.Users.GetClassInfoById();
            await _notificationHub.Clients.All.SendAsync("UpdateClassInfo", classInfo);
            var fundInfo = await UnitOfWork.Expenditures.GetGeneralInfo();
            await _notificationHub.Clients.All.SendAsync("UpdateFundInfo", fundInfo);
        }

        protected override async Task BeforeSave(Expenditure entity)
        {
            entity.ExpenditureDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)entity.ExpenditureDate, "SE Asia Standard Time");
        }

        protected override async Task AfterDeleted()
        {
            // Lấy thông tin tổng tiền mới:
            var classInfo = await UnitOfWork.Users.GetClassInfoById();
            await _notificationHub.Clients.All.SendAsync("UpdateClassInfo", classInfo);

            // Cập nhật thông tin quỹ lớp:
            var fundInfo = await UnitOfWork.Expenditures.GetGeneralInfo();
            await _notificationHub.Clients.All.SendAsync("UpdateFundInfo", fundInfo);
        }
    }
}
