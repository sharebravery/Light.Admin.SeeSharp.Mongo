using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swashbuckle.AspNetCore.SwaggerGen
{
    public class ObjectIdSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(ObjectId))
            {
                schema.Type = "string";
                schema.Format = "24-digit hex string";
                schema.Example = new OpenApiString(ObjectId.Empty.ToString());
            }
            if (context.Type == typeof(ObjectId?))
            {
                schema.Type = "string";
                schema.Format = "24-digit hex string";
            }
        }
    }
}
