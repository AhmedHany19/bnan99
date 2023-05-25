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
    public class ProceduresController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        public static string TaskCode;
        // GET: Procedures
        public ActionResult Index()
        {
            TaskCode = "1801";
            Session["POS"] = "1801";
            if (Session["ST_1801_unhold"].ToString() != "true" || Session["ST_1801_hold"].ToString() != "true" && Session["ST_1801_undelete"].ToString() != "true" || Session["ST_1801_delete"].ToString() != "true")
            {
                var Tamm = from CR_Mas_Sup_Procedures in db.CR_Mas_Sup_Procedures
                           where CR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status != "H" && CR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status != "D"
                           select CR_Mas_Sup_Procedures;
                return View(Tamm);
            }

            else
                if (Session["ST_1801_unhold"].ToString() != "true" || Session["ST_1801_hold"].ToString() != "true")
            {
                var proc = (List<CR_Mas_Sup_Procedures>)db.CR_Mas_Sup_Procedures.Where(x => x.CR_Mas_Sup_Procedures_Status != "H");
                return View(proc);
            }
            else if (Session["ST_1801_undelete"].ToString() != "true" || Session["ST_1801_delete"].ToString() != "true")
            {
                var proc = db.CR_Mas_Sup_Procedures.Where(x => x.CR_Mas_Sup_Procedures_Status != "D");
                return View(proc);
            }
            else
            {
                return View(db.CR_Mas_Sup_Procedures.ToList());
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
                var Service = new System.Data.DataTable("teste");

                Service.Columns.Add("الملاحظات", typeof(string));
                Service.Columns.Add("الحالة", typeof(string));
                Service.Columns.Add("Default_KM", typeof(string));
                Service.Columns.Add("Befor_Expire_KM", typeof(string));
                Service.Columns.Add("Befor_KM", typeof(string));
                Service.Columns.Add("Befor_Expire", typeof(string));
                Service.Columns.Add("Credit_Limit", typeof(string));
                Service.Columns.Add("الإسم", typeof(string));
                Service.Columns.Add("النوع", typeof(string));
                Service.Columns.Add("الرمز", typeof(string));
                var Lrecord = db.CR_Mas_Sup_Procedures.ToList();
                if (Lrecord != null)
                {
                    foreach (var i in Lrecord)
                    {
                        Service.Rows.Add(i.CR_Mas_Sup_Procedures_Reasons, i.CR_Mas_Sup_Procedures_Status, i.CR_Mas_Sup_Procedures_FollowUp_Default_KM
                            , i.CR_Mas_Sup_Procedures_FollowUp_Befor_Expire_KM, i.CR_Mas_Sup_Procedures_FollowUp_Befor_KM, i.CR_Mas_Sup_Procedures_FollowUp_Befor_Expire,
                            i.CR_Mas_Sup_Procedures_FollowUp_Befor_Credit_Limit, i.CR_Mas_Sup_Procedures_Ar_Name, i.CR_Mas_Sup_Procedures_Type, i.CR_Mas_Sup_Procedures_Code);
                    }
                }
                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = Service;
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Procedures.xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return View(db.CR_Mas_Sup_Brand.ToList());
        }


        // GET: Procedures/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Sup_Procedures cR_Mas_Sup_Procedures = db.CR_Mas_Sup_Procedures.Find(id);
            if (cR_Mas_Sup_Procedures == null)
            {
                return HttpNotFound();
            }
            return View(cR_Mas_Sup_Procedures);
        }



        public CR_Mas_Sup_Procedures GetLastRecord()
        {
            var Lrecord = db.CR_Mas_Sup_Procedures.Max(Lr => Lr.CR_Mas_Sup_Procedures_Code);
            CR_Mas_Sup_Procedures b = new CR_Mas_Sup_Procedures();
            if (Lrecord != null)
            {
                int val = int.Parse(Lrecord) + 1;
                b.CR_Mas_Sup_Procedures_Code = val.ToString();
            }
            else
            {
                b.CR_Mas_Sup_Procedures_Code = "10";
            }
            return b;
        }



        // GET: Procedures/Create
        public ActionResult Create()
        {
            CR_Mas_Sup_Procedures proc = new CR_Mas_Sup_Procedures();
            proc = GetLastRecord();
            proc.CR_Mas_Sup_Procedures_Status = "A";
            return View(proc);
        }

        // POST: Procedures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Mas_Sup_Procedures_Code,CR_Mas_Sup_Procedures_Type," +
            "CR_Mas_Sup_Procedures_Ar_Name,CR_Mas_Sup_Procedures_En_Name,CR_Mas_Sup_Procedures_Fr_Name," +
            "CR_Mas_Sup_Procedures_FollowUp_Befor_Credit_Limit,CR_Mas_Sup_Procedures_FollowUp_Befor_Expire," +
            "CR_Mas_Sup_Procedures_FollowUp_Befor_KM,CR_Mas_Sup_Procedures_FollowUp_Befor_Expire_KM," +
            "CR_Mas_Sup_Procedures_FollowUp_Default_KM,CR_Mas_Sup_Procedures_Status,CR_Mas_Sup_Procedures_Reasons")]
            CR_Mas_Sup_Procedures cR_Mas_Sup_Procedures)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_Sup_Procedures.Any(Lr => Lr.CR_Mas_Sup_Procedures_Ar_Name == cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Ar_Name);
                    var LrecordExitEnglish = db.CR_Mas_Sup_Procedures.Any(Lr => Lr.CR_Mas_Sup_Procedures_En_Name == cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_En_Name);
                    var LrecordExitFrench = db.CR_Mas_Sup_Procedures.Any(Lr => Lr.CR_Mas_Sup_Procedures_Fr_Name == cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Fr_Name);


                    if (cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Ar_Name != null && cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_En_Name != null &&
                        cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench)
                    {
                        cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Code = GetLastRecord().CR_Mas_Sup_Procedures_Code;
                        db.CR_Mas_Sup_Procedures.Add(cR_Mas_Sup_Procedures);
                        db.SaveChanges();
                        cR_Mas_Sup_Procedures = new CR_Mas_Sup_Procedures();
                        //cR_Mas_Sup_Virtual_Inspection = GetLastRecord();
                        cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status = "A";
                        TempData["TempModel"] = "تم الحفظ بنجاح";
                        return RedirectToAction("Create", "Procedures");
                    }
                    else
                    {
                        if (cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Procedures.Any(b => b.CR_Mas_Sup_Procedures_Status == "A" && b.CR_Mas_Sup_Procedures_Ar_Name == cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Ar_Name))
                            ViewBag.LRExistAr = "عفوا هذا الفحص مسجل من قبل";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Procedures.Any(b => b.CR_Mas_Sup_Procedures_Status == "A" && b.CR_Mas_Sup_Procedures_En_Name == cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_En_Name))
                            ViewBag.LRExistEn = "عفوا هذا الفحص مسجل من قبل";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Procedures.Any(b => b.CR_Mas_Sup_Procedures_Status == "A" && b.CR_Mas_Sup_Procedures_Fr_Name == cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Fr_Name))
                            ViewBag.LRExistFr = "عفوا هذا الفحص مسجل من قبل";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Procedures.Any(b => b.CR_Mas_Sup_Procedures_Status == "H" && b.CR_Mas_Sup_Procedures_Ar_Name == cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Ar_Name))
                            ViewBag.LRExistAr = "عفوا هذا الفحص مسجل من قبل (موقوف)";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Procedures.Any(b => b.CR_Mas_Sup_Procedures_Status == "H" && b.CR_Mas_Sup_Procedures_En_Name == cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_En_Name))
                            ViewBag.LRExistEn = "عفوا هذا الفحص مسجل من قبل (موقوف)";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Procedures.Any(b => b.CR_Mas_Sup_Procedures_Status == "H" && b.CR_Mas_Sup_Procedures_Fr_Name == cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Fr_Name))
                            ViewBag.LRExistFr = "عفوا هذا الفحص مسجل من قبل (موقوف)";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Procedures.Any(b => b.CR_Mas_Sup_Procedures_Status == "D" && b.CR_Mas_Sup_Procedures_Ar_Name == cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Ar_Name))
                            ViewBag.LRExistAr = "عفوا هذا الفحص مسجل من قبل (محذوف)";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Procedures.Any(b => b.CR_Mas_Sup_Procedures_Status == "D" && b.CR_Mas_Sup_Procedures_En_Name == cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_En_Name))
                            ViewBag.LRExistEn = "عفوا هذا الفحص مسجل من قبل (محذوف)";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Procedures.Any(b => b.CR_Mas_Sup_Procedures_Status == "D" && b.CR_Mas_Sup_Procedures_Fr_Name == cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Fr_Name))
                            ViewBag.LRExistFr = "عفوا هذا الفحص مسجل من قبل (محذوف)";

                    }
                }
            }
            catch (Exception)
            {

            }

            return View(cR_Mas_Sup_Procedures);
        }

        // GET: Procedures/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Sup_Procedures cR_Mas_Sup_Procedures = db.CR_Mas_Sup_Procedures.Find(id);
            if (cR_Mas_Sup_Procedures == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status == "A" ||
                    cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status == "Activated" ||
                    cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status == "1" ||
                    cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status == "Undeleted")
                {
                    ViewBag.stat = "حذف";
                    ViewBag.h = "إيقاف";
                }
                if ((cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status == "D" ||
                    cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status == "Deleted" ||
                    cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status == "0"))
                {
                    ViewBag.stat = "إسترجاع";
                    ViewBag.h = "إيقاف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status == "H" ||
                    cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status == "Hold" ||
                    cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status == "2")
                {
                    ViewBag.h = "تنشيط";
                    ViewBag.stat = "حذف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status == null)
                {
                    ViewBag.h = "إيقاف";
                    ViewBag.stat = "حذف";
                }
                ViewBag.delete = cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status;
            }
            return View(cR_Mas_Sup_Procedures);
        }

        // POST: Procedures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Mas_Sup_Procedures_Code,CR_Mas_Sup_Procedures_Type," +
            "CR_Mas_Sup_Procedures_Ar_Name,CR_Mas_Sup_Procedures_En_Name,CR_Mas_Sup_Procedures_Fr_Name," +
            "CR_Mas_Sup_Procedures_FollowUp_Befor_Credit_Limit,CR_Mas_Sup_Procedures_FollowUp_Befor_Expire," +
            "CR_Mas_Sup_Procedures_FollowUp_Befor_KM,CR_Mas_Sup_Procedures_FollowUp_Befor_Expire_KM,CR_Mas_Sup_Procedures_FollowUp_Default_KM," +
            "CR_Mas_Sup_Procedures_Status,CR_Mas_Sup_Procedures_Reasons")]
            CR_Mas_Sup_Procedures cR_Mas_Sup_Procedures, string save, string delete, string hold)
        {
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_Sup_Procedures.Any(Lr => Lr.CR_Mas_Sup_Procedures_Ar_Name == cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Ar_Name && Lr.CR_Mas_Sup_Procedures_Code != cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Code);
                    var LrecordExitEnglish = db.CR_Mas_Sup_Procedures.Any(Lr => Lr.CR_Mas_Sup_Procedures_En_Name == cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_En_Name && Lr.CR_Mas_Sup_Procedures_Code != cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Code);
                    var LrecordExitFrench = db.CR_Mas_Sup_Procedures.Any(Lr => Lr.CR_Mas_Sup_Procedures_Fr_Name == cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Fr_Name && Lr.CR_Mas_Sup_Procedures_Code != cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Code);


                    if (cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Ar_Name != null && cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_En_Name != null &&
                        cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench)

                    {
                        db.Entry(cR_Mas_Sup_Procedures).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["TempModif"] = "تم التعديل بنجاح";
                        //System.Threading.Thread.Sleep(4000);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Procedures.Any(b => b.CR_Mas_Sup_Procedures_Status == "A" && b.CR_Mas_Sup_Procedures_Ar_Name == cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Ar_Name && b.CR_Mas_Sup_Procedures_Code != cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Code))
                            ViewBag.LRExistAr = "عفوا هذا الفحص مسجل من قبل";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Procedures.Any(b => b.CR_Mas_Sup_Procedures_Status == "A" && b.CR_Mas_Sup_Procedures_En_Name == cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_En_Name && b.CR_Mas_Sup_Procedures_Code != cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Code))
                            ViewBag.LRExistEn = "عفوا هذا الفحص مسجل من قبل";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Procedures.Any(b => b.CR_Mas_Sup_Procedures_Status == "A" && b.CR_Mas_Sup_Procedures_Fr_Name == cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Fr_Name && b.CR_Mas_Sup_Procedures_Code != cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Code))
                            ViewBag.LRExistFr = "عفوا هذا الفحص مسجل من قبل";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Procedures.Any(b => b.CR_Mas_Sup_Procedures_Status == "H" && b.CR_Mas_Sup_Procedures_Ar_Name == cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Ar_Name && b.CR_Mas_Sup_Procedures_Code != cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Code))
                            ViewBag.LRExistAr = "عفوا هذا الفحص مسجل من قبل (موقوف)";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Procedures.Any(b => b.CR_Mas_Sup_Procedures_Status == "H" && b.CR_Mas_Sup_Procedures_En_Name == cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_En_Name && b.CR_Mas_Sup_Procedures_Code != cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Code))
                            ViewBag.LRExistEn = "عفوا هذا الفحص مسجل من قبل (موقوف)";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Procedures.Any(b => b.CR_Mas_Sup_Procedures_Status == "H" && b.CR_Mas_Sup_Procedures_Fr_Name == cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Fr_Name && b.CR_Mas_Sup_Procedures_Code != cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Code))
                            ViewBag.LRExistFr = "عفوا هذا الفحص مسجل من قبل (موقوف)";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Procedures.Any(b => b.CR_Mas_Sup_Procedures_Status == "D" && b.CR_Mas_Sup_Procedures_Ar_Name == cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Ar_Name && b.CR_Mas_Sup_Procedures_Code != cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Code))
                            ViewBag.LRExistAr = "عفوا هذا الفحص مسجل من قبل (محذوف)";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Procedures.Any(b => b.CR_Mas_Sup_Procedures_Status == "D" && b.CR_Mas_Sup_Procedures_En_Name == cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_En_Name && b.CR_Mas_Sup_Procedures_Code != cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Code))
                            ViewBag.LRExistEn = "عفوا هذا الفحص مسجل من قبل (محذوف)";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Procedures.Any(b => b.CR_Mas_Sup_Procedures_Status == "D" && b.CR_Mas_Sup_Procedures_Fr_Name == cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Fr_Name && b.CR_Mas_Sup_Procedures_Code != cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Code))
                            ViewBag.LRExistFr = "عفوا هذا الفحص مسجل من قبل (محذوف)";

                    }
                }
            }
            if (delete == "Delete" || delete == "حذف")
            {
                cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status = "D";

                db.Entry(cR_Mas_Sup_Procedures).State = EntityState.Modified;
                db.SaveChanges();
                TempData["TempModif"] = "تم الحذف بنجاح";
                return RedirectToAction("Index");
            }
            if (delete == "Activate" || delete == "إسترجاع")
            {
                cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status = "A";
                db.Entry(cR_Mas_Sup_Procedures).State = EntityState.Modified;
                db.SaveChanges();
                TempData["TempModif"] = "تم الإسترجاع بنجاح";
                return RedirectToAction("Index");
            }
            if (hold == "إيقاف" || hold == "hold")
            {
                cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status = "H";
                db.Entry(cR_Mas_Sup_Procedures).State = EntityState.Modified;
                db.SaveChanges();
                TempData["TempModif"] = "تم الإيقاف بنجاح";
                return RedirectToAction("Index");
            }
            if (hold == "تنشيط" || hold == "Activate")
            {
                cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status = "A";
                db.Entry(cR_Mas_Sup_Procedures).State = EntityState.Modified;
                db.SaveChanges();
                TempData["TempModif"] = "تم التنشيط بنجاح";
                return RedirectToAction("Index");
            }
            if (cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status == "A" ||
            cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status == "Activated" ||
            cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status == "1" ||
            cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }
            if ((cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status == "D" ||
                 cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status == "Deleted" ||
                 cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
            }
            if (cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status == "H" ||
                cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status == "Hold" ||
                cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
            }
            if (cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }
            ViewBag.delete = cR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status;
            return View(cR_Mas_Sup_Procedures);
        }

        // GET: Procedures/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Sup_Procedures cR_Mas_Sup_Procedures = db.CR_Mas_Sup_Procedures.Find(id);
            if (cR_Mas_Sup_Procedures == null)
            {
                return HttpNotFound();
            }
            return View(cR_Mas_Sup_Procedures);
        }

        // POST: Procedures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Mas_Sup_Procedures cR_Mas_Sup_Procedures = db.CR_Mas_Sup_Procedures.Find(id);
            db.CR_Mas_Sup_Procedures.Remove(cR_Mas_Sup_Procedures);
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
