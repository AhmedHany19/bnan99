using RentCar.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace RentCar.Controllers.CAS
{
    public class DocAlertController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        // GET: DocAlert
        public ActionResult Index()
        {
            var LessorCode = Session["LessorCode"].ToString();
            var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.Where(x => x.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == LessorCode && x.CR_Cas_Sup_Procedures_Type == "1");
            return View(mech);
        }

        public JsonResult CheckBoxChanged(CR_Cas_Sup_Follow_Up_Mechanism item)
        {
            var LessorCode = Session["LessorCode"].ToString();

            var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.FirstOrDefault(x => x.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == LessorCode &&
            x.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code == item.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code);

            mech.CR_Cas_Sup_Follow_Up_Mechanism_Activate_Service = item.CR_Cas_Sup_Follow_Up_Mechanism_Activate_Service;
            SaveTracing();
            db.Entry(mech).State = EntityState.Modified;
            db.SaveChanges();
            return Json("Success");
        }

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

        public JsonResult InputChanged(CR_Cas_Sup_Follow_Up_Mechanism item)
        {
            var LessorCode = Session["LessorCode"].ToString();
            var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.FirstOrDefault(x => x.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == LessorCode && x.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code == item.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code);
            mech.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire = item.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire;
            SaveTracing();
            db.Entry(mech).State = EntityState.Modified;
            db.SaveChanges();



            var doc = db.CR_Cas_Sup_Branch_Documentation.FirstOrDefault(x => x.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode &&
            x.CR_Cas_Sup_Branch_Documentation_Code == item.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code);
            DateTime currentDate = (DateTime)doc.CR_Cas_Sup_Branch_Documentation_End_Date;
            var nbr = Convert.ToDouble(mech.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire);
            var d = currentDate.AddDays(-nbr);
            doc.CR_Cas_Sup_Branch_Documentation_About_To_Expire = d;
            db.Entry(mech).State = EntityState.Modified;
            db.SaveChanges();
            return Json("Success");
        }

        public void SaveTracing()
        {
            /////////////////////////////////////Add Tracing//////////////////////////////////////

            CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var sector = "1";
            var ProcedureCode = "58";
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