using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace cloudworkApi.Models.tableModels
{
    [Table("Messages")]
    public class tbMessages
    {
        public tbMessages()
        {
        }
        public int ID { get; set; }
        public int fromUserID { get; set; }
        public string fromUserName { get; set; }
        public int toUserID { get; set; }
        public string toUserName { get; set; }
        public string message { get; set; }
        public DateTime createDate { get; set; }
        public int read { get; set; } // 0 - unread, 1 - read
        public DateTime? readDate { get; set; }
    }
}
