using Microsoft.AspNetCore.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Microservice.Modo.Aceptacion.Infrastructure;

public class OperationFilter : IOperationFilter
{
    private readonly IWebHostEnvironment _env;

    public OperationFilter(IWebHostEnvironment env)
    {
        _env = env;
    }
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        //operation.Parameters.Add(new OpenApiParameter
        //{
        //    Name = HeaderNames.XNumeroTarjeta,
        //    In = ParameterLocation.Header,
        //    Description = "Número de tarjeta",
        //    Required = true,
        //    Schema = new OpenApiSchema
        //    {
        //        Type = "string",
        //        Default = new OpenApiString(_env.IsDevelopment() ? "5046200718000000" : "")
        //    }
        //});
        //operation.Parameters.Add(new OpenApiParameter
        //{
        //    Name = HeaderNames.XIdRequerimiento,
        //    In = ParameterLocation.Header,
        //    Description = "Id Requerimiento",
        //    Required = true,
        //    Schema = new OpenApiSchema
        //    {
        //        Type = "string",
        //        Default = new OpenApiString(Guid.NewGuid().ToString())
        //    }
        //}); 
        //operation.Parameters.Add(new OpenApiParameter
        //{
        //    Name = HeaderNames.XCuitEmpresa,
        //    In = ParameterLocation.Header,
        //    Description = "CUIT Empresa",
        //    Required = true,
        //    Schema = new OpenApiSchema
        //    {
        //        Type = "string",
        //        Default = new OpenApiString(_env.IsDevelopment() ? "30179326597" : "")
        //    }
        //});
        //operation.Parameters.Add(new OpenApiParameter
        //{
        //    Name = HeaderNames.XCuitUsuario,
        //    In = ParameterLocation.Header,
        //    Description = "CUIT Usuario",
        //    Required = true,
        //    Schema = new OpenApiSchema
        //    {
        //        Type = "string",
        //        Default = new OpenApiString(_env.IsDevelopment() ? "20179326591" : "")
        //    }
        //});
    }
}