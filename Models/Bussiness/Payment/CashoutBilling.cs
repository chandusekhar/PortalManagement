using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness.Payment
{
    public class CashoutBilling
    {
        public long? Amount { get; set; }
        public DateTime? Date { get; set; }
    }
}