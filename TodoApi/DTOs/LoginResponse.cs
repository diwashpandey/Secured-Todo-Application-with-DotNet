namespace TodoApi.DTOs;

public class LoginResponse
{
    public string? RefreshToken {get; set;}
    public string? AccessToken {get; set;}
    public bool Authenticated {get; set;}
}