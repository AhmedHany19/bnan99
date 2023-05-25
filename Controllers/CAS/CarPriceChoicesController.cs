using RentCar.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace RentCar.Controllers.CAS
{
    public class CarPriceChoicesController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: CarPriceChoices
        public ActionResult Index()
        {
            var LessorCode = "";
            var UserLogin = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
            var cR_Cas_Car_Price_Choices = db.CR_Cas_Car_Price_Choices.Include(c => c.CR_Mas_Sup_Choices);
            return View(cR_Cas_Car_Price_Choices.ToList());
        }

        // GET: CarPriceChoices/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Car_Price_Choices cR_Cas_Car_Price_Choices = db.CR_Cas_Car_Price_Choices.Find(id);
            if (cR_Cas_Car_Price_Choices == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Car_Price_Choices);
        }

        // GET: CarPriceChoices/Create
        public ActionResult Create()
        {
            var LessorCode = "";
            var UserLogin = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.CR_Cas_Car_Price_Choices_Code = new SelectList(db.CR_Mas_Sup_Choices, "CR_Mas_Sup_Choices_Code", "CR_Mas_Sup_Choices_Group_Code");
            return View();
        }

        // POST: CarPriceChoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Car_Price_Choices_No,CR_Cas_Car_Price_Choices_Code,CR_Cas_Car_Price_Choices_Value")] CR_Cas_Car_Price_Choices cR_Cas_Car_Price_Choices)
        {
            if (ModelState.IsValid)
            {
                db.CR_Cas_Car_Price_Choices.Add(cR_Cas_Car_Price_Choices);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CR_Cas_Car_Price_Choices_Code = new SelectList(db.CR_Mas_Sup_Choices, "CR_Mas_Sup_Choices_Code", "CR_Mas_Sup_Choices_Group_Code", cR_Cas_Car_Price_Choices.CR_Cas_Car_Price_Choices_Code);
            return View(cR_Cas_Car_Price_Choices);
        }

        // GET: CarPriceChoices/Edit/5
        public ActionResult Edit(string id)
        {
            var LessorCode = "";
            var UserLogin = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Car_Price_Choices cR_Cas_Car_Price_Choices = db.CR_Cas_Car_Price_Choices.Find(id);
            if (cR_Cas_Car_Price_Choices == null)
            {
                return HttpNotFound();
            }
            ViewBag.CR_Cas_Car_Price_Choices_Code = new SelectList(db.CR_Mas_Sup_Choices, "CR_Mas_Sup_Choices_Code", "CR_Mas_Sup_Choices_Group_Code", cR_Cas_Car_Price_Choices.CR_Cas_Car_Price_Choices_Code);
            return View(cR_Cas_Car_Price_Choices);
        }

        // POST: CarPriceChoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Car_Price_Choices_No,CR_Cas_Car_Price_Choices_Code,CR_Cas_Car_Price_Choices_Value")] CR_Cas_Car_Price_Choices cR_Cas_Car_Price_Choices)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cR_Cas_Car_Price_Choices).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CR_Cas_Car_Price_Choices_Code = new SelectList(db.CR_Mas_Sup_Choices, "CR_Mas_Sup_Choices_Code", "CR_Mas_Sup_Choices_Group_Code", cR_Cas_Car_Price_Choices.CR_Cas_Car_Price_Choices_Code);
            return View(cR_Cas_Car_Price_Choices);
        }

        // GET: CarPriceChoices/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Car_Price_Choices cR_Cas_Car_Price_Choices = db.CR_Cas_Car_Price_Choices.Find(id);
            if (cR_Cas_Car_Price_Choices == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Car_Price_Choices);
        }

        // POST: CarPriceChoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Cas_Car_Price_Choices cR_Cas_Car_Price_Choices = db.CR_Cas_Car_Price_Choices.Find(id);
            db.CR_Cas_Car_Price_Choices.Remove(cR_Cas_Car_Price_Choices);
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
