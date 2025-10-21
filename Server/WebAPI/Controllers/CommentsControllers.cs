using Microsoft.AspNetCore.Mvc;
using Entities;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;

    public CommentsController(ICommentRepository commentRepository, IPostRepository postRepository)
    {
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
    }

    [HttpPost]
    public async Task<ActionResult<Comment>> AddComment([FromBody] Comment comment)
    {
        try
        {
            // verify post exists before adding
            await postRepository.GetSingleAsync(comment.PostId);

            Comment created = await commentRepository.AddAsync(comment);
            return Created($"/comments/{created.Id}", created);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<Comment>> GetMany([FromQuery] int? postId)
    {
        var comments = commentRepository.GetManyAsync();

        if (postId.HasValue)
            comments = comments.Where(c => c.PostId == postId.Value);

        return Ok(comments);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await commentRepository.DeleteAsync(id);
            return Ok();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }
}