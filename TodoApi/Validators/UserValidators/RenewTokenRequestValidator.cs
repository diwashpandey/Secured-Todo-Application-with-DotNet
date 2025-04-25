using FluentValidation;
using TodoApi.DTOs.UserDTOs;

namespace TodoApi.Validators.UserValidators;

public class RenewTokenRequestValidator : AbstractValidator<RenewTokenRequest>
{
    public RenewTokenRequestValidator()
    {
        RuleFor(data => data.Username).NotEmpty().WithMessage("Username is required!");
        RuleFor(data => data.RefreshToken).NotEmpty().WithMessage("RefreshToken is required!");
    }
}
