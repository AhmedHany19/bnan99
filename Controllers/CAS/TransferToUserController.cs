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
    public class TransferToUserController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: TransferFromUser
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
                    RedirectToAction("Account", "Login");
                }
            }
            catch (Exception ex)
            {
                RedirectToAction("Login", "Account");
            }
            var Users = db.CR_Cas_User_Information.Where(u=>u.CR_Cas_User_Information_Lessor_Code == LessorCode && u.CR_Cas_User_Information_Id!=UserLogin);
            return View(Users.ToList());
        }

        public PartialViewResult PartialView(string type)
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
                    RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }

            IQueryable<CR_Cas_User_Information> cR_Cas_User_Information = null;
            List<CR_Cas_User_Information> ListUsers = new List<CR_Cas_User_Information>();
            
                cR_Cas_User_Information = db.CR_Cas_User_Information.Where(l => l.CR_Cas_User_Information_Lessor_Code == LessorCode && l.CR_Cas_User_Information_Id != UserLogin && l.CR_Cas_User_Information_Status=="A");
                if (cR_Cas_User_Information != null)
                {
                    foreach (var item in cR_Cas_User_Information)
                    {
                        var proc = db.CR_Cas_Administrative_Procedures.FirstOrDefault(a => a.CR_Cas_Administrative_Procedures_Lessor == LessorCode &&
                         a.CR_Cas_Administrative_Procedures_Code == "62" && a.CR_Cas_Administrative_Procedures_Type == "I" && a.CR_Cas_Administrative_Procedures_Action == false
                         && a.CR_Cas_Administrative_Procedures_Targeted_Action == item.CR_Cas_User_Information_Id);
                        if (proc == null)
                        {
                            CR_Cas_User_Information user = new CR_Cas_User_Information();
                            user.CR_Cas_User_Branch_Validity = item.CR_Cas_User_Branch_Validity;
                            user.CR_Cas_User_Information_Ar_Name = item.CR_Cas_User_Information_Ar_Name;
                            user.CR_Cas_User_Information_Auth_Branch = item.CR_Cas_User_Information_Auth_Branch;
                            user.CR_Cas_User_Information_Auth_Owners = item.CR_Cas_User_Information_Auth_Owners;
                            user.CR_Cas_User_Information_Auth_System = item.CR_Cas_User_Information_Auth_System;
                            user.CR_Cas_User_Information_Balance = item.CR_Cas_User_Information_Balance;
                            user.CR_Cas_User_Information_Branch_Code = item.CR_Cas_User_Information_Branch_Code;
                            user.CR_Cas_User_Information_Emaile = item.CR_Cas_User_Information_Emaile;
                            user.CR_Cas_User_Information_En_Name = item.CR_Cas_User_Information_En_Name;
                            user.CR_Cas_User_Information_Fr_Name = item.CR_Cas_User_Information_Fr_Name;
                            user.CR_Cas_User_Information_Id = item.CR_Cas_User_Information_Id;
                            user.CR_Cas_User_Information_Image = item.CR_Cas_User_Information_Image;
                            user.CR_Cas_User_Information_Lessor_Code = item.CR_Cas_User_Information_Lessor_Code;
                            user.CR_Cas_User_Information_Mobile = item.CR_Cas_User_Information_Mobile;
                            user.CR_Cas_User_Information_PassWord = item.CR_Cas_User_Information_PassWord;
                            user.CR_Cas_User_Information_Status = item.CR_Cas_User_Information_Status;
                            ListUsers.Add(user);
                        }
                    }
                }
            return PartialView(ListUsers);
        }

        // GET: TransferFromUser/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_User_Information cR_Cas_User_Information = db.CR_Cas_User_Information.Find(id);
            if (cR_Cas_User_Information == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_User_Information);
        }

        public CR_Cas_Administrative_Procedures GetLastRecord(string Lessorcode, string ProcedureCode)
        {
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var Lrecord = db.CR_Cas_Administrative_Procedures.Where(x => x.CR_Cas_Administrative_Procedures_Lessor == Lessorcode
                && x.CR_Cas_Administrative_Procedures_Code == ProcedureCode && x.CR_Cas_Administrative_Procedures_Year == y)
                .Max(lr => lr.CR_Cas_Administrative_Procedures_No.Substring(lr.CR_Cas_Administrative_Procedures_No.Length - 7, 7));



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
        // GET: TransferFromUser/Create
        public ActionResult Create(string id)
        {
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var sector = "1";
            var LessorCode = Session["LessorCode"].ToString();
            var autoInc = GetLastRecord(LessorCode, "62");
            var users = db.CR_Cas_User_Information.FirstOrDefault(u => u.CR_Cas_User_Information_Id == id);
            if (users != null)
            {
                ViewBag.Administrative_Procedures_No = y + "-" + sector + "-" + "62" + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
                ViewBag.TransferDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                //ViewBag.CR_Cas_Sup_Bank_Mas_Code = new SelectList(db.CR_Cas_Sup_Bank
                //     .Where(o => o.CR_Cas_Sup_Bank_Status == "A" && o.CR_Cas_Sup_Bank_Mas_Code != "00" && o.CR_Cas_Sup_Bank_Com_Code == LessorCode).Select(x => x.CR_Mas_Sup_Bank),
                // "CR_Mas_Sup_Bank_Code", "CR_Mas_Sup_Bank_Ar_Name");

            }
            
            return View(users);
        }

        // POST: TransferFromUser/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_User_Information_Id,CR_Cas_User_Information_PassWord," +
            "CR_Cas_User_Information_Lessor_Code,CR_Cas_User_Information_Auth_Branch,CR_Cas_User_Information_Auth_System," +
            "CR_Cas_User_Information_Auth_Owners,CR_Cas_User_Information_Branch_Code,CR_Cas_User_Information_Balance," +
            "CR_Cas_User_Information_Ar_Name,CR_Cas_User_Information_En_Name,CR_Cas_User_Information_Fr_Name," +
            "CR_Cas_User_Information_Mobile,CR_Cas_User_Information_Signature,CR_Cas_User_Information_Image," +
            "CR_Cas_User_Information_Emaile,CR_Cas_User_Information_Status,CR_Cas_User_Information_Reasons")]
            CR_Cas_User_Information cR_Cas_User_Information,string TransferedVal,string Reasons)
        {
            
            var LessorCode = Session["LessorCode"].ToString();
            //var BranchCode = Session["BranchCode"].ToString();
            var UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
            if (ModelState.IsValid)
            {
                ///////////////////////////////Tracing//////////////////////////////////////
                CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
                DateTime year = DateTime.Now;
                var y = year.ToString("yy");
                var sector = "1";
                var ProcedureCode = "62";
                var autoInc = GetLastRecord(LessorCode, ProcedureCode);
                decimal val = 0;

                Ad.CR_Cas_Administrative_Procedures_No = y + "-" + sector + "-" + ProcedureCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
                Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
                string currentTime = DateTime.Now.ToString("HH:mm:ss");
                Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
                Ad.CR_Cas_Administrative_Procedures_Year = y;
                Ad.CR_Cas_Administrative_Procedures_Sector = sector;
                Ad.CR_Cas_Administrative_Procedures_Code = ProcedureCode;
                Ad.CR_Cas_Administrative_Int_Procedures_Code = 62;
                Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
                Ad.CR_Cas_Administrative_Procedures_Targeted_Action = cR_Cas_User_Information.CR_Cas_User_Information_Id;
                Ad.CR_Cas_Administrative_Procedures_User_Insert = UserLogin;
                Ad.CR_Cas_Administrative_Procedures_Type = "I";
                Ad.CR_Cas_Administrative_Procedures_Action = false;
                Ad.CR_Cas_Administrative_Procedures_Reasons = Reasons;
                if (TransferedVal != null && TransferedVal != "")
                {
                    val = decimal.Parse(TransferedVal);
                    Ad.CR_Cas_Administrative_Procedures_Value = decimal.Parse(TransferedVal);
                }
                db.CR_Cas_Administrative_Procedures.Add(Ad);
                ////////////////////////////////////////////////////////////////////////////

                //db.CR_Cas_User_Information.Add(cR_Cas_User_Information);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.CR_Cas_Sup_Bank_Mas_Code = new SelectList(db.CR_Cas_Sup_Bank
            //         .Where(o => o.CR_Cas_Sup_Bank_Status == "A" && o.CR_Cas_Sup_Bank_Mas_Code != "00" && o.CR_Cas_Sup_Bank_Com_Code == LessorCode).Select(x => x.CR_Mas_Sup_Bank),
            //     "CR_Mas_Sup_Bank_Code", "CR_Mas_Sup_Bank_Ar_Name");
            return View(cR_Cas_User_Information);
        }

        // GET: TransferFromUser/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_User_Information cR_Cas_User_Information = db.CR_Cas_User_Information.Find(id);
            if (cR_Cas_User_Information == null)
            {
                return HttpNotFound();
            }
            ViewBag.CR_Cas_User_Information_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_User_Information.CR_Cas_User_Information_Lessor_Code);
            ViewBag.CR_Cas_User_Information_Id = new SelectList(db.CR_Cas_User_Validity_Contract, "CR_Cas_User_Validity_Contract_User_Id", "CR_Cas_User_Validity_Contract_Admin", cR_Cas_User_Information.CR_Cas_User_Information_Id);
            return View(cR_Cas_User_Information);
        }

        // POST: TransferFromUser/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_User_Information_Id,CR_Cas_User_Information_PassWord,CR_Cas_User_Information_Lessor_Code,CR_Cas_User_Information_Auth_Branch,CR_Cas_User_Information_Auth_System,CR_Cas_User_Information_Auth_Owners,CR_Cas_User_Information_Branch_Code,CR_Cas_User_Information_Balance,CR_Cas_User_Information_Ar_Name,CR_Cas_User_Information_En_Name,CR_Cas_User_Information_Fr_Name,CR_Cas_User_Information_Mobile,CR_Cas_User_Information_Signature,CR_Cas_User_Information_Image,CR_Cas_User_Information_Emaile,CR_Cas_User_Information_Status,CR_Cas_User_Information_Reasons")] CR_Cas_User_Information cR_Cas_User_Information)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cR_Cas_User_Information).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CR_Cas_User_Information_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_User_Information.CR_Cas_User_Information_Lessor_Code);
            ViewBag.CR_Cas_User_Information_Id = new SelectList(db.CR_Cas_User_Validity_Contract, "CR_Cas_User_Validity_Contract_User_Id", "CR_Cas_User_Validity_Contract_Admin", cR_Cas_User_Information.CR_Cas_User_Information_Id);
            return View(cR_Cas_User_Information);
        }

        // GET: TransferFromUser/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_User_Information cR_Cas_User_Information = db.CR_Cas_User_Information.Find(id);
            if (cR_Cas_User_Information == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_User_Information);
        }

        // POST: TransferFromUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Cas_User_Information cR_Cas_User_Information = db.CR_Cas_User_Information.Find(id);
            db.CR_Cas_User_Information.Remove(cR_Cas_User_Information);
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
