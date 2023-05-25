using RentCar.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace RentCar.Controllers.BranchSys
{
    public class Sys3RenterLessorController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: Sys3RenterLessor
        public ActionResult Index()
        {
            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                BranchCode = Session["BranchCode"].ToString();

                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null || BranchCode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }
            catch (Exception ex)
            {
                RedirectToAction("Login", "Account");
            }
            var cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(r=>r.CR_Cas_Renter_Lessor_Code==LessorCode).Include(c => c.CR_Mas_Com_Lessor).Include(c => c.CR_Mas_Renter_Information);
            return View(cR_Cas_Renter_Lessor.ToList());
        }

        public PartialViewResult PartialIndex(string type)
        {
            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                BranchCode = Session["BranchCode"].ToString();

                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null || BranchCode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }

            IQueryable<CR_Cas_Renter_Lessor> cR_Cas_Renter_Lessor = null;
            if (type == "All")
            {
                cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(l => l.CR_Cas_Renter_Lessor_Code == LessorCode)
                    .Include(c => c.CR_Mas_Com_Lessor)
                    .Include(c => c.CR_Mas_Renter_Information).OrderByDescending(o => o.CR_Cas_Renter_Lessor_Date_Last_Interaction);
            }
            else if (type == "K")
            {
                cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(l => l.CR_Cas_Renter_Lessor_Code == LessorCode && l.CR_Cas_Renter_Lessor_Status == "K")
                    .Include(c => c.CR_Mas_Com_Lessor)
                    .Include(c => c.CR_Mas_Renter_Information).OrderByDescending(o => o.CR_Cas_Renter_Lessor_Date_Last_Interaction);
            }
            else if (type == "R")
            {
                cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(l => l.CR_Cas_Renter_Lessor_Code == LessorCode && l.CR_Cas_Renter_Lessor_Status == "R")
                    .Include(c => c.CR_Mas_Com_Lessor)
                    .Include(c => c.CR_Mas_Renter_Information).OrderByDescending(o => o.CR_Cas_Renter_Lessor_Date_Last_Interaction);
            }
            else if (type == "A")
            {
                cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(l => l.CR_Cas_Renter_Lessor_Code == LessorCode && l.CR_Cas_Renter_Lessor_Status == "A")
                    .Include(c => c.CR_Mas_Com_Lessor)
                    .Include(c => c.CR_Mas_Renter_Information).OrderByDescending(o => o.CR_Cas_Renter_Lessor_Date_Last_Interaction);
            }
            else
            {
                cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(l => l.CR_Cas_Renter_Lessor_Code == LessorCode && (l.CR_Cas_Renter_Lessor_Status == "A" || l.CR_Cas_Renter_Lessor_Status == "R"))
                    .Include(c => c.CR_Mas_Com_Lessor)
                    .Include(c => c.CR_Mas_Renter_Information).OrderByDescending(o=>o.CR_Cas_Renter_Lessor_Date_Last_Interaction);
            }
            return PartialView(cR_Cas_Renter_Lessor);
        }

        // GET: Sys3RenterLessor/Details/5
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

        // GET: Sys3RenterLessor/Create
        public ActionResult Create()
        {
            ViewBag.CR_Cas_Renter_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo");
            ViewBag.CR_Cas_Renter_Lessor_Id = new SelectList(db.CR_Mas_Renter_Information, "CR_Mas_Renter_Information_Id", "CR_Mas_Renter_Information_Sector");
            return View();
        }

        // POST: Sys3RenterLessor/Create
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

            ViewBag.CR_Cas_Renter_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Code);
            ViewBag.CR_Cas_Renter_Lessor_Id = new SelectList(db.CR_Mas_Renter_Information, "CR_Mas_Renter_Information_Id", "CR_Mas_Renter_Information_Sector", cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Id);
            return View(cR_Cas_Renter_Lessor);
        }

        // GET: Sys3RenterLessor/Edit/5
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
            ViewBag.CR_Cas_Renter_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Code);
            ViewBag.CR_Cas_Renter_Lessor_Id = new SelectList(db.CR_Mas_Renter_Information, "CR_Mas_Renter_Information_Id", "CR_Mas_Renter_Information_Sector", cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Id);
            return View(cR_Cas_Renter_Lessor);
        }

        // POST: Sys3RenterLessor/Edit/5
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
            ViewBag.CR_Cas_Renter_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Code);
            ViewBag.CR_Cas_Renter_Lessor_Id = new SelectList(db.CR_Mas_Renter_Information, "CR_Mas_Renter_Information_Id", "CR_Mas_Renter_Information_Sector", cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Id);
            return View(cR_Cas_Renter_Lessor);
        }

        // GET: Sys3RenterLessor/Delete/5
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

        // POST: Sys3RenterLessor/Delete/5
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
