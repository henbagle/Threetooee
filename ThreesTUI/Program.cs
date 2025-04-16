using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Terminal.Gui;
using ThreesTUI.Logging;
using ThreesTUI.Server;
using ThreesTUI.Views;

namespace ThreesTUI;

public static class Program
{
    public static IServiceProvider Services { get; } = ConfigureServices();
    
    private static void Main(string[] args)
    {
        Application.Init();
        Application.Run(Services.GetRequiredService<MainWindow>());
        Application.Top?.Dispose();
        Application.Shutdown();
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        
        ConfigureLogging(services);
        
        services.AddTransient<MainWindow>();
        services.AddTransient<LoginDialog>();
        services.AddTransient<PingView>();
        services.AddTransient<DebugView>();
        services.AddSingleton<INakamaClient, NakamaClient>();
        return services.BuildServiceProvider();
    }
    
    private static void ConfigureLogging(IServiceCollection services)
    {
#if DEBUG
        var fileLogLevel = LogEventLevel.Information;
        var uiLogLevel = LogEventLevel.Warning;
#else
        var fileLogLevel = LogEventLevel.Warning;
        var uiLogLevel = LogEventLevel.Error;
#endif
        
        //TODO: Add logging display for UI

        services.AddSingleton<NakamaSerilogAdapter>();
        
        var logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ThreesTUI");
        if(!Directory.Exists(logDirectory)) Directory.CreateDirectory(logDirectory);
        var logFile = Path.Combine(logDirectory, $"threetooiee_log.txt");
        
        var serilog = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File(logFile, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: fileLogLevel)
            .CreateLogger();
        
        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog(serilog, dispose: true);
        });
    }
}