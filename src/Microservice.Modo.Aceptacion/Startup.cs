using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using HealthChecks.UI.Client;
using Microservice.Modo.Aceptacion.Business;
using Microservice.Modo.Aceptacion.Business.Profiles;
using Microservice.Modo.Aceptacion.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modo.Clients;
using Modo.Clients.Interfaces;
using WyD.Mess;
using WyD.Mess.Docs.Swagger;
using WyD.Mess.Hosting.WindowsServices;
using WyD.Mess.Http;
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
            //.AddHttpClient()
            //.AddConsul()
            //.AddFabio()
            //.AddJaeger()
            //.AddSwaggerDocs() //este lo agrega el de abajo
            .AddWebApiSwaggerDocs(setupAction: genOptions =>
            {
                genOptions.OperationFilter<OperationFilter>();
                genOptions.SchemaFilter<SchemaFilter>();
            });

        builder.Services.AddControllers()
            .AddJsonOptions(configure =>
            {
                configure.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        builder.Services.AddScoped<GenericActionFilter>();
        builder.Services.AddScoped<MessageLoggingHandler>();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        builder.Services.AddHttpClient<IMerchantClient, MerchantClient>(ConfigureClient)
            .AddHttpMessageHandler<MessageLoggingHandler>();

        builder.Services.AddHttpClient<IQrClient, QrClient>(ConfigureClient)
            .AddHttpMessageHandler<MessageLoggingHandler>();

        builder.Services.AddScoped<IModoService, ModoService>();

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

    private void ConfigureClient(HttpClient client)
    {
        var baseUri = Configuration["ModoClientOptions:Uri"];
        var clientId = Configuration["ModoClientOptions:ClientId"];
        var secret = Configuration["ModoClientOptions:Secret"];

        //Basic Authentication
        var authenticationString = $"{clientId}:{secret}";
        var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));

        client.BaseAddress = new Uri(baseUri);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(nameof(AuthenticationSchemes.Basic), base64EncodedAuthenticationString);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseExceptionHandler("/error");

        var fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
        var title = $"{fileVersion.ProductVersion}/{fileVersion.FileVersion}";

        app.UseMess()
            //.UseErrorHandler()
            //.UseJaeger()
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