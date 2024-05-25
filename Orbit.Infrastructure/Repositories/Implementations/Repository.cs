using Orbit.Infrastructure.Repositories.Interfaces;

namespace Orbit.Infrastructure.Repositories.Implementations
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public Task<bool> AddAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> Find(Predicate<TEntity> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}
