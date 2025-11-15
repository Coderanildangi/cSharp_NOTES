using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Note_Server.Data;

var builder = WebApplication.CreateBuilder(args);

// Load from environment variables and appsettings.json by default.
DotNetEnv.Env.Load();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Rettrive the connection string from configuration.
var connectionString = Environment.GetEnvironmentVariable("ConnectionString");

// Add DbContext with SQL Server provider.
builder.Services.AddDbContext<Note_Server.Data.NotesDbContext>(options =>
    options.UseSqlServer(connectionString));

// Enable CORS to allow the React/Angular frontends to access the API
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();

// Apply migrations on startup (ensures DB is created and structure matches model, including seed data)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<NotesDbContext>();
    // This operation will automatically create the DB if it doesn't exist and apply migrations.
    dbContext.Database.Migrate();
}

app.MapControllers();

app.Run();
