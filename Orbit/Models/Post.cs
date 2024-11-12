namespace Orbit.Models;

public partial class Post
{
    public uint PostId { get; set; }

    public uint UserId { get; set; }

    public string PostContent { get; set; } = null!;

    public DateTime PostDate { get; set; }

    public byte[]? PostImageByteType { get; set; }

    public byte[]? PostVideoByteType { get; set; }

    public uint PostLikes { get; set; }

    public virtual User User { get; set; } = null!;
}
