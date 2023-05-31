using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WallpaperBot.Wallpapers.Clients.Unsplash.Contracts;
using WallpaperBot.Shared.Models.Config;
using WallpaperBot.Wallpapers.Services;
using WallpaperBot.Wallpapers.Clients.Unsplash;
using WallpaperBot.Wallpapers.Services.Contracts;
using WallpaperBot.Infrastructure.Services;
using WallpaperBot.Infrastructure.Services.Contracts;

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

            /// Only require singleton for now so keeping it that way.
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IUnsplashPublicClient>(
                        new UnsplashPublicClient(unsplashSettings.AccessKey, unsplashSettings.SecretKey)
                    )
                .AddSingleton<IUnsplashService, UnsplashService>()
                .AddSingleton<IImageDownloadService, ImageDownloadService>()
                .AddTransient<IFileWriter, FileWriter>()
            .BuildServiceProvider();

            var test = serviceProvider.GetService<IImageDownloadService>();
            await test.DownloadImagesAsync();
            var okk = 123;
        }
    }
}