using System.Web.Mvc;

namespace RentCar.Controllers
{
    public class HomeController : Controller
    {

        public static string SystemName = "";
        // GET: Home
        [ActionName("Index")]
        public ActionResult Index_Get()
        {

            Session["Lang"] = "English";
            Session["POS"] = "1002";
            return View();
        }


        [HttpPost]
        [ActionName("Index")]
        public ActionResult Index_Post(string lang)
        {
            if (!string.IsNullOrEmpty(lang))
            {
                if (Session["Language"].ToString() == "1")
                {
                    Session["Language"] = "2";
                    Session["Lang"] = "Arabic";
                }
                else
                {
                    if (Session["Language"].ToString() == "2")
                    {
                        Session["Language"] = "1";
                        Session["Lang"] = "English";
                    }
                }
            }


            return View();
        }

    }
}