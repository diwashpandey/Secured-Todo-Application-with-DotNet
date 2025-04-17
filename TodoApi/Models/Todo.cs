using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoApi.Models;

public class Todo
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id {get; set;} = string.Empty;
    
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId{get;set;} = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? DeadLine {get; set;} 
    public DateTime? CompletedTime {get; set;} // Carries the date of completion if completed
    public bool IsCompleted {get; set;} = false; // Task is not completed when initialized
}