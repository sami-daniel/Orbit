using AutoMapper;
using Orbit.Application.Dtos.Requests;
using Orbit.Application.Dtos.Responses;
using Orbit.Domain.Entities;

namespace Orbit.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            _ = CreateMap<UserAddRequest, User>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.UserProfileName, opt => opt.AllowNull())
                .ForMember(dest => dest.UserDescription, opt => opt.AllowNull())
                .ForMember(dest => dest.UserImageByteType, opt => opt.AllowNull())
                // Permitir que o AutoMapper mapeie valores nulos para esses campos
                .ForSourceMember(src => src.Day, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Month, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Year, opt => opt.DoNotValidate());

            //Ignore os membros Day, Month e Year. Serão resolvidos nos serviços

            _ = CreateMap<User, UserResponse>()
                .ForMember(dest => dest.Followers, opt => opt.MapFrom(src => src.Followers))
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users))
                .ForMember(dest => dest.UserProfileName, opt => opt.AllowNull())
                .ForMember(dest => dest.UserDescription, opt => opt.AllowNull())
                .ForMember(dest => dest.UserImageByteType, opt => opt.AllowNull());
        }
    }
}
