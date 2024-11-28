using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Orbit.Repository.Core.Interfaces;

namespace Orbit.Repository.Core.Implementations;

/// <summary>
/// Implements a generic repository for an entity of type <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TEntity">The type of the entity managed by the repository. Must be a class.</typeparam>
public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly DbSet<TEntity> _entitySet;

    /// <summary>
    /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class with the provided database context.
    /// </summary>
    /// <param name="context">The database context used to access the data source.</param>
    protected Repository(DbContext context)
    {
        // Set the DbSet for the entity type TEntity
        _entitySet = context.Set<TEntity>();
    }

    /// <summary>
    /// Retrieves a collection of entities that match the provided filter criteria.
    /// </summary>
    /// <param name="filter">A predicate to filter the entities. Can be <c>null</c>.</param>
    /// <param name="orderBy">A function to order the entities. Can be <c>null</c>.</param>
    /// <param name="includeProperties">A comma-separated list of navigation properties to include in the query. Can be an empty string if no properties should be included.</param>
    /// <returns>A task that represents the asynchronous operation, with the result being a collection of entities that meet the filter and order criteria.</returns>
    public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "")
    {
        // Initialize the query with the DbSet of the entity
        IQueryable<TEntity> query = _entitySet;

        // Apply the filter if provided
        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Include the specified navigation properties
        foreach (string includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        // Apply ordering if provided, and return the results asynchronously
        return orderBy != null ? await orderBy(query).ToListAsync() : (IEnumerable<TEntity>)await query.ToListAsync();
    }

    /// <summary>
    /// Inserts a new entity into the repository.
    /// </summary>
    /// <param name="entity">The entity to be inserted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public virtual async Task InsertAsync(TEntity entity)
    {
        _ = await _entitySet.AddAsync(entity);
    }

    /// <summary>
    /// Updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to be updated.</param>
    public virtual void Update(TEntity entity)
    {
        _ = _entitySet.Update(entity);
    }

    /// <summary>
    /// Removes an existing entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to be removed.</param>
    public virtual void Delete(TEntity entity)
    {
        _ = _entitySet.Remove(entity);
    }

    /// <summary>
    /// Inserts a range of entities into the repository.
    /// </summary>
    /// <param name="entities">The collection of entities to be inserted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public virtual async Task InsertRangeAsync(IEnumerable<TEntity> entities)
    {
        await _entitySet.AddRangeAsync(entities);
    }

    /// <summary>
    /// Updates a range of entities in the repository.
    /// </summary>
    /// <param name="entities">The collection of entities to be updated.</param>
    public virtual void UpdateRange(IEnumerable<TEntity> entities)
    {
        _entitySet.UpdateRange(entities);
    }

    /// <summary>
    /// Removes a range of entities from the repository.
    /// </summary>
    /// <param name="entities">The collection of entities to be removed.</param>
    public virtual void DeleteRange(IEnumerable<TEntity> entities)
    {
        _entitySet.RemoveRange(entities);
    }
}
