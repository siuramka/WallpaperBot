using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WallpaperBot.Infrastructure.Services.Contracts;

namespace WallpaperBot.Infrastructure.Services
{
    //This could be static but w.e
    internal class FileWriter : IFileWriter
    {
        public async Task WriteBytesToFile(string path, byte[] bytes)
        {
            await File.WriteAllBytesAsync(path, bytes);
        }
    }
}
