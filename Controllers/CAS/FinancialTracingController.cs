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
    public class FinancialTracingController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: FinancialTracing
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

            return View();
        }


        private List<TracingMD> GetTracing(string LessorCode, DateTime sd, DateTime ed, string type)
        {
            List<TracingMD> L = new List<TracingMD>();
            IOrderedQueryable<CR_Cas_Administrative_Procedures> tracing = null;
            var d = DateTime.Now.AddDays(-200);
            if (type == "Date")
            {
                tracing = db.CR_Cas_Administrative_Procedures.Where(t => t.CR_Cas_Administrative_Procedures_Lessor == LessorCode && t.CR_Cas_Administrative_Procedures_Sector == "1"
                 && t.CR_Cas_Administrative_Procedures_Date > d && t.CR_Cas_Administrative_Int_Procedures_Code >= 60 && t.CR_Cas_Administrative_Int_Procedures_Code <= 69 &&
                 t.CR_Cas_Administrative_Procedures_Date >= sd && t.CR_Cas_Administrative_Procedures_Date <= ed)
                 .OrderByDescending(x => x.CR_Cas_Administrative_Procedures_Date)
                 .ThenByDescending(t => t.CR_Cas_Administrative_Procedures_Time);
            }
            else
            {
                tracing = db.CR_Cas_Administrative_Procedures.Where(t => t.CR_Cas_Administrative_Procedures_Lessor == LessorCode && t.CR_Cas_Administrative_Procedures_Sector == "1"
                && t.CR_Cas_Administrative_Procedures_Date > d && t.CR_Cas_Administrative_Int_Procedures_Code >= 60 && t.CR_Cas_Administrative_Int_Procedures_Code <= 69)
                .OrderByDescending(x => x.CR_Cas_Administrative_Procedures_Date)
                .ThenByDescending(t => t.CR_Cas_Administrative_Procedures_Time);
            }

            foreach (var tr in tracing)
            {
                TracingMD T = new TracingMD();
                T.CR_Cas_Administrative_Procedures_Action = (bool)tr.CR_Cas_Administrative_Procedures_Action;
                T.CR_Cas_Administrative_Procedures_Code = tr.CR_Cas_Administrative_Procedures_Code;
                T.CR_Cas_Administrative_Int_Procedures_Code = (int)tr.CR_Cas_Administrative_Int_Procedures_Code;
                var p = db.CR_Mas_Sup_Procedures.FirstOrDefault(pr => pr.CR_Mas_Sup_Procedures_Code == T.CR_Cas_Administrative_Procedures_Code);

                T.CR_Cas_Administrative_Procedures_Com_Supporting = tr.CR_Cas_Administrative_Procedures_Com_Supporting;
                T.CR_Cas_Administrative_Procedures_Date = tr.CR_Cas_Administrative_Procedures_Date.GetValueOrDefault();
                T.CR_Cas_Administrative_Procedures_Doc_Date = tr.CR_Cas_Administrative_Procedures_Doc_Date.GetValueOrDefault();
                T.CR_Cas_Administrative_Procedures_Doc_End_Date = tr.CR_Cas_Administrative_Procedures_Doc_End_Date.GetValueOrDefault();
                T.CR_Cas_Administrative_Procedures_Doc_No = tr.CR_Cas_Administrative_Procedures_Doc_No;
                T.CR_Cas_Administrative_Procedures_Doc_Start_Date = tr.CR_Cas_Administrative_Procedures_Doc_Start_Date.GetValueOrDefault();
                T.CR_Cas_Administrative_Procedures_From_Branch = tr.CR_Cas_Administrative_Procedures_From_Branch;
                T.CR_Cas_Administrative_Procedures_Lessor = tr.CR_Cas_Administrative_Procedures_Lessor;
                T.CR_Cas_Administrative_Procedures_No = tr.CR_Cas_Administrative_Procedures_No;
                T.CR_Cas_Administrative_Procedures_Reasons = tr.CR_Cas_Administrative_Procedures_Reasons;
                T.CR_Cas_Administrative_Procedures_Sector = tr.CR_Cas_Administrative_Procedures_Sector;
                T.CR_Cas_Administrative_Procedures_Targeted_Action = tr.CR_Cas_Administrative_Procedures_Targeted_Action;
                T.CR_Cas_Administrative_Procedures_Time = tr.CR_Cas_Administrative_Procedures_Time.GetValueOrDefault();
                T.CR_Cas_Administrative_Procedures_To_Branch = tr.CR_Cas_Administrative_Procedures_To_Branch;
                T.CR_Cas_Administrative_Procedures_Type = tr.CR_Cas_Administrative_Procedures_Type;


                if (int.Parse(tr.CR_Cas_Administrative_Procedures_Code) == 60)
                {

                    if (tr.CR_Cas_Administrative_Procedures_Targeted_Action != null && tr.CR_Cas_Administrative_Procedures_Targeted_Action != "")
                    {
                        T.ProcedureName = p.CR_Mas_Sup_Procedures_Ar_Name;
                    }
                }
                else if (int.Parse(tr.CR_Cas_Administrative_Procedures_Code) == 61)
                {

                    if (tr.CR_Cas_Administrative_Procedures_Targeted_Action != null && tr.CR_Cas_Administrative_Procedures_Targeted_Action != "")
                    {
                        T.ProcedureName = p.CR_Mas_Sup_Procedures_Ar_Name;
                    }
                }
                else if (int.Parse(tr.CR_Cas_Administrative_Procedures_Code) == 62)
                {

                    var UserName = "";
                    var User = db.CR_Cas_User_Information.FirstOrDefault(b => b.CR_Cas_User_Information_Id == tr.CR_Cas_Administrative_Procedures_Targeted_Action && b.CR_Cas_User_Information_Lessor_Code == LessorCode);
                    if (User != null)
                    {
                        UserName = User.CR_Cas_User_Information_Ar_Name;
                    }

                    if (UserName != "")
                    {
                        if (UserName.Length >= 20)
                        {
                            T.ProcedureName = p.CR_Mas_Sup_Procedures_Ar_Name + " " + " ( " + UserName.Substring(0, 23) + " )";
                        }
                        else
                        {
                            T.ProcedureName = p.CR_Mas_Sup_Procedures_Ar_Name + " " + " ( " + UserName + " )";
                        }

                    }
                }
                else if (int.Parse(tr.CR_Cas_Administrative_Procedures_Code) == 63)
                {
                    var RenterName = "";
                    var Renter = db.CR_Cas_Renter_Lessor.FirstOrDefault(b => b.CR_Cas_Renter_Lessor_Id == tr.CR_Cas_Administrative_Procedures_Targeted_Action && b.CR_Cas_Renter_Lessor_Code == LessorCode);
                    if (Renter != null)
                    {
                        RenterName = Renter.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Ar_Name;
                    }

                    if (RenterName != "")
                    {
                        if (RenterName.Length >= 20)
                        {
                            T.ProcedureName = p.CR_Mas_Sup_Procedures_Ar_Name + " " + " ( " + RenterName.Substring(0, 23) + " )";
                        }
                        else
                        {
                            T.ProcedureName = p.CR_Mas_Sup_Procedures_Ar_Name + " " + " ( " + RenterName + " )";
                        }

                    }
                }
                else if (int.Parse(tr.CR_Cas_Administrative_Procedures_Code) == 64)
                {
                    var RenterName = "";
                    var Renter = db.CR_Cas_Renter_Lessor.FirstOrDefault(b => b.CR_Cas_Renter_Lessor_Id == tr.CR_Cas_Administrative_Procedures_Targeted_Action && b.CR_Cas_Renter_Lessor_Code == LessorCode);
                    if (Renter != null)
                    {
                        RenterName = Renter.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Ar_Name;
                    }

                    if (RenterName != "")
                    {
                        if (RenterName.Length >= 20)
                        {
                            T.ProcedureName = p.CR_Mas_Sup_Procedures_Ar_Name + " " + " ( " + RenterName.Substring(0, 23) + " )";
                        }
                        else
                        {
                            T.ProcedureName = p.CR_Mas_Sup_Procedures_Ar_Name + " " + " ( " + RenterName + " )";
                        }

                    }
                }
                else if (int.Parse(tr.CR_Cas_Administrative_Procedures_Code) == 65)
                {

                    if (tr.CR_Cas_Administrative_Procedures_Targeted_Action != null && tr.CR_Cas_Administrative_Procedures_Targeted_Action != "")
                    {
                        T.ProcedureName = p.CR_Mas_Sup_Procedures_Ar_Name;
                    }
                }
                else if (int.Parse(tr.CR_Cas_Administrative_Procedures_Code) == 66)
                {

                    if (tr.CR_Cas_Administrative_Procedures_Targeted_Action != null && tr.CR_Cas_Administrative_Procedures_Targeted_Action != "")
                    {
                        T.ProcedureName = p.CR_Mas_Sup_Procedures_Ar_Name;
                    }
                }
                else if (int.Parse(tr.CR_Cas_Administrative_Procedures_Code) == 67)
                {

                    if (tr.CR_Cas_Administrative_Procedures_Targeted_Action != null && tr.CR_Cas_Administrative_Procedures_Targeted_Action != "")
                    {
                        T.ProcedureName = p.CR_Mas_Sup_Procedures_Ar_Name;
                    }
                }
                else if (int.Parse(tr.CR_Cas_Administrative_Procedures_Code) == 68)
                {

                    if (tr.CR_Cas_Administrative_Procedures_Targeted_Action != null && tr.CR_Cas_Administrative_Procedures_Targeted_Action != "")
                    {
                        T.ProcedureName = p.CR_Mas_Sup_Procedures_Ar_Name;
                    }
                }
                else if (int.Parse(tr.CR_Cas_Administrative_Procedures_Code) == 69)
                {

                    var SalesPointName = "";
                    var SalesPoint = db.CR_Cas_Sup_SalesPoint.FirstOrDefault(b => b.CR_Cas_Sup_SalesPoint_Code == tr.CR_Cas_Administrative_Procedures_Targeted_Action && b.CR_Cas_Sup_SalesPoint_Com_Code == LessorCode);
                    if (SalesPoint != null)
                    {
                        SalesPointName = SalesPoint.CR_Cas_Sup_SalesPoint_Ar_Name;
                    }

                    if (SalesPointName != "")
                    {
                        if (SalesPointName.Length >= 24)
                        {
                            T.ProcedureName = p.CR_Mas_Sup_Procedures_Ar_Name + " " + " ( " + SalesPointName.Substring(0, 23) + " )";
                        }
                        else
                        {
                            T.ProcedureName = p.CR_Mas_Sup_Procedures_Ar_Name + " " + " ( " + SalesPointName + " )";
                        }

                    }
                }


                if (T.CR_Cas_Administrative_Procedures_Type == "I")
                {

                    T.Type = "إضافة";


                }
                else if (T.CR_Cas_Administrative_Procedures_Type == "M")
                {

                    T.Type = "رسالة";


                }
                else if (T.CR_Cas_Administrative_Procedures_Type == "W")
                {

                    T.Type = "إنتظار";


                }
                else if (T.CR_Cas_Administrative_Procedures_Type == "U")
                {

                    T.Type = "تعديل";


                }
                else if (T.CR_Cas_Administrative_Procedures_Type == "D")
                {

                    T.Type = "حذف";


                }
                else if (T.CR_Cas_Administrative_Procedures_Type == "H")
                {

                    T.Type = "إيقاف";


                }
                else if (T.CR_Cas_Administrative_Procedures_Type == "A")
                {

                    T.Type = "تنشيط";


                }
                else if (T.CR_Cas_Administrative_Procedures_Type == "O")
                {

                    T.Type = "عرض للبيع";


                }
                else if (T.CR_Cas_Administrative_Procedures_Type == "Q")
                {
                    T.Type = "قبول";
                }
                else if (T.CR_Cas_Administrative_Procedures_Type == "Z")
                {
                    T.Type = "رفض";
                }
                T.CR_Cas_Administrative_Procedures_User_Insert = tr.CR_Cas_Administrative_Procedures_User_Insert;
                var user = db.CR_Cas_User_Information.FirstOrDefault(u => u.CR_Cas_User_Information_Id == T.CR_Cas_Administrative_Procedures_User_Insert);
                if (user != null)
                {
                    T.UserUpdate = user.CR_Cas_User_Information_Ar_Name;
                    T.CR_Cas_Administrative_Procedures_Value = tr.CR_Cas_Administrative_Procedures_Value.GetValueOrDefault();
                    T.CR_Cas_Administrative_Procedures_Year = tr.CR_Cas_Administrative_Procedures_Year;
                }


                L.Add(T);
            }

            return L;
        }

        public PartialViewResult Table(string StartDate, string EndDate, string type)
        {
            var LessorCode = "";
            var UserLogin = "";
            List<TracingMD> L = null;
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                DateTime sd = Convert.ToDateTime(StartDate);
                DateTime ed = Convert.ToDateTime(EndDate);

                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null)
                {
                    RedirectToAction("Account", "Login");
                }

                L = GetTracing(LessorCode, sd, ed,type);
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }



            return PartialView(L.ToList());
        }

        // GET: FinancialTracing/Details/5
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

        // GET: FinancialTracing/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FinancialTracing/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Administrative_Procedures_No,CR_Cas_Administrative_Procedures_Date,CR_Cas_Administrative_Procedures_Time,CR_Cas_Administrative_Procedures_Year,CR_Cas_Administrative_Procedures_Sector,CR_Cas_Administrative_Procedures_Code,CR_Cas_Administrative_Int_Procedures_Code,CR_Cas_Administrative_Procedures_Lessor,CR_Cas_Administrative_Procedures_Targeted_Action,CR_Cas_Administrative_Procedures_Com_Supporting,CR_Cas_Administrative_Procedures_Value,CR_Cas_Administrative_Procedures_Doc_No,CR_Cas_Administrative_Procedures_Doc_Date,CR_Cas_Administrative_Procedures_Doc_Start_Date,CR_Cas_Administrative_Procedures_Doc_End_Date,CR_Cas_Administrative_Procedures_From_Branch,CR_Cas_Administrative_Procedures_To_Branch,CR_Cas_Administrative_Procedures_Action,CR_Cas_Administrative_Procedures_User_Insert,CR_Cas_Administrative_Procedures_Type,CR_Cas_Administrative_Procedures_Reasons")] CR_Cas_Administrative_Procedures cR_Cas_Administrative_Procedures)
        {
            if (ModelState.IsValid)
            {
                db.CR_Cas_Administrative_Procedures.Add(cR_Cas_Administrative_Procedures);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cR_Cas_Administrative_Procedures);
        }

        // GET: FinancialTracing/Edit/5
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
            return View(cR_Cas_Administrative_Procedures);
        }

        // POST: FinancialTracing/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Administrative_Procedures_No,CR_Cas_Administrative_Procedures_Date,CR_Cas_Administrative_Procedures_Time,CR_Cas_Administrative_Procedures_Year,CR_Cas_Administrative_Procedures_Sector,CR_Cas_Administrative_Procedures_Code,CR_Cas_Administrative_Int_Procedures_Code,CR_Cas_Administrative_Procedures_Lessor,CR_Cas_Administrative_Procedures_Targeted_Action,CR_Cas_Administrative_Procedures_Com_Supporting,CR_Cas_Administrative_Procedures_Value,CR_Cas_Administrative_Procedures_Doc_No,CR_Cas_Administrative_Procedures_Doc_Date,CR_Cas_Administrative_Procedures_Doc_Start_Date,CR_Cas_Administrative_Procedures_Doc_End_Date,CR_Cas_Administrative_Procedures_From_Branch,CR_Cas_Administrative_Procedures_To_Branch,CR_Cas_Administrative_Procedures_Action,CR_Cas_Administrative_Procedures_User_Insert,CR_Cas_Administrative_Procedures_Type,CR_Cas_Administrative_Procedures_Reasons")] CR_Cas_Administrative_Procedures cR_Cas_Administrative_Procedures)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cR_Cas_Administrative_Procedures).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cR_Cas_Administrative_Procedures);
        }

        // GET: FinancialTracing/Delete/5
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

        // POST: FinancialTracing/Delete/5
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
