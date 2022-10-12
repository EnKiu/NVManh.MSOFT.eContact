using Dapper;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using MS.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MS.Infrastructure.Data
{
    public class AlbumRepository : DapperRepository<Album>, IAlbumRepository
    {
        public AlbumRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        public async override Task<IEnumerable<Album>> AllAsync()
        {
            var storeName = "Proc_Album_GetAll";
            var albums = await UnitOfWork.Connection.QueryAsync<Album>(storeName,transaction: UnitOfWork.Transaction, commandType: System.Data.CommandType.StoredProcedure);
            return albums;
        }
        public async override Task<int> AddAsync(Album entity)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@AlbumId", entity.AlbumId.ToString());
            parameters.Add("@AlbumName", entity.AlbumName);
            parameters.Add("@Description", entity.Description);
            parameters.Add("@AvatarLink", entity.AvatarLink);
            parameters.Add("@AuthId", entity.AuthId);
            var rowAffects = await UnitOfWork.Connection.ExecuteAsync($"Proc_Album_InsertAlbum", param: parameters, transaction: UnitOfWork.Transaction, commandType: System.Data.CommandType.StoredProcedure);
            return rowAffects;
        }

        public async Task<IEnumerable<Picture>> GetPicturesByAlbumId(Guid albumId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@p_AlbumId", albumId);
            var pictures = await UnitOfWork.Connection.QueryAsync<Picture>($"Proc_Picture_GetPicturesByAlbumId", param: parameters, transaction: UnitOfWork.Transaction, commandType: System.Data.CommandType.StoredProcedure);
            return pictures;
        }

        public async Task<int> UpdateTotalViewAlbum(Guid albumId)
        {
            var sql = "UPDATE Album a SET a.TotalViews = IFNULL(a.TotalViews,0)+1 WHERE a.AlbumId = @AlbumId";
            var parameters = new DynamicParameters();
            parameters.Add("@AlbumId", albumId);
            var res = await UnitOfWork.Connection.ExecuteAsync(sql, param: parameters, transaction: UnitOfWork.Transaction, commandType: System.Data.CommandType.Text);
            return res;
        }
    }
}
