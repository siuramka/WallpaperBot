using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WallpaperBot.Wallpapers.Clients.Unsplash.Contracts;
using WallpaperBot.Shared.Models.Config;
using WallpaperBot.Wallpapers.Services;
using WallpaperBot.Wallpapers.Clients.Unsplash;
using WallpaperBot.Wallpapers.Services.Contracts;
using WallpaperBot.Infrastructure.Services;

namespace WallpaperBot
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
                .AddSingleton<IUnsplashPublicClient>(
                        new UnsplashPublicClient(unsplashSettings.AccessKey, unsplashSettings.SecretKey)
                    )
                .AddSingleton<IUnsplashService, UnsplashService>()
                .AddSingleton<IImageDownloadService, ImageDownloadService>()
            .BuildServiceProvider();

            var test = serviceProvider.GetService<IImageDownloadService>();
            await test.DownloadImagesAsync();
            var okk = 123;
        }
    }
}