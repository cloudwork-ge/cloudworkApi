using System;
using cloudworkApi.DataManagers;

namespace cloudworkApi.Models.dsModels
{
    public class dsUsers : DataManager
    {
        public Int32 ID { get; set; }
        public string fullName { get; set; }
        public string phone { get; set; }
        public string email { get; set; }

        public DateTime create_date { get; set; }
        public string create_date_str { get { return create_date.ToString("dd-MM-yyyy"); }}
        public string tin { get; set; }
        public string bxEmail { get; set; }
        public bool has_bx { get {
                if (!string.IsNullOrWhiteSpace(bxEmail))
                    return true;
                return false;
            }
        }
        //public string bxPassword { get; set; }
    }
}
