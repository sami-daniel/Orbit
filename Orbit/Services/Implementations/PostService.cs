using Microsoft.EntityFrameworkCore;
using Orbit.Data.Contexts;
using Orbit.Models;
using Orbit.Services.Exceptions;
using Orbit.Services.Helpers;
using Orbit.Services.Interfaces;
using Orbit.UnitOfWork.Interfaces;

namespace Orbit.Services.Implementations;

public class PostService : IPostService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ApplicationDbContext _context;

    public PostService(IUnitOfWork unitOfWork, ApplicationDbContext context)
    {
        _context = context;
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

        var words = post.PostContent.Split(' ');
        var hashtags = new List<string>();
        foreach (var word in words)
        {
            if (word.StartsWith('#'))
            {
                hashtags.Add(word);
            }
        }

        post.User = postOwner.First();
        post.UserId = postOwner.First().UserId;
    

        await _unitOfWork.PostRepository.InsertAsync(post);
        
        foreach (var hashtag in hashtags)
        {
            var postPreference = new PostPreference
            {
                PostId = post.PostId,
                PreferenceName = hashtag,
            };
            await _unitOfWork.PostPreferenceRepository.InsertAsync(postPreference);
        }
        
        await _unitOfWork.CompleteAsync();
    }

    public async Task<IEnumerable<Post>> GetPaginatedPostAsync(int skip, int take)
    {
        return await _context.Posts
        .OrderBy(p => p.PostId)
        .Skip(skip)
        .Take(take)
        .ToListAsync();
    }

    public async Task<Post?> GetPostByIdAsync(uint postId)
    {
        var post = await _unitOfWork.PostRepository.GetAsync(p => p.PostId == postId);

        if (post == null)
        {
            return null;
        }

        return post.First();
    }

    public async Task<IEnumerable<Post>> GetPostsRandomizedByUserPreferenceAsync(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username)
                   ?? throw new UserNotFoundException($"The user with the username {username} was not found.");

        var userPreferences = await _context.UserPreferences
                          .Where(up => up.UserId == user.UserId)
                          .ToListAsync();

        if (userPreferences.Count == 0)
        {
            // In case that user has no preferences, return the last 50 posts, randomized.
            var unshufledPostsUnpreferenced = await _context.Posts
                   .Take(50)
                   .OrderBy(p => Guid.NewGuid())
                   .Include(p => p.User)
                   .ToListAsync();
            return Shuffle(unshufledPostsUnpreferenced);
        }
        
        var posts = _context.Database.SqlQuery<Post>($"""
            SELECT p.* FROM post p
            JOIN post_preference USING(post_id)
            WHERE post_preference.preference_name = IN({string.Join(',', userPreferences.Select(up => up.PreferenceName))})
            ORDER BY RAND()
            LIMIT 50
        """);

        if (posts.Count() < 50)
        {
            posts = _context.Database.SqlQuery<Post>($"""
                SELECT * FROM 
                (SELECT p.* FROM post p
                JOIN post_preference USING(post_id)
                WHERE post_preference.preference_name IN({string.Join(',', userPreferences.Select(up => up.PreferenceName))})
                UNION
                SELECT p.* FROM post p
                JOIN post_preference USING(post_id)
                WHERE post_preference.preference_name IN({string.Join(',', userPreferences.Select(up => up.PreferenceName))}))
                AS Result 
                ORDER BY RAND()
                LIMIT 50
            """);
        }

        var unshufledPosts = await posts.ToListAsync();

        return Shuffle(unshufledPosts);
    }

    private IEnumerable<Post> Shuffle(IEnumerable<Post> posts)
    {
        // Fisher-Yates shuffle algorithm
        // See https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle for more information

        var list = posts.ToList();
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = new Random().Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }

        return list;
    }
}
