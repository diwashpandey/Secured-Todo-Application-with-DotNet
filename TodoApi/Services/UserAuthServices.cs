// Imports from DotNet
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// Imports from External Libraries
using MongoDB.Driver;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

// Imports from application
using TodoApi.Contexts;
using TodoApi.Models;
using TodoApi.Settings;
using TodoApi.DTOs;


namespace TodoApi.Services;

public class UserAuthServices
{
    private readonly IMongoCollection<User> _userCollection;
    private readonly JWTSettings _jwtSettings;
    private readonly PasswordHasher<User> hasher = new PasswordHasher<User>();

    public UserAuthServices(TodoDBContext todoDBContext, IOptions<JWTSettings> jwtSettings)
    {
        _userCollection = todoDBContext.Users;
        _jwtSettings = jwtSettings.Value;
    }

    private async Task<bool> ValidateUserCredentials(string _username, string _password)
    {
        // Pulling User from DB
        User user = await _userCollection.Find(u => u.Username == _username).FirstOrDefaultAsync();

        if (user == null) return false; // Check if user exists or not
        
        // Compares and varifies the user password
        PasswordVerificationResult result = hasher.VerifyHashedPassword(user, user.HashedPassword, _password);

        if (result == PasswordVerificationResult.Failed) return false;
        
        return true;
    }

    public async Task<SignupResponse> RegisterUser([FromBody] SignupRequest userData)
    {
        
        // Check if user with the same username exists or not
        bool userExists = await _userCollection.Find(u => u.Username == userData.Username).AnyAsync();

        // Return Failed status with message if username already exists
        if (userExists) return new SignupResponse{
            SuccessStatus = false,
            MessageToClient = "Username already exists"
        };

        // Encrypt the password using hasher
        string hashedPassword = hasher.HashPassword(null, userData.RawPassword);
        // Creating new user model
        User newUser = new User {
            Username = userData.Username,
            HashedPassword = hashedPassword,
            Email = userData.Email
        };

        // Inserting the user model to the database
        await _userCollection.InsertOneAsync(newUser);

        // Returns the success result
        return new SignupResponse{
            SuccessStatus = true
        };
    }

    public async Task<LoginResponse> LoginUser([FromBody] LoginRequest user){

        if (! await this.ValidateUserCredentials(user.Username, user.Password))
            return new LoginResponse{
            AccessToken = null,
            RefreshToken = null,
            Authenticated = false
        };
        
        // Rest of the code
        var claims = new [] {
            new Claim(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Creating the access token
        var accessToken = new JwtSecurityToken(
            issuer: _jwtSettings.ValidIssuer,
            audience: _jwtSettings.ValidAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: creds
        );
        // Convert the access token to the string format
        var newAccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken);

        // Create refresh token
        var refreshToken = Guid.NewGuid().ToString();

        return new LoginResponse{
            AccessToken = newAccessToken,
            RefreshToken = refreshToken,
            Authenticated = true
        };
    }
}