using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WallpaperBot.Infrastructure.Services.Contracts;
using WallpaperBot.Wallpapers.Services;
using WallpaperBot.Wallpapers.Services.Contracts;

namespace WallpaperBot.Infrastructure.Services
{

    internal class ImageDownloadService : IImageDownloadService, IGuidable
    {
        private readonly IUnsplashService _unsplashService;
        private readonly IFileWriter _fileWriter;
        public ImageDownloadService(IUnsplashService unsplashService, IFileWriter fileWriter) 
        {
            _unsplashService = unsplashService;
            _fileWriter = fileWriter;
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
                string path = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\WallpaperBot.Shared\Storage\Wallpapers");

                string imageId = GenerateId();
                path += $@"\image-{imageId}.jpeg";
                await _fileWriter.WriteBytesToFile(path, fileBytes);
            }
        }
        public string GenerateId()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString("N");
        }
    }
}
