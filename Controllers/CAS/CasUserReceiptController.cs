using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentCar.Models;
using RentCar.Models.CAS;

namespace RentCar.Controllers.CAS
{
    public class CasUserReceiptController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: CasUserReceipt
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

            List<CasUserMD> ListUser = new List<CasUserMD>();

            var cR_Cas_User_Information = db.CR_Cas_User_Information.Where(u=>u.CR_Cas_User_Information_Status=="A" && u.CR_Cas_User_Information_Lessor_Code==LessorCode)
                .Include(c => c.CR_Mas_Com_Lessor);
            foreach(var UserInfo in cR_Cas_User_Information)
            {
                var receipt = db.CR_Cas_Account_Receipt.Where(r => r.CR_Cas_Account_Receipt_User_Code == UserInfo.CR_Cas_User_Information_Id);
                var nbr = receipt.Count();
                if (nbr > 0)
                {
                    CasUserMD user = new CasUserMD();
                    user.CR_Cas_User_Information_Id = UserInfo.CR_Cas_User_Information_Id;
                    user.CR_Cas_User_Information_PassWord = UserInfo.CR_Cas_User_Information_PassWord;
                    user.CR_Cas_User_Information_Lessor_Code = UserInfo.CR_Cas_User_Information_Lessor_Code;
                    user.CR_Cas_User_Information_Auth_Branch = UserInfo.CR_Cas_User_Information_Auth_Branch;
                    user.CR_Cas_User_Information_Auth_Owners = UserInfo.CR_Cas_User_Information_Auth_Owners;
                    user.CR_Cas_User_Information_Auth_System = UserInfo.CR_Cas_User_Information_Auth_System;
                    user.CR_Cas_User_Information_Branch_Code = UserInfo.CR_Cas_User_Information_Branch_Code;
                    user.CR_Cas_User_Information_Balance = UserInfo.CR_Cas_User_Information_Balance;
                    user.CR_Cas_User_Information_Ar_Name = UserInfo.CR_Cas_User_Information_Ar_Name;
                    user.CR_Cas_User_Information_En_Name = UserInfo.CR_Cas_User_Information_En_Name;
                    user.CR_Cas_User_Information_Fr_Name = UserInfo.CR_Cas_User_Information_Fr_Name;
                    user.CR_Cas_User_Information_Mobile = UserInfo.CR_Cas_User_Information_Mobile;
                    user.CR_Cas_User_Information_Emaile = UserInfo.CR_Cas_User_Information_Emaile;
                    user.ReceiptCreditCount = db.CR_Cas_Account_Receipt.Where(r=>r.CR_Cas_Account_Receipt_User_Code==UserInfo.CR_Cas_User_Information_Id && r.CR_Cas_Account_Receipt_Type=="60").Count();
                    user.ReceiptDebitCount = db.CR_Cas_Account_Receipt.Where(r => r.CR_Cas_Account_Receipt_User_Code == UserInfo.CR_Cas_User_Information_Id && r.CR_Cas_Account_Receipt_Type == "61").Count();
                    ListUser.Add(user);
                }
            }



            return View(ListUser);
        }


        public ActionResult ReceiptList(string id)
        {
            var receipt = db.CR_Cas_Account_Receipt.Where(r => r.CR_Cas_Account_Receipt_User_Code == id && r.CR_Cas_Account_Receipt_Payment_Method!="24");
            var User = db.CR_Cas_User_Information.FirstOrDefault(u=>u.CR_Cas_User_Information_Id==id);
            if (User != null)
            {
                ViewBag.UserName = User.CR_Cas_User_Information_Ar_Name;
            }
            
            return View(receipt);
        }



        // GET: CasUserReceipt/Details/5
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

        // GET: CasUserReceipt/Create
        public ActionResult Create()
        {
            ViewBag.CR_Cas_User_Information_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo");
            ViewBag.CR_Cas_User_Information_Id = new SelectList(db.CR_Cas_User_Validity_Contract, "CR_Cas_User_Validity_Contract_User_Id", "CR_Cas_User_Validity_Contract_Admin");
            return View();
        }

        // POST: CasUserReceipt/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_User_Information_Id,CR_Cas_User_Information_PassWord,CR_Cas_User_Information_Lessor_Code,CR_Cas_User_Information_Auth_Branch,CR_Cas_User_Information_Auth_System,CR_Cas_User_Information_Auth_Owners,CR_Cas_User_Information_Branch_Code,CR_Cas_User_Information_Balance,CR_Cas_User_Information_Ar_Name,CR_Cas_User_Information_En_Name,CR_Cas_User_Information_Fr_Name,CR_Cas_User_Information_Mobile,CR_Cas_User_Information_Signature,CR_Cas_User_Information_Image,CR_Cas_User_Information_Emaile,CR_Cas_User_Information_Status,CR_Cas_User_Information_Reasons")] CR_Cas_User_Information cR_Cas_User_Information)
        {
            if (ModelState.IsValid)
            {
                db.CR_Cas_User_Information.Add(cR_Cas_User_Information);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CR_Cas_User_Information_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_User_Information.CR_Cas_User_Information_Lessor_Code);
            ViewBag.CR_Cas_User_Information_Id = new SelectList(db.CR_Cas_User_Validity_Contract, "CR_Cas_User_Validity_Contract_User_Id", "CR_Cas_User_Validity_Contract_Admin", cR_Cas_User_Information.CR_Cas_User_Information_Id);
            return View(cR_Cas_User_Information);
        }

        // GET: CasUserReceipt/Edit/5
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

        // POST: CasUserReceipt/Edit/5
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

        // GET: CasUserReceipt/Delete/5
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

        // POST: CasUserReceipt/Delete/5
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
