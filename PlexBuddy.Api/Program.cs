using Microsoft.EntityFrameworkCore;
using PlexBuddy.Api.Data;
using PlexBuddy.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=PlexBuddy.db"));
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<PlexAuthService>();
builder.Services.AddScoped<TmdbService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "PlexBuddy API v1");
    options.RoutePrefix = "swagger"; 
});

builder.Configuration.AddUserSecrets<Program>();

app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllers();
app.Run();