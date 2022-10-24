using Microsoft.AspNetCore.Hosting.Server;
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
using static System.Net.WebRequestMethods;

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
        public async Task<string> UploadFile(IFormFile file, string folderPath, string fileName)
        {
            try
            {
                var path = await DoUploadFile(file, folderPath, fileName);
                return path;

            }
            catch (WebException ex)
            {
                FtpWebResponse newResponse = ex.Response as FtpWebResponse;
                if (newResponse.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    var res = MakeFolderInFileServer(folderPath);
                    newResponse.Close();
                    if (res == true)
                        return await DoUploadFile(file, folderPath, fileName);
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
        /// <param name="folderPath">Thư mục chứa tệp trên Server File</param>
        /// <param name="fileName">Tên File</param>
        /// <returns>Đường dẫn đến File đã upload thành công</returns>
        /// CreatedBy: NVMANH (30/08/2022)
        private async Task<string> DoUploadFile(IFormFile file, string folderPath, string fileName)
        {
            var ext = file.FileName[file.FileName.LastIndexOf('.')..].ToLower();
            var fileNameWithExt = String.Format("{0}{1}", fileName, ext);
            var subPath = String.Format("/{0}/{1}", folderPath, fileNameWithExt);// Ex: /folderName/abc.png
            var fullPath = $"/{_serverFilePath}{subPath}";// Ex: /e-contact/folderName/abc.png
            // Thực hiện upload file sang server files:
            var uploadUrl = String.Format("{0}{1}", _ftpHost, fullPath);// Ex: https://192.168.1.1/e-contact/folderName/abc.png

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
                var deleteFileUrl = String.Format("{0}/{1}/{2}", _ftpHost, _serverFilePath, filePath);

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
            catch (WebException ex)
            {
                FtpWebResponse newResponse = ex.Response as FtpWebResponse;
                if (newResponse.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    Console.OutputEncoding = Encoding.UTF8;
                    Console.WriteLine($"{filePath} không tồn tại");
                    //throw new MISAException("Tệp hoặc thư mục không tồn tại trên server files");
                }
                else
                {
                    Console.OutputEncoding = Encoding.UTF8;
                    Console.WriteLine($"{filePath} không thể xóa");
                    //throw new MISAException("Không thể xóa các files.");
                }
                return await Task.FromResult(false);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(false);
            }

        }

        /// <summary>
        /// Xóa thư mục trên Server Files
        /// </summary>
        /// <param name="folderPath">Tên thư mục muốn tạo (VD: folderPath)</param>
        /// <returns>true- nếu thành công; false - nếu không thành công</returns>
        /// CreatedBy: NVMANH (30/08/2022)
        public bool RemoveFolderInFileServer(string folderPath)
        {

            var directoryPath = String.Format("{0}/{1}/{2}", _ftpHost, _serverFilePath, folderPath);
            var request = (FtpWebRequest)WebRequest.Create(directoryPath);

            // Khởi tạo request mới và tạo thư mục:  
            request.Credentials = new NetworkCredential(_ftpUserName, _ftpPassword);
            request.Proxy = null;
            request.KeepAlive = true;
            request.UseBinary = true;
            request.Method = WebRequestMethods.Ftp.RemoveDirectory;

            //// do không thể xóa thư mục nếu nó có không rỗng, nên cần kiểm tra trước;
            //request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            try
            {
                using var resp = (FtpWebResponse)request.GetResponse();
                //Stream responseStream = resp.GetResponseStream();
                //StreamReader reader = new StreamReader(responseStream);
                //string line = reader.ReadLine();

                //while (!string.IsNullOrEmpty(line))
                //{
                //    line = reader.ReadLine();
                //}
                //return true;
                if (resp.StatusCode == FtpStatusCode.FileActionOK)
                {
                    Console.OutputEncoding = Encoding.UTF8;
                    Console.WriteLine(resp.StatusCode);
                    return true;
                }
                else
                    return false;

            }
            catch (WebException ex)
            {
                FtpWebResponse newResponse = ex.Response as FtpWebResponse;
                if (newResponse.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    Console.OutputEncoding = Encoding.UTF8;
                    Console.WriteLine($"{folderPath} không tồn tại");
                    //throw new MISAException("Tệp hoặc thư mục không tồn tại trên server files");
                }
                else
                {
                    Console.OutputEncoding = Encoding.UTF8;
                    Console.WriteLine($"{folderPath} không thể xóa");
                    //throw new MISAException("Không thể xóa các files.");
                }
                return false;
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

            var directoryPath = String.Format("{0}/{1}/{2}", _ftpHost, _serverFilePath, folderName);
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
