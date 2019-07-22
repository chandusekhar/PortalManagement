using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness.Payment
{
    public class Partner
    {
        public long ID { get; set; }
        
        public string Username { get; set; }

        public string Displayname { get; set; }

        public long GameAccountId { get; set; }

        public string Tel { get; set; }

        public string Fb { get; set; }

        public string Telegram { get; set; }

        public string Information { get; set; }

        public DateTime CreatedTime { get; set; }

        public int Creator { get; set; }

        public string CreatorName { get; set; }

        public int Level { get; set; }

        public bool Displayable { get; set; }

        public int Ordinal { get; set; }

        public bool Logged { get; set; }

        public bool SystemAdmin { get; set; }

        public string GameName { get; set; }

        public bool IsLocked { get; set; }

        public bool IsDelete { get; set; }

        public int IndexOrder { get; set; }
    }
}