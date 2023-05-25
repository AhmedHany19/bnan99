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
    public class JobsController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        public static string TaskCode;

        // GET: Job
        public ActionResult Index()
        {
            TaskCode = "1603";
            Session["POS"] = "1603";
            if (Session["ST_1603_unhold"].ToString() != "true" || Session["ST_1603_hold"].ToString() != "true" && Session["ST_1603_undelete"].ToString() != "true" || Session["ST_1603_delete"].ToString() != "true")
            {
                var cR_Mas_Sup_Jobs = db.CR_Mas_Sup_Jobs.Include(c => c.CR_Mas_Sup_Group).Where(stat => stat.CR_Mas_Sup_Jobs_Status != "H" && stat.CR_Mas_Sup_Jobs_Status != "D");
                return View(cR_Mas_Sup_Jobs.ToList());
            }
            else
                if (Session["ST_1603_unhold"].ToString() != "true" || Session["ST_1603_hold"].ToString() != "true")
            {
                var cR_Mas_Sup_Jobs = db.CR_Mas_Sup_Jobs.Include(c => c.CR_Mas_Sup_Group).Where(stat => stat.CR_Mas_Sup_Jobs_Status != "H");
                return View(cR_Mas_Sup_Jobs.ToList());
            }
            else if (Session["ST_1603_undelete"].ToString() != "true" || Session["ST_1603_delete"].ToString() != "true")
            {
                var cR_Mas_Sup_Jobs = db.CR_Mas_Sup_Jobs.Include(c => c.CR_Mas_Sup_Group).Where(stat => stat.CR_Mas_Sup_Jobs_Status != "D");
                return View(cR_Mas_Sup_Jobs.ToList());
            }
            else
            {
                var cR_Mas_Sup_Jobs = db.CR_Mas_Sup_Jobs.Include(c => c.CR_Mas_Sup_Group);
                return View(cR_Mas_Sup_Jobs.ToList());
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
                var jobTable = new System.Data.DataTable("teste");

                jobTable.Columns.Add("الملاحظات", typeof(string));
                jobTable.Columns.Add("الحالة", typeof(string));
                jobTable.Columns.Add("المجموعة", typeof(string));
                jobTable.Columns.Add("الإسم", typeof(string));
                jobTable.Columns.Add("الرمز", typeof(string));
                var Lrecord = db.CR_Mas_Sup_Jobs.ToList();

                if (Lrecord != null)
                {
                    foreach (var i in Lrecord)
                    {
                        jobTable.Rows.Add(i.CR_Mas_Sup_Jobs_Reasons, i.CR_Mas_Sup_Jobs_Status, i.CR_Mas_Sup_Jobs_Group_Code, i.CR_Mas_Sup_Jobs_Ar_Name, i.CR_Mas_Sup_Jobs_Code);
                    }
                }

                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = jobTable;
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
            return View(db.CR_Mas_Sup_Jobs.ToList());
        }

        //////// GET: Model/Details/5
        //////public ActionResult Details(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Sup_Jobs cR_Mas_Sup_Jobs = db.CR_Mas_Sup_Jobs.Find(id);
        //////    if (cR_Mas_Sup_Jobs == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Sup_Jobs);
        //////}
        public CR_Mas_Sup_Jobs GetLastRecord()
        {
            var Lrecord = db.CR_Mas_Sup_Jobs.Max(Lr => Lr.CR_Mas_Sup_Jobs_Code);
            CR_Mas_Sup_Jobs m = new CR_Mas_Sup_Jobs();
            if (Lrecord != null)
            {
                Int64 val = Int64.Parse(Lrecord) + 1;
                m.CR_Mas_Sup_Jobs_Code = val.ToString();
            }
            else
            {
                m.CR_Mas_Sup_Jobs_Code = "1400000001";
            }
            return m;
        }
        // GET: Model/Create
        public ActionResult Create()
        {
            ViewBag.CR_Mas_Sup_Jobs_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code", "CR_Mas_Sup_Group_Ar_Name");
            CR_Mas_Sup_Jobs mod = new CR_Mas_Sup_Jobs();
            mod = GetLastRecord();
            mod.CR_Mas_Sup_Jobs_Status = "A";
            return View(mod);
        }

        // POST: Model/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Mas_Sup_Jobs_Code, CR_Mas_Sup_Jobs_Group_Code, CR_Mas_Sup_Jobs_Ar_Name, " +
        "CR_Mas_Sup_Jobs_En_Name, CR_Mas_Sup_Jobs_Fr_Name, CR_Mas_Sup_Jobs_Status, CR_Mas_Sup_Jobs_Reasons")] CR_Mas_Sup_Jobs
         cR_Mas_Sup_Jobs, string CR_Mas_Sup_Jobs_Ar_Name, string CR_Mas_Sup_Jobs_Fr_Name, string CR_Mas_Sup_Jobs_En_Name)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_Sup_Jobs.Any(Lr => Lr.CR_Mas_Sup_Jobs_Ar_Name == CR_Mas_Sup_Jobs_Ar_Name);
                    var LrecordExitEnglish = db.CR_Mas_Sup_Jobs.Any(Lr => Lr.CR_Mas_Sup_Jobs_En_Name == CR_Mas_Sup_Jobs_En_Name);
                    var LrecordExitFrench = db.CR_Mas_Sup_Jobs.Any(Lr => Lr.CR_Mas_Sup_Jobs_Fr_Name == CR_Mas_Sup_Jobs_Fr_Name);


                    if (cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Ar_Name != null && cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_En_Name != null &&
                        cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench &&
                        cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Ar_Name.Length >= 3 && cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_En_Name.Length >= 3 &&
                        cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Fr_Name.Length >= 3)
                    {
                        cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Code = GetLastRecord().CR_Mas_Sup_Jobs_Code;
                        cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Group_Code = "14";
                        db.CR_Mas_Sup_Jobs.Add(cR_Mas_Sup_Jobs);
                        db.SaveChanges();
                        cR_Mas_Sup_Jobs = new CR_Mas_Sup_Jobs();
                        cR_Mas_Sup_Jobs = GetLastRecord();
                        cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status = "A";
                        TempData["TempModel"] = "تم الحفظ بنجاح";
                        return RedirectToAction("Create", "Jobs");
                    }
                    else
                    {
                        if (cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe)
                            ViewBag.LRExistAr = "عفوا هذه المهنة موجودة";
                        if (LrecordExitEnglish)
                            ViewBag.LRExistEn = "عفوا هذه المهنة موجودة";
                        if (LrecordExitFrench)
                            ViewBag.LRExistFr = "عفوا هذه المهنة موجودة";
                        if (cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Ar_Name != null && cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Ar_Name.Length < 3)
                            ViewBag.LRExistAr = "عفوا الاسم يحتوي على ما بين 3 و 30 حرفًا";
                        if (cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_En_Name != null && cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_En_Name.Length < 3)
                            ViewBag.LRExistEn = "عفوا الاسم يحتوي على ما بين 3 و 30 حرفًا";
                        if (cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Fr_Name != null && cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Fr_Name.Length < 3)
                            ViewBag.LRExistFr = "عفوا الاسم يحتوي على ما بين 3 و 30 حرفًا";
                    }
                }
            }
            catch (Exception) { }
            ViewBag.CR_Mas_Sup_Jobs_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code", "CR_Mas_Sup_Group_Ar_Name",
                                                                 cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Group_Code);
            return View(cR_Mas_Sup_Jobs);
        }

        // GET: Model/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Sup_Jobs cR_Mas_Sup_Jobs = db.CR_Mas_Sup_Jobs.Find(id);
            if (cR_Mas_Sup_Jobs == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status == "A" || cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status == "1")
                {
                    ViewBag.stat = "حذف";
                    ViewBag.h = "إيقاف";
                }
                if ((cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status == "D" || cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status == "0"))
                {
                    ViewBag.stat = "إسترجاع";
                    ViewBag.h = "إيقاف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status == "H" || cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status == "2")
                {
                    ViewBag.h = "تنشيط";
                    ViewBag.stat = "حذف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status == null)
                {
                    ViewBag.h = "إيقاف";
                    ViewBag.stat = "حذف";
                }
                ViewBag.delete = cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status;
            }
            ViewBag.CR_Mas_Sup_Jobs_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code", "CR_Mas_Sup_Group_Ar_Name",
                                                                 cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Group_Code);
            return View(cR_Mas_Sup_Jobs);
        }

        // POST: Model/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Mas_Sup_Jobs_Code, CR_Mas_Sup_Jobs_Group_Code, CR_Mas_Sup_Jobs_Ar_Name, CR_Mas_Sup_Jobs_En_Name, " +
        "CR_Mas_Sup_Jobs_Fr_Name, CR_Mas_Sup_Jobs_Status, CR_Mas_Sup_Jobs_Reasons")] CR_Mas_Sup_Jobs cR_Mas_Sup_Jobs, string save, string delete, string hold)
        {
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_Sup_Jobs.Any(j => j.CR_Mas_Sup_Jobs_Code != cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Code &&
                                                                  j.CR_Mas_Sup_Jobs_Ar_Name == cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Ar_Name);
                    var LrecordExitEnglish = db.CR_Mas_Sup_Jobs.Any(j => j.CR_Mas_Sup_Jobs_Code != cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Code &&
                                                                    j.CR_Mas_Sup_Jobs_En_Name == cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_En_Name);
                    var LrecordExitFrench = db.CR_Mas_Sup_Jobs.Any(j => j.CR_Mas_Sup_Jobs_Code != cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Code &&
                                                                   j.CR_Mas_Sup_Jobs_Fr_Name == cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Fr_Name);

                    if (cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Ar_Name != null && cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_En_Name != null &&
                        cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench &&
                        cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Ar_Name.Length >= 3 && cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_En_Name.Length >= 3 &&
                        cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Fr_Name.Length >= 3)
                    {
                        db.Entry(cR_Mas_Sup_Jobs).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe)
                            ViewBag.LRExistAr = "عفوا هذه المهنة موجودة";
                        if (LrecordExitEnglish)
                            ViewBag.LRExistEn = "عفوا هذه المهنة موجودة";
                        if (LrecordExitFrench)
                            ViewBag.LRExistFr = "عفوا هذه المهنة موجودة";
                        if (cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Ar_Name != null && cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Ar_Name.Length < 3)
                            ViewBag.LRExistAr = "عفوا الاسم يحتوي على ما بين 3 و 30 حرفًا";
                        if (cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_En_Name != null && cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_En_Name.Length < 3)
                            ViewBag.LRExistEn = "عفوا الاسم يحتوي على ما بين 3 و 30 حرفًا";
                        if (cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Fr_Name != null && cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Fr_Name.Length < 3)
                            ViewBag.LRExistFr = "عفوا الاسم يحتوي على ما بين 3 و 30 حرفًا";
                    }
                }
            }
            if (delete == "Delete" || delete == "حذف")
            {
                cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status = "D";
                db.Entry(cR_Mas_Sup_Jobs).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            if (delete == "Activate" || delete == "إسترجاع")
            {
                cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status = "A";
                db.Entry(cR_Mas_Sup_Jobs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "إيقاف" || hold == "hold")
            {
                cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status = "H";
                db.Entry(cR_Mas_Sup_Jobs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "تنشيط" || hold == "Activate")
            {
                cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status = "A";
                db.Entry(cR_Mas_Sup_Jobs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status == "A" ||
            cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status == "Activated" ||
            cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status == "1" ||
            cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }
            if ((cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status == "D" ||
                 cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status == "Deleted" ||
                 cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
            }
            if (cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status == "H" ||
                cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status == "Hold" ||
                cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
            }
            if (cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }
            ViewBag.CR_Mas_Sup_Jobs_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code", "CR_Mas_Sup_Group_Ar_Name", cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Group_Code);
            ViewBag.delete = cR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Status;
            return View(cR_Mas_Sup_Jobs);
        }
        //////// GET: Model/Delete/5
        //////public ActionResult Delete(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Sup_Jobs cR_Mas_Sup_Jobs = db.CR_Mas_Sup_Jobs.Find(id);
        //////    if (cR_Mas_Sup_Jobs == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Sup_Jobs);
        //////}
        //////// POST: Model/Delete/5
        //////[HttpPost, ActionName("Delete")]
        //////[ValidateAntiForgeryToken]
        //////public ActionResult DeleteConfirmed(string id)
        //////{
        //////    CR_Mas_Sup_Jobs cR_Mas_Sup_Jobs = db.CR_Mas_Sup_Jobs.Find(id);
        //////    db.CR_Mas_Sup_Jobs.Remove(cR_Mas_Sup_Jobs);
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