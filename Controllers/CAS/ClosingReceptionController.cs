using Microsoft.Ajax.Utilities;
using RentCar.Models;
using RentCar.Models.CAS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace RentCar.Controllers.CAS
{
    public class ClosingReceptionController : Controller
    {
        // GET: ClosingReception 
        private RentCarDBEntities db = new RentCarDBEntities();
        public ActionResult Index()
        {
            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
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
            var Admin = db.CR_Cas_Administrative_Procedures.Where(a => a.CR_Cas_Administrative_Procedures_Lessor == LessorCode && a.CR_Cas_Administrative_Procedures_Code == "69"
            && a.CR_Cas_Administrative_Procedures_User_Insert != UserLogin);
            var x = Admin.Count();
            List<ClosingReceptionMD> ListReception = new List<ClosingReceptionMD>();
            foreach (var a in Admin)
            {
                ClosingReceptionMD R = new ClosingReceptionMD();
                R.CR_Cas_Administrative_Int_Procedures_Code = a.CR_Cas_Administrative_Int_Procedures_Code;
                R.CR_Cas_Administrative_Procedures_Action = a.CR_Cas_Administrative_Procedures_Action;
                R.CR_Cas_Administrative_Procedures_Code = a.CR_Cas_Administrative_Procedures_Code;
                R.CR_Cas_Administrative_Procedures_Com_Supporting = a.CR_Cas_Administrative_Procedures_Com_Supporting;
                R.CR_Cas_Administrative_Procedures_Date = a.CR_Cas_Administrative_Procedures_Date;
                R.CR_Cas_Administrative_Procedures_Doc_Date = a.CR_Cas_Administrative_Procedures_Doc_Date;
                R.CR_Cas_Administrative_Procedures_Doc_End_Date = a.CR_Cas_Administrative_Procedures_Doc_End_Date;
                R.CR_Cas_Administrative_Procedures_Doc_No = a.CR_Cas_Administrative_Procedures_Doc_No;
                R.CR_Cas_Administrative_Procedures_Doc_Start_Date = a.CR_Cas_Administrative_Procedures_Doc_Start_Date;
                R.CR_Cas_Administrative_Procedures_From_Branch = a.CR_Cas_Administrative_Procedures_From_Branch;
                R.CR_Cas_Administrative_Procedures_Lessor = a.CR_Cas_Administrative_Procedures_Lessor;
                R.CR_Cas_Administrative_Procedures_No = a.CR_Cas_Administrative_Procedures_No;
                R.CR_Cas_Administrative_Procedures_Sector = a.CR_Cas_Administrative_Procedures_Sector;
                R.CR_Cas_Administrative_Procedures_Targeted_Action = a.CR_Cas_Administrative_Procedures_Targeted_Action;
                R.CR_Cas_Administrative_Procedures_Time = a.CR_Cas_Administrative_Procedures_Time;
                R.CR_Cas_Administrative_Procedures_To_Branch = a.CR_Cas_Administrative_Procedures_To_Branch;
                R.CR_Cas_Administrative_Procedures_Type = a.CR_Cas_Administrative_Procedures_Type;
                R.CR_Cas_Administrative_Procedures_User_Insert = a.CR_Cas_Administrative_Procedures_User_Insert;
                R.CR_Cas_Administrative_Procedures_Value = a.CR_Cas_Administrative_Procedures_Value;
                R.CR_Cas_Administrative_Procedures_Year = a.CR_Cas_Administrative_Procedures_Year;
                var SalesPoint = db.CR_Cas_Sup_SalesPoint.FirstOrDefault(s => s.CR_Cas_Sup_SalesPoint_Code == R.CR_Cas_Administrative_Procedures_Targeted_Action);
                if (SalesPoint != null)
                {
                    R.CR_Cas_Sup_SalesPoint_Ar_Name = SalesPoint.CR_Cas_Sup_SalesPoint_Ar_Name;
                    R.CR_Cas_Sup_SalesPoint_En_Name = SalesPoint.CR_Cas_Sup_SalesPoint_En_Name;
                }
                var User = db.CR_Cas_User_Information.FirstOrDefault(u => u.CR_Cas_User_Information_Id == R.CR_Cas_Administrative_Procedures_User_Insert);
                if (User != null)
                {
                    R.CR_Cas_User_Information_Ar_Name = User.CR_Cas_User_Information_Ar_Name;
                }
                ListReception.Add(R);
            }

            return View(ListReception);
        }


        [HttpGet]
        public ActionResult Create(string No, string Date)
        {
            var LessorCode = "";
            var userlogin = "";
            var branchcode = "";
            try
            {
                LessorCode = Session["lessorcode"].ToString();
                userlogin = Session["userlogin"].ToString();
                if (userlogin == null || LessorCode == null || branchcode == null)
                {
                    RedirectToAction("account", "login");
                }
            }
            catch
            {
                RedirectToAction("login", "account");
            }

            var Receipt = db.CR_Cas_Account_Receipt.Where(r => r.CR_Cas_Account_Receipt_Reference_Passing == No.Trim());
            if (Receipt == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.ProcNo = No;
                var d = DateTime.Parse(Date);
                string s = d.ToString("yyyy/MM/dd");
                ViewBag.Date = s;

                var f = Receipt.First();
                var sd = f.CR_Cas_Account_Receipt_Date;
                ViewBag.sd = sd?.ToString("yyyy/MM/dd");

                var l = Receipt.OrderByDescending(x => x.CR_Cas_Account_Receipt_Date).FirstOrDefault();
                var ed = f.CR_Cas_Account_Receipt_Date;
                ViewBag.ed = string.Format("{0:yyyy/MM/dd}", ed);


                return View(Receipt);
            }

        }
        public ActionResult ShowCreate(string No, string Date)
        {
            var LessorCode = "";
            var userlogin = "";
            var branchcode = "";
            try
            {
                LessorCode = Session["lessorcode"].ToString();
                userlogin = Session["userlogin"].ToString();
                if (userlogin == null || LessorCode == null || branchcode == null)
                {
                    RedirectToAction("account", "login");
                }
            }
            catch
            {
                RedirectToAction("login", "account");
            }

            var Receipt = db.CR_Cas_Account_Receipt.Where(r => r.CR_Cas_Account_Receipt_Reference_Passing == No.Trim()&&r.CR_Cas_Account_Receipt_Payment_Method!="25");
            if (Receipt == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.ProcNo = No;
                var d = DateTime.Parse(Date);
                string s = d.ToString("yyyy/MM/dd");
                ViewBag.Date = s;

                var f = Receipt.First();
                var sd = f.CR_Cas_Account_Receipt_Date;
                ViewBag.sd = sd?.ToString("yyyy/MM/dd");

                var l = Receipt.OrderByDescending(x => x.CR_Cas_Account_Receipt_Date).FirstOrDefault();
                var ed = f.CR_Cas_Account_Receipt_Date;
                ViewBag.ed = string.Format("{0:yyyy/MM/dd}", ed);


                return View(Receipt);
            }

        }
        [HttpPost]
        public ActionResult Create(string TracingNo, string save, string Cancel)
        {
            if (!string.IsNullOrEmpty(save))
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var LessorCode = Session["LessorCode"].ToString();
                        var userlogin = Session["UserLogin"].ToString();
                        var BranchCode = "";
                        var p = db.CR_Cas_Administrative_Procedures.Single(x => x.CR_Cas_Administrative_Procedures_No == TracingNo);
                        if (p != null)
                        {
                            //////////////////////////Update Administrative procedures///////////////
                            p.CR_Cas_Administrative_Procedures_Type = "Q";
                            db.Entry(p).State = EntityState.Modified;
                            //////////////////////////////////////////////////////////////////////////
                            ///////////////////////////Update Receipt/////////////////////////////////
                            var Receipt = db.CR_Cas_Account_Receipt.Where(r => r.CR_Cas_Account_Receipt_Reference_Passing == TracingNo.Trim());
                            if (Receipt != null)
                            {
                                foreach (var r in Receipt)
                                {
                                    r.CR_Cas_Account_Receipt_Is_Passing = "2";
                                    r.CR_Cas_Account_Receipt_Passing_Date = DateTime.Now;
                                    r.CR_Cas_Account_Receipt_User_Passing = userlogin;
                                    BranchCode = r.CR_Cas_Account_Receipt_Branch_Code;
                                    db.Entry(r).State = EntityState.Modified;
                                }
                            }
                            /////////////////////////////////////////////////////////////////////////////
                            /////////////////////////Add new receipt/////////////////////////////
                            CR_Cas_Account_Receipt Rcpt = new CR_Cas_Account_Receipt();
                            DateTime year = DateTime.Now;
                            var y = year.ToString("yy");

                            var Sector = "1";
                            var autoinc = GetReceiptLastRecord(LessorCode, BranchCode).CR_Cas_Account_Receipt_No;
                            Rcpt.CR_Cas_Account_Receipt_No = y + "-" + Sector + "-" + "61" + "-" + LessorCode + "-" + BranchCode + autoinc;
                            Rcpt.CR_Cas_Account_Receipt_Year = y;
                            Rcpt.CR_Cas_Account_Receipt_Type = "61";
                            Rcpt.CR_Cas_Account_Receipt_Lessor_Code = LessorCode;
                            Rcpt.CR_Cas_Account_Receipt_Branch_Code = BranchCode;
                            Rcpt.CR_Cas_Account_Receipt_Date = DateTime.Now;
                            Rcpt.CR_Cas_Account_Receipt_Contract_Operation = TracingNo.Trim();
                            Rcpt.CR_Cas_Account_Receipt_Payment = 0;
                            Rcpt.CR_Cas_Account_Receipt_Receipt = p.CR_Cas_Administrative_Procedures_Value;
                            Rcpt.CR_Cas_Account_Receipt_Reference_Type = "تسليم عهدة";
                            Rcpt.CR_Cas_Account_Receipt_Is_Passing = "2";
                            Rcpt.CR_Cas_Account_Receipt_Reference_Passing = TracingNo.Trim();

                            var paymentMethod = Receipt.FirstOrDefault().CR_Mas_Sup_Payment_Method.CR_Mas_Sup_Payment_Method_Code;
                            if (paymentMethod == "20" ||  paymentMethod == "21" || paymentMethod == "22" || paymentMethod == "23" || paymentMethod == "24")
                            {
                                Rcpt.CR_Cas_Account_Receipt_Payment_Method = "25";
                            }
                            else
                            {
                                Rcpt.CR_Cas_Account_Receipt_Payment_Method = "10";
                            }

                            Rcpt.CR_Cas_Account_Receipt_Bank_Code = Receipt.FirstOrDefault().CR_Cas_Account_Receipt_Bank_Code;
                            Rcpt.CR_Cas_Account_Receipt_SalesPoint_No = Receipt.FirstOrDefault().CR_Cas_Account_Receipt_SalesPoint_No;
                            Rcpt.CR_Cas_Account_Receipt_Renter_Code = null;
                            Rcpt.CR_Cas_Account_Receipt_User_Code = Receipt.FirstOrDefault().CR_Cas_Account_Receipt_User_Code;
                            Rcpt.CR_Cas_Account_Receipt_Car_Code = null;
                            Rcpt.CR_Cas_Account_Receipt_Status = Receipt.FirstOrDefault().CR_Cas_Account_Receipt_Status;
                            Rcpt.CR_Cas_Account_Receipt_Passing_Date = DateTime.Now;
                            Rcpt.CR_Cas_Account_Receipt_User_Passing = userlogin;
                            Rcpt.CR_Cas_Account_Receipt_SalesPoint_Previous_Balance = Receipt.FirstOrDefault().CR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Balance + (Receipt.Sum(l => l.CR_Cas_Account_Receipt_Payment) - Receipt.Sum(l => l.CR_Cas_Account_Receipt_Receipt));
                            Rcpt.CR_Cas_Account_Receipt_User_Previous_Balance = Receipt.FirstOrDefault().CR_Cas_User_Information.CR_Cas_User_Information_Balance + (Receipt.Sum(l => l.CR_Cas_Account_Receipt_Payment) - Receipt.Sum(l => l.CR_Cas_Account_Receipt_Receipt));


                            db.CR_Cas_Account_Receipt.Add(Rcpt);

                            /////////////////////////////////////////////////////////////////////

                            //////////////////////////////////////Cas User ///////////////////////////////
                            var User = db.CR_Cas_User_Information.Single(u => u.CR_Cas_User_Information_Id == userlogin);
                            if (User != null)
                            {
                                User.CR_Cas_User_Information_Balance = User.CR_Cas_User_Information_Balance - p.CR_Cas_Administrative_Procedures_Value;
                                db.Entry(User).State = EntityState.Modified;
                            }
                            ////////////////////////////////////////////////////////////////////////////////
                            //////////////////////////////Sales Point///////////////////////////////////////
                            /* var Spoint = db.CR_Cas_Sup_SalesPoint.Single(s => s.CR_Cas_Sup_SalesPoint_Code == p.CR_Cas_Administrative_Procedures_Targeted_Action);
                             if (Spoint != null)
                             {
                                 Spoint.CR_Cas_Sup_SalesPoint_Balance = Spoint.CR_Cas_Sup_SalesPoint_Balance - p.CR_Cas_Administrative_Procedures_Value;
                                 db.Entry(Spoint).State = EntityState.Modified;
                             }*/
                            //////////////////////////////////////////////////////////////////////////////
                            db.SaveChanges();
                            dbTran.Commit();
                            return RedirectToAction("Index", "ClosingReception");
                        }
                        else
                        {
                            return HttpNotFound();
                        }
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var validationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Debug.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                            }
                        }
                        dbTran.Rollback();
                        throw ex;
                    }
                }

            }
            else
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var LessorCode = Session["LessorCode"].ToString();
                        var userlogin = Session["UserLogin"].ToString();
                        //var BranchCode = "";
                        var p = db.CR_Cas_Administrative_Procedures.Single(x => x.CR_Cas_Administrative_Procedures_No == TracingNo);
                        if (p != null)
                        {
                            //////////////////////////Update Administrative procedures///////////////
                            p.CR_Cas_Administrative_Procedures_Type = "Z";
                            db.Entry(p).State = EntityState.Modified;
                            //////////////////////////////////////////////////////////////////////////
                            ///////////////////////////Update Receipt/////////////////////////////////
                            var Receipt = db.CR_Cas_Account_Receipt.Where(r => r.CR_Cas_Account_Receipt_Reference_Passing == TracingNo.Trim());
                            if (Receipt != null)
                            {
                                foreach (var r in Receipt)
                                {
                                    r.CR_Cas_Sup_SalesPoint.CR_Cas_Sup_SalesPoint_Balance += r.CR_Cas_Account_Receipt_Payment;
                                    r.CR_Cas_User_Information.CR_Cas_User_Information_Balance += r.CR_Cas_Account_Receipt_Payment;
                                    r.CR_Cas_Account_Receipt_Is_Passing = "1";
                                    r.CR_Cas_Account_Receipt_Reference_Passing = "";
                                    r.CR_Cas_Account_Receipt_Passing_Date = null;
                                    r.CR_Cas_Account_Receipt_User_Passing = null;
                                    db.Entry(r).State = EntityState.Modified;
                                }
                            }
                            /////////////////////////////////////////////////////////////////////////////
                            ///////////////////////////////Update Cas User information Balance/////////////
                            //var User = db.CR_Cas_User_Information.Single(u => u.CR_Cas_User_Information_Id == p.CR_Cas_Administrative_Procedures_User_Insert);
                            //User.CR_Cas_User_Information_Balance += p.CR_Cas_Administrative_Procedures_Value;
                            //db.Entry(User).State = EntityState.Modified;
                            ///////////////////////////////////////////////////////////////////////////////
                            ///////////////////////////////Update SalesPoint Balance/////////////
                            //var Spoint = db.CR_Cas_Sup_SalesPoint.Single(u => u.CR_Cas_Sup_SalesPoint_Code == p.CR_Cas_Administrative_Procedures_Targeted_Action);
                            //Spoint.CR_Cas_Sup_SalesPoint_Balance += p.CR_Cas_Administrative_Procedures_Value;
                            //db.Entry(Spoint).State = EntityState.Modified;
                            /////////////////////////////////////////////////////////////////////////////

                            db.SaveChanges();
                            dbTran.Commit();
                            return RedirectToAction("Index", "ClosingReception");
                        }
                        else
                        {
                            return HttpNotFound();
                        }
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }

            }
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

        public PartialViewResult PartialIndex(string type)
        {
            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
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


            var Admin = db.CR_Cas_Administrative_Procedures.Where(a => a.CR_Cas_Administrative_Procedures_Lessor == LessorCode && a.CR_Cas_Administrative_Procedures_Code == "69"
            && a.CR_Cas_Administrative_Procedures_User_Insert != UserLogin && a.CR_Cas_Administrative_Procedures_Type != "Z");

            if (type == "I")
            {
                Admin = db.CR_Cas_Administrative_Procedures.Where(a => a.CR_Cas_Administrative_Procedures_Lessor == LessorCode && a.CR_Cas_Administrative_Procedures_Code == "69"
                && a.CR_Cas_Administrative_Procedures_User_Insert != UserLogin && a.CR_Cas_Administrative_Procedures_Type == "I");
            }
            if (type == "All")
            {
                Admin = db.CR_Cas_Administrative_Procedures.Where(a => a.CR_Cas_Administrative_Procedures_Lessor == LessorCode && a.CR_Cas_Administrative_Procedures_Code == "69"
                && a.CR_Cas_Administrative_Procedures_User_Insert != UserLogin && a.CR_Cas_Administrative_Procedures_Type != "Z");
            }
            if (type == "Q")
            {
                Admin = db.CR_Cas_Administrative_Procedures.Where(a => a.CR_Cas_Administrative_Procedures_Lessor == LessorCode && a.CR_Cas_Administrative_Procedures_Code == "69"
                && a.CR_Cas_Administrative_Procedures_User_Insert != UserLogin && a.CR_Cas_Administrative_Procedures_Type == "Q");
            }


            var x = Admin.Count();
            List<ClosingReceptionMD> ListReception = new List<ClosingReceptionMD>();
            foreach (var a in Admin)
            {
                ClosingReceptionMD R = new ClosingReceptionMD();
                R.CR_Cas_Administrative_Int_Procedures_Code = a.CR_Cas_Administrative_Int_Procedures_Code;
                R.CR_Cas_Administrative_Procedures_Action = a.CR_Cas_Administrative_Procedures_Action;
                R.CR_Cas_Administrative_Procedures_Code = a.CR_Cas_Administrative_Procedures_Code;
                R.CR_Cas_Administrative_Procedures_Com_Supporting = a.CR_Cas_Administrative_Procedures_Com_Supporting;
                R.CR_Cas_Administrative_Procedures_Date = a.CR_Cas_Administrative_Procedures_Date;
                R.CR_Cas_Administrative_Procedures_Doc_Date = a.CR_Cas_Administrative_Procedures_Doc_Date;
                R.CR_Cas_Administrative_Procedures_Doc_End_Date = a.CR_Cas_Administrative_Procedures_Doc_End_Date;
                R.CR_Cas_Administrative_Procedures_Doc_No = a.CR_Cas_Administrative_Procedures_Doc_No;
                R.CR_Cas_Administrative_Procedures_Doc_Start_Date = a.CR_Cas_Administrative_Procedures_Doc_Start_Date;
                R.CR_Cas_Administrative_Procedures_From_Branch = a.CR_Cas_Administrative_Procedures_From_Branch;
                R.CR_Cas_Administrative_Procedures_Lessor = a.CR_Cas_Administrative_Procedures_Lessor;
                R.CR_Cas_Administrative_Procedures_No = a.CR_Cas_Administrative_Procedures_No;
                R.CR_Cas_Administrative_Procedures_Sector = a.CR_Cas_Administrative_Procedures_Sector;
                R.CR_Cas_Administrative_Procedures_Targeted_Action = a.CR_Cas_Administrative_Procedures_Targeted_Action;
                R.CR_Cas_Administrative_Procedures_Time = a.CR_Cas_Administrative_Procedures_Time;
                R.CR_Cas_Administrative_Procedures_To_Branch = a.CR_Cas_Administrative_Procedures_To_Branch;
                R.CR_Cas_Administrative_Procedures_Type = a.CR_Cas_Administrative_Procedures_Type;
                R.CR_Cas_Administrative_Procedures_User_Insert = a.CR_Cas_Administrative_Procedures_User_Insert;
                R.CR_Cas_Administrative_Procedures_Value = a.CR_Cas_Administrative_Procedures_Value;
                R.CR_Cas_Administrative_Procedures_Year = a.CR_Cas_Administrative_Procedures_Year;
                var SalesPoint = db.CR_Cas_Sup_SalesPoint.FirstOrDefault(s => s.CR_Cas_Sup_SalesPoint_Code == R.CR_Cas_Administrative_Procedures_Targeted_Action);
                if (SalesPoint != null)
                {
                    R.CR_Cas_Sup_SalesPoint_Ar_Name = SalesPoint.CR_Cas_Sup_SalesPoint_Ar_Name;
                    R.CR_Cas_Sup_SalesPoint_En_Name = SalesPoint.CR_Cas_Sup_SalesPoint_En_Name;
                }
                var User = db.CR_Cas_User_Information.FirstOrDefault(u => u.CR_Cas_User_Information_Id == R.CR_Cas_Administrative_Procedures_User_Insert);
                if (User != null)
                {
                    R.CR_Cas_User_Information_Ar_Name = User.CR_Cas_User_Information_Ar_Name;
                }
                ListReception.Add(R);
            }


            return PartialView(ListReception);
        }
    }
}