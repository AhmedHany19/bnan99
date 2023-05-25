using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentCar.Models;

namespace RentCar.Controllers.CAS
{
    public class CasSalesPointController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: CasSalesPoint
        public ActionResult Index()
        {
            var cR_Cas_Sup_SalesPoint = db.CR_Cas_Sup_SalesPoint.Include(c => c.CR_Cas_Sup_Bank).Include(c => c.CR_Cas_Sup_Branch).Include(c => c.CR_Mas_Com_Lessor);
            return View(cR_Cas_Sup_SalesPoint.ToList());
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

            var CashPoint = LessorCode + "0000";
            IQueryable<CR_Cas_Sup_SalesPoint> cR_Cas_Sup_SalesPoint = null;
            if (type == "All")
            {
                cR_Cas_Sup_SalesPoint = db.CR_Cas_Sup_SalesPoint.Where(s=>s.CR_Cas_Sup_SalesPoint_Bank_Code!=CashPoint && s.CR_Cas_Sup_SalesPoint_Com_Code == LessorCode && s.CR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status=="A")
                    .Include(c => c.CR_Cas_Sup_Bank).Include(c => c.CR_Cas_Sup_Branch).Include(c => c.CR_Mas_Com_Lessor);

            }
            else if (type == "D")
            {
                cR_Cas_Sup_SalesPoint = db.CR_Cas_Sup_SalesPoint.Where(s=>s.CR_Cas_Sup_SalesPoint_Com_Code==LessorCode && s.CR_Cas_Sup_SalesPoint_Status=="D" && s.CR_Cas_Sup_SalesPoint_Bank_Code != CashPoint && s.CR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "A" )
                    .Include(c => c.CR_Cas_Sup_Bank).Include(c => c.CR_Cas_Sup_Branch).Include(c => c.CR_Mas_Com_Lessor);

            }
            else if (type == "H")
            {
                cR_Cas_Sup_SalesPoint = db.CR_Cas_Sup_SalesPoint.Where(s => s.CR_Cas_Sup_SalesPoint_Com_Code == LessorCode && s.CR_Cas_Sup_SalesPoint_Status == "H" && s.CR_Cas_Sup_SalesPoint_Bank_Code != CashPoint && s.CR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "A")
                    .Include(c => c.CR_Cas_Sup_Bank).Include(c => c.CR_Cas_Sup_Branch).Include(c => c.CR_Mas_Com_Lessor);

            }
            else
            {
                cR_Cas_Sup_SalesPoint = db.CR_Cas_Sup_SalesPoint.Where(s => s.CR_Cas_Sup_SalesPoint_Com_Code == LessorCode && s.CR_Cas_Sup_SalesPoint_Status == "A" && s.CR_Cas_Sup_SalesPoint_Bank_Code != CashPoint && s.CR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "A" )
                    .Include(c => c.CR_Cas_Sup_Bank).Include(c => c.CR_Cas_Sup_Branch).Include(c => c.CR_Mas_Com_Lessor);

            }

            return PartialView(cR_Cas_Sup_SalesPoint);
        }



        ////public JsonResult getsalespointlist(string code)
        ////{
        ////    var lessorcode = "";
        ////    var userlogin = "";
        ////    var branchcode = "";
        ////    try
        ////    {
        ////        lessorcode = Session["lessorcode"].ToString();
        ////        branchcode = Session["branchcode"].ToString();

        ////        userlogin = Session["userlogin"].ToString();
        ////        if (userlogin == null || lessorcode == null || branchcode == null)
        ////        {
        ////            RedirectToAction("account", "login");
        ////        }
        ////    }
        ////    catch
        ////    {
        ////        RedirectToAction("login", "account");
        ////    }

        ////    db.Configuration.ProxyCreationEnabled = false;
        ////    //List<CR_Mas_Sup_Bank> banklist = db.CR_Cas_Sup_Bank.Where(b => b.CR_Cas_Sup_Bank_Code == code).Select(x=>x.CR_Mas_Sup_Bank).ToList() ;

        ////    List<CR_Cas_Sup_Bank> banklist = db.CR_Cas_Sup_Bank.Where(b => b.CR_Cas_Sup_Bank_Mas_Code == code && b.CR_Cas_Sup_Bank_Status == "A" && b.CR_Cas_Sup_Bank_Com_Code==lessorcode).ToList();
        ////    return Json(banklist, JsonRequestBehavior.AllowGet);

        ////}
        


        public JsonResult GetAccountNumber(string code)
        {   
            if (code != null && code!="")
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
            List<CR_Mas_Sup_Bank> banklist = db.CR_Cas_Sup_Bank.Where(b => b.CR_Cas_Sup_Bank_Code == code).Select(x=>x.CR_Mas_Sup_Bank).ToList() ;
            return Json(banklist, JsonRequestBehavior.AllowGet);
        }


        // GET: CasSalesPoint/Details/5
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


        public CR_Cas_Administrative_Procedures GetLastRecordTracing(string ProcedureCode, string sector)
        {

            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var LessorCode = Session["LessorCode"].ToString();
            var Lrecord = db.CR_Cas_Administrative_Procedures.Where(x => x.CR_Cas_Administrative_Procedures_Lessor == LessorCode &&
                x.CR_Cas_Administrative_Procedures_Code == ProcedureCode
                && x.CR_Cas_Administrative_Procedures_Sector == sector
                && x.CR_Cas_Administrative_Procedures_Year == y)
                .Max(x => x.CR_Cas_Administrative_Procedures_No.Substring(x.CR_Cas_Administrative_Procedures_No.Length - 7, 7));

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


        private void SaveTracing(string LessorCode, string TargetedAction, string ProcedureType, string reasons)
        {
            CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var sector = "1";
            var ProcedureCode = "44";
            var autoInc = GetLastRecordTracing(ProcedureCode, "1");
            Ad.CR_Cas_Administrative_Procedures_No = y + "-" + sector + "-" + ProcedureCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
            Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
            Ad.CR_Cas_Administrative_Procedures_Year = y;
            Ad.CR_Cas_Administrative_Procedures_Sector = sector;
            Ad.CR_Cas_Administrative_Procedures_Code = ProcedureCode;
            Ad.CR_Cas_Administrative_Int_Procedures_Code = int.Parse(ProcedureCode);
            Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
            Ad.CR_Cas_Administrative_Procedures_Targeted_Action = TargetedAction;
            Ad.CR_Cas_Administrative_Procedures_User_Insert = Session["UserLogin"].ToString();
            Ad.CR_Cas_Administrative_Procedures_Type = ProcedureType;
            Ad.CR_Cas_Administrative_Procedures_Action = true;
            Ad.CR_Cas_Administrative_Procedures_Doc_Date = null;
            Ad.CR_Cas_Administrative_Procedures_Doc_Start_Date = null;
            Ad.CR_Cas_Administrative_Procedures_Doc_End_Date = null;
            Ad.CR_Cas_Administrative_Procedures_Doc_No = "";
            Ad.CR_Cas_Administrative_Procedures_Reasons = reasons;
            db.CR_Cas_Administrative_Procedures.Add(Ad);
        }

        // GET: CasSalesPoint/Create
        public ActionResult Create()
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
         
            ViewBag.CR_Cas_Sup_SalesPoint_Brn_Code = new SelectList(db.CR_Cas_Sup_Branch.Where(b=>b.CR_Cas_Sup_Lessor_Code==LessorCode && b.CR_Cas_Sup_Branch_Status=="A"),
                "CR_Cas_Sup_Branch_Code", "CR_Cas_Sup_Branch_Ar_Short_Name");

            ViewBag.CR_Cas_Sup_SalesPoint_Bank_Code = new SelectList(db.CR_Cas_Sup_Bank.Where(o => o.CR_Cas_Sup_Bank_Status != "D" && o.CR_Cas_Sup_Bank_Mas_Code != "00" && o.CR_Cas_Sup_Bank_Com_Code==LessorCode),
              "CR_Cas_Sup_Bank_Code", "CR_Cas_Sup_Bank_Ar_Name");

            //ViewBag.CR_Mas_Sup_Bank = new SelectList(db.CR_Cas_Sup_Bank.Where(o => o.CR_Cas_Sup_Bank_Status != "D" && o.CR_Cas_Sup_Bank_Mas_Code != "00").Select(x=>x.CR_Mas_Sup_Bank).Distinct(),
            //  "CR_Mas_Sup_Bank_Code", "CR_Mas_Sup_Bank_Ar_Name");


            ////ViewBag.CR_Cas_Sup_SalesPoint_Bank_Code = new SelectList(db.CR_Cas_Sup_Bank.Where(o => o.CR_Cas_Sup_Bank_Status != "D" &&
            ////o.CR_Cas_Sup_Bank_Mas_Code != "00" && o.CR_Cas_Sup_Bank_Com_Code == LessorCode),
            ////    "CR_Cas_Sup_Bank_Code", "CR_Cas_Sup_Bank_Ar_Name");


            return View();
        }

        public string GetLastRecord(string code,string BranchCode)
        {
            var LessorCode = Session["LessorCode"].ToString();
            var Lrecord = db.CR_Cas_Sup_SalesPoint.Where(b => b.CR_Cas_Sup_SalesPoint_Bank_Code == code &&
            b.CR_Cas_Sup_SalesPoint_Com_Code == LessorCode && b.CR_Cas_Sup_SalesPoint_Brn_Code==BranchCode)
                .Max(b => b.CR_Cas_Sup_SalesPoint_Code.Substring(b.CR_Cas_Sup_SalesPoint_Code.Length - 2, 2));

            string s = "";
            if (Lrecord != null)
            {
                Int64 val = Int64.Parse(Lrecord) + 1;
                s = val.ToString("00");
            }
            else
            {
                s = "01";
            }
            return s;
        }


        // POST: CasSalesPoint/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Sup_SalesPoint_Code,CR_Cas_Sup_SalesPoint_Com_Code," +
            "CR_Cas_Sup_SalesPoint_Brn_Code,CR_Cas_Sup_SalesPoint_Bank_Code,CR_Cas_Sup_SalesPoint_Bank_No," +
            "CR_Cas_Sup_SalesPoint_Ar_Name,CR_Cas_Sup_SalesPoint_En_Name,CR_Cas_Sup_SalesPoint_Balance," +
            "CR_Cas_Sup_SalesPoint_Status,CR_Cas_Sup_SalesPoint_Reasons")] CR_Cas_Sup_SalesPoint cR_Cas_Sup_SalesPoint,string CR_Cas_Sup_Bank_Account_No,string CR_Mas_Sup_Bank)
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
                RedirectToAction("Login", "Account");
            }
            var SPnumber = db.CR_Cas_Sup_SalesPoint.Any(s=>s.CR_Cas_Sup_SalesPoint_Bank_No==cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Bank_No && s.CR_Cas_Sup_SalesPoint_Com_Code==LessorCode);
            var ArName = db.CR_Cas_Sup_SalesPoint.Any(s=>s.CR_Cas_Sup_SalesPoint_Ar_Name==cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Ar_Name && s.CR_Cas_Sup_SalesPoint_Com_Code == LessorCode);
            var EngName = db.CR_Cas_Sup_SalesPoint.Any(s=>s.CR_Cas_Sup_SalesPoint_En_Name==cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_En_Name && s.CR_Cas_Sup_SalesPoint_Com_Code == LessorCode);
            if (ModelState.IsValid && !SPnumber && !ArName && !EngName && cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Ar_Name!=null && cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Bank_Code!=null
                && cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Bank_No !=null && cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_En_Name!=null)
            {
                
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        
                        ////////////////////////////////////Documentation////////////////////////////////////////
                        var AutoInc = GetLastRecord(cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Bank_Code,cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Brn_Code);

                        var concat = LessorCode + cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Brn_Code + cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Bank_Code + AutoInc;

                        cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Code = concat;
                        cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Com_Code = LessorCode;
                        cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status = "A";
                        cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Balance = 0;

                        var CasBank = db.CR_Cas_Sup_Bank.FirstOrDefault(cb => cb.CR_Cas_Sup_Bank_Code == cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Bank_Code);
                        if (CasBank != null)
                        {
                            //var MasBank = db.CR_Mas_Sup_Bank.FirstOrDefault(mb => mb.CR_Mas_Sup_Bank_Code == CasBank.CR_Cas_Sup_Bank_Mas_Code);
                            //if (MasBank != null)
                            //{
                                cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Ar_Name =cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Ar_Name;
                                cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_En_Name = cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_En_Name;
                            //}
                        }
                        ///////////////////////////////Tracing//////////////////////////////////////
                        SaveTracing(LessorCode, cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Code, "I", cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Reasons);
                        db.CR_Cas_Sup_SalesPoint.Add(cR_Cas_Sup_SalesPoint);
                        db.SaveChanges();
                        dbTran.Commit();
                        TempData["TempModel"] = "Added";
                        return RedirectToAction("Index");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }
            }

            if (SPnumber)
            {
                ViewBag.SPnumber = "عفوا رقم نقطة البيع متكرر";
            }
            if (ArName)
            {
                ViewBag.ArName = "عفوا إسم نقطة البيع عربي متكرر";
            }
            if (EngName)
            {
                ViewBag.EngName = "عفوا إسم نقطة البيع إنجليزي متكرر";
            }
            if(cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Ar_Name == null)
            {
                ViewBag.ArName = "عفوا هذا الحقل إجباري";
            }
            if (cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_En_Name == null)
            {
                ViewBag.EngName = "عفوا هذا الحقل إجباري";
            }
            if (cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Bank_No == null)
            {
                ViewBag.SPnumber = "عفوا هذا الحقل إجباري";
            }
            if (cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Bank_Code == null)
            {
                ViewBag.BankCode= "عفوا هذا الحقل إجباري";
            }

            ViewBag.Account_No = CR_Cas_Sup_Bank_Account_No;

            ////ViewBag.CR_Cas_Sup_SalesPoint_Bank_Code = new SelectList(db.CR_Cas_Sup_Bank.Where(o => o.CR_Cas_Sup_Bank_Status != "D" && o.CR_Cas_Sup_Bank_Mas_Code != "00"),
            ////    "CR_Cas_Sup_Bank_Code", "CR_Cas_Sup_Bank_Ar_Name");
            ViewBag.CR_Cas_Sup_SalesPoint_Brn_Code = new SelectList(db.CR_Cas_Sup_Branch.Where(b => b.CR_Cas_Sup_Lessor_Code == LessorCode && b.CR_Cas_Sup_Branch_Status == "A"),
                "CR_Cas_Sup_Branch_Code", "CR_Cas_Sup_Branch_Ar_Short_Name");

            ViewBag.CR_Cas_Sup_SalesPoint_Bank_Code = new SelectList(db.CR_Cas_Sup_Bank.Where(o => o.CR_Cas_Sup_Bank_Status != "D" && o.CR_Cas_Sup_Bank_Mas_Code != "00"&& o.CR_Cas_Sup_Bank_Com_Code == LessorCode),
              "CR_Cas_Sup_Bank_Code", "CR_Cas_Sup_Bank_Ar_Name");
            ViewBag.CR_Mas_Sup_BankVal = "v";
            //ViewBag.CR_Mas_Sup_Bank = new SelectList(db.CR_Mas_Sup_Bank.Where(o => o.CR_Mas_Sup_Bank_Code == CR_Mas_Sup_Bank),
            //  "CR_Mas_Sup_Bank_Code", "CR_Mas_Sup_Bank_Ar_Name");
            return View(cR_Cas_Sup_SalesPoint);
        }

        // GET: CasSalesPoint/Edit/5
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
            CR_Cas_Sup_SalesPoint cR_Cas_Sup_SalesPoint = db.CR_Cas_Sup_SalesPoint.Find(id);
            if (cR_Cas_Sup_SalesPoint == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status == "A" ||
                    cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status == "Activated" ||
                    cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status == "1" ||
                    cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status == "Undeleted")
                {
                    ViewBag.stat = "حذف";
                    ViewBag.h = "إيقاف";
                }

                if (cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status == "D" ||
                    cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status == "Deleted" ||
                    cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status == "0")
                {
                    ViewBag.stat = "إسترجاع";
                    ViewBag.h = "إيقاف";
                    ViewData["ReadOnly"] = "true";
                }

                if (cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status == "H" ||
                    cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status == "Hold" ||
                    cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status == "2")
                {
                    ViewBag.h = "تنشيط";
                    ViewBag.stat = "حذف";
                    ViewData["ReadOnly"] = "true";
                }

                if (cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status == null)
                {
                    ViewBag.h = "إيقاف";
                    ViewBag.stat = "حذف";
                }
                //cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Reasons = "";
                ViewBag.AccountNo = cR_Cas_Sup_SalesPoint.CR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Account_No;

                var R = db.CR_Cas_Account_Receipt.FirstOrDefault(r=>r.CR_Cas_Account_Receipt_SalesPoint_No==cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Code);
                if (R != null)
                {
                    if (R.CR_Cas_Account_Receipt_SalesPoint_Previous_Balance > 0 || cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Balance > 0)
                    {
                        TempData["Balance"] = "True";
                    }
                    else
                    {
                        TempData["Balance"] = "False";
                    }
                }
                else
                {
                    if (cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Balance > 0)
                    {
                        TempData["Balance"] = "True";
                    }
                    else
                    {
                        TempData["Balance"] = "False";
                    }
                }

                ViewBag.st = cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status;
            }

            ViewBag.CR_Cas_Sup_SalesPoint_Brn_Code = new SelectList(db.CR_Cas_Sup_Branch.Where(b => b.CR_Cas_Sup_Lessor_Code == LessorCode && b.CR_Cas_Sup_Branch_Status == "A"),
                "CR_Cas_Sup_Branch_Code", "CR_Cas_Sup_Branch_Ar_Name",cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Brn_Code);

            ViewBag.CR_Cas_Sup_SalesPoint_Bank_Code = new SelectList(db.CR_Cas_Sup_Bank.Where(o => o.CR_Cas_Sup_Bank_Status != "D" && o.CR_Cas_Sup_Bank_Mas_Code != "00" && o.CR_Cas_Sup_Bank_Com_Code == LessorCode && o.CR_Cas_Sup_Bank_Account_No==cR_Cas_Sup_SalesPoint.CR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Account_No),
              "CR_Cas_Sup_Bank_Code", "CR_Cas_Sup_Bank_Ar_Name",cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Bank_Code);

            ViewBag.CR_Mas_Sup_Bank = new SelectList(db.CR_Cas_Sup_Bank.Where(o => o.CR_Cas_Sup_Bank_Status != "D" && o.CR_Cas_Sup_Bank_Mas_Code != "00").Select(x => x.CR_Mas_Sup_Bank).Distinct(),
              "CR_Mas_Sup_Bank_Code", "CR_Mas_Sup_Bank_Ar_Name",cR_Cas_Sup_SalesPoint.CR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Mas_Code);

            //ViewBag.CR_Mas_Sup_Bank = new SelectList(db.CR_Mas_Sup_Bank.Where(o => o.CR_Mas_Sup_Bank_Status != "D" && o.CR_Mas_Sup_Bank_Code != "00"),
            //    "CR_Mas_Sup_Bank_Code", "CR_Mas_Sup_Bank_Ar_Name", cR_Cas_Sup_SalesPoint.CR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Mas_Code);

            return View(cR_Cas_Sup_SalesPoint);
        }

        // POST: CasSalesPoint/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Sup_SalesPoint_Code,CR_Cas_Sup_SalesPoint_Com_Code," +
            "CR_Cas_Sup_SalesPoint_Brn_Code,CR_Cas_Sup_SalesPoint_Bank_Code,CR_Cas_Sup_SalesPoint_Bank_No," +
            "CR_Cas_Sup_SalesPoint_Ar_Name,CR_Cas_Sup_SalesPoint_En_Name,CR_Cas_Sup_SalesPoint_Balance," +
            "CR_Cas_Sup_SalesPoint_Status,CR_Cas_Sup_SalesPoint_Reasons")] CR_Cas_Sup_SalesPoint cR_Cas_Sup_SalesPoint,
            string save, string delete, string hold)
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
            
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {

                    using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.Entry(cR_Cas_Sup_SalesPoint).State = EntityState.Modified;
                            ///////////////////////////////Tracing//////////////////////////////////////  /////////////////////////////////////////////////////
                            SaveTracing(LessorCode, cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Code, "U", cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Reasons);
                            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            db.SaveChanges();
                            dbTran.Commit();
                            TempData["TempModel"] = "Saved";
                            return RedirectToAction("Index");
                        }
                        catch (DbEntityValidationException ex)
                        {
                            dbTran.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            if (delete == "Delete" || delete == "حذف")
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status = "D";
                        db.Entry(cR_Cas_Sup_SalesPoint).State = EntityState.Modified;
                        ///////////////////////////////Tracing//////////////////////////////////////  /////////////////////////////////////////////////////
                        SaveTracing(LessorCode, cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Code, "D", cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Reasons);
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        db.SaveChanges();
                        dbTran.Commit();
                        TempData["TempModel"] = "Deleted";
                        return RedirectToAction("Index");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }
            }
            if (delete == "Activate" || delete == "إسترجاع")
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status = "A";
                        ///////////////////////////////Tracing//////////////////////////////////////  /////////////////////////////////////////////////////
                        SaveTracing(LessorCode, cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Code, "A", cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Reasons);
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        db.Entry(cR_Cas_Sup_SalesPoint).State = EntityState.Modified;
                        db.SaveChanges();
                        dbTran.Commit();
                        TempData["TempModel"] = "Activated";
                        return RedirectToAction("Index");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }


            }
            if (hold == "إيقاف" || hold == "hold")
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status = "H";
                        ///////////////////////////////Tracing//////////////////////////////////////  /////////////////////////////////////////////////////
                        SaveTracing(LessorCode, cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Code, "H", cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Reasons);
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        db.Entry(cR_Cas_Sup_SalesPoint).State = EntityState.Modified;
                        db.SaveChanges();
                        dbTran.Commit();
                        TempData["TempModel"] = "Holded";
                        return RedirectToAction("Index");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }

            }
            if (hold == "تنشيط" || hold == "Activate")
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status = "A";
                        ///////////////////////////////Tracing//////////////////////////////////////  /////////////////////////////////////////////////////
                        SaveTracing(LessorCode, cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Code, "A", cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Reasons);
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        db.Entry(cR_Cas_Sup_SalesPoint).State = EntityState.Modified;
                        db.SaveChanges();
                        dbTran.Commit();
                        TempData["TempModel"] = "Activated";
                        return RedirectToAction("Index");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }

            }

            if (cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status == "A" ||
                    cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status == "Activated" ||
                    cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status == "1" ||
                    cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }

            if (cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status == "D" ||
                cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status == "Deleted" ||
                cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status == "0")
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
                ViewData["ReadOnly"] = "true";
            }

            if (cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status == "H" ||
                cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status == "Hold" ||
                cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
                ViewData["ReadOnly"] = "true";
            }

            if (cR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }
            ViewBag.CR_Cas_Sup_SalesPoint_Brn_Code = new SelectList(db.CR_Cas_Sup_Branch.Where(b => b.CR_Cas_Sup_Lessor_Code == LessorCode && b.CR_Cas_Sup_Branch_Status == "A"),
                 "CR_Cas_Sup_Branch_Code", "CR_Cas_Sup_Branch_Ar_Name");

            ViewBag.CR_Cas_Sup_SalesPoint_Bank_Code = new SelectList(db.CR_Cas_Sup_Bank.Where(o => o.CR_Cas_Sup_Bank_Status != "D" && o.CR_Cas_Sup_Bank_Mas_Code != "00" && o.CR_Cas_Sup_Bank_Com_Code == LessorCode),
              "CR_Cas_Sup_Bank_Code", "CR_Cas_Sup_Bank_Ar_Name");

            ViewBag.CR_Mas_Sup_Bank = new SelectList(db.CR_Cas_Sup_Bank.Where(o => o.CR_Cas_Sup_Bank_Status != "D" && o.CR_Cas_Sup_Bank_Mas_Code != "00").Select(x => x.CR_Mas_Sup_Bank).Distinct(),
              "CR_Mas_Sup_Bank_Code", "CR_Mas_Sup_Bank_Ar_Name");
            return View(cR_Cas_Sup_SalesPoint);
        }

        // GET: CasSalesPoint/Delete/5
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

        // POST: CasSalesPoint/Delete/5
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
