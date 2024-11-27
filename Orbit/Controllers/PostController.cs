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
    public async Task<IActionResult> CreatePost([FromForm] PostAddRequest postAddRequest, [FromForm] IFormFile? imageOrVideo, [FromForm] string returnTo)
    {
        if (!ModelState.IsValid)
        {
            var errors = string.Join(',', ModelState.Values.SelectMany(e => e.Errors.Select(e => e.ErrorMessage)));

            return BadRequest(errors);
        }

        if (imageOrVideo != null)
        {    
            if (imageOrVideo.ContentType == "image/png" || imageOrVideo.ContentType == "image/jpg" || imageOrVideo.ContentType == "image/jpeg" || imageOrVideo.ContentType == "image/gif")
            {
                postAddRequest.PostImageByteType = imageOrVideo;
            }
            else if (imageOrVideo.ContentType == "video/mp4" || imageOrVideo.ContentType == "video/mkv" || imageOrVideo.ContentType == "video/avi" || imageOrVideo.ContentType == "video/webm")
            {
                postAddRequest.PostVideoByteType = imageOrVideo;
            }
            else
            {
                return BadRequest("Invalid file type");
            }
        }

        var post = _mapper.Map<Post>(postAddRequest);

        // For some reason, the mapper is setting the byte array to an empty array instead of null.
        if (post.PostImageByteType != null && post.PostImageByteType!.Length == 0)
        {
            post.PostImageByteType = null;
        }

        if (post.PostVideoByteType != null && post.PostVideoByteType!.Length == 0)
        {
            post.PostVideoByteType = null;
        }
        
        try
        {
            await _postService.AddPostAsync(post, postAddRequest.PostOwnerName);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return RedirectPermanent(returnTo);
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
