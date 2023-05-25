using RentCar.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace RentCar.Controllers
{
    public class RegionsController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        public static string TaskCode;

        // GET: Regions
        public ActionResult ListRegion()
        {
            //TaskCode = "1904";
            //Session["POS"] = "1904";

            return View(db.CR_Mas_Sup_Regions.ToList());
        }

        // GET: Regions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Sup_Regions cR_Mas_Sup_Regions = db.CR_Mas_Sup_Regions.Find(id);
            if (cR_Mas_Sup_Regions == null)
            {
                return HttpNotFound();
            }
            return View(cR_Mas_Sup_Regions);
        }

        // GET: Regions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Regions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Mas_Sup_Regions_Code,CR_Mas_Sup_Regions_Ar_Name,CR_Mas_Sup_Regions_En_Name,CR_Mas_Sup_Regions_Fr_Name,CR_Mas_Sup_Regions_L1_Name,CR_Mas_Sup_Regions_L2_Name,CR_Mas_Sup_Regions_Status,CR_Mas_Sup_Regions_Reasons")] CR_Mas_Sup_Regions cR_Mas_Sup_Regions)
        {
            if (ModelState.IsValid)
            {
                db.CR_Mas_Sup_Regions.Add(cR_Mas_Sup_Regions);
                db.SaveChanges();
                return RedirectToAction("ListRegion");
            }

            return View(cR_Mas_Sup_Regions);
        }

        // GET: Regions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Sup_Regions cR_Mas_Sup_Regions = db.CR_Mas_Sup_Regions.Find(id);
            if (cR_Mas_Sup_Regions == null)
            {
                return HttpNotFound();
            }
            return View(cR_Mas_Sup_Regions);
        }

        // POST: Regions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Mas_Sup_Regions_Code,CR_Mas_Sup_Regions_Ar_Name,CR_Mas_Sup_Regions_En_Name,CR_Mas_Sup_Regions_Fr_Name,CR_Mas_Sup_Regions_L1_Name,CR_Mas_Sup_Regions_L2_Name,CR_Mas_Sup_Regions_Status,CR_Mas_Sup_Regions_Reasons")] CR_Mas_Sup_Regions cR_Mas_Sup_Regions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cR_Mas_Sup_Regions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListRegion");
            }
            return View(cR_Mas_Sup_Regions);
        }

        // GET: Regions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Sup_Regions cR_Mas_Sup_Regions = db.CR_Mas_Sup_Regions.Find(id);
            if (cR_Mas_Sup_Regions == null)
            {
                return HttpNotFound();
            }
            return View(cR_Mas_Sup_Regions);
        }

        // POST: Regions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Mas_Sup_Regions cR_Mas_Sup_Regions = db.CR_Mas_Sup_Regions.Find(id);
            db.CR_Mas_Sup_Regions.Remove(cR_Mas_Sup_Regions);
            db.SaveChanges();
            return RedirectToAction("ListRegion");
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
