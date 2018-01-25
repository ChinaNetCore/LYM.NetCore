using AutoMapper;
using LYM.Core.Model.User;
using LYM.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace LYM.Core
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(configuration =>
            {
                configuration.CreateMap<UserLogin, UserLoginModel>();

            });
        }
    }
}
