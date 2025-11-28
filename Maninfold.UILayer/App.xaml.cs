using Manifold.ApplicationLayer;
using Manifold.ApplicationLayer.Services;
using Manifold.InfrastructureLayer;
using Maninfold.UILayer.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace Maninfold.UILayer;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public ServiceProvider? serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();

        // --- Infrastructure: GraphBuilder (graph.json path relative to exe)
        string graphPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "graph.json");
        services.AddSingleton<IGraphBuilder>(sp => new GraphBuilder(graphPath));

        // --- Application services
        services.AddSingleton<PathFinder>();
        services.AddSingleton<RoutingService>();
        services.AddSingleton<FlowService>();
        services.AddSingleton<IManifoldService, ManifoldService>();

        // --- ViewModels and MainWindow
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<MainWindow>();

        serviceProvider = services.BuildServiceProvider();

        var main = serviceProvider.GetRequiredService<MainWindow>();
        main.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        serviceProvider?.Dispose();
        base.OnExit(e);
    }
}