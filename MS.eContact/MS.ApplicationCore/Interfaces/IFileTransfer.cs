using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Interfaces
{
    public interface IFileTransfer
    {
        /// <summary>
        /// Upload file sang Service file qua giao thức FTP
        /// (Lưu ý: tệp sẽ bị ghi đè nếu trùng tên)
        /// </summary>
        /// <param name="file">Tệp muốn upload</param>
        /// <param name="folderName">Tên thư mục chứa tệp sẽ upload</param>
        /// <param name="fileName">Tên tệp lưu trữ</param>
        /// <returns>Chuỗi đường dẫn đến tệp đã được upload thành công đến server file (VD: /avatar/nvmanh.jpg)</returns>
        /// CreatedBy: NVMANH (30/08/2022)
        Task<string> UploadFile(IFormFile file, string folderName, string fileName);

        /// <summary>
        /// Thực hiện xóa file theo tên file
        /// </summary>
        /// <param name="folderName">Tên thư mục sẽ xóa</param>
        /// <param name="fileName">Tên file sẽ xóa</param>
        /// <returns></returns>
        Task<bool> DeleteFile(string filePath);

        bool MakeFolderInFileServer(string folderName);
        bool RemoveFolderInFileServer(string folderPath);
    }
}
