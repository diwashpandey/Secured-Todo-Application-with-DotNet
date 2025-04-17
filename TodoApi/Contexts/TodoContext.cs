// Imports from dotnet
using Microsoft.Extensions.Options;
using MongoDB.Driver;

// Imports from application
using TodoApi.Models;
using TodoApi.Settings;

namespace TodoApi.Contexts;

// This is the context of Todo which helps us connect to the database and retrive the data
public class TodoDBContext
{
    private readonly IMongoCollection<Todo> _todoCollection;
    private readonly IMongoCollection<User> _userCollection;

    public TodoDBContext(IOptions<TodoDatabaseSettings> todoDatabaseSettings){

        // Connecting to the Mongodb
        var mongoClient = new MongoClient(todoDatabaseSettings.Value.ConnectionString);

        // Selecting database of Todo
        var database = mongoClient.GetDatabase(todoDatabaseSettings.Value.DatabaseName);

        // Getting and setting the collection of Todos and Users from the database
        _todoCollection = database.GetCollection<Todo>(todoDatabaseSettings.Value.TodosCollectionName);
        _userCollection = database.GetCollection<User>(todoDatabaseSettings.Value.UserCollectionName);

        // Creates userId index for optimized Todo queries
        var userIdIndex = Builders<Todo>.IndexKeys.Ascending(todo => todo.UserId);
        _todoCollection.Indexes.CreateOne(new CreateIndexModel<Todo>(userIdIndex));
    }

    public IMongoCollection<Todo> Todos => _todoCollection; // Returns Collection of Todo
    public IMongoCollection<User> Users => _userCollection; // Returns Collection of User
}