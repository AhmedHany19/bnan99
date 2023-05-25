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
    public class CityController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        public static string TaskCode;

        // GET: City
        public ActionResult Index()
        {
            TaskCode = "1605";
            Session["POS"] = "1605";
            if (Session["ST_1605_unhold"].ToString() != "true" || Session["ST_1605_hold"].ToString() != "true" && Session["ST_1605_undelete"].ToString() != "true" || Session["ST_1605_delete"].ToString() != "true")
            {
                var cR_Mas_Sup_City = db.CR_Mas_Sup_City.Include(c => c.CR_Mas_Sup_Regions).Include(c => c.CR_Mas_Sup_Group).Where(stat => stat.CR_Mas_Sup_City_Status != "H" && stat.CR_Mas_Sup_City_Status != "D");
                return View(cR_Mas_Sup_City.ToList());
            }
            else
                if (Session["ST_1605_unhold"].ToString() != "true" || Session["ST_1605_hold"].ToString() != "true")
            {
                var cR_Mas_Sup_City = db.CR_Mas_Sup_City.Include(c => c.CR_Mas_Sup_Regions).Include(c => c.CR_Mas_Sup_Group).Where(stat => stat.CR_Mas_Sup_City_Status != "H");
                return View(cR_Mas_Sup_City.ToList());
            }
            else if (Session["ST_1605_undelete"].ToString() != "true" || Session["ST_1605_delete"].ToString() != "true")
            {
                var cR_Mas_Sup_City = db.CR_Mas_Sup_City.Include(c => c.CR_Mas_Sup_Regions).Include(c => c.CR_Mas_Sup_Group).Where(stat => stat.CR_Mas_Sup_City_Status != "D");
                return View(cR_Mas_Sup_City.ToList());
            }
            else
            {
                var cR_Mas_Sup_City = db.CR_Mas_Sup_City.Include(c => c.CR_Mas_Sup_Regions).Include(c => c.CR_Mas_Sup_Group);
                return View(cR_Mas_Sup_City.ToList());
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
                var cityTable = new System.Data.DataTable("teste");

                cityTable.Columns.Add("الملاحظات", typeof(string));
                cityTable.Columns.Add("الحالة", typeof(string));
                cityTable.Columns.Add("المجموعة", typeof(string));
                //cityTable.Columns.Add("الماركة", typeof(string));
                cityTable.Columns.Add("رقم العداد", typeof(string));
                cityTable.Columns.Add("الإسم", typeof(string));
                cityTable.Columns.Add("الرمز", typeof(string));
                var Lrecord = db.CR_Mas_Sup_City.ToList();

                if (Lrecord != null)
                {
                    foreach (var i in Lrecord)
                    {
                        cityTable.Rows.Add(i.CR_Mas_Sup_City_Reasons, i.CR_Mas_Sup_City_Status, i.CR_Mas_Sup_City_Group_Code, i.CR_Mas_Sup_City_Regions_Code,
                                           i.CR_Mas_Sup_City_Counter, i.CR_Mas_Sup_City_Ar_Name, i.CR_Mas_Sup_City_Code, i.CR_Mas_Sup_City_Location_Coordinates);
                    }
                }

                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = cityTable;
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
            return View(db.CR_Mas_Sup_City.ToList());
        }

        //////// GET: City/Details/5
        //////public ActionResult Details(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Sup_City cR_Mas_Sup_City = db.CR_Mas_Sup_City.Find(id);
        //////    if (cR_Mas_Sup_City == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Sup_City);
        //////}
        public CR_Mas_Sup_City GetLastRecord()
        {
            var Lrecord = db.CR_Mas_Sup_City.Max(Lr => Lr.CR_Mas_Sup_City_Code);
            CR_Mas_Sup_City m = new CR_Mas_Sup_City();
            if (Lrecord != null)
            {
                Int64 val = Int64.Parse(Lrecord) + 1;
                m.CR_Mas_Sup_City_Code = val.ToString();
            }
            else
            {
                m.CR_Mas_Sup_City_Code = "1700000001";
            }
            return m;
        }
        // GET: City/Create
        public ActionResult Create()
        {

            if (Session["ST_1605_unhold"].ToString() != "true" || Session["ST_1605_hold"].ToString() != "true" && Session["ST_1605_undelete"].ToString() != "true" || Session["ST_1605_delete"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_City_Regions_Code = new SelectList(db.CR_Mas_Sup_Regions.Where(x => x.CR_Mas_Sup_Regions_Status != "D" && x.CR_Mas_Sup_Regions_Status != "H"), "CR_Mas_Sup_Regions_Code", "CR_Mas_Sup_Regions_Ar_Name");
            }
            else
                if (Session["ST_1605_unhold"].ToString() != "true" || Session["ST_1605_hold"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_City_Regions_Code = new SelectList(db.CR_Mas_Sup_Regions.Where(x => x.CR_Mas_Sup_Regions_Status != "H"), "CR_Mas_Sup_Regions_Code", "CR_Mas_Sup_Regions_Ar_Name");
            }
            else if (Session["ST_1605_undelete"].ToString() != "true" || Session["ST_1605_delete"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_City_Regions_Code = new SelectList(db.CR_Mas_Sup_Regions.Where(x => x.CR_Mas_Sup_Regions_Status != "D"), "CR_Mas_Sup_Regions_Code", "CR_Mas_Sup_Regions_Ar_Name");
            }
            else
            {
                ViewBag.CR_Mas_Sup_City_Regions_Code = new SelectList(db.CR_Mas_Sup_Regions, "CR_Mas_Sup_Regions_Code", "CR_Mas_Sup_Regions_Ar_Name");
            }

            ViewBag.CR_Mas_Sup_City_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code", "CR_Mas_Sup_Group_Ar_Name");
            CR_Mas_Sup_City mod = new CR_Mas_Sup_City();
            mod = GetLastRecord();
            mod.CR_Mas_Sup_City_Status = "A";
            return View(mod);
        }

        // POST: City/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Mas_Sup_City_Code, CR_Mas_Sup_City_Group_Code, CR_Mas_Sup_City_Regions_Code, CR_Mas_Sup_City_Ar_Name, " +
        "CR_Mas_Sup_City_En_Name, CR_Mas_Sup_City_Fr_Name, CR_Mas_Sup_City_Counter, CR_Mas_Sup_City_Location_Coordinates, CR_Mas_Sup_City_Status, " +
        "CR_Mas_Sup_City_Reasons")] CR_Mas_Sup_City cR_Mas_Sup_City, string CR_Mas_Sup_City_Ar_Name, string CR_Mas_Sup_City_Fr_Name, string CR_Mas_Sup_City_En_Name)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_Sup_City.Any(Lr => Lr.CR_Mas_Sup_City_Ar_Name == CR_Mas_Sup_City_Ar_Name);
                    var LrecordExitEnglish = db.CR_Mas_Sup_City.Any(Lr => Lr.CR_Mas_Sup_City_En_Name == CR_Mas_Sup_City_En_Name);
                    var LrecordExitFrench = db.CR_Mas_Sup_City.Any(Lr => Lr.CR_Mas_Sup_City_Fr_Name == CR_Mas_Sup_City_Fr_Name);


                    if (cR_Mas_Sup_City.CR_Mas_Sup_City_Ar_Name != null && cR_Mas_Sup_City.CR_Mas_Sup_City_En_Name != null &&
                        cR_Mas_Sup_City.CR_Mas_Sup_City_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench &&
                        cR_Mas_Sup_City.CR_Mas_Sup_City_Ar_Name.Length >= 3 && cR_Mas_Sup_City.CR_Mas_Sup_City_En_Name.Length >= 3 &&
                        cR_Mas_Sup_City.CR_Mas_Sup_City_Fr_Name.Length >= 3)
                    {
                        cR_Mas_Sup_City.CR_Mas_Sup_City_Code = GetLastRecord().CR_Mas_Sup_City_Code;
                        cR_Mas_Sup_City.CR_Mas_Sup_City_Group_Code = "17";
                        db.CR_Mas_Sup_City.Add(cR_Mas_Sup_City);
                        db.SaveChanges();
                        cR_Mas_Sup_City = new CR_Mas_Sup_City();
                        cR_Mas_Sup_City = GetLastRecord();
                        cR_Mas_Sup_City.CR_Mas_Sup_City_Status = "A";
                        TempData["TempModel"] = "تم الحفظ بنجاح";
                        return RedirectToAction("Create", "City");
                    }
                    else
                    {
                        if (cR_Mas_Sup_City.CR_Mas_Sup_City_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_City.CR_Mas_Sup_City_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_City.CR_Mas_Sup_City_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe)
                            ViewBag.LRExistAr = "عفوا هذه المدينة موجودة";
                        if (LrecordExitEnglish)
                            ViewBag.LRExistEn = "عفوا هذه المدينة موجودة";
                        if (LrecordExitFrench)
                            ViewBag.LRExistFr = "عفوا هذه المدينة موجودة";
                        if (cR_Mas_Sup_City.CR_Mas_Sup_City_Ar_Name != null && cR_Mas_Sup_City.CR_Mas_Sup_City_Ar_Name.Length < 3)
                            ViewBag.LRExistAr = "عفوا الاسم يحتوي على ما بين 3 و 30 حرفًا";
                        if (cR_Mas_Sup_City.CR_Mas_Sup_City_En_Name != null && cR_Mas_Sup_City.CR_Mas_Sup_City_En_Name.Length < 3)
                            ViewBag.LRExistEn = "عفوا الاسم يحتوي على ما بين 3 و 30 حرفًا";
                        if (cR_Mas_Sup_City.CR_Mas_Sup_City_Fr_Name != null && cR_Mas_Sup_City.CR_Mas_Sup_City_Fr_Name.Length < 3)
                            ViewBag.LRExistFr = "عفوا الاسم يحتوي على ما بين 3 و 30 حرفًا";
                    }
                }
            }
            catch (Exception) { }
            ViewBag.CR_Mas_Sup_City_Regions_Code = new SelectList(db.CR_Mas_Sup_Regions, "CR_Mas_Sup_Regions_Code", "CR_Mas_Sup_Regions_Ar_Name",
                                                                 cR_Mas_Sup_City.CR_Mas_Sup_City_Regions_Code);
            ViewBag.CR_Mas_Sup_City_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code", "CR_Mas_Sup_Group_Ar_Name",
                                                                 cR_Mas_Sup_City.CR_Mas_Sup_City_Group_Code);
            return View(cR_Mas_Sup_City);
        }

        // GET: City/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Sup_City cR_Mas_Sup_City = db.CR_Mas_Sup_City.Find(id);
            if (cR_Mas_Sup_City == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Mas_Sup_City.CR_Mas_Sup_City_Status == "A" || cR_Mas_Sup_City.CR_Mas_Sup_City_Status == "1")
                {
                    ViewBag.stat = "حذف";
                    ViewBag.h = "إيقاف";
                }
                if ((cR_Mas_Sup_City.CR_Mas_Sup_City_Status == "D" || cR_Mas_Sup_City.CR_Mas_Sup_City_Status == "0"))
                {
                    ViewBag.stat = "إسترجاع";
                    ViewBag.h = "إيقاف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Mas_Sup_City.CR_Mas_Sup_City_Status == "H" || cR_Mas_Sup_City.CR_Mas_Sup_City_Status == "2")
                {
                    ViewBag.h = "تنشيط";
                    ViewBag.stat = "حذف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Mas_Sup_City.CR_Mas_Sup_City_Status == null)
                {
                    ViewBag.h = "إيقاف";
                    ViewBag.stat = "حذف";
                }
                ViewBag.delete = cR_Mas_Sup_City.CR_Mas_Sup_City_Status;
            }

            ViewBag.CR_Mas_Sup_City_Regions_Code = new SelectList(db.CR_Mas_Sup_Regions, "CR_Mas_Sup_Regions_Code", "CR_Mas_Sup_Regions_Ar_Name",
                                                                 cR_Mas_Sup_City.CR_Mas_Sup_City_Regions_Code);
            ViewBag.CR_Mas_Sup_City_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code", "CR_Mas_Sup_Group_Ar_Name",
                                                                 cR_Mas_Sup_City.CR_Mas_Sup_City_Group_Code);
            return View(cR_Mas_Sup_City);
        }

        // POST: City/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Mas_Sup_City_Code, CR_Mas_Sup_City_Group_Code, CR_Mas_Sup_City_Regions_Code, " +
        "CR_Mas_Sup_City_Ar_Name, CR_Mas_Sup_City_En_Name, CR_Mas_Sup_City_Fr_Name, CR_Mas_Sup_City_Counter, CR_Mas_Sup_City_Location_Coordinates, " +
        "CR_Mas_Sup_City_Status, CR_Mas_Sup_City_Reasons")] CR_Mas_Sup_City cR_Mas_Sup_City, string save, string delete, string hold)
        {
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_Sup_City.Any(c => c.CR_Mas_Sup_City_Code != cR_Mas_Sup_City.CR_Mas_Sup_City_Code &&
                                                                  c.CR_Mas_Sup_City_Ar_Name == cR_Mas_Sup_City.CR_Mas_Sup_City_Ar_Name);
                    var LrecordExitEnglish = db.CR_Mas_Sup_City.Any(c => c.CR_Mas_Sup_City_Code != cR_Mas_Sup_City.CR_Mas_Sup_City_Code &&
                                                                    c.CR_Mas_Sup_City_En_Name == cR_Mas_Sup_City.CR_Mas_Sup_City_En_Name);
                    var LrecordExitFrench = db.CR_Mas_Sup_City.Any(c => c.CR_Mas_Sup_City_Code != cR_Mas_Sup_City.CR_Mas_Sup_City_Code &&
                                                                   c.CR_Mas_Sup_City_Fr_Name == cR_Mas_Sup_City.CR_Mas_Sup_City_Fr_Name);

                    if (cR_Mas_Sup_City.CR_Mas_Sup_City_Ar_Name != null && cR_Mas_Sup_City.CR_Mas_Sup_City_En_Name != null &&
                        cR_Mas_Sup_City.CR_Mas_Sup_City_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench &&
                        cR_Mas_Sup_City.CR_Mas_Sup_City_Ar_Name.Length >= 3 && cR_Mas_Sup_City.CR_Mas_Sup_City_En_Name.Length >= 3 &&
                        cR_Mas_Sup_City.CR_Mas_Sup_City_Fr_Name.Length >= 3)
                    {
                        db.Entry(cR_Mas_Sup_City).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (cR_Mas_Sup_City.CR_Mas_Sup_City_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_City.CR_Mas_Sup_City_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_City.CR_Mas_Sup_City_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe)
                            ViewBag.LRExistAr = "عفوا هذه المدينة موجودة";
                        if (LrecordExitEnglish)
                            ViewBag.LRExistEn = "عفوا هذه المدينة موجودة";
                        if (LrecordExitFrench)
                            ViewBag.LRExistFr = "عفوا هذه المدينة موجودة";
                        if (cR_Mas_Sup_City.CR_Mas_Sup_City_Ar_Name != null && cR_Mas_Sup_City.CR_Mas_Sup_City_Ar_Name.Length < 3)
                            ViewBag.LRExistAr = "عفوا الاسم يحتوي على ما بين 3 و 30 حرفًا";
                        if (cR_Mas_Sup_City.CR_Mas_Sup_City_En_Name != null && cR_Mas_Sup_City.CR_Mas_Sup_City_En_Name.Length < 3)
                            ViewBag.LRExistEn = "عفوا الاسم يحتوي على ما بين 3 و 30 حرفًا";
                        if (cR_Mas_Sup_City.CR_Mas_Sup_City_Fr_Name != null && cR_Mas_Sup_City.CR_Mas_Sup_City_Fr_Name.Length < 3)
                            ViewBag.LRExistFr = "عفوا الاسم يحتوي على ما بين 3 و 30 حرفًا";
                    }
                }
            }
            if (delete == "Delete" || delete == "حذف")
            {
                cR_Mas_Sup_City.CR_Mas_Sup_City_Status = "D";
                db.Entry(cR_Mas_Sup_City).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (delete == "Activate" || delete == "إسترجاع")
            {
                cR_Mas_Sup_City.CR_Mas_Sup_City_Status = "A";
                db.Entry(cR_Mas_Sup_City).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "إيقاف" || hold == "hold")
            {
                cR_Mas_Sup_City.CR_Mas_Sup_City_Status = "H";
                db.Entry(cR_Mas_Sup_City).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "تنشيط" || hold == "Activate")
            {
                cR_Mas_Sup_City.CR_Mas_Sup_City_Status = "A";
                db.Entry(cR_Mas_Sup_City).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (cR_Mas_Sup_City.CR_Mas_Sup_City_Status == "A" ||
            cR_Mas_Sup_City.CR_Mas_Sup_City_Status == "Activated" ||
            cR_Mas_Sup_City.CR_Mas_Sup_City_Status == "1" ||
            cR_Mas_Sup_City.CR_Mas_Sup_City_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }
            if ((cR_Mas_Sup_City.CR_Mas_Sup_City_Status == "D" ||
                 cR_Mas_Sup_City.CR_Mas_Sup_City_Status == "Deleted" ||
                 cR_Mas_Sup_City.CR_Mas_Sup_City_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
            }
            if (cR_Mas_Sup_City.CR_Mas_Sup_City_Status == "H" ||
                cR_Mas_Sup_City.CR_Mas_Sup_City_Status == "Hold" ||
                cR_Mas_Sup_City.CR_Mas_Sup_City_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
            }
            if (cR_Mas_Sup_City.CR_Mas_Sup_City_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }
            ViewBag.CR_Mas_Sup_City_Regions_Code = new SelectList(db.CR_Mas_Sup_Regions, "CR_Mas_Sup_Regions_Code", "CR_Mas_Sup_Regions_Ar_Name", cR_Mas_Sup_City.CR_Mas_Sup_City_Regions_Code);
            ViewBag.CR_Mas_Sup_City_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code", "CR_Mas_Sup_Group_Ar_Name", cR_Mas_Sup_City.CR_Mas_Sup_City_Group_Code);
            ViewBag.delete = cR_Mas_Sup_City.CR_Mas_Sup_City_Status;
            return View(cR_Mas_Sup_City);
        }
        //////// GET: City/Delete/5
        //////public ActionResult Delete(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Sup_City cR_Mas_Sup_City = db.CR_Mas_Sup_City.Find(id);
        //////    if (cR_Mas_Sup_City == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Sup_City);
        //////}
        //////// POST: City/Delete/5
        //////[HttpPost, ActionName("Delete")]
        //////[ValidateAntiForgeryToken]
        //////public ActionResult DeleteConfirmed(string id)
        //////{
        //////    CR_Mas_Sup_City cR_Mas_Sup_City = db.CR_Mas_Sup_City.Find(id);
        //////    db.CR_Mas_Sup_City.Remove(cR_Mas_Sup_City);
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