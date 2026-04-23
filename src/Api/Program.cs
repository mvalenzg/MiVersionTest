using System.Net;
using System.Reflection;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Mi Api Test")
        .WithTheme(ScalarTheme.Moon)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();

app.MapPost("/api/users", async (string name) =>
{
    await Task.Delay(1500);
    var id = Random.Shared.Next(1, 100);
    return Results.Ok(new
    {
        Id = id,
        Name = name,
        email = $"{name}_{1}@company.com"
    });
}).WithTags("Users")
.WithName("CreateUser");

app.MapGet("/version", () =>
{
    var assembly = Assembly.GetExecutingAssembly();
    var infoVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
    .InformationalVersion;

    var fileVersion = assembly.GetName().Version?.ToString();

    return Results.Ok(new
    {
        Version = infoVersion ?? fileVersion,
        Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
        RunTime = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
        ServerTime = DateTime.UtcNow
    });
}).WithName("Version")
.WithTags("Version");

app.Run();
