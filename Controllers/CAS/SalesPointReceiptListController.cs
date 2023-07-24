using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentCar.Models;
using RentCar.Models.CAS;

namespace RentCar.Controllers.CAS
{
    public class SalesPointReceiptListController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: SalesPointReceiptList
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

            List<SalesPointReceiptMD> SalesPointList = new List<SalesPointReceiptMD>();
            var cR_Cas_Sup_SalesPoint = db.CR_Cas_Sup_SalesPoint.Where(s=>s.CR_Cas_Sup_SalesPoint_Com_Code==LessorCode)
                .Include(c => c.CR_Cas_Sup_Bank)
                .Include(c => c.CR_Cas_Sup_Branch)
                .Include(c => c.CR_Mas_Com_Lessor);

            foreach(var SP in cR_Cas_Sup_SalesPoint)
            {
                var Receipt = db.CR_Cas_Account_Receipt;
                if (Receipt != null)
                {
                    SalesPointReceiptMD SalesPoint = new SalesPointReceiptMD();
                    SalesPoint.CR_Cas_Sup_SalesPoint_Code = SP.CR_Cas_Sup_SalesPoint_Code;
                    SalesPoint.CR_Cas_Sup_SalesPoint_Com_Code = SP.CR_Cas_Sup_SalesPoint_Com_Code;
                    SalesPoint.CR_Cas_Sup_SalesPoint_Brn_Code = SP.CR_Cas_Sup_SalesPoint_Brn_Code;
                    SalesPoint.CR_Cas_Sup_SalesPoint_Bank_Code = SP.CR_Cas_Sup_SalesPoint_Bank_Code;
                    SalesPoint.CR_Cas_Sup_SalesPoint_Bank_No = SP.CR_Cas_Sup_SalesPoint_Bank_No;
                    SalesPoint.CR_Cas_Sup_SalesPoint_Ar_Name = SP.CR_Cas_Sup_SalesPoint_Ar_Name;
                    SalesPoint.CR_Cas_Sup_SalesPoint_En_Name = SP.CR_Cas_Sup_SalesPoint_En_Name;
                    SalesPoint.CR_Cas_Sup_SalesPoint_Balance = SP.CR_Cas_Sup_SalesPoint_Balance;
                    SalesPoint.CR_Cas_Sup_SalesPoint_Status = SP.CR_Cas_Sup_SalesPoint_Status;
                    SalesPoint.CR_Cas_Sup_SalesPoint_Reasons = SP.CR_Cas_Sup_SalesPoint_Reasons;
                    SalesPoint.CR_Cas_Sup_Branch = SP.CR_Cas_Sup_Branch;
                    SalesPoint.CR_Cas_Sup_Bank = SP.CR_Cas_Sup_Bank;
                    var CountCredit = db.CR_Cas_Account_Receipt.Where(r=>r.CR_Cas_Account_Receipt_SalesPoint_No==SP.CR_Cas_Sup_SalesPoint_Code && r.CR_Cas_Account_Receipt_Type=="60").Count();
                    SalesPoint.ReceiptCreditCount = CountCredit;
                    var CountDebit = db.CR_Cas_Account_Receipt.Where(r => r.CR_Cas_Account_Receipt_SalesPoint_No == SP.CR_Cas_Sup_SalesPoint_Code && r.CR_Cas_Account_Receipt_Type == "61").Count();
                    SalesPoint.ReceiptDebitCount = CountDebit;
                    SalesPointList.Add(SalesPoint);
                }
           
            }
            return View(SalesPointList.OrderByDescending(l=>l.ReceiptCreditCount).ToList());
        }

        public ActionResult SalesPointMovementSheet(string id)
        {
            var StartDate = DateTime.Now;
            var EndDate = DateTime.Now.AddDays(-1000);

           /* var receipt = db.CR_Cas_Account_Receipt
                .Where(r => r.CR_Cas_Account_Receipt_SalesPoint_No == id && r.CR_Cas_Account_Receipt_Date <= StartDate && r.CR_Cas_Account_Receipt_Date >= EndDate)
                .Include(r => r.CR_Mas_Sup_Payment_Method)
                .Include(r => r.CR_Cas_Sup_SalesPoint)
                .Include(r => r.CR_Cas_User_Information)
                .Include(r => r.CR_Cas_Sup_Branch);*/

            var salespoint = db.CR_Cas_Sup_SalesPoint.FirstOrDefault(s=>s.CR_Cas_Sup_SalesPoint_Code==id);
            if (salespoint != null)
            {
                ViewBag.SalesPointName = salespoint.CR_Cas_Sup_SalesPoint_Ar_Name;
                ViewBag.SalesPointBalance = salespoint.CR_Cas_Sup_SalesPoint_Balance;
                var sd = DateTime.Now.AddDays(-30);
                var ed = DateTime.Now;
                ViewBag.StartDate = string.Format("{0:yyyy-MM-dd}", sd);
                ViewBag.EndDate = string.Format("{0:yyyy-MM-dd}", ed);
                ViewBag.id = id;
            }
            return View();
        }
        
        public PartialViewResult table(string id, DateTime? startDate, DateTime? endDate)
        {
            IQueryable<CR_Cas_Account_Receipt> receipt = null;

            if (startDate != null && endDate != null)
            {
                 receipt = db.CR_Cas_Account_Receipt
               .Where(r => r.CR_Cas_Account_Receipt_SalesPoint_No == id && r.CR_Cas_Account_Receipt_Date <= endDate && r.CR_Cas_Account_Receipt_Date >= startDate)
               .Include(r => r.CR_Mas_Sup_Payment_Method)
               .Include(r => r.CR_Cas_Sup_SalesPoint)
               .Include(r => r.CR_Cas_User_Information)
               .Include(r => r.CR_Cas_Sup_Branch);
            }
            else
            {
                receipt = db.CR_Cas_Account_Receipt
               .Where(r => r.CR_Cas_Account_Receipt_SalesPoint_No == id)
               .Include(r => r.CR_Mas_Sup_Payment_Method)
               .Include(r => r.CR_Cas_Sup_SalesPoint)
               .Include(r => r.CR_Cas_User_Information)
               .Include(r => r.CR_Cas_Sup_Branch);
            }



            return PartialView(receipt);
        }

        

        // GET: SalesPointReceiptList/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_SalesPoint cR_Cas_Sup_SalesPoint = db.CR_Cas_Sup_SalesPoint.Find(id);
            if (cR_Cas_Sup_SalesPoint == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Sup_SalesPoint);
        }

        // GET: SalesPointReceiptList/Create
        public ActionResult Create()
        {
            ViewBag.CR_Cas_Sup_SalesPoint_Bank_Code = new SelectList(db.CR_Cas_Sup_Bank, "CR_Cas_Sup_Bank_Code", "CR_Cas_Sup_Bank_Com_Code");
            ViewBag.CR_Cas_Sup_SalesPoint_Brn_Code = new SelectList(db.CR_Cas_Sup_Branch, "CR_Cas_Sup_Branch_Code", "CR_Cas_Sup_Branch_Ar_Name");
            ViewBag.CR_Cas_Sup_SalesPoint_Com_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo");
            return View();
        }

        // POST: SalesPointReceiptList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Sup_SalesPoint_Code,CR_Cas_Sup_SalesPoint_Com_Code,CR_Cas_Sup_SalesPoint_Brn_Code,CR_Cas_Sup_SalesPoint_Bank_Code,CR_Cas_Sup_SalesPoint_Bank_No,CR_Cas_Sup_SalesPoint_Ar_Name,CR_Cas_Sup_SalesPoint_En_Name,CR_Cas_Sup_SalesPoint_Balance,CR_Cas_Sup_SalesPoint_Status,CR_Cas_Sup_SalesPoint_Reasons")] CR_Cas_Sup_SalesPoint cR_Cas_Sup_SalesPoint)
        {
            if (ModelState.IsValid)
            {
                db.CR_Cas_Sup_SalesPoint.Add(cR_Cas_Sup_SalesPoint);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CR_Cas_Sup_SalesPoint_Bank_Code = new SelectList(db.CR_Cas_Sup_Bank, "CR_Cas_Sup_Bank_Code", "CR_Cas_Sup_Bank_Com_Code", cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Bank_Code);
            ViewBag.CR_Cas_Sup_SalesPoint_Brn_Code = new SelectList(db.CR_Cas_Sup_Branch, "CR_Cas_Sup_Branch_Code", "CR_Cas_Sup_Branch_Ar_Name", cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Brn_Code);
            ViewBag.CR_Cas_Sup_SalesPoint_Com_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Com_Code);
            return View(cR_Cas_Sup_SalesPoint);
        }

        // GET: SalesPointReceiptList/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_SalesPoint cR_Cas_Sup_SalesPoint = db.CR_Cas_Sup_SalesPoint.Find(id);
            if (cR_Cas_Sup_SalesPoint == null)
            {
                return HttpNotFound();
            }
            ViewBag.CR_Cas_Sup_SalesPoint_Bank_Code = new SelectList(db.CR_Cas_Sup_Bank, "CR_Cas_Sup_Bank_Code", "CR_Cas_Sup_Bank_Com_Code", cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Bank_Code);
            ViewBag.CR_Cas_Sup_SalesPoint_Brn_Code = new SelectList(db.CR_Cas_Sup_Branch, "CR_Cas_Sup_Branch_Code", "CR_Cas_Sup_Branch_Ar_Name", cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Brn_Code);
            ViewBag.CR_Cas_Sup_SalesPoint_Com_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Com_Code);
            return View(cR_Cas_Sup_SalesPoint);
        }

        // POST: SalesPointReceiptList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Sup_SalesPoint_Code,CR_Cas_Sup_SalesPoint_Com_Code,CR_Cas_Sup_SalesPoint_Brn_Code,CR_Cas_Sup_SalesPoint_Bank_Code,CR_Cas_Sup_SalesPoint_Bank_No,CR_Cas_Sup_SalesPoint_Ar_Name,CR_Cas_Sup_SalesPoint_En_Name,CR_Cas_Sup_SalesPoint_Balance,CR_Cas_Sup_SalesPoint_Status,CR_Cas_Sup_SalesPoint_Reasons")] CR_Cas_Sup_SalesPoint cR_Cas_Sup_SalesPoint)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cR_Cas_Sup_SalesPoint).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CR_Cas_Sup_SalesPoint_Bank_Code = new SelectList(db.CR_Cas_Sup_Bank, "CR_Cas_Sup_Bank_Code", "CR_Cas_Sup_Bank_Com_Code", cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Bank_Code);
            ViewBag.CR_Cas_Sup_SalesPoint_Brn_Code = new SelectList(db.CR_Cas_Sup_Branch, "CR_Cas_Sup_Branch_Code", "CR_Cas_Sup_Branch_Ar_Name", cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Brn_Code);
            ViewBag.CR_Cas_Sup_SalesPoint_Com_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Com_Code);
            return View(cR_Cas_Sup_SalesPoint);
        }

        // GET: SalesPointReceiptList/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_SalesPoint cR_Cas_Sup_SalesPoint = db.CR_Cas_Sup_SalesPoint.Find(id);
            if (cR_Cas_Sup_SalesPoint == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Sup_SalesPoint);
        }

        // POST: SalesPointReceiptList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Cas_Sup_SalesPoint cR_Cas_Sup_SalesPoint = db.CR_Cas_Sup_SalesPoint.Find(id);
            db.CR_Cas_Sup_SalesPoint.Remove(cR_Cas_Sup_SalesPoint);
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
