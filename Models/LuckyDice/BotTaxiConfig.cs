using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.LuckyDice
{
    public class BotTaxiConfig
    {
        public int MinBot { get; set; }
        public int MaxBot { get; set; }

        public int NumRichBot { get; set; }

        public int NumNormalBot { get; set; }

        public int NumPoorBot { get; set; }

        public int VipChangeRate { get; set; }

        public int NorChangeRate { get; set; }

        public int PoorChangeRate { get; set; }

        public int MinTimeChange { get; set; }

        public int MaxTimeChange { get; set; }

        public bool Enable { get; set; }
    }
}