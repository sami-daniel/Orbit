using Microsoft.EntityFrameworkCore.Storage;
using Orbit.Data.Contexts;
using Orbit.Repository.Interfaces;
using Orbit.UnitOfWork.Interfaces;

namespace Orbit.UnitOfWork.Implementations;

/// <summary>
/// Implementação da unidade de trabalho.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private bool _disposedValue;

    /// <summary>
    /// Repositório de usuários.
    /// </summary>
    public IUserRepository UserRepository { get; }

    public IPostRepository PostRepository { get; }

    public ILikeRepository LikeRepository { get; }

    public IUserPreferenceRepository UserPreferenceRepository { get; }

    public IPostPreferenceRepository PostPreferenceRepository { get; }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="UnitOfWork"/>.
    /// </summary>
    /// <param name="userRepository">O repositório de usuários.</param>
    /// <param name="applicationDbContext">O contexto do banco de dados da aplicação.</param>
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
    /// Salva todas as alterações feitas no contexto.
    /// </summary>
    /// <returns>O número de estados de entidades gravados no banco de dados.</returns>
    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Libera os recursos não gerenciados e, opcionalmente, os recursos gerenciados.
    /// </summary>
    /// <param name="disposing">Se verdadeiro, libera os recursos gerenciados.</param>
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
    /// Libera todos os recursos usados pela instância atual da classe <see cref="UnitOfWork"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Inicia uma nova transação no contexto do banco de dados.
    /// </summary>
    /// <returns>Uma instância de <see cref="IDbContextTransaction"/> que representa a transação.</returns>
    public async Task<IDbContextTransaction> StartTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }
}
