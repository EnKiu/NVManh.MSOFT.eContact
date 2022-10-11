using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MS.ApplicationCore.Exceptions;
using MS.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MS.Infrastructure
{
    public class FileTransfer : IFileTransfer
    {
        readonly IConfiguration _config;
        readonly string _ftpHost;
        readonly string _ftpUserName;
        readonly string _ftpPassword;
        readonly string _serverFilePath;

        public FileTransfer(IConfiguration config)
        {
            _config = config;
            _ftpHost = _config["FTPAccount:Host"];
            _ftpUserName = _config["FTPAccount:UserName"];
            _ftpPassword = _config["FTPAccount:Password"];
            _serverFilePath = _config["ServerFilePath"];
        }

        /// <summary>
        /// Thực hiện upload file sang server file qua giao thức ftp
        /// </summary>
        /// <param name="file">file</param>
        /// <param name="fileName">Tên file</param>
        /// <returns>Đường dẫn đến tệp vừa upload</returns>
        /// CreatedBy: NVMANH (30/08/2022)
        public async Task<string> UploadFile(IFormFile file, string folderName, string fileName)
        {
            try
            {
                var path = await DoUploadFile(file, folderName, fileName);
                return path;

            }
            catch (WebException ex)
            {
                FtpWebResponse newResponse = ex.Response as FtpWebResponse;
                if (newResponse.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    var res = MakeFolderInFileServer(folderName);
                    newResponse.Close();
                    if (res == true)
                        return await DoUploadFile(file, folderName, fileName);
                    else
                        throw new MISAException("Không thể upload file tới Server Files");

                }
                else
                {
                    newResponse.Close();
                    throw new MISAException("Không thể upload file tới Server Files");
                }
            }
        }

        /// <summary>
        /// Thực hiện Upload File đến Server Files
        /// </summary>
        /// <param name="file">Tệp muốn upload</param>
        /// <param name="folderName">Tên thư mục chứa tệp trên Server File</param>
        /// <param name="fileName">Tên File</param>
        /// <returns>Đường dẫn đến File đã upload thành công</returns>
        /// CreatedBy: NVMANH (30/08/2022)
        private async Task<string> DoUploadFile(IFormFile file, string folderName, string fileName)
        {
            var ext = file.FileName[file.FileName.LastIndexOf('.')..].ToLower();
            var fileNameWithExt = String.Format("{0}{1}", fileName, ext);
            var subPath = String.Format("/{0}/{1}", folderName, fileNameWithExt);
            var fullPath = $"/{_serverFilePath}/{subPath}";
            // Thực hiện upload file sang server files:
            var uploadUrl = String.Format("{0}{1}", _ftpHost, fullPath);

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uploadUrl);

            // This example assumes the FTP site uses anonymous logon.  
            request.Credentials = new NetworkCredential(_ftpUserName, _ftpPassword);
            request.Proxy = null;
            request.KeepAlive = true;
            request.UseBinary = true;
            request.Method = WebRequestMethods.Ftp.UploadFile;

            using (Stream ftpStream = request.GetRequestStream())
            {
                file.CopyTo(ftpStream);
            }
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);
            return await Task.FromResult(subPath);
        }

        /// <summary>
        /// Thực hiện xóa file theo tên file
        /// </summary>
        /// <param name="folderName">Tên thư mục sẽ xóa</param>
        /// <param name="fileName">Tên file sẽ xóa</param>
        /// <returns></returns>
        public async Task<bool> DeleteFile(string filePath)
        {
            try
            {
                // Thực hiện upload file sang server files:
                var deleteFileUrl = String.Format("{0}{1}", _ftpHost, filePath);

                FtpWebRequest request = WebRequest.Create(deleteFileUrl) as FtpWebRequest;

                // This example assumes the FTP site uses anonymous logon.  
                request.Credentials = new NetworkCredential(_ftpUserName, _ftpPassword);
                request.Proxy = null;
                request.KeepAlive = true;
                request.UseBinary = true;
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                if (response.StatusCode == FtpStatusCode.FileActionOK)
                {
                    return await Task.FromResult(true);
                }
                else
                {
                    return await Task.FromResult(false);
                }
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }

        }

        /// <summary>
        /// Tạo thư mục mới trên Server Files
        /// </summary>
        /// <param name="folderName">Tên thư mục muốn tạo</param>
        /// <returns>true- nếu thành công; false - nếu không thành công</returns>
        /// CreatedBy: NVMANH (30/08/2022)
        public bool MakeFolderInFileServer(string folderName)
        {
            var directoryPath = String.Format("{0}/{1}", _ftpHost, folderName);
            var request = (FtpWebRequest)WebRequest.Create(directoryPath);

            // Khởi tạo request mới và tạo thư mục:  
            request.Credentials = new NetworkCredential(_ftpUserName, _ftpPassword);
            request.Proxy = null;
            request.KeepAlive = true;
            request.UseBinary = true;
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            using var resp = (FtpWebResponse)request.GetResponse();
            if (resp.StatusCode == FtpStatusCode.PathnameCreated)
            {
                Console.WriteLine(resp.StatusCode);
                return true;
            }
            else
                return false;
        }
    }
}
