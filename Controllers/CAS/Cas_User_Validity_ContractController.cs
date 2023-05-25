using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentCar.Models;

namespace RentCar.Controllers.CAS
{
    public class Cas_User_Validity_ContractController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: Cas_User_Validity_Contract
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
            var cR_Cas_User_Validity_Contract = db.CR_Cas_User_Validity_Contract.Where(u=>u.CR_Cas_User_Information.CR_Cas_User_Information_Status=="A" &&
            u.CR_Cas_User_Validity_Contract_User_Id!=UserLogin &&
            u.CR_Cas_User_Information.CR_Cas_User_Information_Auth_Branch==true &&
            u.CR_Cas_User_Validity_Contract_User_Id!=LessorCode &&
            u.CR_Cas_User_Information.CR_Cas_User_Information_Lessor_Code==LessorCode)
                .Include(v=>v.CR_Cas_User_Information);
            return View(cR_Cas_User_Validity_Contract.ToList());
        }

        // GET: Cas_User_Validity_Contract/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_User_Validity_Contract cR_Cas_User_Validity_Contract = db.CR_Cas_User_Validity_Contract.Find(id);
            if (cR_Cas_User_Validity_Contract == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_User_Validity_Contract);
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


        public void SaveTracing(string LessorCode, string Sector, string ProceduresCode, string ProcedureType, string UserLogin,string TargetedAction, string Reasons)
        {

            CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var sector = "1";
            var autoInc = GetLastRecord(ProceduresCode,sector);

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


        // GET: Cas_User_Validity_Contract/Create
        public ActionResult Create(string id)
        {
            CR_Cas_User_Validity_Contract validity = null;
            validity = db.CR_Cas_User_Validity_Contract.FirstOrDefault(x => x.CR_Cas_User_Validity_Contract_User_Id == id && x.CR_Cas_User_Information.CR_Cas_User_Information_Status=="A");
            if (validity != null)
            {
                validity.CR_Cas_User_Validity_Contract_Reasons = "";
               
            }
            
            return View(validity);
        }

        // POST: Cas_User_Validity_Contract/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_User_Validity_Contract_User_Id,CR_Cas_User_Validity_Contract_Admin," +
            "CR_Cas_User_Validity_Contract_Register,CR_Cas_User_Validity_Contract_Chamber_Commerce,CR_Cas_User_Validity_Contract_Transfer_Permission," +
            "CR_Cas_User_Validity_Contract_Licence_Municipale,CR_Cas_User_Validity_Contract_Company_Address,CR_Cas_User_Validity_Contract_Traffic_License," +
            "CR_Cas_User_Validity_Contract_Insurance,CR_Cas_User_Validity_Contract_Operating_Card,CR_Cas_User_Validity_Contract_Chkec_Up," +
            "CR_Cas_User_Validity_Contract_Id,CR_Cas_User_Validity_Contract_Driving_License,CR_Cas_User_Validity_Contract_Renter_Address," +
            "CR_Cas_User_Validity_Contract_Employer,CR_Cas_User_Validity_Contract_Tires,CR_Cas_User_Validity_Contract_Oil,CR_Cas_User_Validity_Contract_Maintenance," +
            "CR_Cas_User_Validity_Contract_FBrake_Maintenance,CR_Cas_User_Validity_Contract_BBrake_Maintenance,CR_Cas_User_Validity_Contract_Extension," +
            "CR_Cas_User_Validity_Contract_Age,CR_Cas_User_Validity_Contract_Open_Amout_Rate,CR_Cas_User_Validity_Contract_Discount_Rate,CR_Cas_User_Validity_Contract_Km," +
            "CR_Cas_User_Validity_Contract_Hour,CR_Cas_User_Validity_Contract_Cancel,CR_Cas_User_Validity_Contract_End,CR_Cas_User_Validity_Contract_Close," +
            "CR_Cas_User_Validity_Contract_Close_Amount_Rate,CR_Cas_User_Validity_Contract_Status,CR_Cas_User_Validity_Contract_Reasons")]
            CR_Cas_User_Validity_Contract cR_Cas_User_Validity_Contract,string CR_Cas_User_Validity_Contract_Register,string CR_Cas_User_Validity_Contract_Chamber_Commerce,
            string CR_Cas_User_Validity_Contract_Transfer_Permission,string CR_Cas_User_Validity_Contract_Licence_Municipale,string CR_Cas_User_Validity_Contract_Company_Address,
            string CR_Cas_User_Validity_Contract_Traffic_License,string CR_Cas_User_Validity_Contract_Insurance,string CR_Cas_User_Validity_Contract_Operating_Card,
            string CR_Cas_User_Validity_Contract_Chkec_Up,string CR_Cas_User_Validity_Contract_Tires, string CR_Cas_User_Validity_Contract_Oil,
            string CR_Cas_User_Validity_Contract_Maintenance,string CR_Cas_User_Validity_Contract_FBrake_Maintenance,string CR_Cas_User_Validity_Contract_BBrake_Maintenance,
            string CR_Cas_User_Validity_Contract_Id,string CR_Cas_User_Validity_Contract_Driving_License,string CR_Cas_User_Validity_Contract_Renter_Address,
            string CR_Cas_User_Validity_Contract_Employer,string CR_Cas_User_Validity_Contract_Extension,string CR_Cas_User_Validity_Contract_Age,string CR_Cas_User_Validity_Contract_Km,
            string CR_Cas_User_Validity_Contract_Open_Amout_Rate,string CR_Cas_User_Validity_Contract_Cancel,string CR_Cas_User_Validity_Contract_End,string CR_Cas_User_Validity_Contract_Discount_Rate,
            string CR_Cas_User_Validity_Contract_Close,string CR_Cas_User_Validity_Contract_Close_Amount_Rate,string CR_Cas_User_Validity_Contract_Hour, string CR_Cas_User_Validity_Contract_Reasons,string Save,string Delete)
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
            var validity = db.CR_Cas_User_Validity_Contract.FirstOrDefault(x => x.CR_Cas_User_Validity_Contract_User_Id == cR_Cas_User_Validity_Contract.CR_Cas_User_Validity_Contract_User_Id);
            if (Save == "حفظ")
            {
                if (validity != null)
                {

                    SaveTracing(LessorCode, "1", "47", "I", UserLogin,cR_Cas_User_Validity_Contract.CR_Cas_User_Validity_Contract_User_Id, CR_Cas_User_Validity_Contract_Reasons);

                    if (CR_Cas_User_Validity_Contract_Register == "on")
                    {
                        validity.CR_Cas_User_Validity_Contract_Register = true;
                    }
                    else
                    {
                        validity.CR_Cas_User_Validity_Contract_Register = false;
                    }

                    if (CR_Cas_User_Validity_Contract_Chamber_Commerce == "on")
                    {
                        validity.CR_Cas_User_Validity_Contract_Chamber_Commerce = true;
                    }
                    else
                    {
                        validity.CR_Cas_User_Validity_Contract_Chamber_Commerce = false;
                    }


                    if (CR_Cas_User_Validity_Contract_Transfer_Permission == "on")
                    {
                        validity.CR_Cas_User_Validity_Contract_Transfer_Permission = true;
                    }
                    else
                    {
                        validity.CR_Cas_User_Validity_Contract_Transfer_Permission = false;
                    }


                    if (CR_Cas_User_Validity_Contract_Licence_Municipale == "on")
                    {
                        validity.CR_Cas_User_Validity_Contract_Licence_Municipale = true;
                    }
                    else
                    {
                        validity.CR_Cas_User_Validity_Contract_Licence_Municipale = false;
                    }

                    if (CR_Cas_User_Validity_Contract_Company_Address == "on")
                    {
                        validity.CR_Cas_User_Validity_Contract_Company_Address = true;
                    }
                    else
                    {
                        validity.CR_Cas_User_Validity_Contract_Company_Address = false;
                    }

                    if (CR_Cas_User_Validity_Contract_Traffic_License == "on")
                    {
                        validity.CR_Cas_User_Validity_Contract_Traffic_License = true;
                    }
                    else
                    {
                        validity.CR_Cas_User_Validity_Contract_Traffic_License = false;
                    }

                    if (CR_Cas_User_Validity_Contract_Insurance == "on")
                    {
                        validity.CR_Cas_User_Validity_Contract_Insurance = true;
                    }
                    else
                    {
                        validity.CR_Cas_User_Validity_Contract_Insurance = false;
                    }

                    if (CR_Cas_User_Validity_Contract_Operating_Card == "on")
                    {
                        validity.CR_Cas_User_Validity_Contract_Operating_Card = true;
                    }
                    else
                    {
                        validity.CR_Cas_User_Validity_Contract_Operating_Card = false;
                    }


                    if (CR_Cas_User_Validity_Contract_Chkec_Up == "on")
                    {
                        validity.CR_Cas_User_Validity_Contract_Chkec_Up = true;
                    }
                    else
                    {
                        validity.CR_Cas_User_Validity_Contract_Chkec_Up = false;
                    }


                    if (CR_Cas_User_Validity_Contract_Tires == "on")
                    {
                        validity.CR_Cas_User_Validity_Contract_Tires = true;
                    }
                    else
                    {
                        validity.CR_Cas_User_Validity_Contract_Tires = false;
                    }

                    if (CR_Cas_User_Validity_Contract_Oil == "on")
                    {
                        validity.CR_Cas_User_Validity_Contract_Oil = true;
                    }
                    else
                    {
                        validity.CR_Cas_User_Validity_Contract_Oil = false;
                    }

                    if (CR_Cas_User_Validity_Contract_Maintenance == "on")
                    {
                        validity.CR_Cas_User_Validity_Contract_Maintenance = true;
                    }
                    else
                    {
                        validity.CR_Cas_User_Validity_Contract_Maintenance = false;
                    }

                    if (CR_Cas_User_Validity_Contract_FBrake_Maintenance == "on")
                    {
                        validity.CR_Cas_User_Validity_Contract_FBrake_Maintenance = true;
                    }
                    else
                    {
                        validity.CR_Cas_User_Validity_Contract_FBrake_Maintenance = false;
                    }

                    if (CR_Cas_User_Validity_Contract_BBrake_Maintenance == "on")
                    {
                        validity.CR_Cas_User_Validity_Contract_BBrake_Maintenance = true;
                    }
                    else
                    {
                        validity.CR_Cas_User_Validity_Contract_BBrake_Maintenance = false;
                    }

                    if (CR_Cas_User_Validity_Contract_Id == "on")
                    {
                        validity.CR_Cas_User_Validity_Contract_Id = true;
                    }
                    else
                    {
                        validity.CR_Cas_User_Validity_Contract_Id = false;
                    }

                    if (CR_Cas_User_Validity_Contract_Driving_License == "on")
                    {
                        validity.CR_Cas_User_Validity_Contract_Driving_License = true;
                    }
                    else
                    {
                        validity.CR_Cas_User_Validity_Contract_Driving_License = false;
                    }

                    if (CR_Cas_User_Validity_Contract_Renter_Address == "on")
                    {
                        validity.CR_Cas_User_Validity_Contract_Renter_Address = true;
                    }
                    else
                    {
                        validity.CR_Cas_User_Validity_Contract_Renter_Address = false;
                    }

                    if (CR_Cas_User_Validity_Contract_Employer == "on")
                    {
                        validity.CR_Cas_User_Validity_Contract_Employer = true;
                    }
                    else
                    {
                        validity.CR_Cas_User_Validity_Contract_Employer = false;
                    }

                    if (CR_Cas_User_Validity_Contract_Extension == "on")
                    {
                        validity.CR_Cas_User_Validity_Contract_Extension = true;
                    }
                    else
                    {
                        validity.CR_Cas_User_Validity_Contract_Extension = false;
                    }

                    if (CR_Cas_User_Validity_Contract_Age == "on")
                    {
                        validity.CR_Cas_User_Validity_Contract_Age = true;
                    }
                    else
                    {
                        validity.CR_Cas_User_Validity_Contract_Age = false;
                    }


                    validity.CR_Cas_User_Validity_Contract_Open_Amout_Rate = decimal.Parse(CR_Cas_User_Validity_Contract_Open_Amout_Rate);

                    if (CR_Cas_User_Validity_Contract_Cancel == "on")
                    {
                        validity.CR_Cas_User_Validity_Contract_Cancel = true;
                    }
                    else
                    {
                        validity.CR_Cas_User_Validity_Contract_Cancel = false;
                    }


                    if (CR_Cas_User_Validity_Contract_End == "on")
                    {
                        validity.CR_Cas_User_Validity_Contract_End = true;
                    }
                    else
                    {
                        validity.CR_Cas_User_Validity_Contract_End = false;
                    }

                    validity.CR_Cas_User_Validity_Contract_Hour = int.Parse(CR_Cas_User_Validity_Contract_Hour);
                    validity.CR_Cas_User_Validity_Contract_Km = int.Parse(CR_Cas_User_Validity_Contract_Km);
                    validity.CR_Cas_User_Validity_Contract_Close = decimal.Parse(CR_Cas_User_Validity_Contract_Close);
                    validity.CR_Cas_User_Validity_Contract_Discount_Rate = decimal.Parse(CR_Cas_User_Validity_Contract_Discount_Rate);
                    validity.CR_Cas_User_Validity_Contract_Close_Amount_Rate = decimal.Parse(CR_Cas_User_Validity_Contract_Close_Amount_Rate);
                    validity.CR_Cas_User_Validity_Contract_Status = "A";
                    validity.CR_Cas_User_Validity_Contract_Reasons = CR_Cas_User_Validity_Contract_Reasons;
                    db.Entry(validity).State = EntityState.Modified;
                    db.SaveChanges();
                    //SqlConnection cn = new SqlConnection(@"Data Source=MSI\SQLEXPRESS;User Id=sa;Password=Admin123;DataBase=RentCarDB");
                    //SqlCommand cmd = new SqlCommand("Update CR_Cas_User_Validity_Contract Set CR_Cas_User_Validity_Contract_Register='" + false + "' Where CR_Cas_User_Validity_Contract_User_Id=" + 5001, cn);
                    //cn.Open();
                    //cmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
               
            }

            if (Delete == "حذف")
            {
                SaveTracing(LessorCode, "1", "47", "D", UserLogin, cR_Cas_User_Validity_Contract.CR_Cas_User_Validity_Contract_User_Id, CR_Cas_User_Validity_Contract_Reasons);
                validity.CR_Cas_User_Validity_Contract_Admin = null;
                validity.CR_Cas_User_Validity_Contract_Age = false;
                validity.CR_Cas_User_Validity_Contract_BBrake_Maintenance = false;
                validity.CR_Cas_User_Validity_Contract_Cancel = false;
                validity.CR_Cas_User_Validity_Contract_Chamber_Commerce = false;
                validity.CR_Cas_User_Validity_Contract_Chkec_Up = false;
                validity.CR_Cas_User_Validity_Contract_Close = 0;
                validity.CR_Cas_User_Validity_Contract_Close_Amount_Rate = 0;
                validity.CR_Cas_User_Validity_Contract_Company_Address = false;
                validity.CR_Cas_User_Validity_Contract_Discount_Rate = 0;
                validity.CR_Cas_User_Validity_Contract_Driving_License = false;
                validity.CR_Cas_User_Validity_Contract_Employer = false;
                validity.CR_Cas_User_Validity_Contract_End = false;
                validity.CR_Cas_User_Validity_Contract_Extension = false;
                validity.CR_Cas_User_Validity_Contract_FBrake_Maintenance = false;
                validity.CR_Cas_User_Validity_Contract_Hour = 0;
                validity.CR_Cas_User_Validity_Contract_Id = false;
                validity.CR_Cas_User_Validity_Contract_Insurance = false;
                validity.CR_Cas_User_Validity_Contract_Km = 0;
                validity.CR_Cas_User_Validity_Contract_Licence_Municipale = false;
                validity.CR_Cas_User_Validity_Contract_Maintenance = false;
                validity.CR_Cas_User_Validity_Contract_Oil = false;
                validity.CR_Cas_User_Validity_Contract_Open_Amout_Rate = 0;
                validity.CR_Cas_User_Validity_Contract_Discount_Rate = 0;
                validity.CR_Cas_User_Validity_Contract_Hour = 0;
                validity.CR_Cas_User_Validity_Contract_Km = 0;
                validity.CR_Cas_User_Validity_Contract_Operating_Card = false;
                validity.CR_Cas_User_Validity_Contract_Reasons = "";
                validity.CR_Cas_User_Validity_Contract_Register = false;
                validity.CR_Cas_User_Validity_Contract_Renter_Address = false;
                validity.CR_Cas_User_Validity_Contract_Transfer_Permission = false;
                validity.CR_Cas_User_Validity_Contract_Traffic_License = false;
                validity.CR_Cas_User_Validity_Contract_Tires = false;
                validity.CR_Cas_User_Validity_Contract_Status = "A";
                

                db.Entry(validity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

         
            return View(cR_Cas_User_Validity_Contract);
        }

        // GET: Cas_User_Validity_Contract/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_User_Validity_Contract cR_Cas_User_Validity_Contract = db.CR_Cas_User_Validity_Contract.Find(id);
            if (cR_Cas_User_Validity_Contract == null)
            {
                return HttpNotFound();
            }
            ViewBag.CR_Cas_User_Validity_Contract_User_Id = new SelectList(db.CR_Cas_User_Information, "CR_Cas_User_Information_Id", "CR_Cas_User_Information_PassWord", cR_Cas_User_Validity_Contract.CR_Cas_User_Validity_Contract_User_Id);
            return View(cR_Cas_User_Validity_Contract);
        }

        // POST: Cas_User_Validity_Contract/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_User_Validity_Contract_User_Id,CR_Cas_User_Validity_Contract_Admin,CR_Cas_User_Validity_Contract_Register,CR_Cas_User_Validity_Contract_Chamber_Commerce,CR_Cas_User_Validity_Contract_Transfer_Permission,CR_Cas_User_Validity_Contract_Licence_Municipale,CR_Cas_User_Validity_Contract_Company_Address,CR_Cas_User_Validity_Contract_Traffic_License,CR_Cas_User_Validity_Contract_Insurance,CR_Cas_User_Validity_Contract_Operating_Card,CR_Cas_User_Validity_Contract_Chkec_Up,CR_Cas_User_Validity_Contract_Id,CR_Cas_User_Validity_Contract_Driving_License,CR_Cas_User_Validity_Contract_Renter_Address,CR_Cas_User_Validity_Contract_Employer,CR_Cas_User_Validity_Contract_Tires,CR_Cas_User_Validity_Contract_Oil,CR_Cas_User_Validity_Contract_Maintenance,CR_Cas_User_Validity_Contract_FBrake_Maintenance,CR_Cas_User_Validity_Contract_BBrake_Maintenance,CR_Cas_User_Validity_Contract_Extension,CR_Cas_User_Validity_Contract_Age,CR_Cas_User_Validity_Contract_Open_Amout_Rate,CR_Cas_User_Validity_Contract_Discount_Rate,CR_Cas_User_Validity_Contract_Km,CR_Cas_User_Validity_Contract_Hour,CR_Cas_User_Validity_Contract_Cancel,CR_Cas_User_Validity_Contract_End,CR_Cas_User_Validity_Contract_Close,CR_Cas_User_Validity_Contract_Close_Amount_Rate,CR_Cas_User_Validity_Contract_Status,CR_Cas_User_Validity_Contract_Reasons")] CR_Cas_User_Validity_Contract cR_Cas_User_Validity_Contract)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cR_Cas_User_Validity_Contract).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CR_Cas_User_Validity_Contract_User_Id = new SelectList(db.CR_Cas_User_Information, "CR_Cas_User_Information_Id", "CR_Cas_User_Information_PassWord", cR_Cas_User_Validity_Contract.CR_Cas_User_Validity_Contract_User_Id);
            return View(cR_Cas_User_Validity_Contract);
        }

        // GET: Cas_User_Validity_Contract/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_User_Validity_Contract cR_Cas_User_Validity_Contract = db.CR_Cas_User_Validity_Contract.Find(id);
            if (cR_Cas_User_Validity_Contract == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_User_Validity_Contract);
        }

        // POST: Cas_User_Validity_Contract/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Cas_User_Validity_Contract cR_Cas_User_Validity_Contract = db.CR_Cas_User_Validity_Contract.Find(id);
            db.CR_Cas_User_Validity_Contract.Remove(cR_Cas_User_Validity_Contract);
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
