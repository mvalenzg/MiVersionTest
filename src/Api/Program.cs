using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

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
