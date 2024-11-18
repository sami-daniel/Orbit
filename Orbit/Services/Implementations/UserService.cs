using System.Collections.Frozen;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Orbit.Models;
using Orbit.Services.Exceptions;
using Orbit.Services.Helpers;
using Orbit.Services.Interfaces;
using Orbit.UnitOfWork.Interfaces;

namespace Orbit.Services.Implementations;

/// <summary>
/// Serviço para gerenciar operações relacionadas ao usuário.
/// </summary>
public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="UserService"/>.
    /// </summary>
    /// <param name="unitOfWork">A unidade de trabalho para acesso ao repositório.</param>
    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Adiciona um novo usuário de forma assíncrona.
    /// </summary>
    /// <param name="user">O objeto User a ser adicionado.</param>
    /// <exception cref="UserAlredyExistsException">Lançada quando um usuário com o mesmo identificador já existe.</exception>
    public async Task AddUserAsync(User user)
    {

        try
        {
            UserServiceHelpers.ValidateUser(user);
        }
        catch (Exception)
        {
            throw;
        }

        user.UserEmail = user.UserEmail.ToLower();

        await _unitOfWork.UserRepository.InsertAsync(user);
        await _unitOfWork.CompleteAsync();

    }

    /// <summary>
    /// Obtém todos os usuários de forma assíncrona.
    /// </summary>
    /// <param name="filter">Expressão para filtrar os usuários.</param>
    /// <param name="orderBy">Função para ordenar os usuários.</param>
    /// <param name="includeProperties">Propriedades a serem incluídas na consulta.</param>
    /// <returns>Uma coleção de objetos User.</returns>
    public async Task<IEnumerable<User>> GetAllUserAsync(Expression<Func<User, bool>>? filter = null, Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, string includeProperties = "")
    {
        return await _unitOfWork.UserRepository.GetAsync(filter, orderBy, includeProperties);
    }

    /// <summary>
    /// Obtém um usuário pelo identificador de forma assíncrona.
    /// </summary>
    /// <param name="userIdentifier">O identificador do usuário (nome de usuário ou email).</param>
    /// <returns>O objeto User correspondente ou null se não encontrado.</returns>
    public async Task<User?> GetUserByIdentifierAsync(string userIdentifier)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userIdentifier, nameof(userIdentifier));
        IEnumerable<User>? users = null;

        if (userIdentifier.Contains('@'))
        {
            users = await _unitOfWork.UserRepository.GetAsync(u => u.UserEmail.ToLower() == userIdentifier);
        }
        else
        {
            users = await _unitOfWork.UserRepository.GetAsync(u => u.UserName == userIdentifier);
        }

        return users.FirstOrDefault();
    }

    /// <summary>
    /// Atualiza um usuário existente de forma assíncrona.
    /// </summary>
    /// <param name="userIdentifier">O identificador do usuário (nome de usuário ou email).</param>
    /// <param name="updatedUser">O objeto User com as informações atualizadas.</param>
    /// <exception cref="UserAlredyExistsException">Lançada quando um usuário com o mesmo identificador já existe.</exception>
    /// <exception cref="UserNotFoundException">Lançada quando o usuário com o identificador fornecido não é encontrado.</exception>
    public async Task UpdateUserAsync(string userIdentifier, User updatedUser)
    {
        IEnumerable<User>? users = null;

        try
        {
            UserServiceHelpers.ValidateUser(updatedUser);
        }
        catch (Exception)
        {
            throw;
        }

        if (userIdentifier.Contains('@'))
        {
            users = await _unitOfWork.UserRepository.GetAsync(u => u.UserEmail == userIdentifier);
        }

        users = await _unitOfWork.UserRepository.GetAsync(u => u.UserName == userIdentifier);

        var user = users.FirstOrDefault();

        if (user != null)
        {
            user.UserEmail = updatedUser.UserEmail;
            user.UserDescription = updatedUser.UserDescription;
            user.UserProfileImageByteType = updatedUser.UserProfileImageByteType;
            user.UserProfileName = updatedUser.UserProfileName;
            user.UserPassword = updatedUser.UserPassword;
            user.UserName = updatedUser.UserName;

            using (var transaction = await _unitOfWork.StartTransactionAsync())
            {
                try
                {
                    await _unitOfWork.CompleteAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();

                    throw new UserAlredyExistsException("O usuário com esse identificador já existe!");
                }
            }
        }
        else
        {
            throw new UserNotFoundException("O usuário com esse identificador não foi encontrado!");
        }
    }

    public async Task FollowUserAsync(string followerUsername, string userToBeFollowedUserName)
    {
        var follower = await _unitOfWork.UserRepository.GetAsync(u => u.UserName == followerUsername, includeProperties: "Users");
        var followedUser = await _unitOfWork.UserRepository.GetAsync(u => u.UserName == userToBeFollowedUserName, includeProperties: "Followers");

        if (!follower.Any())
        {
            throw new UserNotFoundException($"The user with username '{followerUsername} not exists on data source.");
        }

        if (!followedUser.Any())
        {
            throw new UserNotFoundException($"The user with username '{userToBeFollowedUserName} not exists on data source.");
        }

        using (var transaction = await _unitOfWork.StartTransactionAsync())
        {
            try
            {
                followedUser.First().Followers.Add(follower.First());
                follower.First().Users.Add(followedUser.First());
                await _unitOfWork.CompleteAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateException)
            {
                // Fail quietly
                await transaction.RollbackAsync();
            }
        }
    }

    public async Task UnfollowUserAsync(string followerUsername, string userToBeUnfollowedUserName)
    {
        var follower = await _unitOfWork.UserRepository.GetAsync(u => u.UserName == followerUsername, includeProperties: "Users");
        var followedUser = await _unitOfWork.UserRepository.GetAsync(u => u.UserName == userToBeUnfollowedUserName, includeProperties: "Followers");

        if (!follower.Any())
        {
            throw new UserNotFoundException($"The user with username '{followerUsername} not exists on data source.");
        }

        if (!followedUser.Any())
        {
            throw new UserNotFoundException($"The user with username '{userToBeUnfollowedUserName} not exists on data source.");
        }

        using (var transaction = await _unitOfWork.StartTransactionAsync())
        {
            try
            {
                followedUser.First().Followers.Remove(follower.First());
                follower.First().Users.Remove(followedUser.First());
                await _unitOfWork.CompleteAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateException)
            {
                // Fail quietly
                await transaction.RollbackAsync();
            }
        }
    }
}
