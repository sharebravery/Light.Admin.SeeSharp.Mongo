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

// jwt ��֤ ����
JwtSettings jwtSettings = new JwtSettings();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings);

//ע�����
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true, //�Ƿ���֤Issuer
        ValidIssuer = jwtSettings.Issuer, //������Issuer
        ValidateAudience = true, //�Ƿ���֤Audience
        ValidAudience = jwtSettings.Audience, //������Audience
        ValidateIssuerSigningKey = true, //�Ƿ���֤SecurityKey
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)), //SecurityKey
        ValidateLifetime = true, //�Ƿ���֤ʧЧʱ��
        ClockSkew = TimeSpan.FromSeconds(30), //����ʱ���ݴ�ֵ�������������ʱ�䲻ͬ�����⣨�룩
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
        //���л�ʱ������ֵΪnull�����Ի��ֶ�
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
    options.Filters.Add<ApiResultFilterAttribute>(); // ͳһ����ֵ�������˶�422ģ��У�����Ĵ���
    options.Filters.Add<CustomExceptionAttribute>(); // ͳһ�쳣����
    options.Filters.Add(new AuthorizeFilter());
});

var app = builder.Build();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseInject();

app.MapControllers();

app.Run();