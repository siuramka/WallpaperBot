using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WallpaperBot.Wallpapers.Models.Clients.Parameters.Unsplash;
using WallpaperBot.Wallpapers.Models.Clients.Responses.Unsplash;

namespace WallpaperBot.Wallpapers.Clients.Unsplash.Contracts
{
    public interface IUnsplashPublicClient
    {
        Task<List<PhotosRandomResponse>> GetPhotos(string? orderBy);
        Task<PhotoDownloadResponse> GetPhotoDownloadUrl(string photoId);
    }
}
