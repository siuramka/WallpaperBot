using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WallpaperBot.Wallpapers.Clients.Unsplash.Contracts;

namespace WallpaperBot.Wallpapers.Clients.Unsplash.Authenticators
{
    public class UnsplashPubicAuthenticator : AuthenticatorBase
    {
        readonly string _baseUrl;
        readonly string _accessKey;
        readonly string _secretKey;

        public UnsplashPubicAuthenticator(string baseUrl, string accessKey, string secretKey) : base("")
        {
            _baseUrl = baseUrl;
            _accessKey = accessKey;
            _secretKey = secretKey;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken">This is not used, need to use for override</param>
        /// <returns></returns>
        protected override async ValueTask<Parameter> GetAuthenticationParameter(string accessToken)
        {
            Token = string.IsNullOrEmpty(Token) ? GetToken() : Token;
            return new HeaderParameter(KnownHeaders.Authorization, Token);
        }
        private string GetToken()
        {
            return $"Client-ID {_accessKey}";
        }

    }
}
