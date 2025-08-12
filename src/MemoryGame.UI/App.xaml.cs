using System.Windows;
using MemoryGame.Infrastructure.DI;
using MemoryGame.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace MemoryGame.UI;

public partial class App : System.Windows.Application
{
    private ServiceProvider? _provider;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection()
            .AddMemoryGame()
            .AddSingleton<GameViewModel>()
            .AddSingleton<Views.MainWindow>();

        _provider = services.BuildServiceProvider();

        var window = _provider.GetRequiredService<Views.MainWindow>();
        window.DataContext = _provider.GetRequiredService<GameViewModel>();
        window.Show();
    }
}
