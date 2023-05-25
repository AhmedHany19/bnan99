using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentCar.Models;
using RentCar.Models.CAS;

namespace RentCar.Controllers.CAS
{
    public class PayTaxesController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: PayTaxes
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
                RedirectToAction("Login", "Account");
            }

            List<TracingModel> ListTracing = new List<TracingModel>();
            var tracing = db.CR_Cas_Administrative_Procedures.Where(t => t.CR_Cas_Administrative_Procedures_Code == "66" && t.CR_Cas_Administrative_Procedures_Lessor == LessorCode
              && t.CR_Cas_Administrative_Procedures_Type == "I" && t.CR_Cas_Administrative_Procedures_Type != "D");
            foreach (var t in tracing)
            {
                TracingModel TM = new TracingModel();
                TM.CR_Cas_Administrative_Int_Procedures_Code = t.CR_Cas_Administrative_Int_Procedures_Code;
                TM.CR_Cas_Administrative_Procedures_Action = t.CR_Cas_Administrative_Procedures_Action;
                TM.CR_Cas_Administrative_Procedures_Code = t.CR_Cas_Administrative_Procedures_Code;
                TM.CR_Cas_Administrative_Procedures_Com_Supporting = t.CR_Cas_Administrative_Procedures_Com_Supporting;
                TM.CR_Cas_Administrative_Procedures_Date = t.CR_Cas_Administrative_Procedures_Date;
                TM.CR_Cas_Administrative_Procedures_Doc_Date = t.CR_Cas_Administrative_Procedures_Doc_Date;
                TM.CR_Cas_Administrative_Procedures_Doc_End_Date = t.CR_Cas_Administrative_Procedures_Doc_End_Date;
                TM.CR_Cas_Administrative_Procedures_Doc_No = t.CR_Cas_Administrative_Procedures_Doc_No;
                TM.CR_Cas_Administrative_Procedures_Doc_Start_Date = t.CR_Cas_Administrative_Procedures_Doc_Start_Date;
                TM.CR_Cas_Administrative_Procedures_From_Branch = t.CR_Cas_Administrative_Procedures_From_Branch;
                TM.CR_Cas_Administrative_Procedures_Lessor = t.CR_Cas_Administrative_Procedures_Lessor;
                TM.CR_Cas_Administrative_Procedures_No = t.CR_Cas_Administrative_Procedures_No;
                TM.CR_Cas_Administrative_Procedures_Reasons = t.CR_Cas_Administrative_Procedures_Reasons;
                TM.CR_Cas_Administrative_Procedures_Sector = t.CR_Cas_Administrative_Procedures_Sector;
                TM.CR_Cas_Administrative_Procedures_Targeted_Action = t.CR_Cas_Administrative_Procedures_Targeted_Action;
                TM.CR_Cas_Administrative_Procedures_Time = t.CR_Cas_Administrative_Procedures_Time;
                TM.CR_Cas_Administrative_Procedures_To_Branch = t.CR_Cas_Administrative_Procedures_To_Branch;
                TM.CR_Cas_Administrative_Procedures_Type = t.CR_Cas_Administrative_Procedures_Type;
                TM.CR_Cas_Administrative_Procedures_Value = t.CR_Cas_Administrative_Procedures_Value;
                TM.CR_Cas_Administrative_Procedures_Year = t.CR_Cas_Administrative_Procedures_Year;
                var user = db.CR_Cas_User_Information.FirstOrDefault(u => u.CR_Cas_User_Information_Id == t.CR_Cas_Administrative_Procedures_User_Insert && u.CR_Cas_User_Information_Lessor_Code == LessorCode);
                if (user != null)
                {
                    TM.UserName = user.CR_Cas_User_Information_Ar_Name;
                }
                var taxes = db.CR_Cas_Account_Tax_Owed.Where(tax => tax.CR_Cas_Account_Tax_Owed_Pay_No == TM.CR_Cas_Administrative_Procedures_No);
                TM.ContractNo = taxes.Count();
                ListTracing.Add(TM);
            }

            return View(ListTracing);
        }

        public PartialViewResult PartialIndexTable()
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
            var query = db.CR_Cas_Account_Tax_Owed.Where(c => c.CR_Cas_Account_Tax_Owed_Com_Code == LessorCode && c.CR_Cas_Account_Tax_Owed_Is_Paid==false);
            return PartialView(query.ToList());
        }

        public PartialViewResult PartialIndexTableEdit(string id)
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
            var query = db.CR_Cas_Account_Tax_Owed.Where(c => c.CR_Cas_Account_Tax_Owed_Pay_No == id);
            return PartialView(query.ToList());
        }

        // GET: PayTaxes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Administrative_Procedures cR_Cas_Administrative_Procedures = db.CR_Cas_Administrative_Procedures.Find(id);
            if (cR_Cas_Administrative_Procedures == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Administrative_Procedures);
        }

        // GET: PayTaxes/Create
        public CR_Cas_Administrative_Procedures GetLastRecord(string Lessorcode, string ProcedureCode)
        {
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var Lrecord = db.CR_Cas_Administrative_Procedures.Where(x => x.CR_Cas_Administrative_Procedures_Lessor == Lessorcode
                && x.CR_Cas_Administrative_Procedures_Code == ProcedureCode && x.CR_Cas_Administrative_Procedures_Year == y)
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
                RedirectToAction("Login", "Account");
            }

            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var autoInc = GetLastRecord(LessorCode, "66");
            ViewBag.Administrative_Procedures_No = y + "-" + "1" + "-" + "66" + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
            ViewBag.PayDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            return View();
        }

        // POST: PayTaxes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Administrative_Procedures_No,CR_Cas_Administrative_Procedures_Date," +
            "CR_Cas_Administrative_Procedures_Time,CR_Cas_Administrative_Procedures_Year,CR_Cas_Administrative_Procedures_Sector," +
            "CR_Cas_Administrative_Procedures_Code,CR_Cas_Administrative_Int_Procedures_Code,CR_Cas_Administrative_Procedures_Lessor," +
            "CR_Cas_Administrative_Procedures_Targeted_Action,CR_Cas_Administrative_Procedures_Com_Supporting,CR_Cas_Administrative_Procedures_Value," +
            "CR_Cas_Administrative_Procedures_Doc_No,CR_Cas_Administrative_Procedures_Doc_Date,CR_Cas_Administrative_Procedures_Doc_Start_Date," +
            "CR_Cas_Administrative_Procedures_Doc_End_Date,CR_Cas_Administrative_Procedures_From_Branch,CR_Cas_Administrative_Procedures_To_Branch," +
            "CR_Cas_Administrative_Procedures_Action,CR_Cas_Administrative_Procedures_User_Insert,CR_Cas_Administrative_Procedures_Type,CR_Cas_Administrative_Procedures_Reasons")]
        CR_Cas_Administrative_Procedures cR_Cas_Administrative_Procedures,string Reasons,DateTime PayDate,decimal PayedVal, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                var LessorCode = Session["LessorCode"].ToString();
                ///////////////////////////////Tracing//////////////////////////////////////
                CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
                DateTime year = DateTime.Now;
                var y = year.ToString("yy");
                var sector = "1";
                var autoInc = GetLastRecord(LessorCode, "66");
                

                Ad.CR_Cas_Administrative_Procedures_No = y + "-" + sector + "-" + "66" + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
                Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
                string currentTime = DateTime.Now.ToString("HH:mm:ss");
                Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
                Ad.CR_Cas_Administrative_Procedures_Year = y;
                Ad.CR_Cas_Administrative_Procedures_Sector = sector;
                Ad.CR_Cas_Administrative_Procedures_Code = "66";
                Ad.CR_Cas_Administrative_Int_Procedures_Code = 66;
                Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
                
                Ad.CR_Cas_Administrative_Procedures_User_Insert = Session["UserLogin"].ToString();
                Ad.CR_Cas_Administrative_Procedures_Type = "I";
                Ad.CR_Cas_Administrative_Procedures_Action = true;
                Ad.CR_Cas_Administrative_Procedures_Value = PayedVal;
                Ad.CR_Cas_Administrative_Procedures_Action = true;
                Ad.CR_Cas_Administrative_Procedures_Doc_No = "";
                Ad.CR_Cas_Administrative_Procedures_Reasons = Reasons;
                db.CR_Cas_Administrative_Procedures.Add(Ad);
                //////////////////////////////////////////////////////////////////////////////

                foreach (string item in collection.AllKeys)
                {
                    if (item.StartsWith("Chk_"))
                    {
                        var Key = item.Replace("Chk_", "");
                        var Taxes = db.CR_Cas_Account_Tax_Owed.Single(t => t.CR_Cas_Account_Tax_Owed_Contract_No == Key);
                        if (Taxes != null)
                        {
                            Taxes.CR_Cas_Account_Tax_Owed_Is_Paid = true;
                            Taxes.CR_Cas_Account_Tax_Owed_Pay_Date = PayDate;
                            Taxes.CR_Cas_Account_Tax_Owed_Pay_No = Ad.CR_Cas_Administrative_Procedures_No;
                            db.Entry(Taxes).State = EntityState.Modified;
                        }
                    }
                }

                TempData["TempModel"] = "Saved";
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Indexs", "PayTaxes");
        }

        // GET: PayTaxes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Administrative_Procedures cR_Cas_Administrative_Procedures = db.CR_Cas_Administrative_Procedures.Find(id);
            if (cR_Cas_Administrative_Procedures == null)
            {
                return HttpNotFound();
            }
            ViewBag.Administrative_Procedures_No = id;
            ViewBag.PayDate = cR_Cas_Administrative_Procedures.CR_Cas_Administrative_Procedures_Date;
            ViewBag.PayedVal = cR_Cas_Administrative_Procedures.CR_Cas_Administrative_Procedures_Value;
            ViewBag.Reasons = cR_Cas_Administrative_Procedures.CR_Cas_Administrative_Procedures_Reasons;
            return View(cR_Cas_Administrative_Procedures);
        }

        // POST: PayTaxes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Administrative_Procedures_No,CR_Cas_Administrative_Procedures_Date," +
            "CR_Cas_Administrative_Procedures_Time,CR_Cas_Administrative_Procedures_Year,CR_Cas_Administrative_Procedures_Sector," +
            "CR_Cas_Administrative_Procedures_Code,CR_Cas_Administrative_Int_Procedures_Code,CR_Cas_Administrative_Procedures_Lessor," +
            "CR_Cas_Administrative_Procedures_Targeted_Action,CR_Cas_Administrative_Procedures_Com_Supporting," +
            "CR_Cas_Administrative_Procedures_Value,CR_Cas_Administrative_Procedures_Doc_No,CR_Cas_Administrative_Procedures_Doc_Date," +
            "CR_Cas_Administrative_Procedures_Doc_Start_Date,CR_Cas_Administrative_Procedures_Doc_End_Date," +
            "CR_Cas_Administrative_Procedures_From_Branch,CR_Cas_Administrative_Procedures_To_Branch,CR_Cas_Administrative_Procedures_Action," +
            "CR_Cas_Administrative_Procedures_User_Insert,CR_Cas_Administrative_Procedures_Type,CR_Cas_Administrative_Procedures_Reasons")] 
        CR_Cas_Administrative_Procedures cR_Cas_Administrative_Procedures , string Reasons)
        {
            if (ModelState.IsValid)
            {
                cR_Cas_Administrative_Procedures.CR_Cas_Administrative_Procedures_Type = "D";
                cR_Cas_Administrative_Procedures.CR_Cas_Administrative_Procedures_Reasons = Reasons;
                db.Entry(cR_Cas_Administrative_Procedures).State = EntityState.Modified;

                var Taxes = db.CR_Cas_Account_Tax_Owed.Where(t=>t.CR_Cas_Account_Tax_Owed_Pay_No==cR_Cas_Administrative_Procedures.CR_Cas_Administrative_Procedures_No);
                foreach(var t in Taxes)
                {
                    t.CR_Cas_Account_Tax_Owed_Is_Paid = false;
                    t.CR_Cas_Account_Tax_Owed_Pay_Date = null;
                    t.CR_Cas_Account_Tax_Owed_Pay_No = null;
                    db.Entry(t).State = EntityState.Modified;
                }
                db.SaveChanges();
                TempData["TempModel"] = "Deleted";
                return RedirectToAction("Index");
            }
            return View(cR_Cas_Administrative_Procedures);
        }

        // GET: PayTaxes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Administrative_Procedures cR_Cas_Administrative_Procedures = db.CR_Cas_Administrative_Procedures.Find(id);
            if (cR_Cas_Administrative_Procedures == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Administrative_Procedures);
        }

        // POST: PayTaxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Cas_Administrative_Procedures cR_Cas_Administrative_Procedures = db.CR_Cas_Administrative_Procedures.Find(id);
            db.CR_Cas_Administrative_Procedures.Remove(cR_Cas_Administrative_Procedures);
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
