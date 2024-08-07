﻿using System.Linq.Expressions;

namespace Orbit.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Interface genérica para um repositório de entidades.
    /// </summary>
    /// <typeparam name="TEntity">Tipo da entidade do repositório.</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Obtém uma entidade assincronamente com base no ID fornecido.
        /// </summary>
        /// <param name="id">ID da entidade a ser obtida.</param>
        /// <returns>Tarefa representando a operação assíncrona, resultando na entidade solicitada.</returns>
        Task<TEntity?> GetAsync(int id);

        /// <summary>
        /// Obtém todas as entidades presentes no repositório assincronamente.
        /// </summary>
        /// <returns>Tarefa representando a operação assíncrona, resultando em uma coleção de todas as entidades.</returns>
        [Obsolete("Esse metodo retorna todas as entidades do banco de dados sem tipo algum de filtragem, forçando excessivamente o banco de dados e a pressão do ORM escolhido. Utilize FindAsync() em vez de GetAllAsync()")]
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Obtém todas as entidades presentes no repositório assincronamente.
        /// </summary>
        /// <param name="navProperties">As propriedades a serem incluidas</param>
        /// <returns>Tarefa representando a operação assíncrona, resultando em uma coleção de todas as entidades.</returns>
        [Obsolete("Esse metodo retorna todas as entidades do banco de dados sem tipo algum de filtragem forçando excessivamente o banco de dados e a pressão do ORM escolhido. Utilize FindAsync() em vez de GetAllAsync()")]
        Task<IEnumerable<TEntity>> GetAllAsync(params string[] navProperties);

        /// <summary>
        /// Encontra entidades com base no predicado fornecido.
        /// </summary>
        /// <param name="conditions">Objeto contendo propriedade a serem comparadas.</param>
        /// <returns>Tarefa representando a operação assíncrona, resultando em uma coleção das entidades encontradas.</returns>
        Task<IEnumerable<TEntity>> FindAsync(object conditions);

        /// <summary>
        /// Encontra entidades com base no predicado fornecido.
        /// </summary>
        /// <param name="conditions">Objeto contendo propriedade a serem comparadas.</param>
        /// <param name="navProperties">Objeto contendo propriedades a serem incluidas</param>
        /// <returns>Tarefa representando a operação assíncrona, resultando em uma coleção das entidades encontradas.</returns>
        Task<IEnumerable<TEntity>> FindAsync(object conditions, params string[] navProperties);
        /// <summary>
        /// Adiciona uma entidade ao repositório assincronamente.
        /// </summary>
        /// <param name="entity">Entidade a ser adicionada.</param>
        /// <returns>Tarefa representando a operação assíncrona, indicando se a adição foi bem-sucedida (true) ou não (false).</returns>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// Adiciona uma coleção de entidades ao repositório assincronamente.
        /// </summary>
        /// <param name="entities">Coleção de entidades a serem adicionadas.</param>
        /// <returns>Tarefa representando a operação assíncrona, indicando se a adição em lote foi bem-sucedida (true) ou não (false).</returns>
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Remove uma entidade do repositório assincronamente.
        /// </summary>
        /// <param name="entity">Entidade a ser removida.</param>
        /// <returns>Tarefa representando a operação assíncrona, indicando se a remoção foi bem-sucedida (true) ou não (false).</returns>
        Task RemoveAsync(TEntity entity);

        /// <summary>
        /// Remove uma coleção de entidades do repositório assincronamente.
        /// </summary>
        /// <param name="entities">Coleção de entidades a serem removidas.</param>
        /// <returns>Tarefa representando a operação assíncrona, indicando se a remoção em lote foi bem-sucedida (true) ou não (false).</returns>
        Task RemoveRangeAsync(IEnumerable<TEntity> entities);
    }
}
