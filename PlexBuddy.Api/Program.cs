using Microsoft.EntityFrameworkCore;
using PlexWishlist.Api.Data;
using PlexWishlist.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Add DB Context (Using In-Memory for dev speed, switch to SQL Server easily later)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("PlexWishlistDb")); 
// For SQL Server use: options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))

// 2. Register Plex Service
builder.Services.AddHttpClient<PlexAuthService>();

// 3. Add CORS (Critical for React/Blazor to talk to .NET)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();