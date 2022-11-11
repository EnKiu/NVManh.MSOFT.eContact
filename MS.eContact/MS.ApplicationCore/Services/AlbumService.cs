using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Exceptions;
using MS.ApplicationCore.Interfaces;
using MS.eContact.Core;
using System;
using System.Collections.Generic;
using System.Linq;
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
        readonly ICommonFunction _commonFunction;
        //HttpContextAccessor _httpContext;
        private readonly IHubContext<NotificationHub> _notificationHub;
        public AlbumService(IAlbumRepository repository, IFileTransfer fileTransfer, IPictureRepository pictureRepository, IUnitOfWork unitOfWork, IMapper mapper, IHubContext<NotificationHub> notificationHub, ICommonFunction commonFunction) : base(repository, unitOfWork, mapper)
        {
            _repository = repository;
            _fileTransfer = fileTransfer;
            _pictureRepository = pictureRepository;
            _unitOfWork = unitOfWork;
            _notificationHub = notificationHub;
            _commonFunction = commonFunction;
            //_httpContext = httpContext;
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
                var totalFiles = files.Count;
                //var userId = _httpContext.HttpContext.User?.Claims?.First(x => x.Type == "id").Value;
                var userId = _commonFunction.GetCurrentUserId();
                var connections = NotificationHub._connections;
                var isFinish = false;
                var timeStart = DateTime.Now;
                double totalTimes = 0;
                var progressInfo = new
                {
                    Id = entity.AlbumId,
                    Name = entity.AlbumName
                };
                for (int i = 0; i < files.Count; i++)
                {
                    if (i == files.Count() - 1)
                        isFinish = true;
                    var pic = new Picture() { PictureId = Guid.NewGuid(), AlbumId = entity.AlbumId };
                    pic.UrlPath = await _fileTransfer.UploadFile(files[i], $"pictures/{albumFolderName}", pic.PictureId.ToString());
                    totalPicturesAdd += await _pictureRepository.AddAsync(pic);

                    //await _notificationHub.Clients.User(userId).SendAsync("ShowPecentUpload", i + 1, totalFiles,userId);
                    //await _notificationHub.Clients.All.SendAsync("ShowPecentUpload", i + 1, totalFiles, userId);
                    foreach (var connectionId in connections.GetConnections(userId))
                    {
                        totalTimes = (DateTime.Now - timeStart).TotalSeconds;
                        await _notificationHub.Clients.Client(connectionId).SendAsync("ShowPecentUpload", i + 1, totalFiles, isFinish, totalTimes, progressInfo);
                    }
                }
                _unitOfWork.Commit();
            }

            return addAlbumResult;
        }

        public override async Task<int> RemoveAsync(object key)
        {
            Guid.TryParse(key.ToString(), out var albumId);
            // Lấy toàn bộ thông tin ảnh có trong album:
            UnitOfWork.BeginTransaction();
            var pictures = await UnitOfWork.Albums.GetPicturesByAlbumId(albumId);
            var album = await UnitOfWork.Albums.FindAsync(albumId);
            var res = await base.RemoveAsync(key);
            var count = 1;
            var totalFiles = pictures.Count();
            var userId = _commonFunction.GetCurrentUserId();
            var connections = NotificationHub._connections;
            var isFinish = false;
            var timeStart = DateTime.Now;
            double totalTimes = 0;
            var progressInfo = new
            {
                Id = albumId,
                Name = album.AlbumName
            };
            foreach (var pic in pictures)
            {
                if (count == pictures.Count())
                    isFinish = true;
                _ = _fileTransfer.DeleteFile(pic.UrlPath);
                foreach (var connectionId in connections.GetConnections(userId))
                {
                    totalTimes = (DateTime.Now - timeStart).TotalSeconds;
                    await _notificationHub.Clients.Client(connectionId).SendAsync("ShowPecentDeleted", count, totalFiles, isFinish, totalTimes, progressInfo);
                }
                count++;
            }
            _ = _fileTransfer.RemoveFolderInFileServer($"pictures/{albumId.ToString()}");
            foreach (var connectionId in connections.GetConnections(userId))
            {
                totalTimes = (DateTime.Now - timeStart).TotalSeconds;
                await _notificationHub.Clients.Client(connectionId).SendAsync("ShowPecentDeleted", count, totalFiles, isFinish, totalTimes, progressInfo);
            }
            UnitOfWork.Commit();
            //Thực hiện xóa ảnh từ service files
            return res;
        }
    }
}
