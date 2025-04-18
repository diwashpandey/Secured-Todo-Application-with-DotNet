namespace TodoApi.DTOs.UserDTOs;

public class RenewTokenResponse
{
    public string? NewAccessToken {get; set;}
    public bool SuccessStatus {get; set;} = false; // default is false
}