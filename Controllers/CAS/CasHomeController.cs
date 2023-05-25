using RentCar.Models;
using System.Linq;
using System.Web.Mvc;

namespace RentCar.Controllers.CAS
{
    public class CasHomeController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        // GET: CasHome
        public ActionResult Index()
        {
            var LessorCode = "";
            var UserLogin = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == "" || LessorCode == "")
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public JsonResult OpenBranchSystem()
        {
            db.Configuration.ProxyCreationEnabled = false;
            bool b = false;
            try
            {
                var Auth = Session["AuthBranch"].ToString();
                var BrnchCode = Session["BranchCode"].ToString();
                var UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (Auth == "true")
                {
                   
                    var branch = db.CR_Cas_Sup_Branch.FirstOrDefault(br => br.CR_Cas_Sup_Branch_Code == BrnchCode);
                    if (branch != null)
                    {
                        if (branch.CR_Cas_Sup_Branch_Status != "D")
                        {
                            Session["BranchCode"] = branch.CR_Cas_Sup_Branch_Code;
                            b = true;
                        }
                    }
                    
                    return Json(b, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(b, JsonRequestBehavior.AllowGet);
            }
            return Json(b, JsonRequestBehavior.AllowGet);
        }

    }
}