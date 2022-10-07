using Light.Test.Models.Basics;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// jwt 认证
JwtSettings jwtSettings = new JwtSettings();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings);

//注册服务
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true, //是否验证Issuer
        ValidIssuer = jwtSettings.Issuer, //发行人Issuer
        ValidateAudience = true, //是否验证Audience
        ValidAudience = jwtSettings.Audience, //订阅人Audience
        ValidateIssuerSigningKey = true, //是否验证SecurityKey
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)), //SecurityKey
        ValidateLifetime = true, //是否验证失效时间
        ClockSkew = TimeSpan.FromSeconds(30), //过期时间容错值，解决服务器端时间不同步问题（秒）
        RequireExpirationTime = true,
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); // 1、使用身份验证	

app.UseAuthorization();

app.MapControllers();

app.Run();
