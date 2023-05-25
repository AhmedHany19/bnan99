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
    public class CategoryController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        public static string TaskCode;

        // GET: Category
        public ActionResult Index()
        {

            Session["POS"] = "1503";
            TaskCode = "1503";
            if (Session["ST_1503_unhold"].ToString() != "true" || Session["ST_1503_hold"].ToString() != "true" && Session["ST_1503_undelete"].ToString() != "true" || Session["ST_1503_delete"].ToString() != "true")
            {
                var cR_Mas_Sup_Category = db.CR_Mas_Sup_Category.Include(c => c.CR_Mas_Sup_Group).Where(x => x.CR_Mas_Sup_Category_Status != "H" && x.CR_Mas_Sup_Category_Status != "D");
                return View(cR_Mas_Sup_Category.ToList());
            }
            else
                if (Session["ST_1503_unhold"].ToString() != "true" || Session["ST_1503_hold"].ToString() != "true")
            {
                var cR_Mas_Sup_Category = db.CR_Mas_Sup_Category.Include(c => c.CR_Mas_Sup_Group).Where(x => x.CR_Mas_Sup_Category_Status != "H");
                return View(cR_Mas_Sup_Category.ToList());
            }
            else if (Session["ST_1502_undelete"].ToString() != "true" || Session["ST_1502_delete"].ToString() != "true")
            {
                var cR_Mas_Sup_Category = db.CR_Mas_Sup_Category.Include(c => c.CR_Mas_Sup_Group).Where(x => x.CR_Mas_Sup_Category_Status != "D");
                return View(cR_Mas_Sup_Category.ToList());
            }
            else
            {
                var cR_Mas_Sup_Category = db.CR_Mas_Sup_Category.Include(c => c.CR_Mas_Sup_Group);
                return View(cR_Mas_Sup_Category.ToList());
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
                var colorTable = new System.Data.DataTable("teste");

                colorTable.Columns.Add("الملاحظات", typeof(string));
                colorTable.Columns.Add("الحالة", typeof(string));
                colorTable.Columns.Add("المجموعة", typeof(string));
                colorTable.Columns.Add("الإسم", typeof(string));
                colorTable.Columns.Add("الرمز", typeof(string));
                var Lrecord = db.CR_Mas_Sup_Category.ToList();

                if (Lrecord != null)
                {
                    foreach (var i in Lrecord)
                    {
                        colorTable.Rows.Add(i.CR_Mas_Sup_Category_Reasons, i.CR_Mas_Sup_Category_Status,
                        i.CR_Mas_Sup_Category_Group_Code, i.CR_Mas_Sup_Category_Ar_Name, i.CR_Mas_Sup_Category_Code);
                    }
                }
                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = colorTable;
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
            return View(db.CR_Mas_Sup_Category.ToList());
        }

        //////// GET: Category/Details/5
        //////public ActionResult Details(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Sup_Category cR_Mas_Sup_Category = db.CR_Mas_Sup_Category.Find(id);
        //////    if (cR_Mas_Sup_Category == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Sup_Category);
        //////}
        public CR_Mas_Sup_Category GetLastRecord()
        {
            var Lrecord = db.CR_Mas_Sup_Category.Max(Lr => Lr.CR_Mas_Sup_Category_Code);
            CR_Mas_Sup_Category c = new CR_Mas_Sup_Category();
            if (Lrecord != null)
            {
                Int64 val = Int64.Parse(Lrecord) + 1;
                c.CR_Mas_Sup_Category_Code = val.ToString();
            }
            else
            {
                c.CR_Mas_Sup_Category_Code = "3400000001";
            }
            return c;
        }
        // GET: Model/Create
        public ActionResult Create()
        {
            ViewBag.CR_Mas_Sup_Category_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code", "CR_Mas_Sup_Group_Ar_Name");
            CR_Mas_Sup_Category cat = new CR_Mas_Sup_Category();
            cat = GetLastRecord();
            cat.CR_Mas_Sup_Category_Status = "A";
            return View(cat);
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Mas_Sup_Category_Code, CR_Mas_Sup_Category_Group_Code, CR_Mas_Sup_Category_Ar_Name, " +
        "CR_Mas_Sup_Category_En_Name, CR_Mas_Sup_Category_Fr_Name, sCR_Mas_Sup_Category_Status, CR_Mas_Sup_Category_Reasons")]
        CR_Mas_Sup_Category cR_Mas_Sup_Category, string CR_Mas_Sup_Category_Ar_Name, string CR_Mas_Sup_Category_Fr_Name,
        string CR_Mas_Sup_Category_En_Name)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_Sup_Category.Any(Lr => Lr.CR_Mas_Sup_Category_Ar_Name == CR_Mas_Sup_Category_Ar_Name);
                    var LrecordExitEnglish = db.CR_Mas_Sup_Category.Any(Lr => Lr.CR_Mas_Sup_Category_En_Name == CR_Mas_Sup_Category_En_Name);
                    var LrecordExitFrench = db.CR_Mas_Sup_Category.Any(Lr => Lr.CR_Mas_Sup_Category_Fr_Name == CR_Mas_Sup_Category_Fr_Name);


                    if (cR_Mas_Sup_Category.CR_Mas_Sup_Category_Ar_Name != null && cR_Mas_Sup_Category.CR_Mas_Sup_Category_En_Name != null &&
                        cR_Mas_Sup_Category.CR_Mas_Sup_Category_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench &&
                        cR_Mas_Sup_Category.CR_Mas_Sup_Category_Ar_Name.Length >= 3 && cR_Mas_Sup_Category.CR_Mas_Sup_Category_En_Name.Length >= 3 &&
                        cR_Mas_Sup_Category.CR_Mas_Sup_Category_Fr_Name.Length >= 3)
                    {
                        cR_Mas_Sup_Category.CR_Mas_Sup_Category_Code = GetLastRecord().CR_Mas_Sup_Category_Code;
                        cR_Mas_Sup_Category.CR_Mas_Sup_Category_Group_Code = "34";
                        cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status = "A";
                        db.CR_Mas_Sup_Category.Add(cR_Mas_Sup_Category);
                        db.SaveChanges();
                        cR_Mas_Sup_Category = new CR_Mas_Sup_Category();
                        cR_Mas_Sup_Category = GetLastRecord();
                        cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status = "A";
                        TempData["TempModel"] = "تم الحفظ بنجاح";
                        return RedirectToAction("Create", "CategoryCar");
                    }
                    else
                    {
                        if (cR_Mas_Sup_Category.CR_Mas_Sup_Category_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Category.CR_Mas_Sup_Category_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Category.CR_Mas_Sup_Category_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe)
                            ViewBag.LRExistAr = "عفوا هذه الفئة موجودة";
                        if (LrecordExitEnglish)
                            ViewBag.LRExistEn = "عفوا هذه الفئة موجودة";
                        if (LrecordExitFrench)
                            ViewBag.LRExistFr = "عفوا هذه الفئة موجودة";
                        if (cR_Mas_Sup_Category.CR_Mas_Sup_Category_Ar_Name != null && cR_Mas_Sup_Category.CR_Mas_Sup_Category_Ar_Name.Length < 3)
                            ViewBag.LRExistAr = "عفوا الاسم يحتوي على ما بين 3 و 20 حرفًا";
                        if (cR_Mas_Sup_Category.CR_Mas_Sup_Category_En_Name != null && cR_Mas_Sup_Category.CR_Mas_Sup_Category_En_Name.Length < 3)
                            ViewBag.LRExistEn = "عفوا الاسم يحتوي على ما بين 3 و 20 حرفًا";
                        if (cR_Mas_Sup_Category.CR_Mas_Sup_Category_Fr_Name != null && cR_Mas_Sup_Category.CR_Mas_Sup_Category_Fr_Name.Length < 3)
                            ViewBag.LRExistFr = "عفوا الاسم يحتوي على ما بين 3 و 20 حرفًا";
                    }
                }
            }
            catch (Exception) { }
            ViewBag.CR_Mas_Sup_Category_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code",
                                                        "CR_Mas_Sup_Group_Ar_Name", cR_Mas_Sup_Category.CR_Mas_Sup_Category_Group_Code);
            return View(cR_Mas_Sup_Category);
        }

        // GET: Category/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Sup_Category cR_Mas_Sup_Category = db.CR_Mas_Sup_Category.Find(id);
            if (cR_Mas_Sup_Category == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status == "A" ||
                    cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status == "1")
                {
                    ViewBag.stat = "حذف";
                    ViewBag.h = "إيقاف";
                }
                if ((cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status == "D" ||
                    cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status == "0"))
                {
                    ViewBag.stat = "إسترجاع";
                    ViewBag.h = "إيقاف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status == "H" ||
                    cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status == "2")
                {
                    ViewBag.h = "تنشيط";
                    ViewBag.stat = "حذف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status == null)
                {
                    ViewBag.h = "إيقاف";
                    ViewBag.stat = "حذف";
                }
                ViewBag.delete = cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status;
            }

            ViewBag.CR_Mas_Sup_Category_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code",
            "CR_Mas_Sup_Group_Ar_Name", cR_Mas_Sup_Category.CR_Mas_Sup_Category_Group_Code);
            return View(cR_Mas_Sup_Category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Mas_Sup_Category_Code, CR_Mas_Sup_Category_Group_Code, " +
        "CR_Mas_Sup_Category_Ar_Name, CR_Mas_Sup_Category_En_Name, CR_Mas_Sup_Category_Fr_Name, " +
        "CR_Mas_Sup_Category_Status, CR_Mas_Sup_Category_Reasons")]
        CR_Mas_Sup_Category cR_Mas_Sup_Category, string save, string delete, string hold)
        {
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_Sup_Category.Any(c => c.CR_Mas_Sup_Category_Code != cR_Mas_Sup_Category.CR_Mas_Sup_Category_Code &&
                                                                               c.CR_Mas_Sup_Category_Ar_Name == cR_Mas_Sup_Category.CR_Mas_Sup_Category_Ar_Name);
                    var LrecordExitEnglish = db.CR_Mas_Sup_Category.Any(c => c.CR_Mas_Sup_Category_Code != cR_Mas_Sup_Category.CR_Mas_Sup_Category_Code &&
                                                                            c.CR_Mas_Sup_Category_En_Name == cR_Mas_Sup_Category.CR_Mas_Sup_Category_En_Name);
                    var LrecordExitFrench = db.CR_Mas_Sup_Category.Any(c => c.CR_Mas_Sup_Category_Code != cR_Mas_Sup_Category.CR_Mas_Sup_Category_Code &&
                                                                           c.CR_Mas_Sup_Category_Fr_Name == cR_Mas_Sup_Category.CR_Mas_Sup_Category_Fr_Name);

                    if (cR_Mas_Sup_Category.CR_Mas_Sup_Category_Ar_Name != null && cR_Mas_Sup_Category.CR_Mas_Sup_Category_En_Name != null &&
                        cR_Mas_Sup_Category.CR_Mas_Sup_Category_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench &&
                        cR_Mas_Sup_Category.CR_Mas_Sup_Category_Ar_Name.Length >= 3 && cR_Mas_Sup_Category.CR_Mas_Sup_Category_En_Name.Length >= 3 &&
                        cR_Mas_Sup_Category.CR_Mas_Sup_Category_Fr_Name.Length >= 3)
                    {
                        db.Entry(cR_Mas_Sup_Category).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (cR_Mas_Sup_Category.CR_Mas_Sup_Category_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Category.CR_Mas_Sup_Category_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Category.CR_Mas_Sup_Category_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe)
                            ViewBag.LRExistAr = "عفوا هذه الفئة موجودة";
                        if (LrecordExitEnglish)
                            ViewBag.LRExistEn = "عفوا هذه الفئة موجودة";
                        if (LrecordExitFrench)
                            ViewBag.LRExistFr = "عفوا هذه الفئة موجودة";
                        if (cR_Mas_Sup_Category.CR_Mas_Sup_Category_Ar_Name != null && cR_Mas_Sup_Category.CR_Mas_Sup_Category_Ar_Name.Length < 3)
                            ViewBag.LRExistAr = "عفوا الاسم يحتوي على ما بين 3 و 20 حرفًا";
                        if (cR_Mas_Sup_Category.CR_Mas_Sup_Category_En_Name != null && cR_Mas_Sup_Category.CR_Mas_Sup_Category_En_Name.Length < 3)
                            ViewBag.LRExistEn = "عفوا الاسم يحتوي على ما بين 3 و 20 حرفًا";
                        if (cR_Mas_Sup_Category.CR_Mas_Sup_Category_Fr_Name != null && cR_Mas_Sup_Category.CR_Mas_Sup_Category_Fr_Name.Length < 3)
                            ViewBag.LRExistFr = "عفوا الاسم يحتوي على ما بين 3 و 20 حرفًا";
                    }
                }
            }
            if (delete == "Delete" || delete == "حذف")
            {
                cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status = "D";
                db.Entry(cR_Mas_Sup_Category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (delete == "Activate" || delete == "إسترجاع")
            {
                cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status = "A";
                db.Entry(cR_Mas_Sup_Category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "إيقاف" || hold == "hold")
            {
                cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status = "H";
                db.Entry(cR_Mas_Sup_Category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "تنشيط" || hold == "Activate")
            {
                cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status = "A";
                db.Entry(cR_Mas_Sup_Category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status == "A" ||
            cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status == "Activated" ||
            cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status == "1" ||
            cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }
            if ((cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status == "D" ||
                 cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status == "Deleted" ||
                 cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
            }
            if (cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status == "H" ||
                cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status == "Hold" ||
                cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
            }
            if (cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }
            ViewBag.delete = cR_Mas_Sup_Category.CR_Mas_Sup_Category_Status;
            ViewBag.CR_Mas_Sup_Category_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code", "CR_Mas_Sup_Group_Ar_Name",
                                                                        cR_Mas_Sup_Category.CR_Mas_Sup_Category_Group_Code);
            return View(cR_Mas_Sup_Category);
        }

        //////// GET: Category/Delete/5
        //////public ActionResult Delete(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Sup_Category cR_Mas_Sup_Category = db.CR_Mas_Sup_Category.Find(id);
        //////    if (cR_Mas_Sup_Category == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Sup_Category);
        //////}

        //////// POST: Category/Delete/5
        //////[HttpPost, ActionName("Delete")]
        //////[ValidateAntiForgeryToken]
        //////public ActionResult DeleteConfirmed(string id)
        //////{
        //////    CR_Mas_Sup_Category cR_Mas_Sup_Category = db.CR_Mas_Sup_Category.Find(id);
        //////    db.CR_Mas_Sup_Category.Remove(cR_Mas_Sup_Category);
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