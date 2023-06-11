using RentCar.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentCar.Controllers.BranchSys
{
    public class AcceptCashTransferController : Controller
    {
        // GET: AcceptCashTransfer
        private RentCarDBEntities db = new RentCarDBEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var sector = "1";
            if (Session["LessorCode"] == null || Session["UserLogin"] == null || Session["BranchCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var LessorCode = Session["LessorCode"].ToString();
            var UserLogin = Session["UserLogin"].ToString().Trim();
            var proc = db.CR_Cas_Administrative_Procedures.FirstOrDefault(a=>a.CR_Cas_Administrative_Procedures_Lessor==LessorCode &&
            a.CR_Cas_Administrative_Procedures_Code=="62" && a.CR_Cas_Administrative_Procedures_Type=="I" && a.CR_Cas_Administrative_Procedures_Action==false 
            && a.CR_Cas_Administrative_Procedures_Targeted_Action==UserLogin);

            if (proc != null)
            {
                var user = db.CR_Cas_User_Information.FirstOrDefault(u=>u.CR_Cas_User_Information_Id==proc.CR_Cas_Administrative_Procedures_User_Insert);
                if (user != null)
                {
                    ViewBag.UserName = user.CR_Cas_User_Information_Ar_Name;
                }
                //ViewBag.Administrative_Procedures_No = proc.CR_Cas_Administrative_Procedures_No;
                //ViewBag.Administrative_Procedures_Value = proc.CR_Cas_Administrative_Procedures_Value;

                ViewBag.TransferDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                ViewBag.Reasons = proc.CR_Cas_Administrative_Procedures_Reasons;
            }

            return View(proc);
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
        // POST: TransferFromUser/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Administrative_Procedures_No,CR_Cas_Administrative_Procedures_Date," +
            "CR_Cas_Administrative_Procedures_Time,CR_Cas_Administrative_Procedures_Year,CR_Cas_Administrative_Procedures_Sector," +
            "CR_Cas_Administrative_Procedures_Code,CR_Cas_Administrative_Procedures_Lessor,CR_Cas_Administrative_Procedures_Targeted_Action," +
            "CR_Cas_Administrative_Procedures_Com_Supporting,CR_Cas_Administrative_Procedures_Value,CR_Cas_Administrative_Procedures_Action," +
            "CR_Cas_Administrative_Procedures_User_Insert,CR_Cas_Administrative_Procedures_Type,CR_Cas_Administrative_Procedures_Reasons," +
            "CR_Cas_Administrative_Procedures_Doc_No,CR_Cas_Administrative_Procedures_Doc_Date,CR_Cas_Administrative_Procedures_Doc_Start_Date," +
            "CR_Cas_Administrative_Procedures_Doc_End_Date,CR_Cas_Administrative_Procedures_From_Branch,CR_Cas_Administrative_Procedures_To_Branch")] 
        CR_Cas_Administrative_Procedures cR_Cas_Administrative_Procedures ,string save,string Refuse)
        {
            if (Session["LessorCode"] == null || Session["UserLogin"] == null || Session["BranchCode"] == null )
            {
                return RedirectToAction("Login", "Account");
            }
            var LessorCode = Session["LessorCode"].ToString();
            var BranchCode = Session["BranchCode"].ToString();
            var UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
            if (ModelState.IsValid)
            {
                var Ad = db.CR_Cas_Administrative_Procedures.Single(a => a.CR_Cas_Administrative_Procedures_No == cR_Cas_Administrative_Procedures.CR_Cas_Administrative_Procedures_No);
                if (!string.IsNullOrEmpty(save))
                {
                    Ad.CR_Cas_Administrative_Procedures_Type = "Q";
                    db.Entry(Ad).State = EntityState.Modified;
                    db.SaveChanges();

                    //////////////////////Receipt/////////////////////////
                    DateTime year = DateTime.Now;
                    var y = year.ToString("yy");
                    var Sector = "1";
                    CR_Cas_Account_Receipt Receipt = new CR_Cas_Account_Receipt();
                    var autoinc = GetReceiptLastRecord(LessorCode, BranchCode).CR_Cas_Account_Receipt_No;
                    Receipt.CR_Cas_Account_Receipt_No = y + "-" + Sector + "-" + "60" + "-" + LessorCode + "-" + BranchCode + autoinc;
                    Receipt.CR_Cas_Account_Receipt_Year = y;
                    Receipt.CR_Cas_Account_Receipt_Type = "60";
                    Receipt.CR_Cas_Account_Receipt_Lessor_Code = LessorCode;
                    Receipt.CR_Cas_Account_Receipt_Branch_Code = BranchCode;
                    Receipt.CR_Cas_Account_Receipt_Date = DateTime.Now;
                    Receipt.CR_Cas_Account_Receipt_Contract_Operation = cR_Cas_Administrative_Procedures.CR_Cas_Administrative_Procedures_No;
                    Receipt.CR_Cas_Account_Receipt_Reference_Type = "تغذية";
                    Receipt.CR_Cas_Account_Receipt_Payment = cR_Cas_Administrative_Procedures.CR_Cas_Administrative_Procedures_Value;
                    Receipt.CR_Cas_Account_Receipt_Receipt = 0;
                    Receipt.CR_Cas_Account_Receipt_Bank_Code = LessorCode + "0000";
                    Receipt.CR_Cas_Account_Receipt_SalesPoint_No = LessorCode + BranchCode + LessorCode + "000000";

                    var SalesPoint = db.CR_Cas_Sup_SalesPoint.Single(s=>s.CR_Cas_Sup_SalesPoint_Code==Receipt.CR_Cas_Account_Receipt_SalesPoint_No);
                    if (SalesPoint != null)
                    {
                        Receipt.CR_Cas_Account_Receipt_SalesPoint_Previous_Balance = SalesPoint.CR_Cas_Sup_SalesPoint_Balance;
                        if (SalesPoint.CR_Cas_Sup_SalesPoint_Balance == null)
                        {
                            SalesPoint.CR_Cas_Sup_SalesPoint_Balance = cR_Cas_Administrative_Procedures.CR_Cas_Administrative_Procedures_Value;
                        }
                        else
                        {
                            SalesPoint.CR_Cas_Sup_SalesPoint_Balance += cR_Cas_Administrative_Procedures.CR_Cas_Administrative_Procedures_Value;
                        }
                            
                        db.Entry(SalesPoint).State = EntityState.Modified;
                    }
                   

                    Receipt.CR_Cas_Account_Receipt_Payment_Method = "10";
                    Receipt.CR_Cas_Account_Receipt_User_Code = UserLogin;

                    var user = db.CR_Cas_User_Information.FirstOrDefault(u=>u.CR_Cas_User_Information_Id==UserLogin);
                    if (user != null)
                    {
                        Receipt.CR_Cas_Account_Receipt_User_Previous_Balance = user.CR_Cas_User_Information_Balance;
                        if (user.CR_Cas_User_Information_Balance == null)
                        {
                            user.CR_Cas_User_Information_Balance = cR_Cas_Administrative_Procedures.CR_Cas_Administrative_Procedures_Value;
                        }
                        else
                        {
                            user.CR_Cas_User_Information_Balance += cR_Cas_Administrative_Procedures.CR_Cas_Administrative_Procedures_Value;
                        }
                        
                        db.Entry(user).State = EntityState.Modified;
                    }

                    Receipt.CR_Cas_Account_Receipt_Is_Passing = "1";

                    Receipt.CR_Cas_Account_Receipt_Status = "A";
                    Receipt.CR_Cas_Account_Receipt_Reasons = cR_Cas_Administrative_Procedures.CR_Cas_Administrative_Procedures_Reasons;
                    db.CR_Cas_Account_Receipt.Add(Receipt);
                    /////////////////////////////////////////////////////////

                   

                    RedirectToAction("BranchHome", "BranchHome");
                    db.SaveChanges();
                    TempData["TempModel"] = "Saved";
                }
                else if (!string.IsNullOrEmpty(Refuse))
                {
                    Ad.CR_Cas_Administrative_Procedures_Type = "Z";
                    db.Entry(Ad).State = EntityState.Modified;
                    RedirectToAction("BranchHome", "BranchHome");
                    db.SaveChanges();
                }
                db.SaveChanges();

                return RedirectToAction("BranchHome", "BranchHome");
            }

           
            return RedirectToAction("BranchHome", "BranchHome");
        }
    }
}