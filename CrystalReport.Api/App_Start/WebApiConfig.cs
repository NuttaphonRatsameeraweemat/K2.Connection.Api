using K2.Connection.Helper;
using System.Web.Http;

namespace CrystalReport.Api
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
