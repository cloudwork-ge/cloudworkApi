using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using cloudworkApi.DataManagers;
using Newtonsoft.Json.Linq;
using cloudworkApi.Common;
using System.Text.Json;
using cloudworkApi.Models;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using cloudworkApi.Attributes;

namespace cloudworkApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [EnableCors("AllowAll")]
    public class MainController : Controller
    {
        //public readonly IConfiguration _configuration;
        //public readonly string _connString;
        public readonly HttpContext _httpContext;
        public MainController()
        {
        }
        public void Index(HttpContext context)
        {
            
        }
        public JsonDocument Success()
        {
            return JsonDocument.Parse(ResponseBuilder.Success(new { }));
        }
        public JsonDocument Success(object data)
        {
            return JsonDocument.Parse(ResponseBuilder.Success(data));
        }
        public JsonDocument throwError(string text = null, int statusID = -1)
        {
            return JsonDocument.Parse(ResponseBuilder.throwError(text, statusID));
        }
        public async Task<JsonDocument> throwErrorAsync(string text = null, int statusID = -1)
        {
            await Task.Delay(1);
            return JsonDocument.Parse(ResponseBuilder.throwError(text, statusID));
        }
        public AuthUser authUser {get;set; 
            //get {
            //   return new TokenManager().getAuthUserByToken(authUser.token, _httpContext);
            //} 
        }
    }
}