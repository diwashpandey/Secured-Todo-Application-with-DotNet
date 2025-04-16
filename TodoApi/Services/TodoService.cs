using MongoDB.Driver;
using TodoApi.Contexts;
using TodoApi.Models;

public class TodoService
{
    private readonly IMongoCollection<Todo> _todoCollection;
    public TodoService(TodoDBContext context)
    {
        _todoCollection = context.Todos;
    }
    // This is fills out the readonly _todoCollections;

    public List<Todo> FindAllTodos(){
        return _todoCollection.Find(_ => true).ToList();
    }

    public bool InsertTodo(Todo todo){

        if(todo==null) return false;
        
        _todoCollection.InsertOne(todo);

        return true;
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