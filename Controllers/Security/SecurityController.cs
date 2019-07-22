using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalManagement.Controllers.Security
{
    public class SecurityController : Controller
    {
        // GET: Security
        public ActionResult UserManagement()
        {
            return View();
        }
    }
}