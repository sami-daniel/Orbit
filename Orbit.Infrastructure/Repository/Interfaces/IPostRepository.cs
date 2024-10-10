using Orbit.Domain.Entities;
using Orbit.Infrastructure.Repository.Core.Interfaces;

namespace Orbit.Infrastructure.Repository.Interfaces;

/// <summary>
/// Interface para o repositório de Posts.
/// </summary>
public interface IPostRepository : IRepository<Post>
{
}
