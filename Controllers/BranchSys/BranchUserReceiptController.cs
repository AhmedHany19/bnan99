using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentCar.Models;

namespace RentCar.Controllers
{
    public class BranchUserReceiptController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: BranchUserReceipt
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
                    RedirectToAction("Login", "Account");
                }
            }
            catch (Exception ex)
            {
                RedirectToAction("Login", "Account");
            }
            var sd = DateTime.Now.AddDays(-30);
            var ed = DateTime.Now;
            ViewBag.Sdate = string.Format("{0:yyyy-MM-dd}", sd);
            ViewBag.Edate = string.Format("{0:yyyy-MM-dd}", ed);
            
            var user = db.CR_Cas_User_Information.FirstOrDefault(u=>u.CR_Cas_User_Information_Id==UserLogin);
            if (user != null)
            {
                if (user.CR_Cas_User_Information_Balance==null)
                {
                    ViewBag.UserBalance = 0;
                }
                else
                {
                    var convertUserBalanceToFloat = (float)user.CR_Cas_User_Information_Balance;
                    var Receipt_Receipt = convertUserBalanceToFloat.ToString("N0");
                    ViewBag.UserBalance = Receipt_Receipt;
                }
               
            }

            var rpt = db.CR_Cas_Account_Receipt.Where(rp=>rp.CR_Cas_Account_Receipt_User_Code==UserLogin && rp.CR_Cas_Account_Receipt_Date>=sd &&
            rp.CR_Cas_Account_Receipt_Date<=ed && rp.CR_Cas_Account_Receipt_Payment_Method != "24");
            if (rpt != null)
            {
                var convertReceiptPaymentToFloat= (float) rpt.Select(m => m.CR_Cas_Account_Receipt_Payment).Sum();
                var Receipt_Payment = convertReceiptPaymentToFloat.ToString("N0");
                ViewBag.UserCreit = Receipt_Payment;

                var convertReceiptReceiptToFloat = (float)rpt.Select(m => m.CR_Cas_Account_Receipt_Receipt).Sum();
                var Receipt_Receipt = convertReceiptReceiptToFloat.ToString("N0");
                ViewBag.UserDebit = Receipt_Receipt;
            }
            return View();
        }

        public PartialViewResult Table(string type, string StartDate, string EndDate)
        {
            DateTime sd;
            DateTime sd1= DateTime.Now;
            DateTime ed;
            if (StartDate!=null && EndDate !=null)
            {
                sd = Convert.ToDateTime(StartDate);
                sd = sd.Date;
                ed = Convert.ToDateTime(EndDate);
                ed = ed.Date;
            }
            else
            {
                 
                sd1=sd1.AddDays(-30);
                sd = sd1.Date;
                ed = DateTime.Now.Date;
            }
            
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
                    RedirectToAction("Login", "Account");
                }
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }
            IQueryable<CR_Cas_Account_Receipt> Receipt = null;
            if (type == "All")
            {
                Receipt = db.CR_Cas_Account_Receipt.Where(r => r.CR_Cas_Account_Receipt_Lessor_Code == LessorCode && r.CR_Cas_Account_Receipt_Branch_Code == BranchCode &&
                r.CR_Cas_Account_Receipt_User_Code == UserLogin && r.CR_Cas_Account_Receipt_Payment_Method!="24" &&
                r.CR_Cas_Account_Receipt_Date >= sd && r.CR_Cas_Account_Receipt_Date <= ed);
               
            }
            else if (type=="C")
            {
                Receipt = db.CR_Cas_Account_Receipt.Where(r => r.CR_Cas_Account_Receipt_Lessor_Code == LessorCode && r.CR_Cas_Account_Receipt_Branch_Code == BranchCode &&
                r.CR_Cas_Account_Receipt_Is_Passing == "1" && r.CR_Cas_Account_Receipt_User_Code == UserLogin && r.CR_Cas_Account_Receipt_Payment_Method != "24" &&
                r.CR_Cas_Account_Receipt_Date >= sd && r.CR_Cas_Account_Receipt_Date <= ed);
                
            }
            else if(type=="B")
            {
                Receipt = db.CR_Cas_Account_Receipt.Where(r => r.CR_Cas_Account_Receipt_Lessor_Code == LessorCode && r.CR_Cas_Account_Receipt_Branch_Code == BranchCode &&
                r.CR_Cas_Account_Receipt_Is_Passing == "3" && r.CR_Cas_Account_Receipt_User_Code == UserLogin && r.CR_Cas_Account_Receipt_Payment_Method != "24" &&
                r.CR_Cas_Account_Receipt_Date >= sd && r.CR_Cas_Account_Receipt_Date <= ed);
                
            }
            else if (type == "T")
            {
                Receipt = db.CR_Cas_Account_Receipt.Where(r => r.CR_Cas_Account_Receipt_Lessor_Code == LessorCode && r.CR_Cas_Account_Receipt_Branch_Code == BranchCode &&
                r.CR_Cas_Account_Receipt_Is_Passing == "2" && r.CR_Cas_Account_Receipt_User_Code == UserLogin && r.CR_Cas_Account_Receipt_Payment_Method != "24" &&
                r.CR_Cas_Account_Receipt_Date >= sd && r.CR_Cas_Account_Receipt_Date <= ed);
               
            }
            else if (type == "Date")
            {
                Receipt = db.CR_Cas_Account_Receipt.Where(r => r.CR_Cas_Account_Receipt_Lessor_Code == LessorCode && r.CR_Cas_Account_Receipt_Branch_Code == BranchCode &&
                r.CR_Cas_Account_Receipt_User_Code == UserLogin && r.CR_Cas_Account_Receipt_Payment_Method != "24" &&
                r.CR_Cas_Account_Receipt_Date >= sd && r.CR_Cas_Account_Receipt_Date <= ed);
               
            }
            else
            {
                Receipt = db.CR_Cas_Account_Receipt.Where(r => r.CR_Cas_Account_Receipt_Lessor_Code == LessorCode && r.CR_Cas_Account_Receipt_Branch_Code == BranchCode
                && r.CR_Cas_Account_Receipt_User_Code == UserLogin && r.CR_Cas_Account_Receipt_Payment_Method != "24" &&
                r.CR_Cas_Account_Receipt_Date >= sd1 && r.CR_Cas_Account_Receipt_Date <= ed);
               
            }


            return PartialView(Receipt.OrderBy(x=>x.CR_Cas_Account_Receipt_Date));
        }


        public JsonResult GetReceiptSum(string type, string StartDate, string EndDate)
        {
            DateTime sd;
            DateTime sd1 = DateTime.Now;
            DateTime ed;
            var UserCreit=0;
            var UserDebit = 0;
            if (StartDate != null && EndDate != null)
            {
                sd = Convert.ToDateTime(StartDate);
                sd = sd.Date;
                ed = Convert.ToDateTime(EndDate);
                ed = ed.Date;
            }
            else
            {

                sd1 = sd1.AddDays(-30);
                sd = sd1.Date;
                ed = DateTime.Now.Date;
            }

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
                    RedirectToAction("Login", "Account");
                }
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }
            IQueryable<CR_Cas_Account_Receipt> Receipt = null;
            if (type == "All")
            {
                Receipt = db.CR_Cas_Account_Receipt.Where(r => r.CR_Cas_Account_Receipt_Lessor_Code == LessorCode && r.CR_Cas_Account_Receipt_Branch_Code == BranchCode &&
                r.CR_Cas_Account_Receipt_User_Code == UserLogin && r.CR_Cas_Account_Receipt_Payment_Method != "24" &&
                r.CR_Cas_Account_Receipt_Date >= sd && r.CR_Cas_Account_Receipt_Date <= ed);
                if (Receipt != null && Receipt.Count()>0)
                {
                    UserCreit = (int)Receipt.Select(m => m.CR_Cas_Account_Receipt_Payment).Sum();
                    UserDebit = (int)Receipt.Select(m => m.CR_Cas_Account_Receipt_Receipt).Sum();
                }
                
            }
            else if (type == "C")
            {
                Receipt = db.CR_Cas_Account_Receipt.Where(r => r.CR_Cas_Account_Receipt_Lessor_Code == LessorCode && r.CR_Cas_Account_Receipt_Branch_Code == BranchCode &&
                r.CR_Cas_Account_Receipt_Is_Passing == "1" && r.CR_Cas_Account_Receipt_User_Code == UserLogin && r.CR_Cas_Account_Receipt_Payment_Method != "24" &&
                r.CR_Cas_Account_Receipt_Date >= sd && r.CR_Cas_Account_Receipt_Date <= ed);
                if (Receipt != null && Receipt.Count() > 0)
                {
                    UserCreit = (int)Receipt.Select(m => m.CR_Cas_Account_Receipt_Payment).Sum();
                    UserDebit = (int)Receipt.Select(m => m.CR_Cas_Account_Receipt_Receipt).Sum();
                }
            }
            else if (type == "B")
            {
                Receipt = db.CR_Cas_Account_Receipt.Where(r => r.CR_Cas_Account_Receipt_Lessor_Code == LessorCode && r.CR_Cas_Account_Receipt_Branch_Code == BranchCode &&
                r.CR_Cas_Account_Receipt_Is_Passing == "3" && r.CR_Cas_Account_Receipt_User_Code == UserLogin && r.CR_Cas_Account_Receipt_Payment_Method != "24" &&
                r.CR_Cas_Account_Receipt_Date >= sd && r.CR_Cas_Account_Receipt_Date <= ed);
                if (Receipt != null && Receipt.Count() > 0)
                {
                    UserCreit = (int)Receipt.Select(m => m.CR_Cas_Account_Receipt_Payment).Sum();
                    UserDebit = (int)Receipt.Select(m => m.CR_Cas_Account_Receipt_Receipt).Sum();
                }
            }
            else if (type == "T")
            {
                Receipt = db.CR_Cas_Account_Receipt.Where(r => r.CR_Cas_Account_Receipt_Lessor_Code == LessorCode && r.CR_Cas_Account_Receipt_Branch_Code == BranchCode &&
                r.CR_Cas_Account_Receipt_Is_Passing == "2" && r.CR_Cas_Account_Receipt_User_Code == UserLogin && r.CR_Cas_Account_Receipt_Payment_Method != "24" &&
                r.CR_Cas_Account_Receipt_Date >= sd && r.CR_Cas_Account_Receipt_Date <= ed);
                if (Receipt != null && Receipt.Count() > 0)
                {
                    UserCreit = (int)Receipt.Select(m => m.CR_Cas_Account_Receipt_Payment).Sum();
                    UserDebit = (int)Receipt.Select(m => m.CR_Cas_Account_Receipt_Receipt).Sum();
                }
            }
            else if (type == "Date")
            {
                Receipt = db.CR_Cas_Account_Receipt.Where(r => r.CR_Cas_Account_Receipt_Lessor_Code == LessorCode && r.CR_Cas_Account_Receipt_Branch_Code == BranchCode &&
                r.CR_Cas_Account_Receipt_User_Code == UserLogin && r.CR_Cas_Account_Receipt_Payment_Method != "24" &&
                r.CR_Cas_Account_Receipt_Date >= sd && r.CR_Cas_Account_Receipt_Date <= ed);
                if (Receipt != null && Receipt.Count() > 0)
                {
                    UserCreit = (int)Receipt.Select(m => m.CR_Cas_Account_Receipt_Payment).Sum();
                    UserDebit = (int)Receipt.Select(m => m.CR_Cas_Account_Receipt_Receipt).Sum();
                }
            }
            else
            {
                Receipt = db.CR_Cas_Account_Receipt.Where(r => r.CR_Cas_Account_Receipt_Lessor_Code == LessorCode && r.CR_Cas_Account_Receipt_Branch_Code == BranchCode
                && r.CR_Cas_Account_Receipt_User_Code == UserLogin && r.CR_Cas_Account_Receipt_Payment_Method != "24" &&
                r.CR_Cas_Account_Receipt_Date >= sd1 && r.CR_Cas_Account_Receipt_Date <= ed);
                if (Receipt != null && Receipt.Count() > 0)
                {
                    UserCreit = (int)Receipt.Select(m => m.CR_Cas_Account_Receipt_Payment).Sum();
                    UserDebit = (int)Receipt.Select(m => m.CR_Cas_Account_Receipt_Receipt).Sum();
                }
            }
            return Json(UserCreit+ "/" + UserDebit, JsonRequestBehavior.AllowGet);
        }

        // GET: BranchUserReceipt/Details/5
        public ActionResult Details(string id)
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
            return View(cR_Cas_Account_Receipt);
        }

        // GET: BranchUserReceipt/Create
        public ActionResult Create()
        {
            ViewBag.CR_Cas_Account_Receipt_Renter_Code = new SelectList(db.CR_Cas_Renter_Lessor, "CR_Cas_Renter_Lessor_Id", "CR_Cas_Renter_Membership_Code");
            ViewBag.CR_Cas_Account_Receipt_Bank_Code = new SelectList(db.CR_Cas_Sup_Bank, "CR_Cas_Sup_Bank_Code", "CR_Cas_Sup_Bank_Com_Code");
            ViewBag.CR_Cas_Account_Receipt_Branch_Code = new SelectList(db.CR_Cas_Sup_Branch, "CR_Cas_Sup_Branch_Code", "CR_Cas_Sup_Branch_Ar_Name");
            ViewBag.CR_Cas_Account_Receipt_Car_Code = new SelectList(db.CR_Cas_Sup_Car_Information, "CR_Cas_Sup_Car_Serail_No", "CR_Cas_Sup_Car_Lessor_Code");
            ViewBag.CR_Cas_Account_Receipt_SalesPoint_No = new SelectList(db.CR_Cas_Sup_SalesPoint, "CR_Cas_Sup_SalesPoint_Code", "CR_Cas_Sup_SalesPoint_Com_Code");
            ViewBag.CR_Cas_Account_Receipt_User_Code = new SelectList(db.CR_Cas_User_Information, "CR_Cas_User_Information_Id", "CR_Cas_User_Information_PassWord");
            ViewBag.CR_Cas_Account_Receipt_Payment_Method = new SelectList(db.CR_Mas_Sup_Payment_Method, "CR_Mas_Sup_Payment_Method_Code", "CR_Mas_Sup_Payment_Method_Type");
            return View();
        }

        // POST: BranchUserReceipt/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Account_Receipt_No,CR_Cas_Account_Receipt_Year,CR_Cas_Account_Receipt_Type,CR_Cas_Account_Receipt_Lessor_Code,CR_Cas_Account_Receipt_Branch_Code,CR_Cas_Account_Receipt_Date,CR_Cas_Account_Receipt_Contract_Operation,CR_Cas_Account_Receipt_Reference_Type,CR_Cas_Account_Receipt_Payment,CR_Cas_Account_Receipt_Receipt,CR_Cas_Account_Receipt_Payment_Method,CR_Cas_Account_Receipt_Bank_Code,CR_Cas_Account_Receipt_SalesPoint_No,CR_Cas_Account_Receipt_SalesPoint_Previous_Balance,CR_Cas_Account_Receipt_Renter_Code,CR_Cas_Account_Receipt_Renter_Previous_Balance,CR_Cas_Account_Receipt_User_Code,CR_Cas_Account_Receipt_User_Previous_Balance,CR_Cas_Account_Receipt_Car_Code,CR_Cas_Account_Receipt_Is_Passing,CR_Cas_Account_Receipt_Passing_Date,CR_Cas_Account_Receipt_User_Passing,CR_Cas_Account_Receipt_Reference_Passing,CR_Cas_Account_Receipt_Status,CR_Cas_Account_Receipt_Reasons")] CR_Cas_Account_Receipt cR_Cas_Account_Receipt)
        {
            if (ModelState.IsValid)
            {
                db.CR_Cas_Account_Receipt.Add(cR_Cas_Account_Receipt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CR_Cas_Account_Receipt_Renter_Code = new SelectList(db.CR_Cas_Renter_Lessor, "CR_Cas_Renter_Lessor_Id", "CR_Cas_Renter_Membership_Code", cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_Renter_Code);
            ViewBag.CR_Cas_Account_Receipt_Bank_Code = new SelectList(db.CR_Cas_Sup_Bank, "CR_Cas_Sup_Bank_Code", "CR_Cas_Sup_Bank_Com_Code", cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_Bank_Code);
            ViewBag.CR_Cas_Account_Receipt_Branch_Code = new SelectList(db.CR_Cas_Sup_Branch, "CR_Cas_Sup_Branch_Code", "CR_Cas_Sup_Branch_Ar_Name", cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_Branch_Code);
            ViewBag.CR_Cas_Account_Receipt_Car_Code = new SelectList(db.CR_Cas_Sup_Car_Information, "CR_Cas_Sup_Car_Serail_No", "CR_Cas_Sup_Car_Lessor_Code", cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_Car_Code);
            ViewBag.CR_Cas_Account_Receipt_SalesPoint_No = new SelectList(db.CR_Cas_Sup_SalesPoint, "CR_Cas_Sup_SalesPoint_Code", "CR_Cas_Sup_SalesPoint_Com_Code", cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_SalesPoint_No);
            ViewBag.CR_Cas_Account_Receipt_User_Code = new SelectList(db.CR_Cas_User_Information, "CR_Cas_User_Information_Id", "CR_Cas_User_Information_PassWord", cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_User_Code);
            ViewBag.CR_Cas_Account_Receipt_Payment_Method = new SelectList(db.CR_Mas_Sup_Payment_Method, "CR_Mas_Sup_Payment_Method_Code", "CR_Mas_Sup_Payment_Method_Type", cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_Payment_Method);
            return View(cR_Cas_Account_Receipt);
        }

        // GET: BranchUserReceipt/Edit/5
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
                }else if (cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_Is_Passing == "2")
                {
                    ViewBag.ReceiptStatus = "مرحل";
                }
                else if (cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_Is_Passing == "3")
                {
                    ViewBag.ReceiptStatus = "محجوز";
                }
                ViewBag.TransferDate = string.Format("{0:yyyy/MM/dd}", cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_Passing_Date);
                ViewBag.RefPassing = cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_Reference_Passing;
                var users = db.CR_Cas_User_Information.FirstOrDefault(u => u.CR_Cas_User_Information_Id == cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_User_Passing);
                if (users != null)
                {
                    ViewBag.UserReceipt = users.CR_Cas_User_Information_Ar_Name;
                }
               // ViewBag.TransferDate =string.Format("{0:yyyy-MM-dd}", cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_Passing_Date);
            }
            return View(cR_Cas_Account_Receipt);
        }

        // POST: BranchUserReceipt/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Account_Receipt_No,CR_Cas_Account_Receipt_Year,CR_Cas_Account_Receipt_Type,CR_Cas_Account_Receipt_Lessor_Code,CR_Cas_Account_Receipt_Branch_Code,CR_Cas_Account_Receipt_Date,CR_Cas_Account_Receipt_Contract_Operation,CR_Cas_Account_Receipt_Reference_Type,CR_Cas_Account_Receipt_Payment,CR_Cas_Account_Receipt_Receipt,CR_Cas_Account_Receipt_Payment_Method,CR_Cas_Account_Receipt_Bank_Code,CR_Cas_Account_Receipt_SalesPoint_No,CR_Cas_Account_Receipt_SalesPoint_Previous_Balance,CR_Cas_Account_Receipt_Renter_Code,CR_Cas_Account_Receipt_Renter_Previous_Balance,CR_Cas_Account_Receipt_User_Code,CR_Cas_Account_Receipt_User_Previous_Balance,CR_Cas_Account_Receipt_Car_Code,CR_Cas_Account_Receipt_Is_Passing,CR_Cas_Account_Receipt_Passing_Date,CR_Cas_Account_Receipt_User_Passing,CR_Cas_Account_Receipt_Reference_Passing,CR_Cas_Account_Receipt_Status,CR_Cas_Account_Receipt_Reasons")] CR_Cas_Account_Receipt cR_Cas_Account_Receipt)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cR_Cas_Account_Receipt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CR_Cas_Account_Receipt_Renter_Code = new SelectList(db.CR_Cas_Renter_Lessor, "CR_Cas_Renter_Lessor_Id", "CR_Cas_Renter_Membership_Code", cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_Renter_Code);
            ViewBag.CR_Cas_Account_Receipt_Bank_Code = new SelectList(db.CR_Cas_Sup_Bank, "CR_Cas_Sup_Bank_Code", "CR_Cas_Sup_Bank_Com_Code", cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_Bank_Code);
            ViewBag.CR_Cas_Account_Receipt_Branch_Code = new SelectList(db.CR_Cas_Sup_Branch, "CR_Cas_Sup_Branch_Code", "CR_Cas_Sup_Branch_Ar_Name", cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_Branch_Code);
            ViewBag.CR_Cas_Account_Receipt_Car_Code = new SelectList(db.CR_Cas_Sup_Car_Information, "CR_Cas_Sup_Car_Serail_No", "CR_Cas_Sup_Car_Lessor_Code", cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_Car_Code);
            ViewBag.CR_Cas_Account_Receipt_SalesPoint_No = new SelectList(db.CR_Cas_Sup_SalesPoint, "CR_Cas_Sup_SalesPoint_Code", "CR_Cas_Sup_SalesPoint_Com_Code", cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_SalesPoint_No);
            ViewBag.CR_Cas_Account_Receipt_User_Code = new SelectList(db.CR_Cas_User_Information, "CR_Cas_User_Information_Id", "CR_Cas_User_Information_PassWord", cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_User_Code);
            ViewBag.CR_Cas_Account_Receipt_Payment_Method = new SelectList(db.CR_Mas_Sup_Payment_Method, "CR_Mas_Sup_Payment_Method_Code", "CR_Mas_Sup_Payment_Method_Type", cR_Cas_Account_Receipt.CR_Cas_Account_Receipt_Payment_Method);
            return View(cR_Cas_Account_Receipt);
        }

        // GET: BranchUserReceipt/Delete/5
        public ActionResult Delete(string id)
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
            return View(cR_Cas_Account_Receipt);
        }

        // POST: BranchUserReceipt/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Cas_Account_Receipt cR_Cas_Account_Receipt = db.CR_Cas_Account_Receipt.Find(id);
            db.CR_Cas_Account_Receipt.Remove(cR_Cas_Account_Receipt);
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
