using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness.Payment
{
    public class AgencyTransaction
    {
        public long ID { get; set; }
        public DateTime CreatedTime { get; set; }
        public long Amount { get; set; }
        public long Fee { get; set; }
        public string Sender { get; set; }
        public long SenderID { get; set; }
        public string Recv { get; set; }
        public long RecvID { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public int CreateTimeInt { get; set; }

        public int State { get; set; }
    }


    public class PartnerTranWithLevel : AgencyTransaction
    {
        public int Level { get; set; }
    }
}