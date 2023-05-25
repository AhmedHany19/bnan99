using RentCar.Controllers.CAS;
using RentCar.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace RentCar.Controllers
{
    //[SessionState(System.Web.SessionState.SessionStateBehavior.Default)]
    public class CasUserController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: CasUser
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
            var cR_Cas_User_Information = db.CR_Cas_User_Information.Where(c => c.CR_Cas_User_Information_Lessor_Code == LessorCode && c.CR_Cas_User_Information_Id != LessorCode)
                .Include(c => c.CR_Mas_Com_Lessor);
            return View(cR_Cas_User_Information.ToList());
        }

       
        public PartialViewResult PartialIndex(string type)
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
            if (type == "All")
            {
                cR_Cas_User_Information = db.CR_Cas_User_Information.Where(c => c.CR_Cas_User_Information_Lessor_Code == LessorCode 
                && c.CR_Cas_User_Information_Id != LessorCode && c.CR_Cas_User_Information_Id!=UserLogin)
                .Include(c => c.CR_Mas_Com_Lessor);
            }
            else if (type == "D")
            {
                cR_Cas_User_Information = db.CR_Cas_User_Information.Where(c => c.CR_Cas_User_Information_Lessor_Code == LessorCode
                && c.CR_Cas_User_Information_Id != LessorCode && c.CR_Cas_User_Information_Status == "D" && c.CR_Cas_User_Information_Id != UserLogin)
                .Include(c => c.CR_Mas_Com_Lessor);
            }
            else if (type == "H")
            {
                cR_Cas_User_Information = db.CR_Cas_User_Information.Where(c => c.CR_Cas_User_Information_Lessor_Code == LessorCode
                && c.CR_Cas_User_Information_Id != LessorCode && c.CR_Cas_User_Information_Status == "H" && c.CR_Cas_User_Information_Id != UserLogin)
                .Include(c => c.CR_Mas_Com_Lessor);
            }
            else
            {
                cR_Cas_User_Information = db.CR_Cas_User_Information.Where(c => c.CR_Cas_User_Information_Lessor_Code == LessorCode
                 && c.CR_Cas_User_Information_Id != LessorCode && c.CR_Cas_User_Information_Id != UserLogin && c.CR_Cas_User_Information_Status=="A")
                 .Include(c => c.CR_Mas_Com_Lessor);
            }
            return PartialView(cR_Cas_User_Information);
        }

        public PartialViewResult BranchList()
        {
            var LessorCode = Session["LessorCode"].ToString();
            var b = db.CR_Cas_Sup_Branch.Where(x => x.CR_Cas_Sup_Lessor_Code == LessorCode && x.CR_Cas_Sup_Branch_Status == "A");
            return PartialView(b);
        }

        public PartialViewResult ValidityList(string id)
        {
            var LessorCode = Session["LessorCode"].ToString();
            var branchl = db.CR_Cas_Sup_Branch.Where(b => b.CR_Cas_Sup_Lessor_Code == LessorCode && b.CR_Cas_Sup_Branch_Status == "A");
            List<BranchValidityModel> L = new List<BranchValidityModel>();
            foreach (var bl in branchl)
            {
                BranchValidityModel m = new BranchValidityModel();
                m.CR_Cas_Sup_Branch_Ar_Name = bl.CR_Cas_Sup_Branch_Ar_Name;
                m.CR_Cas_Sup_Branch_Ar_Short_Name = bl.CR_Cas_Sup_Branch_Ar_Short_Name;
                m.CR_Cas_Sup_Branch_Code = bl.CR_Cas_Sup_Branch_Code;
                m.CR_Cas_Sup_Branch_En_Name = bl.CR_Cas_Sup_Branch_En_Name;
                m.CR_Cas_Sup_Branch_En_Short_Name = bl.CR_Cas_Sup_Branch_En_Short_Name;
                m.CR_Cas_Sup_Branch_Fr_Name = bl.CR_Cas_Sup_Branch_Fr_Name;
                m.CR_Cas_Sup_Branch_Fr_Short_Name = bl.CR_Cas_Sup_Branch_Fr_Short_Name;
                L.Add(m);
            }
            var CasVal = db.CR_Cas_User_Branch_Validity.Where(v => v.CR_Cas_User_Branch_Validity_Id == id);
            foreach (var c in CasVal)
            {
                foreach (var m in L)
                {
                    if (c.CR_Cas_User_Branch_Validity_Branch == m.CR_Cas_Sup_Branch_Code)
                    {
                        m.CR_Cas_User_Branch_Validity_Branch = c.CR_Cas_User_Branch_Validity_Branch;
                        m.CR_Cas_User_Branch_Validity_Id = id;
                        m.check = true;
                    }
                }
            }

            return PartialView(L);
        }

        // GET: CasUser/Details/5
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


        private void SaveTracing(string LessorCode, string UserID, string ProcedureType)
        {
            CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var sector = "1";
            var ProcedureCode = "45";
            var autoInc = GetLastRecord(ProcedureCode, "1");
            //var LessorCode = Session["LessorCode"].ToString();
            Ad.CR_Cas_Administrative_Procedures_No = y + "-" + sector + "-" + ProcedureCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
            Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
            Ad.CR_Cas_Administrative_Procedures_Year = y;
            Ad.CR_Cas_Administrative_Procedures_Sector = sector;
            Ad.CR_Cas_Administrative_Procedures_Code = ProcedureCode;
            Ad.CR_Cas_Administrative_Int_Procedures_Code = int.Parse(ProcedureCode);
            Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
            Ad.CR_Cas_Administrative_Procedures_Targeted_Action = UserID;
            Ad.CR_Cas_Administrative_Procedures_User_Insert = Session["UserLogin"].ToString();
            Ad.CR_Cas_Administrative_Procedures_Type = ProcedureType;
            Ad.CR_Cas_Administrative_Procedures_Action = true;
            Ad.CR_Cas_Administrative_Procedures_Doc_Date = null;
            Ad.CR_Cas_Administrative_Procedures_Doc_Start_Date = null;
            Ad.CR_Cas_Administrative_Procedures_Doc_End_Date = null;
            Ad.CR_Cas_Administrative_Procedures_Doc_No = "";
            db.CR_Cas_Administrative_Procedures.Add(Ad);
        }

        public void SaveValidityTracing(string LessorCode, string Sector, string ProceduresCode, string ProcedureType, string UserLogin, string Reasons)
        {

            CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var sector = "1";
            var autoInc = GetLastRecord(ProceduresCode, sector);

            Ad.CR_Cas_Administrative_Procedures_No = y + "-" + sector + "-" + ProceduresCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
            Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
            Ad.CR_Cas_Administrative_Procedures_Year = y;
            Ad.CR_Cas_Administrative_Procedures_Sector = Sector;
            Ad.CR_Cas_Administrative_Procedures_Code = ProceduresCode;
            Ad.CR_Cas_Administrative_Int_Procedures_Code = int.Parse(ProceduresCode);
            Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
            Ad.CR_Cas_Administrative_Procedures_Targeted_Action = UserLogin;
            Ad.CR_Cas_Administrative_Procedures_User_Insert = UserLogin;
            Ad.CR_Cas_Administrative_Procedures_Type = ProcedureType;
            Ad.CR_Cas_Administrative_Procedures_Action = true;// نبحثها لاحقا
            Ad.CR_Cas_Administrative_Procedures_Doc_Date = DateTime.Now;
            Ad.CR_Cas_Administrative_Procedures_Doc_Start_Date = null;
            Ad.CR_Cas_Administrative_Procedures_Doc_End_Date = null;
            Ad.CR_Cas_Administrative_Procedures_Doc_No = "";// نبحثها لاحقا
            Ad.CR_Cas_Administrative_Procedures_Reasons = Reasons;
            db.CR_Cas_Administrative_Procedures.Add(Ad);
        }


        // GET: CasUser/Create
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
            return View();
        }

        // POST: CasUser/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_User_Information_Id,CR_Cas_User_Information_PassWord," +
            "CR_Cas_User_Information_Lessor_Code,CR_Cas_User_Information_Auth_Branch,CR_Cas_User_Information_Auth_System,CR_Cas_User_Information_Auth_Owners," +
            "CR_Cas_User_Information_Branch_Code,CR_Cas_User_Information_Ar_Name,CR_Cas_User_Information_En_Name," +
            "CR_Cas_User_Information_Fr_Name,CR_Cas_User_Information_Mobile,CR_Cas_User_Information_Signature," +
            "CR_Cas_User_Information_Emaile,CR_Cas_User_Information_Status,CR_Cas_User_Information_Reasons")]
            CR_Cas_User_Information cR_Cas_User_Information, FormCollection collection,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
            bool CR_Cas_User_Information_Auth_System, bool CR_Cas_User_Information_Auth_Branch,bool? CR_Cas_User_Information_Auth_Owners,
            HttpPostedFileBase UserImgFile, HttpPostedFileBase UserSignatureFile)
        {
            var LessorCode = Session["LessorCode"].ToString();

            var UserIdExist = db.CR_Cas_User_Information.Any(u => u.CR_Cas_User_Information_Id == cR_Cas_User_Information.CR_Cas_User_Information_Id);
            var UserNameExist = db.CR_Cas_User_Information.Any(u => u.CR_Cas_User_Information_Ar_Name == cR_Cas_User_Information.CR_Cas_User_Information_Ar_Name
            && u.CR_Cas_User_Information_Lessor_Code == LessorCode);
            using (DbContextTransaction dbTran = db.Database.BeginTransaction())
            {
                //try
                //{
                    if (ModelState.IsValid)
                    {
                        if (!UserIdExist && !UserNameExist && cR_Cas_User_Information.CR_Cas_User_Information_Id != null && cR_Cas_User_Information.CR_Cas_User_Information_Ar_Name != null
                            && cR_Cas_User_Information.CR_Cas_User_Information_En_Name != null)
                        {
                            //////////////////////////////////////////Add user///////////////////////////////////////////
                            cR_Cas_User_Information.CR_Cas_User_Information_PassWord = cR_Cas_User_Information.CR_Cas_User_Information_Id;
                            cR_Cas_User_Information.CR_Cas_User_Information_Lessor_Code = LessorCode;
                            cR_Cas_User_Information.CR_Cas_User_Information_Balance = 0;
                            cR_Cas_User_Information.CR_Cas_User_Information_Status = "A";
                            db.CR_Cas_User_Information.Add(cR_Cas_User_Information);
                            /////////////////////////////////////////////////////////////////////////////////////////////
                            ///
                            ////////////////////////////Save images/////////////////////////////////////////

                            string UserImgPath = "";

                            if (UserImgFile != null)
                            {

                                string folderimages = Server.MapPath(string.Format("~/{0}/", "/images"));
                                string FolderUserImg = Server.MapPath(string.Format("~/{0}/", "/images/" + "company" + "/" + LessorCode + "/" + "Users" + "/"
                                    + cR_Cas_User_Information.CR_Cas_User_Information_Id +"/Picture"));

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
                                    if (UserImgFile.FileName.Length > 0)
                                    {
                                        UserImgPath = "~/images/" + "company" + "/" + LessorCode + "/" + "Users" + "/" + cR_Cas_User_Information.CR_Cas_User_Information_Id + "/Picture"  +
                                            "/" + Path.GetFileName(UserImgFile.FileName);
                                        UserImgFile.SaveAs(HttpContext.Server.MapPath(UserImgPath));
                                        cR_Cas_User_Information.CR_Cas_User_Information_Image = UserImgPath;


                                    }
                                }
                            }
                            else
                            {
                                cR_Cas_User_Information.CR_Cas_User_Information_Image = "~/images/common/user.jpg";
                            }

                            string UserSignaturePath = "";

                            if (UserSignatureFile != null)
                            {

                                string folderimages = Server.MapPath(string.Format("~/{0}/", "/images"));
                                string FolderUsersign = Server.MapPath(string.Format("~/{0}/", "/images/" + "company" + "/" + LessorCode + "/" + "Users" + "/"
                                    + cR_Cas_User_Information.CR_Cas_User_Information_Id +"/Signature"));


                                if (!Directory.Exists(folderimages))
                                {
                                    Directory.CreateDirectory(folderimages);
                                }
                                if (!Directory.Exists(FolderUsersign))
                                {
                                    Directory.CreateDirectory(FolderUsersign);
                                }


                                if (Directory.Exists(folderimages))
                                {
                                    if (UserSignatureFile.FileName.Length > 0)
                                    {
                                        UserSignaturePath = "~/images/" + "company" + "/" + LessorCode + "/" + "Users" + "/" + cR_Cas_User_Information.CR_Cas_User_Information_Id + "/Signature" +
                                            "/" + Path.GetFileName(UserSignatureFile.FileName);
                                        UserSignatureFile.SaveAs(HttpContext.Server.MapPath(UserSignaturePath));
                                        cR_Cas_User_Information.CR_Cas_User_Information_Signature = UserSignaturePath;
                                    }
                                }
                            }
                            else
                            {
                                cR_Cas_User_Information.CR_Cas_User_Information_Signature = "~/images/common/DefualtUserSignature.png";
                            }
                            ////////////////////////////////////////////////////////////////////////////////
                            /// 
                            /////////////////////////////////////////Add Tracing/////////////////////////////////////////
                            SaveTracing(LessorCode, cR_Cas_User_Information.CR_Cas_User_Information_Id, "I");
                            /////////////////////////////////////////////////////////////////////////////////////////////
                            ///
                            /////////////////////////////////////////Add Authority///////////////////////////////////////
                            List<CR_Cas_User_Branch_Validity> LValidity = new List<CR_Cas_User_Branch_Validity>();
                            string lastval = "";
                            bool branchvalidity = false;
                            foreach (string item in collection.AllKeys)
                            {
                                if (item.StartsWith("chk_"))
                                {
                                    branchvalidity = true;
                                    CR_Cas_User_Branch_Validity Validity = new CR_Cas_User_Branch_Validity();
                                    Validity.CR_Cas_User_Branch_Validity_Id = cR_Cas_User_Information.CR_Cas_User_Information_Id;
                                    Validity.CR_Cas_User_Branch_Validity_Branch = item.Replace("chk_", "").Trim();
                                    if (lastval == "")
                                    {
                                        lastval = Validity.CR_Cas_User_Branch_Validity_Branch;
                                    }
                                    LValidity.Add(Validity);
                                }
                            }
                            LValidity.ForEach(f => db.CR_Cas_User_Branch_Validity.Add(f));
                            /////////////////////////////////////////////////////////////////////////////////////////////
                            cR_Cas_User_Information.CR_Cas_User_Information_Branch_Code = lastval;
                            cR_Cas_User_Information.CR_Cas_User_Information_Auth_Branch = branchvalidity;
                            /////////////////////////////////////////////////////////////////////////////////////////////
                            ////////////////////////////////////Add Cas User Validity///////////////////////////////////////////////////
                            //SaveValidityTracing(LessorCode, "1", "47", "U", cR_Cas_User_Information.CR_Cas_User_Information_Id, "");
                            CR_Cas_User_Validity_Contract validity = new CR_Cas_User_Validity_Contract();
                            validity.CR_Cas_User_Validity_Contract_User_Id = cR_Cas_User_Information.CR_Cas_User_Information_Id;
                            validity.CR_Cas_User_Validity_Contract_Age = false;
                            validity.CR_Cas_User_Validity_Contract_BBrake_Maintenance = false;
                            validity.CR_Cas_User_Validity_Contract_Cancel = false;
                            validity.CR_Cas_User_Validity_Contract_Chamber_Commerce = false;
                            validity.CR_Cas_User_Validity_Contract_Chkec_Up = false;
                            validity.CR_Cas_User_Validity_Contract_Close = 0;
                            validity.CR_Cas_User_Validity_Contract_Close_Amount_Rate = 0;
                            validity.CR_Cas_User_Validity_Contract_Company_Address = false;
                            validity.CR_Cas_User_Validity_Contract_Discount_Rate = 0;
                            validity.CR_Cas_User_Validity_Contract_Driving_License = false;
                            validity.CR_Cas_User_Validity_Contract_Employer = false;
                            validity.CR_Cas_User_Validity_Contract_End = false;
                            validity.CR_Cas_User_Validity_Contract_Extension = false;
                            validity.CR_Cas_User_Validity_Contract_FBrake_Maintenance = false;
                            validity.CR_Cas_User_Validity_Contract_Hour = 0;
                            validity.CR_Cas_User_Validity_Contract_Id = false;
                            validity.CR_Cas_User_Validity_Contract_Insurance = false;
                            validity.CR_Cas_User_Validity_Contract_Km = 0;
                            validity.CR_Cas_User_Validity_Contract_Licence_Municipale = false;
                            validity.CR_Cas_User_Validity_Contract_Maintenance = false;
                            validity.CR_Cas_User_Validity_Contract_Oil = false;
                            validity.CR_Cas_User_Validity_Contract_Open_Amout_Rate = 0;
                            validity.CR_Cas_User_Validity_Contract_Operating_Card = false;
                            validity.CR_Cas_User_Validity_Contract_Reasons = "";
                            validity.CR_Cas_User_Validity_Contract_Register = false;
                            validity.CR_Cas_User_Validity_Contract_Renter_Address = false;
                            validity.CR_Cas_User_Validity_Contract_Transfer_Permission = false;
                            validity.CR_Cas_User_Validity_Contract_Traffic_License = false;
                            validity.CR_Cas_User_Validity_Contract_Tires = false;
                            validity.CR_Cas_User_Validity_Contract_Status = "A";
                            db.CR_Cas_User_Validity_Contract.Add(validity);
                            /////////////////////////////////////////////////////////////////////////////////////////////
                            db.SaveChanges();
                            dbTran.Commit();
                            TempData["TempModel"] = "Added";
                            return RedirectToAction("Create");
                        }
                        else
                        {
                            if (UserIdExist)
                            {
                                ViewBag.UserID = "رقم المستخدم متكرر";
                            }
                            if (UserNameExist)
                            {
                                ViewBag.UserArName = "إسم المستخدم متكرر";
                            }
                            if (cR_Cas_User_Information.CR_Cas_User_Information_Id == null)
                            {
                                ViewBag.UserID = "الرجاء ادخال بيانات الحقل";
                            }
                            if (cR_Cas_User_Information.CR_Cas_User_Information_Ar_Name == null)
                            {
                                ViewBag.UserArName = "الرجاء ادخال بيانات الحقل";
                            }
                            if (cR_Cas_User_Information.CR_Cas_User_Information_En_Name == null)
                            {
                                ViewBag.UserEnName = "الرجاء ادخال بيانات الحقل";
                            }
                            //if (UserSignatureFile == null)
                            //{
                            //    ViewBag.UserSign = "الرجاء ادخال بيانات الحقل";
                            //}
                            if (UserImgFile != null)
                            {
                                ViewBag.UserImgPath = Path.GetDirectoryName(UserImgFile.FileName);
                            }
                            if (UserSignatureFile != null)
                            {
                                ViewBag.UserSignPath = Path.GetFileName(UserSignatureFile.FileName);
                            }

                         
                        }

                    }
                //}
                //catch (DbEntityValidationException ex)
                //{
                //    dbTran.Rollback();
                //    throw ex;
                //}
            }
            return View(cR_Cas_User_Information);
        }

        // GET: CasUser/Edit/5
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
            CR_Cas_User_Information cR_Cas_User_Information = db.CR_Cas_User_Information.Find(id);
            if (cR_Cas_User_Information == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Cas_User_Information.CR_Cas_User_Information_Status == "A" ||
                    cR_Cas_User_Information.CR_Cas_User_Information_Status == "Activated" ||
                    cR_Cas_User_Information.CR_Cas_User_Information_Status == "1" ||
                    cR_Cas_User_Information.CR_Cas_User_Information_Status == "Undeleted")
                {
                    ViewBag.stat = "حذف";
                    ViewBag.h = "إيقاف";
                }

                if (cR_Cas_User_Information.CR_Cas_User_Information_Status == "D" ||
                    cR_Cas_User_Information.CR_Cas_User_Information_Status == "Deleted" ||
                    cR_Cas_User_Information.CR_Cas_User_Information_Status == "0")
                {
                    ViewBag.stat = "إسترجاع";
                    ViewBag.h = "إيقاف";
                    ViewData["ReadOnly"] = "true";
                }

                if (cR_Cas_User_Information.CR_Cas_User_Information_Status == "H" ||
                    cR_Cas_User_Information.CR_Cas_User_Information_Status == "Hold" ||
                    cR_Cas_User_Information.CR_Cas_User_Information_Status == "2")
                {
                    ViewBag.h = "تنشيط";
                    ViewBag.stat = "حذف";
                    ViewData["ReadOnly"] = "true";
                }

                if (cR_Cas_User_Information.CR_Cas_User_Information_Status == null)
                {
                    ViewBag.h = "إيقاف";
                    ViewBag.stat = "حذف";
                }
                if (cR_Cas_User_Information.CR_Cas_User_Information_Image != null && cR_Cas_User_Information.CR_Cas_User_Information_Image != "")
                {
                    ViewBag.UserImgPath = cR_Cas_User_Information.CR_Cas_User_Information_Image;
                }
                else
                {
                    ViewBag.UserImgPath = "       ";
                    cR_Cas_User_Information.CR_Cas_User_Information_Image = "       ";

                }
                if (cR_Cas_User_Information.CR_Cas_User_Information_Signature != null && cR_Cas_User_Information.CR_Cas_User_Information_Signature != "")
                {
                    ViewBag.UserSignPath = cR_Cas_User_Information.CR_Cas_User_Information_Signature;
                }
                else
                {
                    ViewBag.UserSignPath = "        ";
                    cR_Cas_User_Information.CR_Cas_User_Information_Signature = "      ";
                }
                ViewBag.UserCode = cR_Cas_User_Information.CR_Cas_User_Information_Id;
                ViewBag.delete = cR_Cas_User_Information.CR_Cas_User_Information_Status;

                if (cR_Cas_User_Information.CR_Cas_User_Information_Balance != null)
                {
                    if (cR_Cas_User_Information.CR_Cas_User_Information_Balance != 0)
                    {
                        TempData["HaveReceipt"] = "False";
                    }
                    else
                    {
                        TempData["HaveReceipt"] = "True";
                    }
                }
                else
                {
                    TempData["HaveReceipt"] = "True";
                }
                
            }

            return View(cR_Cas_User_Information);
        }

        // POST: CasUser/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_User_Information_Id,CR_Cas_User_Information_PassWord," +
            "CR_Cas_User_Information_Lessor_Code,CR_Cas_User_Information_Auth_Branch,CR_Cas_User_Information_Auth_System,CR_Cas_User_Information_Auth_Owners," +
            "CR_Cas_User_Information_Branch_Code,CR_Cas_User_Information_Ar_Name,CR_Cas_User_Information_En_Name," +
            "CR_Cas_User_Information_Fr_Name,CR_Cas_User_Information_Mobile,CR_Cas_User_Information_Signature," +
            "CR_Cas_User_Information_Emaile,CR_Cas_User_Information_Status,CR_Cas_User_Information_Reasons")]
            CR_Cas_User_Information cR_Cas_User_Information, FormCollection collection,
            bool CR_Cas_User_Information_Auth_System, bool CR_Cas_User_Information_Auth_Branch,bool CR_Cas_User_Information_Auth_Owners,
            HttpPostedFileBase UserImgFile, HttpPostedFileBase UserSignatureFile, string save, string delete, string hold,
            string UserSignaturePath, string UserImgagePath,string GetPassword)
        {
            var LessorCode = Session["LessorCode"].ToString();
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {

                    using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                    {
                        try
                        {
                            if (UserSignatureFile != null || UserSignaturePath != null)
                            {
                                cR_Cas_User_Information.CR_Cas_User_Information_Status = "A";
                                cR_Cas_User_Information.CR_Cas_User_Information_PassWord = cR_Cas_User_Information.CR_Cas_User_Information_Id;


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
                                            cR_Cas_User_Information.CR_Cas_User_Information_Image = UserImgPath;


                                        }
                                    }
                                }
                                else
                                {
                                    cR_Cas_User_Information.CR_Cas_User_Information_Image = UserImgagePath;
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
                                            cR_Cas_User_Information.CR_Cas_User_Information_Signature = UserSigPath;
                                        }
                                    }
                                }
                                else
                                {
                                    cR_Cas_User_Information.CR_Cas_User_Information_Signature = UserSignaturePath;
                                }
                                ////////////////////////////////////////////////////////////////////////////////
                                /// 
                                /////////////////////////////////////////Add Tracing/////////////////////////////////////////
                                SaveTracing(LessorCode, cR_Cas_User_Information.CR_Cas_User_Information_Id, "U");
                                /////////////////////////////////////////////////////////////////////////////////////////////
                                //////////////////////////////////////Delete Authority///////////////////////////////////////
                                DeleteAuthority(cR_Cas_User_Information.CR_Cas_User_Information_Id);
                                /// /////////////////////////////////////////////////////////////////////////////////////////
                                /////////////////////////////////////////Add Authority///////////////////////////////////////
                                List<CR_Cas_User_Branch_Validity> LValidity = new List<CR_Cas_User_Branch_Validity>();
                                string lastval = "";
                                bool AuthorityBranch = false;
                                foreach (string item in collection.AllKeys)
                                {
                                    if (item.StartsWith("chk_"))
                                    {
                                        AuthorityBranch = true;
                                        CR_Cas_User_Branch_Validity Validity = new CR_Cas_User_Branch_Validity();
                                        Validity.CR_Cas_User_Branch_Validity_Id = cR_Cas_User_Information.CR_Cas_User_Information_Id;
                                        Validity.CR_Cas_User_Branch_Validity_Branch = item.Replace("chk_", "");
                                        if (lastval == "")
                                        {
                                            lastval = Validity.CR_Cas_User_Branch_Validity_Branch;
                                        }
                                        LValidity.Add(Validity);
                                    }
                                }
                                LValidity.ForEach(f => db.CR_Cas_User_Branch_Validity.Add(f));
                                /////////////////////////////////////////////////////////////////////////////////////////////
                                cR_Cas_User_Information.CR_Cas_User_Information_Branch_Code = lastval;
                                cR_Cas_User_Information.CR_Cas_User_Information_Auth_Branch = AuthorityBranch;
                                /////////////////////////////////////////////////////////////////////////////////////////////

                                db.Entry(cR_Cas_User_Information).State = EntityState.Modified;
                                db.SaveChanges();
                                TempData["TempModel"] = "Saved";
                                dbTran.Commit();
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                if (UserSignatureFile == null && UserSignaturePath == null)
                                {
                                    ViewBag.UserSign = "الرجاء ادخال بيانات الحقل";
                                }
                            }
                        }
                        catch (DbEntityValidationException ex)
                        {
                            dbTran.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            if (delete == "Delete" || delete == "حذف")
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        //////////////////////////Save Tracing/////////////////////////////
                        var UserLogin = Session["UserLogin"].ToString();
                        SaveTracing(LessorCode, cR_Cas_User_Information.CR_Cas_User_Information_Id, "D");
                        ///////////////////////////////////////////////////////////////////
                        cR_Cas_User_Information.CR_Cas_User_Information_Image = UserImgagePath;
                        cR_Cas_User_Information.CR_Cas_User_Information_Signature = UserSignaturePath;
                        cR_Cas_User_Information.CR_Cas_User_Information_Status = "D";
                        cR_Cas_User_Information.CR_Cas_User_Information_PassWord = cR_Cas_User_Information.CR_Cas_User_Information_Id;
                        db.Entry(cR_Cas_User_Information).State = EntityState.Modified;
                        db.SaveChanges();
                        dbTran.Commit();
                        TempData["TempModel"] = "Deleted";
                        return RedirectToAction("Index");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }
            }
            if (delete == "Activate" || delete == "إسترجاع")
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {

                        //////////////////////////Save Tracing/////////////////////////////
                        var UserLogin = Session["UserLogin"].ToString();
                        SaveTracing(LessorCode, cR_Cas_User_Information.CR_Cas_User_Information_Id, "A");
                        ///////////////////////////////////////////////////////////////////
                        cR_Cas_User_Information.CR_Cas_User_Information_Image = UserImgagePath;
                        cR_Cas_User_Information.CR_Cas_User_Information_Signature = UserSignaturePath;
                        cR_Cas_User_Information.CR_Cas_User_Information_Status = "A";
                        cR_Cas_User_Information.CR_Cas_User_Information_PassWord = cR_Cas_User_Information.CR_Cas_User_Information_Id;
                        db.Entry(cR_Cas_User_Information).State = EntityState.Modified;
                        db.SaveChanges();
                        dbTran.Commit();
                        TempData["TempModel"] = "Activated";
                        return RedirectToAction("Index");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }


            }
            if (hold == "إيقاف" || hold == "hold")
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        //////////////////////////Save Tracing/////////////////////////////
                        var UserLogin = Session["UserLogin"].ToString();
                        SaveTracing(LessorCode, cR_Cas_User_Information.CR_Cas_User_Information_Id, "H");
                        ///////////////////////////////////////////////////////////////////
                        cR_Cas_User_Information.CR_Cas_User_Information_Image = UserImgagePath;
                        cR_Cas_User_Information.CR_Cas_User_Information_Signature = UserSignaturePath;
                        cR_Cas_User_Information.CR_Cas_User_Information_Status = "H";
                        cR_Cas_User_Information.CR_Cas_User_Information_PassWord = cR_Cas_User_Information.CR_Cas_User_Information_Id;
                        db.Entry(cR_Cas_User_Information).State = EntityState.Modified;
                        db.SaveChanges();
                        dbTran.Commit();
                        TempData["TempModel"] = "Holded";
                        return RedirectToAction("Index");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }

            }
            if (hold == "تنشيط" || hold == "Activate")
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {

                        //////////////////////////Save Tracing/////////////////////////////
                        var UserLogin = Session["UserLogin"].ToString();
                        SaveTracing(LessorCode, cR_Cas_User_Information.CR_Cas_User_Information_Id, "A");
                        ///////////////////////////////////////////////////////////////////
                        cR_Cas_User_Information.CR_Cas_User_Information_Image = UserImgagePath;
                        cR_Cas_User_Information.CR_Cas_User_Information_Signature = UserSignaturePath;
                        cR_Cas_User_Information.CR_Cas_User_Information_Status = "A";
                        cR_Cas_User_Information.CR_Cas_User_Information_PassWord = cR_Cas_User_Information.CR_Cas_User_Information_Id;
                        db.Entry(cR_Cas_User_Information).State = EntityState.Modified;
                        db.SaveChanges();
                        dbTran.Commit();
                        TempData["TempModel"] = "Activated";
                        return RedirectToAction("Index");

                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }

            }
            if (GetPassword == "إسترجاع كلمة السر")
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var user = db.CR_Cas_User_Information.FirstOrDefault(u=>u.CR_Cas_User_Information_Id==cR_Cas_User_Information.CR_Cas_User_Information_Id);
                        if (user != null)
                        {
                            SaveTracing(LessorCode, cR_Cas_User_Information.CR_Cas_User_Information_Id, "U");
                            user.CR_Cas_User_Information_PassWord = cR_Cas_User_Information.CR_Cas_User_Information_Id;
                            db.Entry(user).State = EntityState.Modified;
                            db.SaveChanges();
                            dbTran.Commit();
                            TempData["TempModel"] = "PasswordActivated";
                            return RedirectToAction("Index");
                        }
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }

            }
            if (cR_Cas_User_Information.CR_Cas_User_Information_Status == "A" ||
            cR_Cas_User_Information.CR_Cas_User_Information_Status == "Activated" ||
            cR_Cas_User_Information.CR_Cas_User_Information_Status == "1" ||
            cR_Cas_User_Information.CR_Cas_User_Information_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }
            if ((cR_Cas_User_Information.CR_Cas_User_Information_Status == "D" ||
                 cR_Cas_User_Information.CR_Cas_User_Information_Status == "Deleted" ||
                 cR_Cas_User_Information.CR_Cas_User_Information_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
            }
            if (cR_Cas_User_Information.CR_Cas_User_Information_Status == "H" ||
                cR_Cas_User_Information.CR_Cas_User_Information_Status == "Hold" ||
                cR_Cas_User_Information.CR_Cas_User_Information_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
            }
            if (cR_Cas_User_Information.CR_Cas_User_Information_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }

            return View(cR_Cas_User_Information);
        }
        public void DeleteAuthority(string id)
        {
            id = id.Trim();
            var q = db.CR_Cas_User_Branch_Validity.Where(c => c.CR_Cas_User_Branch_Validity_Id == id);

            foreach (var item in q)
            {
                var auth = db.CR_Cas_User_Branch_Validity.Where(x => x.CR_Cas_User_Branch_Validity_Id == id && x.CR_Cas_User_Branch_Validity_Branch == item.CR_Cas_User_Branch_Validity_Branch).First();
                db.CR_Cas_User_Branch_Validity.Remove(auth);
            }
            //db.SaveChanges();
        }
        // GET: CasUser/Delete/5
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

        // POST: CasUser/Delete/5
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
