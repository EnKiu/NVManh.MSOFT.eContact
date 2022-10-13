﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MS.ApplicationCore.Authorization;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Entities.Auth;
using MS.ApplicationCore.Interface.Service;
using MS.ApplicationCore.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MS.eContact.Web.Controllers
{
    public class AccountsController : BaseController<AspNetUsers>
    {
        readonly IUserRepository _repository;
        readonly IUserService _service;
        readonly IJwtUtils _jwtUntils;
        private readonly IConfiguration _configuration;
        public AccountsController(IUserRepository repository, IUserService baseService, IJwtUtils jwtUntils, IConfiguration configuration) : base(repository, baseService)
        {
            _repository = repository;
            _service = baseService;
            _jwtUntils = jwtUntils;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _repository.GetUserAuthenticate(model.Username, _jwtUntils.HashPassword(model.Password));
            if (user != null)
            {

                var token = _jwtUntils.GenerateJwtToken(user);
                var refreshToken = JwtUtils.GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
                _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                await _repository.UpdateAsync(user);

                var authenticateResponse = new AuthenticateResponse(user, token);
                authenticateResponse.Expiration = DateTime.Now.AddDays(tokenValidityInMinutes);
                // Lấy ra quyền cao nhất của User nếu có
                var role = user.Roles.FirstOrDefault();
                if (role != null)
                {
                    authenticateResponse.RoleValue = role.RoleValue;
                    authenticateResponse.RoleName = role.Name;
                }


                return Ok(authenticateResponse);
            }
            else
            {
                var res = new
                {
                    userMsg = "Thông tin người dùng không hợp lệ",
                };
                return Unauthorized(res);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] AspNetUsers user)
        {
            var result = await _service.Register(user);

            if (result == 0)
                return StatusCode(StatusCodes.Status500InternalServerError, new  { Status = "Error", Message = "Không thể xử lý quá trình đăng ký, vui lòng liên hệ quản trị viên để được trợ giúp." });

            return Ok(new { Status = "Success", Message = "Tạo tài khoản thành công!" });
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _repository.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new  { Status = "Error", Message = "User already exists!" });


            AspNetUsers user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            //var result = await _userManager.CreateAsync(user, model.Password);
            var result = await _repository.AddAsync(user);
            if (result == 0)
                return StatusCode(StatusCodes.Status500InternalServerError, new  { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new  { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            var principal = _jwtUntils.GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var username = principal.Identity.Name;

            var user = await _repository.FindByNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var newAccessToken = _jwtUntils.CreateToken(principal.Claims.ToList());
            var newRefreshToken = JwtUtils.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _repository.UpdateAsync(user);

            return new ObjectResult(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _repository.FindByNameAsync(username);
            if (user == null) return BadRequest("Invalid user name");

            user.RefreshToken = null;
            await _repository.UpdateAsync(user);

            return NoContent();
        }

        [HttpPost]
        [Route("revoke-all")]
        public async Task<IActionResult> RevokeAll()
        {
            //var users = _userManager.Users.ToList();
            //foreach (var user in users)
            //{
            //    user.RefreshToken = null;
            //    await _userManager.UpdateAsync(user);
            //}

            return NoContent();
        }

    }
}
