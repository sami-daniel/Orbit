using AutoMapper;
using Orbit.DTOs.Requests;
using Orbit.DTOs.Responses;
using Orbit.Models;

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
            .ForMember(dest => dest.Followers, opt => opt.MapFrom(src => src.Followers.Select(f => new UserResponse
            {
                UserName = f.UserName,
                UserDescription = f.UserDescription,
                UserEmail = f.UserEmail,
                UserProfileBannerImageByteType = f.UserProfileBannerImageByteType,
                UserProfileImageByteType = f.UserProfileImageByteType,
                UserProfileName = f.UserProfileName,
                IsPrivateProfile = f.IsPrivateProfile == 1UL,
            })))
            .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users.Select(u => new UserResponse
            {
                UserName = u.UserName,
                UserDescription = u.UserDescription,
                UserEmail = u.UserEmail,
                UserProfileBannerImageByteType = u.UserProfileBannerImageByteType,
                UserProfileImageByteType = u.UserProfileImageByteType,
                UserProfileName = u.UserProfileName,
                IsPrivateProfile = u.IsPrivateProfile == 1UL,
            })));
    }
}
