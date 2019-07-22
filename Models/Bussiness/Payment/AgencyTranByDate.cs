using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness.Payment
{
    public class AgencyTranByDate
    {
        public string DateString { get; set; }

        public long TotalGold { get; set; }

        public long TotalMoney { get; set; }

        public int TotalTrans { get; set; }
    }
}