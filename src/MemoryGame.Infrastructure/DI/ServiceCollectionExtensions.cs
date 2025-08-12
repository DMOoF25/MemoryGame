using MemoryGame.Application.Abstractions;
using MemoryGame.Application.Services;
using MemoryGame.Infrastructure.Deck;
using MemoryGame.Infrastructure.Timing;
using Microsoft.Extensions.DependencyInjection;

namespace MemoryGame.Infrastructure.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMemoryGame(this IServiceCollection services)
    {
        services.AddSingleton<IDeckProvider, EmojiDeckProvider>();
        services.AddSingleton<IAsyncDelay, AsyncDelay>();
        services.AddSingleton<ITimerService, DispatcherTimerService>();
        services.AddSingleton<IGameService, GameService>();
        return services;
    }
}
