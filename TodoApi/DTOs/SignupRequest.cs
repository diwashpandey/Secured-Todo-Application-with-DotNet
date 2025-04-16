namespace TodoApi.DTOs;
public class SignupRequest
{
    public string Username {get; set;} = string.Empty;
    public string RawPassword {get; set;} = string.Empty;
    public string Email {get; set;} = string.Empty;
    public string RefreshToken {get; set;} = string.Empty;
    public DateTime RefreshTokenExpiryDate {get; set;}
}