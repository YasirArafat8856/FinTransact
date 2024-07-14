using AccountService;
using AccountService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
// Add services to the container.
var jwtSettings = new JwtSettings();
configuration.Bind(nameof(JwtSettings), jwtSettings);
builder.Services.AddSingleton(jwtSettings);


builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration.GetSection("Redis")["ConnectionString"]));
builder.Services.AddSingleton<RedisCacheService>();
builder.Services.AddScoped<AccountService.Services.AccountService>();
builder.Services.AddScoped<RabbitMQPublisher>();
builder.Services.AddTransient<TokenService>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
    };
});
builder.Services.AddDbContext<AccountDbContext>(options =>
       options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
});


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
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FinTransact API V1");
});

app.Run();
