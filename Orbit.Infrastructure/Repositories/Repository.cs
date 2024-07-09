using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Orbit.Infrastructure.Repositories.Interfaces;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq.Dynamic.Core;
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

        public async Task<IEnumerable<TEntity>> FindAsync(object conditions)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(conditions));
            var conditionString = new List<string>();
            var parameters = new List<object>();
            int index = 0;

            foreach (var property in conditions.GetType().GetProperties())
            {
                var value = property.GetValue(conditions);

                if (value != null)
                {
                    string condition = $"{property.Name} == @{index}";
                    conditionString.Add(condition);
                    parameters.Add(value);
                    index++;
                }
            }

            if (conditionString.Count == 0)
                return [];

            string dinamycWhere = string.Join(" AND ", conditionString);


            return await Context.Set<TEntity>()
                        .Where(dinamycWhere, parameters.ToArray())
                        .ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(object conditions, params string[] navProperties)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(conditions));
            var conditionString = new List<string>();
            var parameters = new List<object>();
            int index = 0;

            foreach (var property in conditions.GetType().GetProperties())
            {
                var value = property.GetValue(conditions);

                if (value != null)
                {
                    string condition = $"{property.Name} == @{index}";
                    conditionString.Add(condition);
                    parameters.Add(value);
                    index++;
                }
            }

            if (conditionString.Count == 0)
                return [];

            string dinamycWhere = string.Join(" AND ", conditionString);


            var query = Context.Set<TEntity>()
                        .Where(dinamycWhere, parameters.ToArray()).AsQueryable();


            foreach(string prop in navProperties)
            {
                query = query.Include(prop);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(params string[] navProperties)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>().AsQueryable();
            foreach (string prop in navProperties)
            {
                query = query.Include(prop);
            }

            return await query.ToListAsync();
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
