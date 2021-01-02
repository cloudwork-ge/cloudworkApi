using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace cloudworkApi.Models.tableModels
{
    [Table("Chats")]
    public class tbChats
    {
        public tbChats()
        {
        }
        public int ID { get; set; }
	    public string Name { get; set; }
	    public int firstUserID { get; set; }
	    public string firstUserName { get; set; }

        public int secondUserID { get; set; }
	    public string secondUserName { get; set; }
        public DateTime createDate { get; set; }
        public string? lastMessage { get; set; }
        public DateTime? lastMessageDate { get; set; }

        public string lastMessageDateStr
        {
            get
            {
                try
                {
                    return DateTime.Parse(lastMessageDate.ToString()).ToString("dd-MM-yyyy HH:mm:ss");
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        public DateTime? lastMessageRead;
    }
}
