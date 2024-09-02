using System;
using System.Collections.Generic;

namespace Orbit.Domain.Entities;

public partial class User
{
    public uint UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string UserEmail { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public string UserProfileName { get; set; } = null!;

    public string? UserDescription { get; set; }

    public byte[]? UserImageByteType { get; set; }

    public byte[]? UserBannerByteType { get; set; }

    public ulong IsPrivateProfile { get; set; }

    public virtual ICollection<User> Followers { get; set; } = new List<User>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
