using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Newtonsoft.Json;
using PortalManagement.Models.Bussiness.Payment;
using PortalManagement.Models.DataAccess.Payment;
using Utilities.Log;

namespace PortalManagement.Controllers.Payment
{
    public class PaymentController : Controller
    {
        // GET: Payment
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Analytic()
        {
            return View();
        }

        public ActionResult History()
        {
            return View();
        }

        public ActionResult CardBank()
        {
            return View();
        }

        public ActionResult Verify()
        {
            return View();
        }

        public ActionResult Billing()
        {
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }

        // Update YagamiRaito

        public ActionResult CardInGameStatistics() // thống kê nạp thẻ
        {


            var cardConfigs = PayDAO.GetCards();
            var numOfPays = cardConfigs.FirstOrDefault().PayOrderConfig.Split('|').Length;
            var types = cardConfigs.GroupBy(x => x.Type).Select(x => x.Key).ToList();
            ViewBag.Types = types;
            ViewBag.NumOfPays = numOfPays;
            return View();
        }

        public ActionResult PayLogData(int day = 0, int month = 0, int year = 0, int pay = 0, int type = 0, int page = 1, int total = 100) // thống kê nạp thẻ
        {
            if (page < 1)
            {
                return new EmptyResult();
            }

            day = day == 0 ? DateTime.Now.Day : day;
            month = month == 0 ? DateTime.Now.Month : month;
            year = year == 0 ? DateTime.Now.Year : year;
            var payLogs = PayDAO.GetAllPayLogs();

            var filter = payLogs.Where(x => x.CreatedTime.Month == month && x.CreatedTime.Year == year && x.CreatedTime.Day == day);
            if (pay != 0)
                filter = filter.Where(x => x.PayId == pay);
            if (type != 0)
                filter = filter.Where(x => x.CardType == type);
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling(filter.Count() * 1.0 / total); // tổng số trang
            filter = filter.Skip((page - 1) * total).Take(total).OrderByDescending(x => x.CreatedTime);

            return View(filter);
        }

        public ActionResult PayLogByDate(int month = 0, int year = 0, int pay = 0, int type = 0)
        {
            //if (page < 1)
            //{
            //    return new EmptyResult();
            //}
            month = month == 0 ? DateTime.Now.Month : month;
            year = year == 0 ? DateTime.Now.Year : year;

            var payLogs = PayDAO.GetAllPayLogs();

            var filter = payLogs.Where(x => x.CreatedTime.Month == month && x.CreatedTime.Year == year);
            if (pay != 0)
                filter = filter.Where(x => x.PayId == pay);
            if (type != 0)
                filter = filter.Where(x => x.CardType == type);


            var dateGroups = filter.GroupBy(x => x.CreatedTime.Date).OrderByDescending(x => x.Key);
            var datePayLogs = new List<DatePayLog>();
            foreach (var dateGroup in dateGroups)
            {
                datePayLogs.Add(new DatePayLog()
                {
                    DateString = dateGroup.Key.ToString("dd/MM/yyyy"),
                    TotalSuccessTran = dateGroup.Count(x => x.Status == 1),
                    TotalFailTran = dateGroup.Count(x => x.Status < 0),
                    TotalPendingTran = dateGroup.Count(x => x.Status == 0),
                    TotalTopup = dateGroup.Where(x => x.Status == 1).Sum(x => x.Amount)

                });
            }

            ViewBag.TotalTopup = filter.Where(x => x.Status == 1).Sum(x => x.Amount);
            ViewBag.TotalSuccessTran = filter.Count(x => x.Status == 1);
            ViewBag.TotalFailTran = filter.Count(x => x.Status < 0);
            ViewBag.TotalPendingTran = filter.Count(x => x.Status == 0);

            //ViewBag.CurrentPage = page;
            //ViewBag.TotalPage = Math.Ceiling(datePayLogs.Count() * 1.0 / total); // tổng số trang
            //datePayLogs = datePayLogs.Skip((page - 1) * total).Take(total).ToList();

            return PartialView(datePayLogs);
        }

        public ActionResult GoldTranferStatistics() // thống kê mua bán vàng của đại lý với nph
        {
            return View();
        }

        public ActionResult GoldTransferStatisticsData(int month = 0)
        {
            var aTrans = PayDAO.GetAllAgencyTransactions();

            month = month == 0 ? DateTime.Now.Month : month;

            var soldByOwner = aTrans.Where(x => (x.SenderID == 860000 || x.SenderID == 860001) && (x.RecvID != 860000 && x.RecvID != 860001) && x.CreatedTime.Month == month).OrderByDescending(x => x.CreatedTime);
            var soldByPartner = aTrans.Where(x => (x.RecvID == 860000 || x.RecvID == 860001) && (x.SenderID != 860000 && x.SenderID != 860001) && x.CreatedTime.Month == month).OrderByDescending(x => x.CreatedTime);

            var ownerGroupByDate = soldByOwner.GroupBy(x => x.CreatedTime.Date);
            var parnerGroupByDate = soldByPartner.GroupBy(x => x.CreatedTime.Date);

            var ownerList = new List<AgencyTranByDate>();
            var partnerList = new List<AgencyTranByDate>();

            foreach (var date in ownerGroupByDate)
            {
                ownerList.Add(new AgencyTranByDate()
                {
                    DateString = date.Key.ToString("dd/MM/yyyy"),
                    TotalGold = date.Sum(x => x.Amount),
                    TotalMoney = (long)(date.Sum(x => x.Amount) * 0.82),
                    TotalTrans = date.Count()
                });
            }

            foreach (var date in parnerGroupByDate)
            {
                partnerList.Add(new AgencyTranByDate()
                {
                    DateString = date.Key.ToString("dd/MM/yyyy"),
                    TotalGold = date.Sum(x => x.Amount),
                    TotalMoney = (long)(date.Sum(x => x.Amount) * 0.82),
                    TotalTrans = date.Count()
                });
            }


            var totalOwnerGold = ViewBag.TotalOwnerGold = soldByOwner.Sum(x => x.Amount);
            ViewBag.TotalOwnerMoney = totalOwnerGold * 0.82;
            ViewBag.TotalOwnerTrans = soldByOwner.Count();

            // by partner
            var totalPartnerGold = ViewBag.TotalPartnerGold = soldByPartner.Sum(x => x.Amount);
            ViewBag.TotalPartnerMoney = totalPartnerGold * 0.82;
            ViewBag.TotalPartnerTrans = soldByPartner.Count();
            var allDate = ownerList.Select(x => x.DateString).Union((partnerList.Select(x => x.DateString)));
            //NLogManager.LogMessage($"ownerList:{JsonConvert.SerializeObject(ownerList)}\npartnerList:{JsonConvert.SerializeObject(partnerList)}\nAlLDate:{JsonConvert.SerializeObject(allDate)}");
            ViewData["OwnerList"] = ownerList;
            ViewData["PartnerList"] = partnerList;
            ViewData["AllDate"] = allDate;
            return PartialView();
        }

        public ActionResult GoldTransferDetails(string date, bool isSelling, int month = 0) //xem chi tiết giao dịch đại lý và nph
        {
            var aTrans = PayDAO.GetAllAgencyTransactions();
            var currentDate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            month = month == 0 ? DateTime.Now.Month : month;
            aTrans = aTrans.Where(x => x.CreatedTime.Month == month);
            aTrans = aTrans.Where(x => x.CreatedTime.Date == currentDate); // xem trong ngày chỉ định
            var transferDetails = new List<AgencyTransaction>();
            if (isSelling)
            {
                transferDetails = aTrans.Where(x => (x.RecvID == 860000 || x.RecvID == 860001) && (x.SenderID != 860000 && x.SenderID != 860001)).OrderByDescending(x => x.CreatedTime).ToList();
            }
            else
            {
                transferDetails = aTrans.Where(x => (x.SenderID == 860000 || x.SenderID == 860001) && (x.RecvID != 860000 && x.RecvID != 860001)).OrderByDescending(x => x.CreatedTime).ToList();
            }

            ViewBag.IsSelling = isSelling; // đại lý bán


            return View(transferDetails);
        }

        public ActionResult BalanceStatistics() // Thống kê số dư
        {
            var data = PayDAO.GetAllBalanceStatitics();
            data = data.OrderByDescending(x => x.Date);
            ViewBag.TotalAllUserBalance = data.Sum(x => x.TotalUserBalance);
            ViewBag.TotalAllPartnerBalance = data.Sum(x => x.TotalPartnerBalance);
            return View(data);
        }

        public ActionResult AgencyTransactionLevel1Statistics(DateTime from, DateTime to, List<int> filterId = null) // Thống kê giao dịch đại lý cấp 1
        {
            var partnerTransLevel = GetPartnerTransWithLevel(1);
            var partners = PayDAO.GetAllPartners();
            var level1Partners = partners.Where(x => x.Level == 1);
            partnerTransLevel = partnerTransLevel.Where(x => x.CreatedTime >= from && x.CreatedTime <= to); // filter by date
            if (filterId != null)
            {
                partnerTransLevel.Where(x => filterId.Any(y => y == x.SenderID || y == x.RecvID)); // filter by ids
                partners.Where(x => filterId.Any(y => y == x.GameAccountId)); // filter by ids
            }
            var data = new List<PartnerTranWithLevel>();
            
            

            return View();
        }

 

        public ActionResult AgencyTransactionLevel2Statistics(DateTime from, DateTime to, List<int> filterId1 = null, List<int> filterId2 = null) // Thống kê giao dịch đại lý cấp 2
        {
            var partnerTransLevel = GetPartnerTransWithLevel(2);
            return View();
        }

        private IEnumerable<PartnerTranWithLevel> GetPartnerTransWithLevel(int level)
        {
            var partners = PayDAO.GetAllPartners();
            var partnerTrans = PayDAO.GetAllAgencyTransactions();
            var partnerTransByLevel = from a in partnerTrans
                from b in partners.Where(p => (p.GameAccountId == a.SenderID) || p.GameAccountId == a.RecvID)
                    .DefaultIfEmpty()
                select new PartnerTranWithLevel()
                {
                    ID = a.ID,
                    CreatedTime = a.CreatedTime,
                    Amount = a.Amount,
                    Fee = a.Fee,
                    Sender = a.Sender,
                    SenderID = a.SenderID,
                    Recv = a.Recv,
                    RecvID = a.RecvID,
                    Description = a.Description,
                    Note = a.Note,
                    CreateTimeInt = a.CreateTimeInt,
                    State = a.State,
                    Level = b.Level
                };
            return partnerTransByLevel.Where(x => x.Level == level);
        }
    }
}