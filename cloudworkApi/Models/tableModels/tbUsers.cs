﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace cloudworkApi.Models.tableModels
{
    [Table("Users")]
    public class tbUsers
    {
        public tbUsers()
        {
        }
        public int ID { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string tin { get; set; }
    }
}
