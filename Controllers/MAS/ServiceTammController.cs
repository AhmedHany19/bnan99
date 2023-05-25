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
    public class ServiceTammController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        public static string TaskCode;
        // GET: ServiceTamm
        public ActionResult Index()
        {
            //return View(db.CR_Mas_Sup_Service_Fee_Tamm.ToList());

            TaskCode = "1402";
            Session["POS"] = "1402";
            if (Session["ST_1402_unhold"].ToString() != "true" || Session["ST_1402_hold"].ToString() != "true" && Session["ST_1402_undelete"].ToString() != "true" || Session["ST_1402_delete"].ToString() != "true")
            {
                var Tamm = from CR_Mas_Sup_Service_Fee_Tamm in db.CR_Mas_Sup_Service_Fee_Tamm
                           where CR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status != "H" && CR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status != "D"
                           select CR_Mas_Sup_Service_Fee_Tamm;
                return View(Tamm);
            }

            else
                if (Session["ST_1402_unhold"].ToString() != "true" || Session["ST_1402_hold"].ToString() != "true")
            {
                var Tamm = (List<CR_Mas_Sup_Service_Fee_Tamm>)db.CR_Mas_Sup_Service_Fee_Tamm.Where(x => x.CR_Mas_Sup_Service_Fee_Tamm_Status != "H");
                return View(Tamm);
            }
            else if (Session["ST_1402_undelete"].ToString() != "true" || Session["ST_1402_delete"].ToString() != "true")
            {
                var Tamm = db.CR_Mas_Sup_Service_Fee_Tamm.Where(x => x.CR_Mas_Sup_Service_Fee_Tamm_Status != "D");
                return View(Tamm);
            }
            else
            {
                return View(db.CR_Mas_Sup_Service_Fee_Tamm.ToList());
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
                Service.Columns.Add("القيمة", typeof(string));
                Service.Columns.Add("الخدمة", typeof(string));
                Service.Columns.Add("الرمز", typeof(string));
                var Lrecord = db.CR_Mas_Sup_Service_Fee_Tamm.ToList();
                if (Lrecord != null)
                {
                    foreach (var i in Lrecord)
                    {
                        Service.Rows.Add(i.CR_Mas_Sup_Service_Fee_Tamm_Reasons, i.CR_Mas_Sup_Service_Fee_Tamm_Value, i.CR_Mas_Sup_Service_Fee_Tamm_Ar_Name,
                                            i.CR_Mas_Sup_Service_Fee_Tamm_Code);
                    }
                }
                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = Service;
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=ServiceTamm.xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return View(db.CR_Mas_Sup_Service_Fee_Tamm.ToList());
        }



        public CR_Mas_Sup_Service_Fee_Tamm GetLastRecord()
        {
            var Lrecord = db.CR_Mas_Sup_Service_Fee_Tamm.Max(Lr => Lr.CR_Mas_Sup_Service_Fee_Tamm_Code);
            CR_Mas_Sup_Service_Fee_Tamm b = new CR_Mas_Sup_Service_Fee_Tamm();
            if (Lrecord != null)
            {
                int val = int.Parse(Lrecord) + 1;
                b.CR_Mas_Sup_Service_Fee_Tamm_Code = val.ToString();
            }
            else
            {
                b.CR_Mas_Sup_Service_Fee_Tamm_Code = "10";
            }
            return b;
        }
        //// GET: ServiceTamm/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CR_Mas_Sup_Service_Fee_Tamm cR_Mas_Sup_Service_Fee_Tamm = db.CR_Mas_Sup_Service_Fee_Tamm.Find(id);
        //    if (cR_Mas_Sup_Service_Fee_Tamm == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cR_Mas_Sup_Service_Fee_Tamm);
        //}

        // GET: ServiceTamm/Create
        public ActionResult Create()
        {

            CR_Mas_Sup_Service_Fee_Tamm Tamm = new CR_Mas_Sup_Service_Fee_Tamm();
            Tamm = GetLastRecord();
            Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status = "A";
            return View(Tamm);
        }

        // POST: ServiceTamm/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Mas_Sup_Service_Fee_Tamm_Code,CR_Mas_Sup_Service_Fee_Tamm_Ar_Name," +
            "CR_Mas_Sup_Service_Fee_Tamm_En_Name,CR_Mas_Sup_Service_Fee_Tamm_Fr_Name,CR_Mas_Sup_Service_Fee_Tamm_Value," +
            "CR_Mas_Sup_Service_Fee_Tamm_Status,CR_Mas_Sup_Service_Fee_Tamm_Reasons")]
            CR_Mas_Sup_Service_Fee_Tamm cR_Mas_Sup_Service_Fee_Tamm)
        {



            try
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_Sup_Service_Fee_Tamm.Any(Lr => Lr.CR_Mas_Sup_Service_Fee_Tamm_Ar_Name == cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Ar_Name);
                    var LrecordExitEnglish = db.CR_Mas_Sup_Service_Fee_Tamm.Any(Lr => Lr.CR_Mas_Sup_Service_Fee_Tamm_En_Name == cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_En_Name);
                    var LrecordExitFrench = db.CR_Mas_Sup_Service_Fee_Tamm.Any(Lr => Lr.CR_Mas_Sup_Service_Fee_Tamm_Fr_Name == cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Fr_Name);


                    if (cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Ar_Name != null && cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_En_Name != null &&
                        cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench)
                    {
                        cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Code = GetLastRecord().CR_Mas_Sup_Service_Fee_Tamm_Code;
                        db.CR_Mas_Sup_Service_Fee_Tamm.Add(cR_Mas_Sup_Service_Fee_Tamm);
                        db.SaveChanges();
                        cR_Mas_Sup_Service_Fee_Tamm = new CR_Mas_Sup_Service_Fee_Tamm();
                        cR_Mas_Sup_Service_Fee_Tamm = GetLastRecord();
                        cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status = "A";
                        TempData["TempModel"] = "تم الحفظ بنجاح";
                        return RedirectToAction("Create", "ServiceTamm");
                    }
                    else
                    {
                        if (cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Service_Fee_Tamm.Any(b => b.CR_Mas_Sup_Service_Fee_Tamm_Status == "A" && b.CR_Mas_Sup_Service_Fee_Tamm_Ar_Name == cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Ar_Name))
                            ViewBag.LRExistAr = "عفوا هذه الخدمة مسجلة من قبل";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Service_Fee_Tamm.Any(b => b.CR_Mas_Sup_Service_Fee_Tamm_Status == "A" && b.CR_Mas_Sup_Service_Fee_Tamm_En_Name == cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_En_Name))
                            ViewBag.LRExistEn = "عفوا هذه الخدمة مسجلة من قبل";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Service_Fee_Tamm.Any(b => b.CR_Mas_Sup_Service_Fee_Tamm_Status == "A" && b.CR_Mas_Sup_Service_Fee_Tamm_Fr_Name == cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Fr_Name))
                            ViewBag.LRExistFr = "عفوا هذه الخدمة مسجلة من قبل";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Service_Fee_Tamm.Any(b => b.CR_Mas_Sup_Service_Fee_Tamm_Status == "H" && b.CR_Mas_Sup_Service_Fee_Tamm_Ar_Name == cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Ar_Name))
                            ViewBag.LRExistAr = "عفوا هذه الخدمة مسجلة من قبل (موقوفة)";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Service_Fee_Tamm.Any(b => b.CR_Mas_Sup_Service_Fee_Tamm_Status == "H" && b.CR_Mas_Sup_Service_Fee_Tamm_En_Name == cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_En_Name))
                            ViewBag.LRExistEn = "عفوا هذه الخدمة مسجلة من قبل (موقوفة)";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Service_Fee_Tamm.Any(b => b.CR_Mas_Sup_Service_Fee_Tamm_Status == "H" && b.CR_Mas_Sup_Service_Fee_Tamm_Fr_Name == cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Fr_Name))
                            ViewBag.LRExistFr = "عفوا هذه الخدمة مسجلة  قبل (موقوفة)";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Service_Fee_Tamm.Any(b => b.CR_Mas_Sup_Service_Fee_Tamm_Status == "D" && b.CR_Mas_Sup_Service_Fee_Tamm_Ar_Name == cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Ar_Name))
                            ViewBag.LRExistAr = "عفوا هذه الخدمة مسجلة من قبل (محذوفة)";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Service_Fee_Tamm.Any(b => b.CR_Mas_Sup_Service_Fee_Tamm_Status == "D" && b.CR_Mas_Sup_Service_Fee_Tamm_En_Name == cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_En_Name))
                            ViewBag.LRExistEn = "عفوا هذه الخدمة مسجلة من قبل (محذوفة)";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Service_Fee_Tamm.Any(b => b.CR_Mas_Sup_Service_Fee_Tamm_Status == "D" && b.CR_Mas_Sup_Service_Fee_Tamm_Fr_Name == cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Fr_Name))
                            ViewBag.LRExistFr = "عفوا هذه الخدمة مسجلة من قبل (محذوفة)";

                    }
                }
            }
            catch (Exception)
            {

            }
            return View(cR_Mas_Sup_Service_Fee_Tamm);

        }

        // GET: ServiceTamm/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Sup_Service_Fee_Tamm cR_Mas_Sup_Service_Fee_Tamm = db.CR_Mas_Sup_Service_Fee_Tamm.Find(id);
            if (cR_Mas_Sup_Service_Fee_Tamm == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status == "A" ||
                    cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status == "Activated" ||
                    cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status == "1" ||
                    cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status == "Undeleted")
                {
                    ViewBag.stat = "حذف";
                    ViewBag.h = "إيقاف";
                }
                if ((cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status == "D" ||
                    cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status == "Deleted" ||
                    cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status == "0"))
                {
                    ViewBag.stat = "إسترجاع";
                    ViewBag.h = "إيقاف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status == "H" ||
                    cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status == "Hold" ||
                    cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status == "2")
                {
                    ViewBag.h = "تنشيط";
                    ViewBag.stat = "حذف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status == null)
                {
                    ViewBag.h = "إيقاف";
                    ViewBag.stat = "حذف";
                }
                ViewBag.delete = cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status;
            }
            return View(cR_Mas_Sup_Service_Fee_Tamm);

        }

        // POST: ServiceTamm/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Mas_Sup_Service_Fee_Tamm_Code," +
            "CR_Mas_Sup_Service_Fee_Tamm_Ar_Name,CR_Mas_Sup_Service_Fee_Tamm_En_Name," +
            "CR_Mas_Sup_Service_Fee_Tamm_Fr_Name,CR_Mas_Sup_Service_Fee_Tamm_Value," +
            "CR_Mas_Sup_Service_Fee_Tamm_Status,CR_Mas_Sup_Service_Fee_Tamm_Reasons")]
            CR_Mas_Sup_Service_Fee_Tamm cR_Mas_Sup_Service_Fee_Tamm, string save, string delete, string hold)
        {
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_Sup_Service_Fee_Tamm.Any(Lr => Lr.CR_Mas_Sup_Service_Fee_Tamm_Ar_Name == cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Ar_Name);
                    var LrecordExitEnglish = db.CR_Mas_Sup_Service_Fee_Tamm.Any(Lr => Lr.CR_Mas_Sup_Service_Fee_Tamm_En_Name == cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_En_Name);
                    var LrecordExitFrench = db.CR_Mas_Sup_Service_Fee_Tamm.Any(Lr => Lr.CR_Mas_Sup_Service_Fee_Tamm_Fr_Name == cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Fr_Name);


                    if (cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Ar_Name != null && cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_En_Name != null &&
                        cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench)

                    {
                        db.Entry(cR_Mas_Sup_Service_Fee_Tamm).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["TempModif"] = "تم التعديل بنجاح";
                        //System.Threading.Thread.Sleep(4000);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Service_Fee_Tamm.Any(b => b.CR_Mas_Sup_Service_Fee_Tamm_Status == "A" && b.CR_Mas_Sup_Service_Fee_Tamm_Ar_Name == cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Ar_Name))
                            ViewBag.LRExistAr = "عفوا هذه الخدمة مسجلة من قبل";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Service_Fee_Tamm.Any(b => b.CR_Mas_Sup_Service_Fee_Tamm_Status == "A" && b.CR_Mas_Sup_Service_Fee_Tamm_En_Name == cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_En_Name))
                            ViewBag.LRExistEn = "عفوا هذه الخدمة مسجلة من قبل";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Service_Fee_Tamm.Any(b => b.CR_Mas_Sup_Service_Fee_Tamm_Status == "A" && b.CR_Mas_Sup_Service_Fee_Tamm_Fr_Name == cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Fr_Name))
                            ViewBag.LRExistFr = "عفوا هذه الخدمة مسجلة من قبل";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Service_Fee_Tamm.Any(b => b.CR_Mas_Sup_Service_Fee_Tamm_Status == "H" && b.CR_Mas_Sup_Service_Fee_Tamm_Ar_Name == cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Ar_Name))
                            ViewBag.LRExistAr = "عفوا هذه الخدمة مسجلة من قبل (موقوفة)";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Service_Fee_Tamm.Any(b => b.CR_Mas_Sup_Service_Fee_Tamm_Status == "H" && b.CR_Mas_Sup_Service_Fee_Tamm_En_Name == cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_En_Name))
                            ViewBag.LRExistEn = "عفوا هذه الخدمة مسجلة من قبل (موقوفة)";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Service_Fee_Tamm.Any(b => b.CR_Mas_Sup_Service_Fee_Tamm_Status == "H" && b.CR_Mas_Sup_Service_Fee_Tamm_Fr_Name == cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Fr_Name))
                            ViewBag.LRExistFr = "عفوا هذه الخدمة مسجلة من قبل (موقوفة)";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Service_Fee_Tamm.Any(b => b.CR_Mas_Sup_Service_Fee_Tamm_Status == "D" && b.CR_Mas_Sup_Service_Fee_Tamm_Ar_Name == cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Ar_Name))
                            ViewBag.LRExistAr = "عفوا هذه الخدمة مسجلة من قبل (محذوفة)";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Service_Fee_Tamm.Any(b => b.CR_Mas_Sup_Service_Fee_Tamm_Status == "D" && b.CR_Mas_Sup_Service_Fee_Tamm_En_Name == cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_En_Name))
                            ViewBag.LRExistEn = "عفوا هذه الخدمة مسجلة من قبل (محذوفة)";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Service_Fee_Tamm.Any(b => b.CR_Mas_Sup_Service_Fee_Tamm_Status == "D" && b.CR_Mas_Sup_Service_Fee_Tamm_Fr_Name == cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Fr_Name))
                            ViewBag.LRExistFr = "عفوا هذه الخدمة مسجلة من قبل (محذوفة)";

                    }
                }
            }
            if (delete == "Delete" || delete == "حذف")
            {
                cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status = "D";

                db.Entry(cR_Mas_Sup_Service_Fee_Tamm).State = EntityState.Modified;
                db.SaveChanges();
                TempData["TempModif"] = "تم الحذف بنجاح";
                return RedirectToAction("Index");
            }
            if (delete == "Activate" || delete == "إسترجاع")
            {
                cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status = "A";
                db.Entry(cR_Mas_Sup_Service_Fee_Tamm).State = EntityState.Modified;
                db.SaveChanges();
                TempData["TempModif"] = "تم الإسترجاع بنجاح";
                return RedirectToAction("Index");
            }
            if (hold == "إيقاف" || hold == "hold")
            {
                cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status = "H";
                db.Entry(cR_Mas_Sup_Service_Fee_Tamm).State = EntityState.Modified;
                db.SaveChanges();
                TempData["TempModif"] = "تم الإيقاف بنجاح";
                return RedirectToAction("Index");
            }
            if (hold == "تنشيط" || hold == "Activate")
            {
                cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status = "A";
                db.Entry(cR_Mas_Sup_Service_Fee_Tamm).State = EntityState.Modified;
                db.SaveChanges();
                TempData["TempModif"] = "تم التنشيط بنجاح";
                return RedirectToAction("Index");
            }
            if (cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status == "A" ||
            cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status == "Activated" ||
            cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status == "1" ||
            cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }
            if ((cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status == "D" ||
                 cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status == "Deleted" ||
                 cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
            }
            if (cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status == "H" ||
                cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status == "Hold" ||
                cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
            }
            if (cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }
            ViewBag.delete = cR_Mas_Sup_Service_Fee_Tamm.CR_Mas_Sup_Service_Fee_Tamm_Status;
            return View(cR_Mas_Sup_Service_Fee_Tamm);

        }

        // GET: ServiceTamm/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Sup_Service_Fee_Tamm cR_Mas_Sup_Service_Fee_Tamm = db.CR_Mas_Sup_Service_Fee_Tamm.Find(id);
            if (cR_Mas_Sup_Service_Fee_Tamm == null)
            {
                return HttpNotFound();
            }
            return View(cR_Mas_Sup_Service_Fee_Tamm);
        }

        // POST: ServiceTamm/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Mas_Sup_Service_Fee_Tamm cR_Mas_Sup_Service_Fee_Tamm = db.CR_Mas_Sup_Service_Fee_Tamm.Find(id);
            db.CR_Mas_Sup_Service_Fee_Tamm.Remove(cR_Mas_Sup_Service_Fee_Tamm);
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
