using System;
using System.Collections.Generic;

namespace Orbit.Domain.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string UserEmail { get; set; } = null!;

    public DateOnly UserDateOfBirth { get; set; }

    public string UserPassword { get; set; } = null!;
}
