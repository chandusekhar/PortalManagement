using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness
{
    public class Dashboard
    {
        public DateTime Date { get; set; }
        public long TotalTopup { get; set; }
        public long TotalSell { get; set; }
        public long TotalCashout { get; set; }
        public long TotalBuy { get; set; }
        public long TotalGameFee { get; set; }
        public long TotalTransFee { get; set; }
        public long TotalGC { get; set; }
        public string DateStr
        {
            get
            {
                return Date.ToString("dd'/'MM'/'yyyy");
            }
        }
    }
}