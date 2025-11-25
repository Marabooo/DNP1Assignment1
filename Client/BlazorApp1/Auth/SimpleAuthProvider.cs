using System.Security.Claims;
using System.Text.Json;
using ApiContracts_DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity.Data;

namespace BlazorApp1.Auth;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient httpClient;
    private ClaimsPrincipal currentClaimsPrincipal;

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        throw new NotImplementedException();
    }
    //
    
    public SimpleAuthProvider(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task Login(string userName, string password)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(
            "auth/login",
            new LoginRequest { Email = userName, Password = password });

        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
        UserDto userDto = JsonSerializer.Deserialize<UserDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;


        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, userDto.UserName),
            new Claim("Id", userDto.Id.ToString()),
            // Add more claims here with your own claim type as a string, e.g.:
            // new Claim("DateOfBirth", userDto.DateOfBirth.ToString("yyyy-MM-dd"))
            // new Claim("Role", userDto.Role)
            // new Claim("Email", userDto.Email)
        };
        ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");
        currentClaimsPrincipal = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(currentClaimsPrincipal))
        );
    }


}