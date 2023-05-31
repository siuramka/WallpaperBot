using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WallpaperBot.Wallpapers.Models.Clients.Responses.Unsplash;

namespace WallpaperBot.Wallpapers.Services.Contracts
{
    public interface IUnsplashService
    {
        Task<List<PhotoDownloadResponse>?> GetPopularPhotos();
    }
}
