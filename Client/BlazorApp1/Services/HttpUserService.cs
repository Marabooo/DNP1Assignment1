using ApiContracts_DTOs;

namespace BlazorApp1.Services;

public class HttpUserService
{
    private readonly HttpClient Client;

    public HttpUserService(HttpClient client)
    {
        this.Client = client;
    }

    public Task<UserDto> AddUserAsync(CreateUserDto request)
    {
        //todo
        return null;
    }

    public Task UpdateUserAsync(int id, UpdateUserDTO request)
    {
        //todo
        return null;
    }
    
    //more methods

}