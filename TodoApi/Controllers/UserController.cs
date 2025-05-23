// Imports from DotNet
using Microsoft.AspNetCore.Mvc;

// Imports from third party libraries
using FluentValidation;

// Imports from application
using TodoApi.DTOs.UserDTOs;
using TodoApi.Common;
using TodoApi.Services;
using TodoApi.Exceptions;
using TodoApi.DTOs.ApiResponse;


namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : CustomControllerBase
{
    private readonly UserAuthService _userAuthService;
    private readonly IValidator<SignupRequest> _signupRequestValidator;
    private readonly IValidator<LoginRequest> _loginRequestValidator;
    private readonly IValidator<RenewTokenRequest> _renewTokenRequestValidator;
    

    public UserController(
        UserAuthService userAuthService,
        IValidator<SignupRequest> signupRequestValidator,
        IValidator<LoginRequest> loginRequestValidator,
        IValidator<RenewTokenRequest> renewTokenRequestValidator
    )
    {
        _userAuthService = userAuthService;
        _signupRequestValidator = signupRequestValidator;
        _loginRequestValidator = loginRequestValidator;
        _renewTokenRequestValidator = renewTokenRequestValidator;
    }

    [HttpPost("login")]
    public async Task<ApiResponse> Login([FromBody] LoginRequest user)
    {
        var validationResult = _loginRequestValidator.Validate(user);

        if (!validationResult.IsValid)
        {
            string errorMessage = validationResult.Errors.FirstOrDefault()?.ErrorMessage ?? "Invalid input!";
            throw new BadRequestException(errorMessage);
        }
        
        return await _userAuthService.LoginUserAsync(user);
    }

    [HttpPost("signup")]
    public async Task<ApiResponse> SignupUser([FromBody] SignupRequest userData)
    {
        var validationResult = _signupRequestValidator.Validate(userData);

        if (!validationResult.IsValid)
        {
            string errorMessage = validationResult.Errors.FirstOrDefault()?.ErrorMessage ?? "Invalid input!";
            throw new BadRequestException(errorMessage);
        }

        return await _userAuthService.RegisterUserAsync(userData);
    }

    [HttpPost("renew-access")]
    public async Task<RenewTokenResponse> RenewToken([FromBody] RenewTokenRequest renewTokenRequest){
        var validationResult = _renewTokenRequestValidator.Validate(renewTokenRequest);

        if (!validationResult.IsValid)
        {   
            string errorMessage = validationResult.Errors.FirstOrDefault()?.ErrorMessage ?? "Invalid input!";
            throw new BadRequestException(errorMessage);
        }

        return await _userAuthService.RenewAccessTokenAsync(renewTokenRequest);
    }
}
