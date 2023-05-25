using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentCar.Models;

namespace RentCar.Controllers.CAS
{
    public class CasRenterContractsController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: CasRenterContracts
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
                    return RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }


            ViewBag.Startdate = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-30));
            ViewBag.Enddate= string.Format("{0:yyyy-MM-dd}", DateTime.Now);


            //var cR_Cas_Contract_Basic = db.CR_Cas_Contract_Basic.Where(c => c.CR_Cas_Contract_Basic_Lessor == LessorCode && c.CR_Cas_Contract_Basic_Owner_Branch == BranchCode 
            //&& c.CR_Cas_Contract_Basic_Status != "U" && c.CR_Cas_Contract_Basic_Status!="y")
            //    .OrderByDescending(d => d.CR_Cas_Contract_Basic_Date)
            //    .Include(c => c.CR_Mas_Com_Lessor).Include(car => car.CR_Cas_Sup_Car_Information);
            return View();
        }
       
        public PartialViewResult Table(string type,string StartDate,string EndDate)
        {

            DateTime sd ;
            DateTime ed ;
            if (StartDate==null|| EndDate == null)
            {
                sd = DateTime.Now.AddDays(-30);
                ed = Convert.ToDateTime(DateTime.Now);
            }
            else
            {
                sd = Convert.ToDateTime(StartDate);
                ed = Convert.ToDateTime(EndDate);
            }
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
             
            IQueryable<CR_Cas_Contract_Basic> query;
            if (type == "E" && StartDate != "" && EndDate != "")
            {
                query = db.CR_Cas_Contract_Basic.Where(c => c.CR_Cas_Contract_Basic_Lessor == LessorCode
                   && c.CR_Cas_Contract_Basic_Status == "E" && c.CR_Cas_Contract_Basic_Date >= sd && c.CR_Cas_Contract_Basic_Date <= ed).OrderByDescending(d => d.CR_Cas_Contract_Basic_Date)
               .Include(c => c.CR_Mas_Com_Lessor).Include(car => car.CR_Cas_Sup_Car_Information);
            }  
            else if (type == "A" && StartDate != "" && EndDate != "")
            {
                query = db.CR_Cas_Contract_Basic.Where(c => c.CR_Cas_Contract_Basic_Lessor == LessorCode
                  && c.CR_Cas_Contract_Basic_Status == "A" && c.CR_Cas_Contract_Basic_Date >= sd && c.CR_Cas_Contract_Basic_Date <= ed).OrderByDescending(d => d.CR_Cas_Contract_Basic_Date)
               .Include(c => c.CR_Mas_Com_Lessor).Include(car => car.CR_Cas_Sup_Car_Information);
            }
            else if (type == "C" && StartDate != "" && EndDate != "")
            {
                query = db.CR_Cas_Contract_Basic.Where(c => c.CR_Cas_Contract_Basic_Lessor == LessorCode
                  && c.CR_Cas_Contract_Basic_Status == "C" && c.CR_Cas_Contract_Basic_Date >= sd && c.CR_Cas_Contract_Basic_Date <= ed).OrderByDescending(d => d.CR_Cas_Contract_Basic_Date)
               .Include(c => c.CR_Mas_Com_Lessor).Include(car => car.CR_Cas_Sup_Car_Information);
            }
            else if (type == "Date" && StartDate!="" && EndDate!="")
            {
                query = db.CR_Cas_Contract_Basic.Where(c => c.CR_Cas_Contract_Basic_Lessor == LessorCode
                  && c.CR_Cas_Contract_Basic_Date >= sd && c.CR_Cas_Contract_Basic_Date <= ed && c.CR_Cas_Contract_Basic_Status!="U").OrderByDescending(d => d.CR_Cas_Contract_Basic_Date)
                    .Include(c => c.CR_Mas_Com_Lessor).Include(car => car.CR_Cas_Sup_Car_Information);
                var nb = query.Count();
            }
            else
            {
                query = db.CR_Cas_Contract_Basic.Where(c => c.CR_Cas_Contract_Basic_Lessor == LessorCode && c.CR_Cas_Contract_Basic_Status!="U" 
                && c.CR_Cas_Contract_Basic_Status != "y" && c.CR_Cas_Contract_Basic_Date >= sd && c.CR_Cas_Contract_Basic_Date <= ed)
               .OrderByDescending(d => d.CR_Cas_Contract_Basic_Date)
               .Include(c => c.CR_Mas_Com_Lessor).Include(car => car.CR_Cas_Sup_Car_Information);
            }
            
            return PartialView(query.ToList());
        }




        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Contract_Basic cR_Cas_Contract_Basic = db.CR_Cas_Contract_Basic.Find(id);
            if (cR_Cas_Contract_Basic == null)
            {
                return HttpNotFound();
            }
            ViewBag.CR_Cas_Contract_Basic_Car_Price_Basic_No = new SelectList(db.CR_Cas_Car_Price_Basic, "CR_Cas_Car_Price_Basic_No", "CR_Cas_Car_Price_Basic_Year", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Car_Price_Basic_No);
            ViewBag.CR_Cas_Contract_Basic_Car_Serail_No = new SelectList(db.CR_Cas_Sup_Car_Information, "CR_Cas_Sup_Car_Serail_No", "CR_Cas_Sup_Car_Lessor_Code", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Car_Serail_No);
            ViewBag.CR_Cas_Contract_Basic_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Lessor);
            ViewBag.CR_Cas_Contract_Basic_Renter_Id = new SelectList(db.CR_Mas_Renter_Information, "CR_Mas_Renter_Information_Id", "CR_Mas_Renter_Information_Sector", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Renter_Id);
            ViewBag.CR_Cas_Contract_Basic_Sector = new SelectList(db.CR_Mas_Sup_Sector, "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Sector);
            return View(cR_Cas_Contract_Basic);
        }

        // POST: CasRenterContracts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Contract_Basic_No,CR_Cas_Contract_Basic_Copy,CR_Cas_Contract_Basic_Year,CR_Cas_Contract_Basic_Type,CR_Cas_Contract_Basic_Lessor,CR_Cas_Contract_Basic_Sector,CR_Cas_Contract_Basic_Owner_Branch,CR_Cas_Contract_Basic_Date,CR_Cas_Contract_Basic_Time,CR_Cas_Contract_Basic_Renter_Id,CR_Cas_Contract_Basic_is_Renter_Driver,CR_Cas_Contract_Basic_Driver_Id,CR_Cas_Contract_Basic_Car_Serail_No,CR_Cas_Contract_Basic_is_Additional_Driver,CR_Cas_Contract_Basic_Additional_Driver_Id,CR_Cas_Contract_Basic_Start_Date,CR_Cas_Contract_Basic_Start_Time,CR_Cas_Contract_Basic_Expected_End_Date,CR_Cas_Contract_Basic_Expected_End_Time,CR_Cas_Contract_Basic_Expected_Rental_Days,CR_Cas_Contract_Basic_Additional_Driver_Value,CR_Cas_Contract_Basic_Authorization_Value,CR_Cas_Contract_Basic_Use_Within_Country,CR_Cas_Contract_Basic_End_Authorization,CR_Cas_Contract_BasicAuthorization_Staus,CR_Cas_Contract_BasicAuthorization_No,CR_Cas_Contract_Basic_Daily_Rent,CR_Cas_Contract_Basic_Weekly_Rent,CR_Cas_Contract_Basic_Monthly_Rent,CR_Cas_Contract_Basic_CurrentMeters,CR_Cas_Contract_Basic_Additional_Value,CR_Cas_Contract_Basic_Choices_Value,CR_Cas_Contract_Basic_Tax_Rate,CR_Cas_Contract_Basic_User_Discount,CR_Cas_Contract_Basic_Daily_Free_KM,CR_Cas_Contract_Basic_Additional_KM_Value,CR_Cas_Contract_Basic_Free_Additional_Hours,CR_Cas_Contract_Basic_Hour_Max,CR_Cas_Contract_Basic_Extra_Hour_Value,CR_Cas_Contract_Basic_is_Km_Open,CR_Cas_Contract_Basic_is_Receiving_Branch,CR_Cas_Contract_Basic_Value,CR_Cas_Contract_Basic_Discount_Value,CR_Cas_Contract_Basic_After_Discount_Value,CR_Cas_Contract_Basic_Tax_Value,CR_Cas_Contract_Basic_Net_Value,CR_Cas_Contract_Basic_Payed_Value,CR_Cas_Contract_Basic_Previous_Balance,CR_Cas_Contract_Basic_User_Insert,CR_Cas_Contract_Basic_Hour_DateTime_Alert,CR_Cas_Contract_Basic_Hour_MsgNo,CR_Cas_Contract_Basic_Day_DateTime_Alert,CR_Cas_Contract_Basic_Day_MsgNo,CR_Cas_Contract_Basic_EndContract_MsgNo,CR_Cas_Contract_Basic_Alert_Status,CR_Cas_Contract_Basic_CreateContract_Pdf,CR_Cas_Contract_Basic_CloseContract_Pdf,CR_Cas_Contract_Basic_Car_Price_Basic_No,CR_Cas_Contract_Basic_Status")] CR_Cas_Contract_Basic cR_Cas_Contract_Basic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cR_Cas_Contract_Basic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CR_Cas_Contract_Basic_Car_Price_Basic_No = new SelectList(db.CR_Cas_Car_Price_Basic, "CR_Cas_Car_Price_Basic_No", "CR_Cas_Car_Price_Basic_Year", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Car_Price_Basic_No);
            ViewBag.CR_Cas_Contract_Basic_Car_Serail_No = new SelectList(db.CR_Cas_Sup_Car_Information, "CR_Cas_Sup_Car_Serail_No", "CR_Cas_Sup_Car_Lessor_Code", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Car_Serail_No);
            ViewBag.CR_Cas_Contract_Basic_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Lessor);
            ViewBag.CR_Cas_Contract_Basic_Renter_Id = new SelectList(db.CR_Mas_Renter_Information, "CR_Mas_Renter_Information_Id", "CR_Mas_Renter_Information_Sector", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Renter_Id);
            ViewBag.CR_Cas_Contract_Basic_Sector = new SelectList(db.CR_Mas_Sup_Sector, "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Sector);
            return View(cR_Cas_Contract_Basic);
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
