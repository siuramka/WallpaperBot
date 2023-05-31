using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WallpaperBot.Wallpapers.Services;
using WallpaperBot.Wallpapers.Services.Contracts;

namespace WallpaperBot.Infrastructure.Services
{
    internal interface IImageDownloadService
    {
        Task DownloadImagesAsync();
    }
    internal class ImageDownloadService : IImageDownloadService
    {
        private readonly IUnsplashService _unsplashService;
        public ImageDownloadService(IUnsplashService unsplashService) 
        {
            _unsplashService = unsplashService;
        }

        async Task IImageDownloadService.DownloadImagesAsync()
        {
            var co = await _unsplashService.GetPopularPhotos();
            var xo = 0;
        }
    }
}
