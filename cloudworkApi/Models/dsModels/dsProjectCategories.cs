using System;
using cloudworkApi.DataManagers;

namespace cloudworkApi.Models.dsModels
{
    public class dsProjectCategory:DataManager
    {
        public int value { get; set; }
        public string label { get; set; }
    }
}
