using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WallpaperBot.Wallpapers.Clients.Unsplash.Contracts;
using WallpaperBot.Wallpapers.Models.Clients.Parameters.Unsplash;
using WallpaperBot.Wallpapers.Models.Clients.Responses.Unsplash;

namespace WallpaperBot.Wallpapers.Services
{
    internal interface IUnsplashService
    {
        Task<List<PhotosRandomResponse>?> GetPopularPhotos();
    }
    internal class UnsplashService : IUnsplashService
    {
        private readonly IUnsplashPublicClient _client;
        public UnsplashService(IUnsplashPublicClient client)
        {
            _client = client;
        }
        //should use a new model instead of PhotosRandomResponse
        public async Task<List<PhotosRandomResponse>?> GetPopularPhotos()
        {
            try
            {
                var popularPhotos = await _client.GetPhotos(OrderBy.Popular);
                var photoDownloadUrl = await _client.GetPhotoDownloadUrl(popularPhotos[0].id);
                return popularPhotos;
            }
            catch (Exception ex)
            {

                return null;

            }
        }
    }
}
