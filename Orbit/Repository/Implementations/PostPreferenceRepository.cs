using Orbit.Data.Contexts;
using Orbit.Models;
using Orbit.Repository.Core.Implementations;
using Orbit.Repository.Interfaces;

namespace Orbit.Repository.Implementations;

public class PostPreferenceRepository : Repository<PostPreference>, IPostPreferenceRepository
{
    public PostPreferenceRepository(ApplicationDbContext context) : base(context)
    {
    }
}