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
    public class UserController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        public static string TaskCode;

        // GET: User_Information
        [ActionName("Index")]
        public ActionResult Index_Get()
        {
            TaskCode = "1701";
            Session["POS"] = "1701";
            if (Session["ST_1701_unhold"].ToString() != "true" || Session["ST_1701_hold"].ToString() != "true" && Session["ST_1701_undelete"].ToString() != "true" || Session["ST_1701_delete"].ToString() != "true")
            {
                var UserLIst = from CR_Mas_User_Information in db.CR_Mas_User_Information
                               where CR_Mas_User_Information.CR_Mas_User_Information_Status != "H" && CR_Mas_User_Information.CR_Mas_User_Information_Status != "D"
                               select CR_Mas_User_Information;
                return View(UserLIst);
            }

            else
                if (Session["ST_1701_unhold"].ToString() != "true" || Session["ST_1701_hold"].ToString() != "true")
            {
                var UserLIst = db.CR_Mas_User_Information.Where(x => x.CR_Mas_User_Information_Status != "H");
                return View(UserLIst);
            }
            else if (Session["ST_1701_undelete"].ToString() != "true" || Session["ST_1701_delete"].ToString() != "true")
            {
                var UserLIst = db.CR_Mas_User_Information.Where(x => x.CR_Mas_User_Information_Status != "D");
                return View(UserLIst);
            }
            else
            {
                return View(db.CR_Mas_User_Information.ToList());
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
                var userTable = new System.Data.DataTable("teste");

                userTable.Columns.Add("الملاحظات", typeof(string));
                userTable.Columns.Add("رقم الهاتف", typeof(string));
                userTable.Columns.Add("الحالة", typeof(string));
                userTable.Columns.Add("الإسم العربي", typeof(string));
                userTable.Columns.Add("الرمز", typeof(string));
                var Lrecord = db.CR_Mas_User_Information.ToList();


                if (Lrecord != null)
                {
                    foreach (var i in Lrecord)
                    {
                        userTable.Rows.Add(i.CR_Mas_User_Information_Reasons, i.CR_Mas_User_Information_Mobile_No, i.CR_Mas_User_Information_Status,
                                            i.CR_Mas_User_Information_Ar_Name, i.CR_Mas_User_Information_Code);
                    }
                }

                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = userTable;
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
            return View(db.CR_Mas_User_Information.ToList());
        }
        //////// GET: User_Information/Details/5
        //////public ActionResult Details(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_User_Information cR_Mas_User_Information = db.CR_Mas_User_Information.Find(id);
        //////    if (cR_Mas_User_Information == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_User_Information);
        //////}
        public CR_Mas_User_Information GetLastRecord()
        {
            var Lrecord = db.CR_Mas_User_Information.Max(Lr => Lr.CR_Mas_User_Information_Code);
            CR_Mas_User_Information u = new CR_Mas_User_Information();
            if (Lrecord != null)
            {
                int val = int.Parse(Lrecord) + 1;
                u.CR_Mas_User_Information_Code = val.ToString();
            }
            else
            {
                u.CR_Mas_User_Information_Code = "10";
            }
            return u;
        }
        // GET: User_Information/Create
        public ActionResult Create()
        {
            CR_Mas_User_Information user = new CR_Mas_User_Information();
            user = GetLastRecord();
            user.CR_Mas_User_Information_Status = "A";
            user.CR_Mas_User_Information_PassWord = user.CR_Mas_User_Information_Code;
            return View(user);
        }

        // POST: User_Information/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Mas_User_Information_Code, CR_Mas_User_Information_PassWord, CR_Mas_User_Information_Ar_Name, " +
        "CR_Mas_User_Information_En_Name, CR_Mas_User_Information_Fr_Name, CR_Mas_User_Information_Mobile_No, CR_Mas_User_Information_Status, " +
        "CR_Mas_User_Information_Reasons")] CR_Mas_User_Information cR_Mas_User_Information, string CR_Mas_User_Information_Ar_Name,
         string CR_Mas_User_Information_Fr_Name, string CR_Mas_User_Information_En_Name)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_User_Information.Any(Lr => Lr.CR_Mas_User_Information_Ar_Name == CR_Mas_User_Information_Ar_Name);
                    var LrecordExitEnglish = db.CR_Mas_User_Information.Any(Lr => Lr.CR_Mas_User_Information_En_Name == CR_Mas_User_Information_En_Name);
                    var LrecordExitFrench = db.CR_Mas_User_Information.Any(Lr => Lr.CR_Mas_User_Information_Fr_Name == CR_Mas_User_Information_Fr_Name);

                    var checkUserExist = db.CR_Cas_User_Information.Any(Lr => Lr.CR_Cas_User_Information_Id == cR_Mas_User_Information.CR_Mas_User_Information_Code);


                    if (cR_Mas_User_Information.CR_Mas_User_Information_Ar_Name != null && cR_Mas_User_Information.CR_Mas_User_Information_En_Name != null &&
                        cR_Mas_User_Information.CR_Mas_User_Information_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench &&
                        cR_Mas_User_Information.CR_Mas_User_Information_Ar_Name.Length >= 3 && cR_Mas_User_Information.CR_Mas_User_Information_En_Name.Length >= 3 &&
                        cR_Mas_User_Information.CR_Mas_User_Information_Fr_Name.Length >= 3 && !checkUserExist)
                    {
                        cR_Mas_User_Information.CR_Mas_User_Information_Code = GetLastRecord().CR_Mas_User_Information_Code;
                        db.CR_Mas_User_Information.Add(cR_Mas_User_Information);

                        /////////////////////creating user sub validation///////////
                        var tasks = db.CR_Mas_Sys_Tasks.AsEnumerable().Where(x => Convert.ToInt32(x.CR_Mas_Sys_Tasks_Code) >= Convert.ToInt32("1101"));
                        List<CR_Mas_User_Sub_Validation> lr = new List<CR_Mas_User_Sub_Validation>();
                        CR_Mas_User_Sub_Validation subval;
                        foreach (var t in tasks)
                        {
                            subval = new CR_Mas_User_Sub_Validation();
                            subval.CR_Mas_User_Sub_Validation_Code = cR_Mas_User_Information.CR_Mas_User_Information_Code;
                            subval.CR_Mas_User_Sub_Validation_Delete = false;
                            subval.CR_Mas_User_Sub_Validation_Hold = false;
                            subval.CR_Mas_User_Sub_Validation_Insert = false;
                            subval.CR_Mas_User_Sub_Validation_Print = false;
                            subval.CR_Mas_User_Sub_Validation_Tasks_Code = t.CR_Mas_Sys_Tasks_Code;
                            subval.CR_Mas_User_Sub_Validation_UnDelete = false;
                            subval.CR_Mas_User_Sub_Validation_UnHold = false;
                            subval.CR_Mas_User_Sub_Validation_UpDate = false;
                            lr.Add(subval);
                        }
                        lr.ForEach(s => db.CR_Mas_User_Sub_Validation.Add(s));

                        /////////////////////////////////////////////////////


                        /////////////////////creating user main validation///////////
                        tasks = db.CR_Mas_Sys_Tasks.AsEnumerable().Where(x => Convert.ToInt32(x.CR_Mas_Sys_Tasks_Code) >= Convert.ToInt32("1004"));
                        List<CR_Mas_User_Main_Validation> lmain = new List<CR_Mas_User_Main_Validation>();
                        CR_Mas_User_Main_Validation mainval;
                        foreach (var t in tasks)
                        {
                            mainval = new CR_Mas_User_Main_Validation();
                            mainval.CR_Mas_User_Main_Validation_Code = cR_Mas_User_Information.CR_Mas_User_Information_Code;
                            mainval.CR_Mas_User_Main_Validation_Tasks_Code = t.CR_Mas_Sys_Tasks_Code;
                            mainval.CR_Mas_User_Main_Validation1 = false;
                            lmain.Add(mainval);
                        }
                        lmain.ForEach(s => db.CR_Mas_User_Main_Validation.Add(s));

                        /////////////////////////////////////////////////////
                        db.SaveChanges();
                        cR_Mas_User_Information = new CR_Mas_User_Information();
                        cR_Mas_User_Information = GetLastRecord();
                        cR_Mas_User_Information.CR_Mas_User_Information_Status = "A";
                        cR_Mas_User_Information.CR_Mas_User_Information_PassWord = cR_Mas_User_Information.CR_Mas_User_Information_Code;

                        TempData["TempModel"] = "تم الحفظ بنجاح";
                        return RedirectToAction("Create", "User");
                    }
                    else
                    {
                        if (cR_Mas_User_Information.CR_Mas_User_Information_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_User_Information.CR_Mas_User_Information_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_User_Information.CR_Mas_User_Information_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe)
                            ViewBag.LRExistAr = "عفوا هذا المستخدم موجود";
                        if (LrecordExitEnglish)
                            ViewBag.LRExistEn = "عفوا هذا المستخدم موجود";
                        if (LrecordExitFrench)
                            ViewBag.LRExistFr = "عفوا هذا المستخدم موجود";
                        if (checkUserExist)
                        {
                            ViewBag.LRExistAr = "عفوا هذا المستخدم موجود";
                            ViewBag.LRExistEn = "عفوا هذا المستخدم موجود";
                            ViewBag.LRExistFr = "عفوا هذا المستخدم موجود";
                        }

                        if (cR_Mas_User_Information.CR_Mas_User_Information_Ar_Name != null && cR_Mas_User_Information.CR_Mas_User_Information_Ar_Name.Length < 3)
                            ViewBag.LRExistAr = "عفوا الاسم يحتوي على ما بين 3 و 50 حرفًا";
                        if (cR_Mas_User_Information.CR_Mas_User_Information_En_Name != null && cR_Mas_User_Information.CR_Mas_User_Information_En_Name.Length < 3)
                            ViewBag.LRExistEn = "عفوا الاسم يحتوي على ما بين 3 و 50 حرفًا";
                        if (cR_Mas_User_Information.CR_Mas_User_Information_Fr_Name != null && cR_Mas_User_Information.CR_Mas_User_Information_Fr_Name.Length < 3)
                            ViewBag.LRExistFr = "عفوا الاسم يحتوي على ما بين 3 و 50 حرفًا";
                    }
                }
            }
            catch (Exception) { }
            return View(cR_Mas_User_Information);
        }

        // GET: User_Information/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_User_Information cR_Mas_User_Information = db.CR_Mas_User_Information.Find(id);
            if (cR_Mas_User_Information == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Mas_User_Information.CR_Mas_User_Information_Status == "A" ||
                    cR_Mas_User_Information.CR_Mas_User_Information_Status == "Activated" ||
                    cR_Mas_User_Information.CR_Mas_User_Information_Status == "1" ||
                    cR_Mas_User_Information.CR_Mas_User_Information_Status == "Undeleted")
                {
                    ViewBag.stat = "حذف";
                    ViewBag.h = "إيقاف";
                }

                if ((cR_Mas_User_Information.CR_Mas_User_Information_Status == "D" ||
                     cR_Mas_User_Information.CR_Mas_User_Information_Status == "Deleted" ||
                     cR_Mas_User_Information.CR_Mas_User_Information_Status == "0"))
                {
                    ViewBag.stat = "إسترجاع";
                    ViewBag.h = "إيقاف";
                    ViewData["ReadOnly"] = "true";
                }

                if (cR_Mas_User_Information.CR_Mas_User_Information_Status == "H" ||
                    cR_Mas_User_Information.CR_Mas_User_Information_Status == "Hold" ||
                    cR_Mas_User_Information.CR_Mas_User_Information_Status == "2")
                {
                    ViewBag.h = "تنشيط";
                    ViewBag.stat = "حذف";
                    ViewData["ReadOnly"] = "true";
                }

                if (cR_Mas_User_Information.CR_Mas_User_Information_Status == null)
                {
                    ViewBag.h = "إيقاف";
                    ViewBag.stat = "حذف";
                }
                ViewBag.delete = cR_Mas_User_Information.CR_Mas_User_Information_Status;
            }
            return View(cR_Mas_User_Information);
        }

        // POST: User_Information/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Mas_User_Information_Code, CR_Mas_User_Information_PassWord, CR_Mas_User_Information_Ar_Name, " +
            "CR_Mas_User_Information_En_Name, CR_Mas_User_Information_Fr_Name, CR_Mas_User_Information_Mobile_No, CR_Mas_User_Information_Status, " +
            "CR_Mas_User_Information_Reasons")] CR_Mas_User_Information cR_Mas_User_Information, string save, string delete, string hold)
        {
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_User_Information.Any(u => u.CR_Mas_User_Information_Code != cR_Mas_User_Information.CR_Mas_User_Information_Code &&
                                                                          u.CR_Mas_User_Information_Ar_Name == cR_Mas_User_Information.CR_Mas_User_Information_Ar_Name);
                    var LrecordExitEnglish = db.CR_Mas_User_Information.Any(u => u.CR_Mas_User_Information_Code != cR_Mas_User_Information.CR_Mas_User_Information_Code &&
                                                                                 u.CR_Mas_User_Information_En_Name == cR_Mas_User_Information.CR_Mas_User_Information_En_Name);
                    var LrecordExitFrench = db.CR_Mas_User_Information.Any(u => u.CR_Mas_User_Information_Code != cR_Mas_User_Information.CR_Mas_User_Information_Code &&
                                                                           u.CR_Mas_User_Information_Fr_Name == cR_Mas_User_Information.CR_Mas_User_Information_Fr_Name);


                    if (cR_Mas_User_Information.CR_Mas_User_Information_Ar_Name != null && cR_Mas_User_Information.CR_Mas_User_Information_En_Name != null &&
                        cR_Mas_User_Information.CR_Mas_User_Information_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench &&
                        cR_Mas_User_Information.CR_Mas_User_Information_Ar_Name.Length >= 3 && cR_Mas_User_Information.CR_Mas_User_Information_En_Name.Length >= 3 &&
                        cR_Mas_User_Information.CR_Mas_User_Information_Fr_Name.Length >= 3)
                    {
                        db.Entry(cR_Mas_User_Information).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (cR_Mas_User_Information.CR_Mas_User_Information_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_User_Information.CR_Mas_User_Information_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_User_Information.CR_Mas_User_Information_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe)
                            ViewBag.LRExistAr = "عفوا هذا المستخدم موجود";
                        if (LrecordExitEnglish)
                            ViewBag.LRExistEn = "عفوا هذا المستخدم موجود";
                        if (LrecordExitFrench)
                            ViewBag.LRExistFr = "عفوا هذا المستخدم موجود";
                        if (cR_Mas_User_Information.CR_Mas_User_Information_Ar_Name != null && cR_Mas_User_Information.CR_Mas_User_Information_Ar_Name.Length < 3)
                            ViewBag.LRExistAr = "عفوا الاسم يحتوي على ما بين 3 و 50 حرفًا";
                        if (cR_Mas_User_Information.CR_Mas_User_Information_En_Name != null && cR_Mas_User_Information.CR_Mas_User_Information_En_Name.Length < 3)
                            ViewBag.LRExistEn = "عفوا الاسم يحتوي على ما بين 3 و 50 حرفًا";
                        if (cR_Mas_User_Information.CR_Mas_User_Information_Fr_Name != null && cR_Mas_User_Information.CR_Mas_User_Information_Fr_Name.Length < 3)
                            ViewBag.LRExistFr = "عفوا الاسم يحتوي على ما بين 3 و 50 حرفًا";
                    }
                }
            }
            if (delete == "Delete" || delete == "حذف")
            {
                cR_Mas_User_Information.CR_Mas_User_Information_Status = "D";
                /*db.CR_Mas_User_Information.Attach(cR_Mas_User_Information);
                db.Entry(cR_Mas_User_Information).Property(b => b.CR_Mas_User_Information_Status).IsModified = true;
                db.Entry(cR_Mas_User_Information).Property(b => b.CR_Mas_User_Information_Mobile_No).IsModified = true;
                db.Entry(cR_Mas_User_Information).Property(b => b.CR_Mas_User_Information_Reasons).IsModified = true;*/
                db.Entry(cR_Mas_User_Information).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (delete == "Activate" || delete == "إسترجاع")
            {
                cR_Mas_User_Information.CR_Mas_User_Information_Status = "A";
                db.Entry(cR_Mas_User_Information).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "إيقاف" || hold == "hold")
            {
                cR_Mas_User_Information.CR_Mas_User_Information_Status = "H";
                db.Entry(cR_Mas_User_Information).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "تنشيط" || hold == "Activate")
            {
                cR_Mas_User_Information.CR_Mas_User_Information_Status = "A";
                db.Entry(cR_Mas_User_Information).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (cR_Mas_User_Information.CR_Mas_User_Information_Status == "A" ||
            cR_Mas_User_Information.CR_Mas_User_Information_Status == "Activated" ||
            cR_Mas_User_Information.CR_Mas_User_Information_Status == "1" ||
            cR_Mas_User_Information.CR_Mas_User_Information_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }

            if ((cR_Mas_User_Information.CR_Mas_User_Information_Status == "D" ||
                 cR_Mas_User_Information.CR_Mas_User_Information_Status == "Deleted" ||
                 cR_Mas_User_Information.CR_Mas_User_Information_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
            }

            if (cR_Mas_User_Information.CR_Mas_User_Information_Status == "H" ||
                cR_Mas_User_Information.CR_Mas_User_Information_Status == "Hold" ||
                cR_Mas_User_Information.CR_Mas_User_Information_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
            }

            if (cR_Mas_User_Information.CR_Mas_User_Information_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }
            ViewBag.delete = cR_Mas_User_Information.CR_Mas_User_Information_Status;
            return View(cR_Mas_User_Information);
        }

        //////// GET: User_Information/Delete/5
        //////public ActionResult Delete(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_User_Information cR_Mas_User_Information = db.CR_Mas_User_Information.Find(id);
        //////    if (cR_Mas_User_Information == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_User_Information);
        //////}

        //////// POST: User_Information/Delete/5
        //////[HttpPost, ActionName("Delete")]
        //////[ValidateAntiForgeryToken]
        //////public ActionResult DeleteConfirmed(string id)
        //////{
        //////    CR_Mas_User_Information cR_Mas_User_Information = db.CR_Mas_User_Information.Find(id);
        //////    db.CR_Mas_User_Information.Remove(cR_Mas_User_Information);
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