using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui;
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
        services.AddTransient<MainWindow>();
        services.AddTransient<LoginDialog>();
        services.AddTransient<PingView>();
        services.AddTransient<DebugView>();
        services.AddSingleton<INakamaClient, NakamaClient>();
        return services.BuildServiceProvider();
    }
}