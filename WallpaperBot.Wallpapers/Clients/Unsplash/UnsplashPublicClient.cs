using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WallpaperBot.Wallpapers.Clients.Unsplash.Authenticators;
using WallpaperBot.Wallpapers.Clients.Unsplash.Contracts;
using WallpaperBot.Wallpapers.Models.Clients.Parameters.Unsplash;
using WallpaperBot.Wallpapers.Models.Clients.Responses.Unsplash;

namespace WallpaperBot.Wallpapers.Clients.Unsplash
{
    public class UnsplashPublicClient : IUnsplashPublicClient, IDisposable
    {
        private readonly RestClient _client;
        private string _baseUrl;
        public UnsplashPublicClient(string accessKey, string secretKey)
        {
            _baseUrl = "https://api.unsplash.com/";
            var options = new RestClientOptions(_baseUrl);
            options.Authenticator = new UnsplashPubicAuthenticator(_baseUrl, accessKey, secretKey);
            _client = new RestClient(options);
        }
        public async Task<List<PhotosRandomResponse>> GetPhotos(string? orderBy)
        {
            orderBy = orderBy ?? OrderBy.Latest;
            string endpoint = $"/photos?order_by={orderBy}";

            var response = await _client.GetJsonAsync<List<PhotosRandomResponse>>(endpoint);
            return response!;
        }
        public async Task<PhotoDownloadResponse> GetPhotoDownloadUrl(string photoId)
        {
            string endpoint = $"/photos/{photoId}/download";

            var response = await _client.GetJsonAsync<PhotoDownloadResponse>(endpoint);
            return response;
        }
        record SingleObject<T>(T Data);
        record MultipleObjects<T>(List<T> Data);
        public void Dispose()
        {
            _client?.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
