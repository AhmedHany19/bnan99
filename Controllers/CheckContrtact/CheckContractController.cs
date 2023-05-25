using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentCar.Models;

namespace RentCar.Controllers.CheckContrtact
{
    public class CheckContractController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: CheckContract
        public ActionResult Index()
        {
            var cR_Cas_Contract_Basic = db.CR_Cas_Contract_Basic
                .Include(c => c.CR_Cas_Car_Price_Basic)
                .Include(c => c.CR_Cas_Sup_Car_Information)
                .Include(c => c.CR_Mas_Com_Lessor)
                .Include(c => c.CR_Mas_Renter_Information)
                .Include(c => c.CR_Mas_Sup_Sector);
            return View(cR_Cas_Contract_Basic.ToList());
        }

        public ActionResult CheckContract(String Serial)
        {
            
            return View();
        }

        public JsonResult Check(string Serial)
        {
            string p = "";
            var path = db.CR_Cas_Contract_Basic.Where(c => c.CR_Cas_Contract_Basic_Car_Serail_No == Serial && c.CR_Cas_Contract_Basic_Status == "A").
                            OrderByDescending(d => d.CR_Cas_Contract_Basic_Copy).FirstOrDefault();
            if (path != null)
            {
                p = path.CR_Cas_Contract_Basic_CreateContract_Pdf;
                if (p != null && p != "")
                {
                    TempData["printCR"] = p;

                }
                else
                {
                    TempData["printCR"] = "";
                }

            }
            else
            {
                p = "X";
            }

            return Json(p, JsonRequestBehavior.AllowGet);
        }

        public FileResult ShowPDF(string p)
        {
            
            string mime = MimeMapping.GetMimeMapping(p);
            
            return File(p, mime);
        }

        // GET: CheckContract/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Contract_Basic cR_Cas_Contract_Basic = db.CR_Cas_Contract_Basic.Find(id);
            if (cR_Cas_Contract_Basic == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Contract_Basic);
        }

       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
