using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orbit.DTOs.Requests;
using Orbit.Models;
using Orbit.Services.Interfaces;

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

    [HttpGet("[controller]/get-post-image/{postID}")]
    public async Task<IActionResult> GetPostImage(uint postID)
    {
        var post = await _postService.GetPostByIdAsync(postID);
    
        if (post == null)
        {
            return NotFound("Post not found");
        }

        if (post.PostImageByteType == null)
        {
            return NotFound("Post has no image");
        }

        return File(post.PostImageByteType, "image/jpeg");
    }

    [HttpGet("[controller]/get-post-video/{postID}")]
    public async Task<IActionResult> GetPostVideo(uint postID)
    {
        var post = await _postService.GetPostByIdAsync(postID);
    
        if (post == null)
        {
            return NotFound("Post not found");
        }

        if (post.PostVideoByteType == null)
        {
            return NotFound("Post has no video");
        }

        return File(post.PostVideoByteType, "video/mp4");
    }
}
