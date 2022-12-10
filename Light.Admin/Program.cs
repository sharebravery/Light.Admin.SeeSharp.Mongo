using Light.Admin.IServices;
using Light.Admin.Services;

//using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;
using Light.Admin.Mongo.Extensions;
using LightForApiDotNet5.Tools;
using Light.Admin.Mongo.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Light.Admin.Mongo.Basics;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc.Authorization;
using Light.Admin.Database;
using Light.Admin.Mongo.Services;
using Light.Admin.Mongo.IServices;

var builder = WebApplication.CreateBuilder(args).Inject(); ;

// Add services to the container.
builder.Services.Configure<DefaultDbSettings>(
 builder.Configuration.GetSection("SiteStoreDatabase"));

// jwt 认证 配置
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
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
});

builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IAccountService, AccountService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers().AddInject()
.AddJsonOptions(
    options =>
    {
        //序列化时，忽略值为null的属性或字段
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddObjectIdBinders(); // support query can use ObjectId

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddObjectIdSwagger();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyMethod()
            .AllowAnyOrigin()
            .AllowAnyHeader();
    });
});

builder.Services.AddRemoteRequest();

builder.Services.AddMvc(options =>
{
    options.Filters.Add<ApiResultFilterAttribute>(); // 统一返回值（包含了对422模型校验错误的处理）
    options.Filters.Add<CustomExceptionAttribute>(); // 统一异常处理
    options.Filters.Add(new AuthorizeFilter());
});

var app = builder.Build();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseInject();

app.MapControllers();

app.Run();