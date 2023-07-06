using RentCar.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;

namespace RentCar.Controllers
{
    public class CasOwnersController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        [HttpGet]
        [ActionName("Index")]
        public ActionResult Index_Get()
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
            var cR_Cas_Sup_Owners = db.CR_Cas_Sup_Owners.Where(x => x.CR_Cas_Sup_Owners_Lessor_Code == LessorCode).Include(c => c.CR_Mas_Com_Lessor);

            List<OwnerCarsModel> l = new List<OwnerCarsModel>();
            foreach (var i in cR_Cas_Sup_Owners)
            {
                OwnerCarsModel o = new OwnerCarsModel();
                var nbr = db.CR_Cas_Sup_Car_Information.Where(c => c.CR_Cas_Sup_Car_Owner_Code == i.CR_Cas_Sup_Owners_Code).Count();
                o.CR_Cas_Sup_Car_Information = i.CR_Cas_Sup_Car_Information;
                o.CR_Cas_Sup_Owners_Ar_Long_Name = i.CR_Cas_Sup_Owners_Ar_Long_Name;
                o.CR_Cas_Sup_Owners_Code = i.CR_Cas_Sup_Owners_Code;
                o.CR_Cas_Sup_Owners_Commercial_Registration_No = i.CR_Cas_Sup_Owners_Commercial_Registration_No;
                o.CR_Cas_Sup_Owners_En_Long_Name = i.CR_Cas_Sup_Owners_En_Long_Name;
                o.CR_Cas_Sup_Owners_Fr_Long_Name = i.CR_Cas_Sup_Owners_Fr_Long_Name;
                o.CR_Cas_Sup_Owners_Lessor_Code = i.CR_Cas_Sup_Owners_Lessor_Code;
                o.CR_Cas_Sup_Owners_Reasons = i.CR_Cas_Sup_Owners_Reasons;
                o.CR_Cas_Sup_Owners_Sector = i.CR_Cas_Sup_Owners_Sector;
                o.CR_Cas_Sup_Owners_Status = i.CR_Cas_Sup_Owners_Status;
                o.CR_Mas_Com_Lessor = i.CR_Mas_Com_Lessor;
                o.CarsNumber = nbr;
                l.Add(o);
            }

            return View(l);
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
            var owners = db.CR_Cas_Sup_Owners.Where(c=>c.CR_Cas_Sup_Owners_Lessor_Code==LessorCode).Count();
            IQueryable<CR_Cas_Sup_Owners> cR_Cas_Sup_Owners = null;
            if (type == "All")
            {
                cR_Cas_Sup_Owners = db.CR_Cas_Sup_Owners.Where(x => x.CR_Cas_Sup_Owners_Lessor_Code == LessorCode)
                    .Include(c => c.CR_Mas_Com_Lessor);
            }
            else if (type == "D")
            {
                cR_Cas_Sup_Owners = db.CR_Cas_Sup_Owners.Where(x => x.CR_Cas_Sup_Owners_Lessor_Code == LessorCode && x.CR_Cas_Sup_Owners_Status == "D")
                    .Include(c => c.CR_Mas_Com_Lessor);
            }
            else if (type == "H")
            {
                cR_Cas_Sup_Owners = db.CR_Cas_Sup_Owners.Where(x => x.CR_Cas_Sup_Owners_Lessor_Code == LessorCode && x.CR_Cas_Sup_Owners_Status == "H")
                    .Include(c => c.CR_Mas_Com_Lessor);
            }
            else
            {
                cR_Cas_Sup_Owners = db.CR_Cas_Sup_Owners.Where(x => x.CR_Cas_Sup_Owners_Lessor_Code == LessorCode && x.CR_Cas_Sup_Owners_Status == "A").Include(c => c.CR_Mas_Com_Lessor);
            }
            

            List<OwnerCarsModel> l = new List<OwnerCarsModel>();
            foreach (var i in cR_Cas_Sup_Owners)
            {
                OwnerCarsModel o = new OwnerCarsModel();
                var nbr = db.CR_Cas_Sup_Car_Information.Where(c => c.CR_Cas_Sup_Car_Owner_Code == i.CR_Cas_Sup_Owners_Code).Count();
                o.CR_Cas_Sup_Car_Information = i.CR_Cas_Sup_Car_Information;
                o.CR_Cas_Sup_Owners_Ar_Long_Name = i.CR_Cas_Sup_Owners_Ar_Long_Name;
                o.CR_Cas_Sup_Owners_Code = i.CR_Cas_Sup_Owners_Code;
                o.CR_Cas_Sup_Owners_Commercial_Registration_No = i.CR_Cas_Sup_Owners_Commercial_Registration_No;
                o.CR_Cas_Sup_Owners_En_Long_Name = i.CR_Cas_Sup_Owners_En_Long_Name;
                o.CR_Cas_Sup_Owners_Fr_Long_Name = i.CR_Cas_Sup_Owners_Fr_Long_Name;
                o.CR_Cas_Sup_Owners_Lessor_Code = i.CR_Cas_Sup_Owners_Lessor_Code;
                o.CR_Cas_Sup_Owners_Reasons = i.CR_Cas_Sup_Owners_Reasons;
                o.CR_Cas_Sup_Owners_Sector = i.CR_Cas_Sup_Owners_Sector;
                o.CR_Cas_Sup_Owners_Status = i.CR_Cas_Sup_Owners_Status;
                o.CR_Mas_Com_Lessor = i.CR_Mas_Com_Lessor;
                o.CarsNumber = nbr;
                l.Add(o);
            }
            return PartialView(l);
        }


        public JsonResult CheckOwners()
        {
           string LessorCode = Session["LessorCode"].ToString();

            var owners = db.CR_Cas_Sup_Owners.Where(c => c.CR_Cas_Sup_Owners_Lessor_Code == LessorCode).Count();
            return Json(owners, JsonRequestBehavior.AllowGet);
        }
                // GET: CasOwners/Details/5
                public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_Owners cR_Cas_Sup_Owners = db.CR_Cas_Sup_Owners.Find(id);
            if (cR_Cas_Sup_Owners == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Sup_Owners);
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult Index_Post(String lang, String excelCall)
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
            if (!string.IsNullOrEmpty(lang))
            {
                if (Session["Language"].ToString() == "1")
                {
                    Session["Language"] = "2";
                    Session["Lang"] = "Arabic";
                }
                else
                {
                    if (Session["Language"].ToString() == "2")
                    {
                        Session["Language"] = "1";
                        Session["Lang"] = "English";
                    }
                }
            }


            List<OwnerCarsModel> l = new List<OwnerCarsModel>();
            if (!string.IsNullOrEmpty(excelCall))
            {

                var b = new System.Data.DataTable("Owners");

                b.Columns.Add("الملاحظات", typeof(string));
                b.Columns.Add("الحالة", typeof(string));
                b.Columns.Add("عدد السيارات", typeof(string));
                b.Columns.Add("الإسم", typeof(string));
                b.Columns.Add("الهوية", typeof(string));


                //var LessorCode = Session["LessorCode"].ToString();
                var cR_Cas_Sup_Owners = db.CR_Cas_Sup_Owners.Where(x => x.CR_Cas_Sup_Owners_Lessor_Code == LessorCode).Include(c => c.CR_Mas_Com_Lessor);


                foreach (var i in cR_Cas_Sup_Owners)
                {
                    OwnerCarsModel o = new OwnerCarsModel();
                    var nbr = db.CR_Cas_Sup_Car_Information.Where(c => c.CR_Cas_Sup_Car_Owner_Code == i.CR_Cas_Sup_Owners_Code).Count();
                    o.CR_Cas_Sup_Car_Information = i.CR_Cas_Sup_Car_Information;
                    o.CR_Cas_Sup_Owners_Ar_Long_Name = i.CR_Cas_Sup_Owners_Ar_Long_Name;
                    o.CR_Cas_Sup_Owners_Code = i.CR_Cas_Sup_Owners_Code;
                    o.CR_Cas_Sup_Owners_Commercial_Registration_No = i.CR_Cas_Sup_Owners_Commercial_Registration_No;
                    o.CR_Cas_Sup_Owners_En_Long_Name = i.CR_Cas_Sup_Owners_En_Long_Name;
                    o.CR_Cas_Sup_Owners_Fr_Long_Name = i.CR_Cas_Sup_Owners_Fr_Long_Name;
                    o.CR_Cas_Sup_Owners_Lessor_Code = i.CR_Cas_Sup_Owners_Lessor_Code;
                    o.CR_Cas_Sup_Owners_Reasons = i.CR_Cas_Sup_Owners_Reasons;
                    o.CR_Cas_Sup_Owners_Sector = i.CR_Cas_Sup_Owners_Sector;
                    o.CR_Cas_Sup_Owners_Status = i.CR_Cas_Sup_Owners_Status;
                    o.CR_Mas_Com_Lessor = i.CR_Mas_Com_Lessor;
                    o.CarsNumber = nbr;
                    l.Add(o);
                }
                if (l != null)
                {
                    foreach (var i in l)
                    {
                        b.Rows.Add(i.CR_Cas_Sup_Owners_Reasons, i.CR_Cas_Sup_Owners_Status, i.CarsNumber,
                                            i.CR_Cas_Sup_Owners_Ar_Long_Name, i.CR_Cas_Sup_Owners_Code);
                    }
                }
                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = b;
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Owners.xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return View(l);
        }



        public CR_Cas_Administrative_Procedures GetLastRecord(string Lessorcode, string ProcedureCode)
        {
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");


            var Lrecord = db.CR_Cas_Administrative_Procedures.Where(x => x.CR_Cas_Administrative_Procedures_Lessor == Lessorcode && x.CR_Cas_Administrative_Procedures_Code == ProcedureCode
            && x.CR_Cas_Administrative_Procedures_Year == y)
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


        public void SaveTracing(string LessorCode, string Sector, string ProceduresCode, string ProcedureType, string UserLogin, string TargetedAction, string Reasons)
        {

            CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var sector = "1";
            var autoInc = GetLastRecord(LessorCode, ProceduresCode);

            Ad.CR_Cas_Administrative_Procedures_No = y + "-" + sector + "-" + ProceduresCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
            Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
            Ad.CR_Cas_Administrative_Procedures_Year = y;
            Ad.CR_Cas_Administrative_Procedures_Sector = Sector;
            Ad.CR_Cas_Administrative_Procedures_Code = ProceduresCode;
            Ad.CR_Cas_Administrative_Int_Procedures_Code = int.Parse(ProceduresCode);
            Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
            Ad.CR_Cas_Administrative_Procedures_Targeted_Action = TargetedAction;
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




        // GET: CasOwners/Create
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
            ViewBag.CR_Cas_Sup_Owners_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo");
            return View();
        }

        // POST: CasOwners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Sup_Owners_Code,CR_Cas_Sup_Owners_Lessor_Code," +
            "CR_Cas_Sup_Owners_Commercial_Registration_No,CR_Cas_Sup_Owners_Sector,CR_Cas_Sup_Owners_Ar_Long_Name," +
            "CR_Cas_Sup_Owners_En_Long_Name,CR_Cas_Sup_Owners_Fr_Long_Name,CR_Cas_Sup_Owners_Status,CR_Cas_Sup_Owners_Reasons")]
            CR_Cas_Sup_Owners cR_Cas_Sup_Owners)
        {
            var LessorCode = Session["LessorCode"].ToString();
            var UserLogin = Session["UserLogin"].ToString();
            using (DbContextTransaction dbTran = db.Database.BeginTransaction())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var LrecordExitArabe = db.CR_Cas_Sup_Owners.Any(Lr => Lr.CR_Cas_Sup_Owners_Ar_Long_Name == cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Ar_Long_Name && Lr.CR_Cas_Sup_Owners_Lessor_Code == LessorCode);
                        var LrecordExitEnglish = db.CR_Cas_Sup_Owners.Any(Lr => Lr.CR_Cas_Sup_Owners_En_Long_Name == cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_En_Long_Name && Lr.CR_Cas_Sup_Owners_Lessor_Code == LessorCode);
                        var LrecordExistGovNo = db.CR_Cas_Sup_Owners.Any(Lr => Lr.CR_Cas_Sup_Owners_Code == cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Code &&
                        Lr.CR_Cas_Sup_Owners_Lessor_Code == LessorCode);
                        var branchGovNo = db.CR_Cas_Sup_Branch.Any(lr => lr.CR_Cas_Sup_Branch_Government_No == cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Code);

                        if (cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Ar_Long_Name != null && cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_En_Long_Name != null &&
                            !LrecordExitArabe && !LrecordExitEnglish && !LrecordExistGovNo && cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Code != null &&
                            cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Commercial_Registration_No != null && !branchGovNo)
                        {

                            /////////////////////////////Save Tracing///////////////////////
                            ///
                            SaveTracing(LessorCode, "1", "49", "I", UserLogin, cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Code, cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Reasons);
                            ///////////////////////////////////////////////////////////////
                            cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status = "A";
                            cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Lessor_Code = LessorCode;
                            cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Sector = "2";
                            db.CR_Cas_Sup_Owners.Add(cR_Cas_Sup_Owners);
                            db.SaveChanges();
                            dbTran.Commit();
                            TempData["TempModel"] = "Added";
                            return RedirectToAction("Create", "CasOwners");
                        }
                        else
                        {
                            if (cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Ar_Long_Name == null)
                                ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                            if (cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_En_Long_Name == null)
                                ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                            if (cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Code == null)
                                ViewBag.LRExistCode = "الرجاء إدخال بيانات الحقل";
                            if (cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Commercial_Registration_No == null)
                                ViewBag.LRExistCommercial = "الرجاء إدخال بيانات الحقل";

                            if (LrecordExistGovNo)
                            {
                                ViewBag.LRExistCode = "عفوا هذا الرقم مسجل من قبل";
                            }
                            if (LrecordExitArabe && db.CR_Cas_Sup_Owners.Any(b => b.CR_Cas_Sup_Owners_Status == "A" && b.CR_Cas_Sup_Owners_Ar_Long_Name == cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Ar_Long_Name))
                                ViewBag.LRExistAr = "عفوا المالك مسجل من قبل";
                            if (LrecordExitEnglish && db.CR_Cas_Sup_Owners.Any(b => b.CR_Cas_Sup_Owners_Status == "A" && b.CR_Cas_Sup_Owners_En_Long_Name == cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_En_Long_Name))
                                ViewBag.LRExistEn = "عفوا المالك مسجل من قبل";

                            if (LrecordExitArabe && db.CR_Cas_Sup_Owners.Any(b => b.CR_Cas_Sup_Owners_Status == "H" && b.CR_Cas_Sup_Owners_Ar_Long_Name == cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Ar_Long_Name))
                                ViewBag.LRExistAr = "عفوا المالك مسجل من قبل (موقوفة)";
                            if (LrecordExitEnglish && db.CR_Cas_Sup_Owners.Any(b => b.CR_Cas_Sup_Owners_Status == "H" && b.CR_Cas_Sup_Owners_En_Long_Name == cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_En_Long_Name))
                                ViewBag.LRExistEn = "عفوا المالك مسجل من قبل (موقوفة)";

                            if (LrecordExitArabe && db.CR_Cas_Sup_Owners.Any(b => b.CR_Cas_Sup_Owners_Status == "D" && b.CR_Cas_Sup_Owners_Ar_Long_Name == cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Ar_Long_Name))
                                ViewBag.LRExistAr = "عفوا المالك مسجل من قبل (محذوفة)";
                            if (LrecordExitEnglish && db.CR_Cas_Sup_Owners.Any(b => b.CR_Cas_Sup_Owners_Status == "D" && b.CR_Cas_Sup_Owners_En_Long_Name == cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_En_Long_Name))
                                ViewBag.LRExistEn = "عفوا المالك مسجل من قبل (محذوفة)";
                            if (branchGovNo)
                            {
                                ViewBag.LRExistCode = "هذا الرقم الحكومي موجود";
                            }

                        }
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    dbTran.Rollback();
                    throw ex;
                }
            }

            return View(cR_Cas_Sup_Owners);
        }

        // GET: CasOwners/Edit/5
        public ActionResult Edit(string LessorCode, string BeneficiaryCode)
        {
            var Lcode = "";
            var UserLogin = "";
            try
            {
                Lcode = Session["LessorCode"].ToString();
                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || Lcode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
            if (BeneficiaryCode == null || LessorCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_Owners cR_Cas_Sup_Owners = db.CR_Cas_Sup_Owners.FirstOrDefault(x => x.CR_Cas_Sup_Owners_Lessor_Code == LessorCode.Trim()
            && x.CR_Cas_Sup_Owners_Code == BeneficiaryCode);
            if (cR_Cas_Sup_Owners == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status == "A" ||
                    cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status == "Activated" ||
                    cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status == "1" ||
                    cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status == "Undeleted")
                {
                    ViewBag.stat = "حذف";
                    ViewBag.h = "إيقاف";
                }
                if ((cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status == "D" ||
                    cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status == "Deleted" ||
                    cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status == "0"))
                {
                    ViewBag.stat = "إسترجاع";
                    ViewBag.h = "إيقاف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status == "H" ||
                    cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status == "Hold" ||
                    cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status == "2")
                {
                    ViewBag.h = "تنشيط";
                    ViewBag.stat = "حذف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status == null)
                {
                    ViewBag.h = "إيقاف";
                    ViewBag.stat = "حذف";
                }
                var b = db.CR_Cas_Sup_Branch.FirstOrDefault(br => br.CR_Cas_Sup_Branch_Government_No == cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Code);
                if (b != null)
                {
                    TempData["BranchCode"] = b.CR_Cas_Sup_Branch_Code;     
                }

                var nbr = db.CR_Cas_Sup_Car_Information.Where(c => c.CR_Cas_Sup_Car_Owner_Code == cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Code && c.CR_Cas_Sup_Car_Status!="D").Count();
                if (nbr > 0)
                {
                    TempData["CarsNo"] = "True";
                    //return RedirectToAction("Index");
                }


                ViewBag.delete = cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status;

            }

            return View(cR_Cas_Sup_Owners);
        }

        // POST: CasOwners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Sup_Owners_Code,CR_Cas_Sup_Owners_Lessor_Code," +
            "CR_Cas_Sup_Owners_Commercial_Registration_No,CR_Cas_Sup_Owners_Sector,CR_Cas_Sup_Owners_Ar_Long_Name," +
            "CR_Cas_Sup_Owners_En_Long_Name,CR_Cas_Sup_Owners_Fr_Long_Name,CR_Cas_Sup_Owners_Status,CR_Cas_Sup_Owners_Reasons")]
            CR_Cas_Sup_Owners cR_Cas_Sup_Owners, string save, string delete, string hold)
        {
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {

                    using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var LessorCode = Session["LessorCode"].ToString();
                            var UserLogin = Session["UserLogin"].ToString();

                            var LrecordExitArabe = db.CR_Cas_Sup_Owners.Any(Lr => Lr.CR_Cas_Sup_Owners_Ar_Long_Name == cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Ar_Long_Name &&
                            Lr.CR_Cas_Sup_Owners_Code != cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Code);
                            var LrecordExitEnglish = db.CR_Cas_Sup_Beneficiary.Any(Lr => Lr.CR_Cas_Sup_Beneficiary_En_Long_Name == cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_En_Long_Name &&
                            Lr.CR_Cas_Sup_Beneficiary_Code != cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Code);

                            if (cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Ar_Long_Name != null && cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_En_Long_Name != null &&
                                !LrecordExitArabe && !LrecordExitEnglish && cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Code != null &&
                                cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Commercial_Registration_No != null)
                            {
                                /////////////////////////////Save Tracing///////////////////////
                                ///
                                SaveTracing(LessorCode, "1", "49", "U", UserLogin, cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Code, cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Reasons);
                                ///////////////////////////////////////////////////////////////
                                ///
                                db.Entry(cR_Cas_Sup_Owners).State = EntityState.Modified;
                                db.SaveChanges();
                                dbTran.Commit();
                                TempData["TempModif"] = "Added";
                                //System.Threading.Thread.Sleep(4000);
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                if (cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Ar_Long_Name == null)
                                    ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                                if (cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_En_Long_Name == null)
                                    ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                                if (cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Code == null)
                                    ViewBag.LRExistCode = "الرجاء إدخال بيانات الحقل";
                                if (cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Commercial_Registration_No == null)
                                    ViewBag.LRExistCommercial = "الرجاء إدخال بيانات الحقل";


                                if (LrecordExitArabe && db.CR_Cas_Sup_Beneficiary.Any(b => b.CR_Cas_Sup_Beneficiary_Status == "A" && b.CR_Cas_Sup_Beneficiary_Ar_Long_Name == cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Ar_Long_Name))
                                    ViewBag.LRExistAr = "عفوا هذا المستفيد مسجل من قبل";
                                if (LrecordExitEnglish && db.CR_Cas_Sup_Beneficiary.Any(b => b.CR_Cas_Sup_Beneficiary_Status == "A" && b.CR_Cas_Sup_Beneficiary_En_Long_Name == cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_En_Long_Name))
                                    ViewBag.LRExistEn = "عفوا هذا المستفيد مسجل من قبل";

                                if (LrecordExitArabe && db.CR_Cas_Sup_Beneficiary.Any(b => b.CR_Cas_Sup_Beneficiary_Status == "H" && b.CR_Cas_Sup_Beneficiary_Ar_Long_Name == cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Ar_Long_Name))
                                    ViewBag.LRExistAr = "عفوا هذا المستفيد مسجل من قبل (موقوفة)";
                                if (LrecordExitEnglish && db.CR_Cas_Sup_Beneficiary.Any(b => b.CR_Cas_Sup_Beneficiary_Status == "H" && b.CR_Cas_Sup_Beneficiary_En_Long_Name == cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_En_Long_Name))
                                    ViewBag.LRExistEn = "عفوا الماركة مسجل من قبل (موقوفة)";

                                if (LrecordExitArabe && db.CR_Cas_Sup_Beneficiary.Any(b => b.CR_Cas_Sup_Beneficiary_Status == "D" && b.CR_Cas_Sup_Beneficiary_Ar_Long_Name == cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Ar_Long_Name))
                                    ViewBag.LRExistAr = "عفوا هذا المستفيد مسجل من قبل (محذوفة)";
                                if (LrecordExitEnglish && db.CR_Cas_Sup_Beneficiary.Any(b => b.CR_Cas_Sup_Beneficiary_Status == "D" && b.CR_Cas_Sup_Beneficiary_En_Long_Name == cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_En_Long_Name))
                                    ViewBag.LRExistEn = "عفوا هذا المستفيد مسجل من قبل (محذوفة)";
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
                        var LessorCode = Session["LessorCode"].ToString();
                        var UserLogin = Session["UserLogin"].ToString();
                        /////////////////////////////Save Tracing///////////////////////
                        ///
                        SaveTracing(LessorCode, "1", "49", "D", UserLogin, cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Code, cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Reasons);
                        ////////////////////////////////////////////////////////////////
                        cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status = "D";
                        db.Entry(cR_Cas_Sup_Owners).State = EntityState.Modified;
                        db.SaveChanges();
                        dbTran.Commit();
                        TempData["TempModif"] = "Deleted";
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
                       
                        var LessorCode = Session["LessorCode"].ToString();
                        var UserLogin = Session["UserLogin"].ToString();
                        /////////////////////////////Save Tracing///////////////////////
                        ///
                        SaveTracing(LessorCode, "1", "49", "A", UserLogin, cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Code, cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Reasons);
                        ////////////////////////////////////////////////////////////////
                        cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status = "A";
                        db.Entry(cR_Cas_Sup_Owners).State = EntityState.Modified;
                        db.SaveChanges();
                        dbTran.Commit();
                        TempData["TempModif"] = "Activated";
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
                        var LessorCode = Session["LessorCode"].ToString();
                        var UserLogin = Session["UserLogin"].ToString();
                        /////////////////////////////Save Tracing///////////////////////
                        ///
                        SaveTracing(LessorCode, "1", "49", "H", UserLogin, cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Code, cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Reasons);
                        ////////////////////////////////////////////////////////////////
                        cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status = "H";
                        db.Entry(cR_Cas_Sup_Owners).State = EntityState.Modified;
                        db.SaveChanges();
                        dbTran.Commit();
                        TempData["TempModif"] = "Holded";
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

                        var LessorCode = Session["LessorCode"].ToString();
                        var UserLogin = Session["UserLogin"].ToString();
                        /////////////////////////////Save Tracing///////////////////////
                        ///
                        SaveTracing(LessorCode, "1", "49", "A", UserLogin, cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Code, cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Reasons);
                        /////////////////////////////////////////////////////////////
                        cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status = "A";
                        db.Entry(cR_Cas_Sup_Owners).State = EntityState.Modified;
                        db.SaveChanges();
                        dbTran.Commit();
                        TempData["TempModif"] = "Activated";
                        return RedirectToAction("Index");

                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }


            }
            if (cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status == "A" ||
            cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status == "Activated" ||
            cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status == "1" ||
            cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }
            if ((cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status == "D" ||
                 cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status == "Deleted" ||
                 cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
            }
            if (cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status == "H" ||
                cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status == "Hold" ||
                cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
            }
            if (cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }
            ViewBag.delete = cR_Cas_Sup_Owners.CR_Cas_Sup_Owners_Status;

            return View(cR_Cas_Sup_Owners);
        }

        // GET: CasOwners/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_Owners cR_Cas_Sup_Owners = db.CR_Cas_Sup_Owners.Find(id);
            if (cR_Cas_Sup_Owners == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Sup_Owners);
        }

        // POST: CasOwners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Cas_Sup_Owners cR_Cas_Sup_Owners = db.CR_Cas_Sup_Owners.Find(id);
            db.CR_Cas_Sup_Owners.Remove(cR_Cas_Sup_Owners);
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
