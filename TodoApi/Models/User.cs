using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoApi.Models;

public class User{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id {get; set;} = string.Empty;
    public string Username {get;set;} = string.Empty;
    public string HashedPassword {get;set;} = string.Empty;
    public string Email {get; set;} = string.Empty;
}