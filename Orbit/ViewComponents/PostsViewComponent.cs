using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Orbit.Data.Contexts;
using Orbit.DTOs.Responses;
using Orbit.Models;
using Orbit.Services.Interfaces;

namespace Orbit.ViewComponents;

// Define the PostsViewComponent class that handles displaying posts
public class PostsViewComponent : ViewComponent
{
    // The InvokeAsync method is responsible for retrieving and displaying posts
    public async Task<IViewComponentResult> InvokeAsync([FromServices] IPostService postService, [FromServices] ApplicationDbContext context, [FromServices] IMapper mapper, int skip = 0, int take = 50)
    {
        // Fetch paginated posts using the provided post service
        var posts = await postService.GetPaginatedPostAsync(skip, take);

        // Shuffle the posts and map them to PostResponse DTO before returning to the view
        return View(Shuffle(posts).Select(mapper.Map<PostResponse>));
    }

    // NonAction attribute ensures this method is not treated as an action by ASP.NET
    [NonAction]
    private static IEnumerable<Post> Shuffle(IEnumerable<Post> posts)
    {
        // Create a new random instance for shuffling
        Random random = new Random();
        var shuffledList = new List<Post>(posts);
        int n = shuffledList.Count;

        // Shuffle posts using the Fisher-Yates algorithm
        for (int i = n - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            // Swap posts at index i and j
            (shuffledList[j], shuffledList[i]) = (shuffledList[i], shuffledList[j]);
        }

        // Return the shuffled list of posts
        return shuffledList;
    }
}
