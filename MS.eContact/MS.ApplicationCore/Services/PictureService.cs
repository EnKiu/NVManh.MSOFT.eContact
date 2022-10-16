using AutoMapper;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.ApplicationCore.Services
{
    public class PictureService:BaseService<Picture>,IPictureService
    {
        IPictureRepository _pictureRepository;
        public PictureService(IPictureRepository repository, IUnitOfWork unitOfWork,IMapper mapper) : base(repository, unitOfWork, mapper)
        {
            _pictureRepository = repository;
        }
    }
}
