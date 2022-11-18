using MS.ApplicationCore.DTOs;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Interfaces
{
    public interface IUserRepository:IRepository<User>
    {
        Task<UserInfo> GetUserInfoResponseById(string id);
        Task<UserRegisterResponse> GetByPhoneNumber (string phoneNumber);
        /// <summary>
        /// Đăng ký User mới
        /// </summary>
        /// <param name="user">Thông tin user</param>
        /// <returns>int>0 là thành công</returns>
        /// CreatedBy: NVMANH (10.09.2022)
        Task<int> Register(User user);

        /// <summary>
        /// Lấy thông tin xác thực người dùng theo userName và password
        /// </summary>
        /// <param name="userName">Tên tài khoản</param>
        /// <param name="password">Mật khẩu</param>
        /// <returns>Thông tin User Name</returns>
        /// CreatedBy: NVMANH (10.09.2022)
        Task<UserInfo> GetUserAuthenticate(string userName, string password);

        /// <summary>
        /// Kiểm tra userName đã tồn tại hay chưa
        /// </summary>
        /// <param name="userName">Tên user name</param>
        /// <returns>true - đã tồn tại, false - chưa tồn tại</returns>
        /// CreatedBy: NVMANH (10/09/2022)
        Task<bool> CheckUserNameExist(string userName);

        Task<string> GetUserNameByContactId(Guid contactId);

        /// <summary>
        /// Kiểm tra số điện thoại đã tồn tại trong hệ thống hay chưa?
        /// </summary>
        /// <param name="phone">Số điện thoại cần kiểm tra</param>
        /// <returns>true - đã được đăng ký; false- chưa được đăng ký</returns>
        /// CreatedBy: NVMANH (10.09.2022)
        Task<bool> CheckPhoneNumberExist(string phone);


        /// <summary>
        /// Kiểm tra số điện thoại đúng với số điện thoại được khai báo trong hệ thống hay không?
        /// </summary>
        /// <param name="phone">Số điện thoại cần kiểm tra</param>
        /// <returns>true - đã được đăng ký; false- chưa được đăng ký</returns>
        /// CreatedBy: NVMANH (10.09.2022)
        Task<string?> GetMobileNumberByContactId(Guid contactId);
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
        Task<IList<string>> GetRolesAsync(User user);

        /// <summary>
        /// Tìm kiếm User theo user Name
        /// </summary>
        /// <param name="userName">Tên tài khoản</param>
        /// <returns>Thông tin User</returns>
        /// CreatedBy: NVMANH (10.09.2022)
        Task<User> FindByNameAsync(string userName);

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
        Task<int> ConfirmMultiUser(IEnumerable<User> users);

        /// <summary>
        /// Lấy toàn bộ các Roles
        /// </summary>
        /// <returns>Danh sách Roles trong hệ thống</returns>
        Task<IEnumerable<Role>> GetRoles();

        /// <summary>
        /// Lấy thông tin lớp (sĩ số; tiền quỹ)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ClassInfo> GetClassInfoById(string? id = null);
    }
}
