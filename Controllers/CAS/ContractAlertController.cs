using RentCar.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace RentCar.Controllers.CAS
{
    public class ContractAlertController : Controller
    {
        // GET: ContractAlert
        private RentCarDBEntities db = new RentCarDBEntities();
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
            var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.Where(x => x.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == LessorCode && x.CR_Cas_Sup_Procedures_Type == "2");
            return View(mech);
        }

        public JsonResult CheckBoxChanged(CR_Cas_Sup_Follow_Up_Mechanism item)
        {
            var LessorCode = Session["LessorCode"].ToString();
            using (DbContextTransaction dbTran = db.Database.BeginTransaction())
            {
                try
                {
                    var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.FirstOrDefault(x => x.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == LessorCode && x.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code == item.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code);
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

        public JsonResult InputChanged(CR_Cas_Sup_Follow_Up_Mechanism item)
        {
            var LessorCode = Session["LessorCode"].ToString();
            using (DbContextTransaction dbTran = db.Database.BeginTransaction())
            {
                try
                {
                    var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.FirstOrDefault(x => x.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == LessorCode && x.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code == item.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code);
                    mech.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire = item.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire;
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

        public JsonResult CreditChanged(CR_Cas_Sup_Follow_Up_Mechanism item)
        {
            var LessorCode = Session["LessorCode"].ToString();
            using (DbContextTransaction dbTran = db.Database.BeginTransaction())
            {
                try
                {
                    var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.FirstOrDefault(x => x.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == LessorCode && x.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code == item.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code);
                    mech.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Credit_Limit = item.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Credit_Limit;
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

        //public CR_Cas_Administrative_Procedures GetLastRecord()
        //{
        //    var Lrecord = db.CR_Cas_Administrative_Procedures.Max(Lr => Lr.CR_Cas_Administrative_Procedures_No.Substring(Lr.CR_Cas_Administrative_Procedures_No.Length - 7, 7));
        //    CR_Cas_Administrative_Procedures T = new CR_Cas_Administrative_Procedures();
        //    if (Lrecord != null)
        //    {
        //        Int64 val = Int64.Parse(Lrecord) + 1;
        //        T.CR_Cas_Administrative_Procedures_No = val.ToString("0000000");
        //    }
        //    else
        //    {
        //        T.CR_Cas_Administrative_Procedures_No = "0000001";
        //    }
        //    return T;
        //}


        public CR_Cas_Administrative_Procedures GetLastRecord(string ProcedureCode, string sector)
        {
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var LessorCode = Session["LessorCode"].ToString();
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

        public void SaveTracing()
        {
            /////////////////////////////////////Add Tracing//////////////////////////////////////

            CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var sector = "1";
            var ProcedureCode = "59";
            var autoInc = GetLastRecord(ProcedureCode, sector);
            var LessorCode = Session["LessorCode"].ToString();
            Ad.CR_Cas_Administrative_Procedures_No = y + "-" + sector + "-" + ProcedureCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
            Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
            Ad.CR_Cas_Administrative_Procedures_Year = y;
            Ad.CR_Cas_Administrative_Procedures_Sector = sector;
            Ad.CR_Cas_Administrative_Procedures_Code = ProcedureCode;
            Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
            Ad.CR_Cas_Administrative_Procedures_Targeted_Action = LessorCode;
            Ad.CR_Cas_Administrative_Procedures_User_Insert = Session["UserLogin"].ToString();
            Ad.CR_Cas_Administrative_Procedures_Type = "I";
            Ad.CR_Cas_Administrative_Procedures_Action = true;

            db.CR_Cas_Administrative_Procedures.Add(Ad);

            //////////////////////////////////////////////////////////////////////////////////////
        }



    }
}