using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace cloudworkApi.Common
{
    public static class ResponseBuilder
    {
        public static int statusID = -1;
        public static bool isUserException { get; set; }
        public static string Success(object data)
        {
            var status = new { ID = 0, TEXT = "ოპერაცია წარმატებით დასრულდა" };
            var obj = new { DATA = data, STATUS = status };

           var x = JsonSerializer.Serialize(obj);
            return x;
        }         
        public static string Error(string error, string defaultErrorText, bool isDevelopment = false)
        {
            var errorText = "";
            if (isUserException)
            {
                errorText = error;
                statusID = statusID == -1 ? -2 : statusID;
            }
            else
            {
                if (isDevelopment)
                    errorText = error;
                else
                    errorText = defaultErrorText;
                statusID = -1;
            }
            var status = new { ID = statusID, TEXT = errorText };
            var obj = new { DATA = "", STATUS = status };

            var x = JsonSerializer.Serialize(obj);
            return x;
        }        
        public static string throwError(string text, int status_id = -1)
        {
            statusID = status_id;
            if (text == null)
                throw new UserExceptions("არასწორი ქმედება.");
             throw new UserExceptions(text);
        } 
    }
}
