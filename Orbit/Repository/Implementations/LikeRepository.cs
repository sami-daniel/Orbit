using Orbit.Data.Contexts;
using Orbit.Models;
using Orbit.Repository.Core.Implementations;
using Orbit.Repository.Interfaces;

namespace Orbit.Repository.Implementations;

/// <summary>
/// Implementation of the Like repository.
/// </summary>
public class LikeRepository : Repository<Like>, ILikeRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LikeRepository"/> class with the provided database context.
    /// </summary>
    /// <param name="context">The <see cref="ApplicationDbContext"/> to be provided.</param>
    public LikeRepository(ApplicationDbContext context) : base(context)
    {
    }
}
