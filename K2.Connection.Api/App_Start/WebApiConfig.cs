using K2.Connection.Api.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace K2.Connection.Api.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Implements dynamic url routing for any controller.
            config.MapHttpAttributeRoutes(new ApiGlobalPrefixRouteProvider("api"));
        }
    }
}