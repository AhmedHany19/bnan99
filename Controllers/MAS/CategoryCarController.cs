using RentCar.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace RentCar.Controllers
{
    public class CategoryCarController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        public static string TaskCode;

        // GET: Category_Car
        public ActionResult Index()
        {
            Session["POS"] = "1508";
            TaskCode = "1508";
            if (Session["ST_1508_unhold"].ToString() != "true" || Session["ST_1508_hold"].ToString() != "true" && Session["ST_1508_undelete"].ToString() != "true" || Session["ST_1508_delete"].ToString() != "true")
            {
                var cR_Mas_Sup_Category_Car = db.CR_Mas_Sup_Category_Car.Include(c => c.CR_Mas_Sup_Category).Include(c => c.CR_Mas_Sup_Model).Where(stat => stat.CR_Mas_Sup_Category_Car_Status != "H" && stat.CR_Mas_Sup_Category_Car_Status != "D");
                return View(cR_Mas_Sup_Category_Car.ToList());
            }
            else
                if (Session["ST_1508_unhold"].ToString() != "true" || Session["ST_1508_hold"].ToString() != "true")
            {
                var cR_Mas_Sup_Category_Car = db.CR_Mas_Sup_Category_Car.Include(c => c.CR_Mas_Sup_Category).Include(c => c.CR_Mas_Sup_Model).Where(stat => stat.CR_Mas_Sup_Category_Car_Status != "H");

                return View(cR_Mas_Sup_Category_Car.ToList());
            }
            else if (Session["ST_1508_undelete"].ToString() != "true" || Session["ST_1508_delete"].ToString() != "true")
            {
                var cR_Mas_Sup_Category_Car = db.CR_Mas_Sup_Category_Car.Include(c => c.CR_Mas_Sup_Category).Include(c => c.CR_Mas_Sup_Model).Where(stat => stat.CR_Mas_Sup_Category_Car_Status != "D");
                return View(cR_Mas_Sup_Category_Car.ToList());
            }
            else
            {
                var cR_Mas_Sup_Category_Car = db.CR_Mas_Sup_Category_Car.Include(c => c.CR_Mas_Sup_Category).Include(c => c.CR_Mas_Sup_Model);
                return View(cR_Mas_Sup_Category_Car.ToList());
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
                var ModelCat = new System.Data.DataTable("teste");

                //ModelCat.Columns.Add("الحمولة", typeof(string));
                //ModelCat.Columns.Add("عدد الأحصنة", typeof(string));
                //ModelCat.Columns.Add("عدد الإسطوانات", typeof(string));
                //ModelCat.Columns.Add("الوزن", typeof(string));
                ModelCat.Columns.Add("الركاب", typeof(string));
                ModelCat.Columns.Add("الشنط الصغيرة", typeof(string));
                ModelCat.Columns.Add("الشنط الكبيرة", typeof(string));
                ModelCat.Columns.Add("عدد الأبواب", typeof(string));
                ModelCat.Columns.Add("الفئة", typeof(string));
                ModelCat.Columns.Add("سنة الصنع", typeof(string));
                ModelCat.Columns.Add("الطراز", typeof(string));
                var Lrecord = db.CR_Mas_Sup_Category_Car.ToList();

                if (Lrecord != null)
                {
                    foreach (var i in Lrecord)
                    {

                        ModelCat.Rows.Add(
                            //i.CR_Mas_Sup_Category_Car_Payload,
                            //i.CR_Mas_Sup_Category_Car_Hourses,
                            //i.CR_Mas_Sup_Category_Car_Clinder,
                            //i.CR_Mas_Sup_Category_Car_Weight,
                            i.CR_Mas_Sup_Category_Car_Passengers_No,
                            i.CR_Mas_Sup_Category_Car_Small_Bags,
                            i.CR_Mas_Sup_Category_Car_Bag_Bags,
                            i.CR_Mas_Sup_Category_Car_Door_No,
                            i.CR_Mas_Sup_Category_Car_Code,
                            i.CR_Mas_Sup_Category_Car_Year,
                            i.CR_Mas_Sup_Category_Car_Code);
                    }
                }

                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = ModelCat;
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
            return View(db.CR_Mas_Sup_Model.ToList());
        }

        //////// GET: Category_Car/Details/5
        //////public async Task<ActionResult> Details(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Sup_Category_Car cR_Mas_Sup_Category_Car = await db.CR_Mas_Sup_Category_Car.FindAsync(id);
        //////    if (cR_Mas_Sup_Category_Car == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Sup_Category_Car);
        //////}

        // GET: Category_Car/Create
        public ActionResult Create()
        {

            //////////////// retrieve category car list switch status////////////
            if (Session["ST_1503_unhold"].ToString() != "true" || Session["ST_1503_hold"].ToString() != "true" && Session["ST_1503_undelete"].ToString() != "true" || Session["ST_1503_delete"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_Category_Car_Code = new SelectList(db.CR_Mas_Sup_Category.Where(x => x.CR_Mas_Sup_Category_Status != "D" && x.CR_Mas_Sup_Category_Status != "H"), "CR_Mas_Sup_Category_Code", "CR_Mas_Sup_Category_Ar_Name");
            }
            else
                if (Session["ST_1503_unhold"].ToString() != "true" || Session["ST_1503_hold"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_Category_Car_Code = new SelectList(db.CR_Mas_Sup_Category.Where(x => x.CR_Mas_Sup_Category_Status != "H"), "CR_Mas_Sup_Category_Code", "CR_Mas_Sup_Category_Ar_Name");
            }
            else if (Session["ST_1503_undelete"].ToString() != "true" || Session["ST_1503_delete"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_Category_Car_Code = new SelectList(db.CR_Mas_Sup_Category.Where(x => x.CR_Mas_Sup_Category_Status != "D"), "CR_Mas_Sup_Category_Code", "CR_Mas_Sup_Category_Ar_Name");
            }
            else
            {
                ViewBag.CR_Mas_Sup_Category_Car_Code = new SelectList(db.CR_Mas_Sup_Category, "CR_Mas_Sup_Category_Code", "CR_Mas_Sup_Category_Ar_Name");
            }
            // ////////////////////////////////////////////////////////////////////

            //////////////// retrieve Model list switch status////////////
            if (Session["ST_1502_unhold"].ToString() != "true" || Session["ST_1502_hold"].ToString() != "true" && Session["ST_1502_undelete"].ToString() != "true" || Session["ST_1502_delete"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_Category_Model_Code = new SelectList(db.CR_Mas_Sup_Model.Where(x => x.CR_Mas_Sup_Model_Status != "D" && x.CR_Mas_Sup_Model_Status != "H"), "CR_Mas_Sup_Model_Code", "CR_Mas_Sup_Model_Ar_Name");
            }
            else
                if (Session["ST_1502_unhold"].ToString() != "true" || Session["ST_1502_hold"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_Category_Model_Code = new SelectList(db.CR_Mas_Sup_Model.Where(x => x.CR_Mas_Sup_Model_Status != "H"), "CR_Mas_Sup_Model_Code", "CR_Mas_Sup_Model_Ar_Name");
            }
            else if (Session["ST_1502_undelete"].ToString() != "true" || Session["ST_1502_delete"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_Category_Model_Code = new SelectList(db.CR_Mas_Sup_Model.Where(x => x.CR_Mas_Sup_Model_Status != "D"), "CR_Mas_Sup_Model_Code", "CR_Mas_Sup_Model_Ar_Name");
            }
            else
            {
                ViewBag.CR_Mas_Sup_Category_Model_Code = new SelectList(db.CR_Mas_Sup_Model, "CR_Mas_Sup_Model_Code", "CR_Mas_Sup_Model_Ar_Name");
            }
            ///////////////////////////////////////////////////////////////////////
            
            return View();
        }

        // POST: Category_Car/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Mas_Sup_Category_Model_Code,CR_Mas_Sup_Category_Car_Year," +
            "CR_Mas_Sup_Category_Car_Code,CR_Mas_Sup_Category_Car_Door_No,CR_Mas_Sup_Category_Car_Bag_Bags," +
            "CR_Mas_Sup_Category_Car_Small_Bags,CR_Mas_Sup_Category_Car_Passengers_No,CR_Mas_Sup_Category_Car_Weight, "+
            "CR_Mas_Sup_Category_Car_Clinder,CR_Mas_Sup_Category_Car_Hourses,CR_Mas_Sup_Category_Car_Payload, "+
            "CR_Mas_Sup_Category_Car_Picture,CR_Mas_Sup_Category_Car_Status,CR_Mas_Sup_Category_Car_Reasons")]
            CR_Mas_Sup_Category_Car cR_Mas_Sup_Category_Car, HttpPostedFileBase img,
            string CR_Mas_Sup_Category_Car_Code, int CR_Mas_Sup_Category_Car_Year, string name)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    //var LrecordExitCatCode = db.CR_Mas_Sup_Category_Car.Any(Lr => Lr.CR_Mas_Sup_Category_Car_Code == CR_Mas_Sup_Category_Car_Code);
                    //var LrecordExitYear = db.CR_Mas_Sup_Category_Car.Any(Lr => Lr.CR_Mas_Sup_Category_Car_Year == CR_Mas_Sup_Category_Car_Year);
                    var exist = db.CR_Mas_Sup_Category_Car.Any(lg => lg.CR_Mas_Sup_Category_Car_Code == CR_Mas_Sup_Category_Car_Code &&
                    lg.CR_Mas_Sup_Category_Car_Year == CR_Mas_Sup_Category_Car_Year && lg.CR_Mas_Sup_Category_Model_Code == cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Model_Code);


                    if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Year < 2000 ||
                        cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Year > DateTime.Now.Year + 1 ||
                        cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Code == null || exist
                        || cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Bag_Bags == null
                        //|| cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Clinder == null
                        || cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Door_No == null
                        //|| cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Hourses == null
                        || cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Passengers_No == null
                        || cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Passengers_No == null
                        //|| cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Payload == null
                        || cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Small_Bags == null
                        //|| cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Weight == null
                        )

                    {
                        if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Bag_Bags == null)
                            ViewBag.LRExistBigBags = "الرجاء إدخال بيانات الحقل";
                        //if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Clinder == null)
                        //    ViewBag.LRExistCylinder = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Door_No == null)
                            ViewBag.LRExistDoor = "الرجاء إدخال بيانات الحقل";
                        //if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Hourses == null)
                        //    ViewBag.LRExistHorses = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Passengers_No == null)
                            ViewBag.LRExistPassenger = "الرجاء إدخال بيانات الحقل";
                        //if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Payload == null)
                        //    ViewBag.LRExistLoad = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Small_Bags == null)
                            ViewBag.LRExistSmallBags = "الرجاء إدخال بيانات الحقل";
                        //if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Weight == null)
                        //    ViewBag.LRExistWeight = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Year < 2000 || cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Year > DateTime.Now.Year + 1)
                            ViewBag.LRExistyear = "الرجاء التأكد من البيانات";
                        if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Year.ToString() == null)
                            ViewBag.LRExistyear = "الرجاء إدخال بيانات الحقل";
                        if (exist)
                            ViewBag.LRExistAr = "عفوا هذ النوع موجود";
                        //if(cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Picture==null)
                        //    ViewBag.LRExistpic = "الرجاء إدخال بيانات الحقل";
                    }
                    else
                    {
                        string filepath = "";
                        if (img != null)
                        {
                            string folderModels = Server.MapPath(string.Format("~/{0}/", "/images/Bnan/Models"));
                            string folderimages = Server.MapPath(string.Format("~/{0}/", "/images"));
                            string folderBnan = Server.MapPath(string.Format("~/{0}/", "/images/Bnan"));
                            string folderCode = Server.MapPath(string.Format("~/{0}/", "/images/Bnan/Models/" + cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Model_Code));
                            string folderYear = Server.MapPath(string.Format("~/{0}/", "/images/Bnan/Models/" + cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Model_Code
                                + "/" + cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Year));
                            string folder = Server.MapPath(string.Format("~/{0}/", "/images/Bnan/Models/" + cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Model_Code + "/" +
                                cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Year));

                            if (!Directory.Exists(folderimages))
                            {
                                Directory.CreateDirectory(folderimages);
                            }
                            if (!Directory.Exists(folderBnan))
                            {
                                Directory.CreateDirectory(folderBnan);
                            }
                            if (!Directory.Exists(folderModels))
                            {
                                Directory.CreateDirectory(folderModels);
                            }
                            if (!Directory.Exists(folderCode))
                            {
                                Directory.CreateDirectory(folderCode);
                            }
                            if (!Directory.Exists(folderYear))
                            {
                                Directory.CreateDirectory(folderYear);
                            }
                            if (Directory.Exists(folder))
                            {
                                if (img.FileName.Length > 0)
                                {
                                    filepath = "~/images/Bnan/Models" + "/" + cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Model_Code +
                                   "/" + cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Year + "/" + Path.GetFileName(img.FileName);
                                    img.SaveAs(HttpContext.Server.MapPath(filepath));
                                    cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Picture = filepath;
                                    
                                    
                                }
                            }
                        }else
                        {
                            cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Picture = "~/images/common/NotFoundCar.png";
                        }
                        
                        
                        cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status = "A";
                        db.CR_Mas_Sup_Category_Car.Add(cR_Mas_Sup_Category_Car);
                        db.SaveChanges();
                        TempData["TempModel"] = "تم الحفظ بنجاح";
                        return RedirectToAction("Create", "CategoryCar");
                    }


                }
            }
            catch (Exception) { }

            //////////////// retrieve category car list switch status////////////
            if (Session["ST_1503_unhold"].ToString() != "true" || Session["ST_1503_hold"].ToString() != "true" && Session["ST_1503_undelete"].ToString() != "true" || Session["ST_1503_delete"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_Category_Car_Code = new SelectList(db.CR_Mas_Sup_Category.Where(x => x.CR_Mas_Sup_Category_Status != "D" && x.CR_Mas_Sup_Category_Status != "H"), "CR_Mas_Sup_Category_Code", "CR_Mas_Sup_Category_Ar_Name");
            }
            else
                if (Session["ST_1503_unhold"].ToString() != "true" || Session["ST_1503_hold"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_Category_Car_Code = new SelectList(db.CR_Mas_Sup_Category.Where(x => x.CR_Mas_Sup_Category_Status != "H"), "CR_Mas_Sup_Category_Code", "CR_Mas_Sup_Category_Ar_Name");
            }
            else if (Session["ST_1503_undelete"].ToString() != "true" || Session["ST_1503_delete"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_Category_Car_Code = new SelectList(db.CR_Mas_Sup_Category.Where(x => x.CR_Mas_Sup_Category_Status != "D"), "CR_Mas_Sup_Category_Code", "CR_Mas_Sup_Category_Ar_Name");
            }
            else
            {
                ViewBag.CR_Mas_Sup_Category_Car_Code = new SelectList(db.CR_Mas_Sup_Category, "CR_Mas_Sup_Category_Code", "CR_Mas_Sup_Category_Ar_Name");
            }
            // ////////////////////////////////////////////////////////////////////

            //////////////// retrieve Model list switch status////////////
            if (Session["ST_1502_unhold"].ToString() != "true" || Session["ST_1502_hold"].ToString() != "true" && Session["ST_1502_undelete"].ToString() != "true" || Session["ST_1502_delete"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_Category_Model_Code = new SelectList(db.CR_Mas_Sup_Model.Where(x => x.CR_Mas_Sup_Model_Status != "D" && x.CR_Mas_Sup_Model_Status != "H"), "CR_Mas_Sup_Model_Code", "CR_Mas_Sup_Model_Ar_Name");
            }
            else
                if (Session["ST_1502_unhold"].ToString() != "true" || Session["ST_1502_hold"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_Category_Model_Code = new SelectList(db.CR_Mas_Sup_Model.Where(x => x.CR_Mas_Sup_Model_Status != "H"), "CR_Mas_Sup_Model_Code", "CR_Mas_Sup_Model_Ar_Name");
            }
            else if (Session["ST_1502_undelete"].ToString() != "true" || Session["ST_1502_delete"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_Category_Model_Code = new SelectList(db.CR_Mas_Sup_Model.Where(x => x.CR_Mas_Sup_Model_Status != "D"), "CR_Mas_Sup_Model_Code", "CR_Mas_Sup_Model_Ar_Name");
            }
            else
            {
                ViewBag.CR_Mas_Sup_Category_Model_Code = new SelectList(db.CR_Mas_Sup_Model, "CR_Mas_Sup_Model_Code", "CR_Mas_Sup_Model_Ar_Name");
            }
            ///////////////////////////////////////////////////////////////////////
            return View(cR_Mas_Sup_Category_Car);
        }

        // GET: Category_Car/Edit/5
        public ActionResult Edit(string id1, string id2)
        {
            if (id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //////CR_Mas_Sup_Category_Car cR_Mas_Sup_Category_Car = await db.CR_Mas_Sup_Category_Car.FindAsync(id);
            CR_Mas_Sup_Category_Car cR_Mas_Sup_Category_Car = db.CR_Mas_Sup_Category_Car.FirstOrDefault(Mcat => Mcat.CR_Mas_Sup_Category_Model_Code == id1 && Mcat.CR_Mas_Sup_Category_Car_Year.ToString() == id2);
            if (cR_Mas_Sup_Category_Car == null)
            {
                return HttpNotFound();
            }

            if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status == "A" || cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status == "1")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }
            if ((cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status == "D" || cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
                ViewData["ReadOnly"] = "true";
            }
            if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status == "H" || cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
                ViewData["ReadOnly"] = "true";
            }
            if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }
            ViewBag.delete = cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status;

            //ViewBag.path=cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Picture.ToString();
            ViewBag.CR_Mas_Sup_Category_Car_Code = new SelectList(db.CR_Mas_Sup_Category, "CR_Mas_Sup_Category_Code", "CR_Mas_Sup_Category_Ar_Name",cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Code);
            ViewBag.CR_Mas_Sup_Category_Model_Code = new SelectList(db.CR_Mas_Sup_Model, "CR_Mas_Sup_Model_Code", "CR_Mas_Sup_Model_Ar_Name",cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Model_Code);
            return View(cR_Mas_Sup_Category_Car);
        }

        // POST: Category_Car/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Mas_Sup_Category_Model_Code," +
            "CR_Mas_Sup_Category_Car_Year,CR_Mas_Sup_Category_Car_Code," +
            "CR_Mas_Sup_Category_Car_Door_No,CR_Mas_Sup_Category_Car_Bag_Bags," +
            "CR_Mas_Sup_Category_Car_Small_Bags,CR_Mas_Sup_Category_Car_Passengers_No," +
            "CR_Mas_Sup_Category_Car_Weight,CR_Mas_Sup_Category_Car_Clinder," +
            "CR_Mas_Sup_Category_Car_Hourses,CR_Mas_Sup_Category_Car_Payload," +
            "CR_Mas_Sup_Category_Car_Picture,CR_Mas_Sup_Category_Car_Status," +
            "CR_Mas_Sup_Category_Car_Reasons")] CR_Mas_Sup_Category_Car cR_Mas_Sup_Category_Car, HttpPostedFileBase img, string save, string delete, string hold,string Category_Car_Code)
        {

            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Year > 2000 &&
                        Category_Car_Code != null &&
                        cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Bag_Bags != null &&
                        //cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Clinder != null &&
                        cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Door_No != null &&
                        //cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Hourses != null &&
                        cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Passengers_No != null &&
                        //cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Payload != null &&
                        cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Small_Bags != null)
                    //cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Weight != null

                    {
                        if (img != null)
                        {

                            string folderModels = Server.MapPath(string.Format("~/{0}/", "/images/Bnan/Models"));
                            string folderimages = Server.MapPath(string.Format("~/{0}/", "/images"));
                            string folderBnan = Server.MapPath(string.Format("~/{0}/", "/images/Bnan"));
                            string folderCode = Server.MapPath(string.Format("~/{0}/", "/images/Bnan/Models/" + cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Model_Code));
                            string folderYear = Server.MapPath(string.Format("~/{0}/", "/images/Bnan/Models/" + cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Model_Code
                                + "/" + cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Year));
                            string folder = Server.MapPath(string.Format("~/{0}/", "/images/Bnan/Models/" + cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Model_Code + "/" +
                                cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Year));
                            string filepath = "";
                            if (!Directory.Exists(folderimages))
                            {
                                Directory.CreateDirectory(folderimages);
                            }
                            if (!Directory.Exists(folderModels))
                            {
                                Directory.CreateDirectory(folderModels);
                            }
                            if (!Directory.Exists(folderCode))
                            {
                                Directory.CreateDirectory(folderCode);
                            }
                            if (!Directory.Exists(folderYear))
                            {
                                Directory.CreateDirectory(folderYear);
                            }
                            if (Directory.Exists(folder))
                            {
                                if (img.FileName.Length > 0)
                                {
                                    filepath = "~/images/Bnan/Models" + "/" + cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Model_Code +
                                   "/" + cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Year + "/" + Path.GetFileName(img.FileName);
                                    img.SaveAs(HttpContext.Server.MapPath(filepath));
                                    cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Picture = filepath;
                                }
                            }
                        }
                        cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Code = Category_Car_Code;
                        cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status = "A";
                        db.Entry(cR_Mas_Sup_Category_Car).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Bag_Bags == null)
                            ViewBag.LRExistBigBags = "الرجاء إدخال بيانات الحقل";
                        //if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Clinder == null)
                        //    ViewBag.LRExistCylinder = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Door_No == null)
                            ViewBag.LRExistDoor = "الرجاء إدخال بيانات الحقل";
                        //if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Hourses == null)
                        //    ViewBag.LRExistHorses = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Passengers_No == null)
                            ViewBag.LRExistPassenger = "الرجاء إدخال بيانات الحقل";
                        //if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Payload == null)
                        //    ViewBag.LRExistLoad = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Small_Bags == null)
                            ViewBag.LRExistSmallBags = "الرجاء إدخال بيانات الحقل";
                        //if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Weight == null)
                        //    ViewBag.LRExistWeight = "الرجاء إدخال بيانات الحقل";

                    }
                }
            }
            if (delete == "Delete" || delete == "حذف")
            {
                cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Code = Category_Car_Code;
                cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status = "D";
                db.Entry(cR_Mas_Sup_Category_Car).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            if (delete == "Activate" || delete == "إسترجاع")
            {
                cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Code = Category_Car_Code;
                cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status = "A";
                db.Entry(cR_Mas_Sup_Category_Car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "إيقاف" || hold == "hold")
            {
                cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Code = Category_Car_Code;
                cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status = "H";
                db.Entry(cR_Mas_Sup_Category_Car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "تنشيط" || hold == "Activate")
            {
                cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Code = Category_Car_Code;
                cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status = "A";
                db.Entry(cR_Mas_Sup_Category_Car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status == "A" ||
            cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status == "Activated" ||
            cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status == "1" ||
            cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }
            if ((cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status == "D" ||
                 cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status == "Deleted" ||
                 cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
            }
            if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status == "H" ||
                cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status == "Hold" ||
                cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
            }
            if (cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }
            //////////////// retrieve category car list switch status////////////
            if (Session["ST_1503_unhold"].ToString() != "true" || Session["ST_1503_hold"].ToString() != "true" && Session["ST_1503_undelete"].ToString() != "true" || Session["ST_1503_delete"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_Category_Car_Code = new SelectList(db.CR_Mas_Sup_Category.Where(x => x.CR_Mas_Sup_Category_Status != "D" && x.CR_Mas_Sup_Category_Status != "H"), "CR_Mas_Sup_Category_Code", "CR_Mas_Sup_Category_Ar_Name");
            }
            else
                if (Session["ST_1503_unhold"].ToString() != "true" || Session["ST_1503_hold"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_Category_Car_Code = new SelectList(db.CR_Mas_Sup_Category.Where(x => x.CR_Mas_Sup_Category_Status != "H"), "CR_Mas_Sup_Category_Code", "CR_Mas_Sup_Category_Ar_Name");
            }
            else if (Session["ST_1503_undelete"].ToString() != "true" || Session["ST_1503_delete"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_Category_Car_Code = new SelectList(db.CR_Mas_Sup_Category.Where(x => x.CR_Mas_Sup_Category_Status != "D"), "CR_Mas_Sup_Category_Code", "CR_Mas_Sup_Category_Ar_Name");
            }
            else
            {
                ViewBag.CR_Mas_Sup_Category_Car_Code = new SelectList(db.CR_Mas_Sup_Category, "CR_Mas_Sup_Category_Code", "CR_Mas_Sup_Category_Ar_Name");
            }
            // ////////////////////////////////////////////////////////////////////

            //////////////// retrieve Model list switch status////////////
            if (Session["ST_1502_unhold"].ToString() != "true" || Session["ST_1502_hold"].ToString() != "true" && Session["ST_1502_undelete"].ToString() != "true" || Session["ST_1502_delete"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_Category_Model_Code = new SelectList(db.CR_Mas_Sup_Model.Where(x => x.CR_Mas_Sup_Model_Status != "D" && x.CR_Mas_Sup_Model_Status != "H"), "CR_Mas_Sup_Model_Code", "CR_Mas_Sup_Model_Ar_Name");
            }
            else
                if (Session["ST_1502_unhold"].ToString() != "true" || Session["ST_1502_hold"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_Category_Model_Code = new SelectList(db.CR_Mas_Sup_Model.Where(x => x.CR_Mas_Sup_Model_Status != "H"), "CR_Mas_Sup_Model_Code", "CR_Mas_Sup_Model_Ar_Name");
            }
            else if (Session["ST_1502_undelete"].ToString() != "true" || Session["ST_1502_delete"].ToString() != "true")
            {
                ViewBag.CR_Mas_Sup_Category_Model_Code = new SelectList(db.CR_Mas_Sup_Model.Where(x => x.CR_Mas_Sup_Model_Status != "D"), "CR_Mas_Sup_Model_Code", "CR_Mas_Sup_Model_Ar_Name");
            }
            else
            {
                ViewBag.CR_Mas_Sup_Category_Model_Code = new SelectList(db.CR_Mas_Sup_Model, "CR_Mas_Sup_Model_Code", "CR_Mas_Sup_Model_Ar_Name");
            }
            ///////////////////////////////////////////////////////////////////////
            ViewBag.delete = cR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Status;
            return View(cR_Mas_Sup_Category_Car);
        }

        //////// GET: Category_Car/Delete/5
        //////public async Task<ActionResult> Delete(string id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    CR_Mas_Sup_Category_Car cR_Mas_Sup_Category_Car = await db.CR_Mas_Sup_Category_Car.FindAsync(id);
        //////    if (cR_Mas_Sup_Category_Car == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(cR_Mas_Sup_Category_Car);
        //////}

        //////// POST: Category_Car/Delete/5
        //////[HttpPost, ActionName("Delete")]
        //////[ValidateAntiForgeryToken]
        //////public async Task<ActionResult> DeleteConfirmed(string id)
        //////{
        //////    CR_Mas_Sup_Category_Car cR_Mas_Sup_Category_Car = await db.CR_Mas_Sup_Category_Car.FindAsync(id);
        //////    db.CR_Mas_Sup_Category_Car.Remove(cR_Mas_Sup_Category_Car);
        //////    await db.SaveChangesAsync();
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
