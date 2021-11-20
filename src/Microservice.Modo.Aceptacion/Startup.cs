using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using HealthChecks.UI.Client;
using Microservice.Modo.Aceptacion.Business.Profiles;
using Microservice.Modo.Aceptacion.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modo.Clients;
using WyD.Mess;
using WyD.Mess.Discovery.Consul;
using WyD.Mess.Docs.Swagger;
using WyD.Mess.Hosting.WindowsServices;
using WyD.Mess.Http;
using WyD.Mess.LoadBalancing.Fabio;
using WyD.Mess.Tracing.Jaeger;
using WyD.Mess.WebApi;
using WyD.Mess.WebApi.Swagger;

#pragma warning disable 1591

namespace Microservice.Modo.Aceptacion;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        var builder = services.AddMess(Configuration)
            //.AddErrorHandler<ExceptionToResponseMapper>()
            .AddWebApi()
            .AddService<Program.Worker>()
            .AddHttpClient()
            .AddConsul()
            .AddFabio()
            .AddJaeger()
            //.AddSwaggerDocs() //este lo agrega el de abajo
            .AddWebApiSwaggerDocs(setupAction: genOptions =>
            {
                genOptions.OperationFilter<OperationFilter>();
                genOptions.SchemaFilter<SchemaFilter>();
            });

        builder.Services.AddScoped<GenericActionFilter>();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddHttpClient<IMerchantClient, MerchantClient>(client =>
        {
            var baseUri = Configuration["ModoClientOptions:Uri"];
            var clientId = Configuration["ModoClientOptions:ClientId"];
            var secret = Configuration["ModoClientOptions:Secret"];

            //Basic Authentication
            var authenticationString = $"{clientId}:{secret}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));

            client.BaseAddress = new Uri(baseUri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(nameof(AuthenticationSchemes.Basic), base64EncodedAuthenticationString);
        });

        //builder.Services.AddScoped<IAfipAccesoService, AfipAccesoService>();
        //builder.Services.AddScoped<IParametrosEntradaHeaderReader, ParametrosEntradaHeaderReader>();
        //builder.Services.AddScoped<IIdRequerimientoGenerator, IdRequerimientoGenerator>();
        //builder.Services.AddScoped<IIpClienteReader, IpClienteReader>();
        builder.Services.AddAutoMapper(expression =>
        {
            expression.AddMemberConfiguration().AddName<CaseInsensitiveName>();

        }, typeof(Program).Assembly, typeof(DefaultProfile).Assembly);

        builder.Services.AddOpenTracing();

        //builder.Services.AddAfip(Configuration);
        //builder.Services.AddAfipBusiness();

        var checkBuilder = builder.Services.AddHealthChecks();

        //var optionsAfipPagos = new AfipPagosClientOptions();
        //Configuration.GetSection(nameof(AfipPagosClientOptions)).Bind(optionsAfipPagos);

        //if (optionsAfipPagos.Health != null)
        //    checkBuilder.AddUrlGroup(optionsAfipPagos.Health, nameof(AfipPagosWrapper), 
        //        configureHttpMessageHandler: provider => new HttpClientHandler
        //        {
        //            UseProxy = false,
        //            ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true

        //        },
        //        configureClient: (provider, client) =>
        //        {
        //            client.DefaultRequestHeaders.Add(HeaderNames.XIbmClientId, optionsAfipPagos.ClientId);
        //            client.DefaultRequestHeaders.Add(HeaderNames.XIbmClientSecret, optionsAfipPagos.Secret);
        //        });

        //var optionsAfipAcceso = new AfipAccesoClientOptions();
        //Configuration.GetSection(nameof(AfipAccesoClientOptions)).Bind(optionsAfipAcceso);

        //if (optionsAfipAcceso.Health != null)
        //    checkBuilder.AddUrlGroup(optionsAfipAcceso.Health, nameof(AfipAccesoWrapper),
        //        configureHttpMessageHandler: provider => new HttpClientHandler
        //        {
        //            UseProxy = false,
        //            ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true

        //        },
        //        configureClient: (provider, client) =>
        //        {
        //            client.DefaultRequestHeaders.Add(HeaderNames.XIbmClientId, optionsAfipAcceso.ClientId);
        //            client.DefaultRequestHeaders.Add(HeaderNames.XIbmClientSecret, optionsAfipAcceso.Secret);
        //        });

        builder.Build();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseExceptionHandler(c => c.Run(async context =>
        {
            var exception = context.Features
                .Get<IExceptionHandlerPathFeature>()
                .Error;
            var response = new { error = exception.Message };
            await context.Response.WriteJsonAsync(response);
        }));

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        var fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
        var title = $"{fileVersion.ProductVersion}/{fileVersion.FileVersion}";

        app.UseMess()
            //.UseErrorHandler()
            .UseJaeger()
            .UseWebApi()
            .UseRouting()
            .UseEndpoints(r =>
            {
                r.MapControllers();

                r.MapHealthChecks("/health", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                r.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = l => l.Name.Contains("self")
                });

                r.MapGet("", ctx => ctx.Response.WriteAsync("Modo Aceptacion API"));
                r.MapGet("/version", async context => await context.Response.WriteAsync(title));
            })
            .UseSwaggerDocs();

    }
}

public class GenericActionFilter : ActionFilterAttribute
{
    private readonly ILogger<GenericActionFilter> _logger;

    public GenericActionFilter(ILogger<GenericActionFilter> logger)
    {
        _logger = logger;
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        Log("OnActionExecuting ", filterContext.RouteData);
    }

    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
        if (filterContext.Exception != null)
        {
            var ex = filterContext.Exception;
            _logger.LogError(ex.Message, ex);
        }
        else
        {
            Log("OnActionExecuted", filterContext.RouteData);
        }
    }

    public override void OnResultExecuting(ResultExecutingContext filterContext)
    {
        Log("OnResultExecuting", filterContext.RouteData);
    }

    public override void OnResultExecuted(ResultExecutedContext filterContext)
    {
        Log("OnResultExecuted", filterContext.RouteData);
    }


    private void Log(string methodName, RouteData routeData)
    {
        var controllerName = routeData.Values["controller"];
        var actionName = routeData.Values["action"];
        var message = $"{methodName} controller:{controllerName} action:{actionName}";
        _logger.LogDebug(message, "Action Filter Log");
    }
}