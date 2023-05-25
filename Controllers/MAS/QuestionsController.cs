using RentCar.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;

namespace RentCar.Controllers
{
    public class QuestionsController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        public static string TaskCode;

        // GET: Questions
        public ActionResult Index()
        {
            TaskCode = "1904";
            Session["POS"] = "1904";
            if (Session["ST_1904_unhold"].ToString() != "true" || Session["ST_1904_hold"].ToString() != "true" && Session["ST_1904_undelete"].ToString() != "true" || Session["ST_1904_delete"].ToString() != "true")
            {
                var cR_Mas_Msg_Questions_Answer = db.CR_Mas_Msg_Questions_Answer.Include(c => c.CR_Mas_Sys_Tasks).Where(x => x.CR_Mas_Msg_Questions_Answer_Status != "D" && x.CR_Mas_Msg_Questions_Answer_Status != "H");
                return View(cR_Mas_Msg_Questions_Answer.ToList());
            }

            else
                if (Session["ST_1904_unhold"].ToString() != "true" || Session["ST_1904_hold"].ToString() != "true")
            {
                var cR_Mas_Msg_Questions_Answer = db.CR_Mas_Msg_Questions_Answer.Include(c => c.CR_Mas_Sys_Tasks).Where(x => x.CR_Mas_Msg_Questions_Answer_Status != "H");
                return View(cR_Mas_Msg_Questions_Answer.ToList());
            }
            else if (Session["ST_1904_undelete"].ToString() != "true" || Session["ST_1904_delete"].ToString() != "true")
            {
                var cR_Mas_Msg_Questions_Answer = db.CR_Mas_Msg_Questions_Answer.Include(c => c.CR_Mas_Sys_Tasks).Where(x => x.CR_Mas_Msg_Questions_Answer_Status != "D");
                return View(cR_Mas_Msg_Questions_Answer.ToList());
            }
            else
            {
                var cR_Mas_Msg_Questions_Answer = db.CR_Mas_Msg_Questions_Answer.Include(c => c.CR_Mas_Sys_Tasks);
                return View(cR_Mas_Msg_Questions_Answer.ToList());
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
                var qstTable = new System.Data.DataTable("teste");

                qstTable.Columns.Add("الملاحظات", typeof(string));
                qstTable.Columns.Add("الحالة", typeof(string));
                qstTable.Columns.Add("المجموعة", typeof(string));
                qstTable.Columns.Add("السؤال", typeof(string));
                qstTable.Columns.Add("الجواب", typeof(string));
                qstTable.Columns.Add("الرمز", typeof(string));
                var Lrecord = db.CR_Mas_Msg_Questions_Answer.ToList();

                if (Lrecord != null)
                {
                    foreach (var i in Lrecord)
                    {
                        qstTable.Rows.Add(i.CR_Mas_Msg_Questions_Answer_Reasons, i.CR_Mas_Msg_Questions_Answer_Status, i.CR_Mas_Msg_Tasks_Code,
                        i.CR_Mas_Msg_Ar_Questions, i.CR_Mas_Msg_Ar_Answer, i.CR_Mas_Msg_Questions_Answer_No);
                    }
                }
                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = qstTable;
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
            return View(db.CR_Mas_Msg_Questions_Answer.ToList());
        }

        public JsonResult GetTaskList(string SystemCode)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<CR_Mas_Sys_Tasks> SystemTask = db.CR_Mas_Sys_Tasks.Where(x => x.CR_Mas_Sys_System_Code == SystemCode).ToList();
            return Json(SystemTask, JsonRequestBehavior.AllowGet);
        }
        //////// GET: Questions/Details/5
        //////public ActionResult Details(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Msg_Questions_Answer cR_Mas_Msg_Questions_Answer = db.CR_Mas_Msg_Questions_Answer.Find(id);
        //////    if (cR_Mas_Msg_Questions_Answer == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Msg_Questions_Answer);
        //////}
        public CR_Mas_Msg_Questions_Answer GetLastRecord()
        {
            var Lrecord = db.CR_Mas_Msg_Questions_Answer.Max(Lr => Lr.CR_Mas_Msg_Questions_Answer_No);
            CR_Mas_Msg_Questions_Answer c = new CR_Mas_Msg_Questions_Answer();
            if (Lrecord != null)
            {
                Int64 val = Int64.Parse(Lrecord) + 1;
                c.CR_Mas_Msg_Questions_Answer_No = val.ToString();
            }
            else
            {
                c.CR_Mas_Msg_Questions_Answer_No = "10";
            }
            return c;
        }
        // GET: Questions/Create
        public ActionResult Create()
        {
            //ViewBag.CR_Mas_Msg_Tasks_Code = new SelectList(db.CR_Mas_Sys_Tasks, "CR_Mas_Sys_Tasks_Code", "CR_Mas_Sys_Tasks_Ar_Name");
            CR_Mas_Msg_Questions_Answer qst = new CR_Mas_Msg_Questions_Answer();
            qst = GetLastRecord();
            qst.CR_Mas_Msg_Questions_Answer_Status = "A";
            ViewBag.CR_Mas_Sup_System_Code = new SelectList(db.CR_Mas_Sys_System_Name, "CR_Mas_Sys_System_Code", "CR_Mas_Sys_System_Ar_Name");
            return View(qst);
        }

        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Mas_Msg_Questions_Answer_No, CR_Mas_Msg_Tasks_Code, CR_Mas_Msg_Ar_Questions, CR_Mas_Msg_Ar_Answer, " +
        "CR_Mas_Msg_En_Questions, CR_Mas_Msg_En_Answer, CR_Mas_Msg_Fr_Questions, CR_Mas_Msg_Fr_Answer, CR_Mas_Msg_Questions_Answer_Status, " +
        "CR_Mas_Msg_Questions_Answer_Reasons")] CR_Mas_Msg_Questions_Answer cR_Mas_Msg_Questions_Answer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Ar_Questions != null && cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Ar_Answer != null &&
                        cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_En_Questions != null && cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_En_Answer != null &&
                        cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Fr_Questions != null && cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Fr_Answer != null)
                    {
                        cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_No = GetLastRecord().CR_Mas_Msg_Questions_Answer_No;
                        db.CR_Mas_Msg_Questions_Answer.Add(cR_Mas_Msg_Questions_Answer);
                        db.SaveChanges();
                        cR_Mas_Msg_Questions_Answer = new CR_Mas_Msg_Questions_Answer();
                        cR_Mas_Msg_Questions_Answer = GetLastRecord();
                        cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status = "A";
                        TempData["TempModel"] = "تم الحفظ بنجاح";
                        return RedirectToAction("Create", "Questions");
                    }
                    else
                    {
                        if (cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Ar_Questions == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Ar_Answer == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_En_Questions == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_En_Answer == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Fr_Questions == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Fr_Answer == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                    }
                }
            }
            catch (Exception) { }
            //ViewBag.CR_Mas_Msg_Tasks_Code = new SelectList(db.CR_Mas_Sys_Tasks, "CR_Mas_Sys_Tasks_Code",
            //"CR_Mas_Sys_Tasks_Ar_Name", cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Tasks_Code);
            ViewBag.CR_Mas_Sup_System_Code = new SelectList(db.CR_Mas_Sys_System_Name, "CR_Mas_Sys_System_Code", "CR_Mas_Sys_System_Ar_Name");
            return View(cR_Mas_Msg_Questions_Answer);
        }

        // GET: Questions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Msg_Questions_Answer cR_Mas_Msg_Questions_Answer = db.CR_Mas_Msg_Questions_Answer.SingleOrDefault(m => m.CR_Mas_Msg_Questions_Answer_No == id);
            if (cR_Mas_Msg_Questions_Answer == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status == "A" || cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status == "1")
                {
                    ViewBag.stat = "حذف";
                    ViewBag.h = "إيقاف";
                }

                if ((cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status == "D" || cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status == "0"))
                {
                    ViewBag.stat = "إسترجاع";
                    ViewBag.h = "إيقاف";
                    ViewData["ReadOnly"] = "true";
                }

                if (cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status == "H" || cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status == "2")
                {
                    ViewBag.h = "تنشيط";
                    ViewBag.stat = "حذف";
                    ViewData["ReadOnly"] = "true";
                }

                if (cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status == null)
                {
                    ViewBag.h = "إيقاف";
                    ViewBag.stat = "حذف";
                }
                ViewBag.delete = cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status;
            }

            //ViewBag.CR_Mas_Msg_Tasks_Code = new SelectList(db.CR_Mas_Sys_Tasks, "CR_Mas_Sys_Tasks_Code",
            //"CR_Mas_Sys_Tasks_Ar_Name", cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Tasks_Code);
            ViewBag.CR_Mas_Sup_System_Code = new SelectList(db.CR_Mas_Sys_System_Name, "CR_Mas_Sys_System_Code", "CR_Mas_Sys_System_Ar_Name");
            return View(cR_Mas_Msg_Questions_Answer);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Mas_Msg_Questions_Answer_No, CR_Mas_Msg_Tasks_Code, CR_Mas_Msg_Ar_Questions, CR_Mas_Msg_Ar_Answer, " +
        "CR_Mas_Msg_En_Questions, CR_Mas_Msg_En_Answer, CR_Mas_Msg_Fr_Questions, CR_Mas_Msg_Fr_Answer, CR_Mas_Msg_Questions_Answer_Status, " +
        "CR_Mas_Msg_Questions_Answer_Reasons")] CR_Mas_Msg_Questions_Answer cR_Mas_Msg_Questions_Answer, string save, string delete, string hold)
        {
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    if (cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Ar_Questions != null && cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Ar_Answer != null &&
                        cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_En_Questions != null && cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_En_Answer != null &&
                        cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Fr_Questions != null && cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Fr_Answer != null)
                    {
                        db.Entry(cR_Mas_Msg_Questions_Answer).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Ar_Questions == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Ar_Answer == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_En_Questions == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_En_Answer == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Fr_Questions == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Fr_Answer == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                    }
                }
            }
            if (delete == "Delete" || delete == "حذف")
            {
                cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status = "D";
                db.Entry(cR_Mas_Msg_Questions_Answer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (delete == "Activate" || delete == "إسترجاع")
            {
                cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status = "A";
                db.Entry(cR_Mas_Msg_Questions_Answer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "إيقاف" || hold == "hold")
            {
                cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status = "H";
                db.Entry(cR_Mas_Msg_Questions_Answer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "تنشيط" || hold == "Activate")
            {
                cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status = "A";
                db.Entry(cR_Mas_Msg_Questions_Answer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status == "A" ||
            cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status == "Activated" ||
            cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status == "1" ||
            cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }

            if ((cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status == "D" ||
                 cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status == "Deleted" ||
                 cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
            }

            if (cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status == "H" ||
                cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status == "Hold" ||
                cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
            }

            if (cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }

            //ViewBag.CR_Mas_Msg_Tasks_Code = new SelectList(db.CR_Mas_Sys_Tasks, "CR_Mas_Sys_Tasks_Code","CR_Mas_Sys_Tasks_Ar_Name", cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Tasks_Code);
            ViewBag.CR_Mas_Sup_System_Code = new SelectList(db.CR_Mas_Sys_System_Name, "CR_Mas_Sys_System_Code", "CR_Mas_Sys_System_Ar_Name");
            ViewBag.delete = cR_Mas_Msg_Questions_Answer.CR_Mas_Msg_Questions_Answer_Status;
            return View(cR_Mas_Msg_Questions_Answer);
        }
        //////// GET: Questions/Delete/5
        //////public ActionResult Delete(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Msg_Questions_Answer cR_Mas_Msg_Questions_Answer = db.CR_Mas_Msg_Questions_Answer.Find(id);
        //////    if (cR_Mas_Msg_Questions_Answer == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Msg_Questions_Answer);
        //////}

        //////// POST: Model/Delete/5
        //////[HttpPost, ActionName("Delete")]
        //////[ValidateAntiForgeryToken]
        //////public ActionResult DeleteConfirmed(string id)
        //////{
        //////    CR_Mas_Msg_Questions_Answer cR_Mas_Msg_Questions_Answer = db.CR_Mas_Msg_Questions_Answer.Find(id);
        //////    db.CR_Mas_Msg_Questions_Answer.Remove(cR_Mas_Msg_Questions_Answer);
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