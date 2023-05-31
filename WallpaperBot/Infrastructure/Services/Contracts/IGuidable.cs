using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallpaperBot.Infrastructure.Services.Contracts
{
    internal interface IGuidable
    {
        string GenerateId();
    }
}
