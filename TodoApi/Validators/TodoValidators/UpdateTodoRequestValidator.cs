using FluentValidation;
using TodoApi.DTOs.TodoDTOs;
using TodoApi.Models;

namespace TodoApi.Validators.TodoValidators;

public class UpdateTodoRequestValidator : AbstractValidator<UpdateTodoRequest>
{
    public UpdateTodoRequestValidator()
    {
        RuleFor(req => req.Id).NotEmpty().WithMessage("Id must be given!");
        RuleFor(req => req.Field).Must(BeValidField).WithMessage("Given Field is not Valid!");
        RuleFor(req =>req.Data).NotEmpty().NotNull().WithMessage("Field should not be empty must be given!");
    }
    public bool BeValidField(string fieldName){
        List<string> validFields = typeof(Todo).GetProperties().Select(p => p.Name).ToList();
        return validFields.Contains(fieldName);
    }
}