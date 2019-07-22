using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness.Transaction
{
    public class RefundLog
    {
        public int AccountId { get; set; }
        public long TotalRefund { get; set; }
    }
}