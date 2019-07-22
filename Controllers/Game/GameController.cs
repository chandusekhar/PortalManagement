using PortalManagement.Models.DataAccess.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalManagement.Controllers.Game
{
    public class GameController : Controller
    {
        // GET: Game
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Jackpot()
        {
            return View();
        }

        public ActionResult GameFund()
        {
            var gf = new GameFundList
            {
                Minipoker = GameDAO.GetFundMinipoker(),
                Slot1 = GameDAO.GetFundSlot1(),
                Slot2 = GameDAO.GetFundSlot2(),
                Slot3 = GameDAO.GetFundSlot3(),
                ThanQuay = GameDAO.GetFundThanQuay(),
                Hilo = GameDAO.GetFundHilo(),
                DiskShaking = GameDAO.GetFundDiskShaking(),
                HooHeyHow = GameDAO.GetFundHooHeyHow(),
                SuperNova = GameDAO.GetFundSuperNova()
            };

            return View(gf);
        }

        public ActionResult ModifyFund()
        {
            return View();
        }

        public ActionResult Fee()
        {
            return View();
        }

        public ActionResult LuckySpinFund()
        {
            return View();
        }
    }

    public class GameFundList
    {
        public dynamic Minipoker { get; set; }
        public dynamic Slot1 { get; set; }
        public dynamic Slot2 { get; set; }
        public dynamic Slot3 { get; set; }
        public dynamic ThanQuay { get; set; }
        public dynamic Hilo { get; set; }
        public dynamic SuperNova { get; set; }
        public dynamic HooHeyHow { get; set; }
        public dynamic DiskShaking { get; set; }
    }
}