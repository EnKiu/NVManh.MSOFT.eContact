using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MS.ApplicationCore.DTOs;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Entities.Auth;
using MS.ApplicationCore.Exceptions;
using MS.ApplicationCore.Utilities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MS.ApplicationCore.Authorization
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(UserInfo user);
        public string? ValidateJwtToken(string token);
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
        public string CreateToken(List<Claim> authClaims);
        string HashPassword(string password);
    }

    /// <summary>
    /// Lớp này sử dụng để tạo và xác thực Token
    /// </summary>
    public class JwtUtils : IJwtUtils
    {
        private readonly IConfiguration _configuration;

        public JwtUtils(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Sinh mã Token mới
        /// </summary>
        /// <param name="user">Thông tin người dùng</param>
        /// <returns></returns>
        public string GenerateJwtToken(UserInfo user)
        {
            var userRoles = user.Roles;

            var authClaims = new List<Claim>
                {
                    new Claim("id", user.UserId.ToString()),
                    new Claim("UserName", user.UserName.ToString()),
                    new Claim("OrganizationId", user.OrganizationId.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                var userRoleString = userRole.RoleName;
                authClaims.Add(new Claim(ClaimTypes.Role, value: userRoleString));
            }
            var token = CreateToken(authClaims);
            return token;
        }

        /// <summary>
        /// Xác thực Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public string? ValidateJwtToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);

            /// Nếu có token hoặc token không hợp lệ sẽ throw ra Exception thông báo lỗi xác thực
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId =jwtToken.Claims.First(x => x.Type == "id").Value.ToString();
            var orgID = jwtToken.Claims.First(x => x.Type == "OrganizationId").Value.ToString();
            CommonFunction.SetCurrentOrganozationId(orgID);
            // return user id from JWT token if validation successful
            return userId;

        }

        /// <summary>
        /// Tạo mới Token (Khi Login và khi Refresh Token)
        /// </summary>
        /// <param name="authClaims"></param>
        /// <returns>JWT Token</returns>
        /// CreatedBy: NVMANH (16/08/2022)
        public string CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Tạo ngẫu nhiên 1 Refresh Token (Khi Login và khi Refresh Token)
        /// </summary>
        /// <returns>Chuỗi Refresh Tojken mới</returns>
        /// CreatedBy: NVMANH (16/08/2022)
        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }

        /// <summary>
        /// Hash password
        /// </summary>
        /// <param name="password">Mật khẩu người dùng</param>
        /// <returns></returns>
        /// CreatedBy : NVMANH (16/01/2020)
        public string HashPassword(string password)
        {
            // generate a 128-bit salt using a secure PRNG
            //byte[] salt = new byte[128 / 8];

            byte[] salt = Encoding.Unicode.GetBytes("Salt");

            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hash;
        }
    }
}
