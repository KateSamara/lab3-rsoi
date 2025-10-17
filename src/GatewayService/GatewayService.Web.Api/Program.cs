using System.Text.Json;
using System.Text.Json.Serialization;
using GatewayService.Application.Helpers.Queues;
using GatewayService.Application.Helpers.Workers;
using GatewayService.Application.Services;
using GatewayService.DataAccess.Gateways;
using GatewayService.DataAccess.Gateways.CircuitBreakers;
using GatewayService.DataAccess.Gateways.Configuration;
using GatewayService.Domain.Interfaces.Gateways;
using GatewayService.Domain.Interfaces.Services;
using GatewayService.Web.Api;
using GatewayService.Web.Api.Middlewares;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower));
});

builder.Services.AddScoped<ValidationFilterAttribute>();

builder.Services.AddTransient<ILibraryGateway, LibraryGateway>();
builder.Services.AddTransient<IRatingGateway, RatingGateway>();
builder.Services.AddTransient<IReservationGateway, ReservationGateway>();

builder.Services.AddTransient<ILibraryService, LibraryService>();
builder.Services.AddTransient<IRatingService, RatingService>();
builder.Services.AddTransient<IReservationService, ReservationService>();

builder.Services.AddSingleton(typeof(CircuitBreaker<>));
builder.Services.AddSingleton(typeof(TaskQueue<>));

builder.Services.AddHostedService<LibraryWorker>();
builder.Services.AddHostedService<RatingWorker>();

builder.Services.Configure<LibrarySystemConfiguration>(
    builder.Configuration.GetSection("LibrarySystemConfiguration"));

builder.Services.Configure<ReservationSystemConfiguration>(
    builder.Configuration.GetSection("ReservationSystemConfiguration"));

builder.Services.Configure<RatingSystemConfiguration>(
    builder.Configuration.GetSection("RatingSystemConfiguration"));

builder.Services.Configure<CircuitBreakerConfiguration>(
    builder.Configuration.GetSection("CircuitBreakerConfiguration"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();