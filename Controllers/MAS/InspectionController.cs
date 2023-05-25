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
    public class InspectionController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        public static string TaskCode;
        // GET: Inspection
        public ActionResult Index()
        {
            TaskCode = "1803";
            Session["POS"] = "1803";
            if (Session["ST_1803_unhold"].ToString() != "true" || Session["ST_1803_hold"].ToString() != "true" && Session["ST_1803_undelete"].ToString() != "true" || Session["ST_1803_delete"].ToString() != "true")
            {
                var inspection = from CR_Mas_Sup_Virtual_Inspection in db.CR_Mas_Sup_Virtual_Inspection
                                 where CR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status != "H" && CR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status != "D"
                                 select CR_Mas_Sup_Virtual_Inspection;
                return View(inspection);
            }

            else
                if (Session["ST_1803_unhold"].ToString() != "true" || Session["ST_1803_hold"].ToString() != "true")
            {
                var inspection = (List<CR_Mas_Sup_Virtual_Inspection>)db.CR_Mas_Sup_Virtual_Inspection.Where(x => x.CR_Mas_Sup_Virtual_Inspection_Status != "H");
                return View(inspection);
            }
            else if (Session["ST_1803_undelete"].ToString() != "true" || Session["ST_1803_delete"].ToString() != "true")
            {
                var inspection = db.CR_Mas_Sup_Virtual_Inspection.Where(x => x.CR_Mas_Sup_Virtual_Inspection_Status != "D");
                return View(inspection);
            }
            else
            {
                return View(db.CR_Mas_Sup_Virtual_Inspection.ToList());
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
                Service.Columns.Add("النوع", typeof(string));
                Service.Columns.Add("الإسم", typeof(string));
                Service.Columns.Add("الرمز", typeof(string));
                var Lrecord = db.CR_Mas_Sup_Virtual_Inspection.ToList();
                if (Lrecord != null)
                {
                    foreach (var i in Lrecord)
                    {
                        Service.Rows.Add(i.CR_Mas_Sup_Virtual_Inspection_Reasons, i.CR_Mas_Sup_Virtual_Inspection_Type, i.CR_Mas_Sup_Virtual_Inspection_Ar_Name,
                                            i.CR_Mas_Sup_Virtual_Inspection_Code);
                    }
                }
                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = Service;
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Inspection.xls");
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

        // GET: Inspection/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Sup_Virtual_Inspection cR_Mas_Sup_Virtual_Inspection = db.CR_Mas_Sup_Virtual_Inspection.Find(id);
            if (cR_Mas_Sup_Virtual_Inspection == null)
            {
                return HttpNotFound();
            }
            return View(cR_Mas_Sup_Virtual_Inspection);
        }



        public CR_Mas_Sup_Virtual_Inspection GetLastRecord()
        {
            var Lrecord = db.CR_Mas_Sup_Virtual_Inspection.Max(Lr => Lr.CR_Mas_Sup_Virtual_Inspection_Code);
            CR_Mas_Sup_Virtual_Inspection b = new CR_Mas_Sup_Virtual_Inspection();
            if (Lrecord != null)
            {
                int val = int.Parse(Lrecord) + 1;
                b.CR_Mas_Sup_Virtual_Inspection_Code = val.ToString();
            }
            else
            {
                b.CR_Mas_Sup_Virtual_Inspection_Code = "10";
            }
            return b;
        }





        // GET: Inspection/Create
        public ActionResult Create()
        {
            CR_Mas_Sup_Virtual_Inspection Inspection = new CR_Mas_Sup_Virtual_Inspection();
            Inspection = GetLastRecord();
            Inspection.CR_Mas_Sup_Virtual_Inspection_Status = "A";
            return View(Inspection);
        }

        // POST: Inspection/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Mas_Sup_Virtual_Inspection_Code,CR_Mas_Sup_Virtual_Inspection_Type" +
            ",CR_Mas_Sup_Virtual_Inspection_Ar_Name,CR_Mas_Sup_Virtual_Inspection_En_Name,CR_Mas_Sup_Virtual_Inspection_Fr_Name" +
            ",CR_Mas_Sup_Virtual_Inspection_Status,CR_Mas_Sup_Virtual_Inspection_Reasons")]
            CR_Mas_Sup_Virtual_Inspection cR_Mas_Sup_Virtual_Inspection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_Sup_Virtual_Inspection.Any(Lr => Lr.CR_Mas_Sup_Virtual_Inspection_Ar_Name == cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Ar_Name);
                    var LrecordExitEnglish = db.CR_Mas_Sup_Virtual_Inspection.Any(Lr => Lr.CR_Mas_Sup_Virtual_Inspection_En_Name == cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_En_Name);
                    var LrecordExitFrench = db.CR_Mas_Sup_Virtual_Inspection.Any(Lr => Lr.CR_Mas_Sup_Virtual_Inspection_Fr_Name == cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Fr_Name);


                    if (cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Ar_Name != null && cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_En_Name != null &&
                        cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench)
                    {
                        cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Code = GetLastRecord().CR_Mas_Sup_Virtual_Inspection_Code;
                        db.CR_Mas_Sup_Virtual_Inspection.Add(cR_Mas_Sup_Virtual_Inspection);
                        db.SaveChanges();
                        cR_Mas_Sup_Virtual_Inspection = new CR_Mas_Sup_Virtual_Inspection();
                        //cR_Mas_Sup_Virtual_Inspection = GetLastRecord();
                        cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status = "A";
                        TempData["TempModel"] = "تم الحفظ بنجاح";
                        return RedirectToAction("Create", "Inspection");
                    }
                    else
                    {
                        if (cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Virtual_Inspection.Any(b => b.CR_Mas_Sup_Virtual_Inspection_Status == "A" && b.CR_Mas_Sup_Virtual_Inspection_Ar_Name == cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Ar_Name))
                            ViewBag.LRExistAr = "عفوا هذا الفحص مسجل من قبل";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Virtual_Inspection.Any(b => b.CR_Mas_Sup_Virtual_Inspection_Status == "A" && b.CR_Mas_Sup_Virtual_Inspection_En_Name == cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_En_Name))
                            ViewBag.LRExistEn = "عفوا هذا الفحص مسجل من قبل";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Virtual_Inspection.Any(b => b.CR_Mas_Sup_Virtual_Inspection_Status == "A" && b.CR_Mas_Sup_Virtual_Inspection_Fr_Name == cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Fr_Name))
                            ViewBag.LRExistFr = "عفوا هذا الفحص مسجل من قبل";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Virtual_Inspection.Any(b => b.CR_Mas_Sup_Virtual_Inspection_Status == "H" && b.CR_Mas_Sup_Virtual_Inspection_Ar_Name == cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Ar_Name))
                            ViewBag.LRExistAr = "عفوا هذا الفحص مسجل من قبل (موقوف)";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Virtual_Inspection.Any(b => b.CR_Mas_Sup_Virtual_Inspection_Status == "H" && b.CR_Mas_Sup_Virtual_Inspection_En_Name == cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_En_Name))
                            ViewBag.LRExistEn = "عفوا هذا الفحص مسجل من قبل (موقوف)";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Virtual_Inspection.Any(b => b.CR_Mas_Sup_Virtual_Inspection_Status == "H" && b.CR_Mas_Sup_Virtual_Inspection_Fr_Name == cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Fr_Name))
                            ViewBag.LRExistFr = "عفوا هذا الفحص مسجل من قبل (موقوف)";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Virtual_Inspection.Any(b => b.CR_Mas_Sup_Virtual_Inspection_Status == "D" && b.CR_Mas_Sup_Virtual_Inspection_Ar_Name == cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Ar_Name))
                            ViewBag.LRExistAr = "عفوا هذا الفحص مسجل من قبل (محذوف)";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Virtual_Inspection.Any(b => b.CR_Mas_Sup_Virtual_Inspection_Status == "D" && b.CR_Mas_Sup_Virtual_Inspection_En_Name == cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_En_Name))
                            ViewBag.LRExistEn = "عفوا هذا الفحص مسجل من قبل (محذوف)";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Virtual_Inspection.Any(b => b.CR_Mas_Sup_Virtual_Inspection_Status == "D" && b.CR_Mas_Sup_Virtual_Inspection_Fr_Name == cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Fr_Name))
                            ViewBag.LRExistFr = "عفوا هذا الفحص مسجل من قبل (محذوف)";

                    }
                }
            }
            catch (Exception)
            {

            }

            return View(cR_Mas_Sup_Virtual_Inspection);
        }

        // GET: Inspection/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Sup_Virtual_Inspection cR_Mas_Sup_Virtual_Inspection = db.CR_Mas_Sup_Virtual_Inspection.Find(id);
            if (cR_Mas_Sup_Virtual_Inspection == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status == "A" ||
                    cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status == "Activated" ||
                    cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status == "1" ||
                    cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status == "Undeleted")
                {
                    ViewBag.stat = "حذف";
                    ViewBag.h = "إيقاف";
                }
                if ((cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status == "D" ||
                    cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status == "Deleted" ||
                    cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status == "0"))
                {
                    ViewBag.stat = "إسترجاع";
                    ViewBag.h = "إيقاف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status == "H" ||
                    cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status == "Hold" ||
                    cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status == "2")
                {
                    ViewBag.h = "تنشيط";
                    ViewBag.stat = "حذف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status == null)
                {
                    ViewBag.h = "إيقاف";
                    ViewBag.stat = "حذف";
                }
                ViewBag.delete = cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status;
            }
            return View(cR_Mas_Sup_Virtual_Inspection);
        }

        // POST: Inspection/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Mas_Sup_Virtual_Inspection_Code,CR_Mas_Sup_Virtual_Inspection_Type," +
            "CR_Mas_Sup_Virtual_Inspection_Ar_Name,CR_Mas_Sup_Virtual_Inspection_En_Name,CR_Mas_Sup_Virtual_Inspection_Fr_Name," +
            "CR_Mas_Sup_Virtual_Inspection_Status,CR_Mas_Sup_Virtual_Inspection_Reasons")]
            CR_Mas_Sup_Virtual_Inspection cR_Mas_Sup_Virtual_Inspection, string save, string delete, string hold)
        {
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_Sup_Virtual_Inspection.Any(Lr => Lr.CR_Mas_Sup_Virtual_Inspection_Ar_Name == cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Ar_Name && Lr.CR_Mas_Sup_Virtual_Inspection_Code != cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Code);
                    var LrecordExitEnglish = db.CR_Mas_Sup_Virtual_Inspection.Any(Lr => Lr.CR_Mas_Sup_Virtual_Inspection_En_Name == cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_En_Name && Lr.CR_Mas_Sup_Virtual_Inspection_Code != cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Code);
                    var LrecordExitFrench = db.CR_Mas_Sup_Virtual_Inspection.Any(Lr => Lr.CR_Mas_Sup_Virtual_Inspection_Fr_Name == cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Fr_Name && Lr.CR_Mas_Sup_Virtual_Inspection_Code != cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Code);


                    if (cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Ar_Name != null && cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_En_Name != null &&
                        cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench)

                    {
                        db.Entry(cR_Mas_Sup_Virtual_Inspection).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["TempModif"] = "تم التعديل بنجاح";
                        //System.Threading.Thread.Sleep(4000);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Virtual_Inspection.Any(b => b.CR_Mas_Sup_Virtual_Inspection_Status == "A" && b.CR_Mas_Sup_Virtual_Inspection_Ar_Name == cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Ar_Name && b.CR_Mas_Sup_Virtual_Inspection_Code != cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Code))
                            ViewBag.LRExistAr = "عفوا هذا الفحص مسجل من قبل";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Virtual_Inspection.Any(b => b.CR_Mas_Sup_Virtual_Inspection_Status == "A" && b.CR_Mas_Sup_Virtual_Inspection_En_Name == cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_En_Name && b.CR_Mas_Sup_Virtual_Inspection_Code != cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Code))
                            ViewBag.LRExistEn = "عفوا هذا الفحص مسجل من قبل";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Virtual_Inspection.Any(b => b.CR_Mas_Sup_Virtual_Inspection_Status == "A" && b.CR_Mas_Sup_Virtual_Inspection_Fr_Name == cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Fr_Name && b.CR_Mas_Sup_Virtual_Inspection_Code != cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Code))
                            ViewBag.LRExistFr = "عفوا هذا الفحص مسجل من قبل";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Virtual_Inspection.Any(b => b.CR_Mas_Sup_Virtual_Inspection_Status == "H" && b.CR_Mas_Sup_Virtual_Inspection_Ar_Name == cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Ar_Name && b.CR_Mas_Sup_Virtual_Inspection_Code != cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Code))
                            ViewBag.LRExistAr = "عفوا هذا الفحص مسجل من قبل (موقوف)";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Virtual_Inspection.Any(b => b.CR_Mas_Sup_Virtual_Inspection_Status == "H" && b.CR_Mas_Sup_Virtual_Inspection_En_Name == cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_En_Name && b.CR_Mas_Sup_Virtual_Inspection_Code != cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Code))
                            ViewBag.LRExistEn = "عفوا هذا الفحص مسجل من قبل (موقوف)";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Virtual_Inspection.Any(b => b.CR_Mas_Sup_Virtual_Inspection_Status == "H" && b.CR_Mas_Sup_Virtual_Inspection_Fr_Name == cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Fr_Name && b.CR_Mas_Sup_Virtual_Inspection_Code != cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Code))
                            ViewBag.LRExistFr = "عفوا هذا الفحص مسجل من قبل (موقوف)";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Virtual_Inspection.Any(b => b.CR_Mas_Sup_Virtual_Inspection_Status == "D" && b.CR_Mas_Sup_Virtual_Inspection_Ar_Name == cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Ar_Name && b.CR_Mas_Sup_Virtual_Inspection_Code != cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Code))
                            ViewBag.LRExistAr = "عفوا هذا الفحص مسجل من قبل (محذوف)";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Virtual_Inspection.Any(b => b.CR_Mas_Sup_Virtual_Inspection_Status == "D" && b.CR_Mas_Sup_Virtual_Inspection_En_Name == cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_En_Name && b.CR_Mas_Sup_Virtual_Inspection_Code != cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Code))
                            ViewBag.LRExistEn = "عفوا هذا الفحص مسجل من قبل (محذوف)";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Virtual_Inspection.Any(b => b.CR_Mas_Sup_Virtual_Inspection_Status == "D" && b.CR_Mas_Sup_Virtual_Inspection_Fr_Name == cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Fr_Name && b.CR_Mas_Sup_Virtual_Inspection_Code != cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Code))
                            ViewBag.LRExistFr = "عفوا هذا الفحص مسجل من قبل (محذوف)";

                    }
                }
            }
            if (delete == "Delete" || delete == "حذف")
            {
                cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status = "D";

                db.Entry(cR_Mas_Sup_Virtual_Inspection).State = EntityState.Modified;
                db.SaveChanges();
                TempData["TempModif"] = "تم الحذف بنجاح";
                return RedirectToAction("Index");
            }
            if (delete == "Activate" || delete == "إسترجاع")
            {
                cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status = "A";
                db.Entry(cR_Mas_Sup_Virtual_Inspection).State = EntityState.Modified;
                db.SaveChanges();
                TempData["TempModif"] = "تم الإسترجاع بنجاح";
                return RedirectToAction("Index");
            }
            if (hold == "إيقاف" || hold == "hold")
            {
                cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status = "H";
                db.Entry(cR_Mas_Sup_Virtual_Inspection).State = EntityState.Modified;
                db.SaveChanges();
                TempData["TempModif"] = "تم الإيقاف بنجاح";
                return RedirectToAction("Index");
            }
            if (hold == "تنشيط" || hold == "Activate")
            {
                cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status = "A";
                db.Entry(cR_Mas_Sup_Virtual_Inspection).State = EntityState.Modified;
                db.SaveChanges();
                TempData["TempModif"] = "تم التنشيط بنجاح";
                return RedirectToAction("Index");
            }
            if (cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status == "A" ||
            cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status == "Activated" ||
            cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status == "1" ||
            cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }
            if ((cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status == "D" ||
                 cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status == "Deleted" ||
                 cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
            }
            if (cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status == "H" ||
                cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status == "Hold" ||
                cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
            }
            if (cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }
            ViewBag.delete = cR_Mas_Sup_Virtual_Inspection.CR_Mas_Sup_Virtual_Inspection_Status;
            return View(cR_Mas_Sup_Virtual_Inspection);
        }

        // GET: Inspection/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Sup_Virtual_Inspection cR_Mas_Sup_Virtual_Inspection = db.CR_Mas_Sup_Virtual_Inspection.Find(id);
            if (cR_Mas_Sup_Virtual_Inspection == null)
            {
                return HttpNotFound();
            }
            return View(cR_Mas_Sup_Virtual_Inspection);
        }

        // POST: Inspection/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Mas_Sup_Virtual_Inspection cR_Mas_Sup_Virtual_Inspection = db.CR_Mas_Sup_Virtual_Inspection.Find(id);
            db.CR_Mas_Sup_Virtual_Inspection.Remove(cR_Mas_Sup_Virtual_Inspection);
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
