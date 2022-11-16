using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using MS.ApplicationCore.DTOs;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Exceptions;
using MS.ApplicationCore.Interfaces;
using MS.eContact.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Services
{
    public class EventDetailService : BaseService<EventDetail>, IEventDetailService
    {
        private readonly IHubContext<NotificationHub> _notificationHub;
        public EventDetailService(IEventDetailRepository repository, IUnitOfWork unitOfWork, IMapper mapper, IHubContext<NotificationHub> notificationHub) : base(repository, unitOfWork, mapper)
        {
            _notificationHub = notificationHub;
        }

        /// <summary>
        /// Nghiệp vụ: Khi thực hiện đăng ký hoặc thêm mới một thành viên đăng ký tham gia sự kiện
        /// - Nếu có thông tin số tiền nộp thì cập nhật luôn ở bảng thu/chi - thêm mới khoản thu vào
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="MISAException"></exception>
        public override async Task<int> AddAsync(EventDetail entity)
        {
            await Validate(entity);
            if (Errors.Count == 0)
            {
                // Kiểm tra xem có thông tin số tiền nộp không, nếu có thì tự động thêm mới khoản thu/ không thì thôi -> chỉ coi như mới đăng ký:
                UnitOfWork.BeginTransaction();

                // Đầu tiên là thêm mới đăng ký đã:
                var res = await UnitOfWork.EventDetails.AddAsync(entity);

                // Thêm mới thành công thì xử lý các nghiệp vụ tiếp theo:
                if (res > 0)
                {
                    var amount = entity.SpendsTotal;
                    if (amount != null && amount > 0)
                    {
                        var eventInfo = await UnitOfWork.Events.FindAsync(entity.EventId);
                        var contactInfo = await UnitOfWork.Contacts.FindAsync(entity.ContactId);

                        // Kiểm tra xem có kế hoạch thu tiền hiện tại hay chưa? - chưa có tạo mới, có thì thêm khoản thu mới:
                        var expenditurePlan = await UnitOfWork.ExpenditurePlans.GetIncrementExpenditurePlanByEventId(entity.EventId);

                        // Khai báo khoản thu mới:
                        var expenditure = new Expenditure()
                        {
                            ExpenditureId = Guid.NewGuid(),
                            ExpenditureDate = DateTime.Now,
                            ContactId = entity.ContactId,
                            EntityState = MSEnums.EntityState.ADD,
                            ExpenditureType = MSEnums.ExpenditureType.INCREMENT_PLAN,
                            Amount = (decimal)entity.SpendsTotal,
                            ExpenditureName = $"[{contactInfo.LastName}] nộp tiền cho sự kiện [{eventInfo.EventName}]",
                            Description = $"[Đóng quỹ cho sự kiện [{eventInfo.EventName}]"
                        };

                        // Không có kế hoạch thu cho sự kiện hiện tại thì thêm mới:
                        if (expenditurePlan == null)
                        {
                            var newExpenditurePlan = new ExpenditurePlan()
                            {
                                ExpenditurePlanId = Guid.NewGuid(),
                                ExpenditurePlanName = $"Thu kinh phí cho sự kiện [{eventInfo.EventName}]",
                                StartDate = DateTime.Now,
                                EndDate = eventInfo.EventDate,
                                Description = $"Kế hoạch thu kinh phí cho sự kiện [{eventInfo.EventName}]",
                                EntityState = MSEnums.EntityState.ADD,
                                EventId = eventInfo.EventId,
                                AmountUnit = eventInfo.Spends,
                                ExpenditurePlanType = MSEnums.ExpenditurePlanType.INCREMENT_EVENT,
                                IsFinish = false
                            };

                            // Thêm kế hoạch mới:
                            await UnitOfWork.ExpenditurePlans.AddAsync(newExpenditurePlan);

                            // Cập nhật kế hoạch thu chi cho khoản thu:
                            expenditure.ExpenditurePlanId = newExpenditurePlan.ExpenditurePlanId;

                        }

                        // Thêm khoản thu vào Database:
                        await UnitOfWork.Expenditures.AddAsync(expenditure);

                        // Thông báo cho các client biết liên hệ đã đăng ký sự kiện thành công:
                        await _notificationHub.Clients.All.SendAsync("RecieveNotifiedWhenContactRegistedEventSuccess", eventInfo, contactInfo);
                    }
                }
                else
                {
                    throw new MISAException(System.Net.HttpStatusCode.InternalServerError, "Không thể đăng ký sự kiện cho thành viên, vui lòng kiểm tra lại.");
                }
                UnitOfWork.Commit();
                return res;
            }
            else
                throw new MISAException(System.Net.HttpStatusCode.BadRequest, Errors);
        }
        protected async Task Validate(EventDetail entity)
        {
            var validateErrors = new List<string>();
            // Kiểm tra xem người đăng ký hiện tại đã đăng ký chưa?
            var res = await UnitOfWork.EventDetails.CheckRegisted(entity);
            if (res == true)
            {
                validateErrors.Add("Thành viên đã đăng ký tham gia sự kiện.");
                Errors.Add("errors", validateErrors);
            }
        }

        public async override Task<int> UpdateAsync(EventDetail entity, object pks)
        {
            // Kiểm tra xem có thông tin số tiền nộp không, nếu có thì tự động thêm mới khoản thu/ không thì thôi -> chỉ coi như mới đăng ký:
            UnitOfWork.BeginTransaction();

            // Đầu tiên là cập nhật thông tin đăng ký đã:
            var res = await base.UpdateAsync(entity, pks);

            // Cập nhật thành công thì xử lý các nghiệp vụ tiếp theo:
            if (res > 0)
            {
                var amount = entity.SpendsTotal;
                if (amount != null && amount > 0)
                {
                    var eventInfo = await UnitOfWork.Events.FindAsync(entity.EventId);
                    var contactInfo = await UnitOfWork.Contacts.FindAsync(entity.ContactId);

                    // Kiểm tra xem có kế hoạch thu tiền hiện tại hay chưa? - chưa có tạo mới, có thì thêm khoản thu mới:
                    var expenditurePlan = await UnitOfWork.ExpenditurePlans.GetIncrementExpenditurePlanByEventId(entity.EventId);
                    var sortDescription = "";
                    if (entity.NumberAccompanying != null && entity.NumberAccompanying > 0)
                    {
                        sortDescription = $"Có {entity.NumberAccompanying} người đi kèm";
                    }
                    // Khai báo khoản thu mới:
                    var expenditure = new Expenditure()
                    {
                        ExpenditureId = Guid.NewGuid(),
                        ExpenditureDate = DateTime.Now,
                        ContactId = entity.ContactId,
                        EntityState = MSEnums.EntityState.ADD,
                        ExpenditureType = MSEnums.ExpenditureType.INCREMENT_PLAN,
                        Amount = (decimal)entity.SpendsTotal,
                        ExpenditureName = $"[{contactInfo.FullName}] nộp tiền cho sự kiện [{eventInfo.EventName}]",
                        Description = $"Đóng quỹ cho sự kiện [{eventInfo.EventName}] ({sortDescription})",
                        ExpenditurePlanId = expenditurePlan.ExpenditurePlanId
                    };

                    // Không có kế hoạch thu cho sự kiện hiện tại thì thêm mới:
                    if (expenditurePlan == null)
                    {
                        var newExpenditurePlan = new ExpenditurePlan()
                        {
                            ExpenditurePlanId = Guid.NewGuid(),
                            ExpenditurePlanName = $"Thu kinh phí cho sự kiện [{eventInfo.EventName}]",
                            StartDate = DateTime.Now,
                            EndDate = eventInfo.EventDate,
                            Description = $"Kế hoạch thu kinh phí cho sự kiện [{eventInfo.EventName}]",
                            EntityState = MSEnums.EntityState.ADD,
                            EventId = eventInfo.EventId,
                            AmountUnit = eventInfo.Spends,
                            ExpenditurePlanType = MSEnums.ExpenditurePlanType.INCREMENT_EVENT,
                            IsFinish = false
                        };

                        // Thêm kế hoạch mới:
                        await UnitOfWork.ExpenditurePlans.AddAsync(newExpenditurePlan);

                        // Cập nhật kế hoạch thu chi cho khoản thu:
                        expenditure.ExpenditurePlanId = newExpenditurePlan.ExpenditurePlanId;

                    }

                    // Thêm khoản thu vào Database:
                    await UnitOfWork.Expenditures.AddAsync(expenditure);

                    // Lấy thông tin tổng tiền mới:
                    var classInfo = await UnitOfWork.Users.GetClassInfoById();
                    await _notificationHub.Clients.All.SendAsync("UpdateClassInfo", classInfo);
                }
            }
            else
            {
                throw new MISAException(System.Net.HttpStatusCode.InternalServerError, "Không thể đăng ký sự kiện cho thành viên, vui lòng kiểm tra lại.");
            }
            UnitOfWork.Commit();
            return res;

        }
    }
}
