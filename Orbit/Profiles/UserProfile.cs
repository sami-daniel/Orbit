using AutoMapper;
using Orbit.Domain.Entities;
using Orbit.DTOs.Requests;
using Orbit.DTOs.Responses;

namespace Orbit.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserAddRequest, User>()
                 .ForMember(dest => dest.UserId, opt => opt.Ignore())
                 .ForMember(dest => dest.UserDescription, opt => opt.Ignore())
                 .ForMember(dest => dest.UserProfileImageByteType, opt => opt.Ignore())
                 .ForMember(dest => dest.UserProfileBannerImageByteType, opt => opt.Ignore())
                 .ForMember(dest => dest.Followers, opt => opt.Ignore())
                 .ForMember(dest => dest.Users, opt => opt.Ignore())
                 .ForMember(dest => dest.IsPrivateProfile, opt => opt.MapFrom(src => src.IsPrivateProfile == "on" ? 1UL : 0UL));

        CreateMap<User, UserResponse>()
                .ForMember(dest => dest.IsPrivateProfile, opt => opt.MapFrom(src => src.IsPrivateProfile == 1UL))
                .PreserveReferences()
                .ForMember(dest => dest.Followers, opt => opt.MapFrom(src => src.Followers.Select(f => f.Followers)))
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users.Select(u => u.Users)));
    }
}
