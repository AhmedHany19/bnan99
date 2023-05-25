using RentCar.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;

namespace RentCar.Controllers.CAS
{
    public class BeneficiaryController : Controller
    {
        public static string TaskCode;
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: Beneficiary
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

            var cR_Cas_Sup_Beneficiary = db.CR_Cas_Sup_Beneficiary.Where(x => x.CR_Cas_Sup_Beneficiary_Status == "A" && x.CR_Cas_Sup_Beneficiary_Lessor == LessorCode).Include(c => c.CR_Mas_Com_Lessor);
            return View(cR_Cas_Sup_Beneficiary.ToList());

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

            if (!string.IsNullOrEmpty(excelCall))
            {
                var b = new System.Data.DataTable("teste");

                b.Columns.Add("الملاحظات", typeof(string));
                b.Columns.Add("الحالة", typeof(string));
                b.Columns.Add("الإسم العربي", typeof(string));
                b.Columns.Add("القطاع", typeof(string));
                b.Columns.Add("السجل التجاري", typeof(string));
                b.Columns.Add("الشركة", typeof(string));
                b.Columns.Add("الرمز", typeof(string));
                var Lrecord = db.CR_Cas_Sup_Beneficiary.ToList();
                if (Lrecord != null)
                {
                    foreach (var i in Lrecord)
                    {
                        b.Rows.Add(i.CR_Cas_Sup_Beneficiary_Reasons, i.CR_Cas_Sup_Beneficiary_Status, i.CR_Cas_Sup_Beneficiary_Ar_Long_Name,
                                            i.CR_Cas_Sup_Beneficiary_Sector, i.CR_Cas_Sup_Beneficiary_Commercial_Registration_No, i.CR_Cas_Sup_Beneficiary_Lessor, i.CR_Cas_Sup_Beneficiary_Code);
                    }
                }
                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = b;
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Beneficiary.xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return View(db.CR_Cas_Sup_Beneficiary.ToList());
        }



        // GET: Beneficiary/Create
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
            ViewBag.CR_Cas_Sup_Beneficiary_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Ar_Short_Name");
            ViewBag.CR_Cas_Sup_Beneficiary_Sector = new SelectList(db.CR_Cas_Sup_Beneficiary, "CR_Cas_Sup_Beneficiary_Code", "CR_Cas_Sup_Beneficiary_Ar_Long_Name");
            return View();
        }

        // POST: Beneficiary/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Sup_Beneficiary_Code,CR_Cas_Sup_Beneficiary_Lessor," +
            "CR_Cas_Sup_Beneficiary_Commercial_Registration_No,CR_Cas_Sup_Beneficiary_Sector,CR_Cas_Sup_Beneficiary_Ar_Long_Name," +
            "CR_Cas_Sup_Beneficiary_En_Long_Name,CR_Cas_Sup_Beneficiary_Fr_Long_Name,CR_Cas_Sup_Beneficiary_Status," +
            "CR_Cas_Sup_Beneficiary_Reasons")] CR_Cas_Sup_Beneficiary cR_Cas_Sup_Beneficiary)
        {
            using (DbContextTransaction dbTran = db.Database.BeginTransaction())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var LessorCode = Session["LessorCode"].ToString();
                        var LrecordExitArabe = db.CR_Cas_Sup_Beneficiary.Any(Lr => Lr.CR_Cas_Sup_Beneficiary_Ar_Long_Name == cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Ar_Long_Name);
                        var LrecordExitEnglish = db.CR_Cas_Sup_Beneficiary.Any(Lr => Lr.CR_Cas_Sup_Beneficiary_En_Long_Name == cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_En_Long_Name);
                        var LrecordExistGovNo = db.CR_Cas_Sup_Beneficiary.Any(Lr => Lr.CR_Cas_Sup_Beneficiary_Code == cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Code);


                        if (cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Ar_Long_Name != null && cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_En_Long_Name != null &&
                            !LrecordExitArabe && !LrecordExitEnglish && !LrecordExistGovNo && cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Code != null &&
                            cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Commercial_Registration_No != null)
                        {
                            cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status = "A";
                            cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Lessor = LessorCode;
                            cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Sector = "2";
                            db.CR_Cas_Sup_Beneficiary.Add(cR_Cas_Sup_Beneficiary);
                            db.SaveChanges();
                            dbTran.Commit();
                            TempData["TempModel"] = "Added";
                            return RedirectToAction("Create", "Beneficiary");
                        }
                        else
                        {
                            if (cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Ar_Long_Name == null)
                                ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                            if (cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_En_Long_Name == null)
                                ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                            if (cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Code == null)
                                ViewBag.LRExistCode = "الرجاء إدخال بيانات الحقل";
                            if (cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Commercial_Registration_No == null)
                                ViewBag.LRExistCommercial = "الرجاء إدخال بيانات الحقل";

                            if (LrecordExistGovNo)
                            {
                                ViewBag.LRExistCode = "عفوا هذا الرقم مسجل من قبل";
                            }
                            if (LrecordExitArabe && db.CR_Cas_Sup_Beneficiary.Any(b => b.CR_Cas_Sup_Beneficiary_Status == "A" && b.CR_Cas_Sup_Beneficiary_Ar_Long_Name == cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Ar_Long_Name))
                                ViewBag.LRExistAr = "عفوا هذا المستفيد مسجل من قبل";
                            if (LrecordExitEnglish && db.CR_Cas_Sup_Beneficiary.Any(b => b.CR_Cas_Sup_Beneficiary_Status == "A" && b.CR_Cas_Sup_Beneficiary_En_Long_Name == cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_En_Long_Name))
                                ViewBag.LRExistEn = "عفوا هذا المستفيد مسجل من قبل";

                            if (LrecordExitArabe && db.CR_Cas_Sup_Beneficiary.Any(b => b.CR_Cas_Sup_Beneficiary_Status == "H" && b.CR_Cas_Sup_Beneficiary_Ar_Long_Name == cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Ar_Long_Name))
                                ViewBag.LRExistAr = "عفوا هذا المستفيد مسجل من قبل (موقوفة)";
                            if (LrecordExitEnglish && db.CR_Cas_Sup_Beneficiary.Any(b => b.CR_Cas_Sup_Beneficiary_Status == "H" && b.CR_Cas_Sup_Beneficiary_En_Long_Name == cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_En_Long_Name))
                                ViewBag.LRExistEn = "عفوا الماركة مسجل من قبل (موقوفة)";

                            if (LrecordExitArabe && db.CR_Cas_Sup_Beneficiary.Any(b => b.CR_Cas_Sup_Beneficiary_Status == "D" && b.CR_Cas_Sup_Beneficiary_Ar_Long_Name == cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Ar_Long_Name))
                                ViewBag.LRExistAr = "عفوا هذا المستفيد مسجل من قبل (محذوفة)";
                            if (LrecordExitEnglish && db.CR_Cas_Sup_Beneficiary.Any(b => b.CR_Cas_Sup_Beneficiary_Status == "D" && b.CR_Cas_Sup_Beneficiary_En_Long_Name == cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_En_Long_Name))
                                ViewBag.LRExistEn = "عفوا هذا المستفيد مسجل من قبل (محذوفة)";

                        }
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    dbTran.Rollback();
                    throw ex;

                    //return RedirectToAction("Create", "Brand");
                    ////CR_Mas_Sup_Brand brand = new CR_Mas_Sup_Brand();
                    //cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Code = GetLastRecord().CR_Mas_Sup_Brand_Code;
                    //cR_Mas_Sup_Brand.CR_Cas_Sup_Beneficiary_Status = "A";
                    //cR_Mas_Sup_Brand.CR_Cas_Sup_Beneficiary_Ar_Long_Name = "A";
                    //cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Fr_Name = "A";
                    //cR_Mas_Sup_Brand.CR_Cas_Sup_Beneficiary_En_Long_Name = "A";
                    //return View(cR_Mas_Sup_Brand);
                }
            }

            ViewBag.CR_Cas_Sup_Beneficiary_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Ar_Short_Name", cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Lessor);
            ViewBag.CR_Cas_Sup_Beneficiary_Sector = new SelectList(db.CR_Cas_Sup_Beneficiary, "CR_Cas_Sup_Beneficiary_Code", "CR_Cas_Sup_Beneficiary_Ar_Long_Name", cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Code);
            return View(cR_Cas_Sup_Beneficiary);
        }

        // GET: Beneficiary/Edit/5
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
            CR_Cas_Sup_Beneficiary cR_Cas_Sup_Beneficiary = db.CR_Cas_Sup_Beneficiary.FirstOrDefault(x => x.CR_Cas_Sup_Beneficiary_Lessor == LessorCode.Trim()
            && x.CR_Cas_Sup_Beneficiary_Code == BeneficiaryCode);
            if (cR_Cas_Sup_Beneficiary == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status == "A" ||
                    cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status == "Activated" ||
                    cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status == "1" ||
                    cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status == "Undeleted")
                {
                    ViewBag.stat = "حذف";
                    ViewBag.h = "إيقاف";
                }
                if ((cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status == "D" ||
                    cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status == "Deleted" ||
                    cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status == "0"))
                {
                    ViewBag.stat = "إسترجاع";
                    ViewBag.h = "إيقاف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status == "H" ||
                    cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status == "Hold" ||
                    cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status == "2")
                {
                    ViewBag.h = "تنشيط";
                    ViewBag.stat = "حذف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status == null)
                {
                    ViewBag.h = "إيقاف";
                    ViewBag.stat = "حذف";
                }
                ViewBag.delete = cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status;

            }
            ViewBag.CR_Cas_Sup_Beneficiary_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Ar_Short_Name", cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Lessor);
            ViewBag.CR_Cas_Sup_Beneficiary_Sector = new SelectList(db.CR_Cas_Sup_Beneficiary, "CR_Cas_Sup_Beneficiary_Code", "CR_Cas_Sup_Beneficiary_Ar_Long_Name", cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Code);
            return View(cR_Cas_Sup_Beneficiary);
        }

        // POST: Beneficiary/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Sup_Beneficiary_Code,CR_Cas_Sup_Beneficiary_Lessor," +
            "CR_Cas_Sup_Beneficiary_Commercial_Registration_No,CR_Cas_Sup_Beneficiary_Sector,CR_Cas_Sup_Beneficiary_Ar_Long_Name," +
            "CR_Cas_Sup_Beneficiary_En_Long_Name,CR_Cas_Sup_Beneficiary_Fr_Long_Name,CR_Cas_Sup_Beneficiary_Status,CR_Cas_Sup_Beneficiary_Reasons")]
            CR_Cas_Sup_Beneficiary cR_Cas_Sup_Beneficiary, string save, string delete, string hold)
        {
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var LrecordExitArabe = db.CR_Cas_Sup_Beneficiary.Any(Lr => Lr.CR_Cas_Sup_Beneficiary_Ar_Long_Name == cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Ar_Long_Name &&
                             Lr.CR_Cas_Sup_Beneficiary_Code != cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Code);
                            var LrecordExitEnglish = db.CR_Cas_Sup_Beneficiary.Any(Lr => Lr.CR_Cas_Sup_Beneficiary_En_Long_Name == cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_En_Long_Name &&
                            Lr.CR_Cas_Sup_Beneficiary_Code != cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Code);

                            if (cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Ar_Long_Name != null && cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_En_Long_Name != null &&
                                !LrecordExitArabe && !LrecordExitEnglish && cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Code != null &&
                                cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Commercial_Registration_No != null)
                            {
                                db.Entry(cR_Cas_Sup_Beneficiary).State = EntityState.Modified;
                                db.SaveChanges();
                                dbTran.Commit();
                                TempData["TempModif"] = "Saved";
                                //System.Threading.Thread.Sleep(4000);
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                if (cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Ar_Long_Name == null)
                                    ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                                if (cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_En_Long_Name == null)
                                    ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                                if (cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Code == null)
                                    ViewBag.LRExistCode = "الرجاء إدخال بيانات الحقل";
                                if (cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Commercial_Registration_No == null)
                                    ViewBag.LRExistCommercial = "الرجاء إدخال بيانات الحقل";

                                if (LrecordExitArabe && db.CR_Cas_Sup_Beneficiary.Any(b => b.CR_Cas_Sup_Beneficiary_Status == "A" && b.CR_Cas_Sup_Beneficiary_Ar_Long_Name == cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Ar_Long_Name))
                                    ViewBag.LRExistAr = "عفوا هذا المستفيد مسجل من قبل";
                                if (LrecordExitEnglish && db.CR_Cas_Sup_Beneficiary.Any(b => b.CR_Cas_Sup_Beneficiary_Status == "A" && b.CR_Cas_Sup_Beneficiary_En_Long_Name == cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_En_Long_Name))
                                    ViewBag.LRExistEn = "عفوا هذا المستفيد مسجل من قبل";

                                if (LrecordExitArabe && db.CR_Cas_Sup_Beneficiary.Any(b => b.CR_Cas_Sup_Beneficiary_Status == "H" && b.CR_Cas_Sup_Beneficiary_Ar_Long_Name == cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Ar_Long_Name))
                                    ViewBag.LRExistAr = "عفوا هذا المستفيد مسجل من قبل (موقوفة)";
                                if (LrecordExitEnglish && db.CR_Cas_Sup_Beneficiary.Any(b => b.CR_Cas_Sup_Beneficiary_Status == "H" && b.CR_Cas_Sup_Beneficiary_En_Long_Name == cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_En_Long_Name))
                                    ViewBag.LRExistEn = "عفوا الماركة مسجل من قبل (موقوفة)";

                                if (LrecordExitArabe && db.CR_Cas_Sup_Beneficiary.Any(b => b.CR_Cas_Sup_Beneficiary_Status == "D" && b.CR_Cas_Sup_Beneficiary_Ar_Long_Name == cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Ar_Long_Name))
                                    ViewBag.LRExistAr = "عفوا هذا المستفيد مسجل من قبل (محذوفة)";
                                if (LrecordExitEnglish && db.CR_Cas_Sup_Beneficiary.Any(b => b.CR_Cas_Sup_Beneficiary_Status == "D" && b.CR_Cas_Sup_Beneficiary_En_Long_Name == cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_En_Long_Name))
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

                        cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status = "D";
                        db.Entry(cR_Cas_Sup_Beneficiary).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["TempModif"] = "تم الحذف بنجاح";
                        dbTran.Commit();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }

                return RedirectToAction("Index");
            }
            if (delete == "Activate" || delete == "إسترجاع")
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status = "A";
                        db.Entry(cR_Cas_Sup_Beneficiary).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["TempModif"] = "Activated";

                        dbTran.Commit();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }

                return RedirectToAction("Index");
            }
            if (hold == "إيقاف" || hold == "hold")
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status = "H";
                        db.Entry(cR_Cas_Sup_Beneficiary).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["TempModif"] = "Holded";

                        dbTran.Commit();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }

                return RedirectToAction("Index");
            }
            if (hold == "تنشيط" || hold == "Activate")
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status = "A";
                        db.Entry(cR_Cas_Sup_Beneficiary).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["TempModif"] = "Activated";

                        dbTran.Commit();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }

                return RedirectToAction("Index");
            }
            if (cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status == "A" ||
            cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status == "Activated" ||
            cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status == "1" ||
            cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }
            if ((cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status == "D" ||
                 cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status == "Deleted" ||
                 cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
            }
            if (cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status == "H" ||
                cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status == "Hold" ||
                cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
            }
            if (cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }
            ViewBag.delete = cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Status;
            ViewBag.CR_Cas_Sup_Beneficiary_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Ar_Short_Name", cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Lessor);
            ViewBag.CR_Cas_Sup_Beneficiary_Sector = new SelectList(db.CR_Cas_Sup_Beneficiary, "CR_Cas_Sup_Beneficiary_Code", "CR_Cas_Sup_Beneficiary_Ar_Long_Name", cR_Cas_Sup_Beneficiary.CR_Cas_Sup_Beneficiary_Code);
            return View(cR_Cas_Sup_Beneficiary);
        }

        // GET: Beneficiary/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_Beneficiary cR_Cas_Sup_Beneficiary = db.CR_Cas_Sup_Beneficiary.Find(id);
            if (cR_Cas_Sup_Beneficiary == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Sup_Beneficiary);
        }

        // POST: Beneficiary/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Cas_Sup_Beneficiary cR_Cas_Sup_Beneficiary = db.CR_Cas_Sup_Beneficiary.Find(id);
            db.CR_Cas_Sup_Beneficiary.Remove(cR_Cas_Sup_Beneficiary);
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
