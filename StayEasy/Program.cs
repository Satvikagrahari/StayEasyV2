using Microsoft.EntityFrameworkCore;
using StayEasy.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<StayEasyDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
