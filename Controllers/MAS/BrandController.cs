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
    public class BrandController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        public static string TaskCode;
        // GET: Brand
        [ActionName("Index")]
        public ActionResult Index_Get()
        {
            TaskCode = "1501";
            Session["POS"] = "1501";
            if (Session["ST_1501_unhold"].ToString() != "true" || Session["ST_1501_hold"].ToString() != "true" && Session["ST_1501_undelete"].ToString() != "true" || Session["ST_1501_delete"].ToString() != "true")
            {
                var BrandLIst = from CR_Mas_Sup_Brand in db.CR_Mas_Sup_Brand
                                where CR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status != "H" && CR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status != "D"
                                select CR_Mas_Sup_Brand;
                return View(BrandLIst);
            }

            else
                if (Session["ST_1501_unhold"].ToString() != "true" || Session["ST_1501_hold"].ToString() != "true")
            {
                var BrandLIst = (List<CR_Mas_Sup_Brand>)db.CR_Mas_Sup_Brand.Where(x => x.CR_Mas_Sup_Brand_Status != "H");
                return View(BrandLIst);
            }
            else if (Session["ST_1501_undelete"].ToString() != "true" || Session["ST_1501_delete"].ToString() != "true")
            {
                var BrandLIst = db.CR_Mas_Sup_Brand.Where(x => x.CR_Mas_Sup_Brand_Status != "D");
                return View(BrandLIst);
            }
            else
            {
                return View(db.CR_Mas_Sup_Brand.ToList());
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
                var brandTable = new System.Data.DataTable("teste");

                brandTable.Columns.Add("الملاحظات", typeof(string));
                brandTable.Columns.Add("الحالة", typeof(string));
                brandTable.Columns.Add("الإسم العربي", typeof(string));
                brandTable.Columns.Add("الرمز", typeof(string));
                var Lrecord = db.CR_Mas_Sup_Brand.ToList();
                if (Lrecord != null)
                {
                    foreach (var i in Lrecord)
                    {
                        brandTable.Rows.Add(i.CR_Mas_Sup_Brand_Reasons, i.CR_Mas_Sup_Brand_Status, i.CR_Mas_Sup_Brand_Ar_Name,
                                            i.CR_Mas_Sup_Brand_Code);
                    }
                }
                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = brandTable;
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
            return View(db.CR_Mas_Sup_Brand.ToList());
        }

        // GET: Brand/Details/5
        //////public ActionResult Details(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Sup_Brand cR_Mas_Sup_Brand = db.CR_Mas_Sup_Brand.Find(id);
        //////    if (cR_Mas_Sup_Brand == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Sup_Brand);
        //////}

        public CR_Mas_Sup_Brand GetLastRecord()
        {
            var Lrecord = db.CR_Mas_Sup_Brand.Max(Lr => Lr.CR_Mas_Sup_Brand_Code);
            CR_Mas_Sup_Brand b = new CR_Mas_Sup_Brand();
            if (Lrecord != null)
            {
                int val = int.Parse(Lrecord) + 1;
                b.CR_Mas_Sup_Brand_Code = val.ToString();
            }
            else
            {
                b.CR_Mas_Sup_Brand_Code = "3001";
            }
            return b;
        }

        ////public void INSNEW(string ArName,string EngName,string FrName)
        ////{
        ////    CR_Mas_Sup_Brand brand = new CR_Mas_Sup_Brand();
        ////    brand = GetLastRecord();
        ////    brand.CR_Mas_Sup_Brand_Ar_Name = ArName;
        ////    brand.CR_Mas_Sup_Brand_En_Name = EngName;
        ////    brand.CR_Mas_Sup_Brand_Fr_Name = FrName;
        ////    brand.CR_Mas_Sup_Brand_Status = "A";
        ////    db.CR_Mas_Sup_Brand.Add(brand);
        ////    db.SaveChanges();
        ////}
        // GET: Brand/Create
        public ActionResult Create()
        {
            CR_Mas_Sup_Brand brand = new CR_Mas_Sup_Brand();
            brand = GetLastRecord();
            brand.CR_Mas_Sup_Brand_Status = "A";
            return View(brand);
        }

        // POST: Brand/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Mas_Sup_Brand_Code, CR_Mas_Sup_Brand_Ar_Name, CR_Mas_Sup_Brand_En_Name, " +
        "CR_Mas_Sup_Brand_Fr_Name, CR_Mas_Sup_Brand_Status, CR_Mas_Sup_Brand_Reasons")] CR_Mas_Sup_Brand cR_Mas_Sup_Brand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_Sup_Brand.Any(Lr => Lr.CR_Mas_Sup_Brand_Ar_Name == cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Ar_Name);
                    var LrecordExitEnglish = db.CR_Mas_Sup_Brand.Any(Lr => Lr.CR_Mas_Sup_Brand_En_Name == cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_En_Name);
                    var LrecordExitFrench = db.CR_Mas_Sup_Brand.Any(Lr => Lr.CR_Mas_Sup_Brand_Fr_Name == cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Fr_Name);


                    if (cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Ar_Name != null && cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_En_Name != null &&
                        cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench &&
                        cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Ar_Name.Length >= 3 && cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_En_Name.Length >= 3 &&
                        cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Fr_Name.Length >= 3)
                    {
                        cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Code = GetLastRecord().CR_Mas_Sup_Brand_Code;
                        db.CR_Mas_Sup_Brand.Add(cR_Mas_Sup_Brand);
                        db.SaveChanges();
                        cR_Mas_Sup_Brand = new CR_Mas_Sup_Brand();
                        cR_Mas_Sup_Brand = GetLastRecord();
                        cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status = "A";
                        TempData["TempModel"] = "تم الحفظ بنجاح";
                        return RedirectToAction("Create", "Brand");
                    }
                    else
                    {
                        if (cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Brand.Any(b => b.CR_Mas_Sup_Brand_Status == "A" && b.CR_Mas_Sup_Brand_Ar_Name == cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Ar_Name))
                            ViewBag.LRExistAr = "عفوا الماركة مسجلة من قبل";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Brand.Any(b => b.CR_Mas_Sup_Brand_Status == "A" && b.CR_Mas_Sup_Brand_En_Name == cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_En_Name))
                            ViewBag.LRExistEn = "عفوا الماركة مسجلة من قبل";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Brand.Any(b => b.CR_Mas_Sup_Brand_Status == "A" && b.CR_Mas_Sup_Brand_Fr_Name == cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Fr_Name))
                            ViewBag.LRExistFr = "عفوا الماركة مسجلة من قبل";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Brand.Any(b => b.CR_Mas_Sup_Brand_Status == "H" && b.CR_Mas_Sup_Brand_Ar_Name == cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Ar_Name))
                            ViewBag.LRExistAr = "عفوا الماركة مسجلة من قبل (موقوفة)";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Brand.Any(b => b.CR_Mas_Sup_Brand_Status == "H" && b.CR_Mas_Sup_Brand_En_Name == cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_En_Name))
                            ViewBag.LRExistEn = "عفوا الماركة مسجلة من قبل (موقوفة)";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Brand.Any(b => b.CR_Mas_Sup_Brand_Status == "H" && b.CR_Mas_Sup_Brand_Fr_Name == cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Fr_Name))
                            ViewBag.LRExistFr = "عفوا الماركة مسجلة من قبل (موقوفة)";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Brand.Any(b => b.CR_Mas_Sup_Brand_Status == "D" && b.CR_Mas_Sup_Brand_Ar_Name == cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Ar_Name))
                            ViewBag.LRExistAr = "عفوا الماركة مسجلة من قبل (محذوفة)";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Brand.Any(b => b.CR_Mas_Sup_Brand_Status == "D" && b.CR_Mas_Sup_Brand_En_Name == cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_En_Name))
                            ViewBag.LRExistEn = "عفوا الماركة مسجلة من قبل (محذوفة)";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Brand.Any(b => b.CR_Mas_Sup_Brand_Status == "D" && b.CR_Mas_Sup_Brand_Fr_Name == cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Fr_Name))
                            ViewBag.LRExistFr = "عفوا الماركة مسجلة من قبل (محذوفة)";
                        if (cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Ar_Name != null && cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Ar_Name.Length < 3)
                            ViewBag.LRExistAr = "الحد الأدنى ٣ حروف";
                        if (cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_En_Name != null && cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_En_Name.Length < 3)
                            ViewBag.LRExistEn = "الحد الأدنى ٣ حروف";
                        if (cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Fr_Name != null && cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Fr_Name.Length < 3)
                            ViewBag.LRExistFr = "الحد الأدنى ٣ حروف";
                    }
                }
            }
            catch (Exception)
            {
                //return RedirectToAction("Create", "Brand");
                ////CR_Mas_Sup_Brand brand = new CR_Mas_Sup_Brand();
                //cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Code = GetLastRecord().CR_Mas_Sup_Brand_Code;
                //cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status = "A";
                //cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Ar_Name = "A";
                //cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Fr_Name = "A";
                //cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_En_Name = "A";
                //return View(cR_Mas_Sup_Brand);
            }
            return View(cR_Mas_Sup_Brand);
        }

        // GET: Brand/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Sup_Brand cR_Mas_Sup_Brand = db.CR_Mas_Sup_Brand.Find(id);
            if (cR_Mas_Sup_Brand == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status == "A" ||
                    cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status == "Activated" ||
                    cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status == "1" ||
                    cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status == "Undeleted")
                {
                    ViewBag.stat = "حذف";
                    ViewBag.h = "إيقاف";
                }
                if ((cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status == "D" ||
                    cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status == "Deleted" ||
                    cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status == "0"))
                {
                    ViewBag.stat = "إسترجاع";
                    ViewBag.h = "إيقاف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status == "H" ||
                    cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status == "Hold" ||
                    cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status == "2")
                {
                    ViewBag.h = "تنشيط";
                    ViewBag.stat = "حذف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status == null)
                {
                    ViewBag.h = "إيقاف";
                    ViewBag.stat = "حذف";
                }
                ViewBag.delete = cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status;
            }
            return View(cR_Mas_Sup_Brand);
        }

        // POST: Brand/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Mas_Sup_Brand_Code, CR_Mas_Sup_Brand_Ar_Name, CR_Mas_Sup_Brand_En_Name, " +
        "CR_Mas_Sup_Brand_Fr_Name, CR_Mas_Sup_Brand_Status, CR_Mas_Sup_Brand_Reasons")] CR_Mas_Sup_Brand cR_Mas_Sup_Brand,
        string save, string delete, string hold)
        {
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_Sup_Brand.Any(b => b.CR_Mas_Sup_Brand_Code != cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Code &&
                                                                   b.CR_Mas_Sup_Brand_Ar_Name == cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Ar_Name);
                    var LrecordExitEnglish = db.CR_Mas_Sup_Brand.Any(b => b.CR_Mas_Sup_Brand_Code != cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Code &&
                                                                     b.CR_Mas_Sup_Brand_En_Name == cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_En_Name);
                    var LrecordExitFrench = db.CR_Mas_Sup_Brand.Any(b => b.CR_Mas_Sup_Brand_Code != cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Code &&
                                                                    b.CR_Mas_Sup_Brand_Fr_Name == cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Fr_Name);


                    if (cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Ar_Name != null && cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_En_Name != null &&
                        cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench &&
                        cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Ar_Name.Length >= 3 && cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_En_Name.Length >= 3 &&
                        cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Fr_Name.Length >= 3)
                    {
                        db.Entry(cR_Mas_Sup_Brand).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["TempModif"] = "تم التعديل بنجاح";
                        //System.Threading.Thread.Sleep(4000);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Brand.Any(b => b.CR_Mas_Sup_Brand_Status == "A" && b.CR_Mas_Sup_Brand_Ar_Name == cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Ar_Name))
                            ViewBag.LRExistAr = "عفوا الماركة مسجلة من قبل";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Brand.Any(b => b.CR_Mas_Sup_Brand_Status == "A" && b.CR_Mas_Sup_Brand_En_Name == cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_En_Name))
                            ViewBag.LRExistEn = "عفوا الماركة مسجلة من قبل";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Brand.Any(b => b.CR_Mas_Sup_Brand_Status == "A" && b.CR_Mas_Sup_Brand_Fr_Name == cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Fr_Name))
                            ViewBag.LRExistFr = "عفوا الماركة مسجلة من قبل";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Brand.Any(b => b.CR_Mas_Sup_Brand_Status == "H" && b.CR_Mas_Sup_Brand_Ar_Name == cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Ar_Name))
                            ViewBag.LRExistAr = "عفوا الماركة مسجلة من قبل (موقوفة)";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Brand.Any(b => b.CR_Mas_Sup_Brand_Status == "H" && b.CR_Mas_Sup_Brand_En_Name == cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_En_Name))
                            ViewBag.LRExistEn = "عفوا الماركة مسجلة من قبل (موقوفة)";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Brand.Any(b => b.CR_Mas_Sup_Brand_Status == "H" && b.CR_Mas_Sup_Brand_Fr_Name == cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Fr_Name))
                            ViewBag.LRExistFr = "عفوا الماركة مسجلة من قبل (موقوفة)";
                        if (LrecordExitArabe && db.CR_Mas_Sup_Brand.Any(b => b.CR_Mas_Sup_Brand_Status == "D" && b.CR_Mas_Sup_Brand_Ar_Name == cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Ar_Name))
                            ViewBag.LRExistAr = "عفوا الماركة مسجلة من قبل (محذوفة)";
                        if (LrecordExitEnglish && db.CR_Mas_Sup_Brand.Any(b => b.CR_Mas_Sup_Brand_Status == "D" && b.CR_Mas_Sup_Brand_En_Name == cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_En_Name))
                            ViewBag.LRExistEn = "عفوا الماركة مسجلة من قبل (محذوفة)";
                        if (LrecordExitFrench && db.CR_Mas_Sup_Brand.Any(b => b.CR_Mas_Sup_Brand_Status == "D" && b.CR_Mas_Sup_Brand_Fr_Name == cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Fr_Name))
                            ViewBag.LRExistFr = "عفوا الماركة مسجلة من قبل (محذوفة)";
                        if (cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Ar_Name != null && cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Ar_Name.Length < 3)
                            ViewBag.LRExistAr = "الحد الأدنى ٣ حروف";
                        if (cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_En_Name != null && cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_En_Name.Length < 3)
                            ViewBag.LRExistEn = "الحد الأدنى ٣ حروف";
                        if (cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Fr_Name != null && cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Fr_Name.Length < 3)
                            ViewBag.LRExistFr = "الحد الأدنى ٣ حروف";
                    }
                }
            }
            if (delete == "Delete" || delete == "حذف")
            {
                cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status = "D";
                /*db.CR_Mas_Sup_Brand.Attach(cR_Mas_Sup_Brand);
                db.Entry(cR_Mas_Sup_Brand).Property(b => b.CR_Mas_Sup_Brand_Status).IsModified = true;
                db.Entry(cR_Mas_Sup_Brand).Property(b => b.CR_Mas_Sup_Brand_Reasons).IsModified = true;*/
                db.Entry(cR_Mas_Sup_Brand).State = EntityState.Modified;
                db.SaveChanges();
                TempData["TempModif"] = "تم الحذف بنجاح";
                return RedirectToAction("Index");
            }
            if (delete == "Activate" || delete == "إسترجاع")
            {
                cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status = "A";
                db.Entry(cR_Mas_Sup_Brand).State = EntityState.Modified;
                db.SaveChanges();
                TempData["TempModif"] = "تم الإسترجاع بنجاح";
                return RedirectToAction("Index");
            }
            if (hold == "إيقاف" || hold == "hold")
            {
                cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status = "H";
                db.Entry(cR_Mas_Sup_Brand).State = EntityState.Modified;
                db.SaveChanges();
                TempData["TempModif"] = "تم الإيقاف بنجاح";
                return RedirectToAction("Index");
            }
            if (hold == "تنشيط" || hold == "Activate")
            {
                cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status = "A";
                db.Entry(cR_Mas_Sup_Brand).State = EntityState.Modified;
                db.SaveChanges();
                TempData["TempModif"] = "تم التنشيط بنجاح";
                return RedirectToAction("Index");
            }
            if (cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status == "A" ||
            cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status == "Activated" ||
            cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status == "1" ||
            cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }
            if ((cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status == "D" ||
                 cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status == "Deleted" ||
                 cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
            }
            if (cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status == "H" ||
                cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status == "Hold" ||
                cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
            }
            if (cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }
            ViewBag.delete = cR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Status;
            return View(cR_Mas_Sup_Brand);
        }

        // GET: Brand/Delete/5
        //////public ActionResult Delete(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Sup_Brand cR_Mas_Sup_Brand = db.CR_Mas_Sup_Brand.Find(id);
        //////    if (cR_Mas_Sup_Brand == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Sup_Brand);
        //////}

        //////// POST: Brand/Delete/5
        //////[HttpPost, ActionName("Delete")]
        //////[ValidateAntiForgeryToken]
        //////public ActionResult DeleteConfirmed(string id)
        //////{
        //////    CR_Mas_Sup_Brand cR_Mas_Sup_Brand = db.CR_Mas_Sup_Brand.Find(id);
        //////    db.CR_Mas_Sup_Brand.Remove(cR_Mas_Sup_Brand);
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