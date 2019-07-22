using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Game
{
    public class GameJackpot
    {
        public long AccountID { get; set; }
        public string AccountName { get; set; }
        public long PrizeValue { get; set; }
        public int GameID { get; set; }
        public long SessionID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string GameName { get; set; }
        public int BetValue { get; set; }
    }
}