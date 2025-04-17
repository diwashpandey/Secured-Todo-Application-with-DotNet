namespace TodoApi.DTOs;

public class RenewTokenRequest
{
    public string Username {get; set;} = string.Empty;
    public string RefreshToken {get; set;} = string.Empty;
}