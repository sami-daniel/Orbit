﻿using System.Linq.Expressions;
using Orbit.Domain.Entities;

namespace Orbit.Application.Interfaces
{
    /// <summary>
    /// Interface que define contratos para os serviços de usuário.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Obtém um usuário pelo identificador.
        /// O identificador pode ser um email ou o nome do usuário.
        /// </summary>
        /// <param name="userIdentifier">O identificador do usuário, que pode ser um email ou o nome do usuário.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. A tarefa contém o usuário correspondente ao identificador.</returns>
        Task<User?> GetUserByIdentifierAsync(string userIdentifier);

        /// <summary>
        /// Obtém todos os usuários.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação assíncrona. A tarefa contém uma lista de todos os usuários.</returns>
        Task<IEnumerable<User>> GetAllUserAsync(Expression<Func<User, bool>>? filter = null, Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, string includeProperties = "");

        /// <summary>
        /// Adiciona um novo usuário ao sistema.
        /// </summary>
        /// <param name="user">O usuário a ser adicionado.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        Task AddUserAsync(User user);

        /// <summary>
        /// Atualiza um usuário no sistema.
        /// </summary>
        /// <param name="userIdentifier">O identificador do usuário, que pode ser um email ou o nome do usuário.</param>
        /// <param name="user">O <see cref="User"/> contendo os dados atualizados do perfil.</param>
        Task UpdateUserAsync(string userIdentifier, User user);
    }
}
