using FluentValidation;
using TodoApi.DTOs.TodoDTOs;

namespace TodoApi.Validators.TodoValidators;

public class PostTodoRequestValidator : AbstractValidator<PostTodoRequest>
{
    public PostTodoRequestValidator()
    {
        RuleFor(x => x.Description).NotEmpty().WithMessage("Please provide the description!")
        .MinimumLength(3).WithMessage("Description must be atlease 3 characters")
        .MaximumLength(500).WithMessage("Description cannot extend more than 500 characters");
    }
}