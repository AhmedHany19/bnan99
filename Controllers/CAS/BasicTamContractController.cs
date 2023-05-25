using RentCar.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace RentCar.Controllers.CAS
{
    public class BasicTamContractController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: BasicTamContract
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
            var cR_Mas_Basic_Contract = db.CR_Mas_Basic_Contract.Include(c => c.CR_Mas_Com_Lessor).Include(c => c.CR_Mas_Sup_Sector);
            return View(cR_Mas_Basic_Contract.ToList());

        }

        public PartialViewResult TammFees()
        {
            var LessorCode = "";
            var UserLogin = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null)
                {
                    RedirectToAction("Login", "Account");
                }
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }
            var Fees = db.CR_Mas_Sup_Service_Fee_Tamm.Where(f => f.CR_Mas_Sup_Service_Fee_Tamm_Status != "D");
            return PartialView(Fees);
        }


        // GET: BasicTamContract/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Basic_Contract cR_Mas_Basic_Contract = db.CR_Mas_Basic_Contract.Find(id);
            if (cR_Mas_Basic_Contract == null)
            {
                return HttpNotFound();
            }
            return View(cR_Mas_Basic_Contract);
        }



        //public CR_Cas_Administrative_Procedures GetLastRecord()
        //{
        //    var Lrecord = db.CR_Cas_Administrative_Procedures.Max(Lr => Lr.CR_Cas_Administrative_Procedures_No.Substring(Lr.CR_Cas_Administrative_Procedures_No.Length - 7, 7));
        //    CR_Cas_Administrative_Procedures T = new CR_Cas_Administrative_Procedures();
        //    if (Lrecord != null)
        //    {
        //        Int64 val = Int64.Parse(Lrecord) + 1;
        //        T.CR_Cas_Administrative_Procedures_No = val.ToString("0000000");
        //    }
        //    else
        //    {
        //        T.CR_Cas_Administrative_Procedures_No = "0000001";
        //    }
        //    return T;
        //}



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







        // GET: BasicTamContract/Create
        public ActionResult Create(string LessorCode, string BranchCode, string ProcCode)
        {
            var Lcode = "";
            var UserLogin = "";
            try
            {
                Lcode = Session["LessorCode"].ToString();
                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || Lcode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
            if (LessorCode != null && BranchCode != null && ProcCode != null)
            {
                ViewBag.proc = ProcCode;
                ViewBag.branch = BranchCode;
                ViewBag.stat = "حذف";

                CR_Mas_Basic_Contract b;
                CR_Cas_Sup_Branch_Documentation cR_Cas_Sup_Branch_Documentation = db.CR_Cas_Sup_Branch_Documentation.FirstOrDefault(z => z.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode
                && z.CR_Cas_Sup_Branch_Documentation_Branch_Code == BranchCode && z.CR_Cas_Sup_Branch_Documentation_Code == ProcCode);
                if (cR_Cas_Sup_Branch_Documentation == null)
                {
                    return HttpNotFound();
                }
                if (cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_No != null)
                {
                    cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_No = cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_No.Trim();
                }

                if (cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_No == "" || cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_No == null)
                {

                    ViewBag.DocDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                    ViewBag.StartDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                    ViewBag.EndDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                    ViewBag.Status = cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Status;
                }
                else
                {
                    b = db.CR_Mas_Basic_Contract.FirstOrDefault(x => x.CR_Mas_Basic_Contract_No == cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_No);
                    if (b != null)
                    {
                        if (cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Date == null)
                        {
                            ViewBag.DocDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                        }
                        else
                        {
                            ViewBag.DocDate = string.Format("{0:yyyy-MM-dd}", cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Date);
                        }

                        if (cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Start_Date == null)
                        {
                            ViewBag.StartDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                        }
                        else
                        {
                            ViewBag.StartDate = string.Format("{0:yyyy-MM-dd}", cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_Start_Date);
                        }

                        if (cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_End_Date == null)
                        {
                            ViewBag.EndDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                        }
                        else
                        {
                            ViewBag.EndDate = string.Format("{0:yyyy-MM-dd}", cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_End_Date);
                        }
                        ViewBag.Status = b.CR_Mas_Basic_Contract_Status;
                        ViewBag.ContractCode = cR_Cas_Sup_Branch_Documentation.CR_Cas_Sup_Branch_Documentation_No.Trim();
                    }
                    else
                    {
                        ViewBag.DocDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                        ViewBag.StartDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                        ViewBag.EndDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                    }
                    return View(b);


                }


            }
            return View();
        }

        // POST: BasicTamContract/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Mas_Basic_Contract_No,CR_Mas_Basic_Contract_Year," +
            "CR_Mas_Basic_Contract_Sector,CR_Mas_Basic_Contract_Code,CR_Mas_Basic_Contract_Lessor," +
            "CR_Mas_Basic_Contract_Com_Code,CR_Mas_Basic_Contract_Date,CR_Mas_Basic_Contract_Start_Date," +
            "CR_Mas_Basic_Contract_End_Date,CR_Mas_Basic_Contract_Annual_Fees,CR_Mas_Basic_Contract_Service_Fees," +
            "CR_Mas_Basic_Contract_Discount_Rate,CR_Mas_Basic_Contract_Tax_Rate,CR_Mas_Basic_Contract_Tamm_User_Id," +
            "CR_Mas_Basic_Contract_Tamm_User_PassWord,CR_Mas_Basic_Contract_Status,CR_Mas_Basic_Contract_Reasons")]
        CR_Mas_Basic_Contract cR_Mas_Basic_Contract, DateTime CR_Mas_Basic_Contract_Date, DateTime CR_Mas_Basic_Contract_Start_Date, DateTime CR_Mas_Basic_Contract_End_Date,
            string ProcedureCode, string branchCode, string save, string delete, string ContractCode)
        {
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    var checkContractExist = db.CR_Mas_Basic_Contract.Any(f => f.CR_Mas_Basic_Contract_No == cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_No);
                    using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                    {
                        try
                        {
                            if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_No != null && !checkContractExist)
                            {

                                ///////////////////////////////Tracing//////////////////////////////////////
                                CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
                                DateTime year = DateTime.Now;
                                var y = year.ToString("yy");
                                var sector = "1";
                                var autoInc = GetLastRecord(ProcedureCode, sector);
                                var LessorCode = Session["LessorCode"].ToString();
                                Ad.CR_Cas_Administrative_Procedures_No = y + "-" + sector + "-" + ProcedureCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
                                Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
                                string currentTime = DateTime.Now.ToString("HH:mm:ss");
                                Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
                                Ad.CR_Cas_Administrative_Procedures_Year = y;
                                Ad.CR_Cas_Administrative_Procedures_Sector = sector;
                                Ad.CR_Cas_Administrative_Procedures_Code = ProcedureCode;
                                Ad.CR_Cas_Administrative_Int_Procedures_Code = Int32.Parse(ProcedureCode);
                                Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
                                Ad.CR_Cas_Administrative_Procedures_Targeted_Action = ""; // inconnu/////////////////////////////////////////////
                                Ad.CR_Cas_Administrative_Procedures_User_Insert = Session["UserLogin"].ToString();
                                Ad.CR_Cas_Administrative_Procedures_Type = "I";
                                Ad.CR_Cas_Administrative_Procedures_Action = true;
                                Ad.CR_Cas_Administrative_Procedures_Doc_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date;
                                Ad.CR_Cas_Administrative_Procedures_Doc_Start_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date;
                                Ad.CR_Cas_Administrative_Procedures_Doc_End_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date;
                                Ad.CR_Cas_Administrative_Procedures_Doc_No = ""; // inconnu/////////////////////////////////////////////
                                db.CR_Cas_Administrative_Procedures.Add(Ad);
                                ////////////////////////////////////Documentation////////////////////////////////////////
                                ///
                                var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.FirstOrDefault(x => x.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == LessorCode &&
                                x.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code == ProcedureCode);

                                var doc = db.CR_Cas_Sup_Branch_Documentation.FirstOrDefault(x => x.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode &&
                                x.CR_Cas_Sup_Branch_Documentation_Branch_Code == branchCode && x.CR_Cas_Sup_Branch_Documentation_Code == ProcedureCode);
                                doc.CR_Cas_Sup_Branch_Documentation_No = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_No;
                                doc.CR_Cas_Sup_Branch_Documentation_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date;
                                doc.CR_Cas_Sup_Branch_Documentation_Start_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date;
                                doc.CR_Cas_Sup_Branch_Documentation_End_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date;
                                doc.CR_Cas_Sup_Branch_Documentation_Status = "A";

                                DateTime currentDate = (DateTime)cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date;
                                var nbr = Convert.ToDouble(mech.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire);
                                var d = currentDate.AddDays(-nbr);
                                doc.CR_Cas_Sup_Branch_Documentation_About_To_Expire = d;


                                if (doc.CR_Cas_Sup_Branch_Documentation_End_Date > DateTime.Now.AddDays(nbr))
                                {
                                    doc.CR_Cas_Sup_Branch_Documentation_Status = "A";

                                }
                                else if (doc.CR_Cas_Sup_Branch_Documentation_End_Date > DateTime.Now && doc.CR_Cas_Sup_Branch_Documentation_End_Date <= DateTime.Now.AddDays(nbr))
                                {
                                    doc.CR_Cas_Sup_Branch_Documentation_Status = "X";
                                }

                                db.Entry(doc).State = EntityState.Modified;
                                /////////////////////////////////////////////Save contract////////////////////////////////
                                cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date;
                                cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date;
                                cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date;
                                //cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_No = y + "-" + sector + "-" + ProcedureCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
                                cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Year = y;
                                cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Sector = "1";
                                cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Code = ProcedureCode;
                                cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Lessor = LessorCode;
                                cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Com_Code = null;

                                cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status = "A";
                                db.CR_Mas_Basic_Contract.Add(cR_Mas_Basic_Contract);
                                /////////////////////////////////////////////////////////////////////////////////////////////
                                ///
                                ////////////////////////////////Save Contract services///////////////////////////////////////
                                var servicefee = db.CR_Mas_Sup_Service_Fee_Tamm.Where(x => x.CR_Mas_Sup_Service_Fee_Tamm_Status == "A");

                                List<CR_Mas_Service_Tamm_Contract> lService = new List<CR_Mas_Service_Tamm_Contract>();
                                CR_Mas_Service_Tamm_Contract Service;
                                foreach (var s in servicefee)
                                {
                                    Service = new CR_Mas_Service_Tamm_Contract();
                                    Service.CR_Mas_Service_Tamm_Contract_No = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_No;
                                    Service.CR_Mas_Service_Tamm_Contract_Code = s.CR_Mas_Sup_Service_Fee_Tamm_Code;
                                    Service.CR_Mas_Service_Tamm_Contract_Fees = s.CR_Mas_Sup_Service_Fee_Tamm_Value;

                                    lService.Add(Service);
                                }
                                lService.ForEach(sf => db.CR_Mas_Service_Tamm_Contract.Add(sf));
                                /////////////////////////////////////////////////////////////////////////////////////////////

                                db.SaveChanges();
                                TempData["TempModel"] = "Added";
                                dbTran.Commit();
                                return RedirectToAction("Index", "CasDocumentationContract");
                            }
                            else
                            {
                                if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_No == null)
                                {
                                    ViewBag.ContractNo = "عفوا هذا الحقل إجباري";
                                }

                                if (checkContractExist)
                                {
                                    ViewBag.ContractNo = "عفوا هذا العقد موجود";
                                }
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
            if (delete == "Delete" || delete == "حذف")
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        /////////////////////////////////////Delete Tracing//////////////////////////////////////

                        CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
                        DateTime year = DateTime.Now;
                        var y = year.ToString("yy");
                        var sector = "1";
                        var autoInc = GetLastRecord(ProcedureCode, sector);
                        var LessorCode = Session["LessorCode"].ToString();
                        Ad.CR_Cas_Administrative_Procedures_No = y + "-" + sector + "-" + ProcedureCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
                        Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
                        string currentTime = DateTime.Now.ToString("HH:mm:ss");
                        Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
                        Ad.CR_Cas_Administrative_Procedures_Year = y;
                        Ad.CR_Cas_Administrative_Procedures_Sector = sector;
                        Ad.CR_Cas_Administrative_Procedures_Code = ProcedureCode;
                        Ad.CR_Cas_Administrative_Int_Procedures_Code = Int32.Parse(ProcedureCode);
                        Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
                        Ad.CR_Cas_Administrative_Procedures_Targeted_Action = ""; // inconnu/////////////////////////////////////////////
                        Ad.CR_Cas_Administrative_Procedures_User_Insert = Session["UserLogin"].ToString();
                        Ad.CR_Cas_Administrative_Procedures_Type = "D";
                        Ad.CR_Cas_Administrative_Procedures_Action = true;
                        Ad.CR_Cas_Administrative_Procedures_Doc_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date;
                        Ad.CR_Cas_Administrative_Procedures_Doc_Start_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date;
                        Ad.CR_Cas_Administrative_Procedures_Doc_End_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date;
                        Ad.CR_Cas_Administrative_Procedures_Doc_No = ""; // inconnu/////////////////////////////////////////////
                        db.CR_Cas_Administrative_Procedures.Add(Ad);

                        //////////////////////////////////////////////////////////////////////////////////////

                        //////////////////////////////////Delete contract///////////////////////////////////
                        var c = db.CR_Mas_Basic_Contract.FirstOrDefault(x => x.CR_Mas_Basic_Contract_No == ContractCode);
                        if (c != null)
                        {
                            c.CR_Mas_Basic_Contract_Status = "D";
                            db.Entry(c).State = EntityState.Modified;
                        }
                        ////////////////////////////////////////////////////////////////////////////////////
                        ///
                        //////////////////////////////////Delete Documentation///////////////////////////////////
                        var d = db.CR_Cas_Sup_Branch_Documentation.FirstOrDefault(x => x.CR_Cas_Sup_Branch_Documentation_No == ContractCode);
                        if (d != null)
                        {
                            d.CR_Cas_Sup_Branch_Documentation_No = "";
                            d.CR_Cas_Sup_Branch_Documentation_Date = null;
                            d.CR_Cas_Sup_Branch_Documentation_Start_Date = null;
                            d.CR_Cas_Sup_Branch_Documentation_End_Date = null;
                            d.CR_Cas_Sup_Branch_Documentation_About_To_Expire = null;
                            d.CR_Cas_Sup_Branch_Documentation_Status = "N";
                            db.Entry(d).State = EntityState.Modified;
                        }
                        ////////////////////////////////////////////////////////////////////////////////////

                        db.SaveChanges();
                        TempData["TempModel"] = "Deleted";
                        ViewBag.stat = "حذف";

                        dbTran.Commit();
                        return RedirectToAction("Index", "CasDocumentationContract");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }
            }
            return RedirectToAction("Index", "CasDocumentationContract");
        }

        // GET: BasicTamContract/Edit/5
        public ActionResult Edit(string id, string id1)
        {
            if (id1 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Basic_Contract cR_Mas_Basic_Contract = db.CR_Mas_Basic_Contract.Find(id1);
            if (cR_Mas_Basic_Contract == null)
            {
                return HttpNotFound();
            }
            ViewBag.CR_Mas_Basic_Contract_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Lessor);
            ViewBag.CR_Mas_Basic_Contract_Sector = new SelectList(db.CR_Mas_Sup_Sector, "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name", cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Sector);
            return View(cR_Mas_Basic_Contract);
        }

        // POST: BasicTamContract/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Mas_Basic_Contract_No,CR_Mas_Basic_Contract_Year,CR_Mas_Basic_Contract_Sector,CR_Mas_Basic_Contract_Code,CR_Mas_Basic_Contract_Lessor,CR_Mas_Basic_Contract_Com_Code,CR_Mas_Basic_Contract_Date,CR_Mas_Basic_Contract_Start_Date,CR_Mas_Basic_Contract_End_Date,CR_Mas_Basic_Contract_Annual_Fees,CR_Mas_Basic_Contract_Service_Fees,CR_Mas_Basic_Contract_Discount_Rate,CR_Mas_Basic_Contract_Tax_Rate,CR_Mas_Basic_Contract_Tamm_User_Id,CR_Mas_Basic_Contract_Tamm_User_PassWord,CR_Mas_Basic_Contract_Status,CR_Mas_Basic_Contract_Reasons")] CR_Mas_Basic_Contract cR_Mas_Basic_Contract)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cR_Mas_Basic_Contract).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CR_Mas_Basic_Contract_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Lessor);
            ViewBag.CR_Mas_Basic_Contract_Sector = new SelectList(db.CR_Mas_Sup_Sector, "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name", cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Sector);
            return View(cR_Mas_Basic_Contract);
        }

        // GET: BasicTamContract/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Basic_Contract cR_Mas_Basic_Contract = db.CR_Mas_Basic_Contract.Find(id);
            if (cR_Mas_Basic_Contract == null)
            {
                return HttpNotFound();
            }
            return View(cR_Mas_Basic_Contract);
        }

        // POST: BasicTamContract/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Mas_Basic_Contract cR_Mas_Basic_Contract = db.CR_Mas_Basic_Contract.Find(id);
            db.CR_Mas_Basic_Contract.Remove(cR_Mas_Basic_Contract);
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
