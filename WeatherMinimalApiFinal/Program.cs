using Microsoft.EntityFrameworkCore;
using WeatherMinimalApiFinal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();// built in healthcheck service for .net

builder.Services.AddDbContext<CityContext>(c => c.UseInMemoryDatabase("City")); //here we have used memory database

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

int apiCall = 0;// API call counter and make it as a global variable

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.MapHealthChecks("/-helath");

app.UseCors();

app.MapGet("/weather/stockholm", () =>
{
    var weather = new WeatherData
    {
        Temparature = "12°C",  //(alt+0176)
        Humidity = "30%",
        Wind = "100km/h"
    };
    return weather;
});

app.MapPost("/favorite-city", async (City city, CityContext db) =>
{
    db.Cities.Add(city);
    await db.SaveChangesAsync();
    return Results.Created($"/save/ {city.CityName}", city);
});
// while i am using dbcontext so that i have to add this service also to the container(line 12).

app.MapGet("/favorite-cities", async (CityContext db) =>
{
    return await db.Cities.ToListAsync();
});

app.MapGet("/health", () =>
{
    return ($"API is healthy {StatusCodes.Status200OK}");
});

app.MapGet("/API/call/statistics", () =>
{
    Interlocked.Increment(ref apiCall); //The Interlocked.Increment method is a thread-safe way to increment an integer value without the risk of multiple threads accessing and modifying the value simultaneously.
    return $"Number of API calls: {apiCall}";
});


app.Run();

