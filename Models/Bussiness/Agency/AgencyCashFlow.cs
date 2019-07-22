using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness.Agency
{
    public class AgencyCashFlow
    {
        public string Displayname { get; set; }
        public long Gold { get; set; }
        public long LockedGold { get; set; }
        public long TotalGold { get; set; }
        public long TRF { get; set; }
        public long TRF2 { get; set; }
        public int TransactionCount { get; set; }
        public int Level { get; set; }
    }
}