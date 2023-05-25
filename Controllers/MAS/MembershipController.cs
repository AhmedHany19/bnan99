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
    public class MembershipController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        public static string TaskCode;

        // GET: Membership
        public ActionResult Index()
        {
            TaskCode = "1604";
            Session["POS"] = "1604";
            if (Session["ST_1604_unhold"].ToString() != "true" || Session["ST_1604_hold"].ToString() != "true" && Session["ST_1604_undelete"].ToString() != "true" || Session["ST_1604_delete"].ToString() != "true")
            {
                var cR_Mas_Sup_Membership = db.CR_Mas_Sup_Membership.Include(c => c.CR_Mas_Sup_Group).Where(x => x.CR_Mas_Sup_Membership_Status != "H" && x.CR_Mas_Sup_Membership_Status != "D");
                return View(cR_Mas_Sup_Membership.ToList());
            }
            else
                if (Session["ST_1604_unhold"].ToString() != "true" || Session["ST_1604_hold"].ToString() != "true")
            {
                var cR_Mas_Sup_Membership = db.CR_Mas_Sup_Membership.Include(c => c.CR_Mas_Sup_Group).Where(x => x.CR_Mas_Sup_Membership_Status != "H");
                return View(cR_Mas_Sup_Membership.ToList());
            }
            else if (Session["ST_1502_undelete"].ToString() != "true" || Session["ST_1502_delete"].ToString() != "true")
            {
                var cR_Mas_Sup_Membership = db.CR_Mas_Sup_Membership.Include(c => c.CR_Mas_Sup_Group).Where(x => x.CR_Mas_Sup_Membership_Status != "D");
                return View(cR_Mas_Sup_Membership.ToList());
            }
            else
            {
                var cR_Mas_Sup_Membership = db.CR_Mas_Sup_Membership.Include(c => c.CR_Mas_Sup_Group);
                return View(cR_Mas_Sup_Membership.ToList());
            }
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult Index_Post(string excelCall, string lang)
        {
            if (!string.IsNullOrEmpty(lang))
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
            }
            if (!string.IsNullOrEmpty(excelCall))
            {
                var colorTable = new System.Data.DataTable("teste");

                colorTable.Columns.Add("الملاحظات", typeof(string));
                colorTable.Columns.Add("الحالة", typeof(string));
                colorTable.Columns.Add("المجموعة", typeof(string));
                colorTable.Columns.Add("الإسم", typeof(string));
                colorTable.Columns.Add("الرمز", typeof(string));
                var Lrecord = db.CR_Mas_Sup_Membership.ToList();

                if (Lrecord != null)
                {
                    foreach (var i in Lrecord)
                    {
                        colorTable.Rows.Add(i.CR_Mas_Sup_Membership_Reasons, i.CR_Mas_Sup_Membership_Status,
                        i.CR_Mas_Sup_Membership_Group_Code, i.CR_Mas_Sup_Membership_Ar_Name, i.CR_Mas_Sup_Membership_Code);
                    }
                }
                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = colorTable;
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
            return View(db.CR_Mas_Sup_Membership.ToList());
        }

        //////// GET: Membership/Details/5
        //////public ActionResult Details(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Sup_Membership cR_Mas_Sup_Membership = db.CR_Mas_Sup_Membership.Find(id);
        //////    if (cR_Mas_Sup_Membership == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Sup_Membership);
        //////}
        public CR_Mas_Sup_Membership GetLastRecord()
        {
            var Lrecord = db.CR_Mas_Sup_Membership.Max(Lr => Lr.CR_Mas_Sup_Membership_Code);
            CR_Mas_Sup_Membership c = new CR_Mas_Sup_Membership();
            if (Lrecord != null)
            {
                Int64 val = Int64.Parse(Lrecord) + 1;
                c.CR_Mas_Sup_Membership_Code = val.ToString();
            }
            else
            {
                c.CR_Mas_Sup_Membership_Code = "3400000001";
            }
            return c;
        }
        // GET: Membership/Create
        public ActionResult Create()
        {
            ViewBag.CR_Mas_Sup_Membership_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code", "CR_Mas_Sup_Group_Ar_Name");
            CR_Mas_Sup_Membership cat = new CR_Mas_Sup_Membership();
            cat = GetLastRecord();
            cat.CR_Mas_Sup_Membership_Status = "A";
            return View(cat);
        }

        // POST: Membership/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Mas_Sup_Membership_Code, CR_Mas_Sup_Membership_Group_Code, CR_Mas_Sup_Membership_Ar_Name, " +
        "CR_Mas_Sup_Membership_En_Name, CR_Mas_Sup_Membership_Fr_Name, sCR_Mas_Sup_Membership_Status, CR_Mas_Sup_Membership_Reasons")]
        CR_Mas_Sup_Membership cR_Mas_Sup_Membership, string CR_Mas_Sup_Membership_Ar_Name, string CR_Mas_Sup_Membership_Fr_Name,
        string CR_Mas_Sup_Membership_En_Name)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_Sup_Membership.Any(Lr => Lr.CR_Mas_Sup_Membership_Ar_Name == CR_Mas_Sup_Membership_Ar_Name);
                    var LrecordExitEnglish = db.CR_Mas_Sup_Membership.Any(Lr => Lr.CR_Mas_Sup_Membership_En_Name == CR_Mas_Sup_Membership_En_Name);
                    var LrecordExitFrench = db.CR_Mas_Sup_Membership.Any(Lr => Lr.CR_Mas_Sup_Membership_Fr_Name == CR_Mas_Sup_Membership_Fr_Name);


                    if (cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Ar_Name != null && cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_En_Name != null &&
                        cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench &&
                        cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Ar_Name.Length >= 3 && cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_En_Name.Length >= 3 &&
                        cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Fr_Name.Length >= 3)
                    {
                        cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Code = GetLastRecord().CR_Mas_Sup_Membership_Code;
                        cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Group_Code = "16";
                        cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status = "A";
                        db.CR_Mas_Sup_Membership.Add(cR_Mas_Sup_Membership);
                        db.SaveChanges();
                        cR_Mas_Sup_Membership = new CR_Mas_Sup_Membership();
                        cR_Mas_Sup_Membership = GetLastRecord();
                        cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status = "A";
                        TempData["TempModel"] = "تم الحفظ بنجاح";
                        return RedirectToAction("Create", "Membership");
                    }
                    else
                    {
                        if (cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe)
                            ViewBag.LRExistAr = "عفوا هذه العضوية موجودة";
                        if (LrecordExitEnglish)
                            ViewBag.LRExistEn = "عفوا هذه العضوية موجودة";
                        if (LrecordExitFrench)
                            ViewBag.LRExistFr = "عفوا هذه العضوية موجودة";
                        if (cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Ar_Name != null && cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Ar_Name.Length < 3)
                            ViewBag.LRExistAr = "عفوا الاسم يحتوي على ما بين 3 و 20 حرفًا";
                        if (cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_En_Name != null && cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_En_Name.Length < 3)
                            ViewBag.LRExistEn = "عفوا الاسم يحتوي على ما بين 3 و 20 حرفًا";
                        if (cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Fr_Name != null && cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Fr_Name.Length < 3)
                            ViewBag.LRExistFr = "عفوا الاسم يحتوي على ما بين 3 و 20 حرفًا";
                    }
                }
            }
            catch (Exception) { }
            ViewBag.CR_Mas_Sup_Membership_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code",
                                                        "CR_Mas_Sup_Group_Ar_Name", cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Group_Code);
            return View(cR_Mas_Sup_Membership);
        }

        // GET: Membership/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Sup_Membership cR_Mas_Sup_Membership = db.CR_Mas_Sup_Membership.Find(id);
            if (cR_Mas_Sup_Membership == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status == "A" ||
                    cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status == "1")
                {
                    ViewBag.stat = "حذف";
                    ViewBag.h = "إيقاف";
                }
                if ((cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status == "D" ||
                    cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status == "0"))
                {
                    ViewBag.stat = "إسترجاع";
                    ViewBag.h = "إيقاف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status == "H" ||
                    cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status == "2")
                {
                    ViewBag.h = "تنشيط";
                    ViewBag.stat = "حذف";
                    ViewData["ReadOnly"] = "true";
                }
                if (cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status == null)
                {
                    ViewBag.h = "إيقاف";
                    ViewBag.stat = "حذف";
                }
                ViewBag.delete = cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status;
            }

            ViewBag.CR_Mas_Sup_Membership_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code",
            "CR_Mas_Sup_Group_Ar_Name", cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Group_Code);
            return View(cR_Mas_Sup_Membership);
        }

        // POST: Membership/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Mas_Sup_Membership_Code, CR_Mas_Sup_Membership_Group_Code, " +
        "CR_Mas_Sup_Membership_Ar_Name, CR_Mas_Sup_Membership_En_Name, CR_Mas_Sup_Membership_Fr_Name, CR_Mas_Sup_Membership_Status, " +
        "CR_Mas_Sup_Membership_Reasons")]CR_Mas_Sup_Membership cR_Mas_Sup_Membership, string save, string delete, string hold)
        {
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabe = db.CR_Mas_Sup_Membership.Any(m => m.CR_Mas_Sup_Membership_Code != cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Code &&
                                                                        m.CR_Mas_Sup_Membership_Ar_Name == cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Ar_Name);
                    var LrecordExitEnglish = db.CR_Mas_Sup_Membership.Any(m => m.CR_Mas_Sup_Membership_Code != cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Code &&
                                                                          m.CR_Mas_Sup_Membership_En_Name == cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_En_Name);
                    var LrecordExitFrench = db.CR_Mas_Sup_Membership.Any(m => m.CR_Mas_Sup_Membership_Code != cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Code &&
                                                                         m.CR_Mas_Sup_Membership_Fr_Name == cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Fr_Name);

                    if (cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Ar_Name != null && cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_En_Name != null &&
                        cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Fr_Name != null && !LrecordExitArabe && !LrecordExitEnglish && !LrecordExitFrench &&
                        cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Ar_Name.Length >= 3 && cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_En_Name.Length >= 3 &&
                        cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Fr_Name.Length >= 3)
                    {
                        db.Entry(cR_Mas_Sup_Membership).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Ar_Name == null)
                            ViewBag.LRExistAr = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_En_Name == null)
                            ViewBag.LRExistEn = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Fr_Name == null)
                            ViewBag.LRExistFr = "الرجاء إدخال بيانات الحقل";
                        if (LrecordExitArabe)
                            ViewBag.LRExistAr = "عفوا هذه العضوية موجودة";
                        if (LrecordExitEnglish)
                            ViewBag.LRExistEn = "عفوا هذه العضوية موجودة";
                        if (LrecordExitFrench)
                            ViewBag.LRExistFr = "عفوا هذه العضوية موجودة";
                        if (cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Ar_Name != null && cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Ar_Name.Length < 3)
                            ViewBag.LRExistAr = "عفوا الاسم يحتوي على ما بين 3 و 20 حرفًا";
                        if (cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_En_Name != null && cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_En_Name.Length < 3)
                            ViewBag.LRExistEn = "عفوا الاسم يحتوي على ما بين 3 و 20 حرفًا";
                        if (cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Fr_Name != null && cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Fr_Name.Length < 3)
                            ViewBag.LRExistFr = "عفوا الاسم يحتوي على ما بين 3 و 20 حرفًا";
                    }
                }
            }
            if (delete == "Delete" || delete == "حذف")
            {
                cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status = "D";
                db.Entry(cR_Mas_Sup_Membership).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (delete == "Activate" || delete == "إسترجاع")
            {
                cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status = "A";
                db.Entry(cR_Mas_Sup_Membership).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "إيقاف" || hold == "hold")
            {
                cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status = "H";
                db.Entry(cR_Mas_Sup_Membership).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "تنشيط" || hold == "Activate")
            {
                cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status = "A";
                db.Entry(cR_Mas_Sup_Membership).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status == "A" ||
            cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status == "Activated" ||
            cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status == "1" ||
            cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }
            if ((cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status == "D" ||
                 cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status == "Deleted" ||
                 cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
            }
            if (cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status == "H" ||
                cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status == "Hold" ||
                cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
            }
            if (cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }
            ViewBag.delete = cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Status;
            ViewBag.CR_Mas_Sup_Membership_Group_Code = new SelectList(db.CR_Mas_Sup_Group, "CR_Mas_Sup_Group_Code", "CR_Mas_Sup_Group_Ar_Name",
                                                                        cR_Mas_Sup_Membership.CR_Mas_Sup_Membership_Group_Code);
            return View(cR_Mas_Sup_Membership);
        }

        //////// GET: Membership/Delete/5
        //////public ActionResult Delete(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Sup_Membership cR_Mas_Sup_Membership = db.CR_Mas_Sup_Membership.Find(id);
        //////    if (cR_Mas_Sup_Membership == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Sup_Membership);
        //////}

        //////// POST: Model/Delete/5
        //////[HttpPost, ActionName("Delete")]
        //////[ValidateAntiForgeryToken]
        //////public ActionResult DeleteConfirmed(string id)
        //////{
        //////    CR_Mas_Sup_Membership cR_Mas_Sup_Membership = db.CR_Mas_Sup_Membership.Find(id);
        //////    db.CR_Mas_Sup_Membership.Remove(cR_Mas_Sup_Membership);
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