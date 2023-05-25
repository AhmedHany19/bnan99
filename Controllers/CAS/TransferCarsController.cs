using RentCar.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace RentCar.Controllers
{
    public class TransferCarsController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: TransferCars
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

        // GET: TransferCars/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_Car_Information cR_Cas_Sup_Car_Information = db.CR_Cas_Sup_Car_Information.Find(id);
            if (cR_Cas_Sup_Car_Information == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Sup_Car_Information);
        }

        // GET: TransferCars/Create
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
            ViewBag.CR_Cas_Sup_Car_Beneficiary_Code = new SelectList(db.CR_Cas_Sup_Beneficiary, "CR_Cas_Sup_Beneficiary_Code", "CR_Cas_Sup_Beneficiary_Commercial_Registration_No");
            ViewBag.CR_Cas_Sup_Car_Owner_Branch_Code = new SelectList(db.CR_Cas_Sup_Branch, "CR_Cas_Sup_Branch_Code", "CR_Cas_Sup_Branch_Ar_Name");
            ViewBag.CR_Cas_Sup_Car_Owner_Code = new SelectList(db.CR_Cas_Sup_Owners, "CR_Cas_Sup_Owners_Code", "CR_Cas_Sup_Owners_Commercial_Registration_No");
            ViewBag.CR_Cas_Sup_Car_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo");
            ViewBag.CR_Cas_Sup_Car_Brand_Code = new SelectList(db.CR_Mas_Sup_Brand, "CR_Mas_Sup_Brand_Code", "CR_Mas_Sup_Brand_Ar_Name");
            ViewBag.CR_Cas_Sup_Car_Model_Code = new SelectList(db.CR_Mas_Sup_Category_Car, "CR_Mas_Sup_Category_Model_Code", "CR_Mas_Sup_Category_Car_Code");
            ViewBag.CR_Cas_Sup_Car_Out_Main_Color_Code = new SelectList(db.CR_Mas_Sup_Color, "CR_Mas_Sup_Color_Code", "CR_Mas_Sup_Color_Group_Code");
            ViewBag.CR_Cas_Sup_Car_Model_Code = new SelectList(db.CR_Mas_Sup_Model, "CR_Mas_Sup_Model_Code", "CR_Mas_Sup_Model_Group_Code");
            ViewBag.CR_Cas_Sup_Car_Registration_Code = new SelectList(db.CR_Mas_Sup_Registration_Car, "CR_Mas_Sup_Registration_Car_Code", "CR_Mas_Sup_Registration_Car_Ar_Name");
            return View();
        }

        // POST: TransferCars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Sup_Car_Serail_No,CR_Cas_Sup_Car_Lessor_Code,CR_Cas_Sup_Car_Owner_Branch_Code,CR_Cas_Sup_Car_Location_Branch_Code,CR_Cas_Sup_Car_Owner_Code,CR_Cas_Sup_Car_Beneficiary_Code,CR_Cas_Sup_Car_Customs_No,CR_Cas_Sup_Car_Structure_No,CR_Cas_Sup_Car_Registration_Code,CR_Cas_Sup_Car_Brand_Code,CR_Cas_Sup_Car_Model_Code,CR_Cas_Sup_Car_Year,CR_Cas_Sup_Car_Category_Code,CR_Cas_Sup_Car_Out_Main_Color_Code,CR_Cas_Sup_Car_Out_Secondary_Color_Code,CR_Cas_Sup_Car_In_Main_Color_Code,CR_Cas_Sup_Car_In_Secondary_Color_Code,CR_Cas_Sup_Car_Plate_Ar_No,CR_Cas_Sup_Car_Plate_En_No,CR_Cas_Sup_Car_No_Current_Meter,CR_Cas_Sup_Car_Traffic_License_Img,CR_Cas_Sup_Car_Joined_Fleet_Date,CR_Cas_Sup_Car_Left_Fleet_Date,CR_Cas_Sup_Car_Last_Pictures,CR_Cas_Sup_Car_Documentation_Status,CR_Cas_Sup_Car_Maintenance_Status,CR_Cas_Sup_Car_Status,CR_Cas_Sup_Car_Reasons,CR_Cas_Sup_Car_Collect_Ar_Name,CR_Cas_Sup_Car_Collect_Ar_Short_Name,CR_Cas_Sup_Car_Collect_En_Name,CR_Cas_Sup_Car_Collect_En_Short_Name,CR_Cas_Sup_Car_Collect_Fr_Name,CR_Cas_Sup_Car_Collect_Fr_Short_Name,CR_Cas_Sup_Car_Price_Status")] CR_Cas_Sup_Car_Information cR_Cas_Sup_Car_Information)
        {
            if (ModelState.IsValid)
            {
                db.CR_Cas_Sup_Car_Information.Add(cR_Cas_Sup_Car_Information);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CR_Cas_Sup_Car_Beneficiary_Code = new SelectList(db.CR_Cas_Sup_Beneficiary, "CR_Cas_Sup_Beneficiary_Code", "CR_Cas_Sup_Beneficiary_Commercial_Registration_No", cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Beneficiary_Code);
            ViewBag.CR_Cas_Sup_Car_Owner_Branch_Code = new SelectList(db.CR_Cas_Sup_Branch, "CR_Cas_Sup_Branch_Code", "CR_Cas_Sup_Branch_Ar_Name", cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Owner_Branch_Code);
            ViewBag.CR_Cas_Sup_Car_Owner_Code = new SelectList(db.CR_Cas_Sup_Owners, "CR_Cas_Sup_Owners_Code", "CR_Cas_Sup_Owners_Commercial_Registration_No", cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Owner_Code);
            ViewBag.CR_Cas_Sup_Car_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Lessor_Code);
            ViewBag.CR_Cas_Sup_Car_Brand_Code = new SelectList(db.CR_Mas_Sup_Brand, "CR_Mas_Sup_Brand_Code", "CR_Mas_Sup_Brand_Ar_Name", cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Brand_Code);
            ViewBag.CR_Cas_Sup_Car_Model_Code = new SelectList(db.CR_Mas_Sup_Category_Car, "CR_Mas_Sup_Category_Model_Code", "CR_Mas_Sup_Category_Car_Code", cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Model_Code);
            ViewBag.CR_Cas_Sup_Car_Out_Main_Color_Code = new SelectList(db.CR_Mas_Sup_Color, "CR_Mas_Sup_Color_Code", "CR_Mas_Sup_Color_Group_Code", cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Out_Main_Color_Code);
            ViewBag.CR_Cas_Sup_Car_Model_Code = new SelectList(db.CR_Mas_Sup_Model, "CR_Mas_Sup_Model_Code", "CR_Mas_Sup_Model_Group_Code", cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Model_Code);
            ViewBag.CR_Cas_Sup_Car_Registration_Code = new SelectList(db.CR_Mas_Sup_Registration_Car, "CR_Mas_Sup_Registration_Car_Code", "CR_Mas_Sup_Registration_Car_Ar_Name", cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Registration_Code);
            return View(cR_Cas_Sup_Car_Information);
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

        public void SaveTracing(string LessorCode, string Sector, string ProceduresCode, string ProcedureType, string UserLogin, string TargetedAction, string Reasons, string OldBranch, string NewBranch)
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
            Ad.CR_Cas_Administrative_Procedures_From_Branch = OldBranch;
            Ad.CR_Cas_Administrative_Procedures_To_Branch = NewBranch;
            db.CR_Cas_Administrative_Procedures.Add(Ad);
        }


        // GET: TransferCars/Edit/5
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
            CR_Cas_Sup_Car_Information cR_Cas_Sup_Car_Information = db.CR_Cas_Sup_Car_Information.Find(id);
            if (cR_Cas_Sup_Car_Information == null)
            {
                return HttpNotFound();
            }
            ViewBag.Location_Branch_Code = new SelectList(db.CR_Cas_Sup_Branch.Where(b => b.CR_Cas_Sup_Lessor_Code == LessorCode && b.CR_Cas_Sup_Branch_Status == "A" &&
            b.CR_Cas_Sup_Branch_Code != cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Owner_Branch_Code),
                "CR_Cas_Sup_Branch_Code", "CR_Cas_Sup_Branch_Ar_Short_Name", cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Owner_Branch_Code);

            ViewBag.OldBranch = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Owner_Branch_Code;


            return View(cR_Cas_Sup_Car_Information);
        }

        // POST: TransferCars/Edit/5
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
            "CR_Cas_Sup_Car_In_Secondary_Color_Code,CR_Cas_Sup_Car_Plate_Ar_No," +
            "CR_Cas_Sup_Car_Plate_En_No,CR_Cas_Sup_Car_No_Current_Meter," +
            "CR_Cas_Sup_Car_Traffic_License_Img,CR_Cas_Sup_Car_Joined_Fleet_Date," +
            "CR_Cas_Sup_Car_Left_Fleet_Date,CR_Cas_Sup_Car_Last_Pictures," +
            "CR_Cas_Sup_Car_Documentation_Status," +
            "CR_Cas_Sup_Car_Maintenance_Status,CR_Cas_Sup_Car_Status,CR_Cas_Sup_Car_Reasons," +
            "CR_Cas_Sup_Car_Collect_Ar_Name,CR_Cas_Sup_Car_Collect_Ar_Short_Name,CR_Cas_Sup_Car_Collect_En_Name," +
            "CR_Cas_Sup_Car_Collect_En_Short_Name,CR_Cas_Sup_Car_Collect_Fr_Name,CR_Cas_Sup_Car_Collect_Fr_Short_Name,CR_Cas_Sup_Car_Price_Status")]
            CR_Cas_Sup_Car_Information cR_Cas_Sup_Car_Information, string Location_Branch_Code, string Reasons, string OldBranch)
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
                        SaveTracing(LessorCode, "1", "52", "U", UserLogin, cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No, Reasons, OldBranch,
                            Location_Branch_Code);

                        /////////////////////////////////////////////////////////////////////////////////
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Location_Branch_Code = Location_Branch_Code;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Owner_Branch_Code = Location_Branch_Code;
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
            ViewBag.Owner_Branch_Code = new SelectList(db.CR_Cas_Sup_Owners.Where(b => b.CR_Cas_Sup_Owners_Lessor_Code == LessorCode && b.CR_Cas_Sup_Owners_Status == "A"),
                "CR_Cas_Sup_Owners_Code", "CR_Cas_Sup_Owners_Ar_Long_Name", cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Owner_Branch_Code);
            return View(cR_Cas_Sup_Car_Information);
        }

        // GET: TransferCars/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_Car_Information cR_Cas_Sup_Car_Information = db.CR_Cas_Sup_Car_Information.Find(id);
            if (cR_Cas_Sup_Car_Information == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Sup_Car_Information);
        }

        // POST: TransferCars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Cas_Sup_Car_Information cR_Cas_Sup_Car_Information = db.CR_Cas_Sup_Car_Information.Find(id);
            db.CR_Cas_Sup_Car_Information.Remove(cR_Cas_Sup_Car_Information);
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
