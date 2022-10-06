using Light.Admin.IServices;
using Light.Admin.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;
using Light.Admin.Mongo.Extensions;
using LightForApiDotNet5.Tools;
using Light.Admin.Mongo.Filters;
using Light.Admin.Models;
using AspNetCore.Identity.Mongo;
using Microsoft.Extensions.Options;
using AspNetCore.Identity.Mongo.Model;
using MongoDB.Bson;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.Configure<MongoDBSettings>(
// builder.Configuration.GetSection("SiteStoreDatabase"));

var CONNECTION_STRING = builder.Configuration.GetConnectionString("Mongo");

builder.Services.AddSingleton<IUserService, UserService>();

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

builder.Services.AddMvc(options =>
{
    options.Filters.Add<ApiResultFilterAttribute>(); // 统一返回值（包含了对422模型校验错误的处理）
    options.Filters.Add<CustomExceptionAttribute>(); // 统一异常处理
});

builder.Services.AddObjectIdBinders(); // support query can use ObjectId


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Docs", Version = "v1" });

    // 使用反射获取xml文件。并构造出文件的路径
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    c.OrderActionsBy(o => o.RelativePath); // 对action的名称进行排序
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

builder.Services.AddIdentityMongoDbProvider<User, Role, ObjectId>(identity =>
{
    // other options
    identity.Password.RequireDigit = false;
    identity.Password.RequiredLength = 6;
    identity.Password.RequireNonAlphanumeric = false;
    identity.Password.RequireUppercase = false;
    identity.Password.RequireLowercase = false;

    // Lockout settings
    identity.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    identity.Lockout.MaxFailedAccessAttempts = 10;

    // ApplicationUser settings
    identity.User.RequireUniqueEmail = true;
    identity.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@.-_";
},
   mongo =>
   {
       mongo.ConnectionString = CONNECTION_STRING;
       // other options
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
