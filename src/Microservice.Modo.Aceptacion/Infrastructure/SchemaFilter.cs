using System;
using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Microservice.Modo.Aceptacion.Infrastructure;

public class SchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var rnd = new Random(Guid.NewGuid().GetHashCode());

        var customAttribute = context.ParameterInfo?.GetCustomAttributes<JsonConverterAttribute>().FirstOrDefault();
        if (customAttribute != null)
        {
            if (customAttribute.ConverterType == typeof(CustomDatetimeConverter))
            {
                var o = (CustomDatetimeConverter)(customAttribute.ConverterParameters == null
                    ? Activator.CreateInstance(customAttribute.ConverterType) : Activator.CreateInstance(customAttribute.ConverterType, customAttribute.ConverterParameters));
                if (o != null)
                {
                    var format = $"{o.Format ?? ""}";
                    schema.Type = "string";
                    schema.Format = format;
                    schema.Example = new OpenApiString(DateTime.Now.ToString(format));
                }
            }
        }

        foreach (var pi in context.Type.GetProperties())
        {
            var attrPropertyName = pi.GetCustomAttributes<JsonPropertyAttribute>().FirstOrDefault();
            var pName = attrPropertyName is { PropertyName: { } } ? attrPropertyName.PropertyName : pi.Name;
            var attrConverter = pi.GetCustomAttributes<JsonConverterAttribute>().FirstOrDefault();

            if (attrConverter == null) 
                continue;

            if (attrConverter.ConverterType == typeof(CustomDatetimeConverter))
            {
                var o = (CustomDatetimeConverter)(attrConverter.ConverterParameters == null
                    ? Activator.CreateInstance(attrConverter.ConverterType) : Activator.CreateInstance(attrConverter.ConverterType, attrConverter.ConverterParameters));

                if (o != null)
                {
                    var format = $"{o.Format}";
                    schema.Properties[pName].Type = "string";
                    schema.Properties[pName].Format = format;
                    schema.Properties[pName].Example = new OpenApiString(DateTime.Now.ToString(format));
                }
            }
        }
    }
}