using System.IO;
using System.Windows;
using MemoryGame.Application.Abstractions;
using MemoryGame.Application.Services;
using MemoryGame.Infrastructure.Decks;
using MemoryGame.Infrastructure.Stats;
using MemoryGame.UI.ViewModels;

namespace MemoryGame.UI;

public partial class App : System.Windows.Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        IDeckProvider deck = new EmojiDeckProvider();
        var statsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MemoryGame", "stats.json");
        IStatsRepository statsRepo = new FileStatsRepository(statsPath);

        IGameService game = new GameService(deck, statsRepo);
        var vm = new GameViewModel(game);

        var window = new MainWindow { DataContext = vm };
        window.Show();
    }
}