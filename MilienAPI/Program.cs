using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MilienAPI.DataBase;
using MilienAPI.UnitOfWork.Interfaces;
using MilienAPI.UnitOfWork;
using MilienAPI.Services.Interfaces;
using MilienAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var databaseConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<Context>(option => option.UseNpgsql(databaseConnection));

var secretKey = builder.Configuration.GetSection("JwtSettings:SecretKey").Value;
var issuer = builder.Configuration.GetSection("JwtSettings:Issuer").Value;
var audience = builder.Configuration.GetSection("JwtSettings:Audience").Value;

var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

builder.Services.AddMemoryCache();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            IssuerSigningKey = signingKey,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddCors(policy => policy.AddPolicy("default", opt =>
{
    opt.WithOrigins("localhost:3000", "https://localhost:3000", "http://localhost:3000",
                    "xn--h1agbg8e4a.xn--p1ai", "https://xn--h1agbg8e4a.xn--p1ai",
                                               "http://xn--h1agbg8e4a.xn--p1ai");
    opt.WithExposedHeaders("count");
    opt.AllowAnyHeader();
    opt.AllowAnyMethod();
    opt.AllowCredentials();
}));

builder.Services.AddScoped<IAddRepository, AdRepository>();
builder.Services.AddScoped<IAdService, AdService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
