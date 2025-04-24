// Importing from third party libraries
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Driver;

// Importing from application
using TodoApi.Contexts;
using TodoApi.DTOs.ApiResponse;
using TodoApi.DTOs.TodoDTOs;
using TodoApi.Exceptions;
using TodoApi.Models;

public class TodoService(TodoDBContext context)
{
    private readonly IMongoCollection<Todo> _todoCollection = context.Todos;

    public async Task<ApiResponse<GetTodosResponse>> GetTodosByUserAsync(string? userId)
    {
        var filter = Builders<Todo>.Filter.Eq(todo => todo.UserId, userId);
        List<Todo> todos =  await _todoCollection.Find(filter).ToListAsync();
        GetTodosResponse data = new() { Todos = todos};

        return new ApiResponse<GetTodosResponse> {
            SuccessStatus=true,
            Data = data
        };
    }

    public async Task<ApiResponse> AddTodoAsync(string userId, PostTodoRequest todoData){

        if (userId==null) throw new UnauthorizedException("Unauthorized request!");

        else{
            Todo newTodo = new() {
                UserId = userId,
                Description = todoData.Description,
                DeadLine = todoData.DeadLine ?? null
            };
            await _todoCollection.InsertOneAsync(newTodo);
        }
        return new ApiResponse{SuccessStatus=true};
    }

    public async Task<ApiResponse> UpdateTodoAsync(string userId, UpdateTodoRequest requestData)
    {
        if (userId==null || userId == "")
            throw new UnauthorizedException("Invalid User!");
        

        var filter = Builders<Todo>.Filter.And(
                        Builders<Todo>.Filter.Eq(todo => todo.Id, requestData.Id),
                        Builders<Todo>.Filter.Eq(todo => todo.UserId, userId)
                    );

        var update = Builders<Todo>.Update.Set(requestData.Field, requestData.Data);
        var result = await _todoCollection.UpdateOneAsync(filter, update);
        
        if (result.IsAcknowledged && result.ModifiedCount > 0)
            return new ApiResponse{SuccessStatus = true};
        
        throw new NotFoundException("Todo not found!");
    }

    public async Task<ApiResponse> DeleteTodoAsync(string userId, string id)
    {
        var filter = Builders<Todo>.Filter.And(
          Builders<Todo>.Filter.Eq(t => t.UserId, userId),  
          Builders<Todo>.Filter.Eq(t => t.Id, id)  
        );

        var result =await _todoCollection.DeleteOneAsync(filter);

        if (result.DeletedCount > 0)
        {
            return new ApiResponse{SuccessStatus=true};
        }
        
        throw new NotFoundException("Todo not found!");
    }
}