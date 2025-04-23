using FluentValidation;
using TodoApi.DTOs.UserDTOs;

namespace TodoApi.Validators.UserValidators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(data => data.Username).NotEmpty().WithMessage("Username is required!");
        RuleFor(data => data.Password).NotEmpty().WithMessage("Password is required!");
    }
}
