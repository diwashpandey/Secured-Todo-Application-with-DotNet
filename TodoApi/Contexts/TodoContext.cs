using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoApi.Models;

namespace TodoApi.Contexts;

// This is the context of Todo which helps us connect to the database and retrive the data
public class TodoDBContext
{
    private readonly IMongoCollection<Todo> _todoCollection;
    
    public TodoDBContext(IOptions<TodoDatabaseSettings> todoDatabaseSettings){

        // Connecting to the Mongodb
        var mongoClient = new MongoClient(todoDatabaseSettings.Value.ConnectionString);

        // Selecting database of Todo
        var database = mongoClient.GetDatabase(todoDatabaseSettings.Value.DatabaseName);

        // Getting and setting the collection of Todos from the database
        _todoCollection = database.GetCollection<Todo>(todoDatabaseSettings.Value.DatabaseName);
    }

    public IMongoCollection<Todo> Todos => _todoCollection; // Returns Collection of Todo
}