using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortalManagement.Models.DataAccess.OtherFeatures;
using Utilities.Log;

namespace PortalManagement.Controllers.OtherFeatures
{
    public class JackpotPrizeController : Controller
    {
        // GET: JackpotPrize
        public ActionResult Index()
        {
            var accountId = UserContext.AccountID;

            if (accountId < 1)
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        [HttpPost]
        public int SetJackpotPrize(int gameId, int roomId)
        {
            var response = OtherFeaturesDAO.SetJackpotPrize(gameId, roomId);
            NLogManager.LogMessage($"BeginSetJackpotPrize=> gameId:{gameId}|RoomId:{roomId}|LoginID:{UserContext.AccountID}|LoginName:{UserContext.AccountName}");
            return response;
        }

        public ActionResult AvailbleRoom(int gameId)
        {
            var role = UserContext.AccountID;
            ViewBag.GameID = gameId;
            return PartialView();
        }
    }
}