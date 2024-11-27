using Microsoft.EntityFrameworkCore.Storage;
using Orbit.Models;
using Orbit.Repository.Interfaces;

namespace Orbit.UnitOfWork.Interfaces;

/// <summary>
/// Define contratos para uma unidade de trabalho que gerencia repositórios e operações de banco de dados.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Obtém o repositório para a entidade <see cref="User"/>.
    /// </summary>
    public IUserRepository UserRepository { get; }

    /// <summary>
    /// Obtém o repositório para a entidade <see cref="Post"/>
    /// </summary>
    public IPostRepository PostRepository { get; }

    /// <summary>
    /// Obtém o repositório para a entidade <see cref="Like"/>
    /// </summary>
    public ILikeRepository LikeRepository { get; }

    /// <summary>
    /// Obtém o repositório para a entidade <see cref="UserPreference"/>
    /// </summary>
    public IUserPreferenceRepository UserPreferenceRepository { get; }

    /// <summary>
    /// Obtém o repositório para a entidade <see cref="PostPreference"/>
    /// </summary>
    public IPostPreferenceRepository PostPreferenceRepository { get; }
    
    /// <summary>
    /// Salva todas as alterações feitas no contexto atual de banco de dados.
    /// </summary>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona. O resultado é o número de alterações feitas no banco de dados.
    /// </returns>
    public Task<int> CompleteAsync();

    /// <summary>
    /// Inicia uma nova transação no contexto atual de banco de dados.
    /// </summary>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona. O resultado é a transação do contexto de banco de dados.
    /// </returns>
    public Task<IDbContextTransaction> StartTransactionAsync();
}
