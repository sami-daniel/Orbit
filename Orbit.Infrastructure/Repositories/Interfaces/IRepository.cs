namespace Orbit.Infrastructure.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> Find(Predicate<TEntity> predicate);

        Task<bool> AddAsync(TEntity entity);
        Task<bool> AddRangeAsync(IEnumerable<TEntity> entities);

        Task<bool> RemoveAsync(TEntity entity);
        Task<bool> RemoveRangeAsync(IEnumerable<TEntity> entities);
    }
}
