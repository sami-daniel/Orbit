using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Orbit.Domain.Entities;
using Orbit.Infrastructure.Repositories.Interfaces;
using System.Linq;
using System.Linq.Expressions;

namespace Orbit.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;

        public Repository(DbContext context, IConfiguration configuration)
        {
            IConfigurationSection sectNamespaces = configuration.GetSection("AllowedEntityNamespaces");
            List<string> namespacesInConfig = [];

            foreach (IConfigurationSection child in sectNamespaces.GetChildren())
            {
                namespacesInConfig.Add(child.Path);
            }

            if (namespacesInConfig.Contains(typeof(TEntity).Namespace!))
            {
                throw new ArgumentException($"O namespace ${typeof(TEntity).Namespace} não está habilitado!");
            }
            Context = context;

            // O generico TEntity não tem nenhuma restrição quanto ao uso
            // de tipos, somente que seja uma classe. Dito isso, a unidade
            // de trabalho e o repositorio funcionarão somente com os modelos
            // gerados via scaffolding pelo efcore. Então como medida de segurança
            // somente entidade de um namespace previamente habilitado poderão ser
            // utilizadas na unidade
        }

        public async Task AddAsync(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(nameof(entity));
            _ = await Context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            ArgumentNullException.ThrowIfNull(nameof(entities));
            await Context.Set<TEntity>().AddRangeAsync(entities);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);
            return await Context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(params string[] navProperties)
        {
            var entities = Context.Set<TEntity>().AsQueryable();
            foreach (var prop in navProperties)
            {
                //Followers
                //Users
                entities = entities.Include(prop);
            }

            return await entities.ToListAsync();
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
                _ = Context.Set<TEntity>().Remove(entity);
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
