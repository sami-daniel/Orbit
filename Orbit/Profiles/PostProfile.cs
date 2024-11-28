using AutoMapper;
using Orbit.DTOs.Requests;
using Orbit.DTOs.Responses;
using Orbit.Models;

namespace Orbit.Profiles;

/// <summary>
/// AutoMapper Profile for mapping between Post-related DTOs and the Post model.
/// </summary>
public class PostProfile : Profile
{
    public PostProfile()
    {
        // Mapping from PostAddRequest DTO to Post model, ignoring specific fields and converting image/video file to byte array.
        CreateMap<PostAddRequest, Post>()
            .ForMember(p => p.PostId, opt => opt.Ignore()) // Ignores PostId field as it might be auto-generated.
            .ForMember(p => p.PostDate, opt => opt.Ignore()) // Ignores PostDate field, it might be set automatically.
            .ForMember(p => p.User, opt => opt.Ignore()) // Ignores User field since it will be handled by the relationship.
            .ForMember(p => p.UserId, opt => opt.Ignore()) // Ignores UserId, it might be assigned later.
            .ForMember(p => p.PostImageByteType, opt => opt.MapFrom(src => ConvertToByteArrayFromIFormFile(src.PostImageByteType))) // Maps the PostImageByteType from IFormFile to byte[].
            .ForMember(p => p.PostVideoByteType, opt => opt.MapFrom(src => ConvertToByteArrayFromIFormFile(src.PostVideoByteType))); // Maps the PostVideoByteType from IFormFile to byte[].

        // Mapping from Post model to PostResponse DTO, straightforward mapping.
        CreateMap<Post, PostResponse>();
    }

    /// <summary>
    /// Converts an IFormFile (image/video) into a byte array.
    /// </summary>
    /// <param name="file">The IFormFile to be converted.</param>
    /// <returns>A byte array representation of the file, or null if no file is provided.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the file is not an image or a video, or if there's an issue reading the file.</exception>
    private static byte[]? ConvertToByteArrayFromIFormFile(IFormFile? file)
    {
        if (file == null)
        {
            return null; // Returns null if no file is provided.
        }

        // Validates that the file is an image or video.
        if (!file.ContentType.Contains("image") && !file.ContentType.Contains("video"))
        {
            throw new InvalidOperationException("The file is not a video or an image");
        }

        if (file.Length > 0)
        {
            using (var ms = new MemoryStream()) // Uses a MemoryStream to read the file contents.
            {
                file.CopyTo(ms); // Copies the file content to the memory stream.
                return ms.ToArray(); // Returns the byte array representation of the file.
            }
        }
        else
        {
            return null; // Returns null if the file length is 0.
        }

        throw new InvalidOperationException(); // Fallback exception if the file cannot be processed.
    }
}
