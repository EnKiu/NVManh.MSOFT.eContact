﻿using MS.ApplicationCore.Entities.Auth;
using MS.ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.ApplicationCore.Entities
{
    public class AuthenticateResponse
    {
        public AuthenticateResponse(AspNetUsers user, string token)
        {
            UserId = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.UserName;
            Token = token;
        }
        public Guid UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? Expiration { get; set; }
        public Role? RoleValue { get; set; }
        public string? RoleName { get; set; }
    }
}
