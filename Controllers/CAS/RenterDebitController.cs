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
    public class RenterDebitController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: RenterDebit
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
            var cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(r=>r.CR_Cas_Renter_Lessor_Code==LessorCode && r.CR_Cas_Renter_Lessor_Balance<0)
                .Include(c => c.CR_Mas_Renter_Information).Include(c => c.CR_Mas_Com_Lessor);
            return View(cR_Cas_Renter_Lessor.ToList());
        }

        // GET: RenterDebit/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Renter_Lessor cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Find(id);
            if (cR_Cas_Renter_Lessor == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Renter_Lessor);
        }

        // GET: RenterDebit/Create
        public ActionResult Create()
        {
            ViewBag.CR_Cas_Renter_Lessor_Id = new SelectList(db.CR_Mas_Renter_Information, "CR_Mas_Renter_Information_Id", "CR_Mas_Renter_Information_Sector");
            ViewBag.CR_Cas_Renter_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo");
            return View();
        }

        // POST: RenterDebit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Renter_Lessor_Id,CR_Cas_Renter_Lessor_Code,CR_Cas_Renter_Lessor_Date_First_Interaction,CR_Cas_Renter_Lessor_Date_Last_Interaction,CR_Cas_Renter_Lessor_Contract_Number,CR_Cas_Renter_Lessor_Days,CR_Cas_Renter_Lessor_Interaction_Amount_Value,CR_Cas_Renter_Lessor_KM,CR_Cas_Renter_Lessor_Balance,CR_Cas_Renter_Rating,CR_Cas_Renter_Membership_Code,CR_Cas_Renter_Admin_Membership_Code,CR_Cas_Renter_Lessor_Status,CR_Cas_Renter_Lessor_Reasons")] CR_Cas_Renter_Lessor cR_Cas_Renter_Lessor)
        {
            if (ModelState.IsValid)
            {
                db.CR_Cas_Renter_Lessor.Add(cR_Cas_Renter_Lessor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CR_Cas_Renter_Lessor_Id = new SelectList(db.CR_Mas_Renter_Information, "CR_Mas_Renter_Information_Id", "CR_Mas_Renter_Information_Sector", cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Id);
            ViewBag.CR_Cas_Renter_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Code);
            return View(cR_Cas_Renter_Lessor);
        }

        // GET: RenterDebit/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Renter_Lessor cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Find(id);
            if (cR_Cas_Renter_Lessor == null)
            {
                return HttpNotFound();
            }
            ViewBag.CR_Cas_Renter_Lessor_Id = new SelectList(db.CR_Mas_Renter_Information, "CR_Mas_Renter_Information_Id", "CR_Mas_Renter_Information_Sector", cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Id);
            ViewBag.CR_Cas_Renter_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Code);
            return View(cR_Cas_Renter_Lessor);
        }

        // POST: RenterDebit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Renter_Lessor_Id,CR_Cas_Renter_Lessor_Code,CR_Cas_Renter_Lessor_Date_First_Interaction,CR_Cas_Renter_Lessor_Date_Last_Interaction,CR_Cas_Renter_Lessor_Contract_Number,CR_Cas_Renter_Lessor_Days,CR_Cas_Renter_Lessor_Interaction_Amount_Value,CR_Cas_Renter_Lessor_KM,CR_Cas_Renter_Lessor_Balance,CR_Cas_Renter_Rating,CR_Cas_Renter_Membership_Code,CR_Cas_Renter_Admin_Membership_Code,CR_Cas_Renter_Lessor_Status,CR_Cas_Renter_Lessor_Reasons")] CR_Cas_Renter_Lessor cR_Cas_Renter_Lessor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cR_Cas_Renter_Lessor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CR_Cas_Renter_Lessor_Id = new SelectList(db.CR_Mas_Renter_Information, "CR_Mas_Renter_Information_Id", "CR_Mas_Renter_Information_Sector", cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Id);
            ViewBag.CR_Cas_Renter_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Code);
            return View(cR_Cas_Renter_Lessor);
        }

        // GET: RenterDebit/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Renter_Lessor cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Find(id);
            if (cR_Cas_Renter_Lessor == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Renter_Lessor);
        }

        // POST: RenterDebit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Cas_Renter_Lessor cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Find(id);
            db.CR_Cas_Renter_Lessor.Remove(cR_Cas_Renter_Lessor);
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
