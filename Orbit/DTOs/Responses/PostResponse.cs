using Orbit.Models;

namespace Orbit.DTOs.Responses;

public class PostResponse
{
    /// <summary>
    /// Gets or sets the content of the post.
    /// </summary>
    public string PostContent { get; set; } = null!;

    /// <summary>
    /// Gets or sets the date when the post was created.
    /// </summary>
    public DateTime PostDate { get; set; }

    /// <summary>
    /// Gets or sets the byte array representing the post's image.
    /// </summary>
    public byte[]? PostImageByteType { get; set; }

    /// <summary>
    /// Gets or sets the byte array representing the post's video.
    /// </summary>
    public byte[]? PostVideoByteType { get; set; }

    /// <summary>
    /// Gets or sets the number of likes the post has received.
    /// </summary>
    public uint PostLikes { get; set; }

    /// <summary>
    /// Gets or sets the user who created the post.
    /// </summary>
    public User User { get; set; } = null!;
}
