using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using cloudworkApi.StoredProcedures;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using cloudworkApi.DataManagers;
using cloudworkApi.Attributes;
using cloudworkApi.Models;
using System.Text.Json;
using System.Net.Http;
using System.Runtime.Caching;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Collections.Immutable;
using System.Security.Policy;
using System.Net;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Reflection.Metadata;
using System.Web;
using cloudworkApi.Common;
using cloudworkApi.Services;
using cloudworkApi.Models.dsModels;
using static cloudworkApi.DataManagers.FilterParam;

namespace cloudworkApi.Controllers
{
    public class UsersController : MainController
    {
        private PKG_USERS provider;
        public UsersController()
        {
            provider = new PKG_USERS();
        }
        [HttpPost]
        //JsonElement
        public JsonDocument Authenticate([FromBody] LoginCredentials user)
        {
            if (user.email == null || user.password == null) throwError("შეიყვანეთ მონაცემები");
            var method = new PKG_USERS();
            //AuthUser authUser = null;
            try
            {
                authUser = new TokenManager().getAuthUserByToken(null,HttpContext);
            }
            catch(Exception ex)
            {

            }

            // if anyone is logged in
            if (authUser != null) 
                return throwError("You are already logged in as " + authUser);
            AuthUser auth = null;
            //if no one is logged in, Authenticate
            if (method.Authenticate(user.email.ToString(), user.password.ToString(),out auth))
            {
                authUser = auth;
                authUser.token = new TokenManager().createSetToken(authUser);
                var dict = new Dictionary<string, object>();
                dict.Add("access_token", authUser.token);
                return Success(dict);
            }
            else
            {
                return throwError("ელ.ფოსტა ან პაროლი არასწორია");
            }
        }

        [HttpPost]
        public JsonDocument Register([FromBody] Registration user)
        {
            var users = new PKG_USERS();
            var register = users.Register(user.email, user.password, user.fullName, user.phone, user.tin, user.samformaType, user.userType);
            if (register)
            {
                var email = new EmailService();
                email.SendEmail(user.email,"რეგისტრაცია {{საიტი}} ზე","თქვენ წარმატებით დარეგისტრირდით {{საიტი}} პორტალზე.");
                LoginCredentials login = new LoginCredentials();
                login.email = user.email;
                login.password = user.password;
                return this.Authenticate(login);
            }
            else
                return throwError("ელ.ფოსტა უკვე რეგისტრირებულია.");
        }
        [HttpPost]
        [Authorization]
        public void Logout()
        {
            new TokenManager().deleteToken(authUser.token);
        }

        [HttpPost]
        [Authorization]
        public JsonDocument GetUserData()
        {
            var dict = new Dictionary<string, object>();
            dict.Add("ID", authUser.ID);
            dict.Add("fullName", authUser.fullName);
            dict.Add("email", authUser.email);
            dict.Add("userType", authUser.userType);

            object output = new
            {
                userData = dict
            };

            return Success(output);
        }
        [HttpPost]
        [Authorization]
        public JsonDocument GetUnreadChats()
        {
            var dict = new Dictionary<string, object>();
            dict.Add("unreadChats", provider.getUnreadChatIDs(authUser.ID));

            object output = new
            {
                unreadChats = dict
            };

            return Success(dict);
        }
        [HttpPost]
        //[Authorization]
        public JsonDocument grdAllUsersExample([FromBody] Grid grid)
        {
            var output = new List<dsUsers>();
            //var grd = new Grid();
             grid.dsViewName = "V_USERS_ALL";
             grid.OrderBy = "ID DESC";

            //var fp = new FilterParam();
            //fp.FieldName = "ID";
            //fp.FilterValue = "2";
            //fp.FilterType = FilterType.Equal;
            ////fp.DataType = DataType.Number;
            
            return Success(grid.GetData<dsUsers>());


        }
        [Authorization]
        public JsonDocument GetUserModules()
        {
            var x = new PKG_USERS().GetUserModules(authUser.ID);

            return Success(x);
        }
    }
}