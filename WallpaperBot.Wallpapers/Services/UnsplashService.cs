using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using WallpaperBot.Wallpapers.Clients.Unsplash.Contracts;
using WallpaperBot.Wallpapers.Models.Clients.Parameters.Unsplash;
using WallpaperBot.Wallpapers.Models.Clients.Responses.Unsplash;
using WallpaperBot.Wallpapers.Services.Contracts;

namespace WallpaperBot.Wallpapers.Services
{

    public class UnsplashService : IUnsplashService
    {
        private readonly IUnsplashPublicClient _client;
        public UnsplashService(IUnsplashPublicClient client)
        {
            _client = client;
        }
        //should use a new model instead of PhotosRandomResponse
        public async Task<List<PhotoDownloadResponse>?> GetPopularPhotos()
        {
            try
            {
                var popularPhotos = await _client.GetPhotos(OrderBy.Popular);

                List<PhotoDownloadResponse> result = new List<PhotoDownloadResponse>();

                await Parallel.ForEachAsync(popularPhotos, async (photo, token) =>
                {
                    var downloadUrl = await _client.GetPhotoDownloadUrl(photo.id);
                    result.Add(downloadUrl);
                });

                return result;
            }
            catch (Exception ex)
            {

                return null;

            }
        }
    }
}
