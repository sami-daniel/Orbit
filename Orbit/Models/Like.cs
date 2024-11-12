namespace Orbit.Models;

public partial class Like
{
    public uint? UserId { get; set; }

    public uint PostId { get; set; }

    public virtual Post Post { get; set; } = null!;

    public virtual User? User { get; set; }
}
