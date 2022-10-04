using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System.Text.Json.Serialization;

namespace Light.Admin.Mongo.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddObjectIdBinders(this IServiceCollection services)
        {
            services.Configure<JsonOptions>(options =>
            {
                options.JsonSerializerOptions.Converters.Insert(0, new ObjectIdConverter());
            });

            services.Configure<MvcOptions>(options =>
            {
                options.ModelBinderProviders.Insert(0, new ObjectIdBinderProvider());
                options.Conventions.Add(new ObjectIdParameterModelConvention());
                options.ModelMetadataDetailsProviders.Add(new ObjectIdBindingMetadataProvider());
            });

            return services;
        }
    }
}
