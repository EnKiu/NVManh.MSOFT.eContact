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
        IAlbumRepository _repository;
        IPictureRepository _pictureRepository;
        IFileTransfer _fileTransfer;
        IUnitOfWork _unitOfWork;
        public AlbumService(IAlbumRepository repository, IFileTransfer fileTransfer, IPictureRepository pictureRepository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
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
            _unitOfWork.Connection.Open();
            _unitOfWork.BeginTransaction();
            var addAlbumResult = await _repository.AddAsync(entity);
            if (addAlbumResult > 0)
            {
                // Thêm mới ảnh vào album, sau đó thêm luôn object picture vào database:
                var files = entity.PictureFiles;
                var totalPicturesAdd = 0;
                // Tạo thư mục tương ứng với album trên server file:
                //var res = _fileTransfer.MakeFolderInFileServer();
                foreach (var file in files)
                {
                    var pic = new Picture() { PictureId = Guid.NewGuid(), AlbumId = entity.AlbumId };
                    pic.UrlPath = await _fileTransfer.UploadFile(file, "pictures", pic.PictureId.ToString());
                    totalPicturesAdd += await _pictureRepository.AddAsync(pic);
                }
                _unitOfWork.Commit();
                _unitOfWork.Connection.Close();
            }

            return addAlbumResult;
        }
    }
}
