using RentCar.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentCar.Controllers.BranchSys
{
    public class CashClosingController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        // GET: CashClosing

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

        public PartialViewResult PartialView(string SalesPointCode)
        {
            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            try
            {
                if (Session["LessorCode"] == null || Session["UserLogin"] == null || Session["BranchCode"] == null)
                {
                    RedirectToAction("Login", "Account");
                }
                LessorCode = Session["LessorCode"].ToString();
                BranchCode = Session["BranchCode"].ToString();
                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString().Trim();
               
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }

            var Receipt = db.CR_Cas_Account_Receipt.Where(r => r.CR_Cas_Account_Receipt_Lessor_Code == LessorCode && r.CR_Cas_Account_Receipt_Branch_Code == BranchCode &&
               r.CR_Cas_Account_Receipt_Is_Passing == "1" && r.CR_Cas_Account_Receipt_User_Code == UserLogin && r.CR_Cas_Account_Receipt_SalesPoint_No == SalesPointCode);
            // var x = Receipt.Count();

            if (SalesPointCode != null && SalesPointCode!="")
            {
                var GetTotalCashInSalesPoint = db.CR_Cas_Sup_SalesPoint.FirstOrDefault(l => l.CR_Cas_Sup_SalesPoint_Code == SalesPointCode).CR_Cas_Sup_SalesPoint_Balance;
                ViewBag.Total1 = GetTotalCashInSalesPoint;

                var userBalance = db.CR_Cas_Account_Receipt.Where(x => x.CR_Cas_Account_Receipt_Lessor_Code == LessorCode && x.CR_Cas_Account_Receipt_Branch_Code == BranchCode && x.CR_Cas_Account_Receipt_User_Code == UserLogin && x.CR_Cas_Account_Receipt_SalesPoint_No == SalesPointCode&&x.CR_Cas_Account_Receipt_Is_Passing=="1").ToList();
                if (userBalance.Count()!=0)
                {
                    var userReceiptPayment = userBalance.Sum(x => x.CR_Cas_Account_Receipt_Payment);
                    var userReceiptReceipt = userBalance.Sum(x => x.CR_Cas_Account_Receipt_Receipt);
                    var userBalanceForEachBranch = userReceiptPayment - userReceiptReceipt;
                    ViewBag.userBalanceForEachBranch = userBalanceForEachBranch;
                }
                else
                {
                    ViewBag.userBalanceForEachBranch = "0.00";
                }

            }
            else
            {
                ViewBag.userBalanceForEachBranch = "0.00";
                ViewBag.Total1 = "0.00";

            }

            return PartialView(Receipt.OrderByDescending(x=>x.CR_Cas_Account_Receipt_Date));
        }



        [HttpGet]
        public ActionResult Create()
        {
            var LessorCode = "";
            var userlogin = "";
            var branchcode = "";
            try
            {
                if (Session["LessorCode"] == null || Session["UserLogin"] == null || Session["BranchCode"] == null)
                {
                    RedirectToAction("Login", "Account");
                }
                LessorCode = Session["lessorcode"].ToString();
                branchcode = Session["branchcode"].ToString();
                userlogin = Session["userlogin"].ToString();
                
            }
            catch
            {
                RedirectToAction("login", "account");
            }
            DateTime year = DateTime.Now;
            ViewBag.Date = DateTime.Now.ToString("yyyy/MM/dd");
            var y = year.ToString("yy");
            var sector = "1";
            var ProcedureCode = "69";
            var autoInc = GetLastRecordTracing(ProcedureCode, "1");
            ViewBag.ProcNo = (y + "-" + sector + "-" + ProcedureCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No).ToString();
           

            var CashPoint = LessorCode + "0001";

            ViewBag.CR_Cas_Sup_SalesPoint_Code = new SelectList(db.CR_Cas_Sup_SalesPoint.Where(o => o.CR_Cas_Sup_SalesPoint_Status != "D" &&
            o.CR_Cas_Sup_SalesPoint_Bank_Code != CashPoint && o.CR_Cas_Sup_SalesPoint_Com_Code == LessorCode && o.CR_Cas_Sup_SalesPoint_Brn_Code == branchcode
            && o.CR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "A"),
                "CR_Cas_Sup_SalesPoint_Code", "CR_Cas_Sup_SalesPoint_Ar_Name");

            /*    var totalCashInSalesPoint = db*/

            ViewBag.sd = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            ViewBag.ed = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            return View();
        }


        [HttpPost]
        public ActionResult Create(string TracingNo, FormCollection collection, string sd, string ed, string CR_Cas_Sup_SalesPoint_Code, string reason ,string TotalVal)
        {
            var b = false;
            var totalS = "";
            if (Session["LessorCode"] == null || Session["UserLogin"] == null || Session["BranchCode"] == null)
            {
                RedirectToAction("Login", "Account");
            }
            var LessorCode = Session["lessorcode"].ToString();
            var branchcode = Session["branchcode"].ToString();
            var userlogin = Session["userlogin"].ToString();
            decimal val = 0;
            foreach (string item in collection.AllKeys)
            {
                 totalS= collection["TotalVal"];
                if (decimal.Parse(totalS)>0)
                {
                    if (item.StartsWith("Chk_"))
                    {
                        b = true;
                        var ReceiptNo = item.Replace("Chk_", "");
                        var Amount = collection["ValCashClosing_" + ReceiptNo];
                        val += Convert.ToDecimal(Amount);
                        var receipt = db.CR_Cas_Account_Receipt.Single(r => r.CR_Cas_Account_Receipt_No == ReceiptNo);
                        receipt.CR_Cas_Account_Receipt_Reasons = reason.ToString();
                       
                        //Get The current user 
                        var currentUser = db.CR_Cas_User_Information.FirstOrDefault(l => l.CR_Cas_User_Information_Id == userlogin);
                        currentUser.CR_Cas_User_Information_Balance -= Convert.ToDecimal(Amount);

                        db.SaveChanges();


                        if (receipt != null)
                        {
                            receipt.CR_Cas_Account_Receipt_Is_Passing = "3";
                            receipt.CR_Cas_Account_Receipt_Reference_Passing = TracingNo;
                            db.Entry(receipt).State = EntityState.Modified;

                        }
                    }
                }
                else
                {
                    totalS = "0";
                    if (item.StartsWith("Chk_"))
                    {
                        b = true;
                        var ReceiptNo = item.Replace("Chk_", "");
                        var Amount = collection["ValCashClosing_" + ReceiptNo];
                        val += Convert.ToDecimal(Amount);
                        var receipt = db.CR_Cas_Account_Receipt.Single(r => r.CR_Cas_Account_Receipt_No == ReceiptNo);
                        receipt.CR_Cas_Account_Receipt_Reasons = reason.ToString();
                        //Get The Current SalesPoint 
                        //var currentSalesPoint = db.CR_Cas_Sup_SalesPoint.FirstOrDefault(l => l.CR_Cas_Sup_SalesPoint_Code == CR_Cas_Sup_SalesPoint_Code);
                        //currentSalesPoint.CR_Cas_Sup_SalesPoint_Balance -= Convert.ToDecimal(Amount);

                        //Get The current user 
                        //var currentUser = db.CR_Cas_User_Information.FirstOrDefault(l => l.CR_Cas_User_Information_Id == userlogin);
                        //currentUser.CR_Cas_User_Information_Balance -= Convert.ToDecimal(Amount);

                        db.SaveChanges();


                        if (receipt != null)
                        {
                            receipt.CR_Cas_Account_Receipt_Is_Passing = "3";
                            receipt.CR_Cas_Account_Receipt_Reference_Passing = TracingNo;
                            db.Entry(receipt).State = EntityState.Modified;

                        }
                    }
                }
                
            }
            if (b == true)
            {
                //Get The Current SalesPoint 
                var currentSalesPoint = db.CR_Cas_Sup_SalesPoint.FirstOrDefault(l => l.CR_Cas_Sup_SalesPoint_Code == CR_Cas_Sup_SalesPoint_Code);
                currentSalesPoint.CR_Cas_Sup_SalesPoint_Balance -= Convert.ToDecimal(TotalVal);

                List<CR_Cas_Account_Receipt> receipts = new List<CR_Cas_Account_Receipt>(); ;

                foreach (string item in collection.AllKeys)
                {
                    if (item.StartsWith("Chk_"))
                    {
                        var ReceiptNo = item.Replace("Chk_", "");
                        var  receipt= db.CR_Cas_Account_Receipt.Single(r => r.CR_Cas_Account_Receipt_No == ReceiptNo);
                        receipts.Add(receipt);
                    }
                }


                //var accountReceipt = db.CR_Cas_Account_Receipt.Where(c => c.CR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Code == CR_Cas_Sup_SalesPoint_Code && c.CR_Cas_Account_Receipt_Lessor_Code == LessorCode
                //&& c.CR_Cas_Account_Receipt_Branch_Code == branchcode && c.CR_Cas_Account_Receipt_User_Code == userlogin).ToList();


                DateTime minDate =(DateTime)receipts.Min(c => c.CR_Cas_Account_Receipt_Date);
                DateTime maxDate =(DateTime)receipts.Max(c => c.CR_Cas_Account_Receipt_Date);

                ///////////////////////////////Tracing//////////////////////////////////////
                CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
                DateTime year = DateTime.Now;
                var y = year.ToString("yy");
                var sector = "1";

                Ad.CR_Cas_Administrative_Procedures_No = TracingNo;
                Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
                string currentTime = DateTime.Now.ToString("HH:mm:ss");
                Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
                Ad.CR_Cas_Administrative_Procedures_Year = y;
                Ad.CR_Cas_Administrative_Procedures_Sector = sector;
                Ad.CR_Cas_Administrative_Procedures_Code = "69";
                Ad.CR_Cas_Administrative_Int_Procedures_Code = Int32.Parse("69");
                Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
                Ad.CR_Cas_Administrative_Procedures_Targeted_Action = CR_Cas_Sup_SalesPoint_Code;
                Ad.CR_Cas_Administrative_Procedures_User_Insert = userlogin;
                Ad.CR_Cas_Administrative_Procedures_Type = "I";
                Ad.CR_Cas_Administrative_Procedures_Action = false;
                Ad.CR_Cas_Administrative_Procedures_Doc_Date = DateTime.Now;
                DateTime StartDate = Convert.ToDateTime(sd);
                DateTime EndDate = Convert.ToDateTime(ed);
                Ad.CR_Cas_Administrative_Procedures_Doc_Start_Date = minDate;
                Ad.CR_Cas_Administrative_Procedures_Doc_End_Date = maxDate;
                Ad.CR_Cas_Administrative_Procedures_Doc_No = "";
                //Ad.CR_Cas_Administrative_Procedures_Value = val;
                Ad.CR_Cas_Administrative_Procedures_Value = decimal.Parse(totalS);

                db.CR_Cas_Administrative_Procedures.Add(Ad);
                ///////////////////////////////////////////////////////////////////////////////

                db.SaveChanges();
            }

            return RedirectToAction("BranchStat", "BranchHome");
        }


    }
}