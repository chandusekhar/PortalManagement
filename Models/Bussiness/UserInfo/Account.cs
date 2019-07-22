using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness.UserInfo
{
    public class Account
    {
        public long AccountID { get; set; }
        public string DisplayName { get; set; }
        public bool IsAgency { get; set; }
        public long GameAccountId { get; set; }
    }
}