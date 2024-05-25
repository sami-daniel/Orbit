using Microsoft.Extensions.Configuration;
using Orbit.Domain.Entities;
using Orbit.Infrastructure.Data.Contexts;
using Orbit.Infrastructure.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Orbit.Infrastructure.Repositories.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context, IConfiguration configuration) : base(context, configuration)
        {
        }
    }
}
