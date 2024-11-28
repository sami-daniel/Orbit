using Microsoft.EntityFrameworkCore.Storage;
using Orbit.Models;
using Orbit.Repository.Interfaces;

namespace Orbit.UnitOfWork.Interfaces;

/// <summary>
/// Defines contracts for a unit of work that manages repositories and database operations.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets the repository for the <see cref="User"/> entity.
    /// </summary>
    public IUserRepository UserRepository { get; }

    /// <summary>
    /// Gets the repository for the <see cref="Post"/> entity.
    /// </summary>
    public IPostRepository PostRepository { get; }

    /// <summary>
    /// Gets the repository for the <see cref="Like"/> entity.
    /// </summary>
    public ILikeRepository LikeRepository { get; }

    /// <summary>
    /// Gets the repository for the <see cref="UserPreference"/> entity.
    /// </summary>
    public IUserPreferenceRepository UserPreferenceRepository { get; }

    /// <summary>
    /// Gets the repository for the <see cref="PostPreference"/> entity.
    /// </summary>
    public IPostPreferenceRepository PostPreferenceRepository { get; }
    
    /// <summary>
    /// Saves all changes made in the current database context.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The result is the number of changes made in the database.
    /// </returns>
    public Task<int> CompleteAsync();

    /// <summary>
    /// Starts a new transaction in the current database context.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The result is the database context transaction.
    /// </returns>
    public Task<IDbContextTransaction> StartTransactionAsync();
}
