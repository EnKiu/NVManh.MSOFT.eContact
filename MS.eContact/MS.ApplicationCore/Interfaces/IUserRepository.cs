using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Interfaces
{
    public interface IUserRepository:IRepository<AspNetUsers>
    {
        /// <summary>
        /// Đăng ký User mới
        /// </summary>
        /// <param name="user">Thông tin user</param>
        /// <returns>int>0 là thành công</returns>
        /// CreatedBy: NVMANH (10.09.2022)
        Task<int> Register(AspNetUsers user);

        /// <summary>
        /// Lấy thông tin xác thực người dùng theo userName và password
        /// </summary>
        /// <param name="userName">Tên tài khoản</param>
        /// <param name="password">Mật khẩu</param>
        /// <returns>Thông tin User Name</returns>
        /// CreatedBy: NVMANH (10.09.2022)
        Task<AspNetUsers> GetUserAuthenticate(string userName, string password);

        /// <summary>
        /// Kiểm tra userName đã tồn tại hay chưa
        /// </summary>
        /// <param name="userName">Tên user name</param>
        /// <returns>true - đã tồn tại, false - chưa tồn tại</returns>
        /// CreatedBy: NVMANH (10/09/2022)
        Task<bool> CheckUserNameExist(string userName);

        /// <summary>
        /// Kiểm tra số điện thoại đã đăng ký hay chưa?
        /// </summary>
        /// <param name="phone">Số điện thoại cần kiểm tra</param>
        /// <returns>true - đã được đăng ký; false- chưa được đăng ký</returns>
        /// CreatedBy: NVMANH (10.09.2022)
        Task<bool> CheckPhoneNumberExist(string phone);

        /// <summary>
        ///  Kiểm tra Email đã được đăng ký hay chưa
        /// </summary>
        /// <param name="email">true- đã được đăng ký; false- chưa được đăng ký</param>
        /// <returns></returns>
        Task<bool> CheckEmailExist(string email);

        /// <summary>
        /// Lấy quyền hạn được cấp theo user
        /// </summary>
        /// <param name="user">thông tin user</param>
        /// <returns>Danh sách các quyền hạn được cấp</returns>
        /// CreatedBy: NVMANH (10.09.2022)
        Task<IList<string>> GetRolesAsync(AspNetUsers user);

        /// <summary>
        /// Tìm kiếm User theo user Name
        /// </summary>
        /// <param name="userName">Tên tài khoản</param>
        /// <returns>Thông tin User</returns>
        /// CreatedBy: NVMANH (10.09.2022)
        Task<AspNetUsers> FindByNameAsync(string userName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userTokenConfirm"></param>
        /// <returns></returns>
        Task<int> AddToken(UserTokenConfirm userTokenConfirm);

        /// <summary>
        /// Xác nhận User theo Token được gửi về Email
        /// </summary>
        /// <param name="userTokenConfirm">Thông tin Token xác nhận</param>
        /// <returns>true- thành công; false- thất bại</returns>
        /// CreatedBy: NVMANH (10.09.2022)
        Task<bool> ConfirmUserToken(UserTokenConfirm userTokenConfirm);

        /// <summary>
        /// Thực hiện xác nhận thông tin User hàng loạt.
        /// </summary>
        /// <param name="users">Danh sách User thực hiện xác thực thông tin</param>
        /// <returns>Số lượng User đã xác thực</returns>
        /// CreatedBy: NVMANH (10/09/2022)
        Task<int> ConfirmMultiUser(IEnumerable<AspNetUsers> users);

        /// <summary>
        /// Lấy toàn bộ các Roles
        /// </summary>
        /// <returns>Danh sách Roles trong hệ thống</returns>
        Task<IEnumerable<AspNetRoles>> GetRoles();
    }
}
