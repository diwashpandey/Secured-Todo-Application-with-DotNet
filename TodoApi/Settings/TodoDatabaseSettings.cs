// It's like a DTO for teh configuration of database settings that is in appsettings.json
public class TodoDatabaseSettings
{
    public string ConnectionString {get; set;} = null!;
    public string DatabaseName {get; set;} = null!;
    public string TodosCollectionName {get; set;} = null!;
}