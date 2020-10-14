using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudworkApi.DataManagers;
using cloudworkApi.Common;
using cloudworkApi.Controllers;
using cloudworkApi.Models;

namespace cloudworkApi.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class AuthorizationAttribute : ActionFilterAttribute
    {
        public string _text;
        public AuthorizationAttribute()
        {
            //this._text = text;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session != null)
            {
                var authUser = new TokenManager().getAuthUserByToken(null, filterContext.HttpContext);
                if (authUser != null) // access token is active, someone is logged in
                {
                    var mainController = filterContext.Controller as MainController;
                    mainController.authUser = authUser;
                    CurrentInstance.userID = authUser.ID;
                }
                else
                    ResponseBuilder.throwError("You are not Authorized",-5);
            }
        }

    }
}
