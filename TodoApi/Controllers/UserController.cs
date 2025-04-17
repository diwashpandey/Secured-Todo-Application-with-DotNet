using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Services;

namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserAuthService _userAuthService;

    public UserController(UserAuthService userAuthService)
    {
        _userAuthService = userAuthService;
    }

    [HttpPost("login")]
    public async Task<LoginResponse> Login([FromBody] LoginRequest user)
    {
        // This service verifies the user and returns tokens if valid
        return await _userAuthService.LoginUserAsync(user);
    }

    [HttpPost("signup")]
    public async Task<SignupResponse> SignupUser([FromBody] SignupRequest userData)
    {
        return await _userAuthService.RegisterUserAsync(userData);
    }

    [HttpPost("renew-access")]
    public async Task<RenewTokenResponse> RenewToken([FromBody] RenewTokenRequest renewTokenRequest){
        return await _userAuthService.RenewAccessTokenAsync(renewTokenRequest);
    }
}
