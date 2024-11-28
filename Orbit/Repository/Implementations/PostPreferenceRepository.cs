using Orbit.Data.Contexts;
using Orbit.Models;
using Orbit.Repository.Core.Implementations;
using Orbit.Repository.Interfaces;

namespace Orbit.Repository.Implementations;

/// <summary>
/// Implementation of the PostPreference repository.
/// </summary>
public class PostPreferenceRepository : Repository<PostPreference>, IPostPreferenceRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PostPreferenceRepository"/> class with the provided database context.
    /// </summary>
    /// <param name="context">The <see cref="ApplicationDbContext"/> to be provided.</param>
    public PostPreferenceRepository(ApplicationDbContext context) : base(context)
    {
    }
}
