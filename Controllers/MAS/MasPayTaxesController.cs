using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentCar.Models;
using RentCar.Models.MAS;

namespace RentCar.Controllers.MAS
{
    public class MasPayTaxesController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: MasPayTaxes
        public ActionResult Index()
        {
            List<MasTracingMD> ListTracing = new List<MasTracingMD>();
            var cR_Cas_Administrative_Procedures = db.CR_Cas_Administrative_Procedures.Where(a=>a.CR_Cas_Administrative_Procedures_Code=="68" && a.CR_Cas_Administrative_Procedures_Type=="I");
            foreach(var t in cR_Cas_Administrative_Procedures)
            {
                MasTracingMD TrMD = new MasTracingMD();
                TrMD.ContractNumber = db.CR_Cas_Account_Bnan_Owed.Where(tr=>tr.CR_Cas_Account_Bnan_Owed_Pay_No==t.CR_Cas_Administrative_Procedures_No).Count();
                var CompanyCode = db.CR_Cas_Company_Contract.FirstOrDefault(c=>c.CR_Cas_Company_Contract_No==t.CR_Cas_Administrative_Procedures_Targeted_Action);
                if (CompanyCode != null)
                {
                    TrMD.CompanyName = db.CR_Mas_Com_Lessor.FirstOrDefault(l=>l.CR_Mas_Com_Lessor_Code==CompanyCode.CR_Cas_Company_Contract_Lessor).CR_Mas_Com_Lessor_Ar_Short_Name;
                }
                TrMD.UserName = db.CR_Mas_User_Information.FirstOrDefault(u=>u.CR_Mas_User_Information_Code==t.CR_Cas_Administrative_Procedures_User_Insert).CR_Mas_User_Information_Ar_Name;
                TrMD.CR_Cas_Administrative_Int_Procedures_Code = t.CR_Cas_Administrative_Int_Procedures_Code;
                TrMD.CR_Cas_Administrative_Procedures_Action = t.CR_Cas_Administrative_Procedures_Action;
                TrMD.CR_Cas_Administrative_Procedures_Code = t.CR_Cas_Administrative_Procedures_Code;
                TrMD.CR_Cas_Administrative_Procedures_Com_Supporting = t.CR_Cas_Administrative_Procedures_Com_Supporting;
                TrMD.CR_Cas_Administrative_Procedures_Date = t.CR_Cas_Administrative_Procedures_Date;
                TrMD.CR_Cas_Administrative_Procedures_Doc_Date = t.CR_Cas_Administrative_Procedures_Doc_Date;
                TrMD.CR_Cas_Administrative_Procedures_Doc_End_Date = t.CR_Cas_Administrative_Procedures_Doc_End_Date;
                TrMD.CR_Cas_Administrative_Procedures_Doc_No = t.CR_Cas_Administrative_Procedures_Doc_No;
                TrMD.CR_Cas_Administrative_Procedures_Doc_Start_Date = t.CR_Cas_Administrative_Procedures_Doc_Start_Date;
                TrMD.CR_Cas_Administrative_Procedures_From_Branch = t.CR_Cas_Administrative_Procedures_From_Branch;
                TrMD.CR_Cas_Administrative_Procedures_Lessor = t.CR_Cas_Administrative_Procedures_Lessor;
                TrMD.CR_Cas_Administrative_Procedures_No = t.CR_Cas_Administrative_Procedures_No;
                TrMD.CR_Cas_Administrative_Procedures_Reasons = t.CR_Cas_Administrative_Procedures_Reasons;
                TrMD.CR_Cas_Administrative_Procedures_Sector = t.CR_Cas_Administrative_Procedures_Sector;
                TrMD.CR_Cas_Administrative_Procedures_Targeted_Action = t.CR_Cas_Administrative_Procedures_Targeted_Action;
                TrMD.CR_Cas_Administrative_Procedures_Time = t.CR_Cas_Administrative_Procedures_Time;
                TrMD.CR_Cas_Administrative_Procedures_To_Branch = t.CR_Cas_Administrative_Procedures_To_Branch;
                TrMD.CR_Cas_Administrative_Procedures_Type = t.CR_Cas_Administrative_Procedures_Type;
                TrMD.CR_Cas_Administrative_Procedures_User_Insert = t.CR_Cas_Administrative_Procedures_User_Insert;
                TrMD.CR_Cas_Administrative_Procedures_Value = t.CR_Cas_Administrative_Procedures_Value;
                TrMD.CR_Cas_Administrative_Procedures_Year = t.CR_Cas_Administrative_Procedures_Year;
                ListTracing.Add(TrMD);
            }
            return View(ListTracing);
        }

        // GET: MasPayTaxes/Details/5
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

       /* public PartialViewResult PartialIndexTable(string ContractNo)
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
            var query = db.CR_Cas_Account_Bnan_Owed.Where(c => c.CR_Cas_Account_Bnan_Owed_Is_Paid == false && c.CR_Cas_Account_Bnan_Owed_Contract_Com==ContractNo);
            return PartialView(query.ToList());
        }*/

        public JsonResult GetContractNo(string No)
        {
            List<CR_Cas_Company_Contract> List = new List<CR_Cas_Company_Contract>();
            CR_Cas_Company_Contract Empty = new CR_Cas_Company_Contract();
            Empty.CR_Cas_Company_Contract_No = "";
            List.Add(Empty);
            db.Configuration.ProxyCreationEnabled = false;
            var Contracts = db.CR_Cas_Company_Contract.Where(x => x.CR_Cas_Company_Contract_Lessor==No && x.CR_Cas_Company_Contract_Status=="A").ToList();
            foreach(var c in Contracts)
            {
                CR_Cas_Company_Contract Contract = new CR_Cas_Company_Contract();
                Contract.CR_Cas_Company_Contract_No = c.CR_Cas_Company_Contract_No;
                List.Add(Contract);
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        // GET: MasPayTaxes/Create
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
            ViewBag.CompanyName = new SelectList(db.CR_Mas_Com_Lessor.Where(l=>l.CR_Mas_Com_Lessor_Code!="1000"), "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Ar_Short_Name");
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var autoInc = GetLastRecord(LessorCode, "68");
            ViewBag.PayNo = y + "-" + "1" + "-" + "68" + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
            ViewBag.PayDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            return View();
        }

        // POST: MasPayTaxes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string PayNo, FormCollection collection, DateTime PayDate, decimal PayedValue,string CompanyName, string ContractNo, string Reasons)
        {
            if (ModelState.IsValid)
            {
                var UserLogin = Session["UserLogin"].ToString();
                ///////////////////////////////Tracing//////////////////////////////////////
                CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
                DateTime year = DateTime.Now;
                var y = year.ToString("yy");
                var sector = "1";

                Ad.CR_Cas_Administrative_Procedures_No = PayNo;
                Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
                string currentTime = DateTime.Now.ToString("HH:mm:ss");
                Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
                Ad.CR_Cas_Administrative_Procedures_Year = y;
                Ad.CR_Cas_Administrative_Procedures_Sector = sector;
                Ad.CR_Cas_Administrative_Procedures_Code = "68";
                Ad.CR_Cas_Administrative_Int_Procedures_Code = Int32.Parse("68");
                Ad.CR_Cas_Administrative_Procedures_Lessor = "1000";
                Ad.CR_Cas_Administrative_Procedures_Targeted_Action = ContractNo;
                Ad.CR_Cas_Administrative_Procedures_User_Insert = UserLogin;
                Ad.CR_Cas_Administrative_Procedures_Type = "I";
                Ad.CR_Cas_Administrative_Procedures_Action = false;
                Ad.CR_Cas_Administrative_Procedures_Doc_Date = DateTime.Now;
               
                Ad.CR_Cas_Administrative_Procedures_Doc_No = "";
                Ad.CR_Cas_Administrative_Procedures_Value = PayedValue;
                Ad.CR_Cas_Administrative_Procedures_Reasons = Reasons;
                db.CR_Cas_Administrative_Procedures.Add(Ad);
                ///////////////////////////////////////////////////////////////////////////////

                foreach (string item in collection.AllKeys)
                {
                    if (item.StartsWith("Chk_"))
                    {
                        var Key = item.Replace("Chk_", "");
                        var Taxes = db.CR_Cas_Account_Bnan_Owed.Single(t => t.CR_Cas_Account_Bnan_Owed_Contract_No == Key);
                        if (Taxes != null)
                        {
                            Taxes.CR_Cas_Account_Bnan_Owed_Is_Paid = true;
                            Taxes.CR_Cas_Account_Bnan_Owed_Pay_Date = PayDate;
                            Taxes.CR_Cas_Account_Bnan_Owed_Pay_No = PayNo;
                            db.Entry(Taxes).State = EntityState.Modified;
                        }
                    }
                }

                TempData["TempModel"] = "Saved";
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            
            return View();
        }

        // GET: MasPayTaxes/Edit/5
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
            ViewBag.CR_Cas_Company_Contract_Code = new SelectList(db.CR_Mas_Sup_Procedures, "CR_Mas_Sup_Procedures_Code", "CR_Mas_Sup_Procedures_Type", cR_Cas_Company_Contract.CR_Cas_Company_Contract_Code);
            ViewBag.CR_Cas_Company_Contract_Sector = new SelectList(db.CR_Mas_Sup_Sector, "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name", cR_Cas_Company_Contract.CR_Cas_Company_Contract_Sector);
            ViewBag.CR_Cas_Company_Contract_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Ar_Long_Name", cR_Cas_Company_Contract.CR_Cas_Company_Contract_Lessor);
            return View(cR_Cas_Company_Contract);
        }

        // POST: MasPayTaxes/Edit/5
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
            ViewBag.CR_Cas_Company_Contract_Code = new SelectList(db.CR_Mas_Sup_Procedures, "CR_Mas_Sup_Procedures_Code", "CR_Mas_Sup_Procedures_Type", cR_Cas_Company_Contract.CR_Cas_Company_Contract_Code);
            ViewBag.CR_Cas_Company_Contract_Sector = new SelectList(db.CR_Mas_Sup_Sector, "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name", cR_Cas_Company_Contract.CR_Cas_Company_Contract_Sector);
            ViewBag.CR_Cas_Company_Contract_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Ar_Long_Name", cR_Cas_Company_Contract.CR_Cas_Company_Contract_Lessor);
            return View(cR_Cas_Company_Contract);
        }

        // GET: MasPayTaxes/Delete/5
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

        // POST: MasPayTaxes/Delete/5
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
