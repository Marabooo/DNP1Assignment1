using Microsoft.AspNetCore.Mvc;
using ApiContracts_DTOs;
using Entities;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;

    public PostsController(IPostRepository postRepo, IUserRepository userRepo)
    {
        this.postRepo = postRepo;
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<Post>> AddPost([FromBody] Post request)
    {
        try
        {
            await userRepo.GetSingleAsync(request.UserId); // verify author exists
            Post created = await postRepo.AddAsync(request);
            return Created($"/posts/{created.Id}", created);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<Post>> GetMany([FromQuery] string? titleContains, [FromQuery] int? userId)
    {
        var posts = postRepo.GetManyAsync();

        if (!string.IsNullOrWhiteSpace(titleContains))
            posts = posts.Where(p => p.Title.Contains(titleContains, StringComparison.OrdinalIgnoreCase));

        if (userId.HasValue)
            posts = posts.Where(p => p.UserId == userId.Value);

        return Ok(posts);
    }
}