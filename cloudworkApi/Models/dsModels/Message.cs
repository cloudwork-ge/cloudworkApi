using System;
using System.ComponentModel.DataAnnotations.Schema;
using cloudworkApi.Models.tableModels;

namespace cloudworkApi.Models.dsModels
{
    [Table("V_Messages")]
    public class Message : tbMessages
    {
        public Message()
        {
        }
        public string fromUserName {get; set;}
        public string toUserName {get; set;}
    }
}
