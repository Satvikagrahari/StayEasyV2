using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.Win32;
using StayEasy.Api.MIddleware;
using StayEasy.Application.Interfaces.External;
using StayEasy.Application.Interfaces.Repositories;
using StayEasy.Application.Services;
using StayEasy.Domain.Entities;
using StayEasy.Infrastructure.Persistence;
using StayEasy.Infrastructure.Repositories;
using StayEasy.Infrastructure.Security;
using StayEasy.Infrastructure.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//DbContext
builder.Services.AddDbContext<StayEasyDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

//Register Infrastructure Repositories & external services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

//Register Application Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IBookingService, BookingService>();

//Register the PaymentServiceClientRegister the PaymentServiceClient
builder.Services.AddHttpClient<IPaymentService, PaymentServiceClient>(client =>
{
    // The URL Payment Microservice is running
    client.BaseAddress = new Uri("http://localhost:5001");
});


// Configure JWT Authentication
var secretKey = builder.Configuration["JwtSettings:Secret"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

//Configure Controllers & Swagger with auth
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StayEasy API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter your JWT token below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<StayEasyDbContext>();

    var admin = context.Users.FirstOrDefault(u => u.Email == "admin@stayeasy.com");
    if (admin == null)
    {
        admin = new User
        {
            UserId = Guid.NewGuid(), 
            Email = "admin@stayeasy.com",            
            UserName = "Admin",
            Role = "Admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123")
        };
        context.Users.Add(admin);
        context.SaveChanges();
    }
}



//Global Exception Middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); // Verify Token
app.UseAuthorization();  // Check Roles
app.MapControllers();
app.Run();