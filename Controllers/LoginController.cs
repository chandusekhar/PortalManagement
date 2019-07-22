using PortalManagement.Models.DataAccess.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Utilities.Encryption;
using Utilities.Log;

namespace PortalManagement.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            try
            {
                var data = Request.Form;
                string username = data["user"];
                string password = data["password"];

                if(!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    var userAccount = AccountDAO.Authenticate(username, Utilities.Encryption.Security.MD5Encrypt(password));
                    if(userAccount != null)
                    {
                        SetAuthCookie(userAccount.AccountID, userAccount.AccountName);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }catch(Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return View();
        }


        private void SetAuthCookie(long accountId, string accountName)
        {
            string cookieUsername = $"{accountId}|{accountName}";
            string cookieDomain = FormsAuthentication.CookieDomain;
            cookieDomain = Request.Url.Host;
            FormsAuthentication.SetAuthCookie(cookieUsername, false, FormsAuthentication.FormsCookiePath);
            HttpCookie cookie = FormsAuthentication.GetAuthCookie(cookieUsername, false, FormsAuthentication.FormsCookiePath);
            cookie.HttpOnly = false;
            cookie.Domain = cookieDomain;
            Response.Cookies.Add(cookie);
        }
    }
}