using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness.UserInfo
{
    public class TransferLog
    {
        public long ID { get; set; }
        public long SendID { get; set; }
        public long RecvID { get; set; }
        public string SendName { get; set; }
        public string RecvName { get; set; }
        public long SendAmount { get; set; }
        public long RecvAmount { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Reason { get; set; }
    }
}