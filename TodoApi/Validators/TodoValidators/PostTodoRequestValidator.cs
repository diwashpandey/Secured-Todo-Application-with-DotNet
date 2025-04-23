using FluentValidation;
using TodoApi.DTOs.TodoDTOs;

namespace TodoApi.Validators.TodoValidators;

public class PostTodoRequestValidator : AbstractValidator<PostTodoRequest>
{
    public PostTodoRequestValidator()
    {
        RuleFor(x => x.Description).NotEmpty().WithMessage("Please provide the description!");
        RuleFor(x => x.Description).NotNull().WithMessage("Please provide the description!");
        RuleFor(x => x.Description).MinimumLength(2).MaximumLength(500).WithMessage("Description length should be between 2 to 500 characters");
    }
}