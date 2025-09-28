using WeatherAPI.Services;
using WeatherAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var apiEndpoint = builder.Configuration["ApiEndpoint"];
var apiKey = builder.Configuration["ApiKey"];


builder.Services.AddHttpClient<IWeatherService, WeatherService>();
builder.Services.AddSingleton(new WeatherConfig { ApiKey = apiKey, ApiEndpoint = apiEndpoint});
builder.Services.AddSingleton<ICountryRepository, InMemoryCountryRepository>();
builder.Services.AddScoped<ICountriesService, CountriesService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // asal Vue
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.MapControllers();
app.UseHttpsRedirection();
app.Run();

public class WeatherConfig
{
    public string ApiKey { get; set; }
    public string ApiEndpoint { get;set; }
}