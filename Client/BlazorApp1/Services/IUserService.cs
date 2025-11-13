using ApiContracts_DTOs;

namespace BlazorApp1.Services;

public interface IUserService
{
    Task<UserDto> AddUserAsync(CreateUserDto request);
    Task UpdateUserAsync(int id, UpdateUserDTO request);
    
    //more methods
}