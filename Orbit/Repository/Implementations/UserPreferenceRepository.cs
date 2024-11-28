namespace Orbit.Repository.Implementations;

using Orbit.Data.Contexts;
using Orbit.Models;
using Orbit.Repository.Core.Implementations;
using Orbit.Repository.Interfaces;

/// <summary>
/// Implementation of the UserPreference repository.
/// </summary>
public class UserPreferenceRepository : Repository<UserPreference>, IUserPreferenceRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserPreferenceRepository"/> class with the provided database context.
    /// </summary>
    /// <param name="context">The <see cref="ApplicationDbContext"/> to be provided.</param>
    public UserPreferenceRepository(ApplicationDbContext context) : base(context)
    {
    }
}
