using System.Text;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Contexts;
using TodoApi.Services;
using TodoApi.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<TodoDatabaseSettings>(builder.Configuration.GetSection("TodoDatabaseSettings"));
builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWT"));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Adding Contexts to the builder
builder.Services.AddSingleton<TodoDBContext>();

// Adding Services to the builder
builder.Services.AddScoped<TodoService>();
builder.Services.AddScoped<UserAuthService>();

builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options => {

    var secretKey = builder.Configuration["JWT:SecretKey"] 
                        ?? throw new InvalidOperationException("JWT: SecretKey configuration is missing.");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
