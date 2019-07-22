using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness.Giftcode
{
    public class StatisticGC
    {
        public int Type { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
        public int Expired { get; set; }
    }

    public class GCStatistic
    {
        public DateTime Date { get; set; }
        public int AgencyExported { get; set; }
        public int AgencyUsed { get; set; }
        public int AgencyExpired { get; set; }
        public int MKTExported { get; set; }
        public int MKTUsed { get; set; }
        public int MKTExpired { get; set; }
        public int TestExported { get; set; }
        public int TestUsed { get; set; }
        public int TestExpired { get; set; }
    }
}