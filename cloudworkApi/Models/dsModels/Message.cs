using System;
using System.ComponentModel.DataAnnotations.Schema;
using cloudworkApi.Models.tableModels;

namespace cloudworkApi.Models.dsModels
{
    public class Message
    {
        public Message()
        {
        }
        public int ID { get; set; }
        public int chatID { get; set; }
        public string messageText {get; set;}
    }
}
