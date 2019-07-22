using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PortalManagement.Controllers
{
    public class LogoutController : Controller
    {
        // GET: Logout
        public ActionResult Index()
        {
            FormsAuthentication.SignOut();
            string cookieDomain = FormsAuthentication.CookieDomain;
            cookieDomain = Request.Url.Host;
            HttpCookie cookie2 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie2.Domain = cookieDomain;
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);
            return RedirectToAction("Index", "Login");
        }
    }
}