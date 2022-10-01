using Light.Admin.Database;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<MongoDBSettings>(
 builder.Configuration.GetSection("SiteStoreDatabase"));

builder.Services.AddSingleton<IMongoDBContext, MongoDBContext>();

builder.Services.AddControllers();
//.AddJsonOptions(
//    options =>{
//        options.JsonSerializerOptions.PropertyNamingPolicy = null;
//    });  // 更改属性名称的默认驼峰式大小写风格，以匹配 CLR 对象属性名称的 Pascal 大小写
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Docs", Version = "v1" });

    // 使用反射获取xml文件。并构造出文件的路径
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    //c.OrderActionsBy(o => o.RelativePath); // 对action的名称进行排序

    c.MapType<ObjectId>(() => new OpenApiSchema()
    {
        Type = "string"
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
