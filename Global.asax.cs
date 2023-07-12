using CliWrap;
using CliWrap.Buffered;
using System;
using System.Diagnostics;
using System.IO;
using System.Timers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RentCar
{
    public class MvcApplication : System.Web.HttpApplication
    {
      /*  Timer timer = new Timer(900000);*/

        public void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles); 

          /*  timer.Elapsed += new ElapsedEventHandler(OnElapsed);
            timer.AutoReset = false;
            timer.Start();*/


        }
       /* private async void OnElapsed(object sender, ElapsedEventArgs e)
        { 
            var FollowUpDir = AppDomain.CurrentDomain.BaseDirectory + @"BnanFollowUpContracts\bin\Debug\followUp.exe";
            var powershell = await Cli.Wrap("powershell")
                            .WithWorkingDirectory(@"C:\Users\ahmed\Desktop\B1\bnan99")
                            .WithArguments(new[] { "Start-Process ", "-FilePath ", FollowUpDir })
                            .ExecuteBufferedAsync();
            Debug.WriteLine(powershell.StandardError);
            timer.Start();
        }*/

    }
}
