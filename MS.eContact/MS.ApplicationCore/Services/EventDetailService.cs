using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Services
{
    public class EventDetailService : BaseService<EventDetail>, IEventDetailService
    {
        IEventDetailRepository _repository;
        public EventDetailService(IEventDetailRepository repository,IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
            _repository = repository;
        }

        protected override async void ValidateObject(EventDetail entity)
        {
            var validateErrors = new List<string>();
            // Kiểm tra xem người đăng ký hiện tại đã đăng ký chưa?
            var res = await _repository.CheckRegisted(entity);
            if (res == true)
            {
                validateErrors.Add("Thành viên đã đăng ký tham gia sự kiện.");
                Errors.Add("errors", validateErrors);
            }
        }
    }
}
