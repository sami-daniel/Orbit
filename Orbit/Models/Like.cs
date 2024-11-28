namespace Orbit.Models;

public partial class Like
{
    public uint LikeId { get; set; }

    public uint? UserId { get; set; }

    public uint PostId { get; set; }

    public virtual Post Post { get; set; } = null!;

    public virtual User? User { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Like like = (Like)obj;

        return UserId == like.UserId
               && PostId == like.PostId;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(UserId,
                                PostId);
    }
}
