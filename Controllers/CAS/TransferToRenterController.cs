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
    public class TransferToRenterController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: TransferToRenter
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
            var cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(l => l.CR_Cas_Renter_Lessor_Code == LessorCode).Include(c => c.CR_Mas_Com_Lessor).Include(c => c.CR_Mas_Renter_Information);
            return View(cR_Cas_Renter_Lessor.ToList());
        }

        public PartialViewResult PartialView(string type)
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
            if (type == "D")
            {
                cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(l => l.CR_Cas_Renter_Lessor_Code == LessorCode && l.CR_Cas_Renter_Lessor_Status == "D")
                    .Include(c => c.CR_Mas_Com_Lessor)
                    .Include(c => c.CR_Mas_Renter_Information);
            }
            else if (type == "K")
            {
                cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(l => l.CR_Cas_Renter_Lessor_Code == LessorCode && l.CR_Cas_Renter_Lessor_Status == "K")
                    .Include(c => c.CR_Mas_Com_Lessor)
                    .Include(c => c.CR_Mas_Renter_Information);
            }
            else
            {
                cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(l => l.CR_Cas_Renter_Lessor_Code == LessorCode && (l.CR_Cas_Renter_Lessor_Status=="A" || l.CR_Cas_Renter_Lessor_Status=="K"))
                    .Include(c => c.CR_Mas_Com_Lessor)
                    .Include(c => c.CR_Mas_Renter_Information);
            }
            return PartialView(cR_Cas_Renter_Lessor);
        }

        // GET: TransferToRenter/Details/5
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


        public CR_Cas_Administrative_Procedures GetLastRecord(string Lessorcode, string ProcedureCode)
        {
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var Lrecord = db.CR_Cas_Administrative_Procedures.Where(x => x.CR_Cas_Administrative_Procedures_Lessor == Lessorcode
                && x.CR_Cas_Administrative_Procedures_Code == ProcedureCode && x.CR_Cas_Administrative_Procedures_Year == y)
                .Max(lr => lr.CR_Cas_Administrative_Procedures_No.Substring(lr.CR_Cas_Administrative_Procedures_No.Length - 7, 7));



            CR_Cas_Administrative_Procedures T = new CR_Cas_Administrative_Procedures();
            if (Lrecord != null)
            {
                Int64 val = Int64.Parse(Lrecord) + 1;
                T.CR_Cas_Administrative_Procedures_No = val.ToString("0000000");
            }
            else
            {
                T.CR_Cas_Administrative_Procedures_No = "0000001";
            }
            return T;
        }

        public CR_Cas_Account_Receipt GetReceiptLastRecord(string LessorCode, string BranchCode)
        {
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var Lrecord = db.CR_Cas_Account_Receipt.Where(x => x.CR_Cas_Account_Receipt_Lessor_Code == LessorCode
                && x.CR_Cas_Account_Receipt_Year == y && x.CR_Cas_Account_Receipt_Branch_Code == BranchCode)
                .Max(x => x.CR_Cas_Account_Receipt_No.Substring(x.CR_Cas_Account_Receipt_No.Length - 4, 4));

            CR_Cas_Account_Receipt c = new CR_Cas_Account_Receipt();
            if (Lrecord != null)
            {
                Int64 val = Int64.Parse(Lrecord) + 1;
                c.CR_Cas_Account_Receipt_No = val.ToString("0000");
            }
            else
            {
                c.CR_Cas_Account_Receipt_No = "0001";
            }

            return c;
        }

        public JsonResult GetAccountNumber(string code)
        {
            if (code != null)
            {
                var AccountNumber = db.CR_Cas_Sup_Bank.FirstOrDefault(b => b.CR_Cas_Sup_Bank_Code == code);
                return Json(AccountNumber.CR_Cas_Sup_Bank_Account_No, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }

        }

        public JsonResult GetBankName(string code)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<CR_Mas_Sup_Bank> banklist = db.CR_Cas_Sup_Bank.Where(b => b.CR_Cas_Sup_Bank_Code == code).Select(x => x.CR_Mas_Sup_Bank).ToList();
            return Json(banklist, JsonRequestBehavior.AllowGet);
        }


        // GET: TransferToRenter/Create
        public ActionResult Create(string id1,string id2)
        {
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var sector = "1";
            var LessorCode = Session["LessorCode"].ToString();
            var autoInc = GetLastRecord(LessorCode, "63");
            var renter = db.CR_Cas_Renter_Lessor.FirstOrDefault(r=>r.CR_Cas_Renter_Lessor_Id==id1 && r.CR_Cas_Renter_Lessor_Code==id2);
            if (renter != null)
            {
                ViewBag.Administrative_Procedures_No = y + "-" + sector + "-" + "63" + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
                ViewBag.TransferDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                
                //////ViewBag.CR_Cas_Sup_Bank_Mas_Code = new SelectList((from c in db.CR_Cas_Sup_Bank
                //////                                                   join m in db.CR_Mas_Sup_Bank on c.CR_Cas_Sup_Bank_Mas_Code equals m.CR_Mas_Sup_Bank_Code
                //////                                                   where c.CR_Cas_Sup_Bank_Status == "A" && c.CR_Cas_Sup_Bank_Mas_Code != "00" && c.CR_Cas_Sup_Bank_Com_Code == LessorCode
                //////                                                   select new
                //////                                                   {
                //////                                                       CR_Mas_Sup_Bank_Ar_Name = m.CR_Mas_Sup_Bank_Ar_Name + "/" + c.CR_Cas_Sup_Bank_Ar_Name,
                //////                                                       CR_Cas_Sup_Bank_Code = c.CR_Cas_Sup_Bank_Code

                //////                                                   }), "CR_Cas_Sup_Bank_Code", "CR_Mas_Sup_Bank_Ar_Name");
            }
            ViewBag.CR_Cas_Sup_Bank_Code = new SelectList(db.CR_Cas_Sup_Bank.Where(b=>b.CR_Cas_Sup_Bank_Com_Code==LessorCode && b.CR_Cas_Sup_Bank_Mas_Code!="00" && b.CR_Cas_Sup_Bank_Status=="A"), "CR_Cas_Sup_Bank_Code", "CR_Cas_Sup_Bank_Ar_Name");

            ViewBag.CR_Mas_Sup_Bank = new SelectList(db.CR_Mas_Sup_Bank.Where(b => b.CR_Mas_Sup_Bank_Status == "A" && b.CR_Mas_Sup_Bank_Code!="00"),
                "CR_Mas_Sup_Bank_Code", "CR_Mas_Sup_Bank_Ar_Name",renter.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Bank);

            ViewBag.IBAN = renter.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Iban;

            return View(renter);
        }

        // POST: TransferToRenter/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Renter_Lessor_Id,CR_Cas_Renter_Lessor_Code," +
            "CR_Cas_Renter_Lessor_Date_First_Interaction,CR_Cas_Renter_Lessor_Date_Last_Interaction," +
            "CR_Cas_Renter_Lessor_Contract_Number,CR_Cas_Renter_Lessor_Days,CR_Cas_Renter_Lessor_Interaction_Amount_Value," +
            "CR_Cas_Renter_Lessor_KM,CR_Cas_Renter_Lessor_Balance,CR_Cas_Renter_Rating,CR_Cas_Renter_Membership_Code," +
            "CR_Cas_Renter_Admin_Membership_Code,CR_Cas_Renter_Lessor_Status,CR_Cas_Renter_Lessor_Reasons")] CR_Cas_Renter_Lessor cR_Cas_Renter_Lessor,string TransferedVal
            ,string Reasons,string CR_Mas_Sup_Bank, string Iban, string CR_Cas_Sup_Bank_Code)
        {
            var LessorCode = Session["LessorCode"].ToString();
            var BranchCode = Session["BranchCode"].ToString();
            var UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
            if (ModelState.IsValid)
            {
                
                ///////////////////////////////Tracing//////////////////////////////////////
                CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
                DateTime year = DateTime.Now;
                var y = year.ToString("yy");
                var sector = "1";
                var autoInc = GetLastRecord(LessorCode, "63");
                decimal val=0;

                Ad.CR_Cas_Administrative_Procedures_No = y + "-" + sector + "-" + "63" + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
                Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
                string currentTime = DateTime.Now.ToString("HH:mm:ss");
                Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
                Ad.CR_Cas_Administrative_Procedures_Year = y;
                Ad.CR_Cas_Administrative_Procedures_Sector = sector;
                Ad.CR_Cas_Administrative_Procedures_Code = "63";
                Ad.CR_Cas_Administrative_Int_Procedures_Code = 63;
                Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
                Ad.CR_Cas_Administrative_Procedures_Targeted_Action = cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Id;
                Ad.CR_Cas_Administrative_Procedures_User_Insert = Session["UserLogin"].ToString();
                Ad.CR_Cas_Administrative_Procedures_Type = "I";
                Ad.CR_Cas_Administrative_Procedures_Action = true;
                if(TransferedVal!=null && TransferedVal != "")
                {
                    val= decimal.Parse(TransferedVal);
                    Ad.CR_Cas_Administrative_Procedures_Value = decimal.Parse(TransferedVal);
                }
                Ad.CR_Cas_Administrative_Procedures_Action = true;
                Ad.CR_Cas_Administrative_Procedures_Doc_No = "";
                Ad.CR_Cas_Administrative_Procedures_Reasons = Reasons;
                db.CR_Cas_Administrative_Procedures.Add(Ad);


                CR_Cas_Account_Receipt Receipt = new CR_Cas_Account_Receipt();
                decimal PrevBalance = 0;
                var renter = db.CR_Cas_Renter_Lessor.FirstOrDefault(r=>r.CR_Cas_Renter_Lessor_Id==cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Id && r.CR_Cas_Renter_Lessor_Code==LessorCode);
                if (renter != null)
                {
                    PrevBalance = (decimal)renter.CR_Cas_Renter_Lessor_Balance;
                    renter.CR_Cas_Renter_Lessor_Balance +=val;    
                    db.Entry(renter).State = EntityState.Modified;
                }


                var MasRenter = db.CR_Mas_Renter_Information.Single(m=>m.CR_Mas_Renter_Information_Id==renter.CR_Cas_Renter_Lessor_Id);
                MasRenter.CR_Mas_Renter_Information_Iban = Iban;
                MasRenter.CR_Mas_Renter_Information_Bank = CR_Mas_Sup_Bank;
                db.Entry(MasRenter).State = EntityState.Modified;


                ////////////////////////////////////////////////
                var Sector = "1";
                var autoinc = GetReceiptLastRecord(LessorCode, BranchCode).CR_Cas_Account_Receipt_No;
                Receipt.CR_Cas_Account_Receipt_No = y + "-" + Sector + "-" + "61" + "-" + LessorCode + "-" + BranchCode + autoinc;
                Receipt.CR_Cas_Account_Receipt_Year = y;
                Receipt.CR_Cas_Account_Receipt_Type = "61";
                Receipt.CR_Cas_Account_Receipt_Lessor_Code = LessorCode;
                Receipt.CR_Cas_Account_Receipt_Branch_Code = BranchCode;
                Receipt.CR_Cas_Account_Receipt_Date = DateTime.Now;
                Receipt.CR_Cas_Account_Receipt_Contract_Operation = Ad.CR_Cas_Administrative_Procedures_No;
                Receipt.CR_Cas_Account_Receipt_Payment = 0;
                Receipt.CR_Cas_Account_Receipt_Receipt = val;
                
                Receipt.CR_Cas_Account_Receipt_Payment_Method = "24";
                Receipt.CR_Cas_Account_Receipt_Renter_Code = cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Id;
                Receipt.CR_Cas_Account_Receipt_Reference_Type = "تحويل";
                Receipt.CR_Cas_Account_Receipt_User_Code = UserLogin;
                Receipt.CR_Cas_Account_Receipt_Renter_Previous_Balance = PrevBalance;
                Receipt.CR_Cas_Account_Receipt_SalesPoint_Previous_Balance = 0;
                Receipt.CR_Cas_Account_Receipt_User_Previous_Balance = 0;
                Receipt.CR_Cas_Account_Receipt_Bank_Code = CR_Cas_Sup_Bank_Code;
                Receipt.CR_Cas_Account_Receipt_Status = "A";
                Receipt.CR_Cas_Account_Receipt_Reasons = Reasons;
                db.CR_Cas_Account_Receipt.Add(Receipt);
                ////////////////////////////////////////////////


                db.SaveChanges();
                TempData["TempModel"] = "Added";
                return RedirectToAction("Index");
            }

            //////ViewBag.CR_Cas_Sup_Bank_Mas_Code = new SelectList((from c in db.CR_Cas_Sup_Bank
            //////                                                   join m in db.CR_Mas_Sup_Bank on c.CR_Cas_Sup_Bank_Mas_Code equals m.CR_Mas_Sup_Bank_Code
            //////                                                   where c.CR_Cas_Sup_Bank_Status == "A" && c.CR_Cas_Sup_Bank_Mas_Code != "00" && c.CR_Cas_Sup_Bank_Com_Code == LessorCode
            //////                                                   select new
            //////                                                   {
            //////                                                       CR_Mas_Sup_Bank_Ar_Name = m.CR_Mas_Sup_Bank_Ar_Name + "/" + c.CR_Cas_Sup_Bank_Ar_Name,
            //////                                                       CR_Cas_Sup_Bank_Code = c.CR_Cas_Sup_Bank_Code

            ////                                                   //}), "CR_Cas_Sup_Bank_Code", "CR_Mas_Sup_Bank_Ar_Name");

            ViewBag.CR_Mas_Sup_Bank = new SelectList(db.CR_Mas_Sup_Bank.Where(b => b.CR_Mas_Sup_Bank_Status == "A" && b.CR_Mas_Sup_Bank_Code!="00"),
                "CR_Mas_Sup_Bank_Code", "CR_Mas_Sup_Bank_Ar_Name");
            ViewBag.CR_Cas_Sup_Bank_Code = new SelectList(db.CR_Cas_Sup_Bank.Where(b => b.CR_Cas_Sup_Bank_Com_Code == LessorCode && b.CR_Cas_Sup_Bank_Mas_Code != "00"),
                "CR_Cas_Sup_Bank_Code", "CR_Cas_Sup_Bank_Ar_Name");

           
            return View(cR_Cas_Renter_Lessor);
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
