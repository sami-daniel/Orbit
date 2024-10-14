using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orbit.Application.Helpers;
using Orbit.Application.Interfaces;
using Orbit.Domain.Entities;
using Orbit.Infrastructure.Data.Contexts;
using Orbit.Infrastructure.UnitOfWork.Interfaces;

namespace Orbit.Application.Services;

public class PostService : IPostService
{
    private readonly IUnitOfWork _unitOfWork;

    public PostService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddPostAsync(Post post, string postOwnerName)
    {
        try
        {
            PostServiceHelpers.ValidatePost(post);
        }
        catch (Exception)
        {
            throw;
        }
        var postOwner = await _unitOfWork.UserRepository.GetAsync(f => f.UserName == postOwnerName);

        if (!postOwnerName.Any())
        {
            throw new ArgumentException("User not found");
        }

        post.User = postOwner.First();
        post.UserId = postOwner.First().UserId;
        await _unitOfWork.PostRepository.InsertAsync(post);
    }

    public async Task<IEnumerable<Post>> GetPaginatedPostAsync(int skip, int take, ApplicationDbContext applicationDbContext)
    {
        return await applicationDbContext.Posts
        .OrderBy(p => p.PostId)
        .Skip(skip)
        .Take(take)
        .ToListAsync();


    }
}
