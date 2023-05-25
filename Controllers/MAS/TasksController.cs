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
    public class TasksController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        public static string TaskCode;

        // GET: Tasks
        public ActionResult Index()
        {
            TaskCode = "1902";
            Session["POS"] = "1902";
            if (Session["ST_1902_unhold"].ToString() != "true" || Session["ST_1902_hold"].ToString() != "true" && Session["ST_1902_undelete"].ToString() != "true" || Session["ST_1902_delete"].ToString() != "true")
            {
                var cR_Mas_Sys_Tasks = db.CR_Mas_Sys_Tasks.Include(c => c.CR_Mas_Sys_System_Name).Where(x => x.CR_Mas_Sys_Tasks_Status != "D" && x.CR_Mas_Sys_Tasks_Status != "H");
                return View(cR_Mas_Sys_Tasks.ToList());
            }

            else
                if (Session["ST_1902_unhold"].ToString() != "true" || Session["ST_1902_hold"].ToString() != "true")
            {
                var cR_Mas_Sys_Tasks = db.CR_Mas_Sys_Tasks.Include(c => c.CR_Mas_Sys_System_Name).Where(x => x.CR_Mas_Sys_Tasks_Status != "H");
                return View(cR_Mas_Sys_Tasks.ToList());
            }
            else if (Session["ST_1902_undelete"].ToString() != "true" || Session["ST_1902_delete"].ToString() != "true")
            {
                var cR_Mas_Sys_Tasks = db.CR_Mas_Sys_Tasks.Include(c => c.CR_Mas_Sys_System_Name).Where(x => x.CR_Mas_Sys_Tasks_Status != "D");
                return View(cR_Mas_Sys_Tasks.ToList());
            }
            else
            {
                var cR_Mas_Sys_Tasks = db.CR_Mas_Sys_Tasks.Include(c => c.CR_Mas_Sys_System_Name);
                return View(cR_Mas_Sys_Tasks.ToList());
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
                var taskTable = new System.Data.DataTable("teste");

                taskTable.Columns.Add("الملاحظات", typeof(string));
                taskTable.Columns.Add("الحالة", typeof(string));
                taskTable.Columns.Add("النظام", typeof(string));
                taskTable.Columns.Add("الإسم", typeof(string));
                taskTable.Columns.Add("الرمز", typeof(string));
                var Lrecord = db.CR_Mas_Sys_Tasks.ToList();

                if (Lrecord != null)
                {
                    foreach (var i in Lrecord)
                    {
                        taskTable.Rows.Add(i.CR_Mas_Sys_Tasks_Reasons, i.CR_Mas_Sys_Tasks_Status, i.CR_Mas_Sys_System_Name,
                        i.CR_Mas_Sys_Tasks_Ar_Name, i.CR_Mas_Sys_Tasks_Code);
                    }
                }
                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = taskTable;
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
            return View(db.CR_Mas_Sys_Tasks.ToList());
        }

        //////// GET: Tasks/Details/5
        //////public ActionResult Details(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Sys_Tasks cR_Mas_Sys_Tasks = db.CR_Mas_Sys_Tasks.Find(id);
        //////    if (cR_Mas_Sys_Tasks == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Sys_Tasks);
        //////}

        // GET: Tasks/Create
        public ActionResult Create()
        {
            ViewBag.CR_Mas_Sys_System_Code = new SelectList(db.CR_Mas_Sys_System_Name, "CR_Mas_Sys_System_Code", "CR_Mas_Sys_System_Ar_Name");
            CR_Mas_Sys_Tasks qst = new CR_Mas_Sys_Tasks();
            qst.CR_Mas_Sys_Tasks_Status = "A";
            return View(qst);
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Mas_Sys_Tasks_Code, CR_Mas_Sys_System_Code, CR_Mas_Sys_Tasks_Ar_Name, CR_Mas_Sys_Tasks_En_Name, " +
        "CR_Mas_Sys_Tasks_Fr_Name, CR_Mas_Sys_Tasks_Status, CR_Mas_Sys_Tasks_Reasons")] CR_Mas_Sys_Tasks cR_Mas_Sys_Tasks)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Ar_Name != null && cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_En_Name != null &&
                        cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Fr_Name != null)
                    {
                        db.CR_Mas_Sys_Tasks.Add(cR_Mas_Sys_Tasks);
                        db.SaveChanges();
                        cR_Mas_Sys_Tasks = new CR_Mas_Sys_Tasks();
                        cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status = "A";
                        TempData["TempModel"] = "تم الحفظ بنجاح";
                        return RedirectToAction("Create", "Tasks");
                    }
                    else
                    {
                        if (cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_En_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Fr_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                    }
                }
            }
            catch (Exception) { }
            ViewBag.CR_Mas_Sys_System_Code = new SelectList(db.CR_Mas_Sys_System_Name, "CR_Mas_Sys_System_Code", "CR_Mas_Sys_System_Ar_Name",
                                                           cR_Mas_Sys_Tasks.CR_Mas_Sys_System_Code);
            return View(cR_Mas_Sys_Tasks);
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Sys_Tasks cR_Mas_Sys_Tasks = db.CR_Mas_Sys_Tasks.SingleOrDefault(m => m.CR_Mas_Sys_Tasks_Code == id);
            if (cR_Mas_Sys_Tasks == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status == "A" || cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status == "1")
                {
                    ViewBag.stat = "حذف";
                    ViewBag.h = "إيقاف";
                }

                if ((cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status == "D" || cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status == "0"))
                {
                    ViewBag.stat = "إسترجاع";
                    ViewBag.h = "إيقاف";
                    ViewData["ReadOnly"] = "true";
                }

                if (cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status == "H" || cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status == "2")
                {
                    ViewBag.h = "تنشيط";
                    ViewBag.stat = "حذف";
                    ViewData["ReadOnly"] = "true";
                }

                if (cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status == null)
                {
                    ViewBag.h = "إيقاف";
                    ViewBag.stat = "حذف";
                }
                ViewBag.delete = cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status;
            }

            ViewBag.CR_Mas_Sys_System_Code = new SelectList(db.CR_Mas_Sys_System_Name, "CR_Mas_Sys_System_Code", "CR_Mas_Sys_System_Ar_Name",
                                                            cR_Mas_Sys_Tasks.CR_Mas_Sys_System_Code);
            return View(cR_Mas_Sys_Tasks);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Mas_Sys_Tasks_Code, CR_Mas_Sys_System_Code, CR_Mas_Sys_Tasks_Ar_Name, CR_Mas_Sys_Tasks_En_Name, " +
        "CR_Mas_Sys_Tasks_Fr_Name, CR_Mas_Sys_Tasks_Status, CR_Mas_Sys_Tasks_Reasons")] CR_Mas_Sys_Tasks cR_Mas_Sys_Tasks, string save, string delete, string hold)
        {
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    if (cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Ar_Name != null && cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_En_Name != null &&
                        cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Fr_Name != null)
                    {
                        db.Entry(cR_Mas_Sys_Tasks).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_En_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Fr_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                    }
                }
            }
            if (delete == "Delete" || delete == "حذف")
            {
                cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status = "D";
                db.Entry(cR_Mas_Sys_Tasks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (delete == "Activate" || delete == "إسترجاع")
            {
                cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status = "A";
                db.Entry(cR_Mas_Sys_Tasks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "إيقاف" || hold == "hold")
            {
                cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status = "H";
                db.Entry(cR_Mas_Sys_Tasks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "تنشيط" || hold == "Activate")
            {
                cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status = "A";
                db.Entry(cR_Mas_Sys_Tasks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status == "A" ||
            cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status == "Activated" ||
            cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status == "1" ||
            cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }

            if ((cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status == "D" ||
                 cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status == "Deleted" ||
                 cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
            }

            if (cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status == "H" ||
                cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status == "Hold" ||
                cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
            }

            if (cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }
            ViewBag.CR_Mas_Sys_System_Code = new SelectList(db.CR_Mas_Sys_System_Name, "CR_Mas_Sys_System_Code", "CR_Mas_Sys_System_Ar_Name",
                                                           cR_Mas_Sys_Tasks.CR_Mas_Sys_System_Code);
            ViewBag.delete = cR_Mas_Sys_Tasks.CR_Mas_Sys_Tasks_Status;
            return View(cR_Mas_Sys_Tasks);
        }
        //////// GET: Tasks/Delete/5
        //////public ActionResult Delete(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Sys_Tasks cR_Mas_Sys_Tasks = db.CR_Mas_Sys_Tasks.Find(id);
        //////    if (cR_Mas_Sys_Tasks == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Sys_Tasks);
        //////}

        //////// POST: Tasks/Delete/5
        //////[HttpPost, ActionName("Delete")]
        //////[ValidateAntiForgeryToken]
        //////public ActionResult DeleteConfirmed(string id)
        //////{
        //////    CR_Mas_Sys_Tasks cR_Mas_Sys_Tasks = db.CR_Mas_Sys_Tasks.Find(id);
        //////    db.CR_Mas_Sys_Tasks.Remove(cR_Mas_Sys_Tasks);
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