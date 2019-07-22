using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness.UserInfo
{
    public class VIPInfo {
        public int Rank { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ReceivedTime { get; set; }
        public long Reward { get; set; }
        public bool Status { get; set; }
    }
}