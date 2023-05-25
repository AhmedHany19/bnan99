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
    public class Company_ContractController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: Company_Contract
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
            var cR_Cas_Company_Contract = db.CR_Cas_Company_Contract.Where(c =>c.CR_Cas_Company_Contract_Status != "D" && c.CR_Cas_Company_Contract_Code=="18" && c.CR_Cas_Company_Contract_Lessor!="1000")
                .Include(c => c.CR_Mas_Com_Lessor).Include(c => c.CR_Mas_Sup_Sector).Include(c => c.CR_Mas_Sup_Procedures);
            //var n = cR_Cas_Company_Contract.Count();
            return View(cR_Cas_Company_Contract.ToList());
        }

        public PartialViewResult PartialViewBnanFees()
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
            var Fees = db.CR_Mas_Sup_Service_Fee_Bnan.Where(f => f.CR_Mas_Sup_Service_Fee_Bnan_Status != "D");
            return PartialView(Fees);
        }

        // GET: Company_Contract/Details/5
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

        // GET: Company_Contract/Create
        public ActionResult Create()
        {
            ViewBag.CR_Cas_Company_Contract_Code = new SelectList(db.CR_Mas_Sup_Procedures, "CR_Mas_Sup_Procedures_Code", "CR_Mas_Sup_Procedures_Type");
            ViewBag.CR_Cas_Company_Contract_Sector = new SelectList(db.CR_Mas_Sup_Sector, "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name");
            ViewBag.CR_Cas_Company_Contract_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo");
            return View();
        }

        // POST: Company_Contract/Create
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
            ViewBag.CR_Cas_Company_Contract_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Company_Contract.CR_Cas_Company_Contract_Lessor);
            return View(cR_Cas_Company_Contract);
        }

        // GET: Company_Contract/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Company_Contract cR_Cas_Company_Contract = db.CR_Cas_Company_Contract.OrderByDescending(z => z.CR_Cas_Company_Contract_No)
                .FirstOrDefault(z => z.CR_Cas_Company_Contract_No == id && z.CR_Cas_Company_Contract_Status!="D");
            if (cR_Cas_Company_Contract == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Cas_Company_Contract.CR_Cas_Company_Contract_No != null)
                {
                    cR_Cas_Company_Contract.CR_Cas_Company_Contract_No = cR_Cas_Company_Contract.CR_Cas_Company_Contract_No.Trim();
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
            }
            

            return View(cR_Cas_Company_Contract);

        }

        public CR_Cas_Company_Contract GetContractLastRecord(string ProcedureCode, string sector,string LessorCode)
        {
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            //var LessorCode = Session["LessorCode"].ToString();
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

        // POST: Company_Contract/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Company_Contract_No,CR_Cas_Company_Contract_Year," +
            "CR_Cas_Company_Contract_Sector,CR_Cas_Company_Contract_Code,CR_Cas_Company_Contract_Lessor," +
            "CR_Cas_Company_Contract_Number,CR_Cas_Company_Contract_Date,CR_Cas_Company_Contract_Start_Date," +
            "CR_Cas_Company_Contract_End_Date,CR_Cas_Company_Contract_Activation,CR_Cas_Company_Contract_About_To_Expire," +
            "CR_Cas_Company_Contract_Annual_Fees,CR_Cas_Company_Contract_Service_Fees,CR_Cas_Company_Contract_Discount_Rate," +
            "CR_Cas_Company_Contract_Tax_Rate,CR_Cas_Company_Contract_Tamm_User_Id,CR_Cas_Company_Contract_Tamm_User_Password," +
            "CR_Cas_Company_Contract_Status,CR_Cas_Company_Contract_Reasons")] CR_Cas_Company_Contract cR_Cas_Company_Contract,
            DateTime CR_Cas_Company_Contract_Date,DateTime CR_Cas_Company_Contract_Start_Date, DateTime CR_Cas_Company_Contract_End_Date,
            string save, string delete,bool? Price)
        {
           
            if (ModelState.IsValid)
            {
                var ProcedureCode = "18";
                //var LessorCode = "1000";
                DateTime year = DateTime.Now;
                var y = year.ToString("yy");
    
                var checkContractExist = db.CR_Cas_Company_Contract.Any(f => f.CR_Cas_Company_Contract_No == cR_Cas_Company_Contract.CR_Cas_Company_Contract_No && f.CR_Cas_Company_Contract_Status == "A");
                if (!string.IsNullOrEmpty(save) && !checkContractExist)
                {
                    cR_Cas_Company_Contract.CR_Cas_Company_Contract_Date = CR_Cas_Company_Contract_Date;
                    cR_Cas_Company_Contract.CR_Cas_Company_Contract_Start_Date = CR_Cas_Company_Contract_Start_Date;
                    cR_Cas_Company_Contract.CR_Cas_Company_Contract_End_Date = CR_Cas_Company_Contract_End_Date;
                    cR_Cas_Company_Contract.CR_Cas_Company_Contract_Year = y;
                    cR_Cas_Company_Contract.CR_Cas_Company_Contract_Code = ProcedureCode;
                    //cR_Cas_Company_Contract.CR_Cas_Company_Contract_Lessor = "1000";
                    cR_Cas_Company_Contract.CR_Cas_Company_Contract_Sector = "1";
                    cR_Cas_Company_Contract.CR_Cas_Company_Contract_Status = "A";
                    /////////////////////////check with project leader///////////////////////
                    //var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.FirstOrDefault(x => x.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == LessorCode &&
                    //            x.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code == ProcedureCode);
                    //DateTime currentDate = (DateTime)CR_Cas_Company_Contract_End_Date;
                    //var nbr = Convert.ToDouble(mech.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire);
                    //var d = currentDate.AddDays(-nbr);
                    //cR_Cas_Company_Contract.CR_Cas_Company_Contract_About_To_Expire = d;
                    /////////////////////////////////////////////////////////////////////////
                    db.Entry(cR_Cas_Company_Contract).State = EntityState.Modified;

                    ////////////////////////////////Save Contract services///////////////////////////////////////
                    var servicefee = db.CR_Mas_Sup_Service_Fee_Bnan.Where(x => x.CR_Mas_Sup_Service_Fee_Bnan_Status == "A");

                    List<CR_Mas_Service_Bnan_Contract> lService = new List<CR_Mas_Service_Bnan_Contract>();
                    CR_Mas_Service_Bnan_Contract Service;
                    foreach (var s in servicefee)
                    {
                        Service = new CR_Mas_Service_Bnan_Contract();
                        Service.CR_Mas_Service_Bnan_Contract_No = cR_Cas_Company_Contract.CR_Cas_Company_Contract_No;
                        Service.CR_Mas_Service_Bnan_Contract_Code = s.CR_Mas_Sup_Service_Fee_Bnan_Code;
                        Service.CR_Mas_Service_Bnan_Contract_Price = s.CR_Mas_Sup_Service_Fee_Bnan_Price;
                        Service.CR_Mas_Service_Bnan_Contract_Price_Or_Percentage = Price;
                        if (Price == true)
                        {
                            Service.CR_Mas_Service_Bnan_Contract_Value = s.CR_Mas_Sup_Service_Fee_Bnan_Value;
                            Service.CR_Mas_Service_Bnan_Contract_Percentage = 0;
                        }
                        else
                        {
                            Service.CR_Mas_Service_Bnan_Contract_Percentage = s.CR_Mas_Sup_Service_Fee_Bnan_Percentage;
                            Service.CR_Mas_Service_Bnan_Contract_Value = 0;
                        }
                        
                        


                        lService.Add(Service);
                    }
                    lService.ForEach(sf => db.CR_Mas_Service_Bnan_Contract.Add(sf));
                    /////////////////////////////////////////////////////////////////////////////////////////////

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else if (delete == "Delete" || delete == "حذف")
                {
                    db.CR_Cas_Company_Contract.Attach(cR_Cas_Company_Contract);
                    cR_Cas_Company_Contract.CR_Cas_Company_Contract_Status = "D";
                    db.Entry(cR_Cas_Company_Contract).Property(x => x.CR_Cas_Company_Contract_Status).IsModified = true;

                    ////////////////////////////////Add New Contract////////////////////////////////

                    CR_Cas_Company_Contract cont = new CR_Cas_Company_Contract();

                    var ContractAutoInc = GetContractLastRecord(ProcedureCode, "1",cR_Cas_Company_Contract.CR_Cas_Company_Contract_Lessor).CR_Cas_Company_Contract_No;

                    cont.CR_Cas_Company_Contract_No = y + "-1-" + ProcedureCode + "-" + cR_Cas_Company_Contract.CR_Cas_Company_Contract_Lessor + "-" + ContractAutoInc;
                    cont.CR_Cas_Company_Contract_Lessor = cR_Cas_Company_Contract.CR_Cas_Company_Contract_Lessor;

                    cont.CR_Cas_Company_Contract_Code = ProcedureCode;
                    cont.CR_Cas_Company_Contract_Year = y;
                    cont.CR_Cas_Company_Contract_Sector = "1";
                    
                    cont.CR_Cas_Company_Contract_Number = null;
                    cont.CR_Cas_Company_Contract_Date = null;
                    cont.CR_Cas_Company_Contract_Start_Date = null;
                    cont.CR_Cas_Company_Contract_End_Date = null;
                    cont.CR_Cas_Company_Contract_Status = "N";
                    db.CR_Cas_Company_Contract.Add(cont);

                    db.SaveChanges();
                    TempData["TempModel"] = "Deleted";
                    ViewBag.stat = "حذف";
                    return RedirectToAction("Index", "Company_Contract");
                    /// //////////////////////////////////////////////////////////////////////////
                }
            }
            return View(cR_Cas_Company_Contract);
        }

        // GET: Company_Contract/Delete/5
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

        // POST: Company_Contract/Delete/5
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
