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
    public class CasSupBankController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: CasSupBank
        public ActionResult Index()
        {
            var bank = db.CR_Cas_Sup_Bank.Where(b => b.CR_Cas_Sup_Bank_Status != "D");
            return View(bank);
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


            IQueryable<CR_Cas_Sup_Bank> cR_Cas_Sup_Bank = null;
            if (type == "All")
            {
                cR_Cas_Sup_Bank = db.CR_Cas_Sup_Bank.Where(b=>b.CR_Cas_Sup_Bank_Com_Code==LessorCode && b.CR_Cas_Sup_Bank_Mas_Code!="00");

            }else if (type == "D")
            {
                cR_Cas_Sup_Bank = db.CR_Cas_Sup_Bank.Where(b=>b.CR_Cas_Sup_Bank_Com_Code==LessorCode && b.CR_Cas_Sup_Bank_Status=="D" && b.CR_Cas_Sup_Bank_Mas_Code != "00");

            }
            else if (type == "H")
            {
                cR_Cas_Sup_Bank = db.CR_Cas_Sup_Bank.Where(b => b.CR_Cas_Sup_Bank_Com_Code == LessorCode && b.CR_Cas_Sup_Bank_Status == "H" && b.CR_Cas_Sup_Bank_Mas_Code != "00");

            }
            else
            {
                cR_Cas_Sup_Bank = db.CR_Cas_Sup_Bank.Where(b => b.CR_Cas_Sup_Bank_Com_Code == LessorCode && b.CR_Cas_Sup_Bank_Status == "A" && b.CR_Cas_Sup_Bank_Mas_Code != "00");

            }
            return PartialView(cR_Cas_Sup_Bank);
        }

                // GET: CasSupBank/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_Bank cR_Cas_Sup_Bank = db.CR_Cas_Sup_Bank.Find(id);
            if (cR_Cas_Sup_Bank == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Sup_Bank);
        }


        public CR_Cas_Administrative_Procedures GetLastRecord(string ProcedureCode, string sector)
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
            var ProcedureCode = "43";
            var autoInc = GetLastRecord(ProcedureCode, "1");
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

        // GET: CasSupBank/Create
        public ActionResult Create()
        {
            ViewBag.CR_Cas_Sup_Bank_Mas_Code = new SelectList(db.CR_Mas_Sup_Bank.Where(o => o.CR_Mas_Sup_Bank_Status=="A" && o.CR_Mas_Sup_Bank_Code!="00"),
                "CR_Mas_Sup_Bank_Code", "CR_Mas_Sup_Bank_Ar_Name");
            return View();
        }

        public CR_Cas_Sup_Bank GetLastRecord(string code)
        {
            var LessorCode = Session["LessorCode"].ToString();
            var Lrecord = db.CR_Cas_Sup_Bank.Where(b=>b.CR_Cas_Sup_Bank_Mas_Code==code && b.CR_Cas_Sup_Bank_Com_Code==LessorCode)
                .Max(b => b.CR_Cas_Sup_Bank_Code.Substring(b.CR_Cas_Sup_Bank_Code.Length - 2,2));

            CR_Cas_Sup_Bank B = new CR_Cas_Sup_Bank();
            if (Lrecord != null)
            {
                Int64 val = Int64.Parse(Lrecord) + 1;
               B.CR_Cas_Sup_Bank_Code = val.ToString("00");
            }
            else
            {
                B.CR_Cas_Sup_Bank_Code = "01";
            }
            return B;
        }
        // POST: CasSupBank/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Sup_Bank_Code,CR_Cas_Sup_Bank_Com_Code," +
            "CR_Cas_Sup_Bank_Mas_Code,CR_Cas_Sup_Bank_Account_No,CR_Cas_Sup_Bank_Ar_Name," +
            "CR_Cas_Sup_Bank_En_Name,CR_Cas_Sup_Bank_Status,CR_Cas_Sup_Bank_Reasons")]
            CR_Cas_Sup_Bank cR_Cas_Sup_Bank)
        {
            var Account = db.CR_Cas_Sup_Bank.Any(b=>b.CR_Cas_Sup_Bank_Account_No==cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Account_No);
            var ArbName = db.CR_Cas_Sup_Bank.Any(a=>a.CR_Cas_Sup_Bank_Ar_Name==cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Ar_Name);
            var EngName = db.CR_Cas_Sup_Bank.Any(a => a.CR_Cas_Sup_Bank_En_Name == cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_En_Name);
            if (ModelState.IsValid && !Account && !ArbName && !EngName && cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Ar_Name!=null && cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_En_Name!=null
                && cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Account_No!=null)
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
               
                ////////////////////////////////////Documentation////////////////////////////////////////
                var auto = GetLastRecord(cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Mas_Code);
                var concat = LessorCode + cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Mas_Code+auto.CR_Cas_Sup_Bank_Code;

                var MasBank = db.CR_Mas_Sup_Bank.FirstOrDefault(b => b.CR_Mas_Sup_Bank_Code == cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Mas_Code);
                if (MasBank != null)
                {
                    cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Ar_Name = cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Ar_Name ;
                    cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_En_Name = cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_En_Name ;
                }
                else
                {
                    cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Ar_Name = cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Ar_Name;
                    cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_En_Name = cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_En_Name;
                }

                cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Code = concat;
                cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Com_Code = LessorCode;
                ///////////////////////////////Tracing//////////////////////////////////////
                SaveTracing(LessorCode, cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Code, "I", cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Reasons);
                ////////////////////////////////////////////////////////////////////////////
                cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status = "A";
                db.CR_Cas_Sup_Bank.Add(cR_Cas_Sup_Bank);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (Account)
            {
               ViewBag.AccountNo = "عفوا رقم الحساب متكرر";
            }
            if (ArbName)
            {
                ViewBag.ArbName = "عفوا وصف الحساب عربي متكرر";
            }
            if (EngName)
            {
                ViewBag.EngName = "عفوا وصف الحساب إنجليزي متكرر";
            }
            if (cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Account_No == null)
            {
                ViewBag.AccountNo = "عفوا هذا الحقل إجباري";
            }
            if (cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Ar_Name == null)
            {
                ViewBag.ArbName = "عفوا هذا الحقل إجباري";
            }
            if (cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_En_Name == null)
            {
                ViewBag.EngName = "عفوا هذا الحقل إجباري";
            }

            ViewBag.CR_Cas_Sup_Bank_Mas_Code = new SelectList(db.CR_Mas_Sup_Bank.Where(o => o.CR_Mas_Sup_Bank_Status == "A" && o.CR_Mas_Sup_Bank_Code != "00"),
                "CR_Mas_Sup_Bank_Code", "CR_Mas_Sup_Bank_Ar_Name");
            return View(cR_Cas_Sup_Bank);
        }

        // GET: CasSupBank/Edit/5
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
            CR_Cas_Sup_Bank cR_Cas_Sup_Bank = db.CR_Cas_Sup_Bank.Find(id);
            if (cR_Cas_Sup_Bank == null)
            {
                return HttpNotFound();
            }
            else
            {
                var CheckSalespoint = db.CR_Cas_Sup_SalesPoint.Any(c=>c.CR_Cas_Sup_SalesPoint_Bank_Code==cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Code);
                if (CheckSalespoint)
                {
                    ViewBag.Disable = "True";
                }
                else
                {
                    ViewBag.Disable = "False";
                }
                if (cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "A" ||
                    cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "Activated" ||
                    cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "1" ||
                    cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "Undeleted")
                {
                    ViewBag.stat = "حذف";
                    ViewBag.h = "إيقاف";
                }

                if (cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "D" ||
                    cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "Deleted" ||
                    cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "0")
                {
                    ViewBag.stat = "إسترجاع";
                    ViewBag.h = "إيقاف";
                   // ViewData["ReadOnly"] = "true";
                }

                if (cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "H" ||
                    cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "Hold" ||
                    cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "2")
                {
                    ViewBag.h = "تنشيط";
                    ViewBag.stat = "حذف";
                    //ViewData["ReadOnly"] = "true";
                }

                if (cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == null)
                {
                    ViewBag.h = "إيقاف";
                    ViewBag.stat = "حذف";
                }

                var salespoint = db.CR_Cas_Sup_SalesPoint.FirstOrDefault(b => b.CR_Cas_Sup_SalesPoint_Bank_Code == cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Code && b.CR_Cas_Sup_SalesPoint_Status!="D");
                if (salespoint != null)
                {
                    TempData["SalesPointNo"] = "T";
                }
                ViewBag.delete = cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status;

            }
            ViewBag.CR_Cas_Sup_Bank_Mas_Code = new SelectList(db.CR_Mas_Sup_Bank.Where(o => o.CR_Mas_Sup_Bank_Status == "A" && o.CR_Mas_Sup_Bank_Code != "00"),
            "CR_Mas_Sup_Bank_Code", "CR_Mas_Sup_Bank_Ar_Name",cR_Cas_Sup_Bank.CR_Mas_Sup_Bank.CR_Mas_Sup_Bank_Code);
            return View(cR_Cas_Sup_Bank);
        }

        // POST: CasSupBank/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Sup_Bank_Code,CR_Cas_Sup_Bank_Com_Code," +
            "CR_Cas_Sup_Bank_Mas_Code,CR_Cas_Sup_Bank_Account_No,CR_Cas_Sup_Bank_Ar_Name,CR_Cas_Sup_Bank_En_Name," +
            "CR_Cas_Sup_Bank_Status,CR_Cas_Sup_Bank_Reasons")] CR_Cas_Sup_Bank cR_Cas_Sup_Bank, string save, string delete, string hold)
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
                            var MasBank = db.CR_Mas_Sup_Bank.FirstOrDefault(b=>b.CR_Mas_Sup_Bank_Code==cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Mas_Code);
                            if (MasBank != null)
                            {
                                cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Ar_Name = cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Ar_Name;
                                cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_En_Name = cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_En_Name;
                            }
                            else
                            {
                                cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Ar_Name = cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Ar_Name ;
                                cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_En_Name = cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_En_Name ;
                            }
                            

                            db.Entry(cR_Cas_Sup_Bank).State = EntityState.Modified;
                            ///////////////////////////////Tracing//////////////////////////////////////
                            SaveTracing(LessorCode, cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Code, "U", cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Reasons);
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
                        cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status = "D";
                        db.Entry(cR_Cas_Sup_Bank).State = EntityState.Modified;
                        ///////////////////////////////Tracing//////////////////////////////////////
                        SaveTracing(LessorCode, cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Code, "D", cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Reasons);
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
                        cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status = "A";
                        db.Entry(cR_Cas_Sup_Bank).State = EntityState.Modified;
                        ///////////////////////////////Tracing//////////////////////////////////////
                        SaveTracing(LessorCode, cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Code, "A", cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Reasons);
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
                        cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status = "H";

                        db.Entry(cR_Cas_Sup_Bank).State = EntityState.Modified;
                        ///////////////////////////////Tracing//////////////////////////////////////
                        SaveTracing(LessorCode, cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Code, "H", cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Reasons);


                        /*var bankAccount = db.CR_Cas_Sup_Bank.Where(l=>l.CR_Cas_Sup_Bank_Account_No == cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Account_No);
                        foreach (var item in bankAccount)
                        {
                            item.CR_Cas_Sup_Sub_Com_Account_Status = "H";
                        }*/
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
                        cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status = "A";
                        db.Entry(cR_Cas_Sup_Bank).State = EntityState.Modified;
                        SaveTracing(LessorCode, cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Code, "A", cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Reasons);
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

            if (cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "A" ||
                    cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "Activated" ||
                    cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "1" ||
                    cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }

            if (cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "D" ||
                cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "Deleted" ||
                cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "0")
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
                ViewData["ReadOnly"] = "true";
            }

            if (cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "H" ||
                cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "Hold" ||
                cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
                ViewData["ReadOnly"] = "true";
            }

            if (cR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }

            ViewBag.CR_Cas_Sup_Bank_Mas_Code = new SelectList(db.CR_Mas_Sup_Bank.Where(o => o.CR_Mas_Sup_Bank_Status == "A" && o.CR_Mas_Sup_Bank_Code != "00"),
            "CR_Mas_Sup_Bank_Code", "CR_Mas_Sup_Bank_Ar_Name", cR_Cas_Sup_Bank.CR_Mas_Sup_Bank.CR_Mas_Sup_Bank_Code);

            return View(cR_Cas_Sup_Bank);
        }

        // GET: CasSupBank/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_Bank cR_Cas_Sup_Bank = db.CR_Cas_Sup_Bank.Find(id);
            if (cR_Cas_Sup_Bank == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Sup_Bank);
        }

        // POST: CasSupBank/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Cas_Sup_Bank cR_Cas_Sup_Bank = db.CR_Cas_Sup_Bank.Find(id);
            db.CR_Cas_Sup_Bank.Remove(cR_Cas_Sup_Bank);
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
