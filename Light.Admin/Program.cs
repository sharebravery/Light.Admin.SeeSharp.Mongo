using Light.Admin.IServices;
using Light.Admin.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;
using Light.Admin.Mongo.Extensions;
using LightForApiDotNet5.Tools;
using Light.Admin.Mongo.Filters;
using Light.Admin.Mongo;
using MongoDB.Bson;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Light.Admin.Mongo.Basics;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;
using Microsoft.IdentityModel.Logging;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Light.Admin.Database;
using Light.Admin.Mongo.Services;
using Light.Admin.Mongo.IServices;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<DefaultDbSettings>(
 builder.Configuration.GetSection("SiteStoreDatabase"));

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
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
});




builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IAccountService, AccountService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers()
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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Docs", Version = "v1" });

    // 使用反射获取xml文件。并构造出文件的路径
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    c.OrderActionsBy(o => o.RelativePath); // 对action的名称进行排序



    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT授权(数据将在请求头中进行传输) 在下方输入Bearer {token} 即可，注意两者之间有空格",
        Name = "Authorization",//jwt默认的参数名称
        In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
        Type = SecuritySchemeType.ApiKey
    });
    //认证方式，此方式为全局添加
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    { new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference()
                    {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                    }
                    }, Array.Empty<string>() }
                    });
});

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



builder.Services.AddMvc(options =>
{
    options.Filters.Add<ApiResultFilterAttribute>(); // 统一返回值（包含了对422模型校验错误的处理）
    options.Filters.Add<CustomExceptionAttribute>(); // 统一异常处理
    options.Filters.Add(new AuthorizeFilter());
});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
