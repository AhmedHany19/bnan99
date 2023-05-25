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
    public class NationalitiesController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        public static string TaskCode;

        // GET: Nationalities
        [ActionName("Index")]
        public ActionResult Index_Get()
        {
            TaskCode = "1601";
            Session["POS"] = "1601";
            if (Session["ST_1601_unhold"].ToString() != "true" || Session["ST_1601_hold"].ToString() != "true" && Session["ST_1601_undelete"].ToString() != "true" || Session["ST_1601_delete"].ToString() != "true")
            {
                var cR_Mas_Sup_Nationalities = db.CR_Mas_Sup_Nationalities.Include(c => c.CR_Mas_Sup_Country).Include(c => c.CR_Mas_Sup_Group).
                                               Where(stat => stat.CR_Mas_Sup_Nationalities_Status != "H" && stat.CR_Mas_Sup_Nationalities_Status != "D");
                return View(db.CR_Mas_Sup_Nationalities.ToList());
            }
            else
                if (Session["ST_1601_unhold"].ToString() != "true" || Session["ST_1601_hold"].ToString() != "true")
            {
                var cR_Mas_Sup_Nationalities = db.CR_Mas_Sup_Nationalities.Include(c => c.CR_Mas_Sup_Country).Include(c => c.CR_Mas_Sup_Group).
                                               Where(stat => stat.CR_Mas_Sup_Nationalities_Status != "H");
                return View(db.CR_Mas_Sup_Nationalities.ToList());
            }
            else if (Session["ST_1601_undelete"].ToString() != "true" || Session["ST_1601_delete"].ToString() != "true")
            {
                var cR_Mas_Sup_Nationalities = db.CR_Mas_Sup_Nationalities.Include(c => c.CR_Mas_Sup_Country).Include(c => c.CR_Mas_Sup_Group).
                                               Where(stat => stat.CR_Mas_Sup_Nationalities_Status != "D");
                return View(db.CR_Mas_Sup_Nationalities.ToList());
            }
            else
            {
                var cR_Mas_Sup_Nationalities = db.CR_Mas_Sup_Nationalities.Include(c => c.CR_Mas_Sup_Country).Include(c => c.CR_Mas_Sup_Group);
                return View(db.CR_Mas_Sup_Nationalities.ToList());
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
                var natTable = new System.Data.DataTable("teste");

                natTable.Columns.Add("الملاحظات", typeof(string));
                natTable.Columns.Add("الحالة", typeof(string));
                //natTable.Columns.Add("المجموعة", typeof(string));
                natTable.Columns.Add("التصنيف", typeof(string));
                //natTable.Columns.Add("رقم العداد", typeof(string));
                natTable.Columns.Add("الجنسية", typeof(string));
                natTable.Columns.Add("الرمز", typeof(string));
                var Lrecord = db.CR_Mas_Sup_Nationalities.ToList();
                if (Lrecord != null)
                {
                    foreach (var i in Lrecord)
                    {
                        natTable.Rows.Add(i.CR_Mas_Sup_Nationalities_Reasons, i.CR_Mas_Sup_Nationalities_Status,
                            i.CR_Mas_Sup_Nationalities_Country_Code, i.CR_Mas_Sup_Nationalities_Ar_Name, i.CR_Mas_Sup_Nationalities_Code);
                    }
                }
                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = natTable;
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
            return View(db.CR_Mas_Sup_Nationalities.ToList());
        }
        //////// GET: Nationalities/Details/5
        //////public async Task<ActionResult> Details(string id)
        ////// {
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Sup_Nationalities cR_Mas_Sup_Nationalities = await db.CR_Mas_Sup_Nationalities.FindAsync(id);
        //////    if (cR_Mas_Sup_Nationalities == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Sup_Nationalities);
        //////}

        public CR_Mas_Sup_Nationalities GetLastRecord()
        {
            var Lrecord = db.CR_Mas_Sup_Nationalities.Max(Lr => Lr.CR_Mas_Sup_Nationalities_Code);
            CR_Mas_Sup_Nationalities n = new CR_Mas_Sup_Nationalities();
            if (Lrecord != null)
            {
                Int64 val = Int64.Parse(Lrecord) + 1;
                n.CR_Mas_Sup_Nationalities_Code = val.ToString();
            }
            else
            {
                n.CR_Mas_Sup_Nationalities_Code = "1000000001";
            }
            return n;
        }

        // GET: Nationalities/Create
        public ActionResult Create()
        {
            if (Session["ST_1601_unhold"].ToString() != "true" || Session["ST_1601_hold"].ToString() != "true" && Session["ST_1601_undelete"].ToString() != "true" || Session["ST_1601_delete"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_Nationalities_Country_Code = new SelectList(db.CR_Mas_Sup_Country.Where(x => x.CR_Mas_Sup_Country_Status != "D" && x.CR_Mas_Sup_Country_Status != "H"), "CR_Mas_Sup_Country_Code", "CR_Mas_Sup_Country_Ar_Name");
            }
            else
                if (Session["ST_1601_unhold"].ToString() != "true" || Session["ST_1601_hold"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_Nationalities_Country_Code = new SelectList(db.CR_Mas_Sup_Country.Where(x => x.CR_Mas_Sup_Country_Status != "H"), "CR_Mas_Sup_Country_Code", "CR_Mas_Sup_Country_Ar_Name");
            }
            else if (Session["ST_1601_undelete"].ToString() != "true" || Session["ST_1601_delete"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_Nationalities_Country_Code = new SelectList(db.CR_Mas_Sup_Country.Where(x => x.CR_Mas_Sup_Country_Status != "D"), "CR_Mas_Sup_Country_Code", "CR_Mas_Sup_Country_Ar_Name");
            }
            else
            {
                ViewBag.CR_Mas_Sup_Nationalities_Country_Code = new SelectList(db.CR_Mas_Sup_Country, "CR_Mas_Sup_Country_Code", "CR_Mas_Sup_Country_Ar_Name");
            }

            ViewBag.CR_Mas_Sup_Nationalities_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code", "CR_Mas_Sup_Group_Ar_Name");
            CR_Mas_Sup_Nationalities nat = new CR_Mas_Sup_Nationalities();
            nat = GetLastRecord();
            nat.CR_Mas_Sup_Nationalities_Status = "A";
            return View(nat);
        }

        // POST: Nationalities/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Mas_Sup_Nationalities_Code, CR_Mas_Sup_Nationalities_Group_Code, CR_Mas_Sup_Nationalities_Country_Code, " +
        "CR_Mas_Sup_Nationalities_Ar_Name, CR_Mas_Sup_Nationalities_En_Name, CR_Mas_Sup_Nationalities_Fr_Name, CR_Mas_Sup_Nationalities_Counter, " +
        "CR_Mas_Sup_Nationalities_Status, CR_Mas_Sup_Nationalities_Reasons")] CR_Mas_Sup_Nationalities cR_Mas_Sup_Nationalities,
        string CR_Mas_Sup_Nationalities_Ar_Name, string CR_Mas_Sup_Nationalities_Fr_Name, string CR_Mas_Sup_Nationalities_En_Name)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_Sup_Nationalities.Any(Lr => Lr.CR_Mas_Sup_Nationalities_Ar_Name == CR_Mas_Sup_Nationalities_Ar_Name);
                    var LrecordExitEnglish = db.CR_Mas_Sup_Nationalities.Any(Lr => Lr.CR_Mas_Sup_Nationalities_En_Name == CR_Mas_Sup_Nationalities_En_Name);
                    var LrecordExitFrench = db.CR_Mas_Sup_Nationalities.Any(Lr => Lr.CR_Mas_Sup_Nationalities_Fr_Name == CR_Mas_Sup_Nationalities_Fr_Name);


                    if (cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Ar_Name != null && cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_En_Name != null &&
                        cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench &&
                        cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Ar_Name.Length >= 3 && cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_En_Name.Length >= 3 &&
                        cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Fr_Name.Length >= 3)
                    {
                        cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Code = GetLastRecord().CR_Mas_Sup_Nationalities_Code;
                        cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Group_Code = "10";
                        db.CR_Mas_Sup_Nationalities.Add(cR_Mas_Sup_Nationalities);
                        db.SaveChanges();
                        cR_Mas_Sup_Nationalities = new CR_Mas_Sup_Nationalities();
                        cR_Mas_Sup_Nationalities = GetLastRecord();
                        cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status = "A";
                        TempData["TempModel"] = "تم الحفظ بنجاح";
                        return RedirectToAction("Create", "Nationalities");
                    }
                    else
                    {
                        if (cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe)
                            ViewBag.LRExistAr = "عفوا هذه الجنسية موجودة";
                        if (LrecordExitEnglish)
                            ViewBag.LRExistEn = "عفوا هذه الجنسية موجودة";
                        if (LrecordExitFrench)
                            ViewBag.LRExistFr = "عفوا هذه الجنسية موجودة";
                        if (cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Ar_Name != null && cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Ar_Name.Length < 3)
                            ViewBag.LRExistAr = "عفوا الاسم يحتوي على ما بين 3 و 30 حرفًا";
                        if (cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_En_Name != null && cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_En_Name.Length < 3)
                            ViewBag.LRExistEn = "عفوا الاسم يحتوي على ما بين 3 و 30 حرفًا";
                        if (cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Fr_Name != null && cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Fr_Name.Length < 3)
                            ViewBag.LRExistFr = "عفوا الاسم يحتوي على ما بين 3 و 30 حرفًا";
                    }
                }
            }
            catch (Exception) { }
            ViewBag.CR_Mas_Sup_Nationalities_Country_Code = new SelectList(db.CR_Mas_Sup_Country, "CR_Mas_Sup_Country_Code", "CR_Mas_Sup_Country_Ar_Name", cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Country_Code);
            ViewBag.CR_Mas_Sup_Nationalities_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code", "CR_Mas_Sup_Group_Ar_Name", cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Group_Code);
            return View(cR_Mas_Sup_Nationalities);
        }

        // GET: Model/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Sup_Nationalities cR_Mas_Sup_Nationalities = db.CR_Mas_Sup_Nationalities.Find(id);
            if (cR_Mas_Sup_Nationalities == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status == "A" || cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status == "1")
                {
                    ViewBag.stat = "حذف";
                    ViewBag.h = "إيقاف";
                }
                if ((cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status == "D" || cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status == "0"))
                {
                    ViewBag.stat = "إسترجاع";
                    ViewBag.h = "إيقاف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status == "H" || cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status == "2")
                {
                    ViewBag.h = "تنشيط";
                    ViewBag.stat = "حذف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status == null)
                {
                    ViewBag.h = "إيقاف";
                    ViewBag.stat = "حذف";
                }
                ViewBag.delete = cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status;
            }
            ViewBag.CR_Mas_Sup_Nationalities_Country_Code = new SelectList(db.CR_Mas_Sup_Country, "CR_Mas_Sup_Country_Code", "CR_Mas_Sup_Country_Ar_Name",
                                                            cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Country_Code);
            ViewBag.CR_Mas_Sup_Nationalities_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code", "CR_Mas_Sup_Group_Ar_Name",
                                                            cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Group_Code);
            return View(cR_Mas_Sup_Nationalities);
        }

        // POST: Nationalities/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Mas_Sup_Nationalities_Code, CR_Mas_Sup_Nationalities_Group_Code, CR_Mas_Sup_Nationalities_Country_Code, " +
        "CR_Mas_Sup_Nationalities_Ar_Name, CR_Mas_Sup_Nationalities_En_Name, CR_Mas_Sup_Nationalities_Fr_Name, CR_Mas_Sup_Nationalities_Counter, " +
        "CR_Mas_Sup_Nationalities_Status, CR_Mas_Sup_Nationalities_Reasons")] CR_Mas_Sup_Nationalities cR_Mas_Sup_Nationalities, string save, string delete,
         string hold)
        {
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_Sup_Nationalities.Any(n => n.CR_Mas_Sup_Nationalities_Code != cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Code &&
                                                                           n.CR_Mas_Sup_Nationalities_Ar_Name == cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Ar_Name);
                    var LrecordExitEnglish = db.CR_Mas_Sup_Nationalities.Any(n => n.CR_Mas_Sup_Nationalities_Code != cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Code &&
                                                                             n.CR_Mas_Sup_Nationalities_En_Name == cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_En_Name);
                    var LrecordExitFrench = db.CR_Mas_Sup_Nationalities.Any(n => n.CR_Mas_Sup_Nationalities_Code != cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Code &&
                                                                            n.CR_Mas_Sup_Nationalities_Fr_Name == cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Fr_Name);

                    if (cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Ar_Name != null && cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_En_Name != null &&
                        cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench &&
                        cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Ar_Name.Length >= 3 && cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_En_Name.Length >= 3 &&
                        cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Fr_Name.Length >= 3)
                    {
                        db.Entry(cR_Mas_Sup_Nationalities).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe)
                            ViewBag.LRExistAr = "عفوا هذه الجنسية موجودة";
                        if (LrecordExitEnglish)
                            ViewBag.LRExistEn = "عفوا هذه الجنسية موجودة";
                        if (LrecordExitFrench)
                            ViewBag.LRExistFr = "عفوا هذه الجنسية موجودة";
                        if (cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Ar_Name != null && cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Ar_Name.Length < 3)
                            ViewBag.LRExistAr = "عفوا الاسم يحتوي على ما بين 3 و 30 حرفًا";
                        if (cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_En_Name != null && cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_En_Name.Length < 3)
                            ViewBag.LRExistEn = "عفوا الاسم يحتوي على ما بين 3 و 30 حرفًا";
                        if (cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Fr_Name != null && cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Fr_Name.Length < 3)
                            ViewBag.LRExistFr = "عفوا الاسم يحتوي على ما بين 3 و 30 حرفًا";
                    }
                }
            }
            if (delete == "Delete" || delete == "حذف")
            {
                cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status = "D";
                db.Entry(cR_Mas_Sup_Nationalities).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            if (delete == "Activate" || delete == "إسترجاع")
            {
                cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status = "A";
                db.Entry(cR_Mas_Sup_Nationalities).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "إيقاف" || hold == "hold")
            {
                cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status = "H";
                db.Entry(cR_Mas_Sup_Nationalities).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "تنشيط" || hold == "Activate")
            {
                cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status = "A";
                db.Entry(cR_Mas_Sup_Nationalities).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status == "A" ||
            cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status == "Activated" ||
            cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status == "1" ||
            cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }
            if ((cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status == "D" ||
                 cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status == "Deleted" ||
                 cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
            }
            if (cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status == "H" ||
                cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status == "Hold" ||
                cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
            }
            if (cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }
            ViewBag.CR_Mas_Sup_Nationalities_Country_Code = new SelectList(db.CR_Mas_Sup_Country, "CR_Mas_Sup_Country_Code", "CR_Mas_Sup_Country_Ar_Name", cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Country_Code);
            ViewBag.CR_Mas_Sup_Nationalities_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code", "CR_Mas_Sup_Group_Ar_Name", cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Group_Code);
            ViewBag.delete = cR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Status;
            return View(cR_Mas_Sup_Nationalities);
        }

        //////// GET: Nationalities/Delete/5
        //////public async Task<ActionResult> Delete(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Sup_Nationalities cR_Mas_Sup_Nationalities = await db.CR_Mas_Sup_Nationalities.FindAsync(id);
        //////    if (cR_Mas_Sup_Nationalities == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Sup_Nationalities);
        //////}

        //////// POST: Nationalities/Delete/5
        //////[HttpPost, ActionName("Delete")]
        //////[ValidateAntiForgeryToken]
        //////public async Task<ActionResult> DeleteConfirmed(string id)
        //////{
        //////    CR_Mas_Sup_Nationalities cR_Mas_Sup_Nationalities = await db.CR_Mas_Sup_Nationalities.FindAsync(id);
        //////    db.CR_Mas_Sup_Nationalities.Remove(cR_Mas_Sup_Nationalities);
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