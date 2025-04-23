using FluentValidation;
using TodoApi.DTOs.UserDTOs;

namespace TodoApi.Validators.UserValidators;

public class SignupRequestValidator : AbstractValidator<SignupRequest>
{
    public SignupRequestValidator()
    {
        RuleFor(data => data.Username)
            .NotEmpty().WithMessage("Username is required!")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters")
            .MaximumLength(20).WithMessage("Username cannot be more than 20 characters");

        RuleFor(data => data.RawPassword)
            .NotEmpty().WithMessage("Password is required!")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .MaximumLength(20).WithMessage("Password cannot be more than 20 characters")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches(@"\d").WithMessage("Password must contain at least one number")
            .Matches(@"[\W_]").WithMessage("Password must contain at least one special character");
    }
}
