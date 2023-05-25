using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentCar.Models;

namespace RentCar.Controllers.MAS
{
    public class CompanyContractTaxOwedController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: CompanyContractTaxOwed
        public ActionResult Index()
        {
            //var cR_Cas_Company_Contract = db.CR_Cas_Company_Contract.Where(c=>c.CR_Cas_Company_Contract_Code=="18" && c.CR_Cas_Company_Contract_Lessor!="1000" &&
            //(c.CR_Cas_Company_Contract_Status=="A" || c.CR_Cas_Company_Contract_Status=="E"))
            //    .Include(c => c.CR_Mas_Sup_Procedures)
            //    .Include(c => c.CR_Mas_Sup_Sector)
            //    .Include(c => c.CR_Mas_Com_Lessor);
            return View();
        }

        public PartialViewResult TaxPartialView(string type)
        {
            

            IQueryable<CR_Cas_Company_Contract> cR_Cas_Company_Contract = null;
            if (type == "E")
            {
               cR_Cas_Company_Contract = db.CR_Cas_Company_Contract.Where(c => c.CR_Cas_Company_Contract_Code == "18" && c.CR_Cas_Company_Contract_Lessor != "1000" 
               && c.CR_Cas_Company_Contract_Status == "E")
                .Include(c => c.CR_Mas_Sup_Procedures)
                .Include(c => c.CR_Mas_Sup_Sector)
                .Include(c => c.CR_Mas_Com_Lessor);
            }
            else if (type == "A")
            {
                cR_Cas_Company_Contract = db.CR_Cas_Company_Contract.Where(c => c.CR_Cas_Company_Contract_Code == "18" && c.CR_Cas_Company_Contract_Lessor != "1000" &&
                c.CR_Cas_Company_Contract_Status == "A")
                .Include(c => c.CR_Mas_Sup_Procedures)
                .Include(c => c.CR_Mas_Sup_Sector)
                .Include(c => c.CR_Mas_Com_Lessor);
            }
            else
            {
               cR_Cas_Company_Contract = db.CR_Cas_Company_Contract.Where(c => c.CR_Cas_Company_Contract_Code == "18" && c.CR_Cas_Company_Contract_Lessor != "1000" &&
                (c.CR_Cas_Company_Contract_Status == "A" || c.CR_Cas_Company_Contract_Status == "E"))
                .Include(c => c.CR_Mas_Sup_Procedures)
                .Include(c => c.CR_Mas_Sup_Sector)
                .Include(c => c.CR_Mas_Com_Lessor);
                var nbr = cR_Cas_Company_Contract.Count();
            }

            return PartialView(cR_Cas_Company_Contract);
        }

        /*public PartialViewResult Table(string type,string ID,DateTime ?StartDate,DateTime ?EndDate)
        {
            IQueryable<CR_Cas_Account_Bnan_Owed> cR_Cas_Account_Bnan_Owed = null;
            if (type == "P")
            {
                cR_Cas_Account_Bnan_Owed = db.CR_Cas_Account_Bnan_Owed.Where(b => b.CR_Cas_Account_Bnan_Owed_Contract_Com == ID && b.CR_Cas_Account_Bnan_Owed_Is_Paid==true
                && b.CR_Cas_Account_Bnan_Owed_Due_Date>=StartDate && b.CR_Cas_Account_Bnan_Owed_Due_Date<=EndDate);
            }
            else if (type == "NP")
            {
                cR_Cas_Account_Bnan_Owed = db.CR_Cas_Account_Bnan_Owed.Where(b => b.CR_Cas_Account_Bnan_Owed_Contract_Com == ID && b.CR_Cas_Account_Bnan_Owed_Is_Paid == false
                && b.CR_Cas_Account_Bnan_Owed_Due_Date >= StartDate && b.CR_Cas_Account_Bnan_Owed_Due_Date <= EndDate);
            }
            else
            {
                if(StartDate ==null && EndDate == null)
                {
                    cR_Cas_Account_Bnan_Owed = db.CR_Cas_Account_Bnan_Owed.Where(b => b.CR_Cas_Account_Bnan_Owed_Contract_Com == ID);
                }
                else
                {
                    cR_Cas_Account_Bnan_Owed = db.CR_Cas_Account_Bnan_Owed.Where(b => b.CR_Cas_Account_Bnan_Owed_Contract_Com == ID
                    && b.CR_Cas_Account_Bnan_Owed_Due_Date >= StartDate && b.CR_Cas_Account_Bnan_Owed_Due_Date <= EndDate);
                }
                
            }

            return PartialView(cR_Cas_Account_Bnan_Owed);
        }*/
        // GET: CompanyContractTaxOwed/Details/5
        /*public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cR_Cas_Account_Bnan_Owed = db.CR_Cas_Account_Bnan_Owed.Where(b=>b.CR_Cas_Account_Bnan_Owed_Contract_Com==id);
            if (cR_Cas_Account_Bnan_Owed != null)
            {
                var BnanOwed = cR_Cas_Account_Bnan_Owed.FirstOrDefault();
                if (BnanOwed == null)
                {
                    return RedirectToAction("Index");
                }
                var lessor = db.CR_Mas_Com_Lessor.FirstOrDefault(l => l.CR_Mas_Com_Lessor_Code == BnanOwed.CR_Cas_Account_Bnan_Owed_Com_Code);
                if (lessor != null)
                {
                    ViewBag.CompanyName = lessor.CR_Mas_Com_Lessor_Ar_Short_Name;
                }
                ViewBag.ContractNo = cR_Cas_Account_Bnan_Owed.FirstOrDefault().CR_Cas_Account_Bnan_Owed_Contract_Com;
                var CompanyContract = db.CR_Cas_Company_Contract.FirstOrDefault(c => c.CR_Cas_Company_Contract_No == id);
                if (CompanyContract != null)
                {
                    ViewBag.ContractFees = CompanyContract.CR_Cas_Company_Contract_Annual_Fees;
                    ViewBag.TaxPercentage = CompanyContract.CR_Cas_Company_Contract_Tax_Rate;
                    ViewBag.Discount = CompanyContract.CR_Cas_Company_Contract_Discount_Rate;
                }
                if (BnanOwed.CR_Cas_Account_Bnan_Owed_Due_Type == true)
                {
                    ViewBag.ContractType = "قيمة";
                }
                else
                {
                    ViewBag.ContractType = "نسبة";
                }
                ViewBag.Edate = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
                ViewBag.Sdate = String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-30));
            }
                
            
            return View();
        }*/

        // GET: CompanyContractTaxOwed/Create
        public ActionResult Create()
        {
            ViewBag.CR_Cas_Company_Contract_Code = new SelectList(db.CR_Mas_Sup_Procedures, "CR_Mas_Sup_Procedures_Code", "CR_Mas_Sup_Procedures_Type");
            ViewBag.CR_Cas_Company_Contract_Sector = new SelectList(db.CR_Mas_Sup_Sector, "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name");
            ViewBag.CR_Cas_Company_Contract_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Ar_Long_Name");
            return View();
        }

        // POST: CompanyContractTaxOwed/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Company_Contract_No,CR_Cas_Company_Contract_Year,CR_Cas_Company_Contract_Sector,CR_Cas_Company_Contract_Code,CR_Cas_Company_Contract_Lessor,CR_Cas_Company_Contract_Number,CR_Cas_Company_Contract_Date,CR_Cas_Company_Contract_Start_Date,CR_Cas_Company_Contract_End_Date,CR_Cas_Company_Contract_Activation,CR_Cas_Company_Contract_About_To_Expire,CR_Cas_Company_Contract_Annual_Fees,CR_Cas_Company_Contract_Service_Fees,CR_Cas_Company_Contract_Discount_Rate,CR_Cas_Company_Contract_Tax_Rate,CR_Cas_Company_Contract_Tamm_User_Id,CR_Cas_Company_Contract_Tamm_User_Password,CR_Cas_Company_Contract_Status,CR_Cas_Company_Contract_Reasons")] CR_Cas_Company_Contract cR_Cas_Company_Contract)
        {
            if (ModelState.IsValid)
            {
                db.CR_Cas_Company_Contract.Add(cR_Cas_Company_Contract);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CR_Cas_Company_Contract_Code = new SelectList(db.CR_Mas_Sup_Procedures, "CR_Mas_Sup_Procedures_Code", "CR_Mas_Sup_Procedures_Type", cR_Cas_Company_Contract.CR_Cas_Company_Contract_Code);
            ViewBag.CR_Cas_Company_Contract_Sector = new SelectList(db.CR_Mas_Sup_Sector, "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name", cR_Cas_Company_Contract.CR_Cas_Company_Contract_Sector);
            ViewBag.CR_Cas_Company_Contract_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Ar_Long_Name", cR_Cas_Company_Contract.CR_Cas_Company_Contract_Lessor);
            return View(cR_Cas_Company_Contract);
        }

        // GET: CompanyContractTaxOwed/Edit/5
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

        // POST: CompanyContractTaxOwed/Edit/5
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

        // GET: CompanyContractTaxOwed/Delete/5
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

        // POST: CompanyContractTaxOwed/Delete/5
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
