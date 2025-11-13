using ApiContracts_DTOs;

namespace BlazorApp1.Services;

public interface IUserService
{
    public Task<UserDto> AddUserAsync(CreateUserDto request);
    public Task UpdateUserAsync(int id, UpdateUserDTO request);
}