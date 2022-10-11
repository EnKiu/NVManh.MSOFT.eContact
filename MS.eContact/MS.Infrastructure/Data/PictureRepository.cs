using Dapper;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using MS.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MS.Infrastructure.Data
{
    public class PictureRepository : DapperRepository<Picture>, IPictureRepository
    {
        IUnitOfWork _unitOfWork;
        public PictureRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async override Task<int> AddAsync(Picture entity)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@PictureId", entity.PictureId.ToString());
            parameters.Add("@Description", entity.Description);
            parameters.Add("@UrlPath", entity.UrlPath);
            parameters.Add("@AlbumId", entity.AlbumId.ToString());
            var rowAffects = await _unitOfWork.Connection.ExecuteAsync($"Proc_Picture_Insert", param: parameters, transaction: _unitOfWork.Transaction, commandType: System.Data.CommandType.StoredProcedure);
            return rowAffects;
        }
    }
}
