using PortalManagement.Models.Bussiness.Giftcode;
using PortalManagement.Models.DataAccess.Giftcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalManagement.Controllers.Giftcode
{
    public class GiftcodeController : Controller
    {
        // GET: Giftcode
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Summary()
        {
            return View();
        }

        public ActionResult StatisticAgency()
        {
            return View();
        }

        public ActionResult Management()
        {
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }

        public ActionResult Analytic(int ? id)
        {
            ViewBag.Id = id;
            ViewBag.eventInfo = GCDAO.GetAllEvent();
            return View();
        }

        public ActionResult Statistic()
        {
            return View();
        }

        public void Download(int eventId)
        {
            MemoryStream memoryStream = new MemoryStream();
            GCReporter report = new GCReporter(eventId);

            report.AppendToStream(memoryStream);

            Response.Clear();
            Response.ContentType = "application/force-download";
            Response.AddHeader($"content-disposition", $"attachment;    filename=GC_{eventId}_{DateTime.Now.Ticks}.xls");
            Response.BinaryWrite(memoryStream.ToArray());
            Response.End();
        }
    }
}