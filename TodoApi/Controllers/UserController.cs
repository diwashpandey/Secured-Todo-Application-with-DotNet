// Imports from DotNet
using Microsoft.AspNetCore.Mvc;

// Imports from third party libraries
using FluentValidation;

// Imports from application
using TodoApi.DTOs.UserDTOs;
using TodoApi.Common;
using TodoApi.Services;
using TodoApi.Validators.UserValidators;
using TodoApi.DTOs.ApiResponse;


namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : CustomControllerBase
{
    private readonly UserAuthService _userAuthService;
    private readonly IValidator<SignupRequest> _signupRequestValidator;
    private readonly IValidator<LoginRequest> _loginRequestValidator;

    public UserController(
        UserAuthService userAuthService,
        IValidator<SignupRequest> signupRequestValidator,
        IValidator<LoginRequest> _loginRequestValidator
        )
    {
        _userAuthService = userAuthService;
        _signupRequestValidator = signupRequestValidator;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse>> Login([FromBody] LoginRequest user)
    {
        var validatedResult = _loginRequestValidator.Validate(user);

        if (!validatedResult.IsValid)
        {
            return BadRequest(new ApiResponse{
                MessageFromServer = validatedResult.Errors.FirstOrDefault()?.ErrorMessage
            });
        }

        var result =  await _userAuthService.LoginUserAsync(user);
        return result.SuccessStatus ? Ok(result) : BadRequest(result);
    }

    [HttpPost("signup")]
    public async Task<ActionResult> SignupUser([FromBody] SignupRequest userData)
    {
        var validationResult = _signupRequestValidator.Validate(userData);

        if (!validationResult.IsValid)
        {
            return BadRequest(new ApiResponse
            {
                MessageFromServer = validationResult.Errors.FirstOrDefault()?.ErrorMessage
            });
        }

        ApiResponse result = await _userAuthService.RegisterUserAsync(userData);

        return result.SuccessStatus ? Ok(result) : BadRequest(result);
    }

    [HttpPost("renew-access")]
    public async Task<RenewTokenResponse> RenewToken([FromBody] RenewTokenRequest renewTokenRequest){
        return await _userAuthService.RenewAccessTokenAsync(renewTokenRequest);
    }
}
