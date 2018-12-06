using K2.Connection.Api.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace K2.Connection.Api
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            // Sets global configuration.
            GlobalConfiguration.Configure(config =>
            {
                WebApiConfig.Register(config);
                WindsorConfig.Register(config);
            });
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
