using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Services;

namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserLoginController : ControllerBase
{
    private readonly UserAuthServices _userAuthServices;

    public UserLoginController(UserAuthServices userAuthServices)
    {
        _userAuthServices = userAuthServices;
    }

    [HttpPost]
    public IActionResult Login(LoginRequest user)
    {
        LoginResponse result =  _userAuthServices.LoginUser(user);

        if (result.Authenticated) return Ok(result);

        return BadRequest("Credentials doesn't match!");

    }
}