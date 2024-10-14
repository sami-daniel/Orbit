using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orbit.Application.Interfaces;
using Orbit.Domain.Entities;
using Orbit.DTOs.Requests;
using Orbit.DTOs.Responses;
using Orbit.Infrastructure.Data.Contexts;

namespace Orbit.Controllers;

[Authorize]
public class PostController : Controller
{
    private readonly IPostService _postService;
    private readonly IMapper _mapper;

    public PostController(IPostService postService, IMapper mapper)
    {
        _postService = postService;
        _mapper = mapper;
    }

    [HttpPost("[controller]/create-post")]
    public async Task<IActionResult> CreatePost([FromBody] PostAddRequest post)
    {
        if (!ModelState.IsValid)
        {
            var errors = string.Join(',', ModelState.Values.SelectMany(e => e.Errors.Select(e => e.ErrorMessage)));

            return BadRequest(errors);
        }

        try
        {
            await _postService.AddPostAsync(_mapper.Map<Post>(post), post.PostOwnerName);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return NoContent();
    }

    [HttpGet("[controller]/get-paginated-post")]
    public async Task<IActionResult> GetPaginatedPost([FromServices] ApplicationDbContext context, [FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        var posts = await _postService.GetPaginatedPostAsync(skip, take, context);
        return Ok(Shuffle(posts).Select(p => _mapper.Map<PostResponse>(p)));
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
