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
    public class ModelController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        public static string TaskCode;
        // GET: Model
        public ActionResult Index()
        {
            TaskCode = "1502";
            Session["POS"] = "1502";
            if (Session["ST_1502_unhold"].ToString() != "true" || Session["ST_1502_hold"].ToString() != "true" && Session["ST_1502_undelete"].ToString() != "true" || Session["ST_1502_delete"].ToString() != "true")
            {
                var cR_Mas_Sup_Model = db.CR_Mas_Sup_Model.Include(c => c.CR_Mas_Sup_Brand).Include(c => c.CR_Mas_Sup_Group).Where(stat => stat.CR_Mas_Sup_Model_Status != "H" && stat.CR_Mas_Sup_Model_Status != "D");
                return View(cR_Mas_Sup_Model.ToList());
            }
            else
                if (Session["ST_1502_unhold"].ToString() != "true" || Session["ST_1502_hold"].ToString() != "true")
            {
                var cR_Mas_Sup_Model = db.CR_Mas_Sup_Model.Include(c => c.CR_Mas_Sup_Brand).Include(c => c.CR_Mas_Sup_Group).Where(stat => stat.CR_Mas_Sup_Model_Status != "H");
                return View(cR_Mas_Sup_Model.ToList());
            }
            else if (Session["ST_1502_undelete"].ToString() != "true" || Session["ST_1502_delete"].ToString() != "true")
            {
                var cR_Mas_Sup_Model = db.CR_Mas_Sup_Model.Include(c => c.CR_Mas_Sup_Brand).Include(c => c.CR_Mas_Sup_Group).Where(stat => stat.CR_Mas_Sup_Model_Status != "D");
                return View(cR_Mas_Sup_Model.ToList());
            }
            else
            {
                var cR_Mas_Sup_Model = db.CR_Mas_Sup_Model.Include(c => c.CR_Mas_Sup_Brand).Include(c => c.CR_Mas_Sup_Group);
                return View(cR_Mas_Sup_Model.ToList());
            }
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult Index_Post(string excelCall, string lang)
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
                var brandTable = new System.Data.DataTable("teste");

                brandTable.Columns.Add("الملاحظات", typeof(string));
                brandTable.Columns.Add("الحالة", typeof(string));
                brandTable.Columns.Add("المجموعة", typeof(string));
                brandTable.Columns.Add("الماركة", typeof(string));
                brandTable.Columns.Add("رقم العداد", typeof(string));
                brandTable.Columns.Add("الإسم", typeof(string));
                brandTable.Columns.Add("الرمز", typeof(string));
                var Lrecord = db.CR_Mas_Sup_Model.ToList();

                if (Lrecord != null)
                {
                    foreach (var i in Lrecord)
                    {
                        brandTable.Rows.Add(i.CR_Mas_Sup_Model_Reasons, i.CR_Mas_Sup_Model_Status, i.CR_Mas_Sup_Model_Group_Code, i.CR_Mas_Sup_Model_Brand_Code, i.CR_Mas_Sup_Model_Counter, i.CR_Mas_Sup_Model_Ar_Name, i.CR_Mas_Sup_Model_Code);
                    }
                }

                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = brandTable;
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=ModelDataTable.xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return View(db.CR_Mas_Sup_Model.ToList());
        }

        //////// GET: Model/Details/5
        //////public ActionResult Details(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Sup_Model cR_Mas_Sup_Model = db.CR_Mas_Sup_Model.Find(id);
        //////    if (cR_Mas_Sup_Model == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Sup_Model);
        //////}
        public CR_Mas_Sup_Model GetLastRecord()
        {
            var Lrecord = db.CR_Mas_Sup_Model.Max(Lr => Lr.CR_Mas_Sup_Model_Code);
            CR_Mas_Sup_Model m = new CR_Mas_Sup_Model();
            if (Lrecord != null)
            {
                Int64 val = Int64.Parse(Lrecord) + 1;
                m.CR_Mas_Sup_Model_Code = val.ToString();
            }
            else
            {
                m.CR_Mas_Sup_Model_Code = "3100000001";
            }
            return m;
        }
        // GET: Model/Create
        public ActionResult Create()
        {

            if (Session["ST_1502_unhold"].ToString() != "true" || Session["ST_1502_hold"].ToString() != "true" && Session["ST_1502_undelete"].ToString() != "true" || Session["ST_1502_delete"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_Model_Brand_Code = new SelectList(db.CR_Mas_Sup_Brand.Where(x => x.CR_Mas_Sup_Brand_Status != "D" && x.CR_Mas_Sup_Brand_Status != "H"), "CR_Mas_Sup_Brand_Code", "CR_Mas_Sup_Brand_Ar_Name");
            }
            else
                if (Session["ST_1502_unhold"].ToString() != "true" || Session["ST_1502_hold"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_Model_Brand_Code = new SelectList(db.CR_Mas_Sup_Brand.Where(x => x.CR_Mas_Sup_Brand_Status != "H"), "CR_Mas_Sup_Brand_Code", "CR_Mas_Sup_Brand_Ar_Name");
            }
            else if (Session["ST_1502_undelete"].ToString() != "true" || Session["ST_1502_delete"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_Model_Brand_Code = new SelectList(db.CR_Mas_Sup_Brand.Where(x => x.CR_Mas_Sup_Brand_Status != "D"), "CR_Mas_Sup_Brand_Code", "CR_Mas_Sup_Brand_Ar_Name");
            }
            else
            {
                ViewBag.CR_Mas_Sup_Model_Brand_Code = new SelectList(db.CR_Mas_Sup_Brand, "CR_Mas_Sup_Brand_Code", "CR_Mas_Sup_Brand_Ar_Name");
            }

            ViewBag.CR_Mas_Sup_Model_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code", "CR_Mas_Sup_Group_Ar_Name");
            CR_Mas_Sup_Model mod = new CR_Mas_Sup_Model();
            mod = GetLastRecord();
            mod.CR_Mas_Sup_Model_Status = "A";
            return View(mod);
        }

        // POST: Model/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Mas_Sup_Model_Code, CR_Mas_Sup_Model_Group_Code, CR_Mas_Sup_Model_Brand_Code, CR_Mas_Sup_Model_Ar_Name, " +
        "CR_Mas_Sup_Model_En_Name, CR_Mas_Sup_Model_Fr_Name, CR_Mas_Sup_Model_Counter, CR_Mas_Sup_Model_Status, CR_Mas_Sup_Model_Reasons")] CR_Mas_Sup_Model
         cR_Mas_Sup_Model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_Sup_Model.Any(Lr => Lr.CR_Mas_Sup_Model_Ar_Name == cR_Mas_Sup_Model.CR_Mas_Sup_Model_Ar_Name);
                    var LrecordExitEnglish = db.CR_Mas_Sup_Model.Any(Lr => Lr.CR_Mas_Sup_Model_En_Name == cR_Mas_Sup_Model.CR_Mas_Sup_Model_En_Name);
                    var LrecordExitFrench = db.CR_Mas_Sup_Model.Any(Lr => Lr.CR_Mas_Sup_Model_Fr_Name == cR_Mas_Sup_Model.CR_Mas_Sup_Model_Fr_Name);


                    if (cR_Mas_Sup_Model.CR_Mas_Sup_Model_Ar_Name != null && cR_Mas_Sup_Model.CR_Mas_Sup_Model_En_Name != null &&
                        cR_Mas_Sup_Model.CR_Mas_Sup_Model_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench &&
                        cR_Mas_Sup_Model.CR_Mas_Sup_Model_Ar_Name.Length >= 3 && cR_Mas_Sup_Model.CR_Mas_Sup_Model_En_Name.Length >= 3 &&
                        cR_Mas_Sup_Model.CR_Mas_Sup_Model_Fr_Name.Length >= 3)
                    {
                        cR_Mas_Sup_Model.CR_Mas_Sup_Model_Code = GetLastRecord().CR_Mas_Sup_Model_Code;
                        cR_Mas_Sup_Model.CR_Mas_Sup_Model_Group_Code = "31";
                        db.CR_Mas_Sup_Model.Add(cR_Mas_Sup_Model);
                        db.SaveChanges();
                        cR_Mas_Sup_Model = new CR_Mas_Sup_Model();
                        cR_Mas_Sup_Model = GetLastRecord();
                        cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status = "A";
                        TempData["TempModel"] = "تم الحفظ بنجاح";
                        return RedirectToAction("Create", "Model");
                    }
                    else
                    {
                        if (cR_Mas_Sup_Model.CR_Mas_Sup_Model_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Model.CR_Mas_Sup_Model_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Model.CR_Mas_Sup_Model_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Model.Any(m => m.CR_Mas_Sup_Model_Status == "A" && m.CR_Mas_Sup_Model_Ar_Name == cR_Mas_Sup_Model.CR_Mas_Sup_Model_Ar_Name))
                            ViewBag.LRExistAr = "عفوا الطراز مسجل من قبل";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Model.Any(m => m.CR_Mas_Sup_Model_Status == "A" && m.CR_Mas_Sup_Model_En_Name == cR_Mas_Sup_Model.CR_Mas_Sup_Model_En_Name))
                            ViewBag.LRExistEn = "عفوا الطراز مسجل من قبل";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Model.Any(m => m.CR_Mas_Sup_Model_Status == "A" && m.CR_Mas_Sup_Model_Fr_Name == cR_Mas_Sup_Model.CR_Mas_Sup_Model_Fr_Name))
                            ViewBag.LRExistFr = "عفوا الطراز مسجل من قبل";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Model.Any(m => m.CR_Mas_Sup_Model_Status == "H" && m.CR_Mas_Sup_Model_Ar_Name == cR_Mas_Sup_Model.CR_Mas_Sup_Model_Ar_Name))
                            ViewBag.LRExistAr = "عفوا الطراز مسجل من قبل (موقوف)";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Model.Any(m => m.CR_Mas_Sup_Model_Status == "H" && m.CR_Mas_Sup_Model_En_Name == cR_Mas_Sup_Model.CR_Mas_Sup_Model_En_Name))
                            ViewBag.LRExistEn = "عفوا الطراز مسجل من قبل (موقوف)";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Model.Any(m => m.CR_Mas_Sup_Model_Status == "H" && m.CR_Mas_Sup_Model_Fr_Name == cR_Mas_Sup_Model.CR_Mas_Sup_Model_Fr_Name))
                            ViewBag.LRExistFr = "عفوا الطراز مسجل من قبل (موقوف)";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Model.Any(m => m.CR_Mas_Sup_Model_Status == "D" && m.CR_Mas_Sup_Model_Ar_Name == cR_Mas_Sup_Model.CR_Mas_Sup_Model_Ar_Name))
                            ViewBag.LRExistAr = "عفوا الطراز مسجل من قبل (محذوف)";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Model.Any(m => m.CR_Mas_Sup_Model_Status == "D" && m.CR_Mas_Sup_Model_En_Name == cR_Mas_Sup_Model.CR_Mas_Sup_Model_En_Name))
                            ViewBag.LRExistEn = "عفوا الطراز مسجل من قبل (محذوف)";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Model.Any(m => m.CR_Mas_Sup_Model_Status == "D" && m.CR_Mas_Sup_Model_Fr_Name == cR_Mas_Sup_Model.CR_Mas_Sup_Model_Fr_Name))
                            ViewBag.LRExistFr = "عفوا الطراز مسجل من قبل (محذوف)";
                        if (cR_Mas_Sup_Model.CR_Mas_Sup_Model_Ar_Name != null && cR_Mas_Sup_Model.CR_Mas_Sup_Model_Ar_Name.Length < 3)
                            ViewBag.LRExistAr = "الحد الأدنى ٣ حروف";
                        if (cR_Mas_Sup_Model.CR_Mas_Sup_Model_En_Name != null && cR_Mas_Sup_Model.CR_Mas_Sup_Model_En_Name.Length < 3)
                            ViewBag.LRExistEn = "الحد الأدنى ٣ حروف";
                        if (cR_Mas_Sup_Model.CR_Mas_Sup_Model_Fr_Name != null && cR_Mas_Sup_Model.CR_Mas_Sup_Model_Fr_Name.Length < 3)
                            ViewBag.LRExistFr = "الحد الأدنى ٣ حروف";
                    }
                }
            }
            catch (Exception) { }
            ViewBag.CR_Mas_Sup_Model_Brand_Code = new SelectList(db.CR_Mas_Sup_Brand, "CR_Mas_Sup_Brand_Code", "CR_Mas_Sup_Brand_Ar_Name",
                                                                 cR_Mas_Sup_Model.CR_Mas_Sup_Model_Brand_Code);
            ViewBag.CR_Mas_Sup_Model_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code", "CR_Mas_Sup_Group_Ar_Name",
                                                                 cR_Mas_Sup_Model.CR_Mas_Sup_Model_Group_Code);
            return View(cR_Mas_Sup_Model);
        }

        // GET: Model/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Sup_Model cR_Mas_Sup_Model = db.CR_Mas_Sup_Model.Find(id);
            if (cR_Mas_Sup_Model == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status == "A" || cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status == "1")
                {
                    ViewBag.stat = "حذف";
                    ViewBag.h = "إيقاف";
                }
                if ((cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status == "D" || cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status == "0"))
                {
                    ViewBag.stat = "إسترجاع";
                    ViewBag.h = "إيقاف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status == "H" || cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status == "2")
                {
                    ViewBag.h = "تنشيط";
                    ViewBag.stat = "حذف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status == null)
                {
                    ViewBag.h = "إيقاف";
                    ViewBag.stat = "حذف";
                }
                ViewBag.delete = cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status;
            }

            ViewBag.CR_Mas_Sup_Model_Brand_Code = new SelectList(db.CR_Mas_Sup_Brand, "CR_Mas_Sup_Brand_Code", "CR_Mas_Sup_Brand_Ar_Name",
                                                                 cR_Mas_Sup_Model.CR_Mas_Sup_Model_Brand_Code);
            ViewBag.CR_Mas_Sup_Model_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code", "CR_Mas_Sup_Group_Ar_Name",
                                                                 cR_Mas_Sup_Model.CR_Mas_Sup_Model_Group_Code);
            return View(cR_Mas_Sup_Model);
        }

        // POST: Model/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Mas_Sup_Model_Code, CR_Mas_Sup_Model_Group_Code, CR_Mas_Sup_Model_Brand_Code, " +
        "CR_Mas_Sup_Model_Ar_Name, CR_Mas_Sup_Model_En_Name, CR_Mas_Sup_Model_Fr_Name, CR_Mas_Sup_Model_Counter, CR_Mas_Sup_Model_Status, " +
        "CR_Mas_Sup_Model_Reasons")] CR_Mas_Sup_Model cR_Mas_Sup_Model, string save, string delete, string hold)
        {
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_Sup_Model.Any(m => m.CR_Mas_Sup_Model_Code != cR_Mas_Sup_Model.CR_Mas_Sup_Model_Code &&
                                                                   m.CR_Mas_Sup_Model_Ar_Name == cR_Mas_Sup_Model.CR_Mas_Sup_Model_Ar_Name);
                    var LrecordExitEnglish = db.CR_Mas_Sup_Model.Any(m => m.CR_Mas_Sup_Model_Code != cR_Mas_Sup_Model.CR_Mas_Sup_Model_Code &&
                                                                     m.CR_Mas_Sup_Model_En_Name == cR_Mas_Sup_Model.CR_Mas_Sup_Model_En_Name);
                    var LrecordExitFrench = db.CR_Mas_Sup_Model.Any(m => m.CR_Mas_Sup_Model_Code != cR_Mas_Sup_Model.CR_Mas_Sup_Model_Code &&
                                                                    m.CR_Mas_Sup_Model_Fr_Name == cR_Mas_Sup_Model.CR_Mas_Sup_Model_Fr_Name);

                    if (cR_Mas_Sup_Model.CR_Mas_Sup_Model_Ar_Name != null && cR_Mas_Sup_Model.CR_Mas_Sup_Model_En_Name != null &&
                        cR_Mas_Sup_Model.CR_Mas_Sup_Model_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench &&
                        cR_Mas_Sup_Model.CR_Mas_Sup_Model_Ar_Name.Length >= 3 && cR_Mas_Sup_Model.CR_Mas_Sup_Model_En_Name.Length >= 3 &&
                        cR_Mas_Sup_Model.CR_Mas_Sup_Model_Fr_Name.Length >= 3)
                    {
                        db.Entry(cR_Mas_Sup_Model).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (cR_Mas_Sup_Model.CR_Mas_Sup_Model_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Model.CR_Mas_Sup_Model_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Model.CR_Mas_Sup_Model_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Model.Any(m => m.CR_Mas_Sup_Model_Status == "A" && m.CR_Mas_Sup_Model_Ar_Name == cR_Mas_Sup_Model.CR_Mas_Sup_Model_Ar_Name))
                            ViewBag.LRExistAr = "عفوا الطراز مسجل من قبل";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Model.Any(m => m.CR_Mas_Sup_Model_Status == "A" && m.CR_Mas_Sup_Model_En_Name == cR_Mas_Sup_Model.CR_Mas_Sup_Model_En_Name))
                            ViewBag.LRExistEn = "عفوا الطراز مسجل من قبل";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Model.Any(m => m.CR_Mas_Sup_Model_Status == "A" && m.CR_Mas_Sup_Model_Fr_Name == cR_Mas_Sup_Model.CR_Mas_Sup_Model_Fr_Name))
                            ViewBag.LRExistFr = "عفوا الطراز مسجل من قبل";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Model.Any(m => m.CR_Mas_Sup_Model_Status == "H" && m.CR_Mas_Sup_Model_Ar_Name == cR_Mas_Sup_Model.CR_Mas_Sup_Model_Ar_Name))
                            ViewBag.LRExistAr = "عفوا الطراز مسجل من قبل (موقوف)";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Model.Any(m => m.CR_Mas_Sup_Model_Status == "H" && m.CR_Mas_Sup_Model_En_Name == cR_Mas_Sup_Model.CR_Mas_Sup_Model_En_Name))
                            ViewBag.LRExistEn = "عفوا الطراز مسجل من قبل (موقوف)";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Model.Any(m => m.CR_Mas_Sup_Model_Status == "H" && m.CR_Mas_Sup_Model_Fr_Name == cR_Mas_Sup_Model.CR_Mas_Sup_Model_Fr_Name))
                            ViewBag.LRExistFr = "عفوا الطراز مسجل من قبل (موقوف)";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Model.Any(m => m.CR_Mas_Sup_Model_Status == "D" && m.CR_Mas_Sup_Model_Ar_Name == cR_Mas_Sup_Model.CR_Mas_Sup_Model_Ar_Name))
                            ViewBag.LRExistAr = "عفوا الطراز مسجل من قبل (محذوف)";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Model.Any(m => m.CR_Mas_Sup_Model_Status == "D" && m.CR_Mas_Sup_Model_En_Name == cR_Mas_Sup_Model.CR_Mas_Sup_Model_En_Name))
                            ViewBag.LRExistEn = "عفوا الطراز مسجل من قبل (محذوف)";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Model.Any(m => m.CR_Mas_Sup_Model_Status == "D" && m.CR_Mas_Sup_Model_Fr_Name == cR_Mas_Sup_Model.CR_Mas_Sup_Model_Fr_Name))
                            ViewBag.LRExistFr = "عفوا الطراز مسجل من قبل (محذوف)";
                        if (cR_Mas_Sup_Model.CR_Mas_Sup_Model_Ar_Name != null && cR_Mas_Sup_Model.CR_Mas_Sup_Model_Ar_Name.Length < 3)
                            ViewBag.LRExistAr = "الحد الأدنى ٣ حروف";
                        if (cR_Mas_Sup_Model.CR_Mas_Sup_Model_En_Name != null && cR_Mas_Sup_Model.CR_Mas_Sup_Model_En_Name.Length < 3)
                            ViewBag.LRExistEn = "الحد الأدنى ٣ حروف";
                        if (cR_Mas_Sup_Model.CR_Mas_Sup_Model_Fr_Name != null && cR_Mas_Sup_Model.CR_Mas_Sup_Model_Fr_Name.Length < 3)
                            ViewBag.LRExistFr = "الحد الأدنى ٣ حروف";
                    }
                }
            }
            if (delete == "Delete" || delete == "حذف")
            {
                cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status = "D";
                db.Entry(cR_Mas_Sup_Model).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            if (delete == "Activate" || delete == "إسترجاع")
            {
                cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status = "A";
                db.Entry(cR_Mas_Sup_Model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "إيقاف" || hold == "hold")
            {
                cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status = "H";
                db.Entry(cR_Mas_Sup_Model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "تنشيط" || hold == "Activate")
            {
                cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status = "A";
                db.Entry(cR_Mas_Sup_Model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status == "A" ||
            cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status == "Activated" ||
            cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status == "1" ||
            cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }
            if ((cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status == "D" ||
                 cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status == "Deleted" ||
                 cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
            }
            if (cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status == "H" ||
                cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status == "Hold" ||
                cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
            }
            if (cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }
            ViewBag.CR_Mas_Sup_Model_Brand_Code = new SelectList(db.CR_Mas_Sup_Brand, "CR_Mas_Sup_Brand_Code", "CR_Mas_Sup_Brand_Ar_Name", cR_Mas_Sup_Model.CR_Mas_Sup_Model_Brand_Code);
            ViewBag.CR_Mas_Sup_Model_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code", "CR_Mas_Sup_Group_Ar_Name", cR_Mas_Sup_Model.CR_Mas_Sup_Model_Group_Code);
            ViewBag.delete = cR_Mas_Sup_Model.CR_Mas_Sup_Model_Status;
            return View(cR_Mas_Sup_Model);
        }
        //////// GET: Model/Delete/5
        //////public ActionResult Delete(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Sup_Model cR_Mas_Sup_Model = db.CR_Mas_Sup_Model.Find(id);
        //////    if (cR_Mas_Sup_Model == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Sup_Model);
        //////}
        //////// POST: Model/Delete/5
        //////[HttpPost, ActionName("Delete")]
        //////[ValidateAntiForgeryToken]
        //////public ActionResult DeleteConfirmed(string id)
        //////{
        //////    CR_Mas_Sup_Model cR_Mas_Sup_Model = db.CR_Mas_Sup_Model.Find(id);
        //////    db.CR_Mas_Sup_Model.Remove(cR_Mas_Sup_Model);
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