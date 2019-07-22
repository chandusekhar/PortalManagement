using PortalManagement.Models.DataAccess.LuckyDice;
using PortalManagement.Models.LuckyDice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalManagement.Controllers.LuckyDice
{
    public class LuckyDiceController : Controller
    {
        // GET: LuckyDice
        public ActionResult Index()
        {
            long accountId = UserContext.AccountID;

            if (accountId < 1)
            {
                return RedirectToAction("Index", "Login");
            }

            // if (accountId != 1)
            //return null;

            var config = LuckyDiceDAO.GetConfigBotTaxi();
            var fundInfo = LuckyDiceDAO.GetFundTaxi();

            var info = new LuckyDiceBotInfo()
            {
                BotConfig = config,
                FundInfo = fundInfo
            };
            return View(info);
        }

        public ActionResult Update(FormCollection col)
        {
            long accountId = UserContext.AccountID;

            if (accountId < 1)
            {
                return RedirectToAction("Index", "Login");
            }

            //if (accountId != 1)
            // return null;

            var minBot = int.Parse(col["minbot"]);
            var maxbot = int.Parse(col["maxbot"]);
            var richbot = int.Parse(col["richbot"]);
            var normalbot = int.Parse(col["normalbot"]);
            var poorbot = int.Parse(col["poorbot"]);
            var richchangerate = int.Parse(col["richchangerate"]);
            var normalchangerate = int.Parse(col["normalchangerate"]);
            var poorchangerate = int.Parse(col["poorchangerate"]);
            var mintimechange = int.Parse(col["mintimechange"]);
            var maxtimechange = int.Parse(col["maxtimechange"]);

            var enable = col["enable"] != null;

            if (minBot >= maxbot)
                return new EmptyResult();
            if (richbot > 146)
                return new EmptyResult();
            if (normalbot > 5074)
                return new EmptyResult();
            if (poorbot > 11826)
                return new EmptyResult();
            if (richchangerate < 0 || richchangerate > 100)
                return new EmptyResult();

            if (poorchangerate < 0 || poorchangerate > 100)
                return new EmptyResult();

            if (poorchangerate < 0 || poorchangerate > 100)
                return new EmptyResult();

            if (mintimechange < 1)
                return new EmptyResult();

            var response = LuckyDiceDAO.BotTaxiUpdate(new BotTaxiConfig()
            {
                MinBot = minBot,
                MaxBot = maxbot,
                NumRichBot = richbot,
                NumNormalBot = normalbot,
                NumPoorBot = poorbot,
                VipChangeRate = richchangerate,
                NorChangeRate = normalchangerate,
                PoorChangeRate = poorchangerate,
                MinTimeChange = mintimechange,
                MaxTimeChange = maxtimechange,
                Enable = enable
            });
            if (response > 0)
                return RedirectToAction("Index");
            return new EmptyResult();
        }

        public ActionResult BetValueData(int vip = 2)
        {
            var accountId = UserContext.AccountID;

            if (accountId < 1)
            {
                return new EmptyResult();
            }
            var botData = LuckyDiceDAO.GetBetData();
            if (vip != -1)
                botData = botData.Where(x => x.Vip == vip);
            ViewBag.Vip = vip;
            return PartialView(botData);
        }

        [HttpPost]
        public int UpdateBetData(int betValue, int vip, int quantity)
        {
            var accountId = UserContext.AccountID;

            if (accountId < 1)
            {
                return -99;
            }
            return LuckyDiceDAO.UpdateBetData(betValue, vip, quantity);
        }
    }
}