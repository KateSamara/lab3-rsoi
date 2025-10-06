using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Application.Services;
using ReservationSystem.DataAccess.Context;
using ReservationSystem.DataAccess.Repositories;
using ReservationSystem.Domain.Interfaces.Repositories;
using ReservationSystem.Domain.Interfaces.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connectionString = Environment.GetEnvironmentVariable("DOCKER_CONNECTION_STRING")
                       ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ReservationSystemContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddTransient<IReservationRepository, ReservationRepository>();
builder.Services.AddTransient<IReservationService, ReservationService>();

var app = builder.Build();

var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<ReservationSystemContext>();
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();