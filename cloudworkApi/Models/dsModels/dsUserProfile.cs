using System;
using cloudworkApi.DataManagers;

namespace cloudworkApi.Models.dsModels
{
    public class dsUserProfile:DataManager
    {
        public string fullname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string tin { get; set; }
        public string samforma { get; set; }
        public decimal userType { get; set; }
        public string userTypeTxt { get; set; }
    }
}
