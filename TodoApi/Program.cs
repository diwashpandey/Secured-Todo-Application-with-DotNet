using TodoApi.Contexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<TodoDatabaseSettings>(builder.Configuration.GetSection("TodoDatabaseSettings"));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Adding Contexts to the builder
builder.Services.AddSingleton<TodoDBContext>();

// Adding Services to the builder
builder.Services.AddScoped<TodoService>();


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
