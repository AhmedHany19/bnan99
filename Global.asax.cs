using CrystalDecisions.CrystalReports.Engine;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Logging;
using System.Timers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System;
using System.Timers;
using Microsoft.Ajax.Utilities;
using RentCar.Models;
using System.Linq;

namespace RentCar
{
    public class MvcApplication : System.Web.HttpApplication
    {
    
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }


       

    }
}
