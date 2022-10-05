using Light.Admin.Database;
using Light.Admin.IServices;
using Light.Admin.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;
using Light.Admin.Mongo.Extensions;
using Microsoft.AspNetCore.Builder;
using LightForApiDotNet5.Tools;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Light.Admin.Mongo.Filters;

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
//.AddXmlDataContractSerializerFormatters()
//.ConfigureApiBehaviorOptions(setupAction =>
//{
//    setupAction.InvalidModelStateResponseFactory = context =>
//    {
//        // 422 验证错误统一处理(模型校验错误)
//        var problemDetail = new ValidationProblemDetails(context.ModelState)
//        {
//            Type = "无所谓",
//            Title = "数据验证失败",
//            Status = StatusCodes.Status422UnprocessableEntity,
//            Detail = "请看详细说明",
//            Instance = context.HttpContext.Request.Path
//        };
//        problemDetail.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
//        return new UnprocessableEntityObjectResult(problemDetail)
//        {
//            ContentTypes = { "application/problem+json" }
//        };
//    };
//});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddMvc(options =>
{
    //options.Filters.Add<ValidateModelAttribute>();
    options.Filters.Add<ApiResultFilterAttribute>(); // 统一返回值
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
