using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Orbit.Infrastructure.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Orbit.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;

        public Repository(DbContext context, IConfiguration configuration)
        {
            if (configuration["AllowedEntityNamespaces"]!.ToString().Split(',').Contains(typeof(TEntity).Namespace))
            {
                throw new ArgumentException($"O namespace ${typeof(TEntity).Namespace} não está habilitado!");
            }
            Context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(nameof(entity));
            await Context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            ArgumentNullException.ThrowIfNull(nameof(entities));
            await Context.Set<TEntity>().AddRangeAsync(entities);
        }

        public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);
            return await Context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity?> GetAsync(int id)
        {
            ArgumentNullException.ThrowIfNull(id);
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public async Task RemoveAsync(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            await Task.Run(() =>
            {
                Context.Set<TEntity>().Remove(entity);
            });
        }

        public async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);
            await Task.Run(() =>
            {
                Context.Set<TEntity>().RemoveRange(entities);
            });
        }

    }
}
