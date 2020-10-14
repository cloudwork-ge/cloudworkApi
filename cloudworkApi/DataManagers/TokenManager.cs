using cloudworkApi.Common;
using cloudworkApi.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace cloudworkApi.DataManagers
{
    public class TokenManager
    {
        public DateTimeOffset tokenExpireTime = DateTimeOffset.Now.AddDays(60); // 60 days
        public AuthUser getAuthUserByToken(string token, HttpContext httpContext = null)
        {
            ObjectCache memCache = MemoryCache.Default;
            var currentAccessToken = string.Empty;
            if (!string.IsNullOrEmpty(token))
                currentAccessToken = token;
            else if (httpContext != null)
                currentAccessToken = httpContext.Request.Headers.ToList().Find(x => x.Key == "Authorization").Value.ToString().Replace("Bearer", "").Trim();
            else throw new Exception("Token and HttpContext not defined, please specify one");

            AuthUser authUser = currentAccessToken == String.Empty ? null : memCache.Get(currentAccessToken) == null ? null : (AuthUser)memCache.Get(currentAccessToken);

            if (authUser == null && currentAccessToken != string.Empty)
            {
                ResponseBuilder.throwError("მიღებული Token არასწორია.",-6);
            }
             return authUser;
        }
        public string getToken(string accessToken, HttpContext httpContext)
        {
            ObjectCache memCache = MemoryCache.Default;
            return memCache.Get(accessToken).ToString();
        }
        public string setToken(string accessToken)
        {
            ObjectCache memCache = MemoryCache.Default;
            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = tokenExpireTime;
            var authUser = getAuthUserByToken(accessToken);
            CacheItem item = new CacheItem(accessToken, authUser);
            memCache.Set(item, policy);
            return accessToken;
        }
        public string setToken(string accessToken, AuthUser authUser)
        {
            ObjectCache memCache = MemoryCache.Default;
            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = tokenExpireTime;
            CacheItem item = new CacheItem(accessToken, authUser);
            memCache.Set(item, policy);
            return accessToken;
        }
        public string createSetToken(AuthUser jsonValue)
        {
            ObjectCache memCache = MemoryCache.Default;
            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = tokenExpireTime;
            var accessToken = Guid.NewGuid().ToString();
            CacheItem item = new CacheItem(accessToken,jsonValue);
            memCache.Add(item,policy);
            return accessToken;
        }
        public string createToken()
        {
            return Guid.NewGuid().ToString();
        }
        public string refreshToken(string accessToken)
        {
            return setToken(accessToken);
        }
        public void deleteToken(string accessToken)
        {
            ObjectCache memCache = MemoryCache.Default;
            if (memCache.Get(accessToken) != null)
                memCache.Remove(accessToken);
        }

    }
}
