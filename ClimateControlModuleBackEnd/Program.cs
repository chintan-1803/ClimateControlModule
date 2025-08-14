using BI;
using ClimateControlModuleBackEnd.AppStart;
using ClimateControlModuleBackEnd.Middleware;
using Contracts.Managers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpClient<IWeatherObservationManager, WeatherObservationManager>(client =>
{
    client.BaseAddress = new Uri("https://www.bom.gov.au/"); // Always HTTPS
    client.DefaultRequestHeaders.UserAgent.ParseAdd(
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
        "(KHTML, like Gecko) Chrome/115.0 Safari/537.36");
});

// Chintan: Separate class to configure application services to keep Program.cs clean and maintainable
ApplicationServicesConfiguration.ConfigureManagers(builder.Services);

// Chintan: Authentication middleware should be inserted first to have user's Identity which is required for Authorisation middleware 
// Chintan: Can add services and middleware of Authentication in future if needed // Chintan: Can add services and middleware of Authorisation in future if needed
// Chintan: Can add services of CORS in future if needed to make the API accessible from other origins and make it more secure
// Chintan: Can add services of rate limiting too to control the number of requests 

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Chintan: Customer middleware to handle exceptions globally
app.UseMiddleware<ExceptionMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
