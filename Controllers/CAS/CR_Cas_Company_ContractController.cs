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

namespace RentCar.Controllers
{
    public class CR_Cas_Company_ContractController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: CR_Cas_Company_Contract
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

            Models.CAS.LoadAlerts lAlerts = new Models.CAS.LoadAlerts();
            lAlerts.GetExpiredDocs(LessorCode);
            var cR_Cas_Company_Contract = db.CR_Cas_Company_Contract.Where(c=>c.CR_Cas_Company_Contract_Lessor==LessorCode 
            && c.CR_Cas_Company_Contract_Status!="D" && c.CR_Cas_Company_Contract_Code!="18")
                .Include(c => c.CR_Mas_Com_Lessor).Include(c => c.CR_Mas_Sup_Sector).Include(c=>c.CR_Mas_Sup_Procedures);
            return View(cR_Cas_Company_Contract.ToList());
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

            IQueryable<CR_Cas_Company_Contract> cR_Cas_Company_Contract = null;
            if (type == "A")
            {
                 cR_Cas_Company_Contract = db.CR_Cas_Company_Contract.Where(c => c.CR_Cas_Company_Contract_Lessor == LessorCode && c.CR_Cas_Company_Contract_Status == "A"
                 && c.CR_Cas_Company_Contract_Code != "18")
                .Include(c => c.CR_Mas_Com_Lessor).Include(c => c.CR_Mas_Sup_Sector).Include(c => c.CR_Mas_Sup_Procedures);
            }
            else if (type == "N")
            {
                 cR_Cas_Company_Contract = db.CR_Cas_Company_Contract.Where(c => c.CR_Cas_Company_Contract_Lessor == LessorCode && c.CR_Cas_Company_Contract_Status == "N"
                 && c.CR_Cas_Company_Contract_Code != "18")
                .Include(c => c.CR_Mas_Com_Lessor).Include(c => c.CR_Mas_Sup_Sector).Include(c => c.CR_Mas_Sup_Procedures);
            }
            else if (type == "E")
            {
                 cR_Cas_Company_Contract = db.CR_Cas_Company_Contract.Where(c => c.CR_Cas_Company_Contract_Lessor == LessorCode && c.CR_Cas_Company_Contract_Status == "E"
                 && c.CR_Cas_Company_Contract_Code != "18")
                 .Include(c => c.CR_Mas_Com_Lessor).Include(c => c.CR_Mas_Sup_Sector).Include(c => c.CR_Mas_Sup_Procedures);
            }
            else if (type == "X")
            {
                 cR_Cas_Company_Contract = db.CR_Cas_Company_Contract.Where(c => c.CR_Cas_Company_Contract_Lessor == LessorCode && c.CR_Cas_Company_Contract_Status == "X"
                 && c.CR_Cas_Company_Contract_Code != "18")
                .Include(c => c.CR_Mas_Com_Lessor).Include(c => c.CR_Mas_Sup_Sector).Include(c => c.CR_Mas_Sup_Procedures);
            }
            else
            {
                 cR_Cas_Company_Contract = db.CR_Cas_Company_Contract.Where(c => c.CR_Cas_Company_Contract_Lessor == LessorCode && c.CR_Cas_Company_Contract_Status != "D"
                 && c.CR_Cas_Company_Contract_Code != "18")
                .Include(c => c.CR_Mas_Com_Lessor).Include(c => c.CR_Mas_Sup_Sector).Include(c => c.CR_Mas_Sup_Procedures);
            }


            return PartialView(cR_Cas_Company_Contract.ToList());
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

        public CR_Cas_Company_Contract GetContractLastRecord(string ProcedureCode, string sector)
        {
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var LessorCode = Session["LessorCode"].ToString();
            var Lrecord = db.CR_Cas_Company_Contract.Where(x => x.CR_Cas_Company_Contract_Lessor == LessorCode &&
                x.CR_Cas_Company_Contract_Code == ProcedureCode
                && x.CR_Cas_Company_Contract_Sector == sector
                && x.CR_Cas_Company_Contract_Year == y)
                .Max(x => x.CR_Cas_Company_Contract_No.Substring(x.CR_Cas_Company_Contract_No.Length - 7, 7));

            CR_Cas_Company_Contract T = new CR_Cas_Company_Contract();
            if (Lrecord != null)
            {
                Int64 val = Int64.Parse(Lrecord) + 1;
                T.CR_Cas_Company_Contract_No = val.ToString("0000000");
            }
            else
            {
                T.CR_Cas_Company_Contract_No = "0000001";
            }
            return T;
        }

        // GET: CR_Cas_Company_Contract/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Company_Contract cR_Cas_Company_Contract = db.CR_Cas_Company_Contract.Find(id);
            if (cR_Cas_Company_Contract == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Company_Contract);
        }

        // GET: CR_Cas_Company_Contract/Create
        public ActionResult Create(string LessorCode, string ProcCode ,string Cno)
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
            if (LessorCode != null &&  ProcCode != null)
            {
                ViewBag.proc = ProcCode;
               // ViewBag.branch = BranchCode;
                ViewBag.stat = "حذف";

                //CR_Cas_Company_Contract b;
                CR_Cas_Company_Contract cR_Cas_Company_Contract = db.CR_Cas_Company_Contract.OrderByDescending(z=>z.CR_Cas_Company_Contract_No).FirstOrDefault(z => z.CR_Cas_Company_Contract_Lessor == LessorCode
                && z.CR_Cas_Company_Contract_Code == ProcCode && z.CR_Cas_Company_Contract_No==Cno);
                if (cR_Cas_Company_Contract == null)
                {
                    return HttpNotFound();
                }
                if (cR_Cas_Company_Contract.CR_Cas_Company_Contract_No != null)
                {
                    cR_Cas_Company_Contract.CR_Cas_Company_Contract_No = cR_Cas_Company_Contract.CR_Cas_Company_Contract_No.Trim();
                }

                if (cR_Cas_Company_Contract.CR_Cas_Company_Contract_No == "" || cR_Cas_Company_Contract.CR_Cas_Company_Contract_No == null)
                {

                    ViewBag.DocDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                    ViewBag.StartDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                    ViewBag.EndDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                    ViewBag.Status = cR_Cas_Company_Contract.CR_Cas_Company_Contract_Status;
                }
                
                    if (cR_Cas_Company_Contract.CR_Cas_Company_Contract_Date == null)
                    {
                        ViewBag.DocDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                    }
                    else
                    {
                        ViewBag.DocDate = string.Format("{0:yyyy-MM-dd}", cR_Cas_Company_Contract.CR_Cas_Company_Contract_Date);
                    }

                    if (cR_Cas_Company_Contract.CR_Cas_Company_Contract_Start_Date == null)
                    {
                        ViewBag.StartDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                    }
                    else
                    {
                        ViewBag.StartDate = string.Format("{0:yyyy-MM-dd}", cR_Cas_Company_Contract.CR_Cas_Company_Contract_Start_Date);
                    }

                    if (cR_Cas_Company_Contract.CR_Cas_Company_Contract_End_Date == null)
                    {
                        ViewBag.EndDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                    }
                    else
                    {
                        ViewBag.EndDate = string.Format("{0:yyyy-MM-dd}", cR_Cas_Company_Contract.CR_Cas_Company_Contract_End_Date);
                    }
                    ViewBag.Status = cR_Cas_Company_Contract.CR_Cas_Company_Contract_Status;
                    ViewBag.ContractCode = cR_Cas_Company_Contract.CR_Cas_Company_Contract_No.Trim();
                
                return View(cR_Cas_Company_Contract);


            }


        
            return View();
        }

        // POST: CR_Cas_Company_Contract/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Company_Contract_No,CR_Cas_Company_Contract_Year," +
            "CR_Cas_Company_Contract_Sector,CR_Cas_Company_Contract_Code,CR_Cas_Company_Contract_Lessor," +
            "CR_Cas_Company_Contract_Number,CR_Cas_Company_Contract_Date,CR_Cas_Company_Contract_Start_Date," +
            "CR_Cas_Company_Contract_End_Date,CR_Cas_Company_Contract_Activation,CR_Cas_Company_Contract_About_To_Expire," +
            "CR_Cas_Company_Contract_Annual_Fees,CR_Cas_Company_Contract_Service_Fees,CR_Cas_Company_Contract_Discount_Rate," +
            "CR_Cas_Company_Contract_Tax_Rate,CR_Cas_Company_Contract_Tamm_User_Id,CR_Cas_Company_Contract_Tamm_User_Password," +
            "CR_Cas_Company_Contract_Status,CR_Cas_Company_Contract_Reasons")] 
            CR_Cas_Company_Contract cR_Cas_Company_Contract, DateTime CR_Cas_Company_Contract_Date, 
            DateTime CR_Cas_Company_Contract_Start_Date, DateTime CR_Cas_Company_Contract_End_Date,
            string ProcedureCode, string save, string delete, string ContractCode)
        {
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {     
                  
                    var checkContractExist = db.CR_Cas_Company_Contract.Any(f => f.CR_Cas_Company_Contract_No == cR_Cas_Company_Contract.CR_Cas_Company_Contract_No && f.CR_Cas_Company_Contract_Status=="A");
                    using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                    {
                        try
                        {
                            if (cR_Cas_Company_Contract.CR_Cas_Company_Contract_No != null && !checkContractExist)
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
                                Ad.CR_Cas_Administrative_Procedures_Doc_Date = CR_Cas_Company_Contract_Date;
                                Ad.CR_Cas_Administrative_Procedures_Doc_Start_Date = CR_Cas_Company_Contract_Start_Date;
                                Ad.CR_Cas_Administrative_Procedures_Doc_End_Date = CR_Cas_Company_Contract_End_Date;
                                Ad.CR_Cas_Administrative_Procedures_Doc_No = ""; // inconnu/////////////////////////////////////////////
                                Ad.CR_Cas_Administrative_Procedures_Reasons = cR_Cas_Company_Contract.CR_Cas_Company_Contract_Reasons;
                                db.CR_Cas_Administrative_Procedures.Add(Ad);
                                ////////////////////////////////////Contract////////////////////////////////////////
                                ///
                                var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.FirstOrDefault(x => x.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == LessorCode &&
                                x.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code == ProcedureCode);

                                var doc = db.CR_Cas_Company_Contract.OrderByDescending(z=>z.CR_Cas_Company_Contract_No).FirstOrDefault(x => x.CR_Cas_Company_Contract_Lessor == LessorCode &&
                                x.CR_Cas_Company_Contract_Code == ProcedureCode && x.CR_Cas_Company_Contract_No==cR_Cas_Company_Contract.CR_Cas_Company_Contract_No);
                                doc.CR_Cas_Company_Contract_Number = cR_Cas_Company_Contract.CR_Cas_Company_Contract_Number;
                                doc.CR_Cas_Company_Contract_Date = CR_Cas_Company_Contract_Date;
                                doc.CR_Cas_Company_Contract_Start_Date = CR_Cas_Company_Contract_Start_Date;
                                doc.CR_Cas_Company_Contract_End_Date = CR_Cas_Company_Contract_End_Date;
                                doc.CR_Cas_Company_Contract_Tax_Rate = cR_Cas_Company_Contract.CR_Cas_Company_Contract_Tax_Rate;
                                doc.CR_Cas_Company_Contract_Annual_Fees = cR_Cas_Company_Contract.CR_Cas_Company_Contract_Annual_Fees;
                                doc.CR_Cas_Company_Contract_Tamm_User_Id = cR_Cas_Company_Contract.CR_Cas_Company_Contract_Tamm_User_Id;
                                doc.CR_Cas_Company_Contract_Tamm_User_Password = cR_Cas_Company_Contract.CR_Cas_Company_Contract_Tamm_User_Password;

                                doc.CR_Cas_Company_Contract_Status = "A";

                                DateTime currentDate = (DateTime)doc.CR_Cas_Company_Contract_End_Date;
                                var nbr = Convert.ToDouble(mech.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire);
                                var d = currentDate.AddDays(-nbr);
                                doc.CR_Cas_Company_Contract_About_To_Expire = d;


                                if (doc.CR_Cas_Company_Contract_End_Date > DateTime.Now.AddDays(nbr))
                                {
                                    doc.CR_Cas_Company_Contract_Status = "A";

                                }
                                else if (doc.CR_Cas_Company_Contract_End_Date > DateTime.Now && doc.CR_Cas_Company_Contract_End_Date <= DateTime.Now.AddDays(nbr))
                                {
                                    doc.CR_Cas_Company_Contract_Status = "X";
                                }

                                db.Entry(doc).State = EntityState.Modified;
                                
                                ////////////////////////////////Save Contract services///////////////////////////////////////
                                var servicefee = db.CR_Mas_Sup_Service_Fee_Tamm.Where(x => x.CR_Mas_Sup_Service_Fee_Tamm_Status == "A");

                                List<CR_Mas_Service_Tamm_Contract> lService = new List<CR_Mas_Service_Tamm_Contract>();
                                CR_Mas_Service_Tamm_Contract Service;
                                foreach (var s in servicefee)
                                {
                                    Service = new CR_Mas_Service_Tamm_Contract();
                                    Service.CR_Mas_Service_Tamm_Contract_No = cR_Cas_Company_Contract.CR_Cas_Company_Contract_No;
                                    Service.CR_Mas_Service_Tamm_Contract_Code = s.CR_Mas_Sup_Service_Fee_Tamm_Code;
                                    Service.CR_Mas_Service_Tamm_Contract_Fees = s.CR_Mas_Sup_Service_Fee_Tamm_Value;

                                    lService.Add(Service);
                                }
                                lService.ForEach(sf => db.CR_Mas_Service_Tamm_Contract.Add(sf));
                                /////////////////////////////////////////////////////////////////////////////////////////////

                                db.SaveChanges();
                                TempData["TempModel"] = "Added";
                                dbTran.Commit();
                                return RedirectToAction("Index", "CR_Cas_Company_Contract");
                            }
                            else
                            {
                                if (cR_Cas_Company_Contract.CR_Cas_Company_Contract_No == null)
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
                        Ad.CR_Cas_Administrative_Procedures_Doc_Date = CR_Cas_Company_Contract_Date;
                        Ad.CR_Cas_Administrative_Procedures_Doc_Start_Date = CR_Cas_Company_Contract_Start_Date;
                        Ad.CR_Cas_Administrative_Procedures_Doc_End_Date = CR_Cas_Company_Contract_End_Date;
                        Ad.CR_Cas_Administrative_Procedures_Doc_No = ""; // inconnu/////////////////////////////////////////////
                        db.CR_Cas_Administrative_Procedures.Add(Ad);

                        //////////////////////////////////////////////////////////////////////////////////////

                        //////////////////////////////////Delete contract///////////////////////////////////
                        var c = db.CR_Cas_Company_Contract.FirstOrDefault(x => x.CR_Cas_Company_Contract_No == ContractCode);
                        if (c != null)
                        {
                            c.CR_Cas_Company_Contract_Status = "D";
                            db.Entry(c).State = EntityState.Modified;
                        }
                        ////////////////////////////////////////////////////////////////////////////////////
                        ///
                        ////////////////////////////////Add New Cas Contracts////////////////////////////////
                        
                            CR_Cas_Company_Contract cont=new CR_Cas_Company_Contract();

                            var ContractAutoInc = GetContractLastRecord(ProcedureCode,"1").CR_Cas_Company_Contract_No;
                            
                            cont.CR_Cas_Company_Contract_No = y+"-1-"+ ProcedureCode + "-"+LessorCode+"-"+ContractAutoInc;
                            cont.CR_Cas_Company_Contract_Lessor = LessorCode;

                            cont.CR_Cas_Company_Contract_Code = ProcedureCode;
                            cont.CR_Cas_Company_Contract_Year = y;
                            cont.CR_Cas_Company_Contract_Sector = "1";

                            cont.CR_Cas_Company_Contract_Number = null;
                            cont.CR_Cas_Company_Contract_Date = null;
                            cont.CR_Cas_Company_Contract_Start_Date = null;
                            cont.CR_Cas_Company_Contract_End_Date = null;
                            cont.CR_Cas_Company_Contract_Status = "N";
                        db.CR_Cas_Company_Contract.Add(cont);
                        /// //////////////////////////////////////////////////////////////////////////
                        db.SaveChanges();
                        TempData["TempModel"] = "Deleted";
                        ViewBag.stat = "حذف";

                        dbTran.Commit();
                        return RedirectToAction("Index", "CR_Cas_Company_Contract");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }
            }
            return RedirectToAction("Index", "CR_Cas_Company_Contract");
        }

        // GET: CR_Cas_Company_Contract/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Company_Contract cR_Cas_Company_Contract = db.CR_Cas_Company_Contract.Find(id);
            if (cR_Cas_Company_Contract == null)
            {
                return HttpNotFound();
            }
            ViewBag.CR_Cas_Company_Contract_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Company_Contract.CR_Cas_Company_Contract_Lessor);
            ViewBag.CR_Cas_Company_Contract_Sector = new SelectList(db.CR_Mas_Sup_Sector, "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name", cR_Cas_Company_Contract.CR_Cas_Company_Contract_Sector);
            return View(cR_Cas_Company_Contract);
        }

        // POST: CR_Cas_Company_Contract/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Company_Contract_No,CR_Cas_Company_Contract_Year,CR_Cas_Company_Contract_Sector,CR_Cas_Company_Contract_Code,CR_Cas_Company_Contract_Lessor,CR_Cas_Company_Contract_Number,CR_Cas_Company_Contract_Date,CR_Cas_Company_Contract_Start_Date,CR_Cas_Company_Contract_End_Date,CR_Cas_Company_Contract_Activation,CR_Cas_Company_Contract_About_To_Expire,CR_Cas_Company_Contract_Annual_Fees,CR_Cas_Company_Contract_Service_Fees,CR_Cas_Company_Contract_Discount_Rate,CR_Cas_Company_Contract_Tax_Rate,CR_Cas_Company_Contract_Tamm_User_Id,CR_Cas_Company_Contract_Tamm_User_Password,CR_Cas_Company_Contract_Status,CR_Cas_Company_Contract_Reasons")] CR_Cas_Company_Contract cR_Cas_Company_Contract)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cR_Cas_Company_Contract).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CR_Cas_Company_Contract_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Company_Contract.CR_Cas_Company_Contract_Lessor);
            ViewBag.CR_Cas_Company_Contract_Sector = new SelectList(db.CR_Mas_Sup_Sector, "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name", cR_Cas_Company_Contract.CR_Cas_Company_Contract_Sector);
            return View(cR_Cas_Company_Contract);
        }

        // GET: CR_Cas_Company_Contract/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Company_Contract cR_Cas_Company_Contract = db.CR_Cas_Company_Contract.Find(id);
            if (cR_Cas_Company_Contract == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Company_Contract);
        }

        // POST: CR_Cas_Company_Contract/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Cas_Company_Contract cR_Cas_Company_Contract = db.CR_Cas_Company_Contract.Find(id);
            db.CR_Cas_Company_Contract.Remove(cR_Cas_Company_Contract);
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
