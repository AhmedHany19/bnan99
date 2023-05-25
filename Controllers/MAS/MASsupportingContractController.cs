using RentCar.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace RentCar.Controllers
{
    public class MASsupportingContractController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        public static string TaskCode;
        // GET: MASsupportingContract
        public ActionResult Index()
        {

            TaskCode = "1103";
            Session["POS"] = "1103";
            if (Session["ST_1103_unhold"].ToString() != "true" || Session["ST_1103_hold"].ToString() != "true" && Session["ST_1103_undelete"].ToString() != "true" || Session["ST_1103_delete"].ToString() != "true")
            {
                var cR_Mas_Basic_Contract = db.CR_Mas_Basic_Contract.Where(x => x.CR_Mas_Basic_Contract_Status == "A" &&
                x.CR_Mas_Basic_Contract_Lessor == "1000" && x.CR_Mas_Basic_Contract_Code == "21").Include(c => c.CR_Mas_Com_Lessor).Include(c => c.CR_Mas_Sup_Sector);
                return View(cR_Mas_Basic_Contract);
            }

            else
                if (Session["ST_1103_unhold"].ToString() != "true" || Session["ST_1103_hold"].ToString() != "true")
            {
                var c = (List<CR_Mas_Basic_Contract>)db.CR_Mas_Basic_Contract.Where(x => x.CR_Mas_Basic_Contract_Status != "H" &&
                x.CR_Mas_Basic_Contract_Lessor == "1000" && x.CR_Mas_Basic_Contract_Code == "21").Include(x => x.CR_Mas_Com_Lessor).Include(x => x.CR_Mas_Sup_Sector);
                return View(c);
            }
            else if (Session["ST_1103_undelete"].ToString() != "true" || Session["ST_1103_delete"].ToString() != "true")
            {
                var c = db.CR_Mas_Basic_Contract.Where(x => x.CR_Mas_Basic_Contract_Status != "D" &&
                x.CR_Mas_Basic_Contract_Lessor == "1000" && x.CR_Mas_Basic_Contract_Code == "21").Include(x => x.CR_Mas_Com_Lessor).Include(x => x.CR_Mas_Sup_Sector);
                return View(c);
            }
            else
            {
                var c = db.CR_Mas_Basic_Contract.Where(x => x.CR_Mas_Basic_Contract_Lessor == "1000" && x.CR_Mas_Basic_Contract_Code == "21").Include(x => x.CR_Mas_Com_Lessor).Include(x => x.CR_Mas_Sup_Sector);
                return View(c);
            }
        }


        public CR_Cas_Administrative_Procedures GetLastRecord()
        {
            var Lrecord = db.CR_Cas_Administrative_Procedures.Max(Lr => Lr.CR_Cas_Administrative_Procedures_No.Substring(Lr.CR_Cas_Administrative_Procedures_No.Length - 7, 7));
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



        // GET: MASsupportingContract/Create
        public ActionResult Create()
        {
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var sector = "1";
            var autoInc = GetLastRecord();
            var LessorCode = Session["LessorCode"].ToString();
            CR_Mas_Basic_Contract BC = new CR_Mas_Basic_Contract();
            BC.CR_Mas_Basic_Contract_No = y + "-" + sector + "-" + "21" + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
            BC.CR_Mas_Basic_Contract_Code = "21";
            BC.CR_Mas_Basic_Contract_Year = y;
            BC.CR_Mas_Basic_Contract_Status = "A";

            ViewBag.DocDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            ViewBag.StartDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            ViewBag.EndDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);

            ViewBag.CR_Mas_Basic_Contract_Com_Code = new SelectList(db.CR_Mas_Com_Supporting, "CR_Mas_Com_Supporting_Code", "CR_Mas_Com_Supporting_Ar_Short_Name");
            return View(BC);
        }

        // POST: MASsupportingContract/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Mas_Basic_Contract_No,CR_Mas_Basic_Contract_Year,CR_Mas_Basic_Contract_Sector," +
            "CR_Mas_Basic_Contract_Code,CR_Mas_Basic_Contract_Lessor,CR_Mas_Basic_Contract_Com_Code,CR_Mas_Basic_Contract_Date," +
            "CR_Mas_Basic_Contract_Start_Date,CR_Mas_Basic_Contract_End_Date,CR_Mas_Basic_Contract_Annual_Fees" +
            ",CR_Mas_Basic_Contract_Service_Fees,CR_Mas_Basic_Contract_Discount_Rate,CR_Mas_Basic_Contract_Tax_Rate," +
            "CR_Mas_Basic_Contract_Tamm_User_Id,CR_Mas_Basic_Contract_Tamm_User_PassWord,CR_Mas_Basic_Contract_Status,CR_Mas_Basic_Contract_Reasons")]
        CR_Mas_Basic_Contract cR_Mas_Basic_Contract)
        {
            var CheckExistContract = db.CR_Mas_Basic_Contract.FirstOrDefault(x => x.CR_Mas_Basic_Contract_Com_Code == cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Com_Code
            && x.CR_Mas_Basic_Contract_Lessor == cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Lessor && x.CR_Mas_Basic_Contract_Status != "D");


            if (ModelState.IsValid)
            {
                if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date <= DateTime.Now && cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date >= cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date
                && cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date >= cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date && cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date != null &&
                cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date != null && cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date != null && CheckExistContract == null)
                {
                    ///////////////////////////////Tracing//////////////////////////////////////
                    CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
                    DateTime year = DateTime.Now;
                    var y = year.ToString("yy");
                    var sector = "1";
                    var autoInc = GetLastRecord();
                    var LessorCode = Session["LessorCode"].ToString();
                    Ad.CR_Cas_Administrative_Procedures_No = y + "-" + sector + "-" + "21" + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
                    Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
                    string currentTime = DateTime.Now.ToString("HH:mm:ss");
                    Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
                    Ad.CR_Cas_Administrative_Procedures_Year = y;
                    Ad.CR_Cas_Administrative_Procedures_Sector = sector;
                    Ad.CR_Cas_Administrative_Procedures_Code = "21";
                    Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
                    Ad.CR_Cas_Administrative_Procedures_Targeted_Action = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Lessor;
                    Ad.CR_Cas_Administrative_Procedures_User_Insert = Session["UserLogin"].ToString();
                    Ad.CR_Cas_Administrative_Procedures_Type = "I";
                    Ad.CR_Cas_Administrative_Procedures_Action = true;
                    Ad.CR_Cas_Administrative_Procedures_Doc_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date;
                    Ad.CR_Cas_Administrative_Procedures_Doc_Start_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date;
                    Ad.CR_Cas_Administrative_Procedures_Doc_End_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date;
                    Ad.CR_Cas_Administrative_Procedures_Doc_No = "";
                    db.CR_Cas_Administrative_Procedures.Add(Ad);

                    /////////////////////////////////////////////Save contract////////////////////////////////
                    //cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date;
                    //cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date;
                    //cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date;
                    //cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_No = y + "-" + sector + "-" + "20" + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
                    //cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Year = y;
                    cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Sector = "1";//////////////////////////check/////////////////////
                    //cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Code = "20";
                    cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Lessor = LessorCode;
                    //cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Com_Code = cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Com_Code;//////////////////////////Check////////////////////////
                    //cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status = "A";

                    db.CR_Mas_Basic_Contract.Add(cR_Mas_Basic_Contract);
                    /////////////////////////////////////////////////////////////////////////////////////////////
                    ///


                    db.SaveChanges();
                    TempData["TempModel"] = "تم الحفظ بنجاح";
                    return RedirectToAction("Create", "MASsupportingContract");
                }
                else
                {
                    if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date < DateTime.Now)
                    {
                        ViewBag.DocEndDateError = "صلاحية الوثيقة منتهية";
                    }

                    if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date > DateTime.Now)
                    {
                        ViewBag.DocDateError = "الرجاء التأكد من تاريخ المستند";
                    }
                    if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date < cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date)
                    {
                        ViewBag.DocStartDateError = "تأكد من التاريخ";
                    }

                    if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date == null)
                    {
                        ViewBag.DocDateError = "الرجاء إختيار التاريخ";
                    }

                    if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date == null)
                    {
                        ViewBag.DocStartDateError = "الرجاء إختيار التاريخ";
                    }

                    if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date == null)
                    {
                        ViewBag.DocEndDateError = "الرجاء إختيار التاريخ";
                    }
                    if (CheckExistContract != null)
                    {
                        TempData["TempModel"] = "هنالك عقد ساري المفعول لهذه الشركة";
                    }

                    ViewBag.EndDate = string.Format("{0:yyyy-MM-dd}", cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date);
                    ViewBag.StartDate = string.Format("{0:yyyy-MM-dd}", cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date);
                    ViewBag.DocDate = string.Format("{0:yyyy-MM-dd}", cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date);
                }


            }


            ViewBag.CR_Mas_Basic_Contract_Com_Code = new SelectList(db.CR_Mas_Com_Supporting, "CR_Mas_Com_Supporting_Code", "CR_Mas_Com_Supporting_Ar_Short_Name");
            return View(cR_Mas_Basic_Contract);
        }

        // GET: MASsupportingContract/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Basic_Contract cR_Mas_Basic_Contract = db.CR_Mas_Basic_Contract.Find(id);
            if (cR_Mas_Basic_Contract == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date == null)
                {
                    ViewBag.DocDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                }
                else
                {
                    ViewBag.DocDate = string.Format("{0:yyyy-MM-dd}", cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Date);
                }

                if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date == null)
                {
                    ViewBag.StartDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                }
                else
                {
                    ViewBag.StartDate = string.Format("{0:yyyy-MM-dd}", cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Start_Date);
                }

                if (cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date == null)
                {
                    ViewBag.EndDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                }
                else
                {
                    ViewBag.EndDate = string.Format("{0:yyyy-MM-dd}", cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_End_Date);
                }
            }
            ViewBag.stat = "حذف";
            ViewBag.CR_Mas_Basic_Contract_Com_Code = new SelectList(db.CR_Mas_Com_Supporting, "CR_Mas_Com_Supporting_Code", "CR_Mas_Com_Supporting_Ar_Short_Name", cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Com_Code);
            return View(cR_Mas_Basic_Contract);
        }

        // POST: MASsupportingContract/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Mas_Basic_Contract_No,CR_Mas_Basic_Contract_Year,CR_Mas_Basic_Contract_Sector," +
            "CR_Mas_Basic_Contract_Code,CR_Mas_Basic_Contract_Lessor,CR_Mas_Basic_Contract_Com_Code,CR_Mas_Basic_Contract_Date," +
            "CR_Mas_Basic_Contract_Start_Date,CR_Mas_Basic_Contract_End_Date,CR_Mas_Basic_Contract_Annual_Fees,CR_Mas_Basic_Contract_Service_Fees," +
            "CR_Mas_Basic_Contract_Discount_Rate,CR_Mas_Basic_Contract_Tax_Rate,CR_Mas_Basic_Contract_Tamm_User_Id,CR_Mas_Basic_Contract_Tamm_User_PassWord," +
            "CR_Mas_Basic_Contract_Status,CR_Mas_Basic_Contract_Reasons")] CR_Mas_Basic_Contract cR_Mas_Basic_Contract, string delete)
        {
            if (!string.IsNullOrEmpty(delete))
            {
                if (ModelState.IsValid)
                {
                    cR_Mas_Basic_Contract.CR_Mas_Basic_Contract_Status = "D";
                    db.Entry(cR_Mas_Basic_Contract).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.CR_Mas_Basic_Contract_Com_Code = new SelectList(db.CR_Mas_Com_Supporting, "CR_Mas_Com_Supporting_Code", "CR_Mas_Com_Supporting_Ar_Short_Name");
            return View(cR_Mas_Basic_Contract);
        }

        // GET: MASsupportingContract/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Basic_Contract cR_Mas_Basic_Contract = db.CR_Mas_Basic_Contract.Find(id);
            if (cR_Mas_Basic_Contract == null)
            {
                return HttpNotFound();
            }
            return View(cR_Mas_Basic_Contract);
        }

        // POST: MASsupportingContract/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Mas_Basic_Contract cR_Mas_Basic_Contract = db.CR_Mas_Basic_Contract.Find(id);
            db.CR_Mas_Basic_Contract.Remove(cR_Mas_Basic_Contract);
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
