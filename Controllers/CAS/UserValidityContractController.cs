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
    public class UserValidityContractController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: UserValidityContract
        public ActionResult Index()
        {
            var cR_Cas_User_Validity_Contract = db.CR_Cas_User_Validity_Contract.Include(c => c.CR_Cas_User_Information);
            return View(cR_Cas_User_Validity_Contract.ToList());
        }

        // GET: UserValidityContract/Details/5
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

        // GET: UserValidityContract/Create
        public ActionResult Create(string id=null)
        {
            
            CR_Cas_User_Validity_Contract cR_Cas_User_Validity_Contract = db.CR_Cas_User_Validity_Contract.Find(id);
            if (cR_Cas_User_Validity_Contract == null)
            {
                cR_Cas_User_Validity_Contract = new CR_Cas_User_Validity_Contract() {CR_Cas_User_Validity_Contract_User_Id=id};

            }
            ViewBag.CR_Cas_User_Validity_Contract_User_Id = new SelectList(db.CR_Cas_User_Information.Where(u=>u.CR_Cas_User_Information_Status=="A").Select(x=>new { CR_Cas_User_Information_Id=x.CR_Cas_User_Information_Id.Trim(), CR_Cas_User_Information_Ar_Name=x.CR_Cas_User_Information_Ar_Name}), "CR_Cas_User_Information_Id", "CR_Cas_User_Information_Ar_Name",id);
            return View(cR_Cas_User_Validity_Contract);
        }

        // POST: UserValidityContract/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_User_Validity_Contract_User_Id," +
            "CR_Cas_User_Validity_Contract_Admin,CR_Cas_User_Validity_Contract_Register," +
            "CR_Cas_User_Validity_Contract_Chamber_Commerce,CR_Cas_User_Validity_Contract_Transfer_Permission," +
            "CR_Cas_User_Validity_Contract_Licence_Municipale,CR_Cas_User_Validity_Contract_Company_Address," +
            "CR_Cas_User_Validity_Contract_Traffic_License,CR_Cas_User_Validity_Contract_Insurance," +
            "CR_Cas_User_Validity_Contract_Operating_Card,CR_Cas_User_Validity_Contract_Chkec_Up," +
            "CR_Cas_User_Validity_Contract_Id,CR_Cas_User_Validity_Contract_Driving_License," +
            "CR_Cas_User_Validity_Contract_Renter_Address,CR_Cas_User_Validity_Contract_Employer," +
            "CR_Cas_User_Validity_Contract_Tires,CR_Cas_User_Validity_Contract_Oil,CR_Cas_User_Validity_Contract_Maintenance," +
            "CR_Cas_User_Validity_Contract_FBrake_Maintenance,CR_Cas_User_Validity_Contract_BBrake_Maintenance," +
            "CR_Cas_User_Validity_Contract_Extension,CR_Cas_User_Validity_Contract_Age,CR_Cas_User_Validity_Contract_Open_Amout_Rate," +
            "CR_Cas_User_Validity_Contract_Discount_Rate,CR_Cas_User_Validity_Contract_Km,CR_Cas_User_Validity_Contract_Hour," +
            "CR_Cas_User_Validity_Contract_Cancel,CR_Cas_User_Validity_Contract_End,CR_Cas_User_Validity_Contract_Close," +
            "CR_Cas_User_Validity_Contract_Close_Amount_Rate,CR_Cas_User_Validity_Contract_Status,CR_Cas_User_Validity_Contract_Reasons")]
        CR_Cas_User_Validity_Contract cR_Cas_User_Validity_Contract, bool CR_Cas_User_Validity_Contract_Register)
            //,bool CR_Cas_User_Validity_Contract_Chamber_Commerce,
            //bool CR_Cas_User_Validity_Contract_Transfer_Permission,bool CR_Cas_User_Validity_Contract_Licence_Municipale,bool CR_Cas_User_Validity_Contract_Company_Address,
            //bool CR_Cas_User_Validity_Contract_Traffic_License,bool CR_Cas_User_Validity_Contract_Insurance, bool CR_Cas_User_Validity_Contract_Operating_Card,
            //bool CR_Cas_User_Validity_Contract_Chkec_Up,bool CR_Cas_User_Validity_Contract_Tires,bool CR_Cas_User_Validity_Contract_Oil,
            //bool CR_Cas_User_Validity_Contract_Maintenance,bool CR_Cas_User_Validity_Contract_FBrake_Maintenance,bool CR_Cas_User_Validity_Contract_BBrake_Maintenance,
            //bool CR_Cas_User_Validity_Contract_Id,bool CR_Cas_User_Validity_Contract_Driving_License,bool CR_Cas_User_Validity_Contract_Extension,
            //bool CR_Cas_User_Validity_Contract_Age,bool CR_Cas_User_Validity_Contract_Open_Amout_Rate,bool CR_Cas_User_Validity_Contract_Cancel,
            //bool CR_Cas_User_Validity_Contract_End,bool CR_Cas_User_Validity_Contract_Close,bool CR_Cas_User_Validity_Contract_Close_Amount_Rate
        {
            if (ModelState.IsValid)
            {
                db.CR_Cas_User_Validity_Contract.Add(cR_Cas_User_Validity_Contract);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CR_Cas_User_Validity_Contract_User_Id = new SelectList(db.CR_Cas_User_Information, "CR_Cas_User_Information_Id", "CR_Cas_User_Information_PassWord", cR_Cas_User_Validity_Contract.CR_Cas_User_Validity_Contract_User_Id);
            return View(cR_Cas_User_Validity_Contract);
        }

        // GET: UserValidityContract/Edit/5
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

        // POST: UserValidityContract/Edit/5
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

        // GET: UserValidityContract/Delete/5
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

        // POST: UserValidityContract/Delete/5
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
