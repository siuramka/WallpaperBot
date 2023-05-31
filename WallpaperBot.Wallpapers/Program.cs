using Microsoft.Extensions.Configuration;
using WallpaperBot.Wallpapers;
using WallpaperBot.Wallpapers.Models;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using WallpaperBot.Wallpapers.Clients.Unsplash;
using WallpaperBot.Wallpapers.Clients.Unsplash.Contracts;
using WallpaperBot.Wallpapers.Services;

namespace WallpaperBot.Wallpapers
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                      .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Configs"))
                      .AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();

            var unsplashSettings = config.GetSection("UnsplashSettings").Get<UnsplashSettings>();

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IUnsplashService>()
                .AddSingleton<IUnsplashPublicClient>(
                        new UnsplashPublicClient(unsplashSettings.AccessKey, unsplashSettings.SecretKey)
                    )
            .BuildServiceProvider();

        }
    }
}
