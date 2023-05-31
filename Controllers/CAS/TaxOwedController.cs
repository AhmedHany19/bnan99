using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentCar.Models;

namespace RentCar.Controllers.CAS
{
    public class TaxOwedController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: TaxOwed
        public ActionResult Index()
        {
            var cR_Cas_Account_Tax_Owed = db.CR_Cas_Account_Tax_Owed.Include(c => c.CR_Cas_Sup_Branch);
            ViewBag.Startdate = DateTime.Now.ToString("yyyy/MM/dd");
            ViewBag.Enddate = DateTime.Now.ToString("yyyy/MM/dd");
            return View(cR_Cas_Account_Tax_Owed.ToList());
        }

        public PartialViewResult PartialIndexTable(string type, string StartDate, string EndDate)
        {

            DateTime sd;
            DateTime ed;
            if (StartDate == "" || EndDate == "")
            {
                sd = DateTime.Now.AddDays(-30);
                ed = Convert.ToDateTime(DateTime.Now);
            }
            else
            {
                sd = Convert.ToDateTime(StartDate);
                ed = Convert.ToDateTime(EndDate);
            }
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
                RedirectToAction("Login", "Account");
            }

            IQueryable<CR_Cas_Account_Tax_Owed> query;
            if (type == "P" && StartDate != "" && EndDate != "")
            {
                query = db.CR_Cas_Account_Tax_Owed.Where(c => c.CR_Cas_Account_Tax_Owed_Com_Code == LessorCode && c.CR_Cas_Account_Tax_Owed_Is_Paid == true
                   && c.CR_Cas_Account_Tax_Owed_Due_Date >= sd && c.CR_Cas_Account_Tax_Owed_Due_Date <= ed).OrderByDescending(d => d.CR_Cas_Account_Tax_Owed_Due_Date)
               .Include(c => c.CR_Cas_Sup_Branch);
                var nbr = query.Count();
            }
            else if (type == "N" && StartDate != "" && EndDate != "")
            {
                query = db.CR_Cas_Account_Tax_Owed.Where(c => c.CR_Cas_Account_Tax_Owed_Com_Code == LessorCode && c.CR_Cas_Account_Tax_Owed_Is_Paid==false
                   && c.CR_Cas_Account_Tax_Owed_Due_Date >= sd && c.CR_Cas_Account_Tax_Owed_Due_Date <= ed).OrderByDescending(d => d.CR_Cas_Account_Tax_Owed_Due_Date)
               .Include(c => c.CR_Cas_Sup_Branch);
            }
            else if (type == "Date" && StartDate != "" && EndDate != "")
            {
                query = db.CR_Cas_Account_Tax_Owed.Where(c => c.CR_Cas_Account_Tax_Owed_Com_Code == LessorCode
                   && (c.CR_Cas_Account_Tax_Owed_Due_Date >= sd && c.CR_Cas_Account_Tax_Owed_Due_Date <= ed)).OrderByDescending(d => d.CR_Cas_Account_Tax_Owed_Due_Date)
               .Include(c => c.CR_Cas_Sup_Branch);
                
            }
            else
            {
                query = db.CR_Cas_Account_Tax_Owed.Where(c => c.CR_Cas_Account_Tax_Owed_Com_Code == LessorCode
                    && c.CR_Cas_Account_Tax_Owed_Due_Date >= sd && c.CR_Cas_Account_Tax_Owed_Due_Date <= ed).OrderByDescending(d => d.CR_Cas_Account_Tax_Owed_Due_Date)
                .Include(c => c.CR_Cas_Sup_Branch);
            }

            return PartialView(query.ToList());
        }


        // GET: TaxOwed/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Account_Tax_Owed cR_Cas_Account_Tax_Owed = db.CR_Cas_Account_Tax_Owed.Find(id);
            if (cR_Cas_Account_Tax_Owed == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Account_Tax_Owed);
        }

        // GET: TaxOwed/Create
        public ActionResult Create()
        {
            ViewBag.CR_Cas_Account_Tax_Owed_Brn_Code = new SelectList(db.CR_Cas_Sup_Branch, "CR_Cas_Sup_Branch_Code", "CR_Cas_Sup_Branch_Ar_Name");
            return View();
        }

        // POST: TaxOwed/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Account_Tax_Owed_Contract_No,CR_Cas_Account_Tax_Owed_Com_Code,CR_Cas_Account_Tax_Owed_Contract_Value,CR_Cas_Account_Tax_Owed_Brn_Code,CR_Cas_Account_Tax_Owed_Percentage,CR_Cas_Account_Tax_Owed_Value,CR_Cas_Account_Tax_Owed_Due_Date,CR_Cas_Account_Tax_Owed_Is_Paid,CR_Cas_Account_Tax_Owed_Pay_Date,CR_Cas_Account_Tax_Owed_Pay_No")] CR_Cas_Account_Tax_Owed cR_Cas_Account_Tax_Owed)
        {
            if (ModelState.IsValid)
            {
                db.CR_Cas_Account_Tax_Owed.Add(cR_Cas_Account_Tax_Owed);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CR_Cas_Account_Tax_Owed_Brn_Code = new SelectList(db.CR_Cas_Sup_Branch, "CR_Cas_Sup_Branch_Code", "CR_Cas_Sup_Branch_Ar_Name", cR_Cas_Account_Tax_Owed.CR_Cas_Account_Tax_Owed_Brn_Code);
            return View(cR_Cas_Account_Tax_Owed);
        }

        // GET: TaxOwed/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Account_Tax_Owed cR_Cas_Account_Tax_Owed = db.CR_Cas_Account_Tax_Owed.Find(id);
            if (cR_Cas_Account_Tax_Owed == null)
            {
                return HttpNotFound();
            }
            ViewBag.CR_Cas_Account_Tax_Owed_Brn_Code = new SelectList(db.CR_Cas_Sup_Branch, "CR_Cas_Sup_Branch_Code", "CR_Cas_Sup_Branch_Ar_Name", cR_Cas_Account_Tax_Owed.CR_Cas_Account_Tax_Owed_Brn_Code);
            return View(cR_Cas_Account_Tax_Owed);
        }

        // POST: TaxOwed/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Account_Tax_Owed_Contract_No,CR_Cas_Account_Tax_Owed_Com_Code,CR_Cas_Account_Tax_Owed_Contract_Value,CR_Cas_Account_Tax_Owed_Brn_Code,CR_Cas_Account_Tax_Owed_Percentage,CR_Cas_Account_Tax_Owed_Value,CR_Cas_Account_Tax_Owed_Due_Date,CR_Cas_Account_Tax_Owed_Is_Paid,CR_Cas_Account_Tax_Owed_Pay_Date,CR_Cas_Account_Tax_Owed_Pay_No")] CR_Cas_Account_Tax_Owed cR_Cas_Account_Tax_Owed)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cR_Cas_Account_Tax_Owed).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CR_Cas_Account_Tax_Owed_Brn_Code = new SelectList(db.CR_Cas_Sup_Branch, "CR_Cas_Sup_Branch_Code", "CR_Cas_Sup_Branch_Ar_Name", cR_Cas_Account_Tax_Owed.CR_Cas_Account_Tax_Owed_Brn_Code);
            return View(cR_Cas_Account_Tax_Owed);
        }

        // GET: TaxOwed/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Account_Tax_Owed cR_Cas_Account_Tax_Owed = db.CR_Cas_Account_Tax_Owed.Find(id);
            if (cR_Cas_Account_Tax_Owed == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Account_Tax_Owed);
        }

        // POST: TaxOwed/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Cas_Account_Tax_Owed cR_Cas_Account_Tax_Owed = db.CR_Cas_Account_Tax_Owed.Find(id);
            db.CR_Cas_Account_Tax_Owed.Remove(cR_Cas_Account_Tax_Owed);
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
