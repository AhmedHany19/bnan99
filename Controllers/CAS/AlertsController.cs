using RentCar.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace RentCar.Controllers
{
    public class AlertsController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: Alerts
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
                    RedirectToAction("Login", "Account");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
            var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.Where(x => x.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == LessorCode && x.CR_Cas_Sup_Procedures_Type == "1")
                .Include(l => l.CR_Mas_Com_Lessor).Include(p => p.CR_Mas_Sup_Procedures);
            return View(mech);
        }

        public PartialViewResult AlertCompanyDoc()
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
            var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.Where(x => x.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == LessorCode && x.CR_Cas_Sup_Procedures_Type == "1")
                .Include(l => l.CR_Mas_Com_Lessor).Include(p => p.CR_Mas_Sup_Procedures);
            return PartialView(mech);
        }

        public PartialViewResult AlertCompanyContract()
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
            var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.Where(x => x.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == LessorCode && x.CR_Cas_Sup_Procedures_Type == "2");
            return PartialView(mech);
        }

        public PartialViewResult AlertCarDoc()
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
            var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.Where(x => x.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == LessorCode && (x.CR_Cas_Sup_Procedures_Type == "3" ||
            x.CR_Cas_Sup_Procedures_Type == "B"));
            return PartialView(mech);
        }

        public PartialViewResult AlertCarMain()
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
            var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.Where(x => x.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == LessorCode && x.CR_Cas_Sup_Procedures_Type == "4");
            return PartialView(mech);
        }

        public JsonResult UpdateCompanyDocs(List<CR_Cas_Sup_Follow_Up_Mechanism> item)
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

            foreach (CR_Cas_Sup_Follow_Up_Mechanism i in item)
            {
                var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.FirstOrDefault(m => m.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == i.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code.Trim()
                && m.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code == i.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code.Trim());
                if (mech != null)
                {
                    mech.CR_Cas_Sup_Follow_Up_Mechanism_Activate_Service = i.CR_Cas_Sup_Follow_Up_Mechanism_Activate_Service;
                    mech.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire = i.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire;
                    mech.CR_Cas_Sup_Follow_Up_Mechanism_Befor_KM = i.CR_Cas_Sup_Follow_Up_Mechanism_Befor_KM;
                    mech.CR_Cas_Sup_Follow_Up_Mechanism_Default_KM = i.CR_Cas_Sup_Follow_Up_Mechanism_Default_KM;

                    
                    db.Entry(mech).State = EntityState.Modified;
                    TempData["TempModel"] = "Saved";
                }
                var docs = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Code == i.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code);
                foreach(var d in docs)
                {
                    d.CR_Cas_Sup_Car_Doc_Mainten_About_To_Expire = d.CR_Cas_Sup_Car_Doc_Mainten_End_Date?.AddDays(-(double)mech.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire);
                    d.CR_Cas_Sup_Car_Doc_Mainten_Activation = i.CR_Cas_Sup_Follow_Up_Mechanism_Activate_Service;
                    db.Entry(d).State = EntityState.Modified;
                    
                }

                var branchDocs = db.CR_Cas_Sup_Branch_Documentation.Where(b=>b.CR_Cas_Sup_Branch_Documentation_Lessor_Code==LessorCode && b.CR_Cas_Sup_Branch_Documentation_Code==i.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code);
                foreach(var b in branchDocs)
                {
                    b.CR_Cas_Sup_Branch_Documentation_About_To_Expire = b.CR_Cas_Sup_Branch_Documentation_End_Date?.AddDays(-(double)mech.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire);
                    b.CR_Cas_Sup_Branch_Documentation_Activation = i.CR_Cas_Sup_Follow_Up_Mechanism_Activate_Service;
                    db.Entry(b).State = EntityState.Modified;
                   
                }

                var Contract = db.CR_Cas_Company_Contract.Where(c=>c.CR_Cas_Company_Contract_Lessor==LessorCode && c.CR_Cas_Company_Contract_Code==i.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code);
                foreach(var c in Contract)
                {
                    c.CR_Cas_Company_Contract_Activation = i.CR_Cas_Sup_Follow_Up_Mechanism_Activate_Service;
                    db.Entry(c).State = EntityState.Modified;
                    
                }
                db.SaveChanges();
            }

            SaveTracing();
            db.SaveChanges();
            return Json("Success");
        }












        public JsonResult CheckBoxChanged(CR_Cas_Sup_Follow_Up_Mechanism item)
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
            using (DbContextTransaction dbTran = db.Database.BeginTransaction())
            {
                try
                {
                    var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.FirstOrDefault(x => x.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == LessorCode &&
                    x.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code == item.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code);

                    mech.CR_Cas_Sup_Follow_Up_Mechanism_Activate_Service = item.CR_Cas_Sup_Follow_Up_Mechanism_Activate_Service;
                    SaveTracing();
                    db.Entry(mech).State = EntityState.Modified;
                    db.SaveChanges();
                    dbTran.Commit();
                }
                catch (DbEntityValidationException ex)
                {
                    dbTran.Rollback();
                    throw ex;
                }
            }

            return Json("Success");
        }

        public CR_Cas_Administrative_Procedures GetLastRecord(string ProcedureCode, string sector)
        {
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
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

        public JsonResult BeforeExpireChanged(CR_Cas_Sup_Follow_Up_Mechanism item)
        {
            using (DbContextTransaction dbTran = db.Database.BeginTransaction())
            {
                try
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
                    var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.FirstOrDefault(x => x.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == LessorCode
                    && x.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code == item.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code);


                    mech.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire = item.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire;

                    SaveTracing();
                    db.Entry(mech).State = EntityState.Modified;
                    db.SaveChanges();



                    var doc = db.CR_Cas_Sup_Branch_Documentation.FirstOrDefault(x => x.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode &&
                    x.CR_Cas_Sup_Branch_Documentation_Code == item.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code);
                    if (doc != null)
                    {
                        if (doc.CR_Cas_Sup_Branch_Documentation_End_Date != null)
                        {
                            DateTime currentDate = (DateTime)doc.CR_Cas_Sup_Branch_Documentation_End_Date;
                            var nbr = Convert.ToDouble(mech.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire);
                            var d = currentDate.AddDays(-nbr);
                            doc.CR_Cas_Sup_Branch_Documentation_About_To_Expire = d;
                        }
                    }


                    db.Entry(mech).State = EntityState.Modified;
                    db.SaveChanges();
                    dbTran.Commit();
                }
                catch (DbEntityValidationException ex)
                {
                    dbTran.Rollback();
                    throw ex;
                }
            }


            return Json("Success");
        }

        public JsonResult AfterExpireChanged(CR_Cas_Sup_Follow_Up_Mechanism item)
        {
            using (DbContextTransaction dbTran = db.Database.BeginTransaction())
            {
                try
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
                    var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.FirstOrDefault(x => x.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == LessorCode
                    && x.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code == item.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code);


                    mech.CR_Cas_Sup_Follow_Up_Mechanism_Default_KM = item.CR_Cas_Sup_Follow_Up_Mechanism_Default_KM;

                    SaveTracing();
                    db.Entry(mech).State = EntityState.Modified;
                    db.SaveChanges();
                    dbTran.Commit();
                }
                catch (DbEntityValidationException ex)
                {
                    dbTran.Rollback();
                    throw ex;
                }
            }

            return Json("Success");
        }


        public JsonResult BeforKmChanged(CR_Cas_Sup_Follow_Up_Mechanism item)
        {
            using (DbContextTransaction dbTran = db.Database.BeginTransaction())
            {
                try
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
                    var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.FirstOrDefault(x => x.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == LessorCode
                    && x.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code == item.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code);
                    mech.CR_Cas_Sup_Follow_Up_Mechanism_Befor_KM = item.CR_Cas_Sup_Follow_Up_Mechanism_Befor_KM;
                    SaveTracing();
                    db.Entry(mech).State = EntityState.Modified;
                    db.SaveChanges();
                    dbTran.Commit();
                }
                catch (DbEntityValidationException ex)
                {
                    dbTran.Rollback();
                    throw ex;
                }
            }
            return Json("Success");
        }

        public void SaveTracing()
        {
            /////////////////////////////////////Add Tracing//////////////////////////////////////

            CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var sector = "1";
            var ProcedureCode = "56";
            var autoInc = GetLastRecord(ProcedureCode, sector);
            var LessorCode = Session["LessorCode"].ToString();
            Ad.CR_Cas_Administrative_Procedures_No = y + "-" + sector + "-" + ProcedureCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
            Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
            Ad.CR_Cas_Administrative_Procedures_Year = y;
            Ad.CR_Cas_Administrative_Procedures_Sector = sector;
            Ad.CR_Cas_Administrative_Procedures_Code = ProcedureCode;
            Ad.CR_Cas_Administrative_Int_Procedures_Code = 56;
            Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
            Ad.CR_Cas_Administrative_Procedures_Targeted_Action = null;
            Ad.CR_Cas_Administrative_Procedures_User_Insert = Session["UserLogin"].ToString();
            Ad.CR_Cas_Administrative_Procedures_Type = "U";
            Ad.CR_Cas_Administrative_Procedures_Action = true;

            db.CR_Cas_Administrative_Procedures.Add(Ad);

            //////////////////////////////////////////////////////////////////////////////////////
        }















        // GET: Alerts/Create
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
            ViewBag.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo");
            ViewBag.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code = new SelectList(db.CR_Mas_Sup_Procedures, "CR_Mas_Sup_Procedures_Code", "CR_Mas_Sup_Procedures_Type");
            return View();
        }

        // POST: Alerts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code,CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code,CR_Cas_Sup_Follow_Up_Mechanism_Activate_Service,CR_Cas_Sup_Follow_Up_Mechanism_Befor_Credit_Limit,CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire,CR_Cas_Sup_Follow_Up_Mechanism_Befor_KM,CR_Cas_Sup_Follow_Up_Mechanism_After_Expire,CR_Cas_Sup_Follow_Up_Mechanism_Default_KM,CR_Cas_Sup_Procedures_Type")] CR_Cas_Sup_Follow_Up_Mechanism cR_Cas_Sup_Follow_Up_Mechanism)
        {
            if (ModelState.IsValid)
            {
                db.CR_Cas_Sup_Follow_Up_Mechanism.Add(cR_Cas_Sup_Follow_Up_Mechanism);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Sup_Follow_Up_Mechanism.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code);
            ViewBag.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code = new SelectList(db.CR_Mas_Sup_Procedures, "CR_Mas_Sup_Procedures_Code", "CR_Mas_Sup_Procedures_Type", cR_Cas_Sup_Follow_Up_Mechanism.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code);
            return View(cR_Cas_Sup_Follow_Up_Mechanism);
        }

        // GET: Alerts/Edit/5
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
            CR_Cas_Sup_Follow_Up_Mechanism cR_Cas_Sup_Follow_Up_Mechanism = db.CR_Cas_Sup_Follow_Up_Mechanism.Find(id);
            if (cR_Cas_Sup_Follow_Up_Mechanism == null)
            {
                return HttpNotFound();
            }
            ViewBag.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Sup_Follow_Up_Mechanism.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code);
            ViewBag.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code = new SelectList(db.CR_Mas_Sup_Procedures, "CR_Mas_Sup_Procedures_Code", "CR_Mas_Sup_Procedures_Type", cR_Cas_Sup_Follow_Up_Mechanism.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code);
            return View(cR_Cas_Sup_Follow_Up_Mechanism);
        }

        // POST: Alerts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code,CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code,CR_Cas_Sup_Follow_Up_Mechanism_Activate_Service,CR_Cas_Sup_Follow_Up_Mechanism_Befor_Credit_Limit,CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire,CR_Cas_Sup_Follow_Up_Mechanism_Befor_KM,CR_Cas_Sup_Follow_Up_Mechanism_After_Expire,CR_Cas_Sup_Follow_Up_Mechanism_Default_KM,CR_Cas_Sup_Procedures_Type")] CR_Cas_Sup_Follow_Up_Mechanism cR_Cas_Sup_Follow_Up_Mechanism)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cR_Cas_Sup_Follow_Up_Mechanism).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Sup_Follow_Up_Mechanism.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code);
            ViewBag.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code = new SelectList(db.CR_Mas_Sup_Procedures, "CR_Mas_Sup_Procedures_Code", "CR_Mas_Sup_Procedures_Type", cR_Cas_Sup_Follow_Up_Mechanism.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code);
            return View(cR_Cas_Sup_Follow_Up_Mechanism);
        }

        // GET: Alerts/Delete/5


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
