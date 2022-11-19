using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Services
{
    public class EventService:BaseService<Event>, IEventService
    {
        IEventRepository _repository;
        public EventService(IEventRepository repository, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, unitOfWork, mapper)
        {
            _repository = repository;
        }

        protected override async Task BeforeSave(Event entity)
        {
            // Chuyển giờ sang UTC:
            //TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Indochina Time");
            entity.StartTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)entity.StartTime, "SE Asia Standard Time");
            // Cập nhật thời gian bắt đầu sự kiện:
            if (entity.StartTime != null)
            {
                entity.EventDate = (DateTime)entity.StartTime;
            }
            if (entity.ExpireRegisterDate == null)
            {
                entity.ExpireRegisterDate = entity.EventDate;
            }
            else
            {
                entity.ExpireRegisterDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)entity.ExpireRegisterDate, "SE Asia Standard Time");
            }
        }
        protected async override Task ValidateObject(Event entity)
        {
            
            // Kiểm tra các thông tin bắt buộc nhập:
            if (String.IsNullOrEmpty(entity.EventName))
            {
                AddErrors("EventName", "Tên sự kiện không được phép để trống.");
            }

            // Ngày hết hạn đăng ký phải nhỏ hơn ngày bắt đầu sự kiện:
            if (entity.StartTime < entity.ExpireRegisterDate)
            {
                AddErrors("ExpireRegisterDate", "Hạn đăng ký không được phép lớn hơn ngày bắt đầu sự kiện.");
            }
            // Ngày bắt đầu sự kiện và hạn đăng ký phải nhỏ hơn ngày hiện tại:
            //var dateNow = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "SE Asia Standard Time"); ;
            //if (entity.EventDate < dateNow)
            //{
            //    AddErrors("EventDate", "Ngày bắt đầu sự kiện phải lớn hơn ngày hiện tại.");
            //}
        }
    }
}
