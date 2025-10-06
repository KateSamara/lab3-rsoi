using System.Text.Json;
using System.Text.Json.Serialization;
using GatewayService.Configuration;
using GatewayService.Web.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower));
});

builder.Services.AddScoped<ValidationFilterAttribute>();

builder.Services.Configure<LibrarySystemConfiguration>(
    builder.Configuration.GetSection("LibrarySystemConfiguration"));

builder.Services.Configure<ReservationSystemConfiguration>(
    builder.Configuration.GetSection("ReservationSystemConfiguration"));

builder.Services.Configure<RatingSystemConfiguration>(
    builder.Configuration.GetSection("RatingSystemConfiguration"));

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();