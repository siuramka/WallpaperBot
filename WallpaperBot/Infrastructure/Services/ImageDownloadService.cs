using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
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
        public async Task DownloadImagesAsync()
        {
            RestClient restClient = new RestClient();
            var imageUrls = await _unsplashService.GetPopularPhotos();
            //

            foreach (var imageDownload in imageUrls)
            {
                var request = new RestRequest(imageDownload.url, Method.Get);
                var fileBytes = await restClient.DownloadDataAsync(request);
                File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\WallpaperBot.Shared\Storage\Wallpapers") + "image.jpg", fileBytes);
            }

        }
    }
}
