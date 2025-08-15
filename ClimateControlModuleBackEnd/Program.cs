using BI.Settings;
using ClimateControlModuleBackEnd.AppStart;
using ClimateControlModuleBackEnd.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<BOMApiSettings>(builder.Configuration.GetSection("BOMApiSettings"));

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
