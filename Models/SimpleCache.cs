using PortalManagement.Models.DataAccess.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models
{
    public class SimpleCache
    {
        public static Dictionary<int, string> Game { get; private set; }
        public static void Initialize()
        {
            Game = new Dictionary<int, string>();
            var gameLst = GameDAO.GetGame();
            foreach(var g in gameLst)
            {
                Game.Add(g.ID, g.Name);
            }
        }
    }
}