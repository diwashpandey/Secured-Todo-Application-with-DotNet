using System.Threading.Tasks;
using System.Xml.XPath;
using MongoDB.Driver;
using TodoApi.Contexts;
using TodoApi.DTOs.ApiResponse;
using TodoApi.DTOs.TodoDTOs;
using TodoApi.Models;

public class TodoService
{
    private readonly IMongoCollection<Todo> _todoCollection;
    public TodoService(TodoDBContext context)
    {
        _todoCollection = context.Todos;
    }

    public async Task<GetTodosResponse> GetTodosByUserAsync(string? userId)
    {
        var filter = Builders<Todo>.Filter.Eq(todo => todo.UserId, userId);
        List<Todo> todos =  await _todoCollection.Find(filter).ToListAsync();
        return new GetTodosResponse{Todos = todos};
    }

    public async Task<ApiResponse> AddTodoAsync(string userId, PostTodoRequest todoData){
        
        ApiResponse apiResponse = new();

        if(todoData.Description==null || todoData.Description == ""){
            apiResponse.MessageFromServer = "Description must be provided!";
        }
        else if (userId==null){
            apiResponse.MessageFromServer = "Invalid userId";
        }
        else{
            Todo newTodo = new() {
                UserId = userId,
                Description = todoData.Description,
                DeadLine = todoData.DeadLine ?? null
            };
            await _todoCollection.InsertOneAsync(newTodo);
            apiResponse.SuccessStatus = true;
        }
        return apiResponse;
    }

    public async Task<ApiResponse> UpdateTodoAsync(string userId, UpdateTodoRequest requestData)
    {
        ApiResponse apiResponse = new();

        if (userId==null || userId == ""){
            apiResponse.MessageFromServer = "Invalid User!";
            return apiResponse;
        }

        var filter = Builders<Todo>.Filter.And(
                        Builders<Todo>.Filter.Eq(todo => todo.Id, requestData.Id),
                        Builders<Todo>.Filter.Eq(todo => todo.UserId, userId)
                    );

        var update = Builders<Todo>.Update.Set(requestData.Field, requestData.Data);
        var result = await _todoCollection.UpdateOneAsync(filter, update);
        
        apiResponse.SuccessStatus = result.IsAcknowledged && result.ModifiedCount > 0;
        
        if (!apiResponse.SuccessStatus) apiResponse.MessageFromServer = "Todo not found!";
        return apiResponse;
    }

    public async Task<bool> DeleteTodoAsync(string userId, string id)
    {
        var filter = Builders<Todo>.Filter.And(
          Builders<Todo>.Filter.Eq(t => t.UserId, userId),  
          Builders<Todo>.Filter.Eq(t => t.Id, id)  
        );

        var result =await _todoCollection.DeleteOneAsync(filter);
        return result.DeletedCount > 0;
    }
}