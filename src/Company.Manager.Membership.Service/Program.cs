using Company.Engine.Registration.Interface;
using Company.iFX.Configuration;
using Company.iFX.Dapr;
using Company.iFX.Grpc;
using Company.iFX.Hosting;
using Company.iFX.Logging;
using Company.iFX.Proxy;
using Company.Manager.Membership.Service;
using ProtoBuf.Grpc.Server;
using Serilog;
using System.Reflection;

string? ServiceName = Assembly.GetExecutingAssembly().GetName().Name;
string? BuildVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();

var hostBuilder = Hosting.CreateGenericBuilder(args, @"Company")
    .ConfigureServices(services =>
    {
        services.AddScoped(_ => TrackingContextDaprClient.Create<IRegistrationEngine>());
        services.AddTrackingContextGrpcInterceptor();

        services.AddCodeFirstGrpc();
        services.AddCodeFirstGrpcReflection();

        LoggerConfiguration loggerConfiguration = Logging.CreateConfiguration().WriteTo.Console();

        if (BuildVersion is not null)
        {
            loggerConfiguration.Enrich.WithProperty(nameof(BuildVersion), BuildVersion);
        }
        if (ServiceName is not null)
        {
            loggerConfiguration.Enrich.WithProperty(nameof(ServiceName), ServiceName);
        }

        if (Configuration.IsDevelopment())
        {
            loggerConfiguration.WriteTo.Seq("http://localhost:5341");
        }

        Serilog.Core.Logger logger = loggerConfiguration.CreateLogger();
        Log.Logger = logger;

        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(logger));
        services.AddSingleton<Serilog.ILogger>(logger);

        services.IncludeErrorLogging(Configuration.Setting<bool>("Zametek:ErrorLogging"));
        services.IncludePerformanceLogging(Configuration.Setting<bool>("Zametek:PerformanceLogging"));
        services.IncludeDiagnosticLogging(Configuration.Setting<bool>("Zametek:DiagnosticLogging"));
        services.IncludeInvocationLogging(Configuration.Setting<bool>("Zametek:InvocationLogging"));

    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.Configure((ctx, app) =>
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<MembershipManagerProxy>();
                endpoints.MapCodeFirstGrpcReflectionService();

                endpoints.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            });
        });
    });

await hostBuilder.RunAsync();
