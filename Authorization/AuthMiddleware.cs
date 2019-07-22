using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Utilities.Log;

namespace PortalManagement.Authorization
{
    public class AuthMiddleware : OwinMiddleware
    {
        public AuthMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public async override Task Invoke(IOwinContext context)
        {
            string path = context.Request.Path.ToString().ToLower();
            var user = UserContext.UserInfo;
            if (path.ToLower().StartsWith("/logout"))
            {
                await Next.Invoke(context);
                return;
            }

            if (user.AccountID <= 0)
            {
                if (!path.ToLower().StartsWith("/login".ToLower()) && !path.ToLower().StartsWith("/Unauthorize".ToLower()))
                {
                    HttpContext.Current.Response.Redirect((context.Request.PathBase + "/Login").ToLower(), true);
                    return;
                }
            }
            else
            {
                if (!RoleBuilder.GetAllowPath().Exists(x => x.ToLower() == path.ToLower()) && path != "/" && !path.ToLower().StartsWith("/Unauthorize".ToLower()))
                {
                    HttpContext.Current.Response.Redirect((context.Request.PathBase + "/Unauthorize").ToLower(), true);
                    return;
                }
            }

            await Next.Invoke(context);
        }
    }
}