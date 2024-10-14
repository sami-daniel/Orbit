using AutoMapper;
using Orbit.Domain.Entities;
using Orbit.DTOs.Requests;
using Orbit.DTOs.Responses;

namespace Orbit.Profiles;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<PostAddRequest, Post>()
                .ForMember(p => p.PostId, opt => opt.Ignore())
                .ForMember(p => p.PostDate, opt => opt.Ignore())
                .ForMember(p => p.User, opt => opt.Ignore())
                .ForMember(p => p.UserId, opt => opt.Ignore())
                .ForMember(p => p.PostImageByteType, opt => opt.MapFrom(src => ConvertToByteArrayFromIFormFile(src.PostImageByteType)))
                .ForMember(p => p.PostVideoByteType, opt => opt.MapFrom(src => ConvertToByteArrayFromIFormFile(src.PostVideoByteType)));

        CreateMap<Post, PostResponse>();
    }

    private static byte[]? ConvertToByteArrayFromIFormFile(IFormFile? file)
    {
        if (file == null)
        {
            return null;
        }

        if (!file.ContentType.Contains("image") || !file.ContentType.Contains("video"))
        {
            throw new InvalidOperationException("The file is not a video or a image");
        }

        if (file == null) return null;

        if (file.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);

                return ms.ToArray();
            }
        }

        throw new InvalidOperationException();
    }
}
