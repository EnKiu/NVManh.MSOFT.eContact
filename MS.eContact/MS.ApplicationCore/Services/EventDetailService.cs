using AutoMapper;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Exceptions;
using MS.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Services
{
    public class EventDetailService : BaseService<EventDetail>, IEventDetailService
    {
        public EventDetailService(IEventDetailRepository repository,IUnitOfWork unitOfWork, IMapper mapper) : base(repository, unitOfWork, mapper)
        {
        }
        public override async Task<int> AddAsync(EventDetail entity)
        {
            await Validate(entity);
            if (Errors.Count == 0)
                return await UnitOfWork.EventDetails.AddAsync(entity);
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
    }
}
