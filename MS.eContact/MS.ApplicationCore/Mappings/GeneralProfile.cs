using AutoMapper;
using MS.ApplicationCore.Authorization;
using MS.ApplicationCore.DTOs;
using MS.ApplicationCore.Entities;
using MS.ApplicationCore.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.ApplicationCore.Mappings
{
    public class GeneralProfile: Profile
    {
        //IJwtUtils _jwtUtils;
        public GeneralProfile()
        {
            //_jwtUtils = jwtUtils;
            CreateMap<User, UserRegisterResponse>();
            CreateMap<ExpenditurePlan, ExpenditurePlanResponse>();
            CreateMap<UserInfo, User>();
            CreateMap<User, UserInfo>();
            CreateMap<RegisterModel, User>()
                .ForMember(user => user.SecurityStamp, act => act.MapFrom(src => Guid.NewGuid().ToString()));
                //.ForMember(user => user.PasswordHash, act => act.MapFrom(src => _jwtUtils.HashPassword(src.Password)));
            
        }
    }
}
