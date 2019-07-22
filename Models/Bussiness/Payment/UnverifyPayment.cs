using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness.Payment
{
    public class UnverifyPayment
    {
        public long ID { get; set; }
        public long AccountId { get; set; }
        public string AccountName { get; set; }
        public long Amount { get; set; }
        public int CardType { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}