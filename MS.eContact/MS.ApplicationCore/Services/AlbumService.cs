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
    public class AlbumService : BaseService<Album>, IAlbumService
    {
        readonly IAlbumRepository _repository;
        readonly IPictureRepository _pictureRepository;
        readonly IFileTransfer _fileTransfer;
        readonly IUnitOfWork _unitOfWork;
        public AlbumService(IAlbumRepository repository, IFileTransfer fileTransfer, IPictureRepository pictureRepository, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, unitOfWork, mapper)
        {
            _repository = repository;
            _fileTransfer = fileTransfer;
            _pictureRepository = pictureRepository;
            _unitOfWork = unitOfWork;
        }
        public async override Task<int> AddAsync(Album entity)
        {
            entity.AlbumId = Guid.NewGuid();
            var errorsMsg = new List<string>();
            //Thực hiện validate dữ liệu
            if (string.IsNullOrEmpty(entity.AlbumName))
            {
                errorsMsg.Add("Tên Album không được phép để trống.");
                Errors.Add("AlbumName", errorsMsg);
                throw new MISAException(System.Net.HttpStatusCode.BadRequest, Errors);
            }
            // Thêm Album vào database:
            //_unitOfWork.Connection.Open();
            _unitOfWork.BeginTransaction();
            var addAlbumResult = await _repository.AddAsync(entity);
            if (addAlbumResult > 0)
            {
                // Tạo tên album:
                var albumFolderName = entity.AlbumId.ToString();

                // Thêm mới ảnh vào album, sau đó thêm luôn object picture vào database:
                var files = entity.PictureFiles;
                var totalPicturesAdd = 0;
                // Tạo thư mục tương ứng với album trên server file:
                //var res = _fileTransfer.MakeFolderInFileServer();
                foreach (var file in files)
                {
                    var pic = new Picture() { PictureId = Guid.NewGuid(), AlbumId = entity.AlbumId };
                    pic.UrlPath = await _fileTransfer.UploadFile(file, $"pictures/{albumFolderName}", pic.PictureId.ToString());
                    totalPicturesAdd += await _pictureRepository.AddAsync(pic);
                }
                _unitOfWork.Commit();
                //_unitOfWork.Connection.Close();
            }

            return addAlbumResult;
        }

        public override async Task RemoveAsync(object key)
        {
            Guid.TryParse(key.ToString(), out var albumId);
            // Lấy toàn bộ thông tin ảnh có trong album:
            UnitOfWork.BeginTransaction();
            var pictures = await UnitOfWork.Albums.GetPicturesByAlbumId(albumId);
            await base.RemoveAsync(key);
            foreach (var pic in pictures)
            {
                _ = _fileTransfer.DeleteFile(pic.UrlPath);
            }
            _ = _fileTransfer.RemoveFolderInFileServer($"pictures/{albumId.ToString()}");
            UnitOfWork.Commit();
            //Thực hiện xóa ảnh từ service files
        }
    }
}
