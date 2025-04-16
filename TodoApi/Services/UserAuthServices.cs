using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TodoApi.Contexts;
using TodoApi.Models;
using TodoApi.Settings;
using TodoApi.DTOs;

namespace TodoApi.Services;

public class UserAuthService
{
    private readonly IMongoCollection<User> _userCollection;
    private readonly JWTSettings _jwtSettings;
    private readonly PasswordHasher<User> _passwordHasher = new();

    public UserAuthService(TodoDBContext context, IOptions<JWTSettings> jwtOptions)
    {
        _userCollection = context.Users;
        _jwtSettings = jwtOptions.Value;
    }

    private async Task<bool> IsValidUserAsync(string username, string password)
    {
        var user = await _userCollection.Find(u => u.Username == username).FirstOrDefaultAsync();
        if (user == null) return false;

        var result = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, password);
        return result == PasswordVerificationResult.Success;
    }

    private string GenerateJwtToken(string username, DateTime expiresAt)
    {
        var claims = new[] { new Claim(ClaimTypes.Name, username) };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.ValidIssuer,
            audience: _jwtSettings.ValidAudience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<SignupResponse> RegisterUserAsync([FromBody] SignupRequest request)
    {
        var userExists = await _userCollection.Find(u => u.Username == request.Username).AnyAsync();
        if (userExists)
        {
            return new SignupResponse
            {
                SuccessStatus = false,
                MessageToClient = "Username already exists"
            };
        }

        var hashedPassword = _passwordHasher.HashPassword(null, request.RawPassword);
        var newUser = new User
        {
            Username = request.Username,
            HashedPassword = hashedPassword,
            Email = request.Email
        };

        await _userCollection.InsertOneAsync(newUser);

        return new SignupResponse { SuccessStatus = true };
    }

    public async Task<LoginResponse> LoginUserAsync([FromBody] LoginRequest request)
    {
        if (!await IsValidUserAsync(request.Username, request.Password))
        {
            return new LoginResponse
            {
                Authenticated = false,
                AccessToken = null,
                RefreshToken = null
            };
        }

        var accessToken = GenerateJwtToken(request.Username, DateTime.Now.AddMinutes(15));
        var refreshToken = Guid.NewGuid().ToString(); // Ideally, store this securely.

        return new LoginResponse
        {
            Authenticated = true,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}
