using RentCar.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace RentCar.Controllers
{
    public class CasMechanismController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: CasMechanism
        public ActionResult Index()
        {
            var cR_Cas_Sup_Follow_Up_Mechanism = db.CR_Cas_Sup_Follow_Up_Mechanism.Include(c => c.CR_Mas_Com_Lessor).Include(c => c.CR_Mas_Sup_Procedures);
            return View(cR_Cas_Sup_Follow_Up_Mechanism.ToList());
        }

        // GET: CasMechanism/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_Follow_Up_Mechanism cR_Cas_Sup_Follow_Up_Mechanism = db.CR_Cas_Sup_Follow_Up_Mechanism.Find(id);
            if (cR_Cas_Sup_Follow_Up_Mechanism == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Sup_Follow_Up_Mechanism);
        }

        // GET: CasMechanism/Create
        public ActionResult Create()
        {
            ViewBag.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo");
            ViewBag.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code = new SelectList(db.CR_Mas_Sup_Procedures, "CR_Mas_Sup_Procedures_Code", "CR_Mas_Sup_Procedures_Type");
            return View();
        }

        // POST: CasMechanism/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code,CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code,CR_Cas_Sup_Follow_Up_Mechanism_Activate_Service,CR_Cas_Sup_Follow_Up_Mechanism_Befor_Credit_Limit,CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire,CR_Cas_Sup_Follow_Up_Mechanism_Befor_KM,CR_Cas_Sup_Follow_Up_Mechanism_After_Expire,CR_Cas_Sup_Follow_Up_Mechanism_Default_KM")] CR_Cas_Sup_Follow_Up_Mechanism cR_Cas_Sup_Follow_Up_Mechanism)
        {
            if (ModelState.IsValid)
            {
                db.CR_Cas_Sup_Follow_Up_Mechanism.Add(cR_Cas_Sup_Follow_Up_Mechanism);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Sup_Follow_Up_Mechanism.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code);
            ViewBag.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code = new SelectList(db.CR_Mas_Sup_Procedures, "CR_Mas_Sup_Procedures_Code", "CR_Mas_Sup_Procedures_Type", cR_Cas_Sup_Follow_Up_Mechanism.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code);
            return View(cR_Cas_Sup_Follow_Up_Mechanism);
        }

        // GET: CasMechanism/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_Follow_Up_Mechanism cR_Cas_Sup_Follow_Up_Mechanism = db.CR_Cas_Sup_Follow_Up_Mechanism.Find(id);
            if (cR_Cas_Sup_Follow_Up_Mechanism == null)
            {
                return HttpNotFound();
            }
            ViewBag.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Sup_Follow_Up_Mechanism.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code);
            ViewBag.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code = new SelectList(db.CR_Mas_Sup_Procedures, "CR_Mas_Sup_Procedures_Code", "CR_Mas_Sup_Procedures_Type", cR_Cas_Sup_Follow_Up_Mechanism.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code);
            return View(cR_Cas_Sup_Follow_Up_Mechanism);
        }

        // POST: CasMechanism/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code,CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code,CR_Cas_Sup_Follow_Up_Mechanism_Activate_Service,CR_Cas_Sup_Follow_Up_Mechanism_Befor_Credit_Limit,CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire,CR_Cas_Sup_Follow_Up_Mechanism_Befor_KM,CR_Cas_Sup_Follow_Up_Mechanism_After_Expire,CR_Cas_Sup_Follow_Up_Mechanism_Default_KM")] CR_Cas_Sup_Follow_Up_Mechanism cR_Cas_Sup_Follow_Up_Mechanism)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cR_Cas_Sup_Follow_Up_Mechanism).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Sup_Follow_Up_Mechanism.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code);
            ViewBag.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code = new SelectList(db.CR_Mas_Sup_Procedures, "CR_Mas_Sup_Procedures_Code", "CR_Mas_Sup_Procedures_Type", cR_Cas_Sup_Follow_Up_Mechanism.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code);
            return View(cR_Cas_Sup_Follow_Up_Mechanism);
        }

        // GET: CasMechanism/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_Follow_Up_Mechanism cR_Cas_Sup_Follow_Up_Mechanism = db.CR_Cas_Sup_Follow_Up_Mechanism.Find(id);
            if (cR_Cas_Sup_Follow_Up_Mechanism == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Sup_Follow_Up_Mechanism);
        }

        // POST: CasMechanism/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Cas_Sup_Follow_Up_Mechanism cR_Cas_Sup_Follow_Up_Mechanism = db.CR_Cas_Sup_Follow_Up_Mechanism.Find(id);
            db.CR_Cas_Sup_Follow_Up_Mechanism.Remove(cR_Cas_Sup_Follow_Up_Mechanism);
            db.SaveChanges();
            return RedirectToAction("Index");
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
