using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortalManagement.Models.DataAccess.AddFeatures;
using Utilities.IP;
using Utilities.Log;

namespace PortalManagement.Controllers.AddFeatures
{
    public class AddFeaturesController : Controller
    {
        // GET: AddFeatures
        public ActionResult Index()
        {
            long accountId = UserContext.AccountID;

            if (accountId < 1)
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        public ActionResult JackpotPrizeLog()
        {
            var accountId = UserContext.AccountID;

            if (accountId < 1)
            {
                return RedirectToAction("Index", "Login");
            }

            var jackpotPrizeLogs = AddFeaturesDAO.GetLog();

            return View(jackpotPrizeLogs);

        }

        [HttpPost]
        public int GetJackpotPrize(FormCollection col)
        {
            var accountId = UserContext.AccountID;
            var accountName = UserContext.AccountName;

            if (accountId < 1)
            {
                return -101;
            }

            try
            {
                var displayName = col["addfeatures_displayname"];
                var gameId = int.Parse(col["gameselection"]);
                var roomId = int.Parse(col["roomselection"]);

                if (string.IsNullOrEmpty(displayName))
                    return -2;

                return AddFeaturesDAO.GetJackpotPrize((int)accountId, accountName, displayName, IPAddressHelper.GetClientIP(), gameId, roomId);
                
            }
            catch (Exception e)
            {
                NLogManager.PublishException(e);
                return -99;   
            }
        
        }
    }
}