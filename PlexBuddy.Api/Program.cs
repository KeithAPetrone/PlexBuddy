using Microsoft.EntityFrameworkCore;
using PlexBuddy.Api.Data;
using PlexBuddy.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("PlexBuddy"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // <--- Required for Swagger
builder.Services.AddHttpClient<PlexAuthService>();
// Add this line to register your service
builder.Services.AddScoped<TmdbService>();

// Ensure you have CORS enabled so React can talk to the API
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();   // <--- Required
    app.UseSwaggerUI(); // <--- Required
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();