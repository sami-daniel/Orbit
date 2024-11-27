namespace Orbit.Models;

public partial class Post
{
    public uint PostId { get; set; }

    public string PostContent { get; set; } = null!;

    public DateTime PostDate { get; set; }

    public byte[]? PostImageByteType { get; set; }

    public byte[]? PostVideoByteType { get; set; }

    public uint PostLikes { get; set; }

    public virtual User User { get; set; } = null!;

    public uint UserId { get; set; }

    public virtual ICollection<PostPreference> PostPreferences { get; set; } = new List<PostPreference>();
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Post post = (Post)obj;

        return PostId == post.PostId
               && UserId == post.UserId
               && PostContent == post.PostContent
               && PostDate == post.PostDate
               && PostImageByteType == post.PostImageByteType
               && PostVideoByteType == post.PostVideoByteType
               && PostLikes == post.PostLikes;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(PostId,
                                UserId,
                                PostContent,
                                PostDate,
                                PostImageByteType,
                                PostVideoByteType,
                                PostLikes);
    }
}
