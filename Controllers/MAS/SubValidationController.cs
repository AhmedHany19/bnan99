using RentCar.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;

namespace RentCar.Controllers
{
    public class SubValidationController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        public static string CodeTask = "";
        public static string usercode = "";
        public static string TaskCode = "";
        // GET: SubValidation
        public ActionResult Index()
        {

            TaskCode = "1702";
            Session["POS"] = "1702";


            var cR_Mas_User_Sub_Validation = db.CR_Mas_User_Sub_Validation.Include(c => c.CR_Mas_User_Information);
            ViewBag.CR_Mas_User_Sub_Validation_Code = new SelectList(db.CR_Mas_User_Information, "CR_Mas_User_Information_Code", "CR_Mas_User_Information_Ar_Name");
            return View(cR_Mas_User_Sub_Validation.ToList());

            //var subval = (from sub in db.CR_Mas_User_Sub_Validation
            //              join task in db.CR_Mas_Sys_Tasks on sub.CR_Mas_User_Sub_Validation_Tasks_Code equals task.CR_Mas_Sys_Tasks_Code
            //              select new
            //              {
            //                  CR_Mas_User_Sub_Validation_UserName = sub.CR_Mas_User_Information.CR_Mas_User_Information_Ar_Name,
            //                  CR_Mas_User_Sub_Validation_Code = sub.CR_Mas_User_Sub_Validation_Code,
            //                  CR_Mas_User_Sub_Validation_Tasks_Code = sub.CR_Mas_User_Sub_Validation_Tasks_Code,
            //                  CR_Mas_User_Sub_Validation_Delete = sub.CR_Mas_User_Sub_Validation_Delete,
            //                  CR_Mas_User_Sub_Validation_Insert = sub.CR_Mas_User_Sub_Validation_Insert,
            //                  CR_Mas_User_Sub_Validation_UpDate = sub.CR_Mas_User_Sub_Validation_UpDate,
            //                  CR_Mas_User_Sub_Validation_Hold = sub.CR_Mas_User_Sub_Validation_Hold,
            //                  CR_Mas_User_Sub_Validation_UnDelete = sub.CR_Mas_User_Sub_Validation_UnDelete,
            //                  CR_Mas_User_Sub_Validation_UnHold = sub.CR_Mas_User_Sub_Validation_UnHold,
            //                  CR_Mas_User_Sub_Validation_Print = sub.CR_Mas_User_Sub_Validation_Print,
            //                  CR_Mas_User_Sub_Validation_TaskName = task.CR_Mas_Sys_Tasks_Ar_Name
            //              });

            //var lbda = subval.ToList().Select(x => new CR_Mas_User_Sub_Validation
            //{
            //    CR_Mas_User_Sub_Validation_UserName = x.CR_Mas_User_Sub_Validation_UserName,
            //    CR_Mas_User_Sub_Validation_TaskName = x.CR_Mas_User_Sub_Validation_TaskName,
            //    CR_Mas_User_Sub_Validation_Delete = x.CR_Mas_User_Sub_Validation_Delete,
            //    CR_Mas_User_Sub_Validation_Insert = x.CR_Mas_User_Sub_Validation_Insert,
            //    CR_Mas_User_Sub_Validation_UpDate = x.CR_Mas_User_Sub_Validation_UpDate,
            //    CR_Mas_User_Sub_Validation_Hold = x.CR_Mas_User_Sub_Validation_Hold,
            //    CR_Mas_User_Sub_Validation_UnDelete = x.CR_Mas_User_Sub_Validation_UnDelete,
            //    CR_Mas_User_Sub_Validation_UnHold = x.CR_Mas_User_Sub_Validation_UnHold,
            //    CR_Mas_User_Sub_Validation_Print = x.CR_Mas_User_Sub_Validation_Print,
            //});
            //return View(lbda);
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
                var SubValTable = new System.Data.DataTable("teste");

                SubValTable.Columns.Add("الطباعة", typeof(string));
                SubValTable.Columns.Add("تنشيط", typeof(string));
                SubValTable.Columns.Add("إيقاف", typeof(string));
                SubValTable.Columns.Add("إسترجاع", typeof(string));
                SubValTable.Columns.Add("حذف", typeof(string));
                SubValTable.Columns.Add("تعديل", typeof(string));
                SubValTable.Columns.Add("إضافة", typeof(string));
                SubValTable.Columns.Add("الشاشة", typeof(string));
                SubValTable.Columns.Add("المستخدم", typeof(string));
                var Lrecord = db.CR_Mas_User_Sub_Validation.ToList();
                if (Lrecord != null)
                {
                    foreach (var i in Lrecord)
                    {
                        SubValTable.Rows.Add(i.CR_Mas_User_Sub_Validation_Print, i.CR_Mas_User_Sub_Validation_UnHold, i.CR_Mas_User_Sub_Validation_Hold,
                                            i.CR_Mas_User_Sub_Validation_UnDelete, i.CR_Mas_User_Sub_Validation_Delete, i.CR_Mas_User_Sub_Validation_UpDate,
                                            i.CR_Mas_User_Sub_Validation_Insert, i.CR_Mas_User_Sub_Validation_Tasks_Code,
                                            i.CR_Mas_User_Sub_Validation_Code);
                    }
                }
                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = SubValTable;
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
            return View(db.CR_Mas_User_Sub_Validation.ToList());

        }

        // GET: SubValidation/Create
        public ActionResult Create()
        {
            ViewBag.CR_Mas_User_Sub_Validation_Code = new SelectList(db.CR_Mas_User_Information, "CR_Mas_User_Information_Code", "CR_Mas_User_Information_Ar_Name");
            ViewBag.CR_Mas_Sup_System_Code = new SelectList(db.CR_Mas_Sys_System_Name, "CR_Mas_Sys_System_Code", "CR_Mas_Sys_System_Ar_Name");
            return View();
        }


        public JsonResult GetTaskList(string SystemCode)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<CR_Mas_Sys_Tasks> SystemTask = db.CR_Mas_Sys_Tasks.Where(x => x.CR_Mas_Sys_System_Code == SystemCode).ToList();
            return Json(SystemTask, JsonRequestBehavior.AllowGet);
        }

        // POST: SubValidation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Mas_User_Sub_Validation_Code,CR_Mas_User_Sub_Validation_Tasks_Code," +
            "CR_Mas_User_Sub_Validation_Insert,CR_Mas_User_Sub_Validation_UpDate,CR_Mas_User_Sub_Validation_Delete," +
            "CR_Mas_User_Sub_Validation_UnDelete,CR_Mas_User_Sub_Validation_Hold,CR_Mas_User_Sub_Validation_UnHold," +
            "CR_Mas_User_Sub_Validation_Print")] CR_Mas_User_Sub_Validation cR_Mas_User_Sub_Validation, bool CR_Mas_User_Main_Validation)
        {


            if (ModelState.IsValid)
            {
                var CheckExist = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == cR_Mas_User_Sub_Validation.CR_Mas_User_Sub_Validation_Code &&
                x.CR_Mas_User_Sub_Validation_Tasks_Code == cR_Mas_User_Sub_Validation.CR_Mas_User_Sub_Validation_Tasks_Code);

                if (CheckExist == null && cR_Mas_User_Sub_Validation.CR_Mas_User_Sub_Validation_Code != null && cR_Mas_User_Sub_Validation.CR_Mas_User_Sub_Validation_Tasks_Code != null)
                {

                    CR_Mas_User_Main_Validation mainval = new CR_Mas_User_Main_Validation();
                    mainval.CR_Mas_User_Main_Validation1 = CR_Mas_User_Main_Validation;
                    mainval.CR_Mas_User_Main_Validation_Code = cR_Mas_User_Sub_Validation.CR_Mas_User_Sub_Validation_Code;
                    mainval.CR_Mas_User_Main_Validation_Tasks_Code = cR_Mas_User_Sub_Validation.CR_Mas_User_Sub_Validation_Tasks_Code;
                    db.CR_Mas_User_Main_Validation.Add(mainval);


                    db.CR_Mas_User_Sub_Validation.Add(cR_Mas_User_Sub_Validation);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    if (CheckExist != null)
                        ViewBag.exist = "هذا المستخدم له صلوحية محددة لهذه الشاشة ,الرجاء تعديلها";
                    if (cR_Mas_User_Sub_Validation.CR_Mas_User_Sub_Validation_Code == null)
                    {
                        ViewBag.UserInfo = "رجاء تختار المستخدم";
                    }
                    if (cR_Mas_User_Sub_Validation.CR_Mas_User_Sub_Validation_Tasks_Code == null)
                    {
                        ViewBag.task = "رجاء تختار الشاشة";
                    }

                }
            }

            ViewBag.CR_Mas_User_Sub_Validation_Code = new SelectList(db.CR_Mas_User_Information, "CR_Mas_User_Information_Code", "CR_Mas_User_Information_Ar_Name");
            ViewBag.CR_Mas_Sup_System_Code = new SelectList(db.CR_Mas_Sys_System_Name, "CR_Mas_Sys_System_Code", "CR_Mas_Sys_System_Ar_Name");
            return View(cR_Mas_User_Sub_Validation);
        }


        ////amir test ajax
        //public IHtmlString List_select(string value)
        //{
        //    string htmlcode = "<label>list</label>" +
        //        "             < div class='col-md-12'>"+
        //        "@Html.DropDownList('CR_Mas_Sup_Task_Code', null, htmlAttributes: new { @class = 'input_form col-md-4', style = 'float:right' })"+
        //   " </ div >";
        //    return new HtmlString(htmlcode);
        //}

        // GET: SubValidation/Edit/5
        public ActionResult Edit(string id1, string id2)
        {
            if (id1 == null && id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }
            CR_Mas_User_Sub_Validation cR_Mas_User_Sub_Validation = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == id1 && x.CR_Mas_User_Sub_Validation_Tasks_Code == id2);

            if (cR_Mas_User_Sub_Validation == null)
            {
                return HttpNotFound();
            }
            var mainval = db.CR_Mas_User_Main_Validation.FirstOrDefault(x => x.CR_Mas_User_Main_Validation_Code == id1 && x.CR_Mas_User_Main_Validation_Tasks_Code == id2);
            if (mainval != null)
            {
                ViewBag.mainauthority = mainval.CR_Mas_User_Main_Validation1;
            }
            CodeTask = id2;
            usercode = id1;
            //ViewBag.CR_Mas_User_Sub_Validation_Code = new SelectList(db.CR_Mas_User_Information, "CR_Mas_User_Information_Code", "CR_Mas_User_Information_PassWord", cR_Mas_User_Sub_Validation.CR_Mas_User_Sub_Validation_Code);
            return View(cR_Mas_User_Sub_Validation);
        }

        // POST: SubValidation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Mas_User_Sub_Validation_Code," +
            "CR_Mas_User_Sub_Validation_Tasks_Code,CR_Mas_User_Sub_Validation_Insert," +
            "CR_Mas_User_Sub_Validation_UpDate,CR_Mas_User_Sub_Validation_Delete," +
            "CR_Mas_User_Sub_Validation_UnDelete,CR_Mas_User_Sub_Validation_Hold," +
            "CR_Mas_User_Sub_Validation_UnHold,CR_Mas_User_Sub_Validation_Print")]
            CR_Mas_User_Sub_Validation cR_Mas_User_Sub_Validation, bool CR_Mas_User_Main_Validation, string save, string delete, string hold)
        {
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    cR_Mas_User_Sub_Validation.CR_Mas_User_Sub_Validation_Code = usercode;
                    cR_Mas_User_Sub_Validation.CR_Mas_User_Sub_Validation_Tasks_Code = CodeTask;

                    CR_Mas_User_Main_Validation mainval = new CR_Mas_User_Main_Validation();
                    mainval.CR_Mas_User_Main_Validation1 = CR_Mas_User_Main_Validation;
                    mainval.CR_Mas_User_Main_Validation_Code = cR_Mas_User_Sub_Validation.CR_Mas_User_Sub_Validation_Code;
                    mainval.CR_Mas_User_Main_Validation_Tasks_Code = cR_Mas_User_Sub_Validation.CR_Mas_User_Sub_Validation_Tasks_Code;
                    db.Entry(mainval).State = EntityState.Modified;
                    db.SaveChanges();



                    db.Entry(cR_Mas_User_Sub_Validation).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["TempModif"] = "تم التعديل بنجاح";

                    usercode = "";
                    CodeTask = "";
                    return RedirectToAction("Index");
                }
            }
            //ViewBag.CR_Mas_User_Sub_Validation_Code = new SelectList(db.CR_Mas_User_Information, "CR_Mas_User_Information_Code", "CR_Mas_User_Information_PassWord", cR_Mas_User_Sub_Validation.CR_Mas_User_Sub_Validation_Code);
            return View(cR_Mas_User_Sub_Validation);
        }
        [HttpPost]
        public ActionResult saveupdate(CR_Mas_User_Sub_Validation cR_Mas_User_Sub_Validation)
        {

            CR_Mas_User_Sub_Validation updatedsub = (from c in db.CR_Mas_User_Sub_Validation
                                                     where c.CR_Mas_User_Sub_Validation_Code == cR_Mas_User_Sub_Validation.CR_Mas_User_Sub_Validation_Code.ToString().Trim() &&
                                                     c.CR_Mas_User_Sub_Validation_Tasks_Code == cR_Mas_User_Sub_Validation.CR_Mas_User_Sub_Validation_Tasks_Code.ToString().Trim()
                                                     select c).FirstOrDefault();
            updatedsub.CR_Mas_User_Sub_Validation_Insert = cR_Mas_User_Sub_Validation.CR_Mas_User_Sub_Validation_Insert;
            updatedsub.CR_Mas_User_Sub_Validation_UpDate = cR_Mas_User_Sub_Validation.CR_Mas_User_Sub_Validation_UpDate;
            updatedsub.CR_Mas_User_Sub_Validation_Delete = cR_Mas_User_Sub_Validation.CR_Mas_User_Sub_Validation_Delete;
            updatedsub.CR_Mas_User_Sub_Validation_Hold = cR_Mas_User_Sub_Validation.CR_Mas_User_Sub_Validation_Hold;
            updatedsub.CR_Mas_User_Sub_Validation_UnHold = false;
            updatedsub.CR_Mas_User_Sub_Validation_UnDelete = false;
            updatedsub.CR_Mas_User_Sub_Validation_Print = cR_Mas_User_Sub_Validation.CR_Mas_User_Sub_Validation_Print;

            CR_Mas_User_Main_Validation updatedMain = (from c in db.CR_Mas_User_Main_Validation
                                                       where c.CR_Mas_User_Main_Validation_Code == cR_Mas_User_Sub_Validation.CR_Mas_User_Sub_Validation_Code.ToString().Trim() &&
                                                       c.CR_Mas_User_Main_Validation_Tasks_Code == cR_Mas_User_Sub_Validation.CR_Mas_User_Sub_Validation_Tasks_Code.ToString().Trim()
                                                       select c).FirstOrDefault();
            /*updatedMain.CR_Mas_User_Main_Validation1 = cR_Mas_User_Sub_Validation.CR_Mas_User_Main_Validation1;*/
            db.SaveChanges();

            return new EmptyResult();
        }

        public JsonResult GetUsersList(string Validation_Code)
        {

            db.Configuration.ProxyCreationEnabled = false;
            List<CR_Mas_User_Sub_Validation> lbda = db.CR_Mas_User_Sub_Validation.Where(x => x.CR_Mas_User_Sub_Validation_Code == Validation_Code).ToList();

            //List<CR_Mas_User_Sub_Validation> q = db.CR_Mas_User_Sub_Validation.SqlQuery("select sub.CR_Mas_User_Sub_Validation_Code,sub.CR_Mas_User_Sub_Validation_Delete,sub.CR_Mas_User_Sub_Validation_Hold,sub.CR_Mas_User_Sub_Validation_Insert,sub.CR_Mas_User_Sub_Validation_Print,sub.CR_Mas_User_Sub_Validation_Tasks_Code,sub.CR_Mas_User_Sub_Validation_UnDelete,sub.CR_Mas_User_Sub_Validation_UnHold,sub.CR_Mas_User_Sub_Validation_UpDate,userinf.CR_Mas_User_Information_Ar_Name as CR_Mas_User_Sub_Validation_UserName,mainval.CR_Mas_User_Main_Validation as CR_Mas_User_Main_Validation1,task.CR_Mas_Sys_Tasks_Ar_Name as CR_Mas_User_Sub_Validation_TaskName from RentCarDB.dbo.CR_Mas_User_Sub_Validation as sub,RentCarDB.dbo.CR_Mas_User_Information as userinf,RentCarDB.dbo.CR_Mas_User_Main_Validation as mainval,RentCarDB.dbo.CR_Mas_Sys_Tasks as task where userinf.CR_Mas_User_Information_Code = sub.CR_Mas_User_Sub_Validation_Code and mainval.CR_Mas_User_Main_Validation_Code = userinf.CR_Mas_User_Information_Code and mainval.CR_Mas_User_Main_Validation_Tasks_Code = sub.CR_Mas_User_Sub_Validation_Tasks_Code and task.CR_Mas_Sys_Tasks_Code = mainval.CR_Mas_User_Main_Validation_Tasks_Code and userinf.CR_Mas_User_Information_Code = '" + Validation_Code + "'").ToList();
            //IEnumerable<CR_Mas_User_Sub_Validation> subval = q as IEnumerable<CR_Mas_User_Sub_Validation>;

            return Json(/*new { data = subval }*/lbda, JsonRequestBehavior.AllowGet);
        }

        // GET: SubValidation/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_User_Sub_Validation cR_Mas_User_Sub_Validation = db.CR_Mas_User_Sub_Validation.Find(id);
            if (cR_Mas_User_Sub_Validation == null)
            {
                return HttpNotFound();
            }
            return View(cR_Mas_User_Sub_Validation);
        }

        // POST: SubValidation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Mas_User_Sub_Validation cR_Mas_User_Sub_Validation = db.CR_Mas_User_Sub_Validation.Find(id);
            db.CR_Mas_User_Sub_Validation.Remove(cR_Mas_User_Sub_Validation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
