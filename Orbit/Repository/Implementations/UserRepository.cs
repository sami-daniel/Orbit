using Orbit.Data.Contexts;
using Orbit.Models;
using Orbit.Repository.Core.Implementations;
using Orbit.Repository.Interfaces;

namespace Orbit.Repository.Implementations;

/// <summary>
/// Implementation of the User repository.
/// </summary>
public class UserRepository : Repository<User>, IUserRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class with the provided database context.
    /// </summary>
    /// <param name="context">The <see cref="ApplicationDbContext"/> to be provided.</param>
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
}
