using AutoMapper;
using MS.ApplicationCore.DTOs;
using MS.ApplicationCore.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.ApplicationCore.Mappings
{
    public class GeneralProfile: Profile
    {
        public GeneralProfile()
        {
            CreateMap<User, UserRegisterResponse>();
        }
    }
}
