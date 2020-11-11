using System;
using System.ComponentModel;

namespace cloudworkApi.Models.dsModels
{
    public class ProjectBids
    {
       public int ID { get; set; }
       public int userID { get; set; }
       public int projectID { get; set; }
       public int budget { get; set; }
       public int deadlineDays { get; set; }
       public string paymentCondition { get; set; }
       public string comment { get; set; }
       public int status { get; set; } // -1 Rejected, 0 - Pending, 1 - Accepted
       private DateTime _create_date = DateTime.Now;
       public DateTime createDate { get { return _create_date; } set { return; } }

    }
}
