using AutoMapper;
using Orbit.DTOs.Requests;
using Orbit.DTOs.Responses;
using Orbit.Models;

namespace Orbit.Profiles;

/// <summary>
/// AutoMapper Profile for mapping between User-related DTOs and the User model.
/// </summary>
public class UserProfile : Profile
{
    public UserProfile()
    {
        // Mapping from UserAddRequest DTO to User model, while ignoring certain properties.
        CreateMap<UserAddRequest, User>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore()) // Ignores UserId field
            .ForMember(dest => dest.UserDescription, opt => opt.Ignore()) // Ignores UserDescription field
            .ForMember(dest => dest.UserProfileImageByteType, opt => opt.Ignore()) // Ignores UserProfileImageByteType field
            .ForMember(dest => dest.UserProfileBannerImageByteType, opt => opt.Ignore()) // Ignores UserProfileBannerImageByteType field
            .ForMember(dest => dest.Followers, opt => opt.Ignore()) // Ignores Followers field
            .ForMember(dest => dest.Users, opt => opt.Ignore()); // Ignores Users field

        // Mapping from User model to UserResponse DTO with preservation of references for Followers and Users.
        CreateMap<User, UserResponse>()
            .PreserveReferences() // Preserves reference identity during the mapping process.
            .ForMember(dest => dest.Followers, opt => opt.MapFrom(src => src.Followers.Select(f => new UserResponse
            {
                // Maps properties from the Followers collection to a UserResponse DTO.
                UserName = f.UserName,
                UserDescription = f.UserDescription,
                UserEmail = f.UserEmail,
                UserProfileBannerImageByteType = f.UserProfileBannerImageByteType,
                UserProfileImageByteType = f.UserProfileImageByteType,
                UserProfileName = f.UserProfileName,
            })))
            .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users.Select(u => new UserResponse
            {
                // Maps properties from the Users collection to a UserResponse DTO.
                UserName = u.UserName,
                UserDescription = u.UserDescription,
                UserEmail = u.UserEmail,
                UserProfileBannerImageByteType = u.UserProfileBannerImageByteType,
                UserProfileImageByteType = u.UserProfileImageByteType,
                UserProfileName = u.UserProfileName,
            })));
    }
}
