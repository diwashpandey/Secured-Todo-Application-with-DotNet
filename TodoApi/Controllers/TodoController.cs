// Importing from DotNet
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// Importing from third party libraries
using FluentValidation;

// Importing from Application
using TodoApi.Models;
using TodoApi.Common;
using TodoApi.DTOs.ApiResponse;
using TodoApi.DTOs.TodoDTOs;
using TodoApi.Validators.TodoValidators;

namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoController : CustomControllerBase
{
    private readonly TodoService _todoService;
    private readonly IValidator<UpdateTodoRequest> _updateTodoRequestValidator;

    public TodoController(

        TodoService todoService,
        IValidator<UpdateTodoRequest> updateTodoRequestValidator

        ){
        _todoService = todoService;
        _updateTodoRequestValidator = updateTodoRequestValidator;
    }

    [Authorize]
    [HttpGet]
    public async Task<GetTodosResponse> GetTodosByUser()
    {
        return await _todoService.GetTodosByUserAsync(this.GetUserId());
    }

    [Authorize]
    [HttpPost]
    public async Task<ApiResponse> PostTodo([FromBody] PostTodoRequest todoData)
    {
        return await _todoService.AddTodoAsync(this.GetUserId(), todoData);
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult> RemoveTodo([FromBody] DeleteTodoRequest req)
    {
        bool success = await _todoService.DeleteTodoAsync(this.GetUserId(), req.Id);

        ApiResponse apiResponse = new(){SuccessStatus=success};

        if (success) return Ok(apiResponse);

        apiResponse.MessageFromServer = "Todo not found!";
        return BadRequest(apiResponse);
    }

    [HttpPut]
    public async Task<ApiResponse> PutTodo([FromBody] Todo new_todo){
        return await _todoService.ReplaceTodoAsync(new_todo); 
    }
    
    [Authorize]
    [HttpPatch]
    public async Task<ActionResult> UpdateTodo([FromBody] UpdateTodoRequest data){        
        var result = _updateTodoRequestValidator.Validate(data);

        if (! result.IsValid){
            return BadRequest(new ApiResponse{
                MessageFromServer = result.Errors.FirstOrDefault()?.ErrorMessage
            });
        }
        
        ApiResponse response = await _todoService.UpdateTodoAsync(this.GetUserId(), data);

        if (response.SuccessStatus) return Ok(result);

        return BadRequest(response);
    }
}