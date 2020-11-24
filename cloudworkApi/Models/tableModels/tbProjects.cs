using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace cloudworkApi.Models.tableModels
{
    [Table("Projects")]
    public class tbProjects
    {
        public tbProjects()
        {
        }
        public int ID { get; set; }
        public int category { get; set; }
        public int type { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string criteria { get; set; }
        public int budget { get; set; }
        public int status { get; set; }
        public DateTime create_date { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public int userId { get; set; }
        public int? workerUserId { get; set; }
        public int? months_length { get; set;
        }
    }
}
