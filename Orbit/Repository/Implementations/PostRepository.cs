using Orbit.Data.Contexts;
using Orbit.Models;
using Orbit.Repository.Core.Implementations;
using Orbit.Repository.Interfaces;

namespace Orbit.Repository.Implementations;

/// <summary>
/// Implementation of the Post repository.
/// </summary>
public class PostRepository : Repository<Post>, IPostRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PostRepository"/> class with the provided database context.
    /// </summary>
    /// <param name="context">The <see cref="ApplicationDbContext"/> to be provided.</param>
    public PostRepository(ApplicationDbContext context) : base(context)
    {
    }
}
