using Microsoft.Extensions.Configuration;
using Orbit.Infrastructure.Data.Contexts;
using Orbit.Infrastructure.Repositories.Implementations;
using Orbit.Infrastructure.Repositories.Interfaces;

namespace Orbit.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IUserRepository User { get; private set; }
        public UnitOfWork(ApplicationDbContext context, IConfiguration configuration) 
        {
            _context = context;
            User = new UserRepository(context, configuration);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
