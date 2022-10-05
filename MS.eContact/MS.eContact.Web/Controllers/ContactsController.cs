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

namespace MS.eContact.Web.Controllers
{
    public class ContactsController : BaseController<Contact>
    {
        IRepository<Contact> _repository;
        IBaseService<Contact> _baseService;
        IConfiguration _configuration;
        readonly IFileTransfer _fileTransfer;
        private readonly IWebHostEnvironment _env;
        public ContactsController(IRepository<Contact> repository, IConfiguration configuration, IWebHostEnvironment env, IFileTransfer fileTransfer, IBaseService<Contact> baseService) : base(repository, baseService)
        {
            _configuration = configuration;
            _env = env;
            _repository = repository;
            _fileTransfer = fileTransfer;
            _baseService = baseService;
        }

        public override async Task<IActionResult> Put(string id)
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
                    contact.AvatarLink = await _fileTransfer.UploadFile(file, "avatars", id);

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
                var newImg = imgDraw.Draw(String.Format("{0} {1}",contact.FirstName,contact.LastName));
                using MemoryStream ms = new();
                newImg.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                HttpResponseMessage result = new(HttpStatusCode.OK);
                var newFile = new FormFile(ms, 0, ms.ToArray().Length, "avatars", "avatar.png");
                contact.AvatarLink = await _fileTransfer.UploadFile(newFile, "avatars", contact.ContactId.ToString());
            }
            // Cập nhật các thông tin khác:
            var rowAffects = _repository.Update(contact, id);
            if (rowAffects > 0)
                return Ok(contact);
            else
                return NoContent();
        }
    }
}
