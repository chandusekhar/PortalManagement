using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness.AddFeatures
{
    public class JackpotPrizeLog
    {
        public int ID { get; set; }
        public long AccountID { get; set; }

        public string AccountName { get; set; }

        public string DisplayName { get; set; }

        public long JackpotPrize { get; set; }

        public long Balance { get; set; }

        public string IpAddress { get; set; }

        public int GameID { get; set; }

        public int CreatedID { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}