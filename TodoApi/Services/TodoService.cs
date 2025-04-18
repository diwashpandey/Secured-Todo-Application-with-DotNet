using MongoDB.Driver;
using TodoApi.Contexts;
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

    public async Task<PostTodoResponse> AddTodoAsync(string userId, PostTodoRequest todoData){
        
        PostTodoResponse postTodoResponse = new();
        if( todoData.Description==null || todoData.Description == ""){
            postTodoResponse.MessageToClient = "Description must be provided!";
        }
        else if (userId==null){
            postTodoResponse.MessageToClient = "Invalid userId";
        }
        else{
            Todo newTodo = new() {
                UserId = userId,
                Description = todoData.Description,
                DeadLine = todoData.DeadLine ?? null
            };
            await _todoCollection.InsertOneAsync(newTodo);
            postTodoResponse.SuccessStatus = true;
        }
        return postTodoResponse;
    }

    public bool UpdateFullTodo(Todo newTodo){
        if (newTodo==null) return false;
        var result = _todoCollection.ReplaceOne(todo => todo.Id == newTodo.Id, newTodo);

        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public bool UpdateField(string id, string fieldName, string data)
    {
        var filter = Builders<Todo>.Filter.Eq("id", id);
        var update = Builders<Todo>.Update.Set(fieldName, data);
        var result = _todoCollection.UpdateOne(filter, update);
        
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public bool DeleteTodo(string id)
    {
        var result = _todoCollection.DeleteOne(todo=> todo.Id == id);
        return result.DeletedCount > 0;
    }
}