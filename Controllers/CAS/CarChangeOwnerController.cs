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
    public class CarChangeOwnerController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: CarChangeOwner
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

            var cR_Cas_Sup_Car_Information = db.CR_Cas_Sup_Car_Information.Where(x => x.CR_Cas_Sup_Car_Lessor_Code == LessorCode && x.CR_Cas_Sup_Car_Status == "A")
                .Include(c => c.CR_Cas_Sup_Beneficiary).Include(c => c.CR_Cas_Sup_Branch).Include(c => c.CR_Cas_Sup_Owners).Include(c => c.CR_Mas_Com_Lessor)
                .Include(c => c.CR_Mas_Sup_Brand).Include(c => c.CR_Mas_Sup_Category_Car).Include(c => c.CR_Mas_Sup_Color).Include(c => c.CR_Mas_Sup_Model)
                .Include(c => c.CR_Mas_Sup_Registration_Car);
            return View(cR_Cas_Sup_Car_Information.ToList());

        }

        public CR_Cas_Administrative_Procedures GetLastRecord(string Lessorcode, string ProcedureCode)
        {
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");


            var Lrecord = db.CR_Cas_Administrative_Procedures.Where(x => x.CR_Cas_Administrative_Procedures_Lessor == Lessorcode && x.CR_Cas_Administrative_Procedures_Code == ProcedureCode
            && x.CR_Cas_Administrative_Procedures_Year == y)
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

        public void SaveTracing(string LessorCode, string Sector, string ProceduresCode, string ProcedureType, string UserLogin, string TargetedAction, string Reasons)
        {

            CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var sector = "1";
            var autoInc = GetLastRecord(LessorCode, ProceduresCode);

            Ad.CR_Cas_Administrative_Procedures_No = y + "-" + sector + "-" + ProceduresCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
            Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
            Ad.CR_Cas_Administrative_Procedures_Year = y;
            Ad.CR_Cas_Administrative_Procedures_Sector = Sector;
            Ad.CR_Cas_Administrative_Procedures_Code = ProceduresCode;
            Ad.CR_Cas_Administrative_Int_Procedures_Code = int.Parse(ProceduresCode);
            Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
            Ad.CR_Cas_Administrative_Procedures_Targeted_Action = TargetedAction;
            Ad.CR_Cas_Administrative_Procedures_User_Insert = UserLogin;
            Ad.CR_Cas_Administrative_Procedures_Type = ProcedureType;
            Ad.CR_Cas_Administrative_Procedures_Action = true;// نبحثها لاحقا
            Ad.CR_Cas_Administrative_Procedures_Doc_Date = DateTime.Now;
            Ad.CR_Cas_Administrative_Procedures_Doc_Start_Date = null;
            Ad.CR_Cas_Administrative_Procedures_Doc_End_Date = null;
            Ad.CR_Cas_Administrative_Procedures_Doc_No = "";// نبحثها لاحقا
            Ad.CR_Cas_Administrative_Procedures_Reasons = Reasons;

            db.CR_Cas_Administrative_Procedures.Add(Ad);
        }

        [HttpGet]
        public ActionResult Edit(string serialNo)
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
            if (serialNo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_Car_Information cR_Cas_Sup_Car_Information = db.CR_Cas_Sup_Car_Information.FirstOrDefault(c => c.CR_Cas_Sup_Car_Serail_No == serialNo);
            if (cR_Cas_Sup_Car_Information == null)
            {
                return HttpNotFound();
            }
            ViewBag.CR_Cas_Sup_Owners = new SelectList(db.CR_Cas_Sup_Owners.Where(o => o.CR_Cas_Sup_Owners_Lessor_Code == LessorCode
            && o.CR_Cas_Sup_Owners_Code != cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Owner_Code && o.CR_Cas_Sup_Owners_Status=="A"),
                "CR_Cas_Sup_Owners_Code", "CR_Cas_Sup_Owners_Ar_Long_Name", cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Owner_Code);

            return View(cR_Cas_Sup_Car_Information);
        }

        // POST: CarChangeOwner/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Sup_Car_Serail_No,CR_Cas_Sup_Car_Lessor_Code," +
            "CR_Cas_Sup_Car_Owner_Branch_Code,CR_Cas_Sup_Car_Location_Branch_Code,CR_Cas_Sup_Car_Owner_Code," +
            "CR_Cas_Sup_Car_Beneficiary_Code,CR_Cas_Sup_Car_Customs_No,CR_Cas_Sup_Car_Structure_No," +
            "CR_Cas_Sup_Car_Registration_Code,CR_Cas_Sup_Car_Brand_Code,CR_Cas_Sup_Car_Model_Code," +
            "CR_Cas_Sup_Car_Year,CR_Cas_Sup_Car_Category_Code,CR_Cas_Sup_Car_Out_Main_Color_Code," +
            "CR_Cas_Sup_Car_Out_Secondary_Color_Code,CR_Cas_Sup_Car_In_Main_Color_Code," +
            "CR_Cas_Sup_Car_In_Secondary_Color_Code,CR_Cas_Sup_Car_Plate_Ar_No,CR_Cas_Sup_Car_Plate_En_No," +
            "CR_Cas_Sup_Car_No_Current_Meter,CR_Cas_Sup_Car_Traffic_License_Img,CR_Cas_Sup_Car_Joined_Fleet_Date," +
            "CR_Cas_Sup_Car_Left_Fleet_Date,CR_Cas_Sup_Car_Last_Pictures,CR_Cas_Sup_Car_Documentation_Status," +
            "CR_Cas_Sup_Car_Maintenance_Status,CR_Cas_Sup_Car_Status,CR_Cas_Sup_Car_Reasons,CR_Cas_Sup_Car_Collect_Ar_Name," +
            "CR_Cas_Sup_Car_Collect_Ar_Short_Name,CR_Cas_Sup_Car_Collect_En_Name,CR_Cas_Sup_Car_Collect_En_Short_Name," +
            "CR_Cas_Sup_Car_Collect_Fr_Name,CR_Cas_Sup_Car_Collect_Fr_Short_Name,CR_Cas_Sup_Car_Price_Status")]
        CR_Cas_Sup_Car_Information cR_Cas_Sup_Car_Information, string CR_Cas_Sup_Owners, string Reasons)
        {
            var LessorCode = Session["LessorCode"].ToString();
            var UserLogin = Session["UserLogin"].ToString();
            if (ModelState.IsValid)
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        //////////////////////////Save Tracing/////////////////////////////
                        SaveTracing(LessorCode, "1", "53", "U", UserLogin, cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No, Reasons);

                        /////////////////////////////////////////////////////////////////////////////////
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Owner_Code = CR_Cas_Sup_Owners;
                        db.Entry(cR_Cas_Sup_Car_Information).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["TempModel"] = "Saved";
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
            ViewBag.CR_Cas_Sup_Owners = new SelectList(db.CR_Cas_Sup_Owners,
                "CR_Cas_Sup_Owners_Code", "CR_Cas_Sup_Owners_Ar_Long_Name", cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Owner_Code);
            return View(cR_Cas_Sup_Car_Information);
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
