#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WallpaperBot.Wallpapers/WallpaperBot.Wallpapers.csproj", "WallpaperBot.Wallpapers/"]
RUN dotnet restore "WallpaperBot.Wallpapers/WallpaperBot.Wallpapers.csproj"
COPY . .
WORKDIR "/src/WallpaperBot.Wallpapers"
RUN dotnet build "WallpaperBot.Wallpapers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WallpaperBot.Wallpapers.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WallpaperBot.Wallpapers.dll"]