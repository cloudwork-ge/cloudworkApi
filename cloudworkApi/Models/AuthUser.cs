using cloudworkApi.DataManagers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudworkApi.Models
{
    public class AuthUser
    {
        public int ID { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string tin { get; set; }
        public string userType { get; set; }
        public string token { get; set; }
        public DateTime authDate { get; set; } = new DateTime();

    }
    public enum UserTypes {
        Freelancer,
        Business
    }
    public static class CurrentInstance
    {
        public static int userID { get; set; }
        public static string IP { get; set; }
        public static HttpContext httpContext { get; set; }

    }
}
