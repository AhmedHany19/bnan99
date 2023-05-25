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
    public class RenterReceiptListController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: RenterReceiptList
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
            List<CR_Cas_Renter_Lessor> RenterList = new List<CR_Cas_Renter_Lessor>();
            var cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(l => l.CR_Cas_Renter_Lessor_Code == LessorCode).Include(c => c.CR_Mas_Com_Lessor).Include(c => c.CR_Mas_Renter_Information).OrderByDescending(d=>d.CR_Cas_Renter_Lessor_Date_First_Interaction);
            foreach(var Renter in cR_Cas_Renter_Lessor)
            {
                var receipt = db.CR_Cas_Account_Receipt.FirstOrDefault(x => x.CR_Cas_Account_Receipt_Renter_Code == Renter.CR_Cas_Renter_Lessor_Id);
                if (receipt != null)
                {
                    CR_Cas_Renter_Lessor r = new CR_Cas_Renter_Lessor();
                    r.CR_Cas_Account_Receipt = Renter.CR_Cas_Account_Receipt;
                    r.CR_Cas_Account_Restrictions = Renter.CR_Cas_Account_Restrictions;
                    r.CR_Cas_Account_Transfers_Tenants = Renter.CR_Cas_Account_Transfers_Tenants;
                    r.CR_Cas_Renter_Admin_Membership_Code = Renter.CR_Cas_Renter_Admin_Membership_Code;
                    r.CR_Cas_Renter_Lessor_Balance = Renter.CR_Cas_Renter_Lessor_Balance;
                    r.CR_Cas_Renter_Lessor_Code = Renter.CR_Cas_Renter_Lessor_Code;
                    r.CR_Cas_Renter_Lessor_Contract_Number = Renter.CR_Cas_Renter_Lessor_Contract_Number;
                    r.CR_Cas_Renter_Lessor_Date_First_Interaction = Renter.CR_Cas_Renter_Lessor_Date_First_Interaction;
                    r.CR_Cas_Renter_Lessor_Date_Last_Interaction = Renter.CR_Cas_Renter_Lessor_Date_Last_Interaction;
                    r.CR_Cas_Renter_Lessor_Days = Renter.CR_Cas_Renter_Lessor_Days;
                    r.CR_Cas_Renter_Lessor_Id = Renter.CR_Cas_Renter_Lessor_Id;
                    r.CR_Cas_Renter_Lessor_Interaction_Amount_Value = Renter.CR_Cas_Renter_Lessor_Interaction_Amount_Value;
                    r.CR_Cas_Renter_Lessor_KM = Renter.CR_Cas_Renter_Lessor_KM;
                    r.CR_Cas_Renter_Membership_Code = Renter.CR_Cas_Renter_Membership_Code;
                    r.CR_Cas_Renter_Rating = Renter.CR_Cas_Renter_Rating;
                    r.CR_Mas_Renter_Information = Renter.CR_Mas_Renter_Information;
                    r.CR_Mas_Com_Lessor = Renter.CR_Mas_Com_Lessor;
                    RenterList.Add(r);
                }
            }
            return View(RenterList);
        }



        public ActionResult GetRenterReceiptList(string id)
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
            var renter = db.CR_Cas_Renter_Lessor.FirstOrDefault(r=>r.CR_Cas_Renter_Lessor_Id==id && r.CR_Cas_Renter_Lessor_Code==LessorCode);
            if (renter != null)
            {
                ViewBag.RenterName = renter.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Ar_Name;
            }
           
            var ReceiptList = db.CR_Cas_Account_Receipt.Where(r=>r.CR_Cas_Account_Receipt_Renter_Code==id && r.CR_Cas_Account_Receipt_Lessor_Code==LessorCode);
            return View(ReceiptList);
        }

        public ActionResult RenterReceiptBalance()
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
            List<CR_Cas_Renter_Lessor> RenterList = new List<CR_Cas_Renter_Lessor>();
            var cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(l => l.CR_Cas_Renter_Lessor_Code == LessorCode && l.CR_Cas_Renter_Lessor_Balance != 0 && l.CR_Cas_Renter_Lessor_Status!="R").Include(c => c.CR_Mas_Com_Lessor).Include(c => c.CR_Mas_Renter_Information).OrderByDescending(d=>d.CR_Cas_Renter_Lessor_Date_Last_Interaction);
            foreach (var Renter in cR_Cas_Renter_Lessor)
            {
                var receipt = db.CR_Cas_Account_Receipt.FirstOrDefault(x => x.CR_Cas_Account_Receipt_Renter_Code == Renter.CR_Cas_Renter_Lessor_Id);
                if (receipt != null)
                {
                    CR_Cas_Renter_Lessor r = new CR_Cas_Renter_Lessor();
                    r.CR_Cas_Account_Receipt = Renter.CR_Cas_Account_Receipt;
                    r.CR_Cas_Account_Restrictions = Renter.CR_Cas_Account_Restrictions;
                    r.CR_Cas_Account_Transfers_Tenants = Renter.CR_Cas_Account_Transfers_Tenants;
                    r.CR_Cas_Renter_Admin_Membership_Code = Renter.CR_Cas_Renter_Admin_Membership_Code;
                    r.CR_Cas_Renter_Lessor_Balance = Renter.CR_Cas_Renter_Lessor_Balance;
                    r.CR_Cas_Renter_Lessor_Code = Renter.CR_Cas_Renter_Lessor_Code;
                    r.CR_Cas_Renter_Lessor_Contract_Number = Renter.CR_Cas_Renter_Lessor_Contract_Number;
                    r.CR_Cas_Renter_Lessor_Date_First_Interaction = Renter.CR_Cas_Renter_Lessor_Date_First_Interaction;
                    r.CR_Cas_Renter_Lessor_Date_Last_Interaction = Renter.CR_Cas_Renter_Lessor_Date_Last_Interaction;
                    r.CR_Cas_Renter_Lessor_Days = Renter.CR_Cas_Renter_Lessor_Days;
                    r.CR_Cas_Renter_Lessor_Id = Renter.CR_Cas_Renter_Lessor_Id;
                    r.CR_Cas_Renter_Lessor_Interaction_Amount_Value = Renter.CR_Cas_Renter_Lessor_Interaction_Amount_Value;
                    r.CR_Cas_Renter_Lessor_KM = Renter.CR_Cas_Renter_Lessor_KM;
                    r.CR_Cas_Renter_Membership_Code = Renter.CR_Cas_Renter_Membership_Code;
                    r.CR_Cas_Renter_Rating = Renter.CR_Cas_Renter_Rating;
                    r.CR_Mas_Renter_Information = Renter.CR_Mas_Renter_Information;
                    r.CR_Mas_Com_Lessor = Renter.CR_Mas_Com_Lessor;
                    RenterList.Add(r);
                }
            }
            return View(RenterList);
        }



        // GET: RenterReceiptList/Details/5
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

        // GET: RenterReceiptList/Create
        public ActionResult Create()
        {
            ViewBag.CR_Cas_Renter_Lessor_Id = new SelectList(db.CR_Mas_Renter_Information, "CR_Mas_Renter_Information_Id", "CR_Mas_Renter_Information_Sector");
            ViewBag.CR_Cas_Renter_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo");
            return View();
        }

        // POST: RenterReceiptList/Create
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

        // GET: RenterReceiptList/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Account_Receipt cR_Cas_Account_Receipt = db.CR_Cas_Account_Receipt.Find(id);
            if (cR_Cas_Account_Receipt == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.Date = string.Format("{0:yyyy-MM-dd}", cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_Date);
                if (cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_Is_Passing == "1")
                {
                    ViewBag.ReceiptStatus = "عهدة";
                }
                else if (cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_Is_Passing == "2")
                {
                    ViewBag.ReceiptStatus = "مرحل";
                }
                else if (cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_Is_Passing == "3")
                {
                    ViewBag.ReceiptStatus = "محجوز";
                }
                ViewBag.TransferDate = string.Format("{0:yyyy-MM-dd}", cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_Passing_Date);
                ViewBag.RefPassing = cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_Reference_Passing;

                var user = db.CR_Cas_User_Information.FirstOrDefault(x=>x.CR_Cas_User_Information_Id==cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_User_Passing);
                if (user != null)
                {
                    ViewBag.UserName = user.CR_Cas_User_Information_Ar_Name;
                }

                ViewBag.TransferDate = string.Format("{0:yyyy-MM-dd}", cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_Passing_Date);

                return View(cR_Cas_Account_Receipt);
            }
        }

        // POST: RenterReceiptList/Edit/5
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

        // GET: RenterReceiptList/Delete/5
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

        // POST: RenterReceiptList/Delete/5
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
