using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Services;

namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserAuthServices _userAuthServices;

    public UserController(UserAuthServices userAuthServices)
    {
        _userAuthServices = userAuthServices;
    }

    [HttpPost("login")]
    public async Task<LoginResponse> Login([FromBody] LoginRequest user)
    {  
        // This service verifies the user and returns tokens if valid
        return await _userAuthServices.LoginUser(user);
    }

    [HttpPost("signup")]
    public async Task<SignupResponse> SignupUser([FromBody] SignupRequest userData)
    {
        return await _userAuthServices.RegisterUser(userData);
    }
}

