using RentCar.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;

namespace RentCar.Controllers
{
    public class AdditionalController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        public static string TaskCode;

        // GET: Additional
        public async Task<ActionResult> Index()
        {
            TaskCode = "1507";
            Session["POS"] = "1507";
            if (Session["ST_1507_unhold"].ToString() != "true" || Session["ST_1507_hold"].ToString() != "true" && Session["ST_1507_undelete"].ToString() != "true" || Session["ST_1507_delete"].ToString() != "true")
            {
                var cR_Mas_Sup_Additional = db.CR_Mas_Sup_Additional.Include(c => c.CR_Mas_Sup_Group).Where(x => x.CR_Mas_Sup_Additional_Status != "D" && x.CR_Mas_Sup_Additional_Status != "H");
                return View(cR_Mas_Sup_Additional.ToList());
            }
            else
                if (Session["ST_1507_unhold"].ToString() != "true" || Session["ST_1507_hold"].ToString() != "true")
            {
                var cR_Mas_Sup_Additional = db.CR_Mas_Sup_Additional.Include(c => c.CR_Mas_Sup_Group).Where(x => x.CR_Mas_Sup_Additional_Status != "H");
                return View(cR_Mas_Sup_Additional.ToList());
            }
            else if (Session["ST_1507_undelete"].ToString() != "true" || Session["ST_1507_delete"].ToString() != "true")
            {
                var cR_Mas_Sup_Additional = db.CR_Mas_Sup_Additional.Include(c => c.CR_Mas_Sup_Group).Where(x => x.CR_Mas_Sup_Additional_Status != "D");
                return View(cR_Mas_Sup_Additional.ToList());
            }
            else
            {
                var cR_Mas_Sup_Additional = db.CR_Mas_Sup_Additional.Include(c => c.CR_Mas_Sup_Group);
                return View(cR_Mas_Sup_Additional.ToList());
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
                var additionalTable = new System.Data.DataTable("teste");

                additionalTable.Columns.Add("الملاحظات", typeof(string));
                additionalTable.Columns.Add("الحالة", typeof(string));
                additionalTable.Columns.Add("المجموعة", typeof(string));
                additionalTable.Columns.Add("الإسم", typeof(string));
                additionalTable.Columns.Add("الرمز", typeof(string));
                var Lrecord = db.CR_Mas_Sup_Additional.ToList();

                if (Lrecord != null)
                {
                    foreach (var i in Lrecord)
                    {
                        additionalTable.Rows.Add(i.CR_Mas_Sup_Additional_Reasons, i.CR_Mas_Sup_Additional_Status, i.CR_Mas_Sup_Additional_Group_Code,
                        i.CR_Mas_Sup_Additional_Ar_Name, i.CR_Mas_Sup_Additional_Code);
                    }
                }
                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = additionalTable;
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
            return View(db.CR_Mas_Sup_Color.ToList());
        }

        //////// GET: Additional/Details/5
        //////public async Task<ActionResult> Details(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Sup_Additional cR_Mas_Sup_Additional = await db.CR_Mas_Sup_Additional.FindAsync(id);
        //////    if (cR_Mas_Sup_Additional == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Sup_Additional);
        //////}
        public CR_Mas_Sup_Additional GetLastRecord()
        {
            var Lrecord = db.CR_Mas_Sup_Additional.Max(Lr => Lr.CR_Mas_Sup_Additional_Code);
            CR_Mas_Sup_Additional A = new CR_Mas_Sup_Additional();
            if (Lrecord != null)
            {
                Int64 val = Int64.Parse(Lrecord) + 1;
                A.CR_Mas_Sup_Additional_Code = val.ToString();
            }
            else
            {
                A.CR_Mas_Sup_Additional_Code = "3500000001";
            }
            return A;
        }

        // GET: Additional/Create
        public ActionResult Create()
        {
            ViewBag.CR_Mas_Sup_Additional_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code", "CR_Mas_Sup_Group_Ar_Name");
            CR_Mas_Sup_Additional additional = new CR_Mas_Sup_Additional();
            additional = GetLastRecord();
            additional.CR_Mas_Sup_Additional_Status = "A";
            return View(additional);
        }
        // POST: Additional/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Mas_Sup_Additional_Code,CR_Mas_Sup_Additional_Group_Code, "+
            "CR_Mas_Sup_Additional_Ar_Name,CR_Mas_Sup_Additional_En_Name,CR_Mas_Sup_Additional_Fr_Name, "+
            "CR_Mas_Sup_Additional_Status,CR_Mas_Sup_Additional_Reasons")] CR_Mas_Sup_Additional cR_Mas_Sup_Additional)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_Sup_Additional.Any(Lr => Lr.CR_Mas_Sup_Additional_Ar_Name == cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Ar_Name);
                    var LrecordExitEnglish = db.CR_Mas_Sup_Additional.Any(Lr => Lr.CR_Mas_Sup_Additional_En_Name == cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_En_Name);
                    var LrecordExitFrench = db.CR_Mas_Sup_Additional.Any(Lr => Lr.CR_Mas_Sup_Additional_Fr_Name == cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Fr_Name);


                    if (cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Ar_Name != null && cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_En_Name != null &&
                            cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench &&
                            cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Ar_Name.Length >= 3 && cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_En_Name.Length >= 3 &&
                            cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Fr_Name.Length >= 3)
                    {
                        cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Code = GetLastRecord().CR_Mas_Sup_Additional_Code;
                        cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Group_Code = "35";
                        db.CR_Mas_Sup_Additional.Add(cR_Mas_Sup_Additional);
                        db.SaveChanges();
                        cR_Mas_Sup_Additional = new CR_Mas_Sup_Additional();
                        cR_Mas_Sup_Additional = GetLastRecord();
                        cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status = "A";
                        TempData["TempModel"] = "تم الحفظ بنجاح";
                        return RedirectToAction("Create", "Additional");
                    }
                    else
                    {
                        if (cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Additional.Any(b => b.CR_Mas_Sup_Additional_Status == "A" && b.CR_Mas_Sup_Additional_Ar_Name == cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Ar_Name))
                            ViewBag.LRExistAr = "عفوا الإضافة مسجلة من قبل";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Additional.Any(b => b.CR_Mas_Sup_Additional_Status == "A" && b.CR_Mas_Sup_Additional_En_Name == cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_En_Name))
                            ViewBag.LRExistEn = "عفوا الإضافة مسجلة من قبل";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Additional.Any(b => b.CR_Mas_Sup_Additional_Status == "A" && b.CR_Mas_Sup_Additional_Fr_Name == cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Fr_Name))
                            ViewBag.LRExistFr = "عفوا الإضافة مسجلة من قبل";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Additional.Any(b => b.CR_Mas_Sup_Additional_Status == "H" && b.CR_Mas_Sup_Additional_Ar_Name == cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Ar_Name))
                            ViewBag.LRExistAr = "عفوا الإضافة مسجلة من قبل (موقوفة)";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Additional.Any(b => b.CR_Mas_Sup_Additional_Status == "H" && b.CR_Mas_Sup_Additional_En_Name == cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_En_Name))
                            ViewBag.LRExistEn = "عفوا الإضافة مسجلة من قبل (موقوفة)";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Additional.Any(b => b.CR_Mas_Sup_Additional_Status == "H" && b.CR_Mas_Sup_Additional_Fr_Name == cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Fr_Name))
                            ViewBag.LRExistFr = "عفوا الإضافة مسجلة من قبل (موقوفة)";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Additional.Any(b => b.CR_Mas_Sup_Additional_Status == "D" && b.CR_Mas_Sup_Additional_Ar_Name == cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Ar_Name))
                            ViewBag.LRExistAr = "عفوا الإضافة مسجلة من قبل (محذوفة)";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Additional.Any(b => b.CR_Mas_Sup_Additional_Status == "D" && b.CR_Mas_Sup_Additional_En_Name == cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_En_Name))
                            ViewBag.LRExistEn = "عفوا الإضافة مسجلة من قبل (محذوفة)";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Additional.Any(b => b.CR_Mas_Sup_Additional_Status == "D" && b.CR_Mas_Sup_Additional_Fr_Name == cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Fr_Name))
                            ViewBag.LRExistFr = "عفوا الإضافة مسجلة من قبل (محذوفة)";
                        if (cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Ar_Name != null && cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Ar_Name.Length < 3)
                            ViewBag.LRExistAr = "الحد الأدنى ٣ حروف";
                        if (cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_En_Name != null && cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_En_Name.Length < 3)
                            ViewBag.LRExistEn = "الحد الأدنى ٣ حروف";
                        if (cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Fr_Name != null && cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Fr_Name.Length < 3)
                            ViewBag.LRExistFr = "الحد الأدنى ٣ حروف";
                    }

                }
            }
            catch (Exception) { }
            ViewBag.CR_Mas_Sup_Additional_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code", "CR_Mas_Sup_Group_Ar_Name", cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Group_Code);
            return View(cR_Mas_Sup_Additional);
        }

        // GET: Additional/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Sup_Additional cR_Mas_Sup_Additional = db.CR_Mas_Sup_Additional.Find(id);
            if (cR_Mas_Sup_Additional == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status == "A" || cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status == "1")
                {
                    ViewBag.stat = "حذف";
                    ViewBag.h = "إيقاف";
                }

                if ((cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status == "D" || cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status == "0"))
                {
                    ViewBag.stat = "إسترجاع";
                    ViewBag.h = "إيقاف";
                    ViewData["ReadOnly"] = "true";
                }

                if (cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status == "H" || cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status == "2")
                {
                    ViewBag.h = "تنشيط";
                    ViewBag.stat = "حذف";
                    ViewData["ReadOnly"] = "true";
                }

                if (cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status == null)
                {
                    ViewBag.h = "إيقاف";
                    ViewBag.stat = "حذف";
                }
                ViewBag.delete = cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status;
            }
            ViewBag.CR_Mas_Sup_Additional_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code", "CR_Mas_Sup_Group_Ar_Name", cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Group_Code);
            return View(cR_Mas_Sup_Additional);
        }

        // POST: Additional/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Mas_Sup_Additional_Code,CR_Mas_Sup_Additional_Group_Code, "+
            "CR_Mas_Sup_Additional_Ar_Name,CR_Mas_Sup_Additional_En_Name,CR_Mas_Sup_Additional_Fr_Name,CR_Mas_Sup_Additional_Status, "+
            "CR_Mas_Sup_Additional_Reasons")] CR_Mas_Sup_Additional cR_Mas_Sup_Additional, string save, string delete, string hold)
        {
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_Sup_Additional.Any(a => a.CR_Mas_Sup_Additional_Code != cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Code &&
                                                                        a.CR_Mas_Sup_Additional_Ar_Name == cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Ar_Name);
                    var LrecordExitEnglish = db.CR_Mas_Sup_Additional.Any(a => a.CR_Mas_Sup_Additional_Code != cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Code &&
                                                                          a.CR_Mas_Sup_Additional_En_Name == cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_En_Name);
                    var LrecordExitFrench = db.CR_Mas_Sup_Additional.Any(a => a.CR_Mas_Sup_Additional_Code != cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Code &&
                                                                         a.CR_Mas_Sup_Additional_Fr_Name == cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Fr_Name);

                    if (cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Ar_Name != null && cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_En_Name != null &&
                            cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench &&
                            cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Ar_Name.Length >= 3 && cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_En_Name.Length >= 3 &&
                            cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Fr_Name.Length >= 3)
                    {
                        db.Entry(cR_Mas_Sup_Additional).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["TempModif"] = "تم التعديل بنجاح";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Additional.Any(b => b.CR_Mas_Sup_Additional_Status == "A" && b.CR_Mas_Sup_Additional_Ar_Name == cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Ar_Name))
                            ViewBag.LRExistAr = "عفوا الإضافة مسجلة من قبل";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Additional.Any(b => b.CR_Mas_Sup_Additional_Status == "A" && b.CR_Mas_Sup_Additional_En_Name == cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_En_Name))
                            ViewBag.LRExistEn = "عفوا الإضافة مسجلة من قبل";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Additional.Any(b => b.CR_Mas_Sup_Additional_Status == "A" && b.CR_Mas_Sup_Additional_Fr_Name == cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Fr_Name))
                            ViewBag.LRExistFr = "عفوا الإضافة مسجلة من قبل";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Additional.Any(b => b.CR_Mas_Sup_Additional_Status == "H" && b.CR_Mas_Sup_Additional_Ar_Name == cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Ar_Name))
                            ViewBag.LRExistAr = "عفوا الإضافة مسجلة من قبل (موقوفة)";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Additional.Any(b => b.CR_Mas_Sup_Additional_Status == "H" && b.CR_Mas_Sup_Additional_En_Name == cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_En_Name))
                            ViewBag.LRExistEn = "عفوا الإضافة مسجلة من قبل (موقوفة)";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Additional.Any(b => b.CR_Mas_Sup_Additional_Status == "H" && b.CR_Mas_Sup_Additional_Fr_Name == cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Fr_Name))
                            ViewBag.LRExistFr = "عفوا الإضافة مسجلة من قبل (موقوفة)";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Additional.Any(b => b.CR_Mas_Sup_Additional_Status == "D" && b.CR_Mas_Sup_Additional_Ar_Name == cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Ar_Name))
                            ViewBag.LRExistAr = "عفوا الإضافة مسجلة من قبل (محذوفة)";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Additional.Any(b => b.CR_Mas_Sup_Additional_Status == "D" && b.CR_Mas_Sup_Additional_En_Name == cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_En_Name))
                            ViewBag.LRExistEn = "عفوا الإضافة مسجلة من قبل (محذوفة)";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Additional.Any(b => b.CR_Mas_Sup_Additional_Status == "D" && b.CR_Mas_Sup_Additional_Fr_Name == cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Fr_Name))
                            ViewBag.LRExistFr = "عفوا الإضافة مسجلة من قبل (محذوفة)";
                        if (cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Ar_Name != null && cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Ar_Name.Length < 3)
                            ViewBag.LRExistAr = "الحد الأدنى ٣ حروف";
                        if (cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_En_Name != null && cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_En_Name.Length < 3)
                            ViewBag.LRExistEn = "الحد الأدنى ٣ حروف";
                        if (cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Fr_Name != null && cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Fr_Name.Length < 3)
                            ViewBag.LRExistFr = "الحد الأدنى ٣ حروف";
                    }
                }
            }
            if (delete == "Delete" || delete == "حذف")
            {
                cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status = "D";
                db.Entry(cR_Mas_Sup_Additional).State = EntityState.Modified;
                db.SaveChanges();
                TempData["TempModif"] = "تم الحذف بنجاح";
                return RedirectToAction("Index");
            }
            if (delete == "Activate" || delete == "إسترجاع")
            {
                cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status = "A";
                db.Entry(cR_Mas_Sup_Additional).State = EntityState.Modified;
                db.SaveChanges();
                TempData["TempModif"] = "تم الإسترجاع بنجاح";
                return RedirectToAction("Index");
            }
            if (hold == "إيقاف" || hold == "hold")
            {
                cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status = "H";
                db.Entry(cR_Mas_Sup_Additional).State = EntityState.Modified;
                db.SaveChanges();
                TempData["TempModif"] = "تم الإيقاف بنجاح";
                return RedirectToAction("Index");
            }
            if (hold == "تنشيط" || hold == "Activate")
            {
                cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status = "A";
                db.Entry(cR_Mas_Sup_Additional).State = EntityState.Modified;
                db.SaveChanges();
                TempData["TempModif"] = "تم التنشيط بنجاح";
                return RedirectToAction("Index");
            }
            if (cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status == "A" ||
            cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status == "Activated" ||
            cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status == "1" ||
            cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }

            if ((cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status == "D" ||
                 cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status == "Deleted" ||
                 cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
            }

            if (cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status == "H" ||
                cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status == "Hold" ||
                cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
            }

            if (cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }
            ViewBag.delete = cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Status;
            ViewBag.CR_Mas_Sup_Additional_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code", "CR_Mas_Sup_Group_Ar_Name", cR_Mas_Sup_Additional.CR_Mas_Sup_Additional_Group_Code);
            return View(cR_Mas_Sup_Additional);
        }

        //////// GET: Additional/Delete/5
        //////public async Task<ActionResult> Delete(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Sup_Additional cR_Mas_Sup_Additional = await db.CR_Mas_Sup_Additional.FindAsync(id);
        //////    if (cR_Mas_Sup_Additional == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Sup_Additional);
        //////}

        //////// POST: Additional/Delete/5
        //////[HttpPost, ActionName("Delete")]
        //////[ValidateAntiForgeryToken]
        //////public async Task<ActionResult> DeleteConfirmed(string id)
        //////{
        //////    CR_Mas_Sup_Additional cR_Mas_Sup_Additional = await db.CR_Mas_Sup_Additional.FindAsync(id);
        //////    db.CR_Mas_Sup_Additional.Remove(cR_Mas_Sup_Additional);
        //////    await db.SaveChangesAsync();
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