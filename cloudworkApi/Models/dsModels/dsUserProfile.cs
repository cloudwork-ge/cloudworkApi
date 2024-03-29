﻿using System;
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
        public int userType { get; set; }
        public string userTypeTxt { get; set; }
        public string description { get; set; }
        public string workType { get; set; }
        public string bankNumber { get; set; }
    }
}
