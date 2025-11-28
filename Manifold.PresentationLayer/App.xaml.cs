using Manifold.ApplicationLayer.Services;
using Manifold.InfrastructureLayer;
using Manifold.PresentationLayer.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;

namespace Manifold.PresentationLayer;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();

        // Infrastructure: provide GraphBuilder with path to graph.json
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string jsonPath = Path.Combine(baseDir, "graph.json");

        // If graph.json not present, try executing dir (when running inside IDE location may differ)
        if (!File.Exists(jsonPath))
        {
            // try relative to project folder
            var tryPath = Path.Combine(AppContext.BaseDirectory, "graph.json");
            if (File.Exists(tryPath)) jsonPath = tryPath;
        }

        services.AddSingleton<IGraphBuilder>(_ => new GraphBuilder(jsonPath));

        // Register routing / flow / manifold services from application layer.
        services.AddSingleton<IRoutingService, RoutingService>();
        services.AddSingleton<IFlowService, FlowService>();
        services.AddSingleton<IManifoldService, ManifoldService>();

        // Graph itself: build once
        services.AddSingleton(sp => {
            var builder = sp.GetRequiredService<IGraphBuilder>();
            return builder.BuildGraph();
        });

        // Presentation registrations
        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<MainWindow>();

        Services = services.BuildServiceProvider();

        var mw = new MainWindow()
        {
            DataContext = Services.GetRequiredService<MainWindowViewModel>()
        };

        mw.Show();
    }
}

