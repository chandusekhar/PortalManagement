using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness.Payment
{
    public class DatePayLog
    {
        public string DateString { get; set; }
        public long TotalTopup { get; set; }

        public  long TotalSuccessTran { get; set; }

        public long TotalFailTran { get; set; }

        public long TotalPendingTran { get; set; }

    }
}