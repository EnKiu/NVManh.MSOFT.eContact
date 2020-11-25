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
using System.Threading.Tasks;

namespace MS.eContact.Web.Controllers
{
    public class ContactsController : BaseController<Contact>
    {
        IRepository<Contact> _repository;
        IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public ContactsController(IRepository<Contact> repository, IConfiguration configuration, IWebHostEnvironment env) : base(repository)
        {
            _configuration = configuration;
            _env = env;
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
                    // Đường dẫn chứa file trên máy chủ:
                    string userAvatarPath = @"Content/imgs/users/avatar/";
                    var folderPath = Path.Combine(_env.WebRootPath, userAvatarPath);
                    var filePath = Path.Combine(_env.WebRootPath, userAvatarPath + id + extension);

                    // Kiểm tra xem thư mục có tồn tại trên máy chủ hay không, nếu không có thì thực hiện tạo mới:
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Thực hiện lưu File trên máy chủ:
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    //file.SaveAs(filePath);

                    // Sau khi lưu xong thực hiện cập nhật thông tin Avatar cho liên hệ:
                    contact.AvatarLink = userAvatarPath.Replace("~", "") + id + extension;
                }
            }
            // Cập nhật các thông tin khác:

            return Ok(contact);
        }
    }
}
