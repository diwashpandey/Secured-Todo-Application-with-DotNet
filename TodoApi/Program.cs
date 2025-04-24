// Importing from DotNet
using System.Text;
using Microsoft.IdentityModel.Tokens;

// Importing from third party libraries
using FluentValidation;

// Importing from application
using TodoApi.Contexts;
using TodoApi.Services;
using TodoApi.Settings;
using TodoApi.Validators.TodoValidators;
using TodoApi.DTOs.TodoDTOs;
using TodoApi.DTOs.UserDTOs;
using TodoApi.Validators.UserValidators;
using TodoApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Configuring Database settings
builder.Services.Configure<TodoDatabaseSettings>(builder.Configuration.GetSection("TodoDatabaseSettings"));

// Configuring JWT settings
builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWT"));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Adding Contexts to the builder
builder.Services.AddSingleton<TodoDBContext>();

// Adding Validator
builder.Services.AddTransient<IValidator<UpdateTodoRequest>, UpdateTodoRequestValidator>();
builder.Services.AddTransient<IValidator<PostTodoRequest>, PostTodoRequestValidator>();
builder.Services.AddTransient<IValidator<SignupRequest>, SignupRequestValidator>();
builder.Services.AddTransient<IValidator<LoginRequest>, LoginRequestValidator>();


// Adding Services to the builder
builder.Services.AddSingleton<TodoService>();
builder.Services.AddSingleton<UserAuthService>();

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
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.Run();
