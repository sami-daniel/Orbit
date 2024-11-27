namespace Orbit.Repository.Implementations;

using Orbit.Data.Contexts;
using Orbit.Models;
using Orbit.Repository.Core.Implementations;
using Orbit.Repository.Interfaces;

public class UserPreferenceRepository : Repository<UserPreference>, IUserPreferenceRepository
{
    public UserPreferenceRepository(ApplicationDbContext context) : base(context)
    {
    }
}