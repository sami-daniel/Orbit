using Orbit.Domain.Entities;

namespace Orbit.DTOs.Responses;

public class PostResponse
{
    public string PostContent { get; set; } = null!;

    public DateTime PostDate { get; set; }

    public byte[]? PostImageByteType { get; set; }

    public byte[]? PostVideoByteType { get; set; }

    public uint PostLikes { get; set; }

    public User User { get; set; } = null!;
}
