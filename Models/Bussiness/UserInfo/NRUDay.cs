using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness.UserInfo
{
    public class NRUDay
    {
        public DateTime CreatedDate { get; set; }
        public int IOS { get; set; }
        public int Android { get; set; }
        public int Web { get; set; }
        public int OSX { get; set; }
        public int EXE { get; set; }
    }

    public class NRUItem
    {
        public int TotalAccount { get; set; }
        public int DeviceType { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}