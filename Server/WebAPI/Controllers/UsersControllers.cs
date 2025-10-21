using Microsoft.AspNetCore.Mvc;
using ApiContracts_DTOs;
using Entities;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
    {
        try
        {
            await VerifyUserNameIsAvailableAsync(request.UserName);

            User user = new User
            {
                Username = request.UserName,
                Password = request.Password
            };
            User created = await userRepo.AddAsync(user);
            UserDto dto = new UserDto
            {
                Id = created.Id,
                UserName = created.Username
            };
            return Created($"/users/{dto.Id}", dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetMany([FromQuery] string? contains)
    {
        var users = userRepo.GetManyAsync();

        if (!string.IsNullOrWhiteSpace(contains))
            users = users.Where(u => u.Username.Contains(contains, StringComparison.OrdinalIgnoreCase));

        var result = users.Select(u => new UserDto { Id = u.Id, UserName = u.Username });
        return Ok(result);
    }
    private async Task VerifyUserNameIsAvailableAsync(string username)
    {
        var existing = userRepo.GetManyAsync()
            .SingleOrDefault(u => u.Username == username);

        if (existing is not null)
            throw new InvalidOperationException("Username already taken.");
    }
}