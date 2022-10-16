using AutoMapper;
using MS.ApplicationCore.Authorization;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Entities.Auth;
using MS.ApplicationCore.Exceptions;
using MS.ApplicationCore.Interface.Service;
using MS.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        IUserRepository _repository;
        IJwtUtils _jwtUtils;

        public UserService(IUserRepository repository, IJwtUtils jwtUtils, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, unitOfWork, mapper)
        {
            _repository = repository;
            _jwtUtils = jwtUtils;
        }
        public async Task<int> Register(RegisterModel registerModel)
        {

            // Update lại thông tin Email và số điện thoại
            //user.Email = user.Employee.Email;
            //user.PhoneNumber = user.Employee.Mobile;
            // Kiểm tra thông tin trước khi thực hiện thêm mới user:
            await ValidateUser(registerModel);

            if (Errors.Count > 0)
            {

                throw new MISAException(System.Net.HttpStatusCode.BadRequest, Errors);
            }
            var newUser = Mapper.Map<RegisterModel, User>(registerModel);
            newUser.PasswordHash = _jwtUtils.HashPassword(registerModel.Password);
            var res = await UnitOfWork.Users.Register(newUser);
            return res;
        }

        public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model)
        {
            var user = await UnitOfWork.Users.GetUserAuthenticate(model.Username, _jwtUtils.HashPassword(model.Password));
            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var token = _jwtUtils.GenerateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll()
        {
            return null;
        }

        public async Task<User> GetById(Guid id)
        {
            return await UnitOfWork.Users.FindAsync(id);
        }

        /// <summary>
        /// Kiểm tra các thông tin của người dùng đã hợp lệ hay chưa?
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        private async Task<bool> ValidateUser(RegisterModel user)
        {

            //if (await _repository.CheckEmailExist(user.Email) == true)
            //    Errors.Add("Email đã được đăng ký.");
            if (await UnitOfWork.Users.CheckPhoneNumberExist(user.PhoneNumber) == true)
                Errors.Add("PhoneNumber", new List<string>() { "Số điện thoại đã được đăng ký." });
            if (user.Password.Trim() != user.RePassword.Trim())
                Errors.Add("RePassword", new List<string>() { "Mật khẩu không khớp." });
            if (await UnitOfWork.Users.CheckUserNameExist(user.UserName) == true)
                Errors.Add("UserName", new List<string>() { "Tên tài khoản đã được đăng ký." });
            if (Errors.Count > 0)
            {
                return false;
            }
            return true;
        }
        public object? LogOut()
        {
            throw new NotImplementedException();
        }

        public async Task<int> ConfirmMultiUser(IEnumerable<User> users)
        {
            return await UnitOfWork.Users.ConfirmMultiUser(users);
        }
    }
}
