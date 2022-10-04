using Light.Admin.Database;
using Light.Admin.IServices;
using Light.Admin.Services;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Light.Admin.Mongo.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<MongoDBSettings>(
 builder.Configuration.GetSection("SiteStoreDatabase"));

builder.Services.AddSingleton<IMongoDBContext, MongoDBContext>();
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


//builder.Services.AddTransient(sp => sp.GetService<IOptions<JsonOptions>>()!.Value.JsonSerializerOptions); // X

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
