using RentCar.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace RentCar.Controllers.CAS
{
    public class CasRenterLessorController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: CasRenterLessor
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
            var cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(l =>l.CR_Cas_Renter_Lessor_Code==LessorCode).Include(c => c.CR_Mas_Com_Lessor).Include(c => c.CR_Mas_Renter_Information);
            return View(cR_Cas_Renter_Lessor.ToList());
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

            IQueryable<CR_Cas_Renter_Lessor> cR_Cas_Renter_Lessor = null;
            if (type == "All")
            {
                cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(l => l.CR_Cas_Renter_Lessor_Code == LessorCode)
                    .Include(c => c.CR_Mas_Com_Lessor)
                    .Include(c => c.CR_Mas_Renter_Information);
            }
            else if (type == "K")
            {
                cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(l => l.CR_Cas_Renter_Lessor_Code == LessorCode && l.CR_Cas_Renter_Lessor_Status == "K")
                    .Include(c => c.CR_Mas_Com_Lessor)
                    .Include(c => c.CR_Mas_Renter_Information);
            }
            else if (type == "R")
            {
                cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(l => l.CR_Cas_Renter_Lessor_Code == LessorCode && l.CR_Cas_Renter_Lessor_Status == "R")
                    .Include(c => c.CR_Mas_Com_Lessor)
                    .Include(c => c.CR_Mas_Renter_Information);
            }
            else if (type == "A")
            {
                cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(l => l.CR_Cas_Renter_Lessor_Code == LessorCode && l.CR_Cas_Renter_Lessor_Status == "A")
                    .Include(c => c.CR_Mas_Com_Lessor)
                    .Include(c => c.CR_Mas_Renter_Information);
            }
            else
            {
                cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(l => l.CR_Cas_Renter_Lessor_Code == LessorCode && (l.CR_Cas_Renter_Lessor_Status=="A"||l.CR_Cas_Renter_Lessor_Status=="R") )
                    .Include(c => c.CR_Mas_Com_Lessor)
                    .Include(c => c.CR_Mas_Renter_Information);
            }
            return PartialView(cR_Cas_Renter_Lessor);
        }

        public ActionResult LoadRenters()
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
            var cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(l => l.CR_Cas_Renter_Lessor_Code == LessorCode && l.CR_Cas_Renter_Lessor_Contract_Number > 0 && l.CR_Cas_Renter_Lessor_Status != "Y" && l.CR_Cas_Renter_Lessor_Status != "U")
                .Include(c => c.CR_Mas_Com_Lessor)
                .Include(c => c.CR_Mas_Renter_Information);
            return View(cR_Cas_Renter_Lessor.ToList());
        }
        public PartialViewResult PartialLoadRenter(string type)
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
            if (type == "A")
            {
                cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(l => l.CR_Cas_Renter_Lessor_Code == LessorCode && l.CR_Cas_Renter_Lessor_Status == "A" && l.CR_Cas_Renter_Lessor_Contract_Number > 0)
                    .Include(c => c.CR_Mas_Com_Lessor)
                    .Include(c => c.CR_Mas_Renter_Information);
            }
            else if (type == "D")
            {
                cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(l => l.CR_Cas_Renter_Lessor_Code == LessorCode && l.CR_Cas_Renter_Lessor_Status == "D" && l.CR_Cas_Renter_Lessor_Contract_Number > 0)
                    .Include(c => c.CR_Mas_Com_Lessor)
                    .Include(c => c.CR_Mas_Renter_Information);
            }
            else if (type == "K")
            {
                cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(l => l.CR_Cas_Renter_Lessor_Code == LessorCode && l.CR_Cas_Renter_Lessor_Status == "K" && l.CR_Cas_Renter_Lessor_Contract_Number > 0)
                    .Include(c => c.CR_Mas_Com_Lessor)
                    .Include(c => c.CR_Mas_Renter_Information);
            }
            else
            {
                cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(l => l.CR_Cas_Renter_Lessor_Code == LessorCode && l.CR_Cas_Renter_Lessor_Contract_Number > 0)
                    .Include(c => c.CR_Mas_Com_Lessor)
                    .Include(c => c.CR_Mas_Renter_Information);
            }
            return PartialView(cR_Cas_Renter_Lessor);
        }

        public ActionResult RentersContracts(string id)
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
            var Renter = db.CR_Cas_Renter_Lessor.FirstOrDefault(r => r.CR_Cas_Renter_Lessor_Id == id);
            ViewBag.RenterID = id;
            ViewBag.RenterName = Renter.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Ar_Name;
            ViewBag.RenterJobs = Renter.CR_Mas_Renter_Information.CR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Ar_Name;

            ViewBag.RenterNationality = db.CR_Mas_Sup_Nationalities.FirstOrDefault(nat => nat.CR_Mas_Sup_Nationalities_Code == Renter.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Nationality).CR_Mas_Sup_Nationalities_Ar_Name;
            ViewBag.RenterMembership = Renter.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Membership;
            ViewBag.RenterFirstInteraction = Renter.CR_Cas_Renter_Lessor_Date_First_Interaction;
            ViewBag.RenterLastInteraction = Renter.CR_Cas_Renter_Lessor_Date_Last_Interaction;
            ViewBag.RenterRating = Renter.CR_Cas_Renter_Rating;
            ViewBag.Km = Renter.CR_Cas_Renter_Lessor_KM;
            ViewBag.InteractionValue = Renter.CR_Cas_Renter_Lessor_Interaction_Amount_Value;
            ViewBag.RenterBalance = Renter.CR_Cas_Renter_Lessor_Balance;


            var Contracts = db.CR_Cas_Contract_Basic.Where(c => c.CR_Cas_Contract_Basic_Lessor == LessorCode && (c.CR_Cas_Contract_Basic_Status != "U"&& c.CR_Cas_Contract_Basic_Status != "y") && (c.CR_Cas_Contract_Basic_Renter_Id == id ||c.CR_Cas_Contract_Basic_Driver_Id==id || c.CR_Cas_Contract_Basic_Additional_Driver_Id==id))
                                                    .Include(c => c.CR_Mas_Com_Lessor)
                                                    .Include(car => car.CR_Cas_Sup_Car_Information);

         /*   var test = new List<CR_Cas_Contract_Basic>
            {
                Contracts
            };
*/
            return View(Contracts);
        }

        // GET: CasRenterLessor/Details/5
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

        // GET: CasRenterLessor/Create
        public ActionResult Create()
        {
            ViewBag.CR_Cas_Renter_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo");
            ViewBag.CR_Cas_Renter_Lessor_Id = new SelectList(db.CR_Mas_Renter_Information, "CR_Mas_Renter_Information_Id", "CR_Mas_Renter_Information_Sector");
            return View();
        }

        // POST: CasRenterLessor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Renter_Lessor_Id,CR_Cas_Renter_Lessor_Code,CR_Cas_Renter_Lessor_Date_First_Interaction,CR_Cas_Renter_Lessor_Date_Last_Interaction,CR_Cas_Renter_Lessor_Contract_Number,CR_Cas_Renter_Lessor_Days,CR_Cas_Renter_Lessor_Interaction_Amount_Value,CR_Cas_Renter_Lessor_KM,CR_Cas_Renter_Lessor_Balance,CR_Cas_Renter_Rating,CR_Cas_Renter_Membership_Code,CR_Cas_Renter_Admin_Membership_Code,CR_Cas_Renter_Lessor_Status,CR_Cas_Renter_Lessor_Reasons")] CR_Cas_Renter_Lessor cR_Cas_Renter_Lessor)
        {
            if (ModelState.IsValid)
            {
                db.CR_Cas_Renter_Lessor.Add(cR_Cas_Renter_Lessor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CR_Cas_Renter_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Code);
            ViewBag.CR_Cas_Renter_Lessor_Id = new SelectList(db.CR_Mas_Renter_Information, "CR_Mas_Renter_Information_Id", "CR_Mas_Renter_Information_Sector", cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Id);
            return View(cR_Cas_Renter_Lessor);
        }

        // GET: CasRenterLessor/Edit/5
        public ActionResult Edit(string id1, string id2)
        {
            if (id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Renter_Lessor cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Find(id1, id2);
            if (cR_Cas_Renter_Lessor == null)
            {
                return HttpNotFound();
            }
            //ViewBag.CR_Cas_Renter_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Code);
            //ViewBag.CR_Cas_Renter_Lessor_Id = new SelectList(db.CR_Mas_Renter_Information, "CR_Mas_Renter_Information_Id", "CR_Mas_Renter_Information_Sector", cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Id);
            ViewBag.FirstInteraction = string.Format("{0:yyyy/MM/dd}", cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Date_First_Interaction);
            ViewBag.LastInteraction = string.Format("{0:yyyy/MM/dd}", cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Date_Last_Interaction);
            ViewBag.BirthDate = string.Format("{0:yyyy/MM/dd}", cR_Cas_Renter_Lessor.CR_Mas_Renter_Information.CR_Mas_Renter_Information_BirthDate);
            ViewBag.IssueIdDate = string.Format("{0:yyyy/MM/dd}", cR_Cas_Renter_Lessor.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Issue_Id_Date);
            ViewBag.ExpiryIdDate = string.Format("{0:yyyy/MM/dd}", cR_Cas_Renter_Lessor.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Expiry_Id_Date);
            ViewBag.ExpiryDrivingLicenseDate = string.Format("{0:yyyy/MM/dd}", cR_Cas_Renter_Lessor.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Expiry_Driving_License_Date);
            ViewBag.MemberShip = 3;

            var renterAdr = db.CR_Mas_Address.FirstOrDefault(x => x.CR_Mas_Address_Id_Code == id1);
            if (renterAdr != null)
            {
                var RegionName = db.CR_Mas_Sup_Regions.FirstOrDefault(p => p.CR_Mas_Sup_Regions_Code == renterAdr.CR_Mas_Address_Regions);
                var CityName = db.CR_Mas_Sup_City.FirstOrDefault(e => e.CR_Mas_Sup_City_Code == renterAdr.CR_Mas_Address_City);

                ViewBag.address = RegionName.CR_Mas_Sup_Regions_Ar_Name + " - " + CityName.CR_Mas_Sup_City_Ar_Name + " - " + renterAdr.CR_Mas_Address_Ar_District + " - "
                    + renterAdr.CR_Mas_Address_Ar_Street;
            }

            var masbank = db.CR_Mas_Sup_Bank.FirstOrDefault(b=>b.CR_Mas_Sup_Bank_Code==cR_Cas_Renter_Lessor.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Bank);
            if (masbank != null)
            {
                ViewBag.bank = masbank.CR_Mas_Sup_Bank_Ar_Name;
            }

            if (cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Status == "K")
            {
                ViewBag.ButtonName = "رفع الحضر";
                ViewBag.ST = "K";
            }
            else if (cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Status == "R")
            {
                ViewBag.ButtonName = "حضر";
                ViewBag.ST = "R";
            }
            else if (cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Status == "A")
            {
                ViewBag.ButtonName = "حضر";
                ViewBag.ST = "A";
            }
            ViewBag.Delete = cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Status;

            //ViewBag.IssueIdDate = string.Format("{0:yyyy-MM-dd}", cR_Cas_Renter_Lessor.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Issue_Id_Date);

            return View(cR_Cas_Renter_Lessor);
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

        public void SaveTracing(string ID,string ProcCode, string Lessor, string procType, string reason)
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
            Ad.CR_Cas_Administrative_Procedures_Targeted_Action = ID;
            Ad.CR_Cas_Administrative_Procedures_User_Insert = Session["UserLogin"].ToString();
            Ad.CR_Cas_Administrative_Procedures_Doc_No = null;
            Ad.CR_Cas_Administrative_Procedures_Type = procType;
            Ad.CR_Cas_Administrative_Procedures_Action = true;
            Ad.CR_Cas_Administrative_Procedures_Doc_Date = null;
            Ad.CR_Cas_Administrative_Procedures_Doc_Start_Date = null;
            Ad.CR_Cas_Administrative_Procedures_Doc_End_Date = null;
            Ad.CR_Cas_Administrative_Procedures_Reasons = reason;
            db.CR_Cas_Administrative_Procedures.Add(Ad);
        }
        // POST: CasRenterLessor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Renter_Lessor_Id,CR_Cas_Renter_Lessor_Code," +
            "CR_Cas_Renter_Lessor_Date_First_Interaction,CR_Cas_Renter_Lessor_Date_Last_Interaction," +
            "CR_Cas_Renter_Lessor_Contract_Number,CR_Cas_Renter_Lessor_Days," +
            "CR_Cas_Renter_Lessor_Interaction_Amount_Value,CR_Cas_Renter_Lessor_KM," +
            "CR_Cas_Renter_Lessor_Balance,CR_Cas_Renter_Rating,CR_Cas_Renter_Membership_Code," +
            "CR_Cas_Renter_Admin_Membership_Code,CR_Cas_Renter_Lessor_Status,CR_Cas_Renter_Lessor_Reasons")]
            CR_Cas_Renter_Lessor cR_Cas_Renter_Lessor , string Save,string Hold)
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
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(Save))
                {
                    ///////////////////////////Add Tracing//////////////////////
                    SaveTracing(cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Id, "59", LessorCode, "U", cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Reasons);
                    ////////////////////////////////////////////////////////////
                    db.Entry(cR_Cas_Renter_Lessor).State = EntityState.Modified;
                    TempData["TempModel"] = "Saved";
                    db.SaveChanges();
                }
                else if (Hold=="حضر")
                {
                    ///////////////////////////Add Tracing//////////////////////
                    SaveTracing(cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Id,"59",LessorCode,"K",cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Reasons);
                    ////////////////////////////////////////////////////////////
                    cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Status = "K";
                    db.Entry(cR_Cas_Renter_Lessor).State = EntityState.Modified;
                    TempData["TempModel"] = "Holded";
                    db.SaveChanges();
                }

                else if (Hold == "رفع الحضر")
                {
                    ///////////////////////////Add Tracing//////////////////////
                    SaveTracing(cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Id, "59", LessorCode, "A", cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Reasons);
                    ////////////////////////////////////////////////////////////
                    cR_Cas_Renter_Lessor.CR_Cas_Renter_Lessor_Status = "A";
                    db.Entry(cR_Cas_Renter_Lessor).State = EntityState.Modified;
                    TempData["TempModel"] = "Activated";
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
           
            return View(cR_Cas_Renter_Lessor);
        }

        // GET: CasRenterLessor/Delete/5
        public ActionResult Delete(string id)
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

        // POST: CasRenterLessor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Cas_Renter_Lessor cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Find(id);
            db.CR_Cas_Renter_Lessor.Remove(cR_Cas_Renter_Lessor);
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
