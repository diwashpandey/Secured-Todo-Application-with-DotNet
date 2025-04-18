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

    private bool IsValidUserAsync(User user, string password)
    {
        if (user == null) return false;

        var result = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, password);
        return result == PasswordVerificationResult.Success;
    }

    private string GenerateJwtToken(User user, DateTime expiresAt)
    {
        var claims = new[] {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("UserId", user.Id)
        };

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

        var newUser = new User
        {
            Username = request.Username,
            Email = request.Email
        };
        var hashedPassword = _passwordHasher.HashPassword(newUser, request.RawPassword);
        newUser.HashedPassword = hashedPassword;

        await _userCollection.InsertOneAsync(newUser);

        return new SignupResponse { SuccessStatus = true };
    }

    public async Task<LoginResponse> LoginUserAsync([FromBody] LoginRequest request)
    {
        User user = await _userCollection.Find(u => u.Username == request.Username).FirstOrDefaultAsync();

        // This Authenticates user
        if (! this.IsValidUserAsync(user, request.Password))
        {
            return new LoginResponse
            {
                Authenticated = false,
                AccessToken = null,
                RefreshToken = null
            };
        }

        // Generate Access Token
        var accessToken = GenerateJwtToken(user, DateTime.Now.AddMinutes(15));
        // Generate random GUID token as refresh token
        var refreshToken = Guid.NewGuid().ToString();
        
        // Inserting the Token to the user databse
        var update = Builders<User>.Update
                        .Set(u => u.RefreshToken, refreshToken)
                        .Set(u=> u.TokenExpiryTime, DateTime.Now.AddDays(7));
        await _userCollection.UpdateOneAsync(u => u.Username == user.Username, update);

        return new LoginResponse
        {
            Authenticated = true,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<RenewTokenResponse> RenewAccessTokenAsync(RenewTokenRequest renewTokenRequest)
    {
        RenewTokenResponse renewTokenResponse = new();
        
        var filter = Builders<User>.Filter.And(
            Builders<User>.Filter.Eq(u=> u.Username, renewTokenRequest.Username),
            Builders<User>.Filter.Eq(u=> u.RefreshToken, renewTokenRequest.RefreshToken)
        );

        User user = await _userCollection.Find(filter).FirstOrDefaultAsync();

        // return false response if no user found or the token is expired
        if (user==null || user.TokenExpiryTime < DateTime.Now) return renewTokenResponse; // SuccessStatus if false in default
        
        renewTokenResponse.NewAccessToken = this.GenerateJwtToken(user, DateTime.Now.AddMinutes(15));
        renewTokenResponse.SuccessStatus = true;

        return renewTokenResponse;
    }
}