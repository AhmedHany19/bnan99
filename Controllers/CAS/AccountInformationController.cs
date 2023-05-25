using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentCar.Models;

namespace RentCar.Controllers.CAS
{
    public class AccountInformationController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: AccountInformation
        public ActionResult Index()
        {
            var cR_Cas_User_Information = db.CR_Cas_User_Information.Include(c => c.CR_Mas_Com_Lessor);
            return View(cR_Cas_User_Information.ToList());
        }

        // GET: AccountInformation/Details/5
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

        // GET: AccountInformation/Create
        public ActionResult Create()
        {
            ViewBag.CR_Cas_User_Information_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo");
            return View();
        }

        // POST: AccountInformation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_User_Information_Id,CR_Cas_User_Information_PassWord,CR_Cas_User_Information_Lessor_Code,CR_Cas_User_Information_Auth_Branch,CR_Cas_User_Information_Auth_System,CR_Cas_User_Information_Branch_Code,CR_Cas_User_Information_Ar_Name,CR_Cas_User_Information_En_Name,CR_Cas_User_Information_Fr_Name,CR_Cas_User_Information_Mobile,CR_Cas_User_Information_Signature,CR_Cas_User_Information_Emaile,CR_Cas_User_Information_Status,CR_Cas_User_Information_Reasons,CR_Cas_User_Information_Image")] CR_Cas_User_Information cR_Cas_User_Information)
        {
            if (ModelState.IsValid)
            {
                db.CR_Cas_User_Information.Add(cR_Cas_User_Information);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CR_Cas_User_Information_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_User_Information.CR_Cas_User_Information_Lessor_Code);
            return View(cR_Cas_User_Information);
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
        private void SaveTracing(string LessorCode, string TargetedAction, string ProcedureType)
        {
            CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var sector = "1";
            var ProcedureCode = "58";
            var autoInc = GetLastRecord(ProcedureCode, "1");
            Ad.CR_Cas_Administrative_Procedures_No = y + "-" + sector + "-" + ProcedureCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
            Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
            Ad.CR_Cas_Administrative_Procedures_Year = y;
            Ad.CR_Cas_Administrative_Procedures_Sector = sector;
            Ad.CR_Cas_Administrative_Procedures_Code = ProcedureCode;
            Ad.CR_Cas_Administrative_Int_Procedures_Code = int.Parse(ProcedureCode);
            Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
            Ad.CR_Cas_Administrative_Procedures_Targeted_Action = TargetedAction;
            Ad.CR_Cas_Administrative_Procedures_User_Insert = Session["UserLogin"].ToString();
            Ad.CR_Cas_Administrative_Procedures_Type = ProcedureType;
            Ad.CR_Cas_Administrative_Procedures_Action = true;
            Ad.CR_Cas_Administrative_Procedures_Doc_Date = null;
            Ad.CR_Cas_Administrative_Procedures_Doc_Start_Date = null;
            Ad.CR_Cas_Administrative_Procedures_Doc_End_Date = null;
            Ad.CR_Cas_Administrative_Procedures_Doc_No = "";
           // Ad.CR_Cas_Administrative_Procedures_Reasons = reasons;
            db.CR_Cas_Administrative_Procedures.Add(Ad);
        }
        // GET: AccountInformation/Edit/5
        public ActionResult Edit()
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
            if (UserLogin == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_User_Information cR_Cas_User_Information = db.CR_Cas_User_Information.Find(UserLogin);
            if (cR_Cas_User_Information == null)
            {
                return HttpNotFound();
            }
            if (cR_Cas_User_Information.CR_Cas_User_Information_Image != null && cR_Cas_User_Information.CR_Cas_User_Information_Image != "")
            {
                ViewBag.UserImgPath = cR_Cas_User_Information.CR_Cas_User_Information_Image;
                
            }
            else
            {
                ViewBag.UserImgPath = "      ";
                cR_Cas_User_Information.CR_Cas_User_Information_Image = "        ";
            }
            if (cR_Cas_User_Information.CR_Cas_User_Information_Signature != null && cR_Cas_User_Information.CR_Cas_User_Information_Signature != "")
            {
                ViewBag.UserSignPath = cR_Cas_User_Information.CR_Cas_User_Information_Signature;
            }
            else
            {
                ViewBag.UserSignPath = "      ";
                cR_Cas_User_Information.CR_Cas_User_Information_Signature = "       ";
            }
            cR_Cas_User_Information.CR_Cas_User_Information_PassWord = cR_Cas_User_Information.CR_Cas_User_Information_PassWord.Trim();
            //cR_Cas_User_Information.CR_Cas_User_Information_PassWord = "";
            return View(cR_Cas_User_Information);
        }

        // POST: AccountInformation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_User_Information_Id,CR_Cas_User_Information_PassWord," +
            "CR_Cas_User_Information_Lessor_Code,CR_Cas_User_Information_Auth_Branch,CR_Cas_User_Information_Auth_System," +
            "CR_Cas_User_Information_Branch_Code,CR_Cas_User_Information_Ar_Name,CR_Cas_User_Information_En_Name," +
            "CR_Cas_User_Information_Fr_Name,CR_Cas_User_Information_Mobile,CR_Cas_User_Information_Signature," +
            "CR_Cas_User_Information_Emaile,CR_Cas_User_Information_Status,CR_Cas_User_Information_Reasons," +
            "CR_Cas_User_Information_Image")] CR_Cas_User_Information cR_Cas_User_Information, string UserSignaturePath, string UserImgagePath,
            HttpPostedFileBase UserImgFile, HttpPostedFileBase UserSignatureFile,string NewPassword)
        {
            if (ModelState.IsValid)
            {
                
                var user = db.CR_Cas_User_Information.FirstOrDefault(u => u.CR_Cas_User_Information_Id == cR_Cas_User_Information.CR_Cas_User_Information_Id);
                var LessorCode = Session["LessorCode"].ToString();
                if (user != null)
                {
                    if(NewPassword!=null && NewPassword != "")
                    {
                        user.CR_Cas_User_Information_PassWord = NewPassword;
                    }
                    
                    user.CR_Cas_User_Information_Emaile = cR_Cas_User_Information.CR_Cas_User_Information_Emaile;
                    user.CR_Cas_User_Information_Mobile = cR_Cas_User_Information.CR_Cas_User_Information_Mobile;
                    ////////////////////////////Save images/////////////////////////////////////////

                    string UserImgPath = "";

                    if (UserImgFile != null)
                    {

                        string folderimages = Server.MapPath(string.Format("~/{0}/", "/images"));
                        string FolderUserImg = Server.MapPath(string.Format("~/{0}/", "/images/" + "company" + "/" + LessorCode + "/" + "Users" + "/"
                            + cR_Cas_User_Information.CR_Cas_User_Information_Id));

                        if (!Directory.Exists(folderimages))
                        {
                            Directory.CreateDirectory(folderimages);
                        }
                        if (!Directory.Exists(FolderUserImg))
                        {
                            Directory.CreateDirectory(FolderUserImg);
                        }
                        if (Directory.Exists(folderimages))
                        {
                            Directory.CreateDirectory(folderimages);
                            Directory.CreateDirectory(FolderUserImg);
                            if (UserImgFile.FileName.Length > 0)
                            {
                                UserImgPath = "~/images/" + "company" + "/" + LessorCode + "/" + "Users" + "/" + cR_Cas_User_Information.CR_Cas_User_Information_Id.Trim() +
                                    "/" + Path.GetFileName(UserImgFile.FileName);
                                UserImgFile.SaveAs(HttpContext.Server.MapPath(UserImgPath));
                                user.CR_Cas_User_Information_Image = UserImgPath;


                            }
                        }
                    }
                    else
                    {
                        user.CR_Cas_User_Information_Image = UserImgagePath;
                    }

                    string UserSigPath = "";

                    if (UserSignatureFile != null)
                    {
                        var UserId = cR_Cas_User_Information.CR_Cas_User_Information_Id.Trim();
                        string folderimages = Server.MapPath(string.Format("~/{0}/", "/images"));
                        string FolderUserImg = Server.MapPath(string.Format("~/{0}/", "/images/" + "company" + "/" + LessorCode + "/" + "Users" + "/"
                            + cR_Cas_User_Information.CR_Cas_User_Information_Id));

                        if (!Directory.Exists(folderimages))
                        {
                            Directory.CreateDirectory(folderimages);
                        }
                        if (!Directory.Exists(FolderUserImg))
                        {
                            Directory.CreateDirectory(FolderUserImg);
                        }

                        if (Directory.Exists(folderimages))
                        {
                            Directory.CreateDirectory(folderimages);
                            Directory.CreateDirectory(FolderUserImg);
                            if (UserSignatureFile.FileName.Length > 0)
                            {
                                UserSigPath = "~/images/" + "company" + "/" + LessorCode + "/" + "Users" + "/" + UserId +
                                    "/" + Path.GetFileName(UserSignatureFile.FileName);
                                UserSignatureFile.SaveAs(HttpContext.Server.MapPath(UserSigPath));
                                user.CR_Cas_User_Information_Signature = UserSigPath;
                            }
                        }
                    }
                    else
                    {
                        user.CR_Cas_User_Information_Signature = UserSignaturePath;
                    }
                    ////////////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////Tracing//////////////////////////////////////
                    SaveTracing(LessorCode, cR_Cas_User_Information.CR_Cas_User_Information_Id, "U");
                    ///////////////////////////////////////////////////////////////////////////////
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "CasHome");
                       
                }
            }
            
            return View(cR_Cas_User_Information);
        }



        // GET: AccountInformation/Edit/5
        public ActionResult EditBranchAccount()
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
            if (UserLogin == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_User_Information cR_Cas_User_Information = db.CR_Cas_User_Information.Find(UserLogin);
            if (cR_Cas_User_Information == null)
            {
                return HttpNotFound();
            }
            if (cR_Cas_User_Information.CR_Cas_User_Information_Image != null && cR_Cas_User_Information.CR_Cas_User_Information_Image != "")
            {
                ViewBag.UserImgPath = cR_Cas_User_Information.CR_Cas_User_Information_Image;

            }
            else
            {
                ViewBag.UserImgPath = "      ";
                cR_Cas_User_Information.CR_Cas_User_Information_Image = "        ";
            }
            if (cR_Cas_User_Information.CR_Cas_User_Information_Signature != null && cR_Cas_User_Information.CR_Cas_User_Information_Signature != "")
            {
                ViewBag.UserSignPath = cR_Cas_User_Information.CR_Cas_User_Information_Signature;
            }
            else
            {
                ViewBag.UserSignPath = "      ";
                cR_Cas_User_Information.CR_Cas_User_Information_Signature = "       ";
            }
            cR_Cas_User_Information.CR_Cas_User_Information_PassWord = cR_Cas_User_Information.CR_Cas_User_Information_PassWord.Trim();
            //cR_Cas_User_Information.CR_Cas_User_Information_PassWord = "";
            return View(cR_Cas_User_Information);
        }

        // POST: AccountInformation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBranchAccount([Bind(Include = "CR_Cas_User_Information_Id,CR_Cas_User_Information_PassWord," +
            "CR_Cas_User_Information_Lessor_Code,CR_Cas_User_Information_Auth_Branch,CR_Cas_User_Information_Auth_System," +
            "CR_Cas_User_Information_Branch_Code,CR_Cas_User_Information_Ar_Name,CR_Cas_User_Information_En_Name," +
            "CR_Cas_User_Information_Fr_Name,CR_Cas_User_Information_Mobile,CR_Cas_User_Information_Signature," +
            "CR_Cas_User_Information_Emaile,CR_Cas_User_Information_Status,CR_Cas_User_Information_Reasons," +
            "CR_Cas_User_Information_Image")] CR_Cas_User_Information cR_Cas_User_Information, string BranchUserSignaturePath, string BranchUserImgagePath,
            HttpPostedFileBase BranchUserImgFile, HttpPostedFileBase BranchUserSignatureFile, string NewPassword)
        {
            if (ModelState.IsValid)
            {

                var user = db.CR_Cas_User_Information.FirstOrDefault(u => u.CR_Cas_User_Information_Id == cR_Cas_User_Information.CR_Cas_User_Information_Id);
                var LessorCode = Session["LessorCode"].ToString();
                if (user != null)
                {
                    if (NewPassword != null && NewPassword != "")
                    {
                        user.CR_Cas_User_Information_PassWord = NewPassword;
                    }

                    user.CR_Cas_User_Information_Emaile = cR_Cas_User_Information.CR_Cas_User_Information_Emaile;
                    user.CR_Cas_User_Information_Mobile = cR_Cas_User_Information.CR_Cas_User_Information_Mobile;
                    ////////////////////////////Save images/////////////////////////////////////////

                    string UserImgPath = "";

                    if (BranchUserImgFile != null)
                    {

                        string folderimages = Server.MapPath(string.Format("~/{0}/", "/images"));
                        string FolderUserImg = Server.MapPath(string.Format("~/{0}/", "/images/" + "company" + "/" + LessorCode + "/" + "Users" + "/"
                            + cR_Cas_User_Information.CR_Cas_User_Information_Id));

                        if (!Directory.Exists(folderimages))
                        {
                            Directory.CreateDirectory(folderimages);
                        }
                        if (!Directory.Exists(FolderUserImg))
                        {
                            Directory.CreateDirectory(FolderUserImg);
                        }
                        if (Directory.Exists(folderimages))
                        {
                            Directory.CreateDirectory(folderimages);
                            Directory.CreateDirectory(FolderUserImg);
                            if (BranchUserImgFile.FileName.Length > 0)
                            {
                                UserImgPath = "~/images/" + "company" + "/" + LessorCode + "/" + "Users" + "/" + cR_Cas_User_Information.CR_Cas_User_Information_Id.Trim() +
                                    "/" + Path.GetFileName(BranchUserImgFile.FileName);
                                BranchUserImgFile.SaveAs(HttpContext.Server.MapPath(UserImgPath));
                                user.CR_Cas_User_Information_Image = UserImgPath;


                            }
                        }
                    }
                    else
                    {
                        user.CR_Cas_User_Information_Image = BranchUserImgagePath;
                    }

                    string UserSigPath = "";

                    if (BranchUserSignatureFile != null)
                    {
                        var UserId = cR_Cas_User_Information.CR_Cas_User_Information_Id.Trim();
                        string folderimages = Server.MapPath(string.Format("~/{0}/", "/images"));
                        string FolderUserImg = Server.MapPath(string.Format("~/{0}/", "/images/" + "company" + "/" + LessorCode + "/" + "Users" + "/"
                            + cR_Cas_User_Information.CR_Cas_User_Information_Id));

                        if (!Directory.Exists(folderimages))
                        {
                            Directory.CreateDirectory(folderimages);
                        }
                        if (!Directory.Exists(FolderUserImg))
                        {
                            Directory.CreateDirectory(FolderUserImg);
                        }

                        if (Directory.Exists(folderimages))
                        {
                            Directory.CreateDirectory(folderimages);
                            Directory.CreateDirectory(FolderUserImg);
                            if (BranchUserSignatureFile.FileName.Length > 0)
                            {
                                UserSigPath = "~/images/" + "company" + "/" + LessorCode + "/" + "Users" + "/" + UserId +
                                    "/" + Path.GetFileName(BranchUserSignatureFile.FileName);
                                BranchUserSignatureFile.SaveAs(HttpContext.Server.MapPath(UserSigPath));
                                user.CR_Cas_User_Information_Signature = UserSigPath;
                            }
                        }
                    }
                    else
                    {
                        user.CR_Cas_User_Information_Signature = BranchUserSignaturePath;
                    }
                    ////////////////////////////////////////////////////////////////////////////////

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("BranchHome", "BranchHome");

                }
            }

            return View(cR_Cas_User_Information);
        }



        // GET: AccountInformation/Delete/5
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

        // POST: AccountInformation/Delete/5
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
