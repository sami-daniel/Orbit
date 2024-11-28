using Microsoft.EntityFrameworkCore.Storage;
using Orbit.Data.Contexts;
using Orbit.Repository.Interfaces;
using Orbit.UnitOfWork.Interfaces;

namespace Orbit.UnitOfWork.Implementations;

/// <summary>
/// Implementation of the unit of work.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private bool _disposedValue;

    /// <summary>
    /// User repository.
    /// </summary>
    public IUserRepository UserRepository { get; }

    public IPostRepository PostRepository { get; }

    public ILikeRepository LikeRepository { get; }

    public IUserPreferenceRepository UserPreferenceRepository { get; }

    public IPostPreferenceRepository PostPreferenceRepository { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="applicationDbContext">The application's database context.</param>
    public UnitOfWork(IUserRepository userRepository, ILikeRepository likeRepository, IPostRepository postRepository, IPostPreferenceRepository postPreferenceRepository, IUserPreferenceRepository userPreferenceRepository, ApplicationDbContext applicationDbContext)
    {
        UserRepository = userRepository;
        PostRepository = postRepository;
        LikeRepository = likeRepository;
        PostPreferenceRepository = postPreferenceRepository;
        UserPreferenceRepository = userPreferenceRepository;
        _context = applicationDbContext;
    }

    /// <summary>
    /// Saves all changes made in the context.
    /// </summary>
    /// <returns>The number of entity states written to the database.</returns>
    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Releases unmanaged resources and, optionally, managed resources.
    /// </summary>
    /// <param name="disposing">If true, releases managed resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            _disposedValue = true;
        }
    }

    /// <summary>
    /// Releases all resources used by the current instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Starts a new transaction in the database context.
    /// </summary>
    /// <returns>An instance of <see cref="IDbContextTransaction"/> representing the transaction.</returns>
    public async Task<IDbContextTransaction> StartTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }
}
