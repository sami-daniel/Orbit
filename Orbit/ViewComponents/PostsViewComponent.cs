using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Orbit.Data.Contexts;
using Orbit.DTOs.Responses;
using Orbit.Models;
using Orbit.Services.Interfaces;

namespace Orbit.ViewComponents;

public class PostsViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync([FromServices] IPostService postService, [FromServices] ApplicationDbContext context, [FromServices] IMapper mapper, int skip = 0, int take = 50)
    {
        var posts = await postService.GetPaginatedPostAsync(skip, take, context);
        return View(Shuffle(posts).Select(mapper.Map<PostResponse>));
    }

    [NonAction]
    private static IEnumerable<Post> Shuffle(IEnumerable<Post> posts)
    {
        Random random = new Random();
        var shuffledList = new List<Post>(posts);
        int n = shuffledList.Count;

        for (int i = n - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            (shuffledList[j], shuffledList[i]) = (shuffledList[i], shuffledList[j]);
        }

        return shuffledList;
    }
}
