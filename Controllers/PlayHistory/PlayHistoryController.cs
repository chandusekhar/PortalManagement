using PortalManagement.Models.Bussiness.PlayHistory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalManagement.Controllers.PlayHistory
{
    public class PlayHistoryController : Controller
    {
        // GET: PlayHistory
        public ActionResult Index()
        {
            return View();
        }

        public void Download(long accountId)
        {
            MemoryStream memoryStream = new MemoryStream();
            PlayReporter report = new PlayReporter(accountId);

            report.AppendToStream(memoryStream);

            Response.Clear();
            Response.ContentType = "application/force-download";
            Response.AddHeader($"content-disposition", $"attachment;    filename=LSChoi_{accountId}_{DateTime.Now.Ticks}.xls");
            Response.BinaryWrite(memoryStream.ToArray());
            Response.End();
        }
    }
}