using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentCar.Models;

namespace RentCar.Controllers
{
    public class RenterAddressController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: RenterAddress
        public ActionResult Index()
        {
            return View(db.CR_MAS_Renter_Address.ToList());
        }

        // GET: RenterAddress/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_MAS_Renter_Address cR_MAS_Renter_Address = db.CR_MAS_Renter_Address.Find(id);
            if (cR_MAS_Renter_Address == null)
            {
                return HttpNotFound();
            }
            return View(cR_MAS_Renter_Address);
        }

        // GET: RenterAddress/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RenterAddress/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Mas_Renter_Address_Id_Code,CR_Mas_Renter_Address_Regions,CR_Mas_Renter_Address_City,CR_Mas_Renter_Address_Ar_District,CR_Mas_Renter_Address_En_District,CR_Mas_Renter_Address_Fr_District,CR_Mas_Renter_Address_Ar_Street,CR_Mas_Renter_Address_En_Street,CR_Mas_Renter_Address_Fr_Street,CR_Mas_Renter_Address_Building,CR_Mas_Renter_Address_Unit_No,CR_Mas_Renter_Address_Zip_Code,CR_Mas_Renter_Address_Additional_Numbers,CR_Mas_Renter_Address_UpDate_Post,CR_Mas_Renter_Address_Status,CR_Mas_Renter_Address_Reasons")] CR_MAS_Renter_Address cR_MAS_Renter_Address)
        {
            if (ModelState.IsValid)
            {
                db.CR_MAS_Renter_Address.Add(cR_MAS_Renter_Address);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cR_MAS_Renter_Address);
        }

        // GET: RenterAddress/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_MAS_Renter_Address cR_MAS_Renter_Address = db.CR_MAS_Renter_Address.Find(id);
            if (cR_MAS_Renter_Address == null)
            {
                return HttpNotFound();
            }
            return View(cR_MAS_Renter_Address);
        }

        // POST: RenterAddress/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Mas_Renter_Address_Id_Code,CR_Mas_Renter_Address_Regions,CR_Mas_Renter_Address_City,CR_Mas_Renter_Address_Ar_District,CR_Mas_Renter_Address_En_District,CR_Mas_Renter_Address_Fr_District,CR_Mas_Renter_Address_Ar_Street,CR_Mas_Renter_Address_En_Street,CR_Mas_Renter_Address_Fr_Street,CR_Mas_Renter_Address_Building,CR_Mas_Renter_Address_Unit_No,CR_Mas_Renter_Address_Zip_Code,CR_Mas_Renter_Address_Additional_Numbers,CR_Mas_Renter_Address_UpDate_Post,CR_Mas_Renter_Address_Status,CR_Mas_Renter_Address_Reasons")] CR_MAS_Renter_Address cR_MAS_Renter_Address)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cR_MAS_Renter_Address).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cR_MAS_Renter_Address);
        }

        // GET: RenterAddress/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_MAS_Renter_Address cR_MAS_Renter_Address = db.CR_MAS_Renter_Address.Find(id);
            if (cR_MAS_Renter_Address == null)
            {
                return HttpNotFound();
            }
            return View(cR_MAS_Renter_Address);
        }

        // POST: RenterAddress/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_MAS_Renter_Address cR_MAS_Renter_Address = db.CR_MAS_Renter_Address.Find(id);
            db.CR_MAS_Renter_Address.Remove(cR_MAS_Renter_Address);
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
