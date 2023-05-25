using RentCar.Models;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;

namespace RentCar.Controllers
{
    public class SupportingController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        public static string TaskCode;

        // GET: Supporting
        [ActionName("Index")]
        public ActionResult Index_Get()
        {
            TaskCode = "1402";
            Session["POS"] = "1402";
            var TypeList = new SelectList(new[] {
                                              new {ID="1",Name="حكومية"},
                                              new{ID="2",Name="تقنية معلومات"},
                                              new{ID="3",Name="بنوك"},
                                              new{ID="4",Name="تأمين"},
                                              new{ID="5",Name="صيانة"},
                                              new{ID="6",Name="إستشارات"},
                                              new{ID="7",Name="أخرى"},
                                              }, "ID", "Name", 1);
            ViewData["list"] = TypeList;

            if (Session["ST_1402_unhold"].ToString() != "true" || Session["ST_1402_hold"].ToString() != "true" && Session["ST_1402_undelete"].ToString() != "true" || Session["ST_1402_delete"].ToString() != "true")
            {
                var SuppLIst = from CR_Mas_Com_Supporting in db.CR_Mas_Com_Supporting
                               where CR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status != "H" && CR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status != "D" && CR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Lessor_Code == Session["LessorCode"].ToString()
                               select CR_Mas_Com_Supporting;
                return View(SuppLIst);
            }
            else
                if (Session["ST_1402_unhold"].ToString() != "true" || Session["ST_1402_hold"].ToString() != "true")
            {
                var SuppLIst = db.CR_Mas_Com_Supporting.Where(x => x.CR_Mas_Com_Supporting_Status != "H");
                return View(db.CR_Mas_Com_Supporting.ToList());
            }
            else if (Session["ST_1402_undelete"].ToString() != "true" || Session["ST_1402_delete"].ToString() != "true")
            {
                var SuppLIst = db.CR_Mas_Com_Supporting.Where(x => x.CR_Mas_Com_Supporting_Status != "D");
                return View(db.CR_Mas_Com_Supporting.ToList());
            }
            else
            {
                return View(db.CR_Mas_Com_Supporting.ToList());
            }

        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult Index_Post(String lang, String excelCall)
        {
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
                var suppTable = new System.Data.DataTable("teste");

                suppTable.Columns.Add("الملاحظات", typeof(string));
                suppTable.Columns.Add("الحالة", typeof(string));
                suppTable.Columns.Add("الإسم", typeof(string));
                suppTable.Columns.Add("الإسم المختصر", typeof(string));
                suppTable.Columns.Add("الرمز", typeof(string));
                var Lrecord = db.CR_Mas_Com_Supporting.ToList();

                if (Lrecord != null)
                {
                    foreach (var i in Lrecord)
                    {
                        suppTable.Rows.Add(i.CR_Mas_Com_Supporting_Reasons, i.CR_Mas_Com_Supporting_Status,
                            i.CR_Mas_Com_Supporting_Ar_Long_Name, i.CR_Mas_Com_Supporting_Ar_Short_Name, i.CR_Mas_Com_Supporting_Code);
                    }
                }
                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = suppTable;
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return View(db.CR_Mas_Com_Supporting.ToList());
        }

        //////// GET: Supporting/Details/5
        //////public ActionResult Details(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Com_Supporting cR_Mas_Com_Supporting = db.CR_Mas_Com_Supporting.Find(id);
        //////    if (cR_Mas_Com_Supporting == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Com_Supporting);
        //////}
        public CR_Mas_Com_Supporting GetLastRecord()
        {
            var LessorCode = Session["LessorCode"].ToString();
            var Lrecord = db.CR_Mas_Com_Supporting.Where(x => x.CR_Mas_Com_Supporting_Lessor_Code == LessorCode).Max(l => l.CR_Mas_Com_Supporting_Code);
            CR_Mas_Com_Supporting s = new CR_Mas_Com_Supporting();
            if (Lrecord != null)
            {
                int val = int.Parse(Lrecord) + 1;
                s.CR_Mas_Com_Supporting_Code = val.ToString();
            }
            else
            {
                s.CR_Mas_Com_Supporting_Code = "7001";
            }
            return s;
        }
        // GET: Supporting/Create
        public ActionResult Create()
        {
            var TypeList = new SelectList(new[] {
                                              new {ID="1",Name="حكومية"},
                                              new{ID="2",Name="تقنية معلومات"},
                                              new{ID="3",Name="بنوك"},
                                              new{ID="4",Name="تأمين"},
                                              new{ID="5",Name="صيانة"},
                                              new{ID="6",Name="إستشارات"},
                                              new{ID="7",Name="أخرى"},
                                              }, "ID", "Name", 1);
            ViewData["list"] = TypeList;
            ViewBag.CR_Mas_Com_Supporting_Type = new SelectList(db.CR_Mas_Com_Supporting, "CR_Mas_Com_Supporting_Type", "CR_Mas_Com_Supporting_Type");
            CR_Mas_Com_Supporting sup = new CR_Mas_Com_Supporting();
            sup = GetLastRecord();
            sup.CR_Mas_Com_Supporting_Status = "A";
            return View(sup);
        }

        // POST: Supporting/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Mas_Com_Supporting_Code, CR_Mas_Com_Supporting_Type, CR_Mas_Com_Supporting_Ar_Long_Name, " +
        "CR_Mas_Com_Supporting_Ar_Short_Name, CR_Mas_Com_Supporting_En_Long_Name,CR_Mas_Com_Supporting_En_Short_Name, CR_Mas_Com_Supporting_Mobile_Number," +
        " CR_Mas_Com_Supporting_Status, CR_Mas_Com_Supporting_Reasons,CR_Mas_Com_Supporting_Fr_Short_Name,CR_Mas_Com_Supporting_Fr_Long_Name")] CR_Mas_Com_Supporting cR_Mas_Com_Supporting, string list)
        {
            //tryvar LessorCode = Session["LessorCode"].ToString();
            //{
            var LessorCode = Session["LessorCode"].ToString();
            if (ModelState.IsValid)
            {
                var LrecordExitArabeLng = db.CR_Mas_Com_Supporting.Any(Lr => Lr.CR_Mas_Com_Supporting_Ar_Long_Name == cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Ar_Long_Name);
                var LrecordExitArabeSh = db.CR_Mas_Com_Supporting.Any(Lr => Lr.CR_Mas_Com_Supporting_Ar_Short_Name == cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Ar_Short_Name);
                var LrecordExitEnglishLng = db.CR_Mas_Com_Supporting.Any(Lr => Lr.CR_Mas_Com_Supporting_En_Long_Name == cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_En_Long_Name);
                var LrecordExitEnglishSh = db.CR_Mas_Com_Supporting.Any(Lr => Lr.CR_Mas_Com_Supporting_En_Short_Name == cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_En_Short_Name);
                var LrecordExitFrenchLng = db.CR_Mas_Com_Supporting.Any(Lr => Lr.CR_Mas_Com_Supporting_Fr_Long_Name == cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Fr_Long_Name);
                var LrecordExitFrenchSh = db.CR_Mas_Com_Supporting.Any(Lr => Lr.CR_Mas_Com_Supporting_Fr_Short_Name == cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Fr_Short_Name);

                if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Ar_Long_Name != null && cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Ar_Short_Name != null &&
                    cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_En_Long_Name != null && cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_En_Short_Name != null &&
                    cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Fr_Long_Name != null && cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Fr_Short_Name != null &&
                    !LrecordExitArabeLng && !LrecordExitArabeSh && !LrecordExitEnglishLng && !LrecordExitEnglishSh && !LrecordExitFrenchLng && !LrecordExitFrenchSh &&
                    cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Ar_Long_Name.Length >= 3 && cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Ar_Short_Name.Length >= 2 &&
                    cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_En_Long_Name.Length >= 3 && cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_En_Short_Name.Length >= 2 &&
                    cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Fr_Long_Name.Length >= 3 && cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Fr_Short_Name.Length >= 2)
                {
                    cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Code = GetLastRecord().CR_Mas_Com_Supporting_Code;
                    cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Lessor_Code = LessorCode;
                    cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Type = list;
                    db.CR_Mas_Com_Supporting.Add(cR_Mas_Com_Supporting);
                    db.SaveChanges();
                    cR_Mas_Com_Supporting = new CR_Mas_Com_Supporting();
                    cR_Mas_Com_Supporting = GetLastRecord();
                    cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status = "A";
                    cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Lessor_Code = LessorCode;
                    TempData["TempModel"] = "تم الحفظ بنجاح";
                    return RedirectToAction("Create", "Supporting");
                }
                else
                {
                    if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Ar_Long_Name == null)
                        ViewBag.LRExistArLng = "الرجاء إدخال بيانات الحقل";
                    if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Ar_Short_Name == null)
                        ViewBag.LRExistArSh = "الرجاء إدخال بيانات الحقل";
                    if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_En_Long_Name == null)
                        ViewBag.LRExistEnLng = "الرجاء إدخال بيانات الحقل";
                    if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_En_Short_Name == null)
                        ViewBag.LRExistEnSh = "الرجاء إدخال بيانات الحقل";
                    if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Fr_Long_Name == null)
                        ViewBag.LRExistFrLng = "الرجاء إدخال بيانات الحقل";
                    if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Fr_Short_Name == null)
                        ViewBag.LRExistFrSh = "الرجاء إدخال بيانات الحقل";
                    if (LrecordExitArabeLng)
                        ViewBag.LRExistArLng = "عفوا هذه الشركة موجودة";
                    if (LrecordExitArabeSh)
                        ViewBag.LRExistArSh = "عفوا هذه الشركة موجودة";
                    if (LrecordExitEnglishLng)
                        ViewBag.LRExistEnLng = "عفوا هذه الشركة موجودة";
                    if (LrecordExitEnglishSh)
                        ViewBag.LRExistEnSh = "عفوا هذه الشركة موجودة";
                    if (LrecordExitFrenchSh)
                        ViewBag.LRExistFrSh = "عفوا هذه الشركة موجودة";
                    if (LrecordExitFrenchLng)
                        ViewBag.LRExistFrLng = "عفوا هذه الشركة موجودة";
                    if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Ar_Long_Name != null && cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Ar_Long_Name.Length < 3)
                        ViewBag.LRExistArLng = "عفوا الحد الأدنى للحروف 3";
                    if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Ar_Short_Name != null && cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Ar_Short_Name.Length < 2)
                        ViewBag.LRExistArSh = "عفوا الحد الأدنى للحروف 2";
                    if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_En_Long_Name != null && cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_En_Long_Name.Length < 3)
                        ViewBag.LRExistEnLng = "عفوا الحد الأدنى للحروف 3";
                    if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_En_Short_Name != null && cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_En_Short_Name.Length < 2)
                        ViewBag.LRExistEnSh = "عفوا الحد الأدنى للحروف 2";
                    if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Fr_Short_Name != null && cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Fr_Short_Name.Length < 2)
                        ViewBag.LRExistFrSh = "عفوا الحد الأدنى للحروف 2";
                    if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Fr_Long_Name != null && cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Fr_Long_Name.Length < 3)
                        ViewBag.LRExistFrLng = "عفوا الحد الأدنى للحروف 3";
                }
            }
            //}
            //catch (Exception) { }
            var TypeList = new SelectList(new[] {
                                              new {ID="1",Name="حكومية"},
                                              new{ID="2",Name="تقنية معلومات"},
                                              new{ID="3",Name="بنوك"},
                                              new{ID="4",Name="تأمين"},
                                              new{ID="5",Name="صيانة"},
                                              new{ID="6",Name="إستشارات"},
                                              new{ID="7",Name="أخرى"},
                                              }, "ID", "Name", 1);
            ViewData["list"] = TypeList;
            return View(cR_Mas_Com_Supporting);
        }

        // GET: Supporting/Edit/5
        public ActionResult Edit(string id1, string id2)
        {
            if (id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Com_Supporting cR_Mas_Com_Supporting = db.CR_Mas_Com_Supporting.FirstOrDefault(x => x.CR_Mas_Com_Supporting_Code == id1 && x.CR_Mas_Com_Supporting_Lessor_Code == id2);
            if (cR_Mas_Com_Supporting == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status == "A" ||
                    cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status == "Activated" ||
                    cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status == "1" ||
                    cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status == "Undeleted")
                {
                    ViewBag.stat = "حذف";
                    ViewBag.h = "إيقاف";
                }
                if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status == "D" ||
                    cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status == "Deleted" ||
                    cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status == "0")
                {
                    ViewBag.stat = "إسترجاع";
                    ViewBag.h = "إيقاف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status == "H" ||
                    cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status == "Hold" ||
                    cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status == "2")
                {
                    ViewBag.h = "تنشيط";
                    ViewBag.stat = "حذف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status == null)
                {
                    ViewBag.h = "إيقاف";
                    ViewBag.stat = "حذف";
                }
                ViewBag.delete = cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status;
                ViewBag.CR_Mas_Com_Supporting_Type = new SelectList(db.CR_Mas_Com_Supporting, "CR_Mas_Com_Supporting_Type", "CR_Mas_Com_Supporting_Type");
                var TypeList = new SelectList(new[] {
                                              new {ID="1",Name="حكومية"},
                                              new{ID="2",Name="تقنية معلومات"},
                                              new{ID="3",Name="بنوك"},
                                              new{ID="4",Name="تأمين"},
                                              new{ID="5",Name="صيانة"},
                                              new{ID="6",Name="إستشارات"},
                                              new{ID="7",Name="أخرى"},
                                              }, "ID", "Name", 1);
                ViewData["list"] = TypeList;
            }
            return View(cR_Mas_Com_Supporting);
        }

        // POST: Supporting/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Mas_Com_Supporting_Code, CR_Mas_Com_Supporting_Type, CR_Mas_Com_Supporting_Ar_Long_Name, " +
        "CR_Mas_Com_Supporting_Ar_Short_Name, CR_Mas_Com_Supporting_En_Long_Name,CR_Mas_Com_Supporting_En_Short_Name, CR_Mas_Com_Supporting_Mobile_Number," +
        " CR_Mas_Com_Supporting_Status, CR_Mas_Com_Supporting_Reasons,CR_Mas_Com_Supporting_Mobile_Number,CR_Mas_Com_Supporting_Fr_Short_Name," +
        ", CR_Mas_Com_Supporting_Fr_Long_Name")] CR_Mas_Com_Supporting cR_Mas_Com_Supporting, string save, string delete, string hold, string list)
        {
            var LessorCode = Session["LessorCode"].ToString();
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabeLng = db.CR_Mas_Com_Supporting.Any(f => f.CR_Mas_Com_Supporting_Code != cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Code &&
                                                                          f.CR_Mas_Com_Supporting_Ar_Long_Name == cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Ar_Long_Name);
                    var LrecordExitArabeSh = db.CR_Mas_Com_Supporting.Any(f => f.CR_Mas_Com_Supporting_Code != cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Code &&
                                                                          f.CR_Mas_Com_Supporting_Ar_Short_Name == cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Ar_Short_Name);
                    var LrecordExitEnglishLng = db.CR_Mas_Com_Supporting.Any(f => f.CR_Mas_Com_Supporting_Code != cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Code &&
                                                                            f.CR_Mas_Com_Supporting_En_Long_Name == cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_En_Long_Name);
                    var LrecordExitEnglishSh = db.CR_Mas_Com_Supporting.Any(f => f.CR_Mas_Com_Supporting_Code != cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Code &&
                                                                            f.CR_Mas_Com_Supporting_En_Short_Name == cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_En_Short_Name);

                    var LrecordExitFrenchSh = db.CR_Mas_Com_Supporting.Any(f => f.CR_Mas_Com_Supporting_Code != cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Code &&
                                                                           f.CR_Mas_Com_Supporting_Fr_Short_Name == cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Fr_Short_Name);

                    var LrecordExitFrenchLng = db.CR_Mas_Com_Supporting.Any(f => f.CR_Mas_Com_Supporting_Code != cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Code &&
                                                                           f.CR_Mas_Com_Supporting_Fr_Long_Name == cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Fr_Long_Name);

                    if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Ar_Long_Name != null && cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Ar_Short_Name != null &&
                        cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_En_Long_Name != null && cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_En_Short_Name != null &&
                        cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Fr_Long_Name != null && cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Fr_Short_Name != null &&
                        cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Ar_Long_Name.Length >= 3 && cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_En_Long_Name.Length >= 3 &&
                         cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Ar_Short_Name.Length >= 3 && cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_En_Short_Name.Length >= 3 &&
                          cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Fr_Long_Name.Length >= 3 && cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Fr_Short_Name.Length >= 3 &&
                        !LrecordExitArabeLng && !LrecordExitArabeSh && !LrecordExitEnglishLng && !LrecordExitEnglishSh && !LrecordExitFrenchLng && !LrecordExitFrenchSh)
                    {
                        db.Entry(cR_Mas_Com_Supporting).State = EntityState.Modified;
                        cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Lessor_Code = LessorCode;
                        cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Type = list;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Ar_Long_Name == null)
                            ViewBag.LRExistArLng = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Ar_Short_Name == null)
                            ViewBag.LRExistArSh = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_En_Long_Name == null)
                            ViewBag.LRExistEnLng = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_En_Short_Name == null)
                            ViewBag.LRExistEnSh = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Fr_Long_Name == null)
                            ViewBag.LRExistFrLng = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Fr_Short_Name == null)
                            ViewBag.LRExistFrSh = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabeLng)
                            ViewBag.LRExistArLng = "عفوا هذه الشركة موجودة";
                        if (LrecordExitArabeSh)
                            ViewBag.LRExistArSh = "عفوا هذه الشركة موجودة";
                        if (LrecordExitEnglishLng)
                            ViewBag.LRExistEnLng = "عفوا هذه الشركة موجودة";
                        if (LrecordExitEnglishSh)
                            ViewBag.LRExistEnSh = "عفوا هذه الشركة موجودة";
                        if (LrecordExitFrenchSh)
                            ViewBag.LRExistFrSh = "عفوا هذه الشركة موجودة";
                        if (LrecordExitFrenchLng)
                            ViewBag.LRExistFrLng = "عفوا هذه الشركة موجودة";
                        if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Ar_Long_Name != null && cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Ar_Long_Name.Length < 3)
                            ViewBag.LRExistArLng = "عفوا الحد الأدنى للحروف 3";
                        if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Ar_Short_Name != null && cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Ar_Short_Name.Length < 2)
                            ViewBag.LRExistArSh = "عفوا الحد الأدنى للحروف 2";
                        if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_En_Long_Name != null && cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_En_Long_Name.Length < 3)
                            ViewBag.LRExistEnLng = "عفوا الحد الأدنى للحروف 3";
                        if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_En_Short_Name != null && cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_En_Short_Name.Length < 2)
                            ViewBag.LRExistEnSh = "عفوا الحد الأدنى للحروف 2";
                        if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Fr_Short_Name != null && cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Fr_Short_Name.Length < 2)
                            ViewBag.LRExistFrSh = "عفوا الحد الأدنى للحروف 2";
                        if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Fr_Long_Name != null && cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Fr_Long_Name.Length < 3)
                            ViewBag.LRExistFrLng = "عفوا الحد الأدنى للحروف 3";
                    }
                }
            }
            if (delete == "Delete" || delete == "حذف")
            {
                cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status = "D";
                cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Type = list;
                db.Entry(cR_Mas_Com_Supporting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (delete == "Activate" || delete == "إسترجاع")
            {
                cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status = "A";
                cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Type = list;
                db.Entry(cR_Mas_Com_Supporting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "إيقاف" || hold == "hold")
            {
                cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status = "H";
                cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Type = list;
                db.Entry(cR_Mas_Com_Supporting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "تنشيط" || hold == "Activate")
            {
                cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status = "A";
                cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Type = list;
                db.Entry(cR_Mas_Com_Supporting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status == "A" ||
            cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status == "Activated" ||
            cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status == "1" ||
            cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }
            if ((cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status == "D" ||
                 cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status == "Deleted" ||
                 cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
            }
            if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status == "H" ||
                cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status == "Hold" ||
                cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
            }
            if (cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }
            ViewBag.delete = cR_Mas_Com_Supporting.CR_Mas_Com_Supporting_Status;
            ViewBag.CR_Mas_Com_Supporting_Type = new SelectList(db.CR_Mas_Com_Supporting, "CR_Mas_Com_Supporting_Type", "CR_Mas_Com_Supporting_Type");
            var TypeList = new SelectList(new[] {
                                              new {ID="1",Name="حكومية"},
                                              new{ID="2",Name="تقنية معلومات"},
                                              new{ID="3",Name="بنوك"},
                                              new{ID="4",Name="تأمين"},
                                              new{ID="5",Name="صيانة"},
                                              new{ID="6",Name="إستشارات"},
                                              new{ID="7",Name="أخرى"},
                                              }, "ID", "Name", 1);
            ViewData["list"] = TypeList;
            return View(cR_Mas_Com_Supporting);
        }

        //////// GET: Supporting/Delete/5
        //////public ActionResult Delete(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Com_Supporting cR_Mas_Com_Supporting = db.CR_Mas_Com_Supporting.Find(id);
        //////    if (cR_Mas_Com_Supporting == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Com_Supporting);
        //////}

        //////// POST: Car_Features/Delete/5
        //////[HttpPost, ActionName("Delete")]
        //////[ValidateAntiForgeryToken]
        //////public ActionResult DeleteConfirmed(string id)
        //////{
        //////    CR_Mas_Com_Supporting cR_Mas_Com_Supporting = db.CR_Mas_Com_Supporting.Find(id);
        //////    db.CR_Mas_Com_Supporting.Remove(cR_Mas_Com_Supporting);
        //////    db.SaveChanges();
        //////    return RedirectToAction("Index");
        //////}
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