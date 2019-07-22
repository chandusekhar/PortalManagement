using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using PortalManagement.Authorization;
using PortalManagement.Models;
using PortalManagement.Models.Category;

[assembly: OwinStartup(typeof(PortalManagement.Startup))]

namespace PortalManagement
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use(typeof(AuthMiddleware));
            UserContext.Initialize();
            SimpleCache.Initialize();
            RoleBuilder.Init();
        }
    }
}
