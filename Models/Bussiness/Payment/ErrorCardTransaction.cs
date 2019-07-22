using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness.Payment
{
    public class ErrorCardTransaction
    {
        public long ID { get; set; }
        public int CardType { get; set; }
        public long Amount { get; set; }
        public string TradeMark { get; set; }
        public DateTime BuyTime { get; set; }
        public string TransactionID { get; set; }
        public string ResultCode { get; set; }
    }
}