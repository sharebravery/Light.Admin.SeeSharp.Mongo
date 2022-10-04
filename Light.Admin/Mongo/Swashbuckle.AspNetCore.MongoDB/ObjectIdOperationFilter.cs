using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Swashbuckle.AspNetCore.SwaggerGen
{
    public class ObjectIdOperationFilter : IOperationFilter
    {
        //prop names we want to ignore
        private readonly HashSet<string> objectIdIgnoreParameters = new HashSet<string>()
        {
            nameof(ObjectId.Timestamp),
#pragma warning disable CS0618 // 类型或成员已过时
            nameof(ObjectId.Machine),
            nameof(ObjectId.Pid),
            nameof(ObjectId.Increment),
#pragma warning restore CS0618 // 类型或成员已过时
            nameof(ObjectId.CreationTime),
        };

        private readonly IEnumerable<XPathNavigator> xmlNavigators;

        public ObjectIdOperationFilter(IEnumerable<string> filePaths)
        {
            xmlNavigators = filePaths != null
                ? filePaths.Select(x => new XPathDocument(x).CreateNavigator())
                : Array.Empty<XPathNavigator>();
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var objectIdParams = context.ApiDescription.ActionDescriptor.Parameters
                .Where(p => (p.ParameterType == typeof(ObjectId) || p.ParameterType == typeof(ObjectId?))
                    && (p.BindingInfo.BindingSource == BindingSource.Query || p.BindingInfo.BindingSource == BindingSource.Path)
                    )
                .ToArray();

            if (objectIdParams.Length > 0)
            {
                var parameters = operation.Parameters;
                for (int i = parameters.Count - 1; i >= 0; i--)
                {
                    if (objectIdIgnoreParameters.Contains(parameters[i].Name))
                    {
                        parameters.RemoveAt(i);
                    }
                }
                foreach (var parameter in objectIdParams)
                {
                    operation.Parameters.Add(new OpenApiParameter()
                    {
                        Name = parameter.Name,
                        Schema = new OpenApiSchema()
                        {
                            Type = "string",
                            Format = "24-digit hex string"
                        },
                        Description = GetFieldDescription(parameter.Name, context),
                        Example = new OpenApiString(ObjectId.Empty.ToString()),
                        In = MapParameterLocation(parameter.BindingInfo.BindingSource)
                    });
                }
            }
        }

        static ParameterLocation? MapParameterLocation(BindingSource? bindingSource)
        {
            if (bindingSource == BindingSource.Query)
            {
                return ParameterLocation.Query;
            }
            else if (bindingSource == BindingSource.Path)
            {
                return ParameterLocation.Path;
            }
            return null;
        }

        //get description from XML
        private string GetFieldDescription(string idName, OperationFilterContext context)
        {
            var name = char.ToUpperInvariant(idName[0]) + idName.Substring(1);
            var classProp = context.MethodInfo.GetParameters().FirstOrDefault()?.ParameterType?.GetProperties().FirstOrDefault(x => x.Name == name);
            var typeAttr = classProp != null
                ? classProp.GetCustomAttribute<DescriptionAttribute>()
                : null;
            if (typeAttr != null)
                return typeAttr?.Description;

            if (classProp != null)
                foreach (var xmlNavigator in xmlNavigators)
                {
                    var propertyMemberName = XmlCommentsNodeNameHelper.GetMemberNameForFieldOrProperty(classProp);
                    var propertySummaryNode = xmlNavigator.SelectSingleNode($"/doc/members/member[@name='{propertyMemberName}']/summary");
                    if (propertySummaryNode != null)
                        return XmlCommentsTextHelper.Humanize(propertySummaryNode.InnerXml);
                }

            return null;
        }
    }
}

