using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using MS.ApplicationCore.Utilities;
using MS.ApplicationCore.Authorization;
using static System.Net.WebRequestMethods;
using MS.ApplicationCore.Exceptions;
using MS.Infrastructure.UnitOfWork;

namespace MS.eContact.Web.Controllers
{
    public class ContactsController : BaseController<Contact>
    {
        IContactRepository _repository;
        IContactService _service;
        IConfiguration _configuration;
        IUnitOfWork _unitOfWork;
        readonly IFileTransfer _fileTransfer;
        private readonly IWebHostEnvironment _env;
        public ContactsController(IContactRepository repository, IConfiguration configuration, IWebHostEnvironment env, IFileTransfer fileTransfer, IContactService baseService, IUnitOfWork unitOfWork) : base(repository, baseService)
        {
            _configuration = configuration;
            _env = env;
            _repository = repository;
            _fileTransfer = fileTransfer;
            _service = baseService;
            _unitOfWork = unitOfWork;
        }

        public async override Task<IActionResult> Get()
        {
            return Ok(await _unitOfWork.Contacts.AllAsync());
        }

        [AllowAnonymous]
        [HttpGet("register")]
        public async Task<IActionResult> GetListContacts()
        {
            var data = await _repository.AllAsync();
            return Ok(data);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post()
        {
            var httpRequest = HttpContext.Request;
            Contact contact = new Contact();
            if (httpRequest.Form["contact"].FirstOrDefault() != null)
            {
                contact = JsonConvert.DeserializeObject<Contact>(httpRequest.Form["contact"].First());
                contact.ContactId = Guid.NewGuid();
            }
            var file = httpRequest.Form.Files.FirstOrDefault();
            await ProcessAvatar(file, contact);
            var rowAffects = await _service.AddAsync(contact);
            if (rowAffects > 0)
                return StatusCode(201, rowAffects);
            else
                return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Put(string id)
        {
            var httpRequest = HttpContext.Request;
            Guid? contactId;
            Contact contact = new Contact();
            if (httpRequest.Form["contactId"].FirstOrDefault() != null)
            {
                contactId = Guid.Parse(httpRequest.Form["contactId"].First().ToString());
            }
            if (httpRequest.Form["contact"].FirstOrDefault() != null)
            {
                contact = JsonConvert.DeserializeObject<Contact>(httpRequest.Form["contact"].First());
            }

            var file = httpRequest.Form.Files.FirstOrDefault();
            if (file != null && file.Length > 0)
            {
               
                // Dung lượng File được phép tải (lấy từ app config):
                int maxSizeConfig = int.Parse("10000");
                int maxContentLength = 1024 * 1024 * maxSizeConfig; //Size = 1 MB  

                // Định dạng File được phép tải (lấy từ app Config): 
                //int fileType = int.Parse(CommonUtility.GetAppSettingByKey("AvatarImgExtensionsAllowed"));
                IList<string> allowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };

                // Tên File
                var ext = file.FileName.Substring(file.FileName.LastIndexOf('.'));

                // Phần mở rộng của File (Ex: .jpg, .png...)
                var extension = ext.ToLower();

                // Kiểm tra tên file và định dạng File có hợp lệ không:
                if (!allowedFileExtensions.Contains(extension))
                {
                    var message = string.Format("Vui lòng chỉ chọn File có định dạng .jpg,.gif,.png.");
                    return BadRequest(message);
                }
                // Kiểm tra dung lượng File:
                else if (file.Length > maxContentLength)
                {

                    var message = string.Format("Vui lòng chọn File có dung lượng tối đa 1 MB.");
                    //throw new System.InvalidOperationException("Vui lòng chọn File có dung lượng tối đa 1 MB");
                    return BadRequest(message);
                }
                else
                {

                    // Upload file sang server files -> bắt đầu từ 03/10/2022 thì file được upload sang server riêng:
                    // Thông tin server file xem trong appsetting.json:
                    contact.AvatarLink = await _fileTransfer.UploadFile(file, "avatars", contact.ContactId.ToString());

                    // Đoạn dưới này là upload file trực tiếp vào máy chủ chứa mã nguồn - bỏ đi từ 03/10/2022
                    //// Đường dẫn chứa file trên máy chủ:
                    //string userAvatarPath = @"Content/imgs/users/avatar/";
                    //var folderPath = Path.Combine(_env.WebRootPath, userAvatarPath);
                    //var filePath = Path.Combine(_env.WebRootPath, userAvatarPath + id + extension);

                    //// Kiểm tra xem thư mục có tồn tại trên máy chủ hay không, nếu không có thì thực hiện tạo mới:
                    //if (!Directory.Exists(folderPath))
                    //{
                    //    Directory.CreateDirectory(folderPath);
                    //}

                    //// Thực hiện lưu File trên máy chủ:
                    //using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    //{
                    //    await file.CopyToAsync(fileStream);
                    //}
                    ////file.SaveAs(filePath);

                    //// Sau khi lưu xong thực hiện cập nhật thông tin Avatar cho liên hệ:
                    //contact.AvatarLink = userAvatarPath.Replace("~", "") + id + extension;
                }
            }
            if(file==null && String.IsNullOrEmpty(contact.AvatarLink))
            {
                // Không có Avatar thì tự vẽ mới:
                var imgDraw = new ImgDraw();
                var newImg = imgDraw.Draw(contact.FullName);
                using MemoryStream ms = new();
                newImg.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                HttpResponseMessage result = new(HttpStatusCode.OK);
                var newFile = new FormFile(ms, 0, ms.ToArray().Length, "avatars", "avatar.png");
                contact.AvatarLink = await _fileTransfer.UploadFile(newFile, "avatars", contact.ContactId.ToString());
            }
            // Cập nhật các thông tin khác:
            var rowAffects = await _repository.UpdateAsync(contact, id);
            if (rowAffects > 0)
                return Ok(contact);
            else
                return NoContent();
        }
        private async Task ProcessAvatar(IFormFile file, Contact contact)
        {
            if (file != null && file.Length > 0)
            {

                // Dung lượng File được phép tải (lấy từ app config):
                int maxSizeConfig = int.Parse("10000");
                int maxContentLength = 1024 * 1024 * maxSizeConfig; //Size = 1 MB  

                // Định dạng File được phép tải (lấy từ app Config): 
                //int fileType = int.Parse(CommonUtility.GetAppSettingByKey("AvatarImgExtensionsAllowed"));
                IList<string> allowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };

                // Tên File
                var ext = file.FileName.Substring(file.FileName.LastIndexOf('.'));

                // Phần mở rộng của File (Ex: .jpg, .png...)
                var extension = ext.ToLower();

                // Kiểm tra tên file và định dạng File có hợp lệ không:
                if (!allowedFileExtensions.Contains(extension))
                {
                    var message = string.Format("Vui lòng chỉ chọn File có định dạng .jpg,.gif,.png.");
                    throw new MISAException(HttpStatusCode.BadRequest,message);
                }
                // Kiểm tra dung lượng File:
                else if (file.Length > maxContentLength)
                {

                    var message = string.Format("Vui lòng chọn File có dung lượng tối đa 1 MB.");
                    //throw new System.InvalidOperationException("Vui lòng chọn File có dung lượng tối đa 1 MB");
                    throw new MISAException(HttpStatusCode.BadRequest, message);
                }
                else
                {

                    // Upload file sang server files -> bắt đầu từ 03/10/2022 thì file được upload sang server riêng:
                    // Thông tin server file xem trong appsetting.json:
                    contact.AvatarLink = await _fileTransfer.UploadFile(file, "avatars", contact.ContactId.ToString());

                    // Đoạn dưới này là upload file trực tiếp vào máy chủ chứa mã nguồn - bỏ đi từ 03/10/2022
                    //// Đường dẫn chứa file trên máy chủ:
                    //string userAvatarPath = @"Content/imgs/users/avatar/";
                    //var folderPath = Path.Combine(_env.WebRootPath, userAvatarPath);
                    //var filePath = Path.Combine(_env.WebRootPath, userAvatarPath + id + extension);

                    //// Kiểm tra xem thư mục có tồn tại trên máy chủ hay không, nếu không có thì thực hiện tạo mới:
                    //if (!Directory.Exists(folderPath))
                    //{
                    //    Directory.CreateDirectory(folderPath);
                    //}

                    //// Thực hiện lưu File trên máy chủ:
                    //using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    //{
                    //    await file.CopyToAsync(fileStream);
                    //}
                    ////file.SaveAs(filePath);

                    //// Sau khi lưu xong thực hiện cập nhật thông tin Avatar cho liên hệ:
                    //contact.AvatarLink = userAvatarPath.Replace("~", "") + id + extension;
                }
            }
            if (file == null && String.IsNullOrEmpty(contact.AvatarLink))
            {
                // Không có Avatar thì tự vẽ mới:
                var imgDraw = new ImgDraw();
                var newImg = imgDraw.Draw(contact.FullName);
                using MemoryStream ms = new();
                newImg.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                HttpResponseMessage result = new(HttpStatusCode.OK);
                var newFile = new FormFile(ms, 0, ms.ToArray().Length, "avatars", "avatar.png");
                contact.AvatarLink = await _fileTransfer.UploadFile(newFile, "avatars", contact.ContactId.ToString());
            }
        }
    }
}
