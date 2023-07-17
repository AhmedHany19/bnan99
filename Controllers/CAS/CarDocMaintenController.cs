using RentCar.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace RentCar.Controllers.CAS
{
    public class CarDocMaintenController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: CarDocMainten
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

            ViewBag.StartDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            ViewBag.EndDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            var cR_Cas_Sup_Car_Doc_Mainten = db.CR_Cas_Sup_Car_Doc_Mainten.Where(x => x.CR_Cas_Sup_Car_Doc_Mainten_Type == "3" &&
                x.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Lessor_Code == LessorCode)
                .Include(c => c.CR_Cas_Sup_Car_Information).Include(c => c.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Branch).Include(b => b.CR_Mas_Sup_Procedures);

            return View(cR_Cas_Sup_Car_Doc_Mainten.ToList());
        }

        public PartialViewResult PartialIndex(string type, string StartDate, string EndDate)
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

            IQueryable<CR_Cas_Sup_Car_Doc_Mainten> cR_Cas_Sup_Car_Doc_Mainten = null;
            if (type == "A")
            {
                cR_Cas_Sup_Car_Doc_Mainten = db.CR_Cas_Sup_Car_Doc_Mainten.Where(x => x.CR_Cas_Sup_Car_Doc_Mainten_Type == "3" &&
                x.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Lessor_Code == LessorCode && x.CR_Cas_Sup_Car_Doc_Mainten_Status == "A" && x.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status != "D")
                .Include(c => c.CR_Cas_Sup_Car_Information).Include(c => c.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Branch).Include(b => b.CR_Mas_Sup_Procedures);
            }
            else if (type == "N")
            {
                cR_Cas_Sup_Car_Doc_Mainten = db.CR_Cas_Sup_Car_Doc_Mainten.Where(x => x.CR_Cas_Sup_Car_Doc_Mainten_Type == "3" &&
              x.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Lessor_Code == LessorCode && x.CR_Cas_Sup_Car_Doc_Mainten_Status == "N" && x.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status != "D")
              .Include(c => c.CR_Cas_Sup_Car_Information).Include(c => c.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Branch).Include(b => b.CR_Mas_Sup_Procedures);
            }
            else if (type == "D")
            {
                cR_Cas_Sup_Car_Doc_Mainten = db.CR_Cas_Sup_Car_Doc_Mainten.Where(x => x.CR_Cas_Sup_Car_Doc_Mainten_Type == "3" &&
              x.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Lessor_Code == LessorCode && x.CR_Cas_Sup_Car_Doc_Mainten_Status == "D" && x.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status != "D")
              .Include(c => c.CR_Cas_Sup_Car_Information).Include(c => c.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Branch).Include(b => b.CR_Mas_Sup_Procedures);
            }
            else if (type == "E")
            {
                cR_Cas_Sup_Car_Doc_Mainten = db.CR_Cas_Sup_Car_Doc_Mainten.Where(x => x.CR_Cas_Sup_Car_Doc_Mainten_Type == "3" &&
              x.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Lessor_Code == LessorCode && x.CR_Cas_Sup_Car_Doc_Mainten_Status == "E" && x.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status != "D")
              .Include(c => c.CR_Cas_Sup_Car_Information).Include(c => c.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Branch).Include(b => b.CR_Mas_Sup_Procedures);
            }
            else if (type == "X")
            {
                cR_Cas_Sup_Car_Doc_Mainten = db.CR_Cas_Sup_Car_Doc_Mainten.Where(x => x.CR_Cas_Sup_Car_Doc_Mainten_Type == "3" &&
              x.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Lessor_Code == LessorCode && x.CR_Cas_Sup_Car_Doc_Mainten_Status == "X" && x.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status != "D")
              .Include(c => c.CR_Cas_Sup_Car_Information).Include(c => c.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Branch).Include(b => b.CR_Mas_Sup_Procedures);
            }
            else if (type == "Date" && StartDate != "" && EndDate != "")
            {
                DateTime sd = Convert.ToDateTime(StartDate);
                DateTime ed = Convert.ToDateTime(EndDate);
                cR_Cas_Sup_Car_Doc_Mainten = db.CR_Cas_Sup_Car_Doc_Mainten.Where(c => c.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode
                  && c.CR_Cas_Sup_Car_Doc_Mainten_Type == "3"
                  && c.CR_Cas_Sup_Car_Doc_Mainten_Date >= sd && c.CR_Cas_Sup_Car_Doc_Mainten_Date <= ed).OrderByDescending(d => d.CR_Cas_Sup_Car_Doc_Mainten_Date)
                    .Include(c => c.CR_Cas_Sup_Car_Information).Include(car => car.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Branch).Include(b => b.CR_Mas_Sup_Procedures);
            }
            else if (type == "All")
            {
                cR_Cas_Sup_Car_Doc_Mainten = db.CR_Cas_Sup_Car_Doc_Mainten.Where(x => x.CR_Cas_Sup_Car_Doc_Mainten_Type == "3" &&
              x.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Lessor_Code == LessorCode && x.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status != "D")
              .Include(c => c.CR_Cas_Sup_Car_Information).Include(c => c.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Branch).Include(b => b.CR_Mas_Sup_Procedures).AsNoTracking();
            }
            else
            {
                cR_Cas_Sup_Car_Doc_Mainten = db.CR_Cas_Sup_Car_Doc_Mainten.Where(x => x.CR_Cas_Sup_Car_Doc_Mainten_Type == "3" &&
              x.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Lessor_Code == LessorCode && x.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status!="D"
              && (x.CR_Cas_Sup_Car_Doc_Mainten_Status=="E" || x.CR_Cas_Sup_Car_Doc_Mainten_Status=="N"))
              .Include(c => c.CR_Cas_Sup_Car_Information).Include(c => c.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Branch).Include(b => b.CR_Mas_Sup_Procedures).AsNoTracking();
            }
            return PartialView(cR_Cas_Sup_Car_Doc_Mainten);
        }

        // GET: CarDocMainten/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_Car_Doc_Mainten cR_Cas_Sup_Car_Doc_Mainten = db.CR_Cas_Sup_Car_Doc_Mainten.Find(id);
            if (cR_Cas_Sup_Car_Doc_Mainten == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Sup_Car_Doc_Mainten);
        }

        // GET: CarDocMainten/Create
        public ActionResult Create()
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

            ViewBag.CR_Cas_Sup_Car_Doc_Mainten_Code = new SelectList(db.CR_Mas_Sup_Procedures, "CR_Mas_Sup_Procedures_Code", "CR_Mas_Sup_Procedures_Type");
            ViewBag.CR_Cas_Sup_Car_Doc_Mainten_Serail_No = new SelectList(db.CR_Cas_Sup_Car_Information, "CR_Cas_Sup_Car_Serail_No", "CR_Cas_Sup_Car_Lessor_Code");
            return View();
        }

        // POST: CarDocMainten/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Sup_Car_Doc_Mainten_Serail_No,CR_Cas_Sup_Car_Doc_Mainten_Code," +
            "CR_Cas_Sup_Car_Doc_Mainten_No,CR_Cas_Sup_Car_Doc_Mainten_Date,CR_Cas_Sup_Car_Doc_Mainten_Start_Date," +
            "CR_Cas_Sup_Car_Doc_Mainten_End_Date,CR_Cas_Sup_Car_Doc_Mainten_Activation,CR_Cas_Sup_Car_Doc_Mainten_Limit_KM," +
            "CR_Cas_Sup_Car_Doc_Mainten_About_To_Expire,CR_Cas_Sup_Car_Doc_Mainten_Default_KM,CR_Cas_Sup_Car_Doc_Mainten_Status," +
            "CR_Cas_Sup_Car_Doc_Mainten_Procedure_Type")] CR_Cas_Sup_Car_Doc_Mainten cR_Cas_Sup_Car_Doc_Mainten)
        {
            if (ModelState.IsValid)
            {
                db.CR_Cas_Sup_Car_Doc_Mainten.Add(cR_Cas_Sup_Car_Doc_Mainten);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CR_Cas_Sup_Car_Doc_Mainten_Code = new SelectList(db.CR_Mas_Sup_Procedures, "CR_Mas_Sup_Procedures_Code", "CR_Mas_Sup_Procedures_Type", cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_Code);
            ViewBag.CR_Cas_Sup_Car_Doc_Mainten_Serail_No = new SelectList(db.CR_Cas_Sup_Car_Information, "CR_Cas_Sup_Car_Serail_No", "CR_Cas_Sup_Car_Lessor_Code", cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_Serail_No);
            return View(cR_Cas_Sup_Car_Doc_Mainten);
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

        public void SaveTracing(string ProcCode, string Lessor, string CarSerialNo,string DocNo, DateTime docDate, DateTime StartDate, DateTime EndDate, string procType, string reason)
        {
            CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var sector = "1";
            var ProcedureCode = ProcCode;
            var autoInc = GetLastRecord(ProcedureCode, sector);
            var LessorCode = Lessor;
            Ad.CR_Cas_Administrative_Procedures_No = y + "-" + sector + "-" + ProcedureCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
            Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
            Ad.CR_Cas_Administrative_Procedures_Year = y;
            Ad.CR_Cas_Administrative_Procedures_Sector = sector;
            Ad.CR_Cas_Administrative_Procedures_Code = ProcedureCode;
            Ad.CR_Cas_Administrative_Int_Procedures_Code = int.Parse(ProcedureCode);
            Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
            Ad.CR_Cas_Administrative_Procedures_Targeted_Action = CarSerialNo;
            Ad.CR_Cas_Administrative_Procedures_User_Insert = Session["UserLogin"].ToString();
            Ad.CR_Cas_Administrative_Procedures_Doc_No = DocNo;
            Ad.CR_Cas_Administrative_Procedures_Type = procType;
            Ad.CR_Cas_Administrative_Procedures_Action = true;
            Ad.CR_Cas_Administrative_Procedures_Doc_Date = docDate;
            Ad.CR_Cas_Administrative_Procedures_Doc_Start_Date = StartDate;
            Ad.CR_Cas_Administrative_Procedures_Doc_End_Date = EndDate;
            Ad.CR_Cas_Administrative_Procedures_Reasons = reason;
            db.CR_Cas_Administrative_Procedures.Add(Ad);
        }






        // GET: CarDocMainten/Edit/5
        public ActionResult Edit(string id1, string id2)
        {
            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                BranchCode = Session["BranchCode"].ToString();
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
            if (id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_Car_Doc_Mainten cR_Cas_Sup_Car_Doc_Mainten = db.CR_Cas_Sup_Car_Doc_Mainten.Find(id1, id2);
            if (cR_Cas_Sup_Car_Doc_Mainten == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_Status == "A")
                {
                    ViewBag.st = "A";
                }
               if(cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_Date!=null)
                {
                    ViewBag.DocDate = string.Format("{0:yyyy-MM-dd}", cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_Date);
                }
                else
                {
                    ViewBag.DocDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                }
                if (cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_Start_Date != null)
                {
                    ViewBag.StartDate = string.Format("{0:yyyy-MM-dd}", cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_Start_Date);
                }
                else
                {
                    ViewBag.StartDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                }
                if (cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_End_Date != null)
                {
                    ViewBag.EndDate = string.Format("{0:yyyy-MM-dd}", cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_End_Date);
                }
                else
                {
                    var currentdate = DateTime.Now;
                    var NewDate = currentdate.AddDays(30);
                    ViewBag.EndDate = string.Format("{0:yyyy-MM-dd}", NewDate);
                }
                //cR_Cas_Sup_Car_Doc_Mainten
            }

            //ViewBag.CR_Cas_Sup_Car_Doc_Mainten_Code = new SelectList(db.CR_Mas_Sup_Procedures, "CR_Mas_Sup_Procedures_Code", "CR_Mas_Sup_Procedures_Type", cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_Code);
            //ViewBag.CR_Cas_Sup_Car_Doc_Mainten_Serail_No = new SelectList(db.CR_Cas_Sup_Car_Information, "CR_Cas_Sup_Car_Serail_No", "CR_Cas_Sup_Car_Lessor_Code", cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_Serail_No);
            return View(cR_Cas_Sup_Car_Doc_Mainten);
        }

        // POST: CarDocMainten/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Sup_Car_Doc_Mainten_Serail_No," +
            "CR_Cas_Sup_Car_Doc_Mainten_Code," +
            "CR_Cas_Sup_Car_Doc_Mainten_No," +
            "CR_Cas_Sup_Car_Doc_Mainten_Date," +
            "CR_Cas_Sup_Car_Doc_Mainten_Start_Date," +
            "CR_Cas_Sup_Car_Doc_Mainten_End_Date," +
            "CR_Cas_Sup_Car_Doc_Mainten_Activation," +
            "CR_Cas_Sup_Car_Doc_Mainten_Limit_KM," +
            "CR_Cas_Sup_Car_Doc_Mainten_About_To_Expire," +
            "CR_Cas_Sup_Car_Doc_Mainten_Default_KM," +
            "CR_Cas_Sup_Car_Doc_Mainten_Status," +
            "CR_Cas_Sup_Car_Doc_Mainten_Procedure_Type," +
            "CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code," +
            "CR_Cas_Sup_Car_Doc_Mainten_Branch_Code," +
            "CR_Cas_Sup_Car_Doc_Mainten_Type," +
            "CR_Cas_Sup_Car_Doc_Mainten_End_KM")] CR_Cas_Sup_Car_Doc_Mainten cR_Cas_Sup_Car_Doc_Mainten,
            string save, string delete,string Reasons)
           
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(save))
                {
                    var LessorCode = "";
                    var UserLogin = "";
                    var BranchCode = "";
                    using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                    {
                        try
                        {
                            LessorCode = Session["LessorCode"].ToString();
                            BranchCode = Session["BranchCode"].ToString();

                            UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                            if (UserLogin == null || LessorCode == null || BranchCode == null)
                            {
                                RedirectToAction("Account", "Login");
                            }

                            ///////////////////////////Add Tracing//////////////////////////////
                            SaveTracing(cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_Code, LessorCode, cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_Serail_No,
                                cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_No,(DateTime)cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_Date,
                               (DateTime)cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_Start_Date,
                               (DateTime)cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_End_Date,
                                "I", Reasons);
                            ////////////////////////////////////////////////////////////////////
                            cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_Activation = true;

                            //get CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code 
                            var currMechanism = db.CR_Cas_Sup_Follow_Up_Mechanism.FirstOrDefault(l=>l.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code && l.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code == cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_Code); 
                            cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_About_To_Expire = cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_End_Date?.AddDays(-(double)currMechanism.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire);
                            
                            cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_Status = "A";
                            db.Entry(cR_Cas_Sup_Car_Doc_Mainten).State = EntityState.Modified;
                            db.SaveChanges();
                            dbTran.Commit();
                            return RedirectToAction("Index");
                        }
                        catch (DbEntityValidationException ex)
                        {
                            dbTran.Rollback();
                            throw ex;
                        }
                    }

                        
                }  



                if (delete == "Delete" || delete == "حذف")
                {
                    using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var LessorCode = Session["LessorCode"].ToString();
                            var BranchCode = Session["BranchCode"].ToString();
                            ////////////////////////////////////Add Delete Tracing//////////////////////////////////////
                            SaveTracing(cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_Code, LessorCode, cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_Serail_No,
                                    cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_No, (DateTime)cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_Date,
                                   (DateTime)cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_Start_Date,
                                   (DateTime)cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_End_Date,
                                    "D", Reasons);
                            ////////////////////////////////////////////////////////////////////////////////////////////
                            ////////////////////////////////////////////////////////////////////////////////////////////
                            CR_Cas_Sup_Car_Doc_Mainten d = db.CR_Cas_Sup_Car_Doc_Mainten.Find(cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_Serail_No, cR_Cas_Sup_Car_Doc_Mainten.CR_Cas_Sup_Car_Doc_Mainten_Code);
                            d.CR_Cas_Sup_Car_Doc_Mainten_Start_Date = null;
                            d.CR_Cas_Sup_Car_Doc_Mainten_Date = null;
                            d.CR_Cas_Sup_Car_Doc_Mainten_End_Date = null;
                            d.CR_Cas_Sup_Car_Doc_Mainten_No = null;
                            d.CR_Cas_Sup_Car_Doc_Mainten_About_To_Expire = null;
                            d.CR_Cas_Sup_Car_Doc_Mainten_Status = "N";
                            db.Entry(d).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["TempModel"] = "Deleted";
                            dbTran.Commit();
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
            
                return View(cR_Cas_Sup_Car_Doc_Mainten);
        }

        // GET: CarDocMainten/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_Car_Doc_Mainten cR_Cas_Sup_Car_Doc_Mainten = db.CR_Cas_Sup_Car_Doc_Mainten.Find(id);
            if (cR_Cas_Sup_Car_Doc_Mainten == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Sup_Car_Doc_Mainten);
        }

        // POST: CarDocMainten/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Cas_Sup_Car_Doc_Mainten cR_Cas_Sup_Car_Doc_Mainten = db.CR_Cas_Sup_Car_Doc_Mainten.Find(id);
            db.CR_Cas_Sup_Car_Doc_Mainten.Remove(cR_Cas_Sup_Car_Doc_Mainten);
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
