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
    public class FeaturesController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        public static string TaskCode;

        // GET: Features
        [ActionName("Index")]
        public ActionResult Index_Get()
        {
            TaskCode = "1505";
            Session["POS"] = "1505";
            if (Session["ST_1505_unhold"].ToString() != "true" || Session["ST_1505_hold"].ToString() != "true" && Session["ST_1505_undelete"].ToString() != "true" || Session["ST_1505_delete"].ToString() != "true")
            {
                var FeaturesLIst = from CR_Mas_Sup_Features in db.CR_Mas_Sup_Features
                                   where CR_Mas_Sup_Features.CR_Mas_Sup_Features_Status != "H" && CR_Mas_Sup_Features.CR_Mas_Sup_Features_Status != "D"
                                   select CR_Mas_Sup_Features;
                return View(FeaturesLIst);
            }
            else
                if (Session["ST_1505_unhold"].ToString() != "true" || Session["ST_1505_hold"].ToString() != "true")
            {
                var FeaturesLIst = db.CR_Mas_Sup_Features.Where(x => x.CR_Mas_Sup_Features_Status != "H");
                return View(db.CR_Mas_Sup_Features.ToList());
            }
            else if (Session["ST_1505_undelete"].ToString() != "true" || Session["ST_1505_delete"].ToString() != "true")
            {
                var FeaturesLIst = db.CR_Mas_Sup_Features.Where(x => x.CR_Mas_Sup_Features_Status != "D");
                return View(db.CR_Mas_Sup_Features.ToList());
            }
            else
            {
                return View(db.CR_Mas_Sup_Features.ToList());
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
                var featuresTable = new System.Data.DataTable("teste");

                featuresTable.Columns.Add("الملاحظات", typeof(string));
                featuresTable.Columns.Add("الحالة", typeof(string));
                featuresTable.Columns.Add("الإسم العربي", typeof(string));
                featuresTable.Columns.Add("الرمز", typeof(string));
                var Lrecord = db.CR_Mas_Sup_Features.ToList();

                if (Lrecord != null)
                {
                    foreach (var i in Lrecord)
                    {
                        featuresTable.Rows.Add(i.CR_Mas_Sup_Features_Reasons, i.CR_Mas_Sup_Features_Status,
                            i.CR_Mas_Sup_Features_Ar_Name, i.CR_Mas_Sup_Features_Code);
                    }
                }
                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = featuresTable;
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
            return View(db.CR_Mas_Sup_Features.ToList());
        }

        //////// GET: Features/Details/5
        //////public ActionResult Details(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Sup_Features cR_Mas_Sup_Features = db.CR_Mas_Sup_Features.Find(id);
        //////    if (cR_Mas_Sup_Features == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Sup_Features);
        //////}
        public CR_Mas_Sup_Features GetLastRecord()
        {
            var Lrecord = db.CR_Mas_Sup_Features.Max(Lr => Lr.CR_Mas_Sup_Features_Code);
            CR_Mas_Sup_Features f = new CR_Mas_Sup_Features();
            if (Lrecord != null)
            {
                int val = int.Parse(Lrecord) + 1;
                f.CR_Mas_Sup_Features_Code = val.ToString();
            }
            else
            {
                f.CR_Mas_Sup_Features_Code = "1001";
            }
            return f;
        }
        // GET: Features/Create
        public ActionResult Create()
        {
            CR_Mas_Sup_Features feature = new CR_Mas_Sup_Features();
            feature = GetLastRecord();
            feature.CR_Mas_Sup_Features_Status = "A";
            return View(feature);
        }

        // POST: Features/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Mas_Sup_Features_Code, CR_Mas_Sup_Features_Ar_Name, CR_Mas_Sup_Features_En_Name," +
        "CR_Mas_Sup_Features_Fr_Name, CR_Mas_Sup_Features_Status, CR_Mas_Sup_Features_Reasons")] CR_Mas_Sup_Features cR_Mas_Sup_Features,
        string CR_Mas_Sup_Features_Ar_Name, string CR_Mas_Sup_Features_Fr_Name, string CR_Mas_Sup_Features_En_Name)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_Sup_Features.Any(Lr => Lr.CR_Mas_Sup_Features_Ar_Name == CR_Mas_Sup_Features_Ar_Name);
                    var LrecordExitEnglish = db.CR_Mas_Sup_Features.Any(Lr => Lr.CR_Mas_Sup_Features_En_Name == CR_Mas_Sup_Features_En_Name);
                    var LrecordExitFrench = db.CR_Mas_Sup_Features.Any(Lr => Lr.CR_Mas_Sup_Features_Fr_Name == CR_Mas_Sup_Features_Fr_Name);


                    if (cR_Mas_Sup_Features.CR_Mas_Sup_Features_Ar_Name != null && cR_Mas_Sup_Features.CR_Mas_Sup_Features_En_Name != null &&
                        cR_Mas_Sup_Features.CR_Mas_Sup_Features_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench &&
                        cR_Mas_Sup_Features.CR_Mas_Sup_Features_Ar_Name.Length >= 3 && cR_Mas_Sup_Features.CR_Mas_Sup_Features_En_Name.Length >= 3 &&
                        cR_Mas_Sup_Features.CR_Mas_Sup_Features_Fr_Name.Length >= 3)
                    {
                        cR_Mas_Sup_Features.CR_Mas_Sup_Features_Code = GetLastRecord().CR_Mas_Sup_Features_Code;
                        db.CR_Mas_Sup_Features.Add(cR_Mas_Sup_Features);
                        db.SaveChanges();
                        cR_Mas_Sup_Features = new CR_Mas_Sup_Features();
                        cR_Mas_Sup_Features = GetLastRecord();
                        cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status = "A";
                        TempData["TempModel"] = "تم الحفظ بنجاح";
                        return RedirectToAction("Create", "Features");
                    }
                    else
                    {
                        if (cR_Mas_Sup_Features.CR_Mas_Sup_Features_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Features.CR_Mas_Sup_Features_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Features.CR_Mas_Sup_Features_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe)
                            ViewBag.LRExistAr = "عفوا هذه الميزة موجودة";
                        if (LrecordExitEnglish)
                            ViewBag.LRExistEn = "عفوا هذه الميزة موجودة";
                        if (LrecordExitFrench)
                            ViewBag.LRExistFr = "عفوا هذه الميزة موجودة";
                        if (cR_Mas_Sup_Features.CR_Mas_Sup_Features_Ar_Name != null && cR_Mas_Sup_Features.CR_Mas_Sup_Features_Ar_Name.Length < 3)
                            ViewBag.LRExistAr = "عفوا الاسم يحتوي على ما بين 3 و 30 حرفًا";
                        if (cR_Mas_Sup_Features.CR_Mas_Sup_Features_En_Name != null && cR_Mas_Sup_Features.CR_Mas_Sup_Features_En_Name.Length < 3)
                            ViewBag.LRExistEn = "عفوا الاسم يحتوي على ما بين 3 و 30 حرفًا";
                        if (cR_Mas_Sup_Features.CR_Mas_Sup_Features_Fr_Name != null && cR_Mas_Sup_Features.CR_Mas_Sup_Features_Fr_Name.Length < 3)
                            ViewBag.LRExistFr = "عفوا الاسم يحتوي على ما بين 3 و 30 حرفًا";
                    }
                }
            }
            catch (Exception) { }
            return View(cR_Mas_Sup_Features);
        }

        // GET: Features/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Sup_Features cR_Mas_Sup_Features = db.CR_Mas_Sup_Features.Find(id);
            if (cR_Mas_Sup_Features == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status == "A" ||
                    cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status == "Activated" ||
                    cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status == "1" ||
                    cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status == "Undeleted")
                {
                    ViewBag.stat = "حذف";
                    ViewBag.h = "إيقاف";
                }

                if (cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status == "D" ||
                    cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status == "Deleted" ||
                    cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status == "0")
                {
                    ViewBag.stat = "إسترجاع";
                    ViewBag.h = "إيقاف";
                    ViewData["ReadOnly"] = "true";
                }

                if (cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status == "H" ||
                    cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status == "Hold" ||
                    cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status == "2")
                {
                    ViewBag.h = "تنشيط";
                    ViewBag.stat = "حذف";
                    ViewData["ReadOnly"] = "true";
                }

                if (cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status == null)
                {
                    ViewBag.h = "إيقاف";
                    ViewBag.stat = "حذف";
                }
                ViewBag.delete = cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status;
            }
            return View(cR_Mas_Sup_Features);
        }

        // POST: Features/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Mas_Sup_Features_Code, CR_Mas_Sup_Features_Ar_Name, CR_Mas_Sup_Features_En_Name," +
            "CR_Mas_Sup_Features_Fr_Name, CR_Mas_Sup_Features_Status, CR_Mas_Sup_Features_Reasons")]
             CR_Mas_Sup_Features cR_Mas_Sup_Features, string save, string delete, string hold)
        {
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_Sup_Features.Any(f => f.CR_Mas_Sup_Features_Code != cR_Mas_Sup_Features.CR_Mas_Sup_Features_Code &&
                                                                          f.CR_Mas_Sup_Features_Ar_Name == cR_Mas_Sup_Features.CR_Mas_Sup_Features_Ar_Name);
                    var LrecordExitEnglish = db.CR_Mas_Sup_Features.Any(f => f.CR_Mas_Sup_Features_Code != cR_Mas_Sup_Features.CR_Mas_Sup_Features_Code &&
                                                                            f.CR_Mas_Sup_Features_En_Name == cR_Mas_Sup_Features.CR_Mas_Sup_Features_En_Name);
                    var LrecordExitFrench = db.CR_Mas_Sup_Features.Any(f => f.CR_Mas_Sup_Features_Code != cR_Mas_Sup_Features.CR_Mas_Sup_Features_Code &&
                                                                           f.CR_Mas_Sup_Features_Fr_Name == cR_Mas_Sup_Features.CR_Mas_Sup_Features_Fr_Name);


                    if (cR_Mas_Sup_Features.CR_Mas_Sup_Features_Ar_Name != null && cR_Mas_Sup_Features.CR_Mas_Sup_Features_En_Name != null &&
                        cR_Mas_Sup_Features.CR_Mas_Sup_Features_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench &&
                        cR_Mas_Sup_Features.CR_Mas_Sup_Features_Ar_Name.Length >= 3 && cR_Mas_Sup_Features.CR_Mas_Sup_Features_En_Name.Length >= 3 &&
                        cR_Mas_Sup_Features.CR_Mas_Sup_Features_Fr_Name.Length >= 3)
                    {
                        db.Entry(cR_Mas_Sup_Features).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (cR_Mas_Sup_Features.CR_Mas_Sup_Features_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Features.CR_Mas_Sup_Features_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Features.CR_Mas_Sup_Features_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe)
                            ViewBag.LRExistAr = "عفوا هذه الميزة موجودة";
                        if (LrecordExitEnglish)
                            ViewBag.LRExistEn = "عفوا هذه الميزة موجودة";
                        if (LrecordExitFrench)
                            ViewBag.LRExistFr = "عفوا هذه الميزة موجودة";
                        if (cR_Mas_Sup_Features.CR_Mas_Sup_Features_Ar_Name != null && cR_Mas_Sup_Features.CR_Mas_Sup_Features_Ar_Name.Length < 3)
                            ViewBag.LRExistAr = "عفوا الاسم يحتوي على ما بين 3 و 30 حرفًا";
                        if (cR_Mas_Sup_Features.CR_Mas_Sup_Features_En_Name != null && cR_Mas_Sup_Features.CR_Mas_Sup_Features_En_Name.Length < 3)
                            ViewBag.LRExistEn = "عفوا الاسم يحتوي على ما بين 3 و 30 حرفًا";
                        if (cR_Mas_Sup_Features.CR_Mas_Sup_Features_Fr_Name != null && cR_Mas_Sup_Features.CR_Mas_Sup_Features_Fr_Name.Length < 3)
                            ViewBag.LRExistFr = "عفوا الاسم يحتوي على ما بين 3 و 30 حرفًا";
                    }
                }
            }
            if (delete == "Delete" || delete == "حذف")
            {
                cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status = "D";
                db.Entry(cR_Mas_Sup_Features).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (delete == "Activate" || delete == "إسترجاع")
            {
                cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status = "A";
                db.Entry(cR_Mas_Sup_Features).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "إيقاف" || hold == "hold")
            {
                cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status = "H";
                db.Entry(cR_Mas_Sup_Features).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "تنشيط" || hold == "Activate")
            {
                cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status = "A";
                db.Entry(cR_Mas_Sup_Features).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status == "A" ||
            cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status == "Activated" ||
            cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status == "1" ||
            cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }
            if ((cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status == "D" ||
                 cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status == "Deleted" ||
                 cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
            }
            if (cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status == "H" ||
                cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status == "Hold" ||
                cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
            }
            if (cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }
            ViewBag.delete = cR_Mas_Sup_Features.CR_Mas_Sup_Features_Status;
            return View(cR_Mas_Sup_Features);
        }

        //////// GET: Features/Delete/5
        //////public ActionResult Delete(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Sup_Features cR_Mas_Sup_Features = db.CR_Mas_Sup_Features.Find(id);
        //////    if (cR_Mas_Sup_Features == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Sup_Features);
        //////}

        //////// POST: Features/Delete/5
        //////[HttpPost, ActionName("Delete")]
        //////[ValidateAntiForgeryToken]
        //////public ActionResult DeleteConfirmed(string id)
        //////{
        //////    CR_Mas_Sup_Features cR_Mas_Sup_Features = db.CR_Mas_Sup_Features.Find(id);
        //////    db.CR_Mas_Sup_Features.Remove(cR_Mas_Sup_Features);
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