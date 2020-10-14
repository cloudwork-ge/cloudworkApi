using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudworkApi.DataManagers
{
    public class DataManager
    {
        public static IConfiguration configuration;
        public static string _connectionString { get {
                return configuration.GetConnectionString("DefaultDB").ToString();
            } 
        }
        public static DateTime timeStampDateUTC(int timestamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);// Universal time zone
            var timestampDate = dtDateTime.AddSeconds(timestamp);
            return timestampDate;
        }
        protected string connectionString { get {
                return DataManager._connectionString;
            }
        }
    }
}
