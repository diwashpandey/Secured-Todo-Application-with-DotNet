// Importing from DotNet
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// Importing from third party libraries
using FluentValidation;

// Importing from Application
using TodoApi.Common;
using TodoApi.DTOs.ApiResponse;
using TodoApi.DTOs.TodoDTOs;
using TodoApi.Exceptions;

namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoController : CustomControllerBase
{
    private readonly TodoService _todoService;
    private readonly IValidator<UpdateTodoRequest> _updateTodoRequestValidator;
    private readonly IValidator<PostTodoRequest> _postTodoRequestValidator;

    public TodoController(

        TodoService todoService,
        IValidator<UpdateTodoRequest> updateTodoRequestValidator,
        IValidator<PostTodoRequest> postTodoRequestValidator

        ){
        _todoService = todoService;
        _updateTodoRequestValidator = updateTodoRequestValidator;
        _postTodoRequestValidator = postTodoRequestValidator;
    }

    [Authorize]
    [HttpGet]
    public async Task<ApiResponse<GetTodosResponse>> GetTodosByUser()
    {
        return await _todoService.GetTodosByUserAsync(this.GetUserId());
    }

    [Authorize]
    [HttpPost]
    public async Task<ApiResponse> PostTodo([FromBody] PostTodoRequest todoData)
    {
        var validated_data = _postTodoRequestValidator.Validate(todoData);

        if (!validated_data.IsValid)
        {
            var errorMessage = validated_data.Errors.FirstOrDefault()?.ErrorMessage ?? "Wrong Input";
            throw new BadRequestException(errorMessage);
        }

        return await _todoService.AddTodoAsync(this.GetUserId(), todoData);
    }

    [Authorize]
    [HttpDelete]
    public async Task<ApiResponse> RemoveTodo([FromBody] DeleteTodoRequest req)
    {
        return await _todoService.DeleteTodoAsync(this.GetUserId(), req.Id);
    }
    
    [Authorize]
    [HttpPatch]
    public async Task<ApiResponse> UpdateTodo([FromBody] UpdateTodoRequest data){        
        var validated_data = _updateTodoRequestValidator.Validate(data);

        if (!validated_data.IsValid)
        {
            var errorMessage = validated_data.Errors.FirstOrDefault()?.ErrorMessage ?? "Wrong Input";
            throw new BadRequestException(errorMessage);
        }
        
        return await _todoService.UpdateTodoAsync(this.GetUserId(), data);
    }
}