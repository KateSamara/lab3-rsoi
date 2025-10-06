using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RatingSystem.Application.Jobs;
using RatingSystem.Application.Services;
using RatingSystem.DataAccess.Context;
using RatingSystem.DataAccess.Repositories;
using RatingSystem.Domain.Interfaces.Repositories;
using RatingSystem.Domain.Interfaces.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connectionString = Environment.GetEnvironmentVariable("DOCKER_CONNECTION_STRING")
                       ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<RatingSystemContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddTransient<IRatingService, RatingService>();

builder.Services.AddScoped<InitializeDatabaseJob>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


var app = builder.Build();

var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<RatingSystemContext>();
var pendingMigrations = context.Database.GetPendingMigrations().ToList();
if (pendingMigrations.Any())
{
    Console.WriteLine($"Applying {pendingMigrations.Count} migrations...");
    context.Database.Migrate();
    Console.WriteLine("Migrations applied successfully");
}
else
{
    Console.WriteLine("Database is up-to-date");
}

var job = services.GetRequiredService<InitializeDatabaseJob>();
await job.InitializeDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();