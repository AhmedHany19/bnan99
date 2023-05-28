using RentCar.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace RentCar.Controllers.CAS
{
    public class CarBasicPriceController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: CarBasicPrice
        public ActionResult Index()
        {
            var LessorCode = "";
            var UserLogin = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }

            return View();

        }

        public PartialViewResult PartialIndex(string type, string StartDate, string EndDate)
        {
            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                BranchCode = Session["BranchCode"].ToString();

                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null || BranchCode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }

            catch
            {
                RedirectToAction("Login", "Account");
            }

            IQueryable<CR_Cas_Car_Price_Basic> cR_Cas_Car_Price_Basic = null;
            if (type == "All")
            {
                cR_Cas_Car_Price_Basic = db.CR_Cas_Car_Price_Basic.Where(b => b.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode
                && b.CR_Cas_Car_Price_Basic_Sector == "1")
                    .Include(c => c.CR_Mas_Com_Lessor)
                    .Include(c => c.CR_Mas_Sup_Brand)
                    .Include(c => c.CR_Mas_Sup_Model)
                    .Include(c => c.CR_Mas_Sup_Sector);
            }

            else if (type == "D")
            {
                cR_Cas_Car_Price_Basic = db.CR_Cas_Car_Price_Basic.Where(b => b.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode && b.CR_Cas_Car_Price_Basic_Status == "D"
                && b.CR_Cas_Car_Price_Basic_Sector == "1")
                .Include(c => c.CR_Mas_Com_Lessor)
                .Include(c => c.CR_Mas_Sup_Brand)
                .Include(c => c.CR_Mas_Sup_Model)
                .Include(c => c.CR_Mas_Sup_Sector);
            }
            //else if(type=="N")
            //{
            //    cR_Cas_Car_Price_Basic = db.CR_Cas_Car_Price_Basic.Where(b => b.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode && b.CR_Cas_Car_Price_Basic_Status == "N")
            //        .Include(c => c.CR_Mas_Com_Lessor)
            //        .Include(c => c.CR_Mas_Sup_Brand)
            //        .Include(c => c.CR_Mas_Sup_Model)
            //        .Include(c => c.CR_Mas_Sup_Sector);
            //}
            else if (type == "X")
            {
                cR_Cas_Car_Price_Basic = db.CR_Cas_Car_Price_Basic.Where(b => b.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode && b.CR_Cas_Car_Price_Basic_Status == "X"
                && b.CR_Cas_Car_Price_Basic_Sector == "1")
                    .Include(c => c.CR_Mas_Com_Lessor)
                    .Include(c => c.CR_Mas_Sup_Brand)
                    .Include(c => c.CR_Mas_Sup_Model)
                    .Include(c => c.CR_Mas_Sup_Sector);
            }
            else if (type == "E")
            {
                cR_Cas_Car_Price_Basic = db.CR_Cas_Car_Price_Basic.Where(b => b.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode && b.CR_Cas_Car_Price_Basic_Status == "E"
                && b.CR_Cas_Car_Price_Basic_Sector == "1")
                    .Include(c => c.CR_Mas_Com_Lessor)
                    .Include(c => c.CR_Mas_Sup_Brand)
                    .Include(c => c.CR_Mas_Sup_Model)
                    .Include(c => c.CR_Mas_Sup_Sector);
            }
            else if (type == "Date" && StartDate != "" && EndDate != "")
            {
                DateTime sd = Convert.ToDateTime(StartDate);
                DateTime ed = Convert.ToDateTime(EndDate);
                cR_Cas_Car_Price_Basic = db.CR_Cas_Car_Price_Basic.Where(b => b.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode && b.CR_Cas_Car_Price_Basic_Status == "E"
                    && b.CR_Cas_Car_Price_Basic_Date >= sd && b.CR_Cas_Car_Price_Basic_Date <= ed && b.CR_Cas_Car_Price_Basic_Sector == "1")
                    .Include(c => c.CR_Mas_Com_Lessor)
                    .Include(c => c.CR_Mas_Sup_Brand)
                    .Include(c => c.CR_Mas_Sup_Model)
                    .Include(c => c.CR_Mas_Sup_Sector);
            }
            else
            {
                cR_Cas_Car_Price_Basic = db.CR_Cas_Car_Price_Basic.Where(b => b.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode && b.CR_Cas_Car_Price_Basic_Sector == "1" && b.CR_Cas_Car_Price_Basic_Status == "A")
                    .Include(c => c.CR_Mas_Com_Lessor)
                    .Include(c => c.CR_Mas_Sup_Brand)
                    .Include(c => c.CR_Mas_Sup_Model)
                    .Include(c => c.CR_Mas_Sup_Sector);
            }
            return PartialView(cR_Cas_Car_Price_Basic);
        }



        public JsonResult GetBrandCatCode(string ModelCode, int Year)
        {
            var category = "";
            var BrandCode = "";
            var LessorCode = "";
            var UserLogin = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }
            if (ModelCode != "" || Year > 0)
            {
                var cat = db.CR_Mas_Sup_Category_Car.FirstOrDefault(x => x.CR_Mas_Sup_Category_Model_Code == ModelCode && x.CR_Mas_Sup_Category_Car_Year == Year
                && x.CR_Mas_Sup_Category_Model_Code == ModelCode);
                if (cat != null)
                {
                    category = cat.CR_Mas_Sup_Category_Car_Code;
                }

                var m = db.CR_Mas_Sup_Model.FirstOrDefault(mod => mod.CR_Mas_Sup_Model_Code == ModelCode);
                if (m != null)
                {
                    BrandCode = m.CR_Mas_Sup_Model_Brand_Code;
                }

                var FeaturesNbr = db.CR_Cas_Sup_Features.Where(x => x.CR_Cas_Sup_Features_Lessor_Code == LessorCode && x.CR_Cas_Sup_Features_Model_Code == ModelCode &&
               x.CR_Cas_Sup_Features_Model_Year == Year.ToString()).Count();

                string ImagePath = "";
                var carcat = db.CR_Mas_Sup_Category_Car.FirstOrDefault(c => c.CR_Mas_Sup_Category_Model_Code == ModelCode && c.CR_Mas_Sup_Category_Car_Year == Year);
                if (carcat != null)
                {
                    if (carcat.CR_Mas_Sup_Category_Car_Picture != "")
                    {
                        ImagePath = carcat.CR_Mas_Sup_Category_Car_Picture.Remove(0, 1);
                    }

                }

                return Json(BrandCode + "," + category + "," + FeaturesNbr + "," + ImagePath, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBrandCat(string ModelCode, int Year)
        {
            var LessorCode = "";
            var UserLogin = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }
            String BrandCategory = "";
            string category = "";
            String CatName = "";
            String BrandCode = "";
            db.Configuration.ProxyCreationEnabled = false;
            if (ModelCode != "" || Year > 0)
            {
                var cat = db.CR_Mas_Sup_Category_Car.FirstOrDefault(x => x.CR_Mas_Sup_Category_Model_Code == ModelCode && x.CR_Mas_Sup_Category_Car_Year == Year);
                if (cat != null)
                {
                    category = cat.CR_Mas_Sup_Category_Car_Code;
                }


                var q = db.CR_Mas_Sup_Category.FirstOrDefault(c => c.CR_Mas_Sup_Category_Code == category);
                if (q != null)
                {
                    CatName = q.CR_Mas_Sup_Category_Ar_Name;
                }

                var m = db.CR_Mas_Sup_Model.FirstOrDefault(mod => mod.CR_Mas_Sup_Model_Code == ModelCode);
                if (m != null)
                {
                    BrandCode = m.CR_Mas_Sup_Model_Brand_Code;
                }

                var nbr = db.CR_Cas_Sup_Car_Information.Where(x => x.CR_Cas_Sup_Car_Lessor_Code == LessorCode && x.CR_Cas_Sup_Car_Model_Code == ModelCode &&
               x.CR_Cas_Sup_Car_Year == Year).Count();


                var b = db.CR_Mas_Sup_Brand.FirstOrDefault(brand => brand.CR_Mas_Sup_Brand_Code == BrandCode);
                if (b != null)
                {
                    BrandCategory = b.CR_Mas_Sup_Brand_Ar_Name + " / " + CatName + " / " + nbr;
                }

                return Json(BrandCategory, JsonRequestBehavior.AllowGet);

            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CheckPriceExist(string ModelCode, int Year, string sector)
        {
            var LessorCode = "";
            var UserLogin = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }
            db.Configuration.ProxyCreationEnabled = false;

            if (ModelCode != "" && Year > 0 && sector != "")
            {

                bool price = false;
                if (sector != "0")
                {
                    price = db.CR_Cas_Car_Price_Basic.Any(p => p.CR_Cas_Car_Price_Basic_Model_Code == ModelCode && p.CR_Cas_Car_Price_Basic_Car_Year == Year &&
                p.CR_Cas_Car_Price_Basic_Sector == sector && p.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode && p.CR_Cas_Car_Price_Basic_Status == "A");

                }
                else if (sector == "0")
                {
                    price = db.CR_Cas_Car_Price_Basic.Any(p => p.CR_Cas_Car_Price_Basic_Model_Code == ModelCode && p.CR_Cas_Car_Price_Basic_Car_Year == Year &&
                 p.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode && p.CR_Cas_Car_Price_Basic_Status == "A" && p.CR_Cas_Car_Price_Basic_Sector == "1");
                }

                return Json(price, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }


        public PartialViewResult ChoicesList()
        {
            var choices = db.CR_Mas_Sup_Choices.Where(x => x.CR_Mas_Sup_Choices_Status == "A");
            return PartialView(choices.ToList());
        }

        public PartialViewResult AdditionalList()
        {

            var additional = db.CR_Mas_Sup_Additional.Where(x => x.CR_Mas_Sup_Additional_Status == "A");

            List<AdditionalPrice> List = new List<AdditionalPrice>();

            foreach (var Item in additional)
            {
                AdditionalPrice ad = new AdditionalPrice();
                ad.CR_Mas_Sup_Additional_Ar_Name = Item.CR_Mas_Sup_Additional_Ar_Name;
                ad.CR_Mas_Sup_Additional_Code = Item.CR_Mas_Sup_Additional_Code;
                ad.CR_Mas_Sup_Additional_En_Name = Item.CR_Mas_Sup_Additional_En_Name;
                ad.CR_Mas_Sup_Additional_Fr_Name = Item.CR_Mas_Sup_Additional_Fr_Name;
                ad.CR_Mas_Sup_Additional_Group_Code = Item.CR_Mas_Sup_Additional_Group_Code;
                ad.CR_Mas_Sup_Additional_Reasons = Item.CR_Mas_Sup_Additional_Reasons;
                ad.CR_Mas_Sup_Additional_Status = Item.CR_Mas_Sup_Additional_Status;
                List.Add(ad);
            }
            return PartialView(List.ToList());
        }


        public PartialViewResult FeaturesList(string modelcode, string year)
        {
            var LessorCode = "";
            var UserLogin = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }
            var CasFeatures = db.CR_Cas_Sup_Features.Where(x => x.CR_Cas_Sup_Features_Model_Code == modelcode && x.CR_Cas_Sup_Features_Model_Year == year &&
            x.CR_Cas_Sup_Features_Lessor_Code == LessorCode);

            var MasFeature = db.CR_Mas_Sup_Features.Where(f => f.CR_Mas_Sup_Features_Status == "A");


            List<MasCasFeaturesModel> l = new List<MasCasFeaturesModel>();


            foreach (var MasItem in MasFeature)
            {
                foreach (var CasItem in CasFeatures)
                {
                    MasCasFeaturesModel result = l.Find(x => x.CR_Cas_Sup_Features_Code == MasItem.CR_Mas_Sup_Features_Code);

                    if (result == null)
                    {
                        if (MasItem.CR_Mas_Sup_Features_Code == CasItem.CR_Cas_Sup_Features_Code)
                        {
                            MasCasFeaturesModel m = new MasCasFeaturesModel();
                            m.CR_Cas_Sup_Features_Code = MasItem.CR_Mas_Sup_Features_Code;
                            m.CR_Cas_Sup_Features_Lessor_Code = CasItem.CR_Cas_Sup_Features_Lessor_Code;
                            m.CR_Cas_Sup_Features_Model_Code = CasItem.CR_Cas_Sup_Features_Model_Code;
                            m.CR_Cas_Sup_Features_Model_Year = CasItem.CR_Cas_Sup_Features_Model_Year;
                            m.CR_Mas_Sup_Features_Name = MasItem.CR_Mas_Sup_Features_Ar_Name;
                            m.CR_Cas_Sup_Features_Serial_No = CasItem.CR_Cas_Sup_Features_Serial_No;
                            l.Add(m);
                        }
                    }

                }
            }
            int n = l.Count();


            return PartialView(l.ToList());
        }



        public ActionResult Create()
        {
            var LessorCode = "";
            var UserLogin = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }


            ViewBag.CR_Cas_Car_Price_Basic_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo");
            ViewBag.CR_Cas_Car_Price_Basic_Brand_Code = new SelectList(db.CR_Mas_Sup_Brand, "CR_Mas_Sup_Brand_Code", "CR_Mas_Sup_Brand_Ar_Name");
            ViewBag.CR_Cas_Car_Price_Basic_Category_Code = new SelectList(db.CR_Mas_Sup_Category, "CR_Mas_Sup_Category_Code", "CR_Mas_Sup_Category_Group_Code");
            ViewBag.CR_Cas_Car_Price_Basic_Model_Code = new SelectList(db.CR_Mas_Sup_Model, "CR_Mas_Sup_Model_Code", "CR_Mas_Sup_Model_Ar_Name");

            CR_Cas_Car_Price_Basic cR_Cas_Car_Price_Basic = new CR_Cas_Car_Price_Basic();
            cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_No_Free_Additional_Hours = 0;
            cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Additional_Driver_Value = 0;
            cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Hour_Max = 0;
            cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Extra_Hour_Value = 0;
            var Tax = new SelectList(new[] {
                                              new {ID="1",Name="لا"},
                                              new{ID="2",Name="نعم"},
                                              }, "ID", "Name", 1);
            ViewData["Tax"] = Tax;

            ViewBag.CR_Cas_Car_Price_Basic_Sector = new SelectList(db.CR_Mas_Sup_Sector.Where(x => x.CR_Mas_Sup_Sector_Status != "D"), "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name", "0");



            var ListGear = new SelectList(new[] {
                                              new {ID="1",Name="أوتوماتيكي"},
                                              new{ID="2",Name="عادي"},
                                              }, "ID", "Name", 1);
            ViewData["GearType"] = ListGear;


            var d = DateTime.Now.Year.ToString();
            var y = int.Parse(d);
            var ListYear = new SelectList(new[] {
                                              new{ID=(y-10).ToString(),Name=(y-10).ToString()},
                                              new{ID=(y-9).ToString(),Name=(y-9).ToString()},
                                              new{ID=(y-8).ToString(),Name=(y-8).ToString()},
                                              new{ID=(y-7).ToString(),Name=(y-7).ToString()},
                                              new{ID=(y-6).ToString(),Name=(y-6).ToString()},
                                              new{ID=(y-5).ToString(),Name=(y-5).ToString()},
                                              new{ID=(y-4).ToString(),Name=(y-4).ToString()},
                                              new{ID=(y-3).ToString(),Name=(y-3).ToString()},
                                              new{ID=(y-2).ToString(),Name=(y-2).ToString()},
                                              new{ID=(y-1).ToString(),Name=(y-1).ToString()},
                                              new{ID=(y).ToString(),Name=(y).ToString()},
                                              new{ID=(y+1).ToString(),Name=(y+1).ToString()},
                                              }, "ID", "Name");
            ViewData["ListYear"] = ListYear;

            ViewBag.StartDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            ViewBag.EndDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);

            return View(cR_Cas_Car_Price_Basic);
        }

        // GET: CarBasicPrice/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Car_Price_Basic cR_Cas_Car_Price_Basic = db.CR_Cas_Car_Price_Basic.Find(id);
            if (cR_Cas_Car_Price_Basic == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Car_Price_Basic);
        }

        // GET: CarBasicPrice/Create
        public CR_Cas_Administrative_Procedures GetLastRecord(string ProcedureCode, string sector)
        {

            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var LessorCode = Session["LessorCode"].ToString();
            var Lrecord = db.CR_Cas_Administrative_Procedures.Where(x => x.CR_Cas_Administrative_Procedures_Lessor == LessorCode &&
                x.CR_Cas_Administrative_Procedures_Code == ProcedureCode
                && x.CR_Cas_Administrative_Procedures_Sector == sector
                && x.CR_Cas_Administrative_Procedures_Year == y)
                .Max(x => x.CR_Cas_Administrative_Procedures_No.Substring(x.CR_Cas_Administrative_Procedures_No.Length - 7, 7));

            CR_Cas_Administrative_Procedures T = new CR_Cas_Administrative_Procedures();
            if (Lrecord != null)
            {
                Int64 val = Int64.Parse(Lrecord) + 1;
                T.CR_Cas_Administrative_Procedures_No = val.ToString("0000000");
            }
            else
            {
                T.CR_Cas_Administrative_Procedures_No = "0000001";
            }




            return T;
        }


        // POST: CarBasicPrice/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Car_Price_Basic_No,CR_Cas_Car_Price_Basic_Year," +
            "CR_Cas_Car_Price_Basic_Type,CR_Cas_Car_Price_Basic_Lessor_Code,CR_Cas_Car_Price_Basic_Sector," +
            "CR_Cas_Car_Price_Basic_Brand_Code,CR_Cas_Car_Price_Basic_Model_Code,CR_Cas_Car_Price_Basic_Car_Year," +
            "CR_Cas_Car_Price_Basic_Category_Code,CR_Cas_Car_Price_Basic_Date,CR_Cas_Car_Price_Basic_Start_Date," +
            "CR_Cas_Car_Price_Basic_End_Date,CR_Cas_Car_Price_Basic_Daily_Rent,CR_Cas_Car_Price_Basic_Weekly_Rent," +
            "CR_Cas_Car_Price_Basic_Monthly_Rent,CR_Cas_Car_Price_Basic_Rental_Tax_Rate,CR_Cas_Car_Price_Basic_No_Daily_Free_KM," +
            "CR_Cas_Car_Price_Basic_Additional_KM_Value,CR_Cas_Car_Price_Basic_No_Free_Additional_Hours,CR_Cas_Car_Price_Basic_Hour_Max," +
            "CR_Cas_Car_Price_Basic_Additional_Driver,CR_Cas_Car_Price_Basic_Additional_Driver_Value,CR_Cas_Car_Price_Basic_Internal_Fees_Tamm,CR_Cas_Car_Price_Basic_International_Fees_Tamm," +
            "CR_Cas_Car_Price_Basic_Extra_Hour_Value,CR_Cas_Car_Price_Basic_Min_Age_Renter,CR_Cas_Car_Price_Basic_Max_Age_Renter," +
            "CR_Cas_Car_Price_Basic_Does_Require_Financial_Credit,CR_Cas_Car_Price_Basic_Min_Percentage_Contract_Value," +
            "CR_Cas_Car_Price_Basic_Carrying_Accident,CR_Cas_Car_Price_Basic_Carrying_Fire,CR_Cas_Car_Price_Basic_Stealing," +
            "CR_Cas_Car_Price_Basic_Drowning,CR_Cas_Car_Price_Basic_Activation,CR_Cas_Car_Price_Basic_About_To_Expire,C" +
            "R_Cas_Car_Price_Basic_Status,CR_Cas_Car_Price_Basic_Reasons")] CR_Cas_Car_Price_Basic cR_Cas_Car_Price_Basic, FormCollection collection,
            string CR_Cas_Car_Price_Basic_Brand_Code, string Delete, string add, string listyear)
        {
            if (!string.IsNullOrEmpty(add))
            {
                if (ModelState.IsValid)
                {
                    using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var LessorCode = "";
                            var UserLogin = "";

                            LessorCode = Session["LessorCode"].ToString();
                            UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                            if (UserLogin == null || LessorCode == null)
                            {
                                RedirectToAction("Account", "Login");
                            }

                            LessorCode = Session["LessorCode"].ToString();
                            bool price = false;
                            if (cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Sector != "0")
                            {
                                price = db.CR_Cas_Car_Price_Basic.Any(p => p.CR_Cas_Car_Price_Basic_Model_Code == cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Model_Code && p.CR_Cas_Car_Price_Basic_Car_Year == cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Car_Year &&
                            p.CR_Cas_Car_Price_Basic_Sector == cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Sector && p.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode && p.CR_Cas_Car_Price_Basic_Status == "A");


                            }
                            else if (cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Sector == "0")
                            {
                                price = db.CR_Cas_Car_Price_Basic.Any(p => p.CR_Cas_Car_Price_Basic_Model_Code == cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Model_Code && p.CR_Cas_Car_Price_Basic_Car_Year == cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Car_Year &&
                             p.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode && p.CR_Cas_Car_Price_Basic_Status == "A" && p.CR_Cas_Car_Price_Basic_Sector == "1");

                            }

                            if (price)
                            {
                                ViewBag.Price = "هذا السعر موجود";
                            }
                            else
                            {
                                CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
                                DateTime year = DateTime.Now;
                                var y = year.ToString("yy");
                                var sector = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Sector;
                                var ProcedureCode = "30";
                                var autoInc = GetLastRecord(ProcedureCode, cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Sector);
                                //var LessorCode = Session["LessorCode"].ToString();

                                var sectorsLis = db.CR_Mas_Sup_Sector.Where(s => s.CR_Mas_Sup_Sector_Code != "0" && s.CR_Mas_Sup_Sector_Status == "A");


                                var CR_Mas_Sup_Choices_Code = collection["item.CR_Mas_Sup_Choices_Code"].Split(',');
                                List<CR_Cas_Car_Price_Choices> choiceslist = new List<CR_Cas_Car_Price_Choices>();


                                var AdditionalCode = collection["item.CR_Mas_Sup_Additional_Code"].Split(',');
                                List<CR_Cas_Car_Price_Additional> LAdd = new List<CR_Cas_Car_Price_Additional>();



                                bool checkF = false;
                                foreach (string item in collection.AllKeys)
                                {
                                    if (item.Equals("item.CR_Cas_Sup_Features_Code"))
                                    {
                                        checkF = true;
                                    }
                                }
                                string[] FeaturesCode = null;
                                if (checkF == true)
                                {
                                    FeaturesCode = collection["item.CR_Cas_Sup_Features_Code"].Split(',');
                                }




                                List<CR_Cas_Car_Price_Features> LFeatures = new List<CR_Cas_Car_Price_Features>();



                                var nbrDay = 0;
                                var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.FirstOrDefault(x => x.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == LessorCode && x.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code == "30");
                                if (mech != null)
                                {
                                    nbrDay = (int)mech.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire;
                                }
                                //////////////////////////////////////////Save price data ////////////////////////////////////
                                ///
                                cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Sector = "0";////// to delete
                                if (cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Sector == "0")
                                {
                                    var sectors = db.CR_Mas_Sup_Sector.Where(s => s.CR_Mas_Sup_Sector_Code != "0" && s.CR_Mas_Sup_Sector_Status == "A");

                                    foreach (var s in sectors)
                                    {

                                        autoInc = GetLastRecord(ProcedureCode, s.CR_Mas_Sup_Sector_Code);
                                        ///////////////////////////////Tracing//////////////////////////////////////

                                        Ad = new CR_Cas_Administrative_Procedures();
                                        Ad.CR_Cas_Administrative_Procedures_No = y + "-" + s.CR_Mas_Sup_Sector_Code + "-" + ProcedureCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
                                        Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
                                        string currentTime = DateTime.Now.ToString("HH:mm:ss");
                                        Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
                                        Ad.CR_Cas_Administrative_Procedures_Year = y;
                                        Ad.CR_Cas_Administrative_Procedures_Sector = s.CR_Mas_Sup_Sector_Code;
                                        Ad.CR_Cas_Administrative_Procedures_Code = ProcedureCode;
                                        Ad.CR_Cas_Administrative_Int_Procedures_Code = int.Parse(ProcedureCode);
                                        Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
                                        Ad.CR_Cas_Administrative_Procedures_Targeted_Action = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Model_Code + ";" + listyear;
                                        Ad.CR_Cas_Administrative_Procedures_User_Insert = Session["UserLogin"].ToString();
                                        Ad.CR_Cas_Administrative_Procedures_Type = "I";
                                        Ad.CR_Cas_Administrative_Procedures_Date = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Date;
                                        Ad.CR_Cas_Administrative_Procedures_Doc_Start_Date = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Start_Date;
                                        Ad.CR_Cas_Administrative_Procedures_Doc_End_Date = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_End_Date;
                                        Ad.CR_Cas_Administrative_Procedures_Action = true;
                                        Ad.CR_Cas_Administrative_Procedures_Doc_Date = DateTime.Now;
                                        Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
                                        Ad.CR_Cas_Administrative_Procedures_Doc_Start_Date = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Start_Date;
                                        Ad.CR_Cas_Administrative_Procedures_Doc_End_Date = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_End_Date;
                                        Ad.CR_Cas_Administrative_Procedures_Doc_No = "";    
                                        db.CR_Cas_Administrative_Procedures.Add(Ad);
                                        ////////////////////////////////////////////////////////////////////////////////////////////////////////

                                        int? nullableInt;

                                        if (int.TryParse(listyear, out int result))
                                        {
                                            nullableInt = result;
                                        }
                                        else
                                        {
                                            nullableInt = null;
                                        }

                                        var currentPrice = db.CR_Cas_Car_Price_Basic.FirstOrDefault(l => l.CR_Cas_Car_Price_Basic_Model_Code == cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Model_Code
                                                                                                      && l.CR_Cas_Car_Price_Basic_Car_Year == nullableInt
                                                                                                      && l.CR_Cas_Car_Price_Basic_Category_Code == cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Category_Code
                                                                                                      && l.CR_Cas_Car_Price_Basic_Brand_Code == cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Brand_Code);
                                        if(currentPrice != null)
                                        {
                                            db.CR_Cas_Car_Price_Basic.Remove(currentPrice);
                                            db.SaveChanges();
                                        }

                                        CR_Cas_Car_Price_Basic n = new CR_Cas_Car_Price_Basic();
                                        n.CR_Cas_Car_Price_Basic_No = y + "-" + s.CR_Mas_Sup_Sector_Code + "-" + ProcedureCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;



                                        n.CR_Cas_Car_Price_Basic_Year = y;
                                        n.CR_Cas_Car_Price_Basic_Type = ProcedureCode;
                                        //n.CR_Cas_Car_Price_Basic_Lessor_Code = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Lessor_Code;
                                        n.CR_Cas_Car_Price_Basic_Date = DateTime.Now;
                                        n.CR_Cas_Car_Price_Basic_Does_Require_Financial_Credit = false;

                                        n.CR_Cas_Car_Price_Basic_Sector = s.CR_Mas_Sup_Sector_Code;
                                        if (listyear != null)
                                        {
                                            n.CR_Cas_Car_Price_Basic_Car_Year = int.Parse(listyear);
                                        }


                                        if (cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_End_Date.ToString() != "" && cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_End_Date != null)
                                        {
                                            DateTime EndDate = (DateTime)cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_End_Date;
                                            var aboutExpire = EndDate.AddDays(-nbrDay);
                                            n.CR_Cas_Car_Price_Basic_About_To_Expire = aboutExpire;
                                            var x = (EndDate - DateTime.Now).TotalDays;
                                            if (x > nbrDay)
                                            {
                                                n.CR_Cas_Car_Price_Basic_Status = "A";
                                            }
                                            else
                                            {
                                                n.CR_Cas_Car_Price_Basic_Status = "X";
                                            }
                                        }



                                        //n.CR_Cas_Car_Price_Basic_About_To_Expire = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_About_To_Expire;
                                        n.CR_Cas_Car_Price_Basic_Activation = mech.CR_Cas_Sup_Follow_Up_Mechanism_Activate_Service;
                                        n.CR_Cas_Car_Price_Basic_Additional_KM_Value = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Additional_KM_Value;
                                        n.CR_Cas_Car_Price_Basic_Brand_Code = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Brand_Code;
                                        n.CR_Cas_Car_Price_Basic_Carrying_Accident = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Carrying_Accident;
                                        n.CR_Cas_Car_Price_Basic_Carrying_Fire = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Carrying_Fire;
                                        n.CR_Cas_Car_Price_Basic_Car_Year = int.Parse(listyear);
                                        n.CR_Cas_Car_Price_Basic_Category_Code = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Category_Code;
                                        n.CR_Cas_Car_Price_Basic_Daily_Rent = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Daily_Rent;
                                        n.CR_Cas_Car_Price_Basic_Drowning = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Drowning;
                                        n.CR_Cas_Car_Price_Basic_End_Date = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_End_Date;
                                        n.CR_Cas_Car_Price_Basic_Extra_Hour_Value = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Extra_Hour_Value;
                                        n.CR_Cas_Car_Price_Basic_Hour_Max = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Hour_Max;
                                        n.CR_Cas_Car_Price_Basic_Max_Age_Renter = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Max_Age_Renter;
                                        n.CR_Cas_Car_Price_Basic_Min_Age_Renter = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Min_Age_Renter;
                                        n.CR_Cas_Car_Price_Basic_Min_Percentage_Contract_Value = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Min_Percentage_Contract_Value;
                                        n.CR_Cas_Car_Price_Basic_Model_Code = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Model_Code;
                                        n.CR_Cas_Car_Price_Basic_Monthly_Rent = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Monthly_Rent;
                                        n.CR_Cas_Car_Price_Basic_No_Daily_Free_KM = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_No_Daily_Free_KM;
                                        n.CR_Cas_Car_Price_Basic_No_Free_Additional_Hours = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_No_Free_Additional_Hours;
                                        n.CR_Cas_Car_Price_Basic_Rental_Tax_Rate = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Rental_Tax_Rate;
                                        n.CR_Cas_Car_Price_Basic_Start_Date = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Start_Date;
                                        n.CR_Cas_Car_Price_Basic_Stealing = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Stealing;
                                        n.CR_Cas_Car_Price_Basic_Weekly_Rent = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Weekly_Rent;
                                        n.CR_Cas_Car_Price_Basic_Lessor_Code = LessorCode;
                                        n.CR_Cas_Car_Price_Basic_Additional_Driver = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Additional_Driver;
                                        n.CR_Cas_Car_Price_Basic_Additional_Driver_Value = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Additional_Driver_Value;
                                        n.CR_Cas_Car_Price_Basic_Internal_Fees_Tamm = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Internal_Fees_Tamm;
                                        n.CR_Cas_Car_Price_Basic_International_Fees_Tamm = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_International_Fees_Tamm;

                                        db.CR_Cas_Car_Price_Basic.Add(n);



                                        /////////////////////////////////////////////////Save choices////////////////////////////////////////////
                                        foreach (string code in CR_Mas_Sup_Choices_Code)
                                        {
                                            var p = collection["val_" + code];
                                            if (p != null && p != "")
                                            {
                                                CR_Cas_Car_Price_Choices ch = new CR_Cas_Car_Price_Choices();
                                                ch.CR_Cas_Car_Price_Choices_No = y + "-" + s.CR_Mas_Sup_Sector_Code + "-" + ProcedureCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
                                                ch.CR_Cas_Car_Price_Choices_Code = code;
                                                ch.CR_Cas_Car_Price_Choices_Value = Convert.ToDecimal(collection["val_" + code]);
                                                if (ch.CR_Cas_Car_Price_Choices_Value != null && ch.CR_Cas_Car_Price_Choices_Value > 0)
                                                {
                                                    choiceslist.Add(ch);
                                                }
                                            }
                                        }

                                        ///////////////////////////////////////////save additional///////////////////////////////////////
                                        foreach (string itemcode in AdditionalCode)
                                        {
                                            var c = collection["v_" + itemcode];
                                            if (c != null && c != "")
                                            {
                                                CR_Cas_Car_Price_Additional ad = new CR_Cas_Car_Price_Additional();
                                                ad.CR_Cas_Car_Price_Additional_No = y + "-" + s.CR_Mas_Sup_Sector_Code + "-" + ProcedureCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
                                                ad.CR_Cas_Car_Price_Additional_Code = itemcode;
                                                ad.CR_Cas_Car_Price_Additional_Value = Convert.ToDecimal(collection["v_" + itemcode]);
                                                if (ad.CR_Cas_Car_Price_Additional_Value != null && ad.CR_Cas_Car_Price_Additional_Value > 0)
                                                {
                                                    LAdd.Add(ad);
                                                }
                                            }
                                        }


                                        ///////////////////////////////////////////save Features///////////////////////////////////////
                                        if (FeaturesCode != null)
                                        {
                                            foreach (string FCode in FeaturesCode)
                                            {
                                                var c = collection["F_" + FCode];
                                                if (c != null && c != "")
                                                {
                                                    CR_Cas_Car_Price_Features f = new CR_Cas_Car_Price_Features();
                                                    f.CR_Cas_Car_Price_Features_No = y + "-" + s.CR_Mas_Sup_Sector_Code + "-" + ProcedureCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
                                                    f.CR_Cas_Car_Price_Features_Code = FCode;
                                                    f.CR_Cas_Car_Price_Features_Value = Convert.ToDecimal(collection["F_" + FCode]);
                                                    if (f.CR_Cas_Car_Price_Features_Value != null && f.CR_Cas_Car_Price_Features_Value > 0)
                                                    {
                                                        LFeatures.Add(f);
                                                    }

                                                }
                                            }
                                        }


                                    }

                                    if (listyear != null && listyear != "")
                                    {
                                        var CarYear = int.Parse(listyear);
                                        var cars = db.CR_Cas_Sup_Car_Information.Where(c => c.CR_Cas_Sup_Car_Lessor_Code == LessorCode
                                        && c.CR_Cas_Sup_Car_Year == CarYear
                                        && c.CR_Cas_Sup_Car_Model_Code == cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Model_Code);
                                        foreach (var car in cars)
                                        {
                                            car.CR_Cas_Sup_Car_Price_Status = "1";
                                            db.Entry(car).State = EntityState.Modified;
                                        }
                                    }

                                }
                                else
                                {

                                    ///////////////////////////////Tracing//////////////////////////////////////


                                    Ad.CR_Cas_Administrative_Procedures_No = y + "-" + sector + "-" + ProcedureCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
                                    Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
                                    string currentTime = DateTime.Now.ToString("HH:mm:ss");
                                    Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
                                    Ad.CR_Cas_Administrative_Procedures_Year = y;
                                    Ad.CR_Cas_Administrative_Procedures_Sector = sector;
                                    Ad.CR_Cas_Administrative_Procedures_Code = ProcedureCode;
                                    Ad.CR_Cas_Administrative_Int_Procedures_Code = 30;
                                    Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
                                    Ad.CR_Cas_Administrative_Procedures_Targeted_Action = "";
                                    Ad.CR_Cas_Administrative_Procedures_User_Insert = Session["UserLogin"].ToString();
                                    Ad.CR_Cas_Administrative_Procedures_Type = "I";
                                    Ad.CR_Cas_Administrative_Procedures_Action = true;
                                    Ad.CR_Cas_Administrative_Procedures_Date = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Date;
                                    Ad.CR_Cas_Administrative_Procedures_Doc_Start_Date = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Start_Date;
                                    Ad.CR_Cas_Administrative_Procedures_Doc_End_Date = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_End_Date;
                                    Ad.CR_Cas_Administrative_Procedures_Doc_Date = DateTime.Now;
                                    Ad.CR_Cas_Administrative_Procedures_Doc_Start_Date = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Start_Date;
                                    Ad.CR_Cas_Administrative_Procedures_Doc_End_Date = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_End_Date;
                                    Ad.CR_Cas_Administrative_Procedures_Doc_No = "";
                                    Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
                                    db.CR_Cas_Administrative_Procedures.Add(Ad);
                                    ////////////////////////////////////////////////////////////////////////////////////////////////////////


                                    cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_No = Ad.CR_Cas_Administrative_Procedures_No;
                                    cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Year = y;
                                    if (listyear != null)
                                    {
                                        cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Car_Year = int.Parse(listyear);
                                    }

                                    cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Type = ProcedureCode;
                                    cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Lessor_Code = LessorCode;
                                    cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Date = DateTime.Now;
                                    cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Does_Require_Financial_Credit = false;
                                    cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Status = "A";
                                    //cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Sector = sector;

                                    if (cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_End_Date.ToString() != "" && cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_End_Date != null)
                                    {
                                        DateTime EndDate = (DateTime)cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_End_Date;
                                        var aboutExpire = EndDate.AddDays(-nbrDay);
                                        cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_About_To_Expire = aboutExpire;
                                    }
                                    db.CR_Cas_Car_Price_Basic.Add(cR_Cas_Car_Price_Basic);



                                    /////////////////////////////////////////////////Save choices////////////////////////////////////////////
                                    foreach (string code in CR_Mas_Sup_Choices_Code)
                                    {
                                        var p = collection["val_" + code];
                                        if (p != null && p != "")
                                        {
                                            CR_Cas_Car_Price_Choices ch = new CR_Cas_Car_Price_Choices();
                                            ch.CR_Cas_Car_Price_Choices_No = y + "-" + cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Sector + "-" + ProcedureCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
                                            ch.CR_Cas_Car_Price_Choices_Code = code;
                                            ch.CR_Cas_Car_Price_Choices_Value = Convert.ToDecimal(collection["val_" + code]);
                                            //choiceslist.Add(ch);

                                            if (ch.CR_Cas_Car_Price_Choices_Value != null && ch.CR_Cas_Car_Price_Choices_Value > 0)
                                            {
                                                choiceslist.Add(ch);
                                            }
                                        }
                                    }

                                    ///////////////////////////////////////////save additional///////////////////////////////////////
                                    foreach (string itemcode in AdditionalCode)
                                    {

                                        var c = collection["v_" + itemcode];
                                        if (c != null && c != "")
                                        {
                                            CR_Cas_Car_Price_Additional ad = new CR_Cas_Car_Price_Additional();
                                            ad.CR_Cas_Car_Price_Additional_No = y + "-" + cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Sector + "-" + ProcedureCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
                                            ad.CR_Cas_Car_Price_Additional_Code = itemcode;
                                            ad.CR_Cas_Car_Price_Additional_Value = Convert.ToDecimal(collection["v_" + itemcode]);
                                            //LAdd.Add(ad);
                                            if (ad.CR_Cas_Car_Price_Additional_Value != null && ad.CR_Cas_Car_Price_Additional_Value > 0)
                                            {
                                                LAdd.Add(ad);
                                            }
                                        }
                                    }

                                    ///////////////////////////////////////////save Features///////////////////////////////////////
                                    if (FeaturesCode != null)
                                    {
                                        foreach (string FCode in FeaturesCode)
                                        {
                                            var c = collection["F_" + FCode];
                                            if (c != null && c != "")
                                            {
                                                CR_Cas_Car_Price_Features f = new CR_Cas_Car_Price_Features();
                                                f.CR_Cas_Car_Price_Features_No = y + "-" + cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Sector + "-" + ProcedureCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
                                                f.CR_Cas_Car_Price_Features_Code = FCode;
                                                f.CR_Cas_Car_Price_Features_Value = Convert.ToDecimal(collection["F_" + FCode]);
                                                //LFeatures.Add(f);
                                                if (f.CR_Cas_Car_Price_Features_Value != null && f.CR_Cas_Car_Price_Features_Value > 0)
                                                {
                                                    LFeatures.Add(f);
                                                }
                                            }
                                        }
                                    }

                                    if (listyear != null && listyear != "")
                                    {
                                        var CarYear = int.Parse(listyear);
                                        var cars = db.CR_Cas_Sup_Car_Information.Where(c => c.CR_Cas_Sup_Car_Lessor_Code == LessorCode
                                        && c.CR_Cas_Sup_Car_Year == CarYear
                                        && c.CR_Cas_Sup_Car_Model_Code == cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Model_Code);
                                        foreach (var car in cars)
                                        {
                                            car.CR_Cas_Sup_Car_Price_Status = "1";
                                            db.Entry(car).State = EntityState.Modified;
                                        }
                                    }

                                }

                                choiceslist.ForEach(c => db.CR_Cas_Car_Price_Choices.Add(c));
                                LAdd.ForEach(f => db.CR_Cas_Car_Price_Additional.Add(f));
                                LFeatures.ForEach(feat => db.CR_Cas_Car_Price_Features.Add(feat));
                                ////////////////////////////////////////////////////////////////////////////////////////////////////////
                                ///
                                db.SaveChanges();
                                TempData["TempModel"] = "Added";
                                dbTran.Commit();
                                return RedirectToAction("Index");


                                ////////////////////////////////////////////////////////////////////////////
                            }


                        }
                        catch (DbEntityValidationException ex)
                        {
                            dbTran.Rollback();
                            throw ex;
                        }
                    }



                }
            }


            ViewBag.CR_Cas_Car_Price_Basic_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Code");
            ViewBag.CR_Cas_Car_Price_Basic_Brand_Code = new SelectList(db.CR_Mas_Sup_Brand, "CR_Mas_Sup_Brand_Code", "CR_Mas_Sup_Brand_Ar_Name");
            ViewBag.CR_Cas_Car_Price_Basic_Category_Code = new SelectList(db.CR_Mas_Sup_Category, "CR_Mas_Sup_Category_Code", "CR_Mas_Sup_Category_Code");
            ViewBag.CR_Cas_Car_Price_Basic_Model_Code = new SelectList(db.CR_Mas_Sup_Model, "CR_Mas_Sup_Model_Code", "CR_Mas_Sup_Model_Ar_Name");



            var Tax = new SelectList(new[] {
                                              new {ID="1",Name="لا"},
                                              new{ID="2",Name="نعم"},
                                              }, "ID", "Name", 1);
            ViewData["Tax"] = Tax;

            ViewBag.CR_Cas_Car_Price_Basic_Sector = new SelectList(db.CR_Mas_Sup_Sector.Where(x => x.CR_Mas_Sup_Sector_Status != "D"), "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name");



            var ListGear = new SelectList(new[] {
                                              new {ID="1",Name="أوتوماتيكي"},
                                              new{ID="2",Name="عادي"},
                                              }, "ID", "Name", 1);
            ViewData["GearType"] = ListGear;

            var d = DateTime.Now.Year.ToString();
            var yd = int.Parse(d);
            var ListYear = new SelectList(new[] {
                                              new{ID=(yd-10).ToString(),Name=(yd-10).ToString()},
                                              new{ID=(yd-9).ToString(),Name=(yd-9).ToString()},
                                              new{ID=(yd-8).ToString(),Name=(yd-8).ToString()},
                                              new{ID=(yd-7).ToString(),Name=(yd-7).ToString()},
                                              new{ID=(yd-6).ToString(),Name=(yd-6).ToString()},
                                              new{ID=(yd-5).ToString(),Name=(yd-5).ToString()},
                                              new{ID=(yd-4).ToString(),Name=(yd-4).ToString()},
                                              new{ID=(yd-3).ToString(),Name=(yd-3).ToString()},
                                              new{ID=(yd-2).ToString(),Name=(yd-2).ToString()},
                                              new{ID=(yd-1).ToString(),Name=(yd-1).ToString()},
                                              new{ID=(yd).ToString(),Name=(yd).ToString()},
                                              new{ID=(yd+1).ToString(),Name=(yd+1).ToString()},
                                              }, "ID", "Name");
            ViewData["ListYear"] = ListYear;


            return View(cR_Cas_Car_Price_Basic);
        }

        // GET: CarBasicPrice/Edit/5




        public PartialViewResult ChoicesListEdit(string serialnumber)
        {
            List<ChoicesListVal> list = new List<ChoicesListVal>();



            var choices = db.CR_Mas_Sup_Choices.ToList();
            foreach (var x in choices)
            {
                ChoicesListVal ch = new ChoicesListVal();
                ch.CR_Mas_Sup_Choices_Ar_Name = x.CR_Mas_Sup_Choices_Ar_Name;
                ch.CR_Mas_Sup_Choices_Code = x.CR_Mas_Sup_Choices_Code;
                ch.CR_Mas_Sup_Choices_En_Name = x.CR_Mas_Sup_Choices_En_Name;
                ch.CR_Mas_Sup_Choices_Fr_Name = x.CR_Mas_Sup_Choices_Fr_Name;
                ch.CR_Mas_Sup_Choices_Group_Code = x.CR_Mas_Sup_Choices_Group_Code;
                ch.CR_Mas_Sup_Choices_Reasons = x.CR_Mas_Sup_Choices_Reasons;
                ch.CR_Mas_Sup_Choices_Status = x.CR_Mas_Sup_Choices_Status;
                list.Add(ch);
            }

            var CasChoices = db.CR_Cas_Car_Price_Choices.Where(c => c.CR_Cas_Car_Price_Choices_No == serialnumber);
            foreach (var c in CasChoices)
            {
                foreach (var m in list)
                {
                    if (c.CR_Cas_Car_Price_Choices_Code == m.CR_Mas_Sup_Choices_Code)
                    {
                        m.CR_Cas_Car_Price_Choices_Value = c.CR_Cas_Car_Price_Choices_Value;
                    }
                }
            }
            return PartialView(list);
        }


        public PartialViewResult FeaturesListEdit(string serialnumber)
        {
            List<FeaturesListVal> list = new List<FeaturesListVal>();

            var FeaturesPrice = db.CR_Cas_Car_Price_Features.Where(c => c.CR_Cas_Car_Price_Features_No == serialnumber);
            foreach (var x in FeaturesPrice)
            {
                FeaturesListVal f = new FeaturesListVal();
                f.CR_Cas_Sup_Features_Serial_No = serialnumber;
                f.CR_Cas_Sup_Features_Code = x.CR_Cas_Car_Price_Features_Code;
                f.CR_Cas_Car_Price_Features_Value = x.CR_Cas_Car_Price_Features_Value;
                list.Add(f);
            }

            var features = db.CR_Mas_Sup_Features.ToList();
            foreach (var f in features)
            {
                foreach (var p in list)
                {
                    if (p.CR_Cas_Sup_Features_Code == f.CR_Mas_Sup_Features_Code)
                    {
                        p.Name = f.CR_Mas_Sup_Features_Ar_Name;
                    }
                }
            }
            return PartialView(list);
        }


        public PartialViewResult AdditionalListEdit(string serialnumber)
        {
            List<AdditionalListVal> list = new List<AdditionalListVal>();

            var AdditionalPrice = db.CR_Cas_Car_Price_Additional.Where(c => c.CR_Cas_Car_Price_Additional_No == serialnumber);
            foreach (var x in AdditionalPrice)
            {
                AdditionalListVal ad = new AdditionalListVal();
                ad.CR_Cas_Car_Price_Additional_No = serialnumber;
                ad.CR_Cas_Car_Price_Additional_Code = x.CR_Cas_Car_Price_Additional_Code;
                ad.CR_Cas_Car_Price_Additional_Value = x.CR_Cas_Car_Price_Additional_Value;
                list.Add(ad);
            }

            var additionals = db.CR_Mas_Sup_Additional.ToList();
            foreach (var d in additionals)
            {
                foreach (var l in list)
                {
                    if (l.CR_Cas_Car_Price_Additional_Code == d.CR_Mas_Sup_Additional_Code)
                    {
                        l.Name = d.CR_Mas_Sup_Additional_Ar_Name;
                    }
                }
            }
            return PartialView(list);
        }

        public ActionResult Edit(string id)
        {
            var LessorCode = "";
            var UserLogin = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Car_Price_Basic cR_Cas_Car_Price_Basic = db.CR_Cas_Car_Price_Basic.Find(id);
            if (cR_Cas_Car_Price_Basic == null)
            {
                return HttpNotFound();
            }
            ViewBag.CR_Cas_Car_Price_Basic_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Lessor_Code);
            ViewBag.CR_Cas_Car_Price_Basic_Brand_Code = new SelectList(db.CR_Mas_Sup_Brand, "CR_Mas_Sup_Brand_Code", "CR_Mas_Sup_Brand_Ar_Name", cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Brand_Code);
            ViewBag.CR_Cas_Car_Price_Basic_Category_Code = new SelectList(db.CR_Mas_Sup_Category, "CR_Mas_Sup_Category_Code", "CR_Mas_Sup_Category_Group_Code", cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Category_Code);
            ViewBag.CR_Cas_Car_Price_Basic_Model_Code = new SelectList(db.CR_Mas_Sup_Model, "CR_Mas_Sup_Model_Code", "CR_Mas_Sup_Model_Ar_Name", cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Model_Code);
            ViewBag.CR_Cas_Car_Price_Basic_Sector = new SelectList(db.CR_Mas_Sup_Sector, "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name", "0");

            ViewBag.StartDate = string.Format("{0:yyyy/MM/dd}", cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Start_Date);
            ViewBag.EndDate = string.Format("{0:yyyy/MM/dd}", cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_End_Date);


            var Tax = new SelectList(new[] {
                                              new {ID="1",Name="لا"},
                                              new{ID="2",Name="نعم"},
                                              }, "ID", "Name", 1);
            ViewData["Tax"] = Tax;

            var d = DateTime.Now.Year.ToString();
            var yd = int.Parse(d);
            var ListYear = new SelectList(new[] {
                                              new{ID=(yd-10).ToString(),Name=(yd-10).ToString()},
                                              new{ID=(yd-9).ToString(),Name=(yd-9).ToString()},
                                              new{ID=(yd-8).ToString(),Name=(yd-8).ToString()},
                                              new{ID=(yd-7).ToString(),Name=(yd-7).ToString()},
                                              new{ID=(yd-6).ToString(),Name=(yd-6).ToString()},
                                              new{ID=(yd-5).ToString(),Name=(yd-5).ToString()},
                                              new{ID=(yd-4).ToString(),Name=(yd-4).ToString()},
                                              new{ID=(yd-3).ToString(),Name=(yd-3).ToString()},
                                              new{ID=(yd-2).ToString(),Name=(yd-2).ToString()},
                                              new{ID=(yd-1).ToString(),Name=(yd-1).ToString()},
                                              new{ID=(yd).ToString(),Name=(yd).ToString()},
                                              new{ID=(yd+1).ToString(),Name=(yd+1).ToString()},
                                              }, "ID", "Name", cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Car_Year);
            ViewData["ListYear"] = ListYear;

            ViewBag.serialNumber = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_No;
            return View(cR_Cas_Car_Price_Basic);
        }

        // POST: CarBasicPrice/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Car_Price_Basic_No,CR_Cas_Car_Price_Basic_Year,CR_Cas_Car_Price_Basic_Type," +
            "CR_Cas_Car_Price_Basic_Lessor_Code,CR_Cas_Car_Price_Basic_Sector,CR_Cas_Car_Price_Basic_Brand_Code,CR_Cas_Car_Price_Basic_Model_Code," +
            "CR_Cas_Car_Price_Basic_Car_Year,CR_Cas_Car_Price_Basic_Category_Code,CR_Cas_Car_Price_Basic_Date,CR_Cas_Car_Price_Basic_Start_Date," +
            "CR_Cas_Car_Price_Basic_End_Date,CR_Cas_Car_Price_Basic_Daily_Rent,CR_Cas_Car_Price_Basic_Weekly_Rent,CR_Cas_Car_Price_Basic_Monthly_Rent," +
            "CR_Cas_Car_Price_Basic_Rental_Tax_Rate,CR_Cas_Car_Price_Basic_No_Daily_Free_KM,CR_Cas_Car_Price_Basic_Additional_KM_Value," +
            "CR_Cas_Car_Price_Basic_No_Free_Additional_Hours,CR_Cas_Car_Price_Basic_Hour_Max,CR_Cas_Car_Price_Basic_Extra_Hour_Value," +
            "CR_Cas_Car_Price_Basic_Min_Age_Renter,CR_Cas_Car_Price_Basic_Max_Age_Renter,CR_Cas_Car_Price_Basic_Does_Require_Financial_Credit," +
            "CR_Cas_Car_Price_Basic_Min_Percentage_Contract_Value,CR_Cas_Car_Price_Basic_Carrying_Accident,CR_Cas_Car_Price_Basic_Carrying_Fire," +
            "CR_Cas_Car_Price_Basic_Stealing,CR_Cas_Car_Price_Basic_Drowning,CR_Cas_Car_Price_Basic_Activation,CR_Cas_Car_Price_Basic_About_To_Expire," +
            "CR_Cas_Car_Price_Basic_Status,CR_Cas_Car_Price_Basic_Reasons")] CR_Cas_Car_Price_Basic cR_Cas_Car_Price_Basic, string Delete, string listyear)
        {
            if (ModelState.IsValid)
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var LessorCode = Session["LessorCode"].ToString();

                        if (Delete == "Delete" || Delete == "حذف")
                        {
                            {
                                var prices = db.CR_Cas_Car_Price_Basic.Where(p => p.CR_Cas_Car_Price_Basic_Model_Code == cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Model_Code &&
                                p.CR_Cas_Car_Price_Basic_Car_Year == cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Car_Year && p.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode && p.CR_Cas_Car_Price_Basic_Status == "A");
                                foreach (var price in prices)
                                {
                                    CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
                                    DateTime year = DateTime.Now;
                                    var y = year.ToString("yy");
                                    var sector = price.CR_Cas_Car_Price_Basic_Sector;
                                    var ProcedureCode = "30";
                                    var autoInc = GetLastRecord(ProcedureCode, sector);

                                    // var sectors = db.CR_Mas_Sup_Sector.Where(s => s.CR_Mas_Sup_Sector_Code != "0" && s.CR_Mas_Sup_Sector_Status == "A");


                                    ///////////////////////////////Tracing//////////////////////////////////////
                                    Ad.CR_Cas_Administrative_Procedures_No = y + "-" + sector + "-" + ProcedureCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
                                    Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
                                    string currentTime = DateTime.Now.ToString("HH:mm:ss");
                                    Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
                                    Ad.CR_Cas_Administrative_Procedures_Year = y;
                                    Ad.CR_Cas_Administrative_Procedures_Sector = sector;
                                    Ad.CR_Cas_Administrative_Procedures_Code = ProcedureCode;
                                    Ad.CR_Cas_Administrative_Int_Procedures_Code = 30;
                                    Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
                                    Ad.CR_Cas_Administrative_Procedures_Targeted_Action = cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Model_Code + ";" + cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Car_Year;
                                    Ad.CR_Cas_Administrative_Procedures_User_Insert = Session["UserLogin"].ToString();
                                    Ad.CR_Cas_Administrative_Procedures_Type = "D";
                                    Ad.CR_Cas_Administrative_Procedures_Action = true;
                                    Ad.CR_Cas_Administrative_Procedures_Doc_Date = null;
                                    Ad.CR_Cas_Administrative_Procedures_Doc_Start_Date = null;
                                    Ad.CR_Cas_Administrative_Procedures_Doc_End_Date = null;
                                    Ad.CR_Cas_Administrative_Procedures_Doc_No = "";
                                    db.CR_Cas_Administrative_Procedures.Add(Ad);
                                    ////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    if (listyear != null)
                                    {
                                        cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Car_Year = int.Parse(listyear);
                                    }

                                    price.CR_Cas_Car_Price_Basic_Year = y;
                                    price.CR_Cas_Car_Price_Basic_Type = ProcedureCode;
                                    price.CR_Cas_Car_Price_Basic_Lessor_Code = Session["UserLogin"].ToString();
                                    price.CR_Cas_Car_Price_Basic_Date = DateTime.Now;
                                    price.CR_Cas_Car_Price_Basic_Does_Require_Financial_Credit = false;
                                    price.CR_Cas_Car_Price_Basic_Status = "D";
                                    price.CR_Cas_Car_Price_Basic_Type = "30";
                                    price.CR_Cas_Car_Price_Basic_Lessor_Code = LessorCode;
                                    //////////////////////////////////////////////////////////////////////////////////////////////////////
                                    db.Entry(price).State = EntityState.Modified;
                                }


                                var cars = db.CR_Cas_Sup_Car_Information.Where(c => c.CR_Cas_Sup_Car_Lessor_Code == LessorCode
                                && c.CR_Cas_Sup_Car_Year == cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Car_Year
                                && c.CR_Cas_Sup_Car_Model_Code == cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Model_Code);
                                foreach (var car in cars)
                                {
                                    car.CR_Cas_Sup_Car_Price_Status = "0";
                                    db.Entry(car).State = EntityState.Modified;
                                }


                                db.SaveChanges();
                                TempData["TempModel"] = "Deleted";
                                dbTran.Commit();
                                return RedirectToAction("Index");
                            }
                        }
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }
            }
            ViewBag.CR_Cas_Car_Price_Basic_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Lessor_Code);
            ViewBag.CR_Cas_Car_Price_Basic_Brand_Code = new SelectList(db.CR_Mas_Sup_Brand, "CR_Mas_Sup_Brand_Code", "CR_Mas_Sup_Brand_Ar_Name", cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Brand_Code);
            ViewBag.CR_Cas_Car_Price_Basic_Category_Code = new SelectList(db.CR_Mas_Sup_Category, "CR_Mas_Sup_Category_Code", "CR_Mas_Sup_Category_Group_Code", cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Category_Code);
            ViewBag.CR_Cas_Car_Price_Basic_Model_Code = new SelectList(db.CR_Mas_Sup_Model, "CR_Mas_Sup_Model_Code", "CR_Mas_Sup_Model_Group_Code", cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Model_Code);
            ViewBag.CR_Cas_Car_Price_Basic_Sector = new SelectList(db.CR_Mas_Sup_Sector, "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name", cR_Cas_Car_Price_Basic.CR_Cas_Car_Price_Basic_Sector);

            var d = DateTime.Now.Year.ToString();
            var yd = int.Parse(d);
            var ListYear = new SelectList(new[] {
                                              new{ID=(yd-10).ToString(),Name=(yd-10).ToString()},
                                              new{ID=(yd-9).ToString(),Name=(yd-9).ToString()},
                                              new{ID=(yd-8).ToString(),Name=(yd-8).ToString()},
                                              new{ID=(yd-7).ToString(),Name=(yd-7).ToString()},
                                              new{ID=(yd-6).ToString(),Name=(yd-6).ToString()},
                                              new{ID=(yd-5).ToString(),Name=(yd-5).ToString()},
                                              new{ID=(yd-4).ToString(),Name=(yd-4).ToString()},
                                              new{ID=(yd-3).ToString(),Name=(yd-3).ToString()},
                                              new{ID=(yd-2).ToString(),Name=(yd-2).ToString()},
                                              new{ID=(yd-1).ToString(),Name=(yd-1).ToString()},
                                              new{ID=(yd).ToString(),Name=(yd).ToString()},
                                              new{ID=(yd+1).ToString(),Name=(yd+1).ToString()},
                                              }, "ID", "Name");
            ViewData["ListYear"] = ListYear;

            return View(cR_Cas_Car_Price_Basic);
        }

        // GET: CarBasicPrice/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Car_Price_Basic cR_Cas_Car_Price_Basic = db.CR_Cas_Car_Price_Basic.Find(id);
            if (cR_Cas_Car_Price_Basic == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Car_Price_Basic);
        }

        // POST: CarBasicPrice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Cas_Car_Price_Basic cR_Cas_Car_Price_Basic = db.CR_Cas_Car_Price_Basic.Find(id);
            db.CR_Cas_Car_Price_Basic.Remove(cR_Cas_Car_Price_Basic);
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
