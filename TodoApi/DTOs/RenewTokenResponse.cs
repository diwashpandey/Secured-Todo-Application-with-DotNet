namespace TodoApi.DTOs;

public class RenewTokenResponse
{
    public string? NewAccessToken {get; set;}
    public bool SuccessStatus {get; set;} = false; // default is false
}