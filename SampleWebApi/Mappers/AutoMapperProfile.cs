using AutoMapper;
using SampleWebApi.Dtos;
using SampleWebApi.Models;

namespace SampleWebApi.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // source destination
            CreateMap<UserLoginDto, User>();
            CreateMap<UserRegisterDto, User>();
        }
    }
}
