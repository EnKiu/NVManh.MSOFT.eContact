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
    public class AlbumRepository : DapperRepository<Album>, IAlbumRepository
    {
        IUnitOfWork _unitOfWork;
        public AlbumRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async override Task<int> AddAsync(Album entity)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@AlbumId", entity.AlbumId.ToString());
            parameters.Add("@AlbumName", entity.AlbumName);
            parameters.Add("@Description", entity.Description);
            parameters.Add("@AvatarLink", entity.AvatarLink);
            parameters.Add("@AuthId", entity.AuthId);
            var rowAffects = await _unitOfWork.Connection.ExecuteAsync($"Proc_Album_InsertAlbum", param: parameters, transaction: _unitOfWork.Transaction, commandType: System.Data.CommandType.StoredProcedure);
            return rowAffects;
        }
    }
}
