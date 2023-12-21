using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DoAn_LapTrinhWeb.Controllers
{
    public class RouteController : Controller
    {
        // GET: Route
        public ActionResult Index()
        {
            //Get all routes in RouteConfig
            var routes = RouteTable.Routes;
            var routeCollection = routes.Cast<Route>().ToList();
            var routeData = routes.GetRouteData(new HttpContextWrapper(HttpContext.ApplicationInstance.Context));
            var routeDictionary = routes.Cast<Route>().ToDictionary(x => x.Url, x => x.Defaults);
            return Json(routeDictionary.Keys, JsonRequestBehavior.AllowGet);
        }
    }
}