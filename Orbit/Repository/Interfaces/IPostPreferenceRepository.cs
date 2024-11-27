using Orbit.Models;
using Orbit.Repository.Core.Interfaces;

namespace Orbit.Repository.Interfaces;

/// <summary>
/// Interface para o repositório de Preferências de Post.
/// </summary>
/// <inheritdoc cref="IRepository{TEntity}"/>
public interface IPostPreferenceRepository : IRepository<PostPreference>
{
}