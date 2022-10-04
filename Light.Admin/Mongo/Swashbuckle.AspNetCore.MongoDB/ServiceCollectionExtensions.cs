using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddObjectIdSwagger(this IServiceCollection services)
        {
            services.Configure<SwaggerGenOptions>(options =>
            {
                options.SchemaFilter<ObjectIdSchemaFilter>();
                options.OperationFilter<ObjectIdOperationFilter>();

                options.MapType<ObjectId>(() => new OpenApiSchema()
                {
                    Type = "string",
                    Description = "24-digit hex string",
                    Example = new OpenApiString(ObjectId.Empty.ToString())
                });
                options.MapType<ObjectId?>(() => new OpenApiSchema()
                {
                    Type = "string",
                    Description = "24-digit hex string",
                    Nullable = true
                });
            });

            return services;
        }
    }
}
