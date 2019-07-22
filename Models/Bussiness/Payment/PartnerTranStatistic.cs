using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness.Payment
{
    public class PartnerTranStatistic
    {
        public string DateString { get; set; }

        public string PartnerName { get; set; }

        public long GoldOut { get; set; }

        public long GoldIn { get; set; }

        public long RefundFee { get; set; }
    }
}