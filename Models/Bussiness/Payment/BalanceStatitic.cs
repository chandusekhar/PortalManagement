using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness.Payment
{
    public class BalanceStatitic
    {
        public string Date { get; set; }
        public long TotalUserBalance { get; set; }

        public long TotalPartnerBalance { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}