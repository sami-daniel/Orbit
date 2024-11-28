using Orbit.Models;
using Orbit.Repository.Core.Interfaces;

namespace Orbit.Repository.Interfaces;

/// <summary>
/// Interface for the Post Preferences repository.
/// </summary>
/// <inheritdoc cref="IRepository{TEntity}"/>
public interface IPostPreferenceRepository : IRepository<PostPreference>
{
}
