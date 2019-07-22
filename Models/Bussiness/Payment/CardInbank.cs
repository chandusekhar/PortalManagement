using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness.Payment
{
    public class CardInBank
    {
        public long Total { get; set; }
        public int CardType { get; set; }
        public long Amount { get; set; }
    }
}