using PortalManagement.Models.Bussiness.Transaction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalManagement.Controllers.Agency
{
    public class AgencyController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Management()
        {
            return View();
        }

        public ActionResult Authorize()
        {
            return View();
        }

        public ActionResult AgencyTransaction()
        {
            return View();
        }

        public ActionResult AgencyDashboard()
        {
            return View();
        }

        public ActionResult TransAnalytic()
        {
            return View();
        }


        public void Download(int id, int start, int end, int type)
        {
            MemoryStream memoryStream = new MemoryStream();
            Reporter report = new Reporter(id, start, end, type);

            report.AppendToStream(memoryStream);

            Response.Clear();
            Response.ContentType = "application/force-download";
            Response.AddHeader($"content-disposition", $"attachment;    filename=LSGD_{DateTime.Now.Ticks}.xls");
            Response.BinaryWrite(memoryStream.ToArray());
            Response.End();
        }
    }
}