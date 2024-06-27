using Orbit.Infrastructure.Data.Contexts;
using Orbit.Infrastructure.Repositories.Interfaces;

namespace Orbit.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IUserRepository User { get; private set; }
        public UnitOfWork(ApplicationDbContext context, IUserRepository userRepository)
        {
            _context = context;
            User = userRepository;
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
