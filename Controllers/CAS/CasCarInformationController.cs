using RentCar.Models;
using RentCar.Models.CAS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
namespace RentCar.Controllers.CAS
{
    public class CasCarInformationController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: CasCarInformation

        public PartialViewResult _CarInfoFeatures()
        {

            var features = db.CR_Mas_Sup_Features.Where(x => x.CR_Mas_Sup_Features_Status == "A");
            return PartialView(features);

        }


        public PartialViewResult _CasInfoFeatures(string serialnumber)
        {

            //////var Features = db.CR_Cas_Sup_Features.
            //////   Join(db.CR_Mas_Sup_Features, c => c.CR_Cas_Sup_Features_Code, m => m.CR_Mas_Sup_Features_Code,
            //////   (c, m) => new { c, m })
            //////   .Where(m => m.c.CR_Cas_Sup_Features_Serial_No == serialnumber)
            //////   .Select(x => new MasCasFeaturesModel
            //////   {
            //////       CR_Cas_Sup_Features_Serial_No = x.c.CR_Cas_Sup_Features_Serial_No,
            //////       CR_Mas_Sup_Features_Name = x.m.CR_Mas_Sup_Features_Ar_Name,
            //////       CR_Cas_Sup_Features_Code = x.c.CR_Cas_Sup_Features_Code,
            //////       CR_Cas_Sup_Features_Model_Code = x.c.CR_Cas_Sup_Features_Model_Code,
            //////       CR_Cas_Sup_Features_Model_Year = x.c.CR_Cas_Sup_Features_Model_Year,
            //////   });
            ///

            List<MasCasFeaturesModel> list = new List<MasCasFeaturesModel>();



            var Features = db.CR_Mas_Sup_Features.ToList();
            foreach (var x in Features)
            {
                MasCasFeaturesModel m = new MasCasFeaturesModel();
                m.CR_Cas_Sup_Features_Code = x.CR_Mas_Sup_Features_Code;
                m.CR_Mas_Sup_Features_Name = x.CR_Mas_Sup_Features_Ar_Name;
                list.Add(m);
            }

            var CasFeatures = db.CR_Cas_Sup_Features.Where(f => f.CR_Cas_Sup_Features_Serial_No == serialnumber);
            foreach (var c in CasFeatures)
            {
                foreach (var m in list)
                {
                    if (c.CR_Cas_Sup_Features_Code == m.CR_Cas_Sup_Features_Code)
                    {
                        m.CR_Cas_Sup_Features_Lessor_Code = c.CR_Cas_Sup_Features_Lessor_Code;
                        m.CR_Cas_Sup_Features_Model_Code = c.CR_Cas_Sup_Features_Model_Code;
                        m.CR_Cas_Sup_Features_Model_Year = c.CR_Cas_Sup_Features_Model_Year;
                        m.CR_Cas_Sup_Features_Serial_No = c.CR_Cas_Sup_Features_Serial_No;
                        m.check = true;
                    }
                }
            }


            return PartialView(list);
        }


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

        public PartialViewResult PartialIndex(string type)
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


            IQueryable<CR_Cas_Sup_Car_Information> cR_Cas_Sup_Car_Information = null;
            if (type == "All")
            {
                cR_Cas_Sup_Car_Information = db.CR_Cas_Sup_Car_Information.Where(x => x.CR_Cas_Sup_Car_Lessor_Code == LessorCode )
                .Include(c => c.CR_Cas_Sup_Beneficiary)
                .Include(c => c.CR_Cas_Sup_Branch)
                .Include(c => c.CR_Cas_Sup_Owners)
                .Include(c => c.CR_Mas_Com_Lessor)
                .Include(c => c.CR_Mas_Sup_Brand)
                .Include(c => c.CR_Mas_Sup_Color)
                .Include(c => c.CR_Mas_Sup_Model)
                .Include(c => c.CR_Mas_Sup_Registration_Car)
                .Include(c => c.CR_Mas_Sup_Category_Car);
            }   else if (type == "D")
            {
                cR_Cas_Sup_Car_Information = db.CR_Cas_Sup_Car_Information.Where(x => x.CR_Cas_Sup_Car_Lessor_Code == LessorCode && x.CR_Cas_Sup_Car_Status == "D")
                .Include(c => c.CR_Cas_Sup_Beneficiary)
                .Include(c => c.CR_Cas_Sup_Branch)
                .Include(c => c.CR_Cas_Sup_Owners)
                .Include(c => c.CR_Mas_Com_Lessor)
                .Include(c => c.CR_Mas_Sup_Brand)
                .Include(c => c.CR_Mas_Sup_Color)
                .Include(c => c.CR_Mas_Sup_Model)
                .Include(c => c.CR_Mas_Sup_Registration_Car)
                .Include(c => c.CR_Mas_Sup_Category_Car);
            } else if (type == "H")
            {
                cR_Cas_Sup_Car_Information = db.CR_Cas_Sup_Car_Information.Where(x => x.CR_Cas_Sup_Car_Lessor_Code == LessorCode && x.CR_Cas_Sup_Car_Status == "H")
                .Include(c => c.CR_Cas_Sup_Beneficiary)
                .Include(c => c.CR_Cas_Sup_Branch)
                .Include(c => c.CR_Cas_Sup_Owners)
                .Include(c => c.CR_Mas_Com_Lessor)
                .Include(c => c.CR_Mas_Sup_Brand)
                .Include(c => c.CR_Mas_Sup_Color)
                .Include(c => c.CR_Mas_Sup_Model)
                .Include(c => c.CR_Mas_Sup_Registration_Car)
                .Include(c => c.CR_Mas_Sup_Category_Car);
            } else if (type == "R")
            {
                cR_Cas_Sup_Car_Information = db.CR_Cas_Sup_Car_Information.Where(x => x.CR_Cas_Sup_Car_Lessor_Code == LessorCode && x.CR_Cas_Sup_Car_Status == "R")
                .Include(c => c.CR_Cas_Sup_Beneficiary)
                .Include(c => c.CR_Cas_Sup_Branch)
                .Include(c => c.CR_Cas_Sup_Owners)
                .Include(c => c.CR_Mas_Com_Lessor)
                .Include(c => c.CR_Mas_Sup_Brand)
                .Include(c => c.CR_Mas_Sup_Color)
                .Include(c => c.CR_Mas_Sup_Model)
                .Include(c => c.CR_Mas_Sup_Registration_Car)
                .Include(c => c.CR_Mas_Sup_Category_Car);
            }
            else if (type == "M")
            {
                cR_Cas_Sup_Car_Information = db.CR_Cas_Sup_Car_Information.Where(x => x.CR_Cas_Sup_Car_Lessor_Code == LessorCode && x.CR_Cas_Sup_Car_Status == "M")
                .Include(c => c.CR_Cas_Sup_Beneficiary)
                .Include(c => c.CR_Cas_Sup_Branch)
                .Include(c => c.CR_Cas_Sup_Owners)
                .Include(c => c.CR_Mas_Com_Lessor)
                .Include(c => c.CR_Mas_Sup_Brand)
                .Include(c => c.CR_Mas_Sup_Color)
                .Include(c => c.CR_Mas_Sup_Model)
                .Include(c => c.CR_Mas_Sup_Registration_Car)
                .Include(c => c.CR_Mas_Sup_Category_Car);
            }
            else if (type == "O")
            {
                cR_Cas_Sup_Car_Information = db.CR_Cas_Sup_Car_Information.Where(x => x.CR_Cas_Sup_Car_Lessor_Code == LessorCode && x.CR_Cas_Sup_Car_Status == "O")
                .Include(c => c.CR_Cas_Sup_Beneficiary)
                .Include(c => c.CR_Cas_Sup_Branch)
                .Include(c => c.CR_Cas_Sup_Owners)
                .Include(c => c.CR_Mas_Com_Lessor)
                .Include(c => c.CR_Mas_Sup_Brand)
                .Include(c => c.CR_Mas_Sup_Color)
                .Include(c => c.CR_Mas_Sup_Model)
                .Include(c => c.CR_Mas_Sup_Registration_Car)
                .Include(c => c.CR_Mas_Sup_Category_Car);
            }
            else if (type == "S")
            {
                cR_Cas_Sup_Car_Information = db.CR_Cas_Sup_Car_Information.Where(x => x.CR_Cas_Sup_Car_Lessor_Code == LessorCode && x.CR_Cas_Sup_Car_Status == "S")
                .Include(c => c.CR_Cas_Sup_Beneficiary)
                .Include(c => c.CR_Cas_Sup_Branch)
                .Include(c => c.CR_Cas_Sup_Owners)
                .Include(c => c.CR_Mas_Com_Lessor)
                .Include(c => c.CR_Mas_Sup_Brand)
                .Include(c => c.CR_Mas_Sup_Color)
                .Include(c => c.CR_Mas_Sup_Model)
                .Include(c => c.CR_Mas_Sup_Registration_Car)
                .Include(c => c.CR_Mas_Sup_Category_Car);
            }
            else
            {
                cR_Cas_Sup_Car_Information = db.CR_Cas_Sup_Car_Information.Where(x => x.CR_Cas_Sup_Car_Lessor_Code == LessorCode && (x.CR_Cas_Sup_Car_Status == "A"))
                .Include(c => c.CR_Cas_Sup_Beneficiary)
                .Include(c => c.CR_Cas_Sup_Branch)
                .Include(c => c.CR_Cas_Sup_Owners)
                .Include(c => c.CR_Mas_Com_Lessor)
                .Include(c => c.CR_Mas_Sup_Brand)
                .Include(c => c.CR_Mas_Sup_Color)
                .Include(c => c.CR_Mas_Sup_Model)
                .Include(c => c.CR_Mas_Sup_Registration_Car)
                .Include(c => c.CR_Mas_Sup_Category_Car);
            }
                


            List<CarInfoModel> cars = new List<CarInfoModel>();
            foreach (var car in cR_Cas_Sup_Car_Information)
            {
                CarInfoModel c = new CarInfoModel();
                c.CR_Cas_Sup_Car_Beneficiary_Code = car.CR_Cas_Sup_Car_Beneficiary_Code;
                c.CR_Cas_Sup_Car_Brand_Code = car.CR_Cas_Sup_Car_Brand_Code;
                c.CR_Cas_Sup_Car_Category_Code = car.CR_Cas_Sup_Car_Category_Code;
                c.CR_Cas_Sup_Car_Collect_Ar_Name = car.CR_Cas_Sup_Car_Collect_Ar_Name;
                c.CR_Cas_Sup_Car_Collect_Ar_Short_Name = car.CR_Cas_Sup_Car_Collect_Ar_Short_Name;
                c.CR_Cas_Sup_Car_Collect_En_Name = car.CR_Cas_Sup_Car_Collect_En_Name;
                c.CR_Cas_Sup_Car_Collect_En_Short_Name = car.CR_Cas_Sup_Car_Collect_En_Short_Name;
                c.CR_Cas_Sup_Car_Collect_Fr_Name = car.CR_Cas_Sup_Car_Collect_Fr_Name;
                c.CR_Cas_Sup_Car_Collect_Fr_Short_Name = car.CR_Cas_Sup_Car_Collect_Fr_Short_Name;
                c.CR_Cas_Sup_Car_Customs_No = car.CR_Cas_Sup_Car_Customs_No;
                c.CR_Cas_Sup_Car_Documentation_Status = car.CR_Cas_Sup_Car_Documentation_Status;
                c.CR_Cas_Sup_Car_In_Main_Color_Code = car.CR_Cas_Sup_Car_In_Main_Color_Code;
                c.CR_Cas_Sup_Car_In_Secondary_Color_Code = car.CR_Cas_Sup_Car_In_Secondary_Color_Code;
                c.CR_Cas_Sup_Car_Joined_Fleet_Date = car.CR_Cas_Sup_Car_Joined_Fleet_Date;
                c.CR_Cas_Sup_Car_Last_Pictures = car.CR_Cas_Sup_Car_Last_Pictures;
                c.CR_Cas_Sup_Car_Left_Fleet_Date = car.CR_Cas_Sup_Car_Left_Fleet_Date;
                c.CR_Cas_Sup_Car_Lessor_Code = car.CR_Cas_Sup_Car_Lessor_Code;
                c.CR_Cas_Sup_Car_Location_Branch_Code = car.CR_Cas_Sup_Car_Location_Branch_Code;
                c.CR_Cas_Sup_Car_Maintenance_Status = car.CR_Cas_Sup_Car_Maintenance_Status;
                c.CR_Cas_Sup_Car_Model_Code = car.CR_Cas_Sup_Car_Model_Code;
                c.CR_Cas_Sup_Car_No_Current_Meter = car.CR_Cas_Sup_Car_No_Current_Meter;
                c.CR_Cas_Sup_Car_Out_Main_Color_Code = car.CR_Cas_Sup_Car_Out_Main_Color_Code;
                c.CR_Cas_Sup_Car_Out_Secondary_Color_Code = car.CR_Cas_Sup_Car_Out_Secondary_Color_Code;
                c.CR_Cas_Sup_Car_Owner_Branch_Code = car.CR_Cas_Sup_Car_Owner_Branch_Code;
                c.CR_Cas_Sup_Car_Owner_Code = car.CR_Cas_Sup_Car_Owner_Code;
                c.CR_Cas_Sup_Car_Plate_Ar_No = car.CR_Cas_Sup_Car_Plate_Ar_No;
                c.CR_Cas_Sup_Car_Plate_En_No = car.CR_Cas_Sup_Car_Plate_En_No;
                c.CR_Cas_Sup_Car_Reasons = car.CR_Cas_Sup_Car_Reasons;
                c.CR_Cas_Sup_Car_Registration_Code = car.CR_Cas_Sup_Car_Registration_Code;
                c.CR_Cas_Sup_Car_Serail_No = car.CR_Cas_Sup_Car_Serail_No;
                c.CR_Cas_Sup_Car_Status = car.CR_Cas_Sup_Car_Status;
                c.CR_Cas_Sup_Car_Structure_No = car.CR_Cas_Sup_Car_Structure_No;
                c.CR_Cas_Sup_Car_Traffic_License_Img = car.CR_Cas_Sup_Car_Traffic_License_Img;
                c.CR_Cas_Sup_Car_Year = car.CR_Cas_Sup_Car_Year;
                c.CR_Cas_Sup_Beneficiary = car.CR_Cas_Sup_Beneficiary;
                c.CR_Cas_Sup_Branch = car.CR_Cas_Sup_Branch;
                c.CR_Cas_Sup_Owners = car.CR_Cas_Sup_Owners;
                c.CR_Mas_Com_Lessor = car.CR_Mas_Com_Lessor;
                c.CR_Mas_Sup_Brand = car.CR_Mas_Sup_Brand;
                c.CR_Mas_Sup_Color = car.CR_Mas_Sup_Color;
                c.CR_Mas_Sup_Model = car.CR_Mas_Sup_Model;
                c.CR_Mas_Sup_Registration_Car = car.CR_Mas_Sup_Registration_Car;
                c.CR_Mas_Sup_Category_Car = car.CR_Mas_Sup_Category_Car;

                var PriceSt = db.CR_Cas_Car_Price_Basic.Any(p => p.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode && p.CR_Cas_Car_Price_Basic_Car_Year == c.CR_Cas_Sup_Car_Year
                && p.CR_Cas_Car_Price_Basic_Model_Code == c.CR_Cas_Sup_Car_Model_Code && (p.CR_Cas_Car_Price_Basic_Status == "A" || p.CR_Cas_Car_Price_Basic_Status == "X"));

                var DocSt = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Serail_No == c.CR_Cas_Sup_Car_Serail_No
                && d.CR_Cas_Sup_Car_Doc_Mainten_Status != "A").Count();

                if (!PriceSt || DocSt > 0)
                {
                    c.Status = false;
                }
                else
                {
                    c.Status = true;
                }


                cars.Add(c);
            }
            return PartialView(cars);
        }
       
        // GET: CasCarInformation/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_Car_Information cR_Cas_Sup_Car_Information = db.CR_Cas_Sup_Car_Information.Find(id);
            if (cR_Cas_Sup_Car_Information == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Sup_Car_Information);
        }



        public JsonResult GetBrand(string ModelCode)
        {
            db.Configuration.ProxyCreationEnabled = false;
            CR_Mas_Sup_Brand b;
            CR_Mas_Sup_Model model = db.CR_Mas_Sup_Model.FirstOrDefault(x => x.CR_Mas_Sup_Model_Code == ModelCode);
            if (model != null)
            {
                b = db.CR_Mas_Sup_Brand.FirstOrDefault(x => x.CR_Mas_Sup_Brand_Code == model.CR_Mas_Sup_Model_Brand_Code);
            }
            else
            {
                b = null;
            }
            CR_Mas_Sup_Brand brand = new CR_Mas_Sup_Brand();
            brand.CR_Mas_Sup_Brand_Code = b.CR_Mas_Sup_Brand_Code;
            brand.CR_Mas_Sup_Brand_Ar_Name = b.CR_Mas_Sup_Brand_Ar_Name;
            brand.CR_Mas_Sup_Brand_En_Name = b.CR_Mas_Sup_Brand_En_Name;


            return Json(brand, JsonRequestBehavior.AllowGet);
        }




        public JsonResult GetCategory(string ModelCode, int Year)
        {
            db.Configuration.ProxyCreationEnabled = false;
            CR_Mas_Sup_Category cat = null;
            CR_Mas_Sup_Category_Car c = db.CR_Mas_Sup_Category_Car.FirstOrDefault(x => x.CR_Mas_Sup_Category_Model_Code == ModelCode && x.CR_Mas_Sup_Category_Car_Year == Year);
            if (c != null)
            {
                cat = db.CR_Mas_Sup_Category.FirstOrDefault(x => x.CR_Mas_Sup_Category_Code == c.CR_Mas_Sup_Category_Car_Code);
                if (cat != null)
                {
                    CR_Mas_Sup_Category category = new CR_Mas_Sup_Category();
                    category.CR_Mas_Sup_Category_Code = c.CR_Mas_Sup_Category_Car_Code;
                    category.CR_Mas_Sup_Category_Ar_Name = cat.CR_Mas_Sup_Category_Ar_Name;
                    if (c.CR_Mas_Sup_Category_Car_Picture.Length > 0)
                    {
                        category.CR_Mas_Sup_Category_Reasons = c.CR_Mas_Sup_Category_Car_Picture.Remove(0, 1);
                    }

                    return Json(category, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                CR_Mas_Sup_Category category = new CR_Mas_Sup_Category();
                category.CR_Mas_Sup_Category_Code = "";
                category.CR_Mas_Sup_Category_Ar_Name = "";
                category.CR_Mas_Sup_Category_Reasons = "";
                return Json(category, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }



        public CR_Cas_Administrative_Procedures GetLastRecord(string ProcedureCode, string sector)
        {
            //var Lrecord = db.CR_Cas_Administrative_Procedures.Max(Lr => Lr.CR_Cas_Administrative_Procedures_No.Substring(Lr.CR_Cas_Administrative_Procedures_No.Length - 7, 7));

            ////var Lrecord = db.CR_Cas_Administrative_Procedures.Where(x => x.CR_Cas_Administrative_Procedures_Lessor == AccountController.LessorCode &&
            ////    x.CR_Cas_Administrative_Procedures_Code == ProcedureCode 
            ////    && x.CR_Cas_Administrative_Procedures_No== x.CR_Cas_Administrative_Procedures_No.Substring(0, 2) 
            ////    && x.CR_Cas_Administrative_Procedures_No == x.CR_Cas_Administrative_Procedures_No.Substring(3, 1))
            ////    .Max(x=>x.CR_Cas_Administrative_Procedures_No.Substring(x.CR_Cas_Administrative_Procedures_No.Length - 7, 7));
            ///
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

        // GET: CasCarInformation/Create
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


            ViewBag.CR_Cas_Sup_Car_Beneficiary_Code = new SelectList(db.CR_Cas_Sup_Beneficiary.Where
                (x => x.CR_Cas_Sup_Beneficiary_Lessor == LessorCode && x.CR_Cas_Sup_Beneficiary_Status == "A"),
                "CR_Cas_Sup_Beneficiary_Code", "CR_Cas_Sup_Beneficiary_Ar_Long_Name");

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


            ViewBag.CR_Cas_Sup_Car_Brand_Code = new SelectList(db.CR_Mas_Sup_Brand.Where(x => x.CR_Mas_Sup_Brand_Status == "A")
                , "CR_Mas_Sup_Brand_Code", "CR_Mas_Sup_Brand_Ar_Name");


            ViewBag.CR_Cas_Sup_Car_Out_Main_Color_Code = new SelectList(db.CR_Mas_Sup_Color.Where(x => x.CR_Mas_Sup_Color_Status == "A")
                , "CR_Mas_Sup_Color_Code", "CR_Mas_Sup_Color_Ar_Name");

            ViewBag.CR_Cas_Sup_Car_Out_Secondary_Color_Code = new SelectList(db.CR_Mas_Sup_Color.Where(x => x.CR_Mas_Sup_Color_Status == "A")
                , "CR_Mas_Sup_Color_Code", "CR_Mas_Sup_Color_Ar_Name");


            ViewBag.CR_Cas_Sup_Car_Model_Code = new SelectList(db.CR_Mas_Sup_Model.Where(x => x.CR_Mas_Sup_Model_Status == "A")
                , "CR_Mas_Sup_Model_Code", "CR_Mas_Sup_Model_Ar_Name");


            ViewBag.CR_Cas_Sup_Car_Registration_Code = new SelectList(db.CR_Mas_Sup_Registration_Car.Where(x => x.CR_Mas_Sup_Registration_Car_Status == "A")
                , "CR_Mas_Sup_Registration_Car_Code", "CR_Mas_Sup_Registration_Car_Ar_Name");


            ViewBag.CR_Cas_Sup_Car_In_Main_Color_Code = new SelectList(db.CR_Mas_Sup_Color.Where(x => x.CR_Mas_Sup_Color_Status == "A")
                , "CR_Mas_Sup_Color_Code", "CR_Mas_Sup_Color_Ar_Name");


            ViewBag.CR_Cas_Sup_Car_In_Secondary_Color_Code = new SelectList(db.CR_Mas_Sup_Color.Where(x => x.CR_Mas_Sup_Color_Status == "A")
                , "CR_Mas_Sup_Color_Code", "CR_Mas_Sup_Color_Ar_Name");

            //ViewBag.CR_Cas_Sup_Car_Category_Code= new SelectList(db.CR_Mas_Sup_Category_Car.Where(x => x.CR_Mas_Sup_Category_Car_Status == "A")
            //    , "CR_Cas_Sup_Car_Category_Code", "CR_Mas_Sup_Registration_Car_Ar_Name");

            ViewBag.joined = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            ViewBag.Left = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            ViewBag.CarteEndDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);

            CR_Cas_Sup_Car_Information cR_Cas_Sup_Car_Information = new CR_Cas_Sup_Car_Information();

            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status = "A";
            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_No_Current_Meter = 0;
            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Documentation_Status = false;
            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Maintenance_Status = false;


            //dynamic mv = new ExpandoObject();
            //mv.carinfo = cR_Cas_Sup_Car_Information;
            //mv.features = getFeatures();

            return View(cR_Cas_Sup_Car_Information);
        }

        private void SaveTracing(string LessorCode, string SerialNo, string ProcedureType, string reasons)
        {
            CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var sector = "1";
            var ProcedureCode = "51";
            var autoInc = GetLastRecord(ProcedureCode, "1");
            //var LessorCode = Session["LessorCode"].ToString();
            Ad.CR_Cas_Administrative_Procedures_No = y + "-" + sector + "-" + ProcedureCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
            Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
            Ad.CR_Cas_Administrative_Procedures_Year = y;
            Ad.CR_Cas_Administrative_Procedures_Sector = sector;
            Ad.CR_Cas_Administrative_Procedures_Code = ProcedureCode;
            Ad.CR_Cas_Administrative_Int_Procedures_Code = int.Parse(ProcedureCode);
            Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
            Ad.CR_Cas_Administrative_Procedures_Targeted_Action = SerialNo;
            Ad.CR_Cas_Administrative_Procedures_User_Insert = Session["UserLogin"].ToString();
            Ad.CR_Cas_Administrative_Procedures_Type = ProcedureType;
            Ad.CR_Cas_Administrative_Procedures_Action = true;
            Ad.CR_Cas_Administrative_Procedures_Doc_Date = null;
            Ad.CR_Cas_Administrative_Procedures_Doc_Start_Date = null;
            Ad.CR_Cas_Administrative_Procedures_Doc_End_Date = null;
            Ad.CR_Cas_Administrative_Procedures_Doc_No = "";
            Ad.CR_Cas_Administrative_Procedures_Reasons = reasons;
            db.CR_Cas_Administrative_Procedures.Add(Ad);
        }

        // POST: CasCarInformation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Sup_Car_Serail_No,CR_Cas_Sup_Car_Lessor_Code,CR_Cas_Sup_Car_Owner_Branch_Code," +
            "CR_Cas_Sup_Car_Location_Branch_Code,CR_Cas_Sup_Car_Owner_Code,CR_Cas_Sup_Car_Beneficiary_Code,CR_Cas_Sup_Car_Customs_No," +
            "CR_Cas_Sup_Car_Structure_No,CR_Cas_Sup_Car_Registration_Code,CR_Cas_Sup_Car_Brand_Code,CR_Cas_Sup_Car_Model_Code," +
            "CR_Cas_Sup_Car_Year,CR_Cas_Sup_Car_Category_Code,CR_Cas_Sup_Car_Out_Main_Color_Code,CR_Cas_Sup_Car_Out_Secondary_Color_Code," +
            "CR_Cas_Sup_Car_In_Main_Color_Code,CR_Cas_Sup_Car_In_Secondary_Color_Code,CR_Cas_Sup_Car_Plate_Ar_No,CR_Cas_Sup_Car_Plate_En_No," +
            "CR_Cas_Sup_Car_No_Current_Meter,CR_Cas_Sup_Car_Traffic_License_Img,CR_Cas_Sup_Car_Joined_Fleet_Date,CR_Cas_Sup_Car_Left_Fleet_Date," +
            "CR_Cas_Sup_Car_Last_Pictures,CR_Cas_Sup_Car_Documentation_Status,CR_Cas_Sup_Car_Maintenance_Status,CR_Cas_Sup_Car_Status,CR_Cas_Sup_Car_Price_Status" +
            "CR_Cas_Sup_Car_Reasons")] CR_Cas_Sup_Car_Information cR_Cas_Sup_Car_Information, DateTime CR_Cas_Sup_Branch_Documentation_Start_Date,
            bool CR_Cas_Sup_Car_Documentation_Status, bool CR_Cas_Sup_Car_Maintenance_Status, string BrandCode, string CategoryCode,
            HttpPostedFileBase imgLicence, string CarImage, FormCollection collection, string listyear)
        {
            using (DbContextTransaction dbTran = db.Database.BeginTransaction())
            {
                try
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

                    var carinfo = db.CR_Cas_Sup_Car_Information.Any(x => x.CR_Cas_Sup_Car_Serail_No == cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No);
                    var registrationAr = db.CR_Cas_Sup_Car_Information.Any(x => x.CR_Cas_Sup_Car_Plate_Ar_No == cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Plate_Ar_No &&
                    x.CR_Cas_Sup_Car_Registration_Code == cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Registration_Code);

                    var registrationEn = db.CR_Cas_Sup_Car_Information.Any(x => x.CR_Cas_Sup_Car_Plate_En_No == cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Plate_En_No &&
                    x.CR_Cas_Sup_Car_Registration_Code == cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Registration_Code);

                    var GovNo = db.CR_Mas_Com_Lessor.FirstOrDefault(g => g.CR_Mas_Com_Lessor_Code == LessorCode);
                    var BrandName = "";
                    var Model = "";
                    var color = "";
                    if (ModelState.IsValid)
                    {
                        if (!carinfo && !registrationAr && !registrationEn && cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No != null &&
                            listyear != null && cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Category_Code != null && CR_Cas_Sup_Branch_Documentation_Start_Date <= DateTime.Now)
                        {

                            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Category_Code = CategoryCode;
                            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Brand_Code = BrandCode;
                            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status = "A";
                            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_No_Current_Meter = 0;
                            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Lessor_Code = LessorCode;
                            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Owner_Branch_Code = "100";
                            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Location_Branch_Code = "100";
                            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Left_Fleet_Date = null;
                            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Year = int.Parse(listyear);
                            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Last_Pictures = CarImage;
                            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Maintenance_Status = true;
                            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Documentation_Status = false;
                            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Branch_Status = "A";
                            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Owner_Status = "A";
                            if (GovNo != null)
                            {
                                cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Owner_Code = GovNo.CR_Mas_Com_Lessor_Government_No;
                                cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Beneficiary_Code = GovNo.CR_Mas_Com_Lessor_Government_No;
                            }


                            var brand = db.CR_Mas_Sup_Brand.FirstOrDefault(b => b.CR_Mas_Sup_Brand_Code == BrandCode);
                            if (brand != null)
                            {
                                BrandName = brand.CR_Mas_Sup_Brand_Ar_Name;
                            }

                            var mod = db.CR_Mas_Sup_Model.FirstOrDefault(c => c.CR_Mas_Sup_Model_Code == cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Model_Code);
                            if (mod != null)
                            {
                                Model = mod.CR_Mas_Sup_Model_Ar_Name;
                            }

                            var MainColor = db.CR_Mas_Sup_Color.FirstOrDefault(c => c.CR_Mas_Sup_Color_Code == cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Out_Main_Color_Code);
                            if (MainColor != null)
                            {
                                color = MainColor.CR_Mas_Sup_Color_Ar_Name;
                            }

                            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Collect_Ar_Name = BrandName + " - " + Model + " - " + color + " - " + cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Year + " - " +
                                cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Plate_Ar_No;



                            var price = db.CR_Cas_Car_Price_Basic.FirstOrDefault(p=>p.CR_Cas_Car_Price_Basic_Model_Code==cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Model_Code && 
                            p.CR_Cas_Car_Price_Basic_Car_Year== cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Year && (p.CR_Cas_Car_Price_Basic_Status=="A" || p.CR_Cas_Car_Price_Basic_Status=="X")
                            && p.CR_Cas_Car_Price_Basic_Lessor_Code==cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Lessor_Code);
                            if (price != null)
                            {
                                cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Price_Status = "1";
                            }

                            db.CR_Cas_Sup_Car_Information.Add(cR_Cas_Sup_Car_Information);



                            ///////////////////////////////Tracing//////////////////////////////////////
                            SaveTracing(LessorCode, cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No, "I",cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Reasons);
                            ////////////////////////////////////Documentation////////////////////////////////////////
                            var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.Where(
                                  x => x.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == LessorCode
                                  && (x.CR_Cas_Sup_Procedures_Type == "3"
                                  || x.CR_Cas_Sup_Procedures_Type == "4")
                                 && x.CR_Mas_Sup_Procedures.CR_Mas_Sup_Procedures_Status != "D");

                            //int n = mech.Count();
                            List<CR_Cas_Sup_Car_Doc_Mainten> ldoc = new List<CR_Cas_Sup_Car_Doc_Mainten>();
                            CR_Cas_Sup_Car_Doc_Mainten doc;

                            foreach (var m in mech)
                            {
                                doc = new CR_Cas_Sup_Car_Doc_Mainten();
                                doc.CR_Cas_Sup_Car_Doc_Mainten_Serail_No = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No;
                                doc.CR_Cas_Sup_Car_Doc_Mainten_Code = m.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code;
                                doc.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code = LessorCode;
                                
                                doc.CR_Cas_Sup_Car_Doc_Mainten_Date = null;
                                doc.CR_Cas_Sup_Car_Doc_Mainten_Start_Date = null;
                                doc.CR_Cas_Sup_Car_Doc_Mainten_End_Date = null;
                                doc.CR_Cas_Sup_Car_Doc_Mainten_Activation = true;
                                doc.CR_Cas_Sup_Car_Doc_Mainten_Default_KM = m.CR_Cas_Sup_Follow_Up_Mechanism_Default_KM;
                                doc.CR_Cas_Sup_Car_Doc_Mainten_Owner_Status = "A";
                                doc.CR_Cas_Sup_Car_Doc_Mainten_Branch_Status = "A";
        

                                doc.CR_Cas_Sup_Car_Doc_Mainten_Type = m.CR_Cas_Sup_Procedures_Type;

                                if (m.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code == "26" || m.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code == "29")
                                {
                                    doc.CR_Cas_Sup_Car_Doc_Mainten_No = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No;
                                    doc.CR_Cas_Sup_Car_Doc_Mainten_Status = "A";
                                    doc.CR_Cas_Sup_Car_Doc_Mainten_Date = DateTime.Now;
                                    doc.CR_Cas_Sup_Car_Doc_Mainten_Start_Date = CR_Cas_Sup_Branch_Documentation_Start_Date;

                                    DateTime startdate = (DateTime)CR_Cas_Sup_Branch_Documentation_Start_Date;
                                    var daynbr = (int)m.CR_Cas_Sup_Follow_Up_Mechanism_After_Expire;
                                    var d = startdate.AddDays(daynbr);
                                    doc.CR_Cas_Sup_Car_Doc_Mainten_End_Date = d;


                                    DateTime currentdate = (DateTime)d;
                                    var nbrday = Convert.ToDouble(m.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire);
                                    var aboutExpire = currentdate.AddDays(-nbrday);
                                    doc.CR_Cas_Sup_Car_Doc_Mainten_About_To_Expire = aboutExpire;
                                }
                                else
                                {
                                    doc.CR_Cas_Sup_Car_Doc_Mainten_Status = "N";
                                    doc.CR_Cas_Sup_Car_Doc_Mainten_Date = null;
                                    doc.CR_Cas_Sup_Car_Doc_Mainten_Start_Date = null;
                                    doc.CR_Cas_Sup_Car_Doc_Mainten_End_Date = null;
                                    doc.CR_Cas_Sup_Car_Doc_Mainten_About_To_Expire = null;
                                }


                                if (m.CR_Cas_Sup_Procedures_Type == "4")
                                {
                                    doc.CR_Cas_Sup_Car_Doc_Mainten_Status = "A";
                                    doc.CR_Cas_Sup_Car_Doc_Mainten_Date = DateTime.Now;
                                    doc.CR_Cas_Sup_Car_Doc_Mainten_Start_Date = CR_Cas_Sup_Branch_Documentation_Start_Date;
                                    doc.CR_Cas_Sup_Car_Doc_Mainten_Limit_KM = doc.CR_Cas_Sup_Car_Doc_Mainten_Default_KM - m.CR_Cas_Sup_Follow_Up_Mechanism_Befor_KM;
                                    doc.CR_Cas_Sup_Car_Doc_Mainten_End_KM = m.CR_Cas_Sup_Follow_Up_Mechanism_Default_KM + doc.CR_Cas_Sup_Car_Doc_Mainten_Default_KM;
                                    DateTime startdate = (DateTime)CR_Cas_Sup_Branch_Documentation_Start_Date;
                                    var daynbr = (int)m.CR_Cas_Sup_Follow_Up_Mechanism_After_Expire;
                                    var d = startdate.AddDays(daynbr);
                                    doc.CR_Cas_Sup_Car_Doc_Mainten_End_Date = d;

                                    DateTime EndDate = (DateTime)d;
                                    var nbrday = Convert.ToDouble(m.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire);
                                    var aboutExpire = EndDate.AddDays(-nbrday);
                                    doc.CR_Cas_Sup_Car_Doc_Mainten_About_To_Expire = aboutExpire;
                                }



                                ldoc.Add(doc);
                            }
                            ldoc.ForEach(d => db.CR_Cas_Sup_Car_Doc_Mainten.Add(d));
                            ////////////////////////////////////////////////////////////////////////////////////////
                            ////////////////////////////Save images/////////////////////////////////////////

                            string LicencePath = "";

                            if (imgLicence != null)
                            {

                                string folderimages = Server.MapPath(string.Format("~/{0}/", "/images"));
                                string folderBnan = Server.MapPath(string.Format("~/{0}/", "/images/Bnan"));
                                /*tring FolderCompanyCode = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + AccountController.LessorCode));*/
                                string FolderCars = Server.MapPath(string.Format("~/{0}/", "/images/Bnan/" + "CarRegistration" + "/" + cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No));
                                //String FolderCarSerial = Server.MapPath(string.Format("~/{0}/", "/images/" + AccountController.LessorCode
                                //    + "/" + cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No));

                                if (!Directory.Exists(folderimages))
                                {
                                    Directory.CreateDirectory(folderimages);
                                }
                                if (!Directory.Exists(folderBnan))
                                {
                                    Directory.CreateDirectory(folderBnan);
                                }
                                //if (!Directory.Exists(FolderCompanyCode))
                                //{
                                //    Directory.CreateDirectory(FolderCompanyCode);
                                //}
                                if (!Directory.Exists(FolderCars))
                                {
                                    Directory.CreateDirectory(FolderCars);
                                    Directory.CreateDirectory(FolderCars);
                                    if (imgLicence.FileName.Length > 0)
                                    {
                                        LicencePath = "~/images/Bnan/" + "CarRegistration" + "/" + cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No +
                                            "/" + Path.GetFileName(imgLicence.FileName);
                                        imgLicence.SaveAs(HttpContext.Server.MapPath(LicencePath));
                                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Traffic_License_Img = LicencePath;
                                    }
                                }

                            }
                            ////////////////////////////////////////////////////////////////////////////////
                            ///

                            List<CR_Cas_Sup_Features> LFeatures = new List<CR_Cas_Sup_Features>();

                            foreach (string item in collection.AllKeys)
                            {
                                if (item.StartsWith("chk_"))
                                {
                                    CR_Cas_Sup_Features CasF = new CR_Cas_Sup_Features();
                                    CasF.CR_Cas_Sup_Features_Serial_No = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No;
                                    CasF.CR_Cas_Sup_Features_Model_Code = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Model_Code;
                                    CasF.CR_Cas_Sup_Features_Lessor_Code = LessorCode;
                                    CasF.CR_Cas_Sup_Features_Model_Year = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Year.ToString();
                                    CasF.CR_Cas_Sup_Features_Code = item.Replace("chk_", "");
                                    LFeatures.Add(CasF);
                                }
                            }
                            LFeatures.ForEach(f => db.CR_Cas_Sup_Features.Add(f));

                            ////////List<string> listCode = new List<string>();
                            //////foreach (string item in collection.AllKeys)
                            //////{
                            //////    //if (item.StartsWith("chk_"))
                            //////    //{
                            //////    //    listCode.Add(item.Replace("chk_", ""));
                            //////    //}
                            //////}
                            /////////////////////////////////////////////////////////////////////////////////

                            db.SaveChanges();
                            dbTran.Commit();
                            TempData["TempModel"] = "Saved";
                            return RedirectToAction("Create");
                        }
                        else
                        {
                            if (carinfo)
                            {
                                ViewBag.CarInfo = "الرقم التسلسلي متكرر";
                            }
                            if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No == null)
                            {
                                ViewBag.CarInfo = "الرجاء إدخال بيانات الحقل";
                            }
                            //if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No == null)
                            //{
                            //    ViewBag.CarInfo = "الرجاء التأكد من بيانات الحقل";

                            //}

                            if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No != null)
                            {
                                if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No.Length < 10)
                                {
                                    ViewBag.CarInfo = "الرقم مكون من 10 أرقام";
                                }
                            }


                            if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Model_Code == null)
                            {
                                ViewBag.Carmodel = "الرجاء إدخال بيانات الحقل";
                            }

                            if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Registration_Code == null)
                            {
                                ViewBag.Registration = "الرجاء إدخال بيانات الحقل";
                            }

                            if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Out_Main_Color_Code == null)
                            {
                                ViewBag.color = "الرجاء إدخال بيانات الحقل";
                            }

                            if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Plate_Ar_No == null)
                            {
                                ViewBag.RegAr = "الرجاء إدخال بيانات الحقل";
                            }

                            if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Plate_En_No == null)
                            {
                                ViewBag.RegEn = "الرجاء إدخال بيانات الحقل";
                            }

                            if (registrationAr)
                            {
                                ViewBag.RegAr = "هذه اللوحة متكررة";
                            }
                            if (registrationEn)
                            {
                                ViewBag.RegEn = "هذه اللوحة متكررة";
                            }

                            if (listyear == null || cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Category_Code == null)
                            {
                                ViewBag.CarCat = "عفوا هذه السيارة غير مصنفة";
                            }

                            if (CR_Cas_Sup_Branch_Documentation_Start_Date > DateTime.Now)
                            {
                                ViewBag.StartDate = "الرجاء التأكد من تاريخ الاصدار";
                            }

                            ViewBag.Picturepath = CarImage;



                            ViewBag.joined = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                            ViewBag.Left = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                            ViewBag.CarteEndDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
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
                                              }, "ID", "Name", y);
                            ViewData["listyear"] = ListYear;
                            TempData["TempModel"] = "Error";
                        }

                    }
                }
                catch (DbEntityValidationException ex)
                {
                    dbTran.Rollback();
                    throw ex;
                }
            }
            var LesorCode = Session["LessorCode"].ToString();

            ViewBag.CR_Cas_Sup_Car_Brand_Code = new SelectList(db.CR_Mas_Sup_Brand.Where(x => x.CR_Mas_Sup_Brand_Status == "A")
                , "CR_Mas_Sup_Brand_Code", "CR_Mas_Sup_Brand_Ar_Name");


            ViewBag.CR_Cas_Sup_Car_Out_Main_Color_Code = new SelectList(db.CR_Mas_Sup_Color.Where(x => x.CR_Mas_Sup_Color_Status == "A")
                , "CR_Mas_Sup_Color_Code", "CR_Mas_Sup_Color_Ar_Name");

            ViewBag.CR_Cas_Sup_Car_Out_Secondary_Color_Code = new SelectList(db.CR_Mas_Sup_Color.Where(x => x.CR_Mas_Sup_Color_Status == "A")
                , "CR_Mas_Sup_Color_Code", "CR_Mas_Sup_Color_Ar_Name");


            ViewBag.CR_Cas_Sup_Car_Model_Code = new SelectList(db.CR_Mas_Sup_Model.Where(x => x.CR_Mas_Sup_Model_Status == "A")
                , "CR_Mas_Sup_Model_Code", "CR_Mas_Sup_Model_Ar_Name");


            ViewBag.CR_Cas_Sup_Car_Registration_Code = new SelectList(db.CR_Mas_Sup_Registration_Car.Where(x => x.CR_Mas_Sup_Registration_Car_Status == "A")
                , "CR_Mas_Sup_Registration_Car_Code", "CR_Mas_Sup_Registration_Car_Ar_Name");


            ViewBag.CR_Cas_Sup_Car_In_Main_Color_Code = new SelectList(db.CR_Mas_Sup_Color.Where(x => x.CR_Mas_Sup_Color_Status == "A")
                , "CR_Mas_Sup_Color_Code", "CR_Mas_Sup_Color_Ar_Name");


            ViewBag.CR_Cas_Sup_Car_In_Secondary_Color_Code = new SelectList(db.CR_Mas_Sup_Color.Where(x => x.CR_Mas_Sup_Color_Status == "A")
                , "CR_Mas_Sup_Color_Code", "CR_Mas_Sup_Color_Ar_Name");
            return View(cR_Cas_Sup_Car_Information);
        }






        // GET: CasCarInformation/Edit/5
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
            CR_Cas_Sup_Car_Information cR_Cas_Sup_Car_Information = db.CR_Cas_Sup_Car_Information.Find(id);
            if (cR_Cas_Sup_Car_Information == null)
            {
                return HttpNotFound();
            }

            //var LessorCode = Session["LessorCode"].ToString();
            ViewBag.serialNumber = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No;

            var cat = db.CR_Mas_Sup_Category_Car.FirstOrDefault(c => c.CR_Mas_Sup_Category_Car_Year == cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Year
            && c.CR_Mas_Sup_Category_Model_Code == cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Model_Code);
            ViewBag.Picturepath = cat.CR_Mas_Sup_Category_Car_Picture;

            var brand = db.CR_Mas_Sup_Brand.FirstOrDefault(b => b.CR_Mas_Sup_Brand_Code == cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Brand_Code);
            ViewBag.BrandCode = brand.CR_Mas_Sup_Brand_Code;
            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Brand_Code = brand.CR_Mas_Sup_Brand_Ar_Name;

           
            if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "R")
            {
                TempData["CarStatus"] = "Rented";
            }
            if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "O")
            {
                TempData["CarStatus"] = "Buy";
            }
            //var carinfo = db.CR_Cas_Sup_Car_Information.Where(c => c.CR_Cas_Sup_Car_Lessor_Code == LessorCode && c.CR_Cas_Sup_Car_Status != "D"
            //   && c.CR_Cas_Sup_Car_Owner_Branch_Code == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code).Count();
            //if (carinfo > 0)
            //{
            //    TempData["CarsNo"] = "True";
            //    //return RedirectToAction("Index");
            //}




            var category = db.CR_Mas_Sup_Category.FirstOrDefault(c => c.CR_Mas_Sup_Category_Code == cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Category_Code);
            ViewBag.CategoryName = category.CR_Mas_Sup_Category_Ar_Name;
            ViewBag.CategoryCode = category.CR_Mas_Sup_Category_Code;

            ViewBag.CR_Cas_Sup_Car_Brand_Code = new SelectList(db.CR_Mas_Sup_Brand.Where(x => x.CR_Mas_Sup_Brand_Status == "A")
                , "CR_Mas_Sup_Brand_Code", "CR_Mas_Sup_Brand_Ar_Name");


            ViewBag.CR_Cas_Sup_Car_Out_Main_Color_Code = new SelectList(db.CR_Mas_Sup_Color.Where(x => x.CR_Mas_Sup_Color_Status == "A")
                , "CR_Mas_Sup_Color_Code", "CR_Mas_Sup_Color_Ar_Name", cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Out_Main_Color_Code);

            ViewBag.CR_Cas_Sup_Car_Out_Secondary_Color_Code = new SelectList(db.CR_Mas_Sup_Color.Where(x => x.CR_Mas_Sup_Color_Status == "A")
                , "CR_Mas_Sup_Color_Code", "CR_Mas_Sup_Color_Ar_Name", cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Out_Secondary_Color_Code);


            ViewBag.CR_Cas_Sup_Car_Model_Code = new SelectList(db.CR_Mas_Sup_Model.Where(x => x.CR_Mas_Sup_Model_Status == "A")
                , "CR_Mas_Sup_Model_Code", "CR_Mas_Sup_Model_Ar_Name", cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Model_Code);


            ViewBag.CR_Cas_Sup_Car_Registration_Code = new SelectList(db.CR_Mas_Sup_Registration_Car.Where(x => x.CR_Mas_Sup_Registration_Car_Status == "A")
                , "CR_Mas_Sup_Registration_Car_Code", "CR_Mas_Sup_Registration_Car_Ar_Name", cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Registration_Code);


            ViewBag.CR_Cas_Sup_Car_In_Main_Color_Code = new SelectList(db.CR_Mas_Sup_Color.Where(x => x.CR_Mas_Sup_Color_Status == "A")
                , "CR_Mas_Sup_Color_Code", "CR_Mas_Sup_Color_Ar_Name", cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_In_Main_Color_Code);


            ViewBag.CR_Cas_Sup_Car_In_Secondary_Color_Code = new SelectList(db.CR_Mas_Sup_Color.Where(x => x.CR_Mas_Sup_Color_Status == "A")
                , "CR_Mas_Sup_Color_Code", "CR_Mas_Sup_Color_Ar_Name", cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_In_Secondary_Color_Code);

            if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Traffic_License_Img == "" || cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Traffic_License_Img == null)
            {
                ViewBag.LicenceCar = " ";
            }
            else
            {
                ViewBag.LicenceCar = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Traffic_License_Img;
            }


            ViewBag.ModelCode = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Model_Code;

            ViewBag.RegistrationCode = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Registration_Code;
            ViewBag.OutMainColor = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Out_Main_Color_Code;

            if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "A" ||
                    cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "Activated" ||
                    cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "1" ||
                    cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
                ViewBag.Sell = "عرض للبيع";
                ViewBag.Maintenance = "صيانة";
            }
            if ((cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "D" ||
                cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "Deleted" ||
                cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
                ViewBag.Sell = "عرض للبيع";
                ViewBag.Maintenance = "صيانة";
                ViewData["ReadOnly"] = "true";
            }
            if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "H" ||
                cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "Hold" ||
                cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
                ViewBag.Sell = "عرض للبيع";
                ViewBag.Maintenance = "صيانة";
                ViewData["ReadOnly"] = "true";
            }
            if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
                ViewBag.Sell = "عرض للبيع";
                ViewBag.Maintenance = "صيانة";
            }
            if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "O")
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
                ViewBag.Sell = "إلغاء العرض";
                ViewBag.Maintenance = "صيانة";
            }
            if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "M")
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
                ViewBag.Sell = "عرض للبيع";
                ViewBag.Maintenance = "إسترجاع";
            }
            if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "R")
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
                ViewBag.Sell = "عرض للبيع";
                ViewBag.Maintenance = "إسترجاع";
            }
            ViewBag.delete = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status;


            DateTime date = DateTime.Now;
            var shortDate = date.ToString("yyyy-MM-dd");
            ViewBag.CarteEndDate = shortDate;


            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Plate_Ar_No = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Plate_Ar_No.Trim();
            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Plate_En_No = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Plate_En_No.Trim();
            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Reasons = "";
            return View(cR_Cas_Sup_Car_Information);
        }

        // POST: CasCarInformation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Sup_Car_Serail_No,CR_Cas_Sup_Car_Lessor_Code,CR_Cas_Sup_Car_Owner_Branch_Code," +
            "CR_Cas_Sup_Car_Location_Branch_Code,CR_Cas_Sup_Car_Owner_Code,CR_Cas_Sup_Car_Beneficiary_Code,CR_Cas_Sup_Car_Customs_No," +
            "CR_Cas_Sup_Car_Structure_No,CR_Cas_Sup_Car_Registration_Code,CR_Cas_Sup_Car_Brand_Code,CR_Cas_Sup_Car_Model_Code," +
            "CR_Cas_Sup_Car_Year,CR_Cas_Sup_Car_Category_Code,CR_Cas_Sup_Car_Out_Main_Color_Code,CR_Cas_Sup_Car_Out_Secondary_Color_Code," +
            "CR_Cas_Sup_Car_In_Main_Color_Code,CR_Cas_Sup_Car_In_Secondary_Color_Code,CR_Cas_Sup_Car_Plate_Ar_No,CR_Cas_Sup_Car_Plate_En_No," +
            "CR_Cas_Sup_Car_No_Current_Meter,CR_Cas_Sup_Car_Traffic_License_Img,CR_Cas_Sup_Car_Joined_Fleet_Date,CR_Cas_Sup_Car_Left_Fleet_Date," +
            "CR_Cas_Sup_Car_Last_Pictures,CR_Cas_Sup_Car_Documentation_Status,CR_Cas_Sup_Car_Maintenance_Status,CR_Cas_Sup_Car_Status,CR_Cas_Sup_Car_Reasons," +
            "CR_Cas_Sup_Car_Collect_Ar_Name,CR_Cas_Sup_Car_Collect_Ar_Short_Name,CR_Cas_Sup_Car_Collect_En_Name,CR_Cas_Sup_Car_Collect_En_Short_Name," +
            "CR_Cas_Sup_Car_Collect_Fr_Name,CR_Cas_Sup_Car_Collect_Fr_Short_Name")]
            CR_Cas_Sup_Car_Information cR_Cas_Sup_Car_Information, DateTime CR_Cas_Sup_Branch_Documentation_Start_Date,
            HttpPostedFileBase imgLicence, FormCollection collection, string ImgL, string save, string delete, string hold, string Sell, string Maintenance,
            string BrandCode, string ModelCode, string RegistrationCode, string OutMainColor)
        {
            var LessorCode = Session["LessorCode"].ToString();
            var bracnhcode1 = db.CR_Cas_Sup_Car_Information.AsNoTracking().FirstOrDefault(l => l.CR_Cas_Sup_Car_Customs_No == cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Customs_No).CR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code;

            if (!string.IsNullOrEmpty(save))
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {

                        var registrationAr = db.CR_Cas_Sup_Car_Information.Any(x => x.CR_Cas_Sup_Car_Plate_Ar_No == cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Plate_Ar_No &&
                        x.CR_Cas_Sup_Car_Registration_Code == cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Registration_Code && x.CR_Cas_Sup_Car_Serail_No != cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No);

                        var registrationEn = db.CR_Cas_Sup_Car_Information.Any(x => x.CR_Cas_Sup_Car_Plate_En_No == cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Plate_En_No &&
                        x.CR_Cas_Sup_Car_Registration_Code == cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Registration_Code && x.CR_Cas_Sup_Car_Serail_No != cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No);

                        var serial = db.CR_Cas_Sup_Car_Information.Any(x => x.CR_Cas_Sup_Car_Serail_No == cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No);

                        if (ModelState.IsValid)
                        {
                            if (!registrationAr && !registrationEn && cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No != null &&
                                cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Year != null && cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Category_Code != null)
                            {

                                cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Category_Code = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Category_Code;
                                cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Brand_Code = BrandCode;
                                cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Model_Code = ModelCode;
                                cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Registration_Code = RegistrationCode;
                                cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Out_Main_Color_Code = OutMainColor;
                                cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status = "A";

                                cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Lessor_Code = LessorCode;
                                cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Owner_Branch_Code = "100";
                                cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Location_Branch_Code = "100";
                                cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Left_Fleet_Date = null;
                                ///////////////////////////////Tracing//////////////////////////////////////
                                SaveTracing(LessorCode, cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No, "U",cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Reasons);

                                ////////////////////////////Save images/////////////////////////////////////////

                                string LicencePath = "";

                                if (imgLicence != null)
                                {

                                    string folderimages = Server.MapPath(string.Format("~/{0}/", "/images"));
                                    string folderBnan = Server.MapPath(string.Format("~/{0}/", "/images/Bnan"));
                                    /*tring FolderCompanyCode = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + AccountController.LessorCode));*/
                                    string FolderCars = Server.MapPath(string.Format("~/{0}/", "/images/Bnan/" + "CarRegistration" + "/" + cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No));
                                    //String FolderCarSerial = Server.MapPath(string.Format("~/{0}/", "/images/" + AccountController.LessorCode
                                    //    + "/" + cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No));

                                    if (!Directory.Exists(folderimages))
                                    {
                                        Directory.CreateDirectory(folderimages);
                                    }
                                    if (!Directory.Exists(folderBnan))
                                    {
                                        Directory.CreateDirectory(folderBnan);
                                    }
                                    //if (!Directory.Exists(FolderCompanyCode))
                                    //{
                                    //    Directory.CreateDirectory(FolderCompanyCode);
                                    //}
                                    if (!Directory.Exists(FolderCars))
                                    {
                                        Directory.CreateDirectory(FolderCars);
                                        
                                        if (imgLicence.FileName.Length > 0)
                                        {
                                            LicencePath = "~/images/Bnan/" + "CarRegistration" + "/" + cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No +
                                                "/" + Path.GetFileName(imgLicence.FileName);
                                            imgLicence.SaveAs(HttpContext.Server.MapPath(LicencePath));
                                            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Traffic_License_Img = LicencePath;
                                        }
                                    }

                                }
                                else
                                {
                                    cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Traffic_License_Img = ImgL;
                                }
                                ////////////////////////////////////////////////////////////////////////////////
                                ///


                                DeleteFeatures(cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No);

                                List<CR_Cas_Sup_Features> LFeatures = new List<CR_Cas_Sup_Features>();

                                foreach (string item in collection.AllKeys)
                                {
                                    if (item.StartsWith("chk_"))
                                    {
                                        CR_Cas_Sup_Features CasF = new CR_Cas_Sup_Features();
                                        CasF.CR_Cas_Sup_Features_Serial_No = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No;
                                        CasF.CR_Cas_Sup_Features_Model_Code = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Model_Code;
                                        CasF.CR_Cas_Sup_Features_Lessor_Code = LessorCode;
                                        CasF.CR_Cas_Sup_Features_Model_Year = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Year.ToString();
                                        CasF.CR_Cas_Sup_Features_Code = item.Replace("chk_", "");
                                        LFeatures.Add(CasF);
                                    }
                                }
                                LFeatures.ForEach(f => db.CR_Cas_Sup_Features.Add(f));


                                db.Entry(cR_Cas_Sup_Car_Information).State = EntityState.Modified;
                                db.SaveChanges();
                                dbTran.Commit();
                                TempData["TempModel"] = "Saved";
                                return RedirectToAction("index");
                            }
                            else
                            {

                                if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No == null)
                                {
                                    ViewBag.CarInfo = "الرجاء إدخال بيانات الحقل";
                                }
                                if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No != null && cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No.Length < 10)
                                {
                                    ViewBag.CarInfo = "الرجاء التأكد من بيانات الحقل";
                                }
                                if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Model_Code == null)
                                {
                                    ViewBag.Carmodel = "الرجاء إدخال بيانات الحقل";
                                }

                                if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Registration_Code == null)
                                {
                                    ViewBag.Registration = "الرجاء إدخال بيانات الحقل";
                                }

                                if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Out_Main_Color_Code == null)
                                {
                                    ViewBag.color = "الرجاء إدخال بيانات الحقل";
                                }

                                if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Plate_Ar_No == null)
                                {
                                    ViewBag.RegAr = "الرجاء إدخال بيانات الحقل";
                                }

                                if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Plate_En_No == null)
                                {
                                    ViewBag.RegEn = "الرجاء إدخال بيانات الحقل";
                                }

                                if (registrationAr)
                                {
                                    ViewBag.RegAr = "هذه اللوحة متكررة";
                                }
                                if (registrationEn)
                                {
                                    ViewBag.RegEn = "هذه اللوحة متكررة";
                                }

                                if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Year == null || cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Category_Code == null)
                                {
                                    ViewBag.Year = "الرجاء إدخال بيانات الحقل";
                                }
                                ViewBag.joined = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                                ViewBag.Left = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                                ViewBag.CarteEndDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                                ViewBag.CR_Cas_Sup_Car_Brand_Code = new SelectList(db.CR_Mas_Sup_Brand.Where(x => x.CR_Mas_Sup_Brand_Status == "A")
                                    , "CR_Mas_Sup_Brand_Code", "CR_Mas_Sup_Brand_Ar_Name");


                                ViewBag.CR_Cas_Sup_Car_Out_Main_Color_Code = new SelectList(db.CR_Mas_Sup_Color.Where(x => x.CR_Mas_Sup_Color_Status == "A")
                                    , "CR_Mas_Sup_Color_Code", "CR_Mas_Sup_Color_Ar_Name");

                                ViewBag.CR_Cas_Sup_Car_Out_Secondary_Color_Code = new SelectList(db.CR_Mas_Sup_Color.Where(x => x.CR_Mas_Sup_Color_Status == "A")
                                    , "CR_Mas_Sup_Color_Code", "CR_Mas_Sup_Color_Ar_Name");


                                ViewBag.CR_Cas_Sup_Car_Model_Code = new SelectList(db.CR_Mas_Sup_Model.Where(x => x.CR_Mas_Sup_Model_Status == "A")
                                    , "CR_Mas_Sup_Model_Code", "CR_Mas_Sup_Model_Ar_Name");


                                ViewBag.CR_Cas_Sup_Car_Registration_Code = new SelectList(db.CR_Mas_Sup_Registration_Car.Where(x => x.CR_Mas_Sup_Registration_Car_Status == "A")
                                    , "CR_Mas_Sup_Registration_Car_Code", "CR_Mas_Sup_Registration_Car_Ar_Name");


                                ViewBag.CR_Cas_Sup_Car_In_Main_Color_Code = new SelectList(db.CR_Mas_Sup_Color.Where(x => x.CR_Mas_Sup_Color_Status == "A")
                                    , "CR_Mas_Sup_Color_Code", "CR_Mas_Sup_Color_Ar_Name");


                                ViewBag.CR_Cas_Sup_Car_In_Secondary_Color_Code = new SelectList(db.CR_Mas_Sup_Color.Where(x => x.CR_Mas_Sup_Color_Status == "A")
                                    , "CR_Mas_Sup_Color_Code", "CR_Mas_Sup_Color_Ar_Name");
                                return View(cR_Cas_Sup_Car_Information);
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

            if (delete == "Delete" || delete == "حذف")
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        ////////////////////////////////////////////Save Tracing/////////////////////////////////////
                        SaveTracing(LessorCode, cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No, "D",cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Reasons);
                        ////////////////////////////////////////////////////////////////////////////////////////////
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Category_Code = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Category_Code;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Brand_Code = BrandCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Model_Code = ModelCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Registration_Code = RegistrationCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Out_Main_Color_Code = OutMainColor;
                        //cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_No_Current_Meter = 0;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Lessor_Code = LessorCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Owner_Branch_Code = bracnhcode1;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Location_Branch_Code = bracnhcode1;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Traffic_License_Img = ImgL;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status = "D";
                        db.Entry(cR_Cas_Sup_Car_Information).State = EntityState.Modified;
                        ///////////////////////////////////Delete Car Docs//////////////////////////////////////////
                        var docs = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d=>d.CR_Cas_Sup_Car_Doc_Mainten_Serail_No==cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No);
                        foreach (var d in docs)
                        {
                            d.CR_Cas_Sup_Car_Doc_Mainten_Status = "D";
                            db.Entry(d).State = EntityState.Modified;
                        }
                        
                        ///////////////////////////////////////////////////////////////////////////////////////////////
                        
                        db.SaveChanges();
                        TempData["TempModel"] = "Deleted";
                        dbTran.Commit();
                        return RedirectToAction("Index");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }

            }
            if (delete == "Activate" || delete == "إسترجاع")
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {

                        ////////////////////////////////////////////Save Tracing///////////////////////////////////////
                        SaveTracing(LessorCode, cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No, "A",cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Reasons);
                        ///////////////////////////////////////////////////////////////////////////////////////////////
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Category_Code = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Category_Code;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Brand_Code = BrandCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Model_Code = ModelCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Registration_Code = RegistrationCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Out_Main_Color_Code = OutMainColor;
                        //cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_No_Current_Meter = 0;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Lessor_Code = LessorCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Owner_Branch_Code = bracnhcode1;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Location_Branch_Code = bracnhcode1;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Traffic_License_Img = ImgL;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status = "A";
                        db.Entry(cR_Cas_Sup_Car_Information).State = EntityState.Modified;
                        ///////////////////////////////////Activate Car Docs//////////////////////////////////////////
                        var docs = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Serail_No == cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No);
                        foreach (var d in docs)
                        {
                            d.CR_Cas_Sup_Car_Doc_Mainten_Status = "A";
                            db.Entry(d).State = EntityState.Modified;
                        }



                        ///////////////////////////////////////////////////////////////////////////////////////////////
                        db.SaveChanges();
                        TempData["TempModel"] = "Activated";
                        dbTran.Commit();
                        return RedirectToAction("Index");

                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }

            }
            if (Maintenance == "Activate" || Maintenance == "إسترجاع")
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {

                        ////////////////////////////////////////////Save Tracing///////////////////////////////////////
                        SaveTracing(LessorCode, cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No, "A",cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Reasons);
                        ///////////////////////////////////////////////////////////////////////////////////////////////
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Category_Code = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Category_Code;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Brand_Code = BrandCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Model_Code = ModelCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Registration_Code = RegistrationCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Out_Main_Color_Code = OutMainColor;
                        //cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_No_Current_Meter = 0;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Lessor_Code = LessorCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Owner_Branch_Code = bracnhcode1;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Location_Branch_Code = bracnhcode1;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Traffic_License_Img = ImgL;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status = "A";
                        db.Entry(cR_Cas_Sup_Car_Information).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["TempModel"] = "Activated";
                        dbTran.Commit();
                        return RedirectToAction("Index");

                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }

            }
            if (hold == "إيقاف" || hold == "hold")
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        ////////////////////////////////////////////Save Tracing///////////////////////////////////////
                        SaveTracing(LessorCode, cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No, "H",cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Reasons);
                        ///////////////////////////////////////////////////////////////////////////////////////////////
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Category_Code = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Category_Code;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Brand_Code = BrandCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Model_Code = ModelCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Registration_Code = RegistrationCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Out_Main_Color_Code = OutMainColor;
                        //cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_No_Current_Meter = 0;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Lessor_Code = LessorCode;



                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Owner_Branch_Code = bracnhcode1;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Location_Branch_Code = bracnhcode1;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Traffic_License_Img = ImgL;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status = "H";
                        db.Entry(cR_Cas_Sup_Car_Information).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["TempModel"] = "Holded";
                        dbTran.Commit();
                        return RedirectToAction("Index");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }

            }
            if (Sell == "عرض للبيع" || Sell == "Sell")
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        ////////////////////////////////////////////Save Tracing///////////////////////////////////////
                        SaveTracing(LessorCode, cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No, "O",cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Reasons);
                        ///////////////////////////////////////////////////////////////////////////////////////////////
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Category_Code = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Category_Code;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Brand_Code = BrandCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Model_Code = ModelCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Registration_Code = RegistrationCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Out_Main_Color_Code = OutMainColor;
                        //cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_No_Current_Meter = 0;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Lessor_Code = LessorCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Owner_Branch_Code = bracnhcode1;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Location_Branch_Code = bracnhcode1;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Traffic_License_Img = ImgL;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status = "O";
                        db.Entry(cR_Cas_Sup_Car_Information).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["TempModel"] = "Selled";
                        dbTran.Commit();
                        return RedirectToAction("Index");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }

            }
            if (Maintenance == "صيانة" || Maintenance == "Maintenance")
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        ////////////////////////////////////////////Save Tracing///////////////////////////////////////
                        SaveTracing(LessorCode, cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No, "M",cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Reasons);
                        ///////////////////////////////////////////////////////////////////////////////////////////////
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Category_Code = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Category_Code;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Brand_Code = BrandCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Model_Code = ModelCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Registration_Code = RegistrationCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Out_Main_Color_Code = OutMainColor;
                        //cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_No_Current_Meter = 0;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Lessor_Code = LessorCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Owner_Branch_Code = bracnhcode1;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Location_Branch_Code = bracnhcode1;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Traffic_License_Img = ImgL;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status = "M";
                        db.Entry(cR_Cas_Sup_Car_Information).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["TempModel"] = "Maintained";
                        dbTran.Commit();
                        return RedirectToAction("Index");


                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }

            }
            if (Sell == "إلغاء العرض" || Sell == "Activate")
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {

                        ////////////////////////////////////////////Save Tracing///////////////////////////////////////
                        SaveTracing(LessorCode, cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No, "A",cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Reasons);
                        ///////////////////////////////////////////////////////////////////////////////////////////////
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Category_Code = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Category_Code;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Brand_Code = BrandCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Model_Code = ModelCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Registration_Code = RegistrationCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Out_Main_Color_Code = OutMainColor;
                        //cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_No_Current_Meter = 0;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Lessor_Code = LessorCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Owner_Branch_Code = bracnhcode1;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Location_Branch_Code = bracnhcode1;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Traffic_License_Img = ImgL;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status = "A";
                        db.Entry(cR_Cas_Sup_Car_Information).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["TempModel"] = "Activated";
                        dbTran.Commit();
                        return RedirectToAction("Index");

                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }

            }
            if (hold == "تنشيط" || hold == "Activate")
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        ////////////////////////////////////////////Save Tracing///////////////////////////////////////
                        SaveTracing(LessorCode, cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Serail_No, "A",cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Reasons);
                        ///////////////////////////////////////////////////////////////////////////////////////////////
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Category_Code = cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Category_Code;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Brand_Code = BrandCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Model_Code = ModelCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Registration_Code = RegistrationCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Out_Main_Color_Code = OutMainColor;
                        //cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_No_Current_Meter = 0;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Lessor_Code = LessorCode;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Owner_Branch_Code = bracnhcode1;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Location_Branch_Code = bracnhcode1;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Traffic_License_Img = ImgL;
                        cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status = "A";
                        db.Entry(cR_Cas_Sup_Car_Information).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["TempModel"] = "Activated";
                        dbTran.Commit();
                        return RedirectToAction("Index");


                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }

            }
            if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "A" ||
            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "Activated" ||
            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "1" ||
            cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }
            if ((cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "D" ||
                 cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "Deleted" ||
                 cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
            }
            if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "H" ||
                cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "Hold" ||
                cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
            }
            if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == "O")
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }
            if (cR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }

            //ViewBag.CR_Cas_Sup_Car_Beneficiary_Code = new SelectList(db.CR_Cas_Sup_Beneficiary.Where
            //        (x => x.CR_Cas_Sup_Beneficiary_Lessor == LessorCode && x.CR_Cas_Sup_Beneficiary_Status == "A"),
            //        "CR_Cas_Sup_Beneficiary_Code", "CR_Cas_Sup_Beneficiary_Ar_Long_Name");


            //ViewBag.CR_Cas_Sup_Car_Owner_Branch_Code = new SelectList(db.CR_Cas_Sup_Branch.Where
            //    (x => x.CR_Cas_Sup_Lessor_Code == LessorCode && x.CR_Cas_Sup_Branch_Status == "A"),
            //    "CR_Cas_Sup_Branch_Code", "CR_Cas_Sup_Branch_Ar_Name");


            //ViewBag.CR_Cas_Sup_Car_Owner_Code = new SelectList(db.CR_Cas_Sup_Owners.Where
            //    (x => x.CR_Cas_Sup_Owners_Lessor_Code == LessorCode && x.CR_Cas_Sup_Owners_Status == "A"),
            //    "CR_Cas_Sup_Owners_Code", "CR_Cas_Sup_Owners_Ar_Long_Name");


            //ViewBag.CR_Cas_Sup_Car_Lessor_Code = new SelectList(db.CR_Mas_Com_Lessor.Where
            //    (x => x.CR_Mas_Com_Lessor_Code == LessorCode && x.CR_Mas_Com_Lessor_Status == "A"),
            //    "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Ar_Short_Name");


            ViewBag.CR_Cas_Sup_Car_Brand_Code = new SelectList(db.CR_Mas_Sup_Brand.Where(x => x.CR_Mas_Sup_Brand_Status == "A")
                , "CR_Mas_Sup_Brand_Code", "CR_Mas_Sup_Brand_Ar_Name");


            ViewBag.CR_Cas_Sup_Car_Out_Main_Color_Code = new SelectList(db.CR_Mas_Sup_Color.Where(x => x.CR_Mas_Sup_Color_Status == "A")
                , "CR_Mas_Sup_Color_Code", "CR_Mas_Sup_Color_Ar_Name");

            ViewBag.CR_Cas_Sup_Car_Out_Secondary_Color_Code = new SelectList(db.CR_Mas_Sup_Color.Where(x => x.CR_Mas_Sup_Color_Status == "A")
                , "CR_Mas_Sup_Color_Code", "CR_Mas_Sup_Color_Ar_Name");


            ViewBag.CR_Cas_Sup_Car_Model_Code = new SelectList(db.CR_Mas_Sup_Model.Where(x => x.CR_Mas_Sup_Model_Status == "A")
                , "CR_Mas_Sup_Model_Code", "CR_Mas_Sup_Model_Ar_Name");


            ViewBag.CR_Cas_Sup_Car_Registration_Code = new SelectList(db.CR_Mas_Sup_Registration_Car.Where(x => x.CR_Mas_Sup_Registration_Car_Status == "A")
                , "CR_Mas_Sup_Registration_Car_Code", "CR_Mas_Sup_Registration_Car_Ar_Name");


            ViewBag.CR_Cas_Sup_Car_In_Main_Color_Code = new SelectList(db.CR_Mas_Sup_Color.Where(x => x.CR_Mas_Sup_Color_Status == "A")
                , "CR_Mas_Sup_Color_Code", "CR_Mas_Sup_Color_Ar_Name");


            ViewBag.CR_Cas_Sup_Car_In_Secondary_Color_Code = new SelectList(db.CR_Mas_Sup_Color.Where(x => x.CR_Mas_Sup_Color_Status == "A")
                , "CR_Mas_Sup_Color_Code", "CR_Mas_Sup_Color_Ar_Name");
            return View(cR_Cas_Sup_Car_Information);

        }

        public void DeleteFeatures(string id)
        {
            var q = db.CR_Cas_Sup_Features.Where(c => c.CR_Cas_Sup_Features_Serial_No == id);

            foreach (var item in q)
            {
                var feature = db.CR_Cas_Sup_Features.Where(x => x.CR_Cas_Sup_Features_Serial_No == id && x.CR_Cas_Sup_Features_Code == item.CR_Cas_Sup_Features_Code).First();
                db.CR_Cas_Sup_Features.Remove(feature);
            }
            //db.SaveChanges();
        }




        // GET: CasCarInformation/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_Car_Information cR_Cas_Sup_Car_Information = db.CR_Cas_Sup_Car_Information.Find(id);
            if (cR_Cas_Sup_Car_Information == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Sup_Car_Information);
        }

        // POST: CasCarInformation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Cas_Sup_Car_Information cR_Cas_Sup_Car_Information = db.CR_Cas_Sup_Car_Information.Find(id);
            db.CR_Cas_Sup_Car_Information.Remove(cR_Cas_Sup_Car_Information);
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
