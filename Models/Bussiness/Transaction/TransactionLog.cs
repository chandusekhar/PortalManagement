using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness.Transaction
{
    public class TransactionLog
    {
        public long Amount { get; set; }
        public long SenderID { get; set; }
        public long RecvID { get; set; }
    }

    public class AgencyTransaction
    {
        public long ReferId { get; set; }
        public DateTime CreatedTime { get; set; }
        public long Amount { get; set; }
        public long Fee { get; set; }
        public long SenderID { get; set; }
        public long RecvID { get; set; }
        public string Sender { get; set; }
        public string Recv { get; set; }
        public string Description { get; set; }
        public int State { get; set; }
    }

    public class TotalAgencyTransaction
    {
        public int CreatedDateInt { get; set; }
        public long TotalTransaction { get; set; }
    }
}