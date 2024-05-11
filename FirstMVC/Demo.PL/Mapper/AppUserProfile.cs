using AutoMapper;
using Demo.DAL.Entities;
using Demo.PL.Models;
using Microsoft.AspNetCore.Identity;

namespace Demo.PL.Mapper
{
    public class AppUserProfile : Profile
    {
        public AppUserProfile()
        {
            CreateMap<AppUser, AppUserVM>();
        }
    }
}
