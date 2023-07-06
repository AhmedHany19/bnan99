using RentCar.Models;
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
using System.Web.UI;

namespace RentCar.Controllers.CAS
{
    public class CasBranchController : Controller
    {
        public static string regioncode = "";
        private RentCarDBEntities db = new RentCarDBEntities();

        [HttpGet]
        [ActionName("Index")]
        // GET: CasBranch
        public ActionResult Index_Get()
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
            //var ListStart = new SelectList(new[] {
            //                                  new {ID="1",Name="السبت"},
            //                                  new{ID="2",Name="الأحد"},
            //                                  new{ID="3",Name="الإثنين"},
            //                                  new{ID="4",Name="الثلاثاء"},
            //                                  new{ID="5",Name="الإربعاء"},
            //                                  new{ID="6",Name="الخميس"},
            //                                  new{ID="7",Name="الجمعة"},
            //                                  }, "ID", "Name", 1);
            //ViewData["listStart"] = ListStart;
            //var ListEnd = new SelectList(new[] {
            //                                  new {ID="1",Name="السبت"},
            //                                  new{ID="2",Name="الأحد"},
            //                                  new{ID="3",Name="الإثنين"},
            //                                  new{ID="4",Name="الثلاثاء"},
            //                                  new{ID="5",Name="الإربعاء"},
            //                                  new{ID="6",Name="الخميس"},
            //                                  new{ID="7",Name="الجمعة"},
            //                                  }, "ID", "Name", 1);
            //ViewData["listEnd"] = ListEnd;
            //var cR_Cas_Sup_Branch = db.CR_Cas_Sup_Branch.Where(x => x.CR_Cas_Sup_Lessor_Code == LessorCode).Include(c => c.CR_Mas_Com_Lessor);
            //List<BranchCarsModel> l = new List<BranchCarsModel>();
            //BranchCarsModel b = new BranchCarsModel();


            //foreach (var branch in cR_Cas_Sup_Branch)
            //{
            //    b = new BranchCarsModel();
            //    var nbr = db.CR_Cas_Sup_Car_Information.Where(x => x.CR_Cas_Sup_Car_Lessor_Code == LessorCode && x.CR_Cas_Sup_Car_Owner_Branch_Code == branch.CR_Cas_Sup_Branch_Code).Count();

            //    var adr = db.CR_Mas_Address.FirstOrDefault(a => a.CR_Mas_Address_Id_Code == branch.CR_Cas_Sup_Branch_Government_No);
            //    if (adr != null)
            //    {

            //        var city = db.CR_Mas_Sup_City.FirstOrDefault(c => c.CR_Mas_Sup_City_Code == adr.CR_Mas_Address_City);
            //        if (city != null)
            //        {
            //            var reg = db.CR_Mas_Sup_Regions.FirstOrDefault(r => r.CR_Mas_Sup_Regions_Code == city.CR_Mas_Sup_City_Regions_Code);

            //            b.CarsNumber = nbr.ToString();
            //            b.cityName = city.CR_Mas_Sup_City_Ar_Name;
            //            b.CR_Cas_Sup_Branch_Ar_Name = branch.CR_Cas_Sup_Branch_Ar_Name;
            //            b.CR_Cas_Sup_Branch_Ar_Short_Name = branch.CR_Cas_Sup_Branch_Ar_Short_Name;
            //            b.CR_Cas_Sup_Branch_Code = branch.CR_Cas_Sup_Branch_Code;
            //            b.CR_Cas_Sup_Branch_Commercial_Registration_No = branch.CR_Cas_Sup_Branch_Commercial_Registration_No;
            //            b.CR_Cas_Sup_Branch_Documentation = branch.CR_Cas_Sup_Branch_Documentation;
            //            b.CR_Cas_Sup_Branch_Duration_One_End_Time = branch.CR_Cas_Sup_Branch_Duration_One_End_Time.GetValueOrDefault();
            //            b.CR_Cas_Sup_Branch_Duration_One_Start_Time = branch.CR_Cas_Sup_Branch_Duration_One_Start_Time.GetValueOrDefault(); ;
            //            b.CR_Cas_Sup_Branch_Duration_Three_End_Time = branch.CR_Cas_Sup_Branch_Duration_Three_End_Time.GetValueOrDefault(); ;
            //            b.CR_Cas_Sup_Branch_Duration_Three_Start_Time = branch.CR_Cas_Sup_Branch_Duration_Three_Start_Time.GetValueOrDefault(); ;
            //            b.CR_Cas_Sup_Branch_Duration_Tow_End_Time = branch.CR_Cas_Sup_Branch_Duration_Tow_End_Time.GetValueOrDefault(); ;
            //            b.CR_Cas_Sup_Branch_Duration_Tow_Start_Time = branch.CR_Cas_Sup_Branch_Duration_Tow_Start_Time.GetValueOrDefault(); ;
            //            b.CR_Cas_Sup_Branch_End_Day = branch.CR_Cas_Sup_Branch_End_Day.GetValueOrDefault(); ;
            //            b.CR_Cas_Sup_Branch_En_Name = branch.CR_Cas_Sup_Branch_En_Name;
            //            b.CR_Cas_Sup_Branch_En_Short_Name = branch.CR_Cas_Sup_Branch_En_Short_Name;
            //            b.CR_Cas_Sup_Branch_Fr_Name = branch.CR_Cas_Sup_Branch_Fr_Name;
            //            b.CR_Cas_Sup_Branch_Fr_Short_Name = branch.CR_Cas_Sup_Branch_Fr_Short_Name;
            //            b.CR_Cas_Sup_Branch_Government_No = branch.CR_Cas_Sup_Branch_Government_No;
            //            b.CR_Cas_Sup_Branch_LogoMap = branch.CR_Cas_Sup_Branch_LogoMap;
            //            b.CR_Cas_Sup_Branch_Mobile = branch.CR_Cas_Sup_Branch_Mobile;
            //            b.CR_Cas_Sup_Branch_Reasons = branch.CR_Cas_Sup_Branch_Reasons;
            //            b.CR_Cas_Sup_Branch_Signature_Ar_Director_Name = branch.CR_Cas_Sup_Branch_Signature_Ar_Director_Name;
            //            b.CR_Cas_Sup_Branch_Signature_Director = branch.CR_Cas_Sup_Branch_Signature_Director;
            //            b.CR_Cas_Sup_Branch_Signature_En_Director_Name = branch.CR_Cas_Sup_Branch_Signature_En_Director_Name;
            //            b.CR_Cas_Sup_Branch_Signature_Fr_Director_Name = branch.CR_Cas_Sup_Branch_Signature_Fr_Director_Name;
            //            b.CR_Cas_Sup_Branch_Stamp = branch.CR_Cas_Sup_Branch_Stamp;
            //            b.CR_Cas_Sup_Branch_Start_Day = branch.CR_Cas_Sup_Branch_Start_Day.GetValueOrDefault(); ;
            //            b.CR_Cas_Sup_Branch_Status = branch.CR_Cas_Sup_Branch_Status;
            //            b.CR_Cas_Sup_Branch_Tax_No = branch.CR_Cas_Sup_Branch_Tax_No;
            //            b.CR_Cas_Sup_Branch_Tel = branch.CR_Cas_Sup_Branch_Tel;
            //            b.CR_Cas_Sup_Car_Information = branch.CR_Cas_Sup_Car_Information;
            //            b.CR_Cas_Sup_Lessor_Code = branch.CR_Cas_Sup_Lessor_Code;
            //            b.CR_Mas_Com_Lessor = branch.CR_Mas_Com_Lessor;
            //            b.Region = reg.CR_Mas_Sup_Regions_Ar_Name;
            //        }
            //    }

            //    l.Add(b);
            //}


            return View();
        }

        public JsonResult CheckBranch()
        {
            string LessorCode = Session["LessorCode"].ToString();

            var branchs = db.CR_Cas_Sup_Branch.Where(c => c.CR_Cas_Sup_Lessor_Code == LessorCode).Count();
            return Json(branchs, JsonRequestBehavior.AllowGet);
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
            var LessorCode = Session["LessorCode"].ToString();
            if (!string.IsNullOrEmpty(excelCall))
            {
                var branchTable = new System.Data.DataTable("teste");


                branchTable.Columns.Add("الحالة", typeof(string));
                branchTable.Columns.Add("عدد السيارات", typeof(string));
                branchTable.Columns.Add("رقم الجوال", typeof(string));
                branchTable.Columns.Add("رقم الهاتف", typeof(string));
                branchTable.Columns.Add("المدير", typeof(string));
                branchTable.Columns.Add("المدينة", typeof(string));
                branchTable.Columns.Add("الفرع", typeof(string));
                var cRBranch = db.CR_Cas_Sup_Branch.Where(x => x.CR_Cas_Sup_Lessor_Code == LessorCode).Include(c => c.CR_Mas_Com_Lessor);

                List<BranchCarsModel> l = new List<BranchCarsModel>();
                BranchCarsModel b = new BranchCarsModel();


                foreach (var branch in cRBranch)
                {
                    b = new BranchCarsModel();
                    var nbr = db.CR_Cas_Sup_Car_Information.Where(x => x.CR_Cas_Sup_Car_Lessor_Code == LessorCode && x.CR_Cas_Sup_Car_Owner_Branch_Code == branch.CR_Cas_Sup_Branch_Code).Count();
                    var adr = db.CR_Mas_Address.FirstOrDefault(a => a.CR_Mas_Address_Id_Code == branch.CR_Cas_Sup_Branch_Government_No);

                    var city = db.CR_Mas_Sup_City.FirstOrDefault(c => c.CR_Mas_Sup_City_Code == adr.CR_Mas_Address_City);

                    b.CarsNumber = nbr.ToString();
                    b.cityName = city.CR_Mas_Sup_City_Ar_Name;
                    b.CR_Cas_Sup_Branch_Ar_Name = branch.CR_Cas_Sup_Branch_Ar_Name;
                    b.CR_Cas_Sup_Branch_Ar_Short_Name = branch.CR_Cas_Sup_Branch_Ar_Short_Name;
                    b.CR_Cas_Sup_Branch_Code = branch.CR_Cas_Sup_Branch_Code;
                    b.CR_Cas_Sup_Branch_Commercial_Registration_No = branch.CR_Cas_Sup_Branch_Commercial_Registration_No;
                    b.CR_Cas_Sup_Branch_Documentation = branch.CR_Cas_Sup_Branch_Documentation;
                    b.CR_Cas_Sup_Branch_Duration_One_End_Time = branch.CR_Cas_Sup_Branch_Duration_One_End_Time.GetValueOrDefault(); ;
                    b.CR_Cas_Sup_Branch_Duration_One_Start_Time = branch.CR_Cas_Sup_Branch_Duration_One_Start_Time.GetValueOrDefault(); ;
                    b.CR_Cas_Sup_Branch_Duration_Three_End_Time = branch.CR_Cas_Sup_Branch_Duration_Three_End_Time.GetValueOrDefault(); ;
                    b.CR_Cas_Sup_Branch_Duration_Three_Start_Time = branch.CR_Cas_Sup_Branch_Duration_Three_Start_Time.GetValueOrDefault(); ;
                    b.CR_Cas_Sup_Branch_Duration_Tow_End_Time = branch.CR_Cas_Sup_Branch_Duration_Tow_End_Time.GetValueOrDefault(); ;
                    b.CR_Cas_Sup_Branch_Duration_Tow_Start_Time = branch.CR_Cas_Sup_Branch_Duration_Tow_Start_Time.GetValueOrDefault(); ;
                    b.CR_Cas_Sup_Branch_End_Day = branch.CR_Cas_Sup_Branch_End_Day.GetValueOrDefault(); ;
                    b.CR_Cas_Sup_Branch_En_Name = branch.CR_Cas_Sup_Branch_En_Name;
                    b.CR_Cas_Sup_Branch_En_Short_Name = branch.CR_Cas_Sup_Branch_En_Short_Name;
                    b.CR_Cas_Sup_Branch_Fr_Name = branch.CR_Cas_Sup_Branch_Fr_Name;
                    b.CR_Cas_Sup_Branch_Fr_Short_Name = branch.CR_Cas_Sup_Branch_Fr_Short_Name;
                    b.CR_Cas_Sup_Branch_Government_No = branch.CR_Cas_Sup_Branch_Government_No;
                    b.CR_Cas_Sup_Branch_LogoMap = branch.CR_Cas_Sup_Branch_LogoMap;
                    b.CR_Cas_Sup_Branch_Mobile = branch.CR_Cas_Sup_Branch_Mobile;
                    b.CR_Cas_Sup_Branch_Reasons = branch.CR_Cas_Sup_Branch_Reasons;
                    b.CR_Cas_Sup_Branch_Signature_Ar_Director_Name = branch.CR_Cas_Sup_Branch_Signature_Ar_Director_Name;
                    b.CR_Cas_Sup_Branch_Signature_Director = branch.CR_Cas_Sup_Branch_Signature_Director;
                    b.CR_Cas_Sup_Branch_Signature_En_Director_Name = branch.CR_Cas_Sup_Branch_Signature_En_Director_Name;
                    b.CR_Cas_Sup_Branch_Signature_Fr_Director_Name = branch.CR_Cas_Sup_Branch_Signature_Fr_Director_Name;
                    b.CR_Cas_Sup_Branch_Stamp = branch.CR_Cas_Sup_Branch_Stamp;
                    b.CR_Cas_Sup_Branch_Start_Day = branch.CR_Cas_Sup_Branch_Start_Day.GetValueOrDefault(); ;
                    b.CR_Cas_Sup_Branch_Status = branch.CR_Cas_Sup_Branch_Status;
                    b.CR_Cas_Sup_Branch_Tax_No = branch.CR_Cas_Sup_Branch_Tax_No;
                    b.CR_Cas_Sup_Branch_Tel = branch.CR_Cas_Sup_Branch_Tel;
                    b.CR_Cas_Sup_Car_Information = branch.CR_Cas_Sup_Car_Information;
                    b.CR_Cas_Sup_Lessor_Code = branch.CR_Cas_Sup_Lessor_Code;
                    b.CR_Mas_Com_Lessor = branch.CR_Mas_Com_Lessor;
                    l.Add(b);
                }
                if (l != null)
                {
                    foreach (var i in l)
                    {
                        branchTable.Rows.Add(i.CR_Cas_Sup_Branch_Status, i.CarsNumber, i.CR_Cas_Sup_Branch_Mobile, i.CR_Cas_Sup_Branch_Tel, i.CR_Cas_Sup_Branch_Signature_Ar_Director_Name,
                                            i.cityName, i.CR_Cas_Sup_Branch_Ar_Name);
                    }
                }
                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = branchTable;
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=ListOfBranchs.xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            //var LessorCode = Session["LessorCode"].ToString();
            var ListStart = new SelectList(new[] {
                                              new {ID="1",Name="السبت"},
                                              new{ID="2",Name="الأحد"},
                                              new{ID="3",Name="الإثنين"},
                                              new{ID="4",Name="الثلاثاء"},
                                              new{ID="5",Name="الإربعاء"},
                                              new{ID="6",Name="الخميس"},
                                              new{ID="7",Name="الجمعة"},
                                              }, "ID", "Name", 1);
            ViewData["listStart"] = ListStart;
            var ListEnd = new SelectList(new[] {
                                              new {ID="1",Name="السبت"},
                                              new{ID="2",Name="الأحد"},
                                              new{ID="3",Name="الإثنين"},
                                              new{ID="4",Name="الثلاثاء"},
                                              new{ID="5",Name="الإربعاء"},
                                              new{ID="6",Name="الخميس"},
                                              new{ID="7",Name="الجمعة"},
                                              }, "ID", "Name", 1);
            ViewData["listEnd"] = ListEnd;
            var cR_Cas_Sup_Branch = db.CR_Cas_Sup_Branch.Where(x => x.CR_Cas_Sup_Lessor_Code == LessorCode).Include(c => c.CR_Mas_Com_Lessor);
            return View(cR_Cas_Sup_Branch.ToList());
        }

        // GET: CasBranch/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_Branch cR_Cas_Sup_Branch = db.CR_Cas_Sup_Branch.Find(id);
            if (cR_Cas_Sup_Branch == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Sup_Branch);
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

            IQueryable<CR_Cas_Sup_Branch> cR_Cas_Sup_Branch = null;
            if (type == "A")
            {
                cR_Cas_Sup_Branch = db.CR_Cas_Sup_Branch.Where(x => x.CR_Cas_Sup_Lessor_Code == LessorCode && x.CR_Cas_Sup_Branch_Status == "A").Include(c => c.CR_Mas_Com_Lessor);
            }
            else if (type == "All")
            {
                cR_Cas_Sup_Branch = db.CR_Cas_Sup_Branch.Where(x => x.CR_Cas_Sup_Lessor_Code == LessorCode).Include(c => c.CR_Mas_Com_Lessor);
            }
            else if (type == "H")
            {
                cR_Cas_Sup_Branch = db.CR_Cas_Sup_Branch.Where(x => x.CR_Cas_Sup_Lessor_Code == LessorCode && x.CR_Cas_Sup_Branch_Status == "H").Include(c => c.CR_Mas_Com_Lessor);
            }
            else if (type == "D")
            {
                cR_Cas_Sup_Branch = db.CR_Cas_Sup_Branch.Where(x => x.CR_Cas_Sup_Lessor_Code == LessorCode && x.CR_Cas_Sup_Branch_Status == "D").Include(c => c.CR_Mas_Com_Lessor);
            }
            else
            {
                cR_Cas_Sup_Branch = db.CR_Cas_Sup_Branch.Where(x => x.CR_Cas_Sup_Lessor_Code == LessorCode && x.CR_Cas_Sup_Branch_Status == "A").Include(c => c.CR_Mas_Com_Lessor);
            }

            List<BranchCarsModel> l = new List<BranchCarsModel>();
            BranchCarsModel b = new BranchCarsModel();


            foreach (var branch in cR_Cas_Sup_Branch)
            {
                b = new BranchCarsModel();
                var nbr = db.CR_Cas_Sup_Car_Information.Where(x => x.CR_Cas_Sup_Car_Lessor_Code == LessorCode && x.CR_Cas_Sup_Car_Owner_Branch_Code == branch.CR_Cas_Sup_Branch_Code).Count();

                var adr = db.CR_Mas_Address.FirstOrDefault(a => a.CR_Mas_Address_Id_Code == branch.CR_Cas_Sup_Branch_Government_No);
                if (adr != null)
                {

                    var city = db.CR_Mas_Sup_City.FirstOrDefault(c => c.CR_Mas_Sup_City_Code == adr.CR_Mas_Address_City);
                    if (city != null)
                    {
                        var reg = db.CR_Mas_Sup_Regions.FirstOrDefault(r => r.CR_Mas_Sup_Regions_Code == city.CR_Mas_Sup_City_Regions_Code);

                        b.CarsNumber = nbr.ToString();
                        b.cityName = city.CR_Mas_Sup_City_Ar_Name;
                        b.CR_Cas_Sup_Branch_Ar_Name = branch.CR_Cas_Sup_Branch_Ar_Name;
                        b.CR_Cas_Sup_Branch_Ar_Short_Name = branch.CR_Cas_Sup_Branch_Ar_Short_Name;
                        b.CR_Cas_Sup_Branch_Code = branch.CR_Cas_Sup_Branch_Code;
                        b.CR_Cas_Sup_Branch_Commercial_Registration_No = branch.CR_Cas_Sup_Branch_Commercial_Registration_No;
                        b.CR_Cas_Sup_Branch_Documentation = branch.CR_Cas_Sup_Branch_Documentation;
                        b.CR_Cas_Sup_Branch_Duration_One_End_Time = branch.CR_Cas_Sup_Branch_Duration_One_End_Time.GetValueOrDefault();
                        b.CR_Cas_Sup_Branch_Duration_One_Start_Time = branch.CR_Cas_Sup_Branch_Duration_One_Start_Time.GetValueOrDefault(); ;
                        b.CR_Cas_Sup_Branch_Duration_Three_End_Time = branch.CR_Cas_Sup_Branch_Duration_Three_End_Time.GetValueOrDefault(); ;
                        b.CR_Cas_Sup_Branch_Duration_Three_Start_Time = branch.CR_Cas_Sup_Branch_Duration_Three_Start_Time.GetValueOrDefault(); ;
                        b.CR_Cas_Sup_Branch_Duration_Tow_End_Time = branch.CR_Cas_Sup_Branch_Duration_Tow_End_Time.GetValueOrDefault(); ;
                        b.CR_Cas_Sup_Branch_Duration_Tow_Start_Time = branch.CR_Cas_Sup_Branch_Duration_Tow_Start_Time.GetValueOrDefault(); ;
                        b.CR_Cas_Sup_Branch_End_Day = branch.CR_Cas_Sup_Branch_End_Day.GetValueOrDefault(); ;
                        b.CR_Cas_Sup_Branch_En_Name = branch.CR_Cas_Sup_Branch_En_Name;
                        b.CR_Cas_Sup_Branch_En_Short_Name = branch.CR_Cas_Sup_Branch_En_Short_Name;
                        b.CR_Cas_Sup_Branch_Fr_Name = branch.CR_Cas_Sup_Branch_Fr_Name;
                        b.CR_Cas_Sup_Branch_Fr_Short_Name = branch.CR_Cas_Sup_Branch_Fr_Short_Name;
                        b.CR_Cas_Sup_Branch_Government_No = branch.CR_Cas_Sup_Branch_Government_No;
                        b.CR_Cas_Sup_Branch_LogoMap = branch.CR_Cas_Sup_Branch_LogoMap;
                        b.CR_Cas_Sup_Branch_Mobile = branch.CR_Cas_Sup_Branch_Mobile;
                        b.CR_Cas_Sup_Branch_Reasons = branch.CR_Cas_Sup_Branch_Reasons;
                        b.CR_Cas_Sup_Branch_Signature_Ar_Director_Name = branch.CR_Cas_Sup_Branch_Signature_Ar_Director_Name;
                        b.CR_Cas_Sup_Branch_Signature_Director = branch.CR_Cas_Sup_Branch_Signature_Director;
                        b.CR_Cas_Sup_Branch_Signature_En_Director_Name = branch.CR_Cas_Sup_Branch_Signature_En_Director_Name;
                        b.CR_Cas_Sup_Branch_Signature_Fr_Director_Name = branch.CR_Cas_Sup_Branch_Signature_Fr_Director_Name;
                        b.CR_Cas_Sup_Branch_Stamp = branch.CR_Cas_Sup_Branch_Stamp;
                        b.CR_Cas_Sup_Branch_Start_Day = branch.CR_Cas_Sup_Branch_Start_Day.GetValueOrDefault(); ;
                        b.CR_Cas_Sup_Branch_Status = branch.CR_Cas_Sup_Branch_Status;
                        b.CR_Cas_Sup_Branch_Tax_No = branch.CR_Cas_Sup_Branch_Tax_No;
                        b.CR_Cas_Sup_Branch_Tel = branch.CR_Cas_Sup_Branch_Tel;
                        b.CR_Cas_Sup_Car_Information = branch.CR_Cas_Sup_Car_Information;
                        b.CR_Cas_Sup_Lessor_Code = branch.CR_Cas_Sup_Lessor_Code;
                        b.CR_Mas_Com_Lessor = branch.CR_Mas_Com_Lessor;
                        b.Region = reg.CR_Mas_Sup_Regions_Ar_Name;
                    }
                }

                l.Add(b);
            }


            return PartialView(l);
        }

        public JsonResult GetCity(string CityCode)
        {
            regioncode = "";
            var city = new CR_Mas_Sup_City();
            db.Configuration.ProxyCreationEnabled = false;
            if (CityCode != "" && CityCode != null)
            {
                city = db.CR_Mas_Sup_City.FirstOrDefault(c => c.CR_Mas_Sup_City_Code == CityCode && c.CR_Mas_Sup_City_Status != "D");
            }

            var reg = new CR_Mas_Sup_Regions();
            if (city.CR_Mas_Sup_City_Regions_Code != null && city.CR_Mas_Sup_City_Regions_Code != "")
            {
                regioncode = city.CR_Mas_Sup_City_Regions_Code.ToString();
                reg = db.CR_Mas_Sup_Regions.FirstOrDefault(c => c.CR_Mas_Sup_Regions_Code == regioncode);
                regioncode = reg.CR_Mas_Sup_Regions_Code;
            }

            return Json(reg.CR_Mas_Sup_Regions_Ar_Name, JsonRequestBehavior.AllowGet);
        }


        public CR_Cas_Sup_Branch GetBranchLastCode()
        {
            var LessorCode = Session["LessorCode"].ToString();
            var Lrecord = db.CR_Cas_Sup_Branch.Where(x => x.CR_Cas_Sup_Lessor_Code == LessorCode).Max(l => l.CR_Cas_Sup_Branch_Code);
            CR_Cas_Sup_Branch b = new CR_Cas_Sup_Branch();
            if (Lrecord != null)
            {
                int val = int.Parse(Lrecord) + 1;
                b.CR_Cas_Sup_Branch_Code = val.ToString();
            }
            else
            {
                b.CR_Cas_Sup_Branch_Code = "100";
            }
            return b;
        }

        // GET: CasBranch/Create
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


            var ListStart = new SelectList(new[] {
                                              new {ID="1",Name="السبت"},
                                              new{ID="2",Name="الأحد"},
                                              new{ID="3",Name="الإثنين"},
                                              new{ID="4",Name="الثلاثاء"},
                                              new{ID="5",Name="الإربعاء"},
                                              new{ID="6",Name="الخميس"},
                                              new{ID="7",Name="الجمعة"},
                                              }, "ID", "Name", 1);
            ViewData["listStart"] = ListStart;
            var ListEnd = new SelectList(new[] {
                                              new {ID="1",Name="السبت"},
                                              new{ID="2",Name="الأحد"},
                                              new{ID="3",Name="الإثنين"},
                                              new{ID="4",Name="الثلاثاء"},
                                              new{ID="5",Name="الإربعاء"},
                                              new{ID="6",Name="الخميس"},
                                              new{ID="7",Name="الجمعة"},
                                              }, "ID", "Name", 5);
            ViewData["listEnd"] = ListEnd;
            CR_Cas_Sup_Branch branch = new CR_Cas_Sup_Branch();
            //branch = GetBranchLastCode();
            branch.CR_Cas_Sup_Branch_Status = "A";

            ViewBag.CR_MAS_SUP_City_Code = new SelectList(db.CR_Mas_Sup_City.Where(x => x.CR_Mas_Sup_Regions.CR_Mas_Sup_Regions_Status != "D"), "CR_Mas_Sup_City_Code", "CR_Mas_Sup_City_Ar_Name");
            return View(branch);
        }

        public CR_Cas_Administrative_Procedures GetLastRecord(string Lessorcode, string ProcedureCode)
        {
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");


            var Lrecord = db.CR_Cas_Administrative_Procedures.Where(x => x.CR_Cas_Administrative_Procedures_Lessor == Lessorcode && x.CR_Cas_Administrative_Procedures_Code == ProcedureCode
            && x.CR_Cas_Administrative_Procedures_Year == y)
                .Max(lr => lr.CR_Cas_Administrative_Procedures_No.Substring(lr.CR_Cas_Administrative_Procedures_No.Length - 7, 7));



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

        public void SaveTracing(string LessorCode, string Sector, string ProceduresCode, string ProcedureType, string UserLogin, string BranchCode, string Reasons)
        {

            CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var sector = "1";
            var autoInc = GetLastRecord(LessorCode, ProceduresCode);

            Ad.CR_Cas_Administrative_Procedures_No = y + "-" + sector + "-" + ProceduresCode + "-" + LessorCode + "-" + autoInc.CR_Cas_Administrative_Procedures_No;
            Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
            Ad.CR_Cas_Administrative_Procedures_Year = y;
            Ad.CR_Cas_Administrative_Procedures_Sector = Sector;
            Ad.CR_Cas_Administrative_Procedures_Code = ProceduresCode;
            Ad.CR_Cas_Administrative_Int_Procedures_Code = int.Parse(ProceduresCode);
            Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
            Ad.CR_Cas_Administrative_Procedures_Targeted_Action = BranchCode;
            Ad.CR_Cas_Administrative_Procedures_User_Insert = UserLogin;
            Ad.CR_Cas_Administrative_Procedures_Type = ProcedureType;
            Ad.CR_Cas_Administrative_Procedures_Action = true;// نبحثها لاحقا
            Ad.CR_Cas_Administrative_Procedures_Doc_Date = DateTime.Now;
            Ad.CR_Cas_Administrative_Procedures_Doc_Start_Date = null;
            Ad.CR_Cas_Administrative_Procedures_Doc_End_Date = null;
            Ad.CR_Cas_Administrative_Procedures_Doc_No = "";// نبحثها لاحقا
            Ad.CR_Cas_Administrative_Procedures_Reasons = Reasons;
            db.CR_Cas_Administrative_Procedures.Add(Ad);
        }



        // POST: CasBranch/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Sup_Branch_Code,CR_Cas_Sup_Lessor_Code," +
            "CR_Cas_Sup_Branch_Ar_Name,CR_Cas_Sup_Branch_Ar_Short_Name,CR_Cas_Sup_Branch_En_Name," +
            "CR_Cas_Sup_Branch_En_Short_Name,CR_Cas_Sup_Branch_Fr_Name,CR_Cas_Sup_Branch_Fr_Short_Name," +
            "CR_Cas_Sup_Branch_Commercial_Registration_No,CR_Cas_Sup_Branch_Government_No," +
            "CR_Cas_Sup_Branch_Tax_No,CR_Cas_Sup_Branch_Tel,CR_Cas_Sup_Branch_Mobile," +
            "CR_Cas_Sup_Branch_Stamp,CR_Cas_Sup_Branch_Signature_Director," +
            "CR_Cas_Sup_Branch_Signature_Ar_Director_Name,CR_Cas_Sup_Branch_Signature_En_Director_Name," +
            "CR_Cas_Sup_Branch_Signature_Fr_Director_Name,CR_Cas_Sup_Branch_Start_Day," +
            "CR_Cas_Sup_Branch_End_Day,CR_Cas_Sup_Branch_Duration_One_Start_Time," +
            "CR_Cas_Sup_Branch_Duration_One_End_Time,CR_Cas_Sup_Branch_Duration_Tow_Start_Time," +
            "CR_Cas_Sup_Branch_Duration_Tow_End_Time,CR_Cas_Sup_Branch_Duration_Three_Start_Time," +
            "CR_Cas_Sup_Branch_Duration_Three_End_Time,CR_Cas_Sup_Branch_LogoMap,CR_Cas_Sup_Branch_Status," +
            "CR_Cas_Sup_Branch_Reasons")] CR_Cas_Sup_Branch cR_Cas_Sup_Branch, HttpPostedFileBase imgStamp, HttpPostedFileBase imgDirector,
            string CR_MAS_SUP_City_Code, string CR_MAS_SUP_Region_Code, string CR_Mas_Address_Ar_District,
            string CR_Mas_Address_En_District, string CR_Mas_Address_Fr_District, string CR_Mas_Address_Ar_Street, string CR_Mas_Address_En_Street,
            string CR_Mas_Address_Fr_Street, string CR_Mas_Address_Building, string CR_Mas_Address_Unit_No, string CR_Mas_Address_Zip_Code,
            string CR_Mas_Address_Additional_Numbers, string CR_MAS_SUP_Region_Ar_Name, string listStart, string listEnd)
        {
            var LessorCode = Session["LessorCode"].ToString();
            var UserLogin = Session["UserLogin"].ToString();
            if (ModelState.IsValid)
            {
                var LrecordExitArabeLong = db.CR_Cas_Sup_Branch.Any(Lr => Lr.CR_Cas_Sup_Branch_Ar_Name == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Ar_Name && Lr.CR_Cas_Sup_Lessor_Code == LessorCode);
                var LrecordExitArabeShort = db.CR_Cas_Sup_Branch.Any(Lr => Lr.CR_Cas_Sup_Branch_Ar_Short_Name == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Ar_Short_Name && Lr.CR_Cas_Sup_Lessor_Code == LessorCode);

                var LrecordExitEnglishLong = db.CR_Cas_Sup_Branch.Any(Lr => Lr.CR_Cas_Sup_Branch_En_Name == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_En_Name && Lr.CR_Cas_Sup_Lessor_Code == LessorCode);
                var LrecordExitEnglishShort = db.CR_Cas_Sup_Branch.Any(Lr => Lr.CR_Cas_Sup_Branch_En_Short_Name == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_En_Short_Name && Lr.CR_Cas_Sup_Lessor_Code == LessorCode);

                ////var LrecordExitFrenchLong = db.CR_Cas_Sup_Branch.Any(Lr => Lr.CR_Cas_Sup_Branch_Fr_Name == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Fr_Name);
                ////var LrecordExitFrenchShort = db.CR_Cas_Sup_Branch.Any(Lr => Lr.CR_Cas_Sup_Branch_Fr_Short_Name == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Fr_Short_Name);
                var LrecordExistGov = db.CR_Cas_Sup_Branch.Any(lr => lr.CR_Cas_Sup_Branch_Government_No == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Government_No);
                if (!LrecordExistGov)
                {
                    LrecordExistGov = db.CR_Cas_Sup_Owners.Any(lr => lr.CR_Cas_Sup_Owners_Code == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Government_No);
                }
                var LrecordExistTax = db.CR_Cas_Sup_Branch.Any(lr => lr.CR_Cas_Sup_Branch_Tax_No == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Tax_No);


                if (cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Ar_Name != null && cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Ar_Short_Name != null &&
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_En_Name != null && cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_En_Short_Name != null &&

                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Government_No != null && !LrecordExistGov && !LrecordExistTax
                    && !LrecordExitArabeLong && !LrecordExitArabeShort && !LrecordExitEnglishLong && !LrecordExitEnglishShort &&
                     cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Ar_Name.Length >= 3 && cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Ar_Short_Name.Length >= 2 &&
                        cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_En_Name.Length >= 3 && cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_En_Short_Name.Length >= 2 &&

                        cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Government_No != null && CR_Mas_Address_Ar_District != "" && CR_Mas_Address_En_District != "" && CR_Mas_Address_Fr_District != ""
                        && CR_Mas_Address_Ar_Street != "" && CR_Mas_Address_En_Street != "" && CR_Mas_Address_Fr_Street != "" && CR_MAS_SUP_City_Code != null
                        && CR_MAS_SUP_City_Code != "")
                {

                    using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                    {
                        try
                        {

                            ///////////////////////////////save branch/////////////////////////
                            ///
                            cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code = GetBranchLastCode().CR_Cas_Sup_Branch_Code;
                            cR_Cas_Sup_Branch.CR_Cas_Sup_Lessor_Code = LessorCode;
                            cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status = "A";
                            cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Start_Day = int.Parse(listStart);
                            cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_End_Day = int.Parse(listEnd);

                            db.CR_Cas_Sup_Branch.Add(cR_Cas_Sup_Branch);
                            ///////////////////////////////////////////////////////////////////
                            ///
                            ////////////////////////////////Save Documents/////////////////////////////////////////////

                            var mech = db.CR_Cas_Sup_Follow_Up_Mechanism.Where(m => m.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code == LessorCode && m.CR_Cas_Sup_Procedures_Type == "1");

                            int n = mech.Count();
                            List<CR_Cas_Sup_Branch_Documentation> ldoc = new List<CR_Cas_Sup_Branch_Documentation>();
                            CR_Cas_Sup_Branch_Documentation doc;
                            foreach (var p in mech)
                            {
                                doc = new CR_Cas_Sup_Branch_Documentation();
                                doc.CR_Cas_Sup_Branch_Documentation_Lessor_Code = LessorCode;
                                doc.CR_Cas_Sup_Branch_Documentation_Branch_Code = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code;
                                doc.CR_Cas_Sup_Branch_Documentation_Code = p.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code;
                                doc.CR_Cas_Sup_Branch_Documentation_Activation = p.CR_Cas_Sup_Follow_Up_Mechanism_Activate_Service;
                                doc.CR_Cas_Sup_Branch_Status = "A";

                                doc.CR_Cas_Sup_Branch_Documentation_Status = "N";
                                doc.CR_Cas_Sup_Procedures_Type = p.CR_Cas_Sup_Procedures_Type;
                                ldoc.Add(doc);
                            }
                            ldoc.ForEach(d => db.CR_Cas_Sup_Branch_Documentation.Add(d));
                            /// //////////////////////////////////////////////////////////////////////////
                            /// 

                            /// //////////////////////////////////save Address////////////////////////////////
                            /// 
                            CR_Mas_Address adr = new CR_Mas_Address();
                            adr.CR_Mas_Address_Id_Code = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Government_No;
                            adr.CR_Mas_Address_Regions = regioncode;
                            adr.CR_Mas_Address_City = CR_MAS_SUP_City_Code;
                            if (CR_Mas_Address_Additional_Numbers != "")
                            {
                                adr.CR_Mas_Address_Additional_Numbers = int.Parse(CR_Mas_Address_Additional_Numbers);
                            }
                            adr.CR_Mas_Address_Ar_District = CR_Mas_Address_Ar_District;
                            adr.CR_Mas_Address_Ar_Street = CR_Mas_Address_Ar_Street;
                            if (CR_Mas_Address_Building != "")
                            {
                                adr.CR_Mas_Address_Building = int.Parse(CR_Mas_Address_Building);
                            }

                            adr.CR_Mas_Address_En_District = CR_Mas_Address_En_District;
                            adr.CR_Mas_Address_En_Street = CR_Mas_Address_En_Street;
                            adr.CR_Mas_Address_Fr_District = CR_Mas_Address_Fr_District;
                            adr.CR_Mas_Address_Fr_Street = CR_Mas_Address_Fr_Street;
                            adr.CR_Mas_Address_Status = "A";
                            adr.CR_Mas_Address_Unit_No = CR_Mas_Address_Unit_No;
                            if (CR_Mas_Address_Zip_Code != "")
                            {
                                adr.CR_Mas_Address_Zip_Code = int.Parse(CR_Mas_Address_Zip_Code);
                            }

                            if (CR_Mas_Address_Additional_Numbers != "")
                            {
                                adr.CR_Mas_Address_Additional_Numbers = int.Parse(CR_Mas_Address_Additional_Numbers);
                            }
                            adr.CR_Mas_Address_UpDate_Post = DateTime.Now;
                            db.CR_Mas_Address.Add(adr);

                            /////////////////////////////////////////////////////////////////////////////////
                            ////////////////////////////////Save MainAccount (deleted)////////////////////////////////
                           
                            /// //////////////////////////////////////////////////////////////////////////
                            ////////////////////////////////Save SubAccount (deleted)////////////////////////////////
                           
                            /// //////////////////////////////////////////////////////////////////////////
                            ////////////////////////////////Save Sales Point cash////////////////////////////////
                            ///
                            CR_Cas_Sup_SalesPoint SalesPoint = new CR_Cas_Sup_SalesPoint();
                            SalesPoint.CR_Cas_Sup_SalesPoint_Code = cR_Cas_Sup_Branch.CR_Cas_Sup_Lessor_Code + cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code + cR_Cas_Sup_Branch.CR_Cas_Sup_Lessor_Code + "000000";
                            SalesPoint.CR_Cas_Sup_SalesPoint_Com_Code = cR_Cas_Sup_Branch.CR_Cas_Sup_Lessor_Code;
                            SalesPoint.CR_Cas_Sup_SalesPoint_Brn_Code = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code;
                            SalesPoint.CR_Cas_Sup_SalesPoint_Bank_Code = cR_Cas_Sup_Branch.CR_Cas_Sup_Lessor_Code + "0000";
                            SalesPoint.CR_Cas_Sup_SalesPoint_Bank_No = cR_Cas_Sup_Branch.CR_Cas_Sup_Lessor_Code + "0000";
                            SalesPoint.CR_Cas_Sup_SalesPoint_Ar_Name = " صندوق فرع" + cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Ar_Short_Name;
                            SalesPoint.CR_Cas_Sup_SalesPoint_En_Name = "Branch fund" + cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_En_Short_Name;
                            SalesPoint.CR_Cas_Sup_SalesPoint_Balance = 0;
                            SalesPoint.CR_Cas_Sup_SalesPoint_Status = "A";
                            SalesPoint.CR_Cas_Sup_SalesPoint_Branch_Status = "A";
                            SalesPoint.CR_Cas_Sup_SalesPoint_Account_Status = "A";
                            db.CR_Cas_Sup_SalesPoint.Add(SalesPoint);
                            /// //////////////////////////////////////////////////////////////////////////
                            //////////////////////////////Save signature manager branch/////////////////////////////////////////
                            //string logopath = "";

                            string signaturepath = "";


                            string folderimages = Server.MapPath(string.Format("~/{0}/", "/images"));
                            string folderCompany = Server.MapPath(string.Format("~/{0}/", "/images/Company"));
                            string FolderCompanyCode = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + LessorCode));
                            string FolderBranchCode = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + LessorCode
                                + "/" + cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code));

                            string FolderContract = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + LessorCode
                                + "/" + cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code + "/Contract"));

                           



                            string FolderSignature = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + LessorCode
                                + "/" + cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code + "/ManagerSignature"));

                            if (!Directory.Exists(folderimages))
                            {
                                Directory.CreateDirectory(folderimages);
                            }
                            if (!Directory.Exists(folderCompany))
                            {
                                Directory.CreateDirectory(folderCompany);
                            }
                            if (!Directory.Exists(FolderCompanyCode))
                            {
                                Directory.CreateDirectory(FolderCompanyCode);
                            }
                            if (!Directory.Exists(FolderBranchCode))
                            {
                                Directory.CreateDirectory(FolderBranchCode);
                            }
                           

                            if (!Directory.Exists(FolderSignature))
                            {
                                Directory.CreateDirectory(FolderSignature);
                            }

                            if (!Directory.Exists(FolderContract))
                            {
                                Directory.CreateDirectory(FolderContract);
                            }

                            if (Directory.Exists(FolderSignature))
                            {
                                if (imgDirector != null)
                                {
                                    if (imgDirector.FileName.Length > 0)
                                    {
                                        signaturepath = "~/images/Company/" + LessorCode + "/" + cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code + "/ManagerSignature/" + Path.GetFileName(imgDirector.FileName);
                                        imgDirector.SaveAs(HttpContext.Server.MapPath(signaturepath));
                                        cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Signature_Director = signaturepath;
                                    }
                                }
                                else
                                {
                                    signaturepath = "~/images/common/DefualtUserSignature.png";
                                    //string loc = "~/images/Company/" + LessorCode + "/" + cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code + "/ManagerSignature/" + "DefualtUserSignature.png";
                                    //imgDirector.SaveAs(HttpContext.Server.MapPath(loc));
                                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Signature_Director = signaturepath;
                                }
                            }


                            //////////////////////////Save Tracing/////////////////////////////
                            SaveTracing(LessorCode, "1", "48", "I", UserLogin, cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code, cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Reasons);

                            /////////////////////////////////////////////////////////////////////////////////
                            db.SaveChanges();
                            dbTran.Commit();
                            cR_Cas_Sup_Branch = new CR_Cas_Sup_Branch();
                            cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status = "A";
                            TempData["TempModel"] = "Added";
                            return RedirectToAction("Index", "CasBranch");
                        }
                        catch (Exception ex)
                        {
                            dbTran.Rollback();
                            throw ex;
                        }
                    }


                }
                else
                {
                    if (cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Ar_Name == null)
                        ViewBag.LRExistArLng = "الرجاء إدخال بيانات الحقل";
                    if (cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Ar_Short_Name == null)
                        ViewBag.LRExistArSh = "الرجاء إدخال بيانات الحقل";

                    if (cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_En_Name == null)
                        ViewBag.LRExistEnLng = "الرجاء إدخال بيانات الحقل";
                    if (cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_En_Short_Name == null)
                        ViewBag.LRExistEnSh = "الرجاء إدخال بيانات الحقل";

                    if (CR_MAS_SUP_City_Code == null || CR_MAS_SUP_City_Code == "")
                    {
                        ViewBag.CityCode = "الرجاء إختيار المدينة";
                    }

                    if (LrecordExitArabeLong)
                        ViewBag.LRExistArLng = "عفوا هذه الشركة موجودة";
                    if (LrecordExitArabeShort)
                        ViewBag.LRExistArSh = "عفوا هذه الشركة موجودة";


                    if (LrecordExitEnglishLong)
                        ViewBag.LRExistEnLng = "عفوا هذه الشركة موجودة";
                    if (LrecordExitEnglishShort)
                        ViewBag.LRExistEnSh = "عفوا هذه الشركة موجودة";
                    if (LrecordExistGov)
                        ViewBag.ExistGov = "عفوا هذا الرقم الحكومي موجود";
                    if (LrecordExistTax)
                        ViewBag.ExistTax = "عفوا هذا الرقم الضريبي موجود";

                    if (cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Government_No == null)
                        ViewBag.GovNumber = "الرجاء إدخال بيانات الحقل";

                    if (CR_Mas_Address_Ar_District == "")
                        ViewBag.ArDistrict = "الرجاء إدخال بيانات الحقل";

                    if (CR_Mas_Address_En_District == "")
                        ViewBag.EnDistrict = "الرجاء إدخال بيانات الحقل";


                    if (CR_Mas_Address_Ar_Street == "")
                        ViewBag.ArStreet = "الرجاء إدخال بيانات الحقل";

                    if (CR_Mas_Address_En_Street == "")
                        ViewBag.EnStreet = "الرجاء إدخال بيانات الحقل";

                    ViewBag.B_Ar_Name = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Ar_Name;
                    ViewBag.B_Ar_Sh_Name = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Ar_Short_Name;
                    ViewBag.B_En_Name = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_En_Name;
                    ViewBag.B_En_Sh_Name = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_En_Short_Name;
                    ViewBag.Gov = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Government_No;
                    ViewBag.RegNo = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Commercial_Registration_No;
                    ViewBag.tax = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Tax_No;
                    ViewBag.Tel = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Tel;
                    ViewBag.Mobile = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Mobile;
                    ViewBag.B_Director_Ar_Name = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Signature_Ar_Director_Name;
                    ViewBag.B_Director_Name = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Signature_En_Director_Name;


                    ViewBag.ST1 = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Duration_One_Start_Time;
                    ViewBag.ET1 = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Duration_One_End_Time;

                    ViewBag.ST2 = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Duration_Tow_Start_Time;
                    ViewBag.ET2 = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Duration_Tow_End_Time;

                    ViewBag.ST3 = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Duration_Three_Start_Time;
                    ViewBag.ET3 = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Duration_Three_End_Time;
                    ViewBag.RegionName = CR_MAS_SUP_Region_Ar_Name;
                    ViewBag.DistrictAr = CR_Mas_Address_Ar_District;
                    ViewBag.DistrictEn = CR_Mas_Address_En_District;
                    ViewBag.StreetAr = CR_Mas_Address_Ar_Street;
                    ViewBag.StreetEn = CR_Mas_Address_En_Street;
                    ViewBag.Bno = CR_Mas_Address_Building;
                    ViewBag.UnitNo = CR_Mas_Address_Unit_No;
                    ViewBag.ZipCode = CR_Mas_Address_Zip_Code;
                    ViewBag.AdditionalNo = CR_Mas_Address_Additional_Numbers;
                }
            }
          /*  var ListStart = new SelectList(new[] {
                                              new {ID="1",Name="السبت"},
                                              new{ID="2",Name="الأحد"},
                                              new{ID="3",Name="الإثنين"},
                                              new{ID="4",Name="الثلاثاء"},
                                              new{ID="5",Name="الإربعاء"},
                                              new{ID="6",Name="الخميس"},
                                              new{ID="7",Name="الجمعة"},
                                              }, "ID", "Name", int.Parse(listStart));
            ViewData["listStart"] = ListStart;
            var ListEnd = new SelectList(new[] {
                                              new {ID="1",Name="السبت"},
                                              new{ID="2",Name="الأحد"},
                                              new{ID="3",Name="الإثنين"},
                                              new{ID="4",Name="الثلاثاء"},
                                              new{ID="5",Name="الإربعاء"},
                                              new{ID="6",Name="الخميس"},
                                              new{ID="7",Name="الجمعة"},
                                              }, "ID", "Name", int.Parse(listEnd));
            ViewData["listEnd"] = ListEnd;*/
            ViewBag.CR_MAS_SUP_City_Code = new SelectList(db.CR_Mas_Sup_City.Where(x => x.CR_Mas_Sup_Regions.CR_Mas_Sup_Regions_Status != "D"), "CR_Mas_Sup_City_Code", "CR_Mas_Sup_City_Ar_Name");
            return View(cR_Cas_Sup_Branch);
        }




        // GET: CasBranch/Edit/5
        public ActionResult Edit(string id1, string id2, string id3)
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

            int citycode = 0;
            if (id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_Branch cR_Cas_Sup_Branch = db.CR_Cas_Sup_Branch.FirstOrDefault(x => x.CR_Cas_Sup_Branch_Code == id1 && x.CR_Cas_Sup_Lessor_Code == id2);
            if (cR_Cas_Sup_Branch == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status == "A" ||
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status == "Activated" ||
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status == "1" ||
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status == "Undeleted")
                {
                    ViewBag.stat = "حذف";
                    ViewBag.h = "إيقاف";
                }

                if (cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status == "D" ||
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status == "Deleted" ||
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status == "0")
                {
                    ViewBag.stat = "إسترجاع";
                    ViewBag.h = "إيقاف";
                    ViewData["ReadOnly"] = "true";
                }

                if (cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status == "H" ||
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status == "Hold" ||
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status == "2")
                {
                    ViewBag.h = "تنشيط";
                    ViewBag.stat = "حذف";
                    ViewData["ReadOnly"] = "true";
                }

                if (cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status == null)
                {
                    ViewBag.h = "إيقاف";
                    ViewBag.stat = "حذف";
                }



                ViewBag.directorEnName = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Signature_En_Director_Name;
                ViewBag.CR_Cas_Sup_Branch_En_Name = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_En_Name;
                ViewBag.CR_Cas_Sup_Branch_En_Short_Name = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_En_Short_Name;
                ViewBag.CR_Cas_Sup_Branch_Signature_En_Director_Name = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Signature_En_Director_Name;
                ViewBag.CR_Cas_Sup_Branch_Duration_One_Start_Time = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Duration_One_Start_Time;
                ViewBag.CR_Cas_Sup_Branch_Duration_One_End_Time = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Duration_One_End_Time;
                ViewBag.CR_Cas_Sup_Branch_Duration_Tow_Start_Time = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Duration_Tow_Start_Time;
                ViewBag.CR_Cas_Sup_Branch_Duration_Tow_End_Time = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Duration_Tow_End_Time;
                ViewBag.CR_Cas_Sup_Branch_Duration_Three_Start_Time = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Duration_Three_Start_Time;
                ViewBag.CR_Cas_Sup_Branch_Duration_Three_End_Time = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Duration_Three_End_Time;
                ViewBag.delete = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status;
                ViewBag.path = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Signature_Director;
                if (cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Government_No != null && cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Government_No != "")
                {
                    var adr = db.CR_Mas_Address.FirstOrDefault(x => x.CR_Mas_Address_Id_Code == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Government_No);
                    if (adr != null)
                    {
                        var regionName = db.CR_Mas_Sup_Regions.FirstOrDefault(x => x.CR_Mas_Sup_Regions_Code == adr.CR_Mas_Address_Regions);
                        if (regionName != null)
                        {
                            ViewBag.Region = regionName.CR_Mas_Sup_Regions_Ar_Name;
                        }

                        var City = db.CR_Mas_Sup_City.FirstOrDefault(x => x.CR_Mas_Sup_City_Code == adr.CR_Mas_Address_City);
                        citycode = int.Parse(City.CR_Mas_Sup_City_Code);
                    }
                }


                var carinfo = db.CR_Cas_Sup_Car_Information.Where(c => c.CR_Cas_Sup_Car_Lessor_Code == LessorCode && c.CR_Cas_Sup_Car_Status != "D"
                && c.CR_Cas_Sup_Car_Owner_Branch_Code == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code).Count();
                if (carinfo > 0)
                {
                    TempData["CarsNo"] = "True";
                    //return RedirectToAction("Index");
                }
                var BankCode = cR_Cas_Sup_Branch.CR_Cas_Sup_Lessor_Code + "0000";
                var SalesPoint = db.CR_Cas_Sup_SalesPoint.Where(s => s.CR_Cas_Sup_SalesPoint_Brn_Code == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code &&
                s.CR_Cas_Sup_SalesPoint_Com_Code == LessorCode && s.CR_Cas_Sup_SalesPoint_Bank_Code != BankCode && s.CR_Cas_Sup_SalesPoint_Status == "A");
                if (SalesPoint != null)
                {
                    //var SalesPointCount = SalesPoint.Count();
                    var SalesPointBalance = SalesPoint.Where(s => s.CR_Cas_Sup_SalesPoint_Balance > 0).Count();
                    if (SalesPointBalance > 0)
                    {
                        TempData["SalesPointNo"] = "True";
                        //return RedirectToAction("Index");
                    }

                }
            }

            //// cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Reasons = "";
            //TempData["BankCode"] = cR_Cas_Sup_Branch.CR_Cas_Sup_Lessor_Code + "0000"; 
            //TempData["BankCode"] = cR_Cas_Sup_Branch.CR_Cas_Sup_Lessor_Code + "0000";

            ViewBag.CR_MAS_SUP_City_Code = new SelectList(db.CR_Mas_Sup_City.Where(x => x.CR_Mas_Sup_Regions.CR_Mas_Sup_Regions_Status != "D"),
                "CR_Mas_Sup_City_Code", "CR_Mas_Sup_City_Ar_Name", citycode);

            var ListStart = new SelectList(new[] {
                                              new {ID="1",Name="السبت"},
                                              new{ID="2",Name="الأحد"},
                                              new{ID="3",Name="الإثنين"},
                                              new{ID="4",Name="الثلاثاء"},
                                              new{ID="5",Name="الإربعاء"},
                                              new{ID="6",Name="الخميس"},
                                              new{ID="7",Name="الجمعة"},
                                              }, "ID", "Name", cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Start_Day);
            ViewData["listStart"] = ListStart;
            var ListEnd = new SelectList(new[] {
                                              new {ID="1",Name="السبت"},
                                              new{ID="2",Name="الأحد"},
                                              new{ID="3",Name="الإثنين"},
                                              new{ID="4",Name="الثلاثاء"},
                                              new{ID="5",Name="الإربعاء"},
                                              new{ID="6",Name="الخميس"},
                                              new{ID="7",Name="الجمعة"},
                                              }, "ID", "Name", cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_End_Day);
            ViewData["listEnd"] = ListEnd;

            if (id3 != null || id3 != "")
            {
                CR_Mas_Address cR_Mas_Address = db.CR_Mas_Address.FirstOrDefault(x => x.CR_Mas_Address_Id_Code == id3);
                ViewBag.AdditionalNumber = cR_Mas_Address.CR_Mas_Address_Additional_Numbers;
                ViewBag.ZipCode = cR_Mas_Address.CR_Mas_Address_Zip_Code;
                ViewBag.UnitNumber = cR_Mas_Address.CR_Mas_Address_Unit_No;
                ViewBag.BuildingNumber = cR_Mas_Address.CR_Mas_Address_Building;
                ViewBag.StreetFr = cR_Mas_Address.CR_Mas_Address_Fr_Street;
                ViewBag.StreetAr = cR_Mas_Address.CR_Mas_Address_Ar_Street;
                ViewBag.Streeten = cR_Mas_Address.CR_Mas_Address_En_Street;
                ViewBag.DistrictAr = cR_Mas_Address.CR_Mas_Address_Ar_District;
                ViewBag.DistrictFr = cR_Mas_Address.CR_Mas_Address_Fr_District;
                ViewBag.DistrictEn = cR_Mas_Address.CR_Mas_Address_En_District;
            }

            return View(cR_Cas_Sup_Branch);
        }

        // POST: CasBranch/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Sup_Branch_Code,CR_Cas_Sup_Lessor_Code," +
            "CR_Cas_Sup_Branch_Ar_Name,CR_Cas_Sup_Branch_Ar_Short_Name,CR_Cas_Sup_Branch_En_Name," +
            "CR_Cas_Sup_Branch_En_Short_Name,CR_Cas_Sup_Branch_Fr_Name,CR_Cas_Sup_Branch_Fr_Short_Name," +
            "CR_Cas_Sup_Branch_Commercial_Registration_No,CR_Cas_Sup_Branch_Government_No,CR_Cas_Sup_Branch_Tax_No," +
            "CR_Cas_Sup_Branch_Tel,CR_Cas_Sup_Branch_Mobile,CR_Cas_Sup_Branch_Stamp,CR_Cas_Sup_Branch_Signature_Director," +
            "CR_Cas_Sup_Branch_Signature_Ar_Director_Name,CR_Cas_Sup_Branch_Signature_En_Director_Name," +
            "CR_Cas_Sup_Branch_Signature_Fr_Director_Name,CR_Cas_Sup_Branch_Start_Day,CR_Cas_Sup_Branch_End_Day," +
            "CR_Cas_Sup_Branch_Duration_One_Start_Time,CR_Cas_Sup_Branch_Duration_One_End_Time," +
            "CR_Cas_Sup_Branch_Duration_Tow_Start_Time,CR_Cas_Sup_Branch_Duration_Tow_End_Time," +
            "CR_Cas_Sup_Branch_Duration_Three_Start_Time,CR_Cas_Sup_Branch_Duration_Three_End_Time," +
            "CR_Cas_Sup_Branch_LogoMap,CR_Cas_Sup_Branch_Status,CR_Cas_Sup_Branch_Reasons")]
            CR_Cas_Sup_Branch cR_Cas_Sup_Branch, HttpPostedFileBase imgStamp, HttpPostedFileBase imgDirector,
            string CR_MAS_SUP_City_Code, string CR_MAS_SUP_Region_Code, string CR_Mas_Address_Ar_District,
            string CR_Mas_Address_En_District, string CR_Mas_Address_Fr_District, string CR_Mas_Address_Ar_Street, string CR_Mas_Address_En_Street,
            string CR_Mas_Address_Fr_Street, string CR_Mas_Address_Building, string CR_Mas_Address_Unit_No, string CR_Mas_Address_Zip_Code,
            string CR_Mas_Address_Additional_Numbers, string CR_MAS_SUP_Region_Ar_Name,
            string save, string delete, string hold, string listStart, string listEnd, string SignDirector)
        {

            var LessorCode = Session["LessorCode"].ToString();
            var BranchCode = Session["BranchCode"].ToString();
            string signaturepath = "";
            if (!string.IsNullOrEmpty(save))
            {


                if (ModelState.IsValid)
                {
                    var LrecordExitArabeLong = db.CR_Cas_Sup_Branch.Where(x => x.CR_Cas_Sup_Branch_Code != cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code).Any(Lr => Lr.CR_Cas_Sup_Branch_Ar_Name == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Ar_Name && Lr.CR_Cas_Sup_Lessor_Code == LessorCode);
                    var LrecordExitArabeShort = db.CR_Cas_Sup_Branch.Where(x => x.CR_Cas_Sup_Branch_Code != cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code).Any(Lr => Lr.CR_Cas_Sup_Branch_Ar_Short_Name == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Ar_Short_Name && Lr.CR_Cas_Sup_Lessor_Code == LessorCode);

                    var LrecordExitEnglishLong = db.CR_Cas_Sup_Branch.Where(x => x.CR_Cas_Sup_Branch_Code != cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code).Any(Lr => Lr.CR_Cas_Sup_Branch_En_Name == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_En_Name && Lr.CR_Cas_Sup_Lessor_Code == LessorCode);
                    var LrecordExitEnglishShort = db.CR_Cas_Sup_Branch.Where(x => x.CR_Cas_Sup_Branch_Code != cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code).Any(Lr => Lr.CR_Cas_Sup_Branch_En_Short_Name == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_En_Short_Name && Lr.CR_Cas_Sup_Lessor_Code == LessorCode);

                    if (cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Ar_Name != null && cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Ar_Short_Name != null &&
                        cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_En_Name != null && cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_En_Short_Name != null &&

                        cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Government_No != null
                        && !LrecordExitArabeLong && !LrecordExitArabeShort && !LrecordExitEnglishLong && !LrecordExitEnglishShort && cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Ar_Name.Length >= 3 &&
                        cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Ar_Short_Name.Length >= 2 &&
                            cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_En_Name.Length >= 3 && cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_En_Short_Name.Length >= 2 &&
                            CR_Mas_Address_Ar_District != "" && CR_Mas_Address_En_District != ""
                            && CR_Mas_Address_Ar_Street != "" && CR_Mas_Address_En_Street != "")
                    {
                        ////////////////////////////Save images/////////////////////////////////////////
                        /// string logopath = "";
                       // string stamppath = "";
                        using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                        {
                            try
                            {

                                if (imgDirector != null)
                                {

                                    string folderimages = Server.MapPath(string.Format("~/{0}/", "/images"));
                                    string folderCompany = Server.MapPath(string.Format("~/{0}/", "/images/Company"));
                                    string FolderCompanyCode = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + LessorCode));
                                    string FolderBranchCode = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + LessorCode
                                        + "/" + cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code));

                                    string FolderContract = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + LessorCode
                                        + "/" + cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code + "/Contract"));

                                    //String FolderLOGO = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + AccountController.LessorCode
                                    //    + "/" + cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code + "/" + "LOGO"));



                                    String FolderSignature = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + LessorCode
                                        + "/" + cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code + "/ManagerSignature"));

                                    if (!Directory.Exists(folderimages))
                                    {
                                        Directory.CreateDirectory(folderimages);
                                    }
                                    if (!Directory.Exists(folderCompany))
                                    {
                                        Directory.CreateDirectory(folderCompany);
                                    }
                                    if (!Directory.Exists(FolderCompanyCode))
                                    {
                                        Directory.CreateDirectory(FolderCompanyCode);
                                    }
                                    if (!Directory.Exists(FolderBranchCode))
                                    {
                                        Directory.CreateDirectory(FolderBranchCode);
                                    }
                                    //if (!Directory.Exists(FolderLOGO))
                                    //{
                                    //    Directory.CreateDirectory(FolderLOGO);
                                    //}

                                    if (!Directory.Exists(FolderSignature))
                                    {
                                        Directory.CreateDirectory(FolderSignature);
                                    }

                                    if (!Directory.Exists(FolderContract))
                                    {
                                        Directory.CreateDirectory(FolderContract);
                                    }

                                    if (Directory.Exists(FolderSignature))
                                    {
                                        if (imgDirector.FileName.Length > 0)
                                        {
                                            signaturepath = "~/images/Company/" + LessorCode + "/" + cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code + "/ManagerSignature/" + Path.GetFileName(imgDirector.FileName);
                                            imgDirector.SaveAs(HttpContext.Server.MapPath(signaturepath));
                                            cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Signature_Director = signaturepath;
                                        }
                                    }
                                }
                                else
                                {

                                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Signature_Director = SignDirector;

                                }



                                //////////////////////////Save Tracing/////////////////////////////
                                var UserLogin = Session["UserLogin"].ToString();
                                SaveTracing(LessorCode, "1", "48", "U", UserLogin, cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code, cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Reasons);
                                /////////////////////////////////update address/////////////////////////////////
                                var adr = db.CR_Mas_Address.FirstOrDefault(a => a.CR_Mas_Address_Id_Code == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Government_No);
                                if (adr != null)
                                {
                                    adr.CR_Mas_Address_Ar_District = CR_Mas_Address_Ar_District;
                                    adr.CR_Mas_Address_Ar_Street = CR_Mas_Address_Ar_Street;
                                    if (adr.CR_Mas_Address_Building != null)
                                    {
                                        adr.CR_Mas_Address_Building = int.Parse(CR_Mas_Address_Building);
                                    }

                                    adr.CR_Mas_Address_En_District = CR_Mas_Address_En_District;
                                    adr.CR_Mas_Address_En_Street = CR_Mas_Address_En_Street;
                                    adr.CR_Mas_Address_Unit_No = CR_Mas_Address_Unit_No;
                                    if (adr.CR_Mas_Address_Zip_Code != null)
                                    {
                                        adr.CR_Mas_Address_Zip_Code = int.Parse(CR_Mas_Address_Zip_Code);
                                    }
                                    if (adr.CR_Mas_Address_Additional_Numbers != null)
                                    {
                                        adr.CR_Mas_Address_Additional_Numbers = int.Parse(CR_Mas_Address_Additional_Numbers);
                                    }

                                    db.Entry(adr).State = EntityState.Modified;
                                }

                                /////////////////////////////////////////////////////////////////////////////////
                                ///

                                //cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status = "A";
                                cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Start_Day = int.Parse(listStart);
                                cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_End_Day = int.Parse(listEnd);
                                db.Entry(cR_Cas_Sup_Branch).State = EntityState.Modified;
                                db.SaveChanges();
                                dbTran.Commit();
                                TempData["TempModel"] = "Saved";
                            }
                            catch (DbEntityValidationException ex)
                            {
                                dbTran.Rollback();
                                throw ex;
                            }
                        }
                    }
                    else
                    {
                        if (cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Ar_Name == null)
                            ViewBag.LRExistArLng = "الرجاء إدخال بيانات الحقل";
                        if (cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Ar_Short_Name == null)
                            ViewBag.LRExistArSh = "الرجاء إدخال بيانات الحقل";

                        if (cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_En_Name == null)
                            ViewBag.LRExistEnLng = "الرجاء إدخال بيانات الحقل";
                        if (cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_En_Short_Name == null)
                            ViewBag.LRExistEnSh = "الرجاء إدخال بيانات الحقل";

                        if (cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Fr_Name == null)
                            ViewBag.LRExistFrLng = "الرجاء إدخال بيانات الحقل";
                        if (cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Fr_Short_Name == null)
                            ViewBag.LRExistFrSh = "الرجاء إدخال بيانات الحقل";


                        if (LrecordExitArabeLong)
                            ViewBag.LRExistArLng = "عفوا هذه الشركة موجودة";
                        if (LrecordExitArabeShort)
                            ViewBag.LRExistArSh = "عفوا هذه الشركة موجودة";


                        if (LrecordExitEnglishLong)
                            ViewBag.LRExistEnLng = "عفوا هذه الشركة موجودة";
                        if (LrecordExitEnglishShort)
                            ViewBag.LRExistEnSh = "عفوا هذه الشركة موجودة";


                       

                        if (cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Government_No == null)
                            ViewBag.GovNumber = "الرجاء إدخال بيانات الحقل";

                        if (CR_Mas_Address_Ar_District == "")
                            ViewBag.ArDistrict = "الرجاء إدخال بيانات الحقل";

                        if (CR_Mas_Address_En_District == "")
                            ViewBag.EnDistrict = "الرجاء إدخال بيانات الحقل";

                        if (CR_Mas_Address_Fr_District == "")
                            ViewBag.FrDistrict = "الرجاء إدخال بيانات الحقل";

                        if (CR_Mas_Address_Ar_Street == "")
                            ViewBag.ArStreet = "الرجاء إدخال بيانات الحقل";

                        if (CR_Mas_Address_En_Street == "")
                            ViewBag.EnStreet = "الرجاء إدخال بيانات الحقل";

                        if (CR_Mas_Address_Fr_Street == "")
                            ViewBag.FrStreet = "الرجاء إدخال بيانات الحقل";

                        if (CR_Mas_Address_Building == "")
                            ViewBag.BuildingNo = "الرجاء إدخال بيانات الحقل";
                        TempData["TempModel"] = "Error";
                    }

                    return RedirectToAction("Index");
                }

            }
            if (delete == "Delete" || delete == "حذف")
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        //////////////////////////Save Tracing/////////////////////////////
                        var UserLogin = Session["UserLogin"].ToString();
                        SaveTracing(LessorCode, "1", "48", "D", UserLogin, cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code, cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Reasons);
                       
                        ///////////////////////Delete validity branch//////////////////////////
                        DeleteAuthority(cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code);
                        //////////////////////////////////////////////////////////////////
                        cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Signature_Director = SignDirector;
                        cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status = "D";
                        db.Entry(cR_Cas_Sup_Branch).State = EntityState.Modified;

                        ///////////////////////Delete  branch Docs //////////////////////////

                        var branch_Doc = db.CR_Cas_Sup_Branch_Documentation.Where(br => br.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode && br.CR_Cas_Sup_Branch_Documentation_Branch_Code == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code);

                        foreach (var doc in branch_Doc)
                        {
                            doc.CR_Cas_Sup_Branch_Status = "D";

                        }
                        ///////////////////////Delete  branch Docs ////////////////////////// 



                        ///////////////////////Delete  branch sales point  //////////////////////////

                        var branch_Sales = db.CR_Cas_Sup_SalesPoint.Where(br => br.CR_Cas_Sup_SalesPoint_Com_Code == LessorCode && br.CR_Cas_Sup_SalesPoint_Brn_Code == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code);

                        foreach (var sale in branch_Sales)
                        {
                            sale.CR_Cas_Sup_SalesPoint_Branch_Status = "D";
                            sale.CR_Cas_Sup_SalesPoint_Status = "D";
                        }
                        ///////////////////////Delete  branch sales point  //////////////////////////

                        ///////////////////////Delete  branch User Validate  //////////////////////////

                        var branch_User_Validate = db.CR_Cas_User_Branch_Validity.Where(br => br.CR_Cas_User_Branch_Validity_Lessor == LessorCode && br.CR_Cas_User_Branch_Validity_Branch == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code);

                        foreach (var user_valdity in branch_User_Validate)
                        {
                            user_valdity.CR_Mas_User_Branch_Validity_Branch_Status = "D";
                        }
                        /////////////////////// Delete  branch User Validate //////////////////////////


                        db.SaveChanges();
                        dbTran.Commit();
                        TempData["TempModel"] = "Deleted";
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

                        //////////////////////////Save Tracing/////////////////////////////
                        var UserLogin = Session["UserLogin"].ToString();
                        SaveTracing(LessorCode, "1", "48", "A", UserLogin, cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code, cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Reasons);
                        ///////////////////////////////////////////////////////////////////
                        ////////////////////////////Delete docs/////////////////////////////
                        //var doc = db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode && d.CR_Cas_Sup_Branch_Documentation_Branch_Code == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code);
                        //var nbr = doc.Count();
                        //if (doc != null)
                        //{
                        //    foreach (var d in doc)
                        //    {
                        //        d.CR_Cas_Sup_Branch_Documentation_About_To_Expire = null;
                        //        //d.CR_Cas_Sup_Branch_Documentation_Activation = null;
                        //        d.CR_Cas_Sup_Branch_Documentation_No = null;
                        //        d.CR_Cas_Sup_Branch_Documentation_Date = null;
                        //        d.CR_Cas_Sup_Branch_Documentation_Start_Date = null;
                        //        d.CR_Cas_Sup_Branch_Documentation_End_Date = null;
                        //        d.CR_Cas_Sup_Branch_Documentation_Credit_Limit = null;
                        //        d.CR_Cas_Sup_Branch_Documentation_Status = "N";
                        //        db.Entry(d).State = EntityState.Modified;
                        //    }
                        //}
                        //////////////////////////////////////////////////////////////////
                        cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Signature_Director = SignDirector;
                        cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status = "A";
                        db.Entry(cR_Cas_Sup_Branch).State = EntityState.Modified;


                        ///////////////////////Activate  branch Docs //////////////////////////

                        var branch_Doc = db.CR_Cas_Sup_Branch_Documentation.Where(br => br.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode && br.CR_Cas_Sup_Branch_Documentation_Branch_Code == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code);

                        foreach (var doc in branch_Doc)
                        {
                            doc.CR_Cas_Sup_Branch_Status = "A";

                        }
                        ///////////////////////Activate  branch Docs ////////////////////////// 



                        ///////////////////////Activate  branch sales point  //////////////////////////

                        var branch_Sales = db.CR_Cas_Sup_SalesPoint.Where(br => br.CR_Cas_Sup_SalesPoint_Com_Code == LessorCode && br.CR_Cas_Sup_SalesPoint_Brn_Code == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code);

                        foreach (var sale in branch_Sales)
                        {
                            sale.CR_Cas_Sup_SalesPoint_Branch_Status = "A";
                        }
                        ///////////////////////Activate  branch sales point  //////////////////////////

                        ///////////////////////Activate  branch User Validate  //////////////////////////

                        var branch_User_Validate = db.CR_Cas_User_Branch_Validity.Where(br => br.CR_Cas_User_Branch_Validity_Lessor == LessorCode && br.CR_Cas_User_Branch_Validity_Branch == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code);

                        foreach (var user_valdity in branch_User_Validate)
                        {
                            user_valdity.CR_Mas_User_Branch_Validity_Branch_Status = "A";
                        }
                        /////////////////////// Activate  branch User Validate //////////////////////////




                        db.SaveChanges();
                        dbTran.Commit();
                        TempData["TempModel"] = "Activated";
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
                        //////////////////////////Save Tracing/////////////////////////////
                        var UserLogin = Session["UserLogin"].ToString();
                        SaveTracing(LessorCode, "1", "48", "H", UserLogin, cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code, cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Reasons);
                        ///////////////////////////////////////////////////////////////////
                        cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Signature_Director = SignDirector;
                        cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status = "H";


                        // Check And HOld All Cars in this branch/////////////////////////////////////////////////////////////////
                        /* var CarsActive = db.CR_Cas_Sup_Car_Information.Where(c => c.CR_Cas_Sup_Car_Status == "A" && c.CR_Cas_Sup_Car_Lessor_Code == LessorCode &&
                           c.CR_Cas_Sup_Car_Location_Branch_Code == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code && c.CR_Cas_Sup_Car_Documentation_Status == true && c.CR_Cas_Sup_Car_Price_Status == "1"
                           && c.CR_Cas_Sup_Car_Maintenance_Status == true).ToList();

                         foreach (var item in CarsActive)
                         {
                             item.CR_Cas_Sup_Car_Status = "A";

                         }*/
                        // Check And HOld All Cars in this branch/////////////////////////////////////////////////////////////////

                        var CarsActive = db.CR_Cas_Sup_Car_Information.Where(c => c.CR_Cas_Sup_Car_Status == "A" && c.CR_Cas_Sup_Car_Lessor_Code == LessorCode &&
                       c.CR_Cas_Sup_Car_Location_Branch_Code == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code && c.CR_Cas_Sup_Car_Documentation_Status == true && c.CR_Cas_Sup_Car_Price_Status == "1"
                       && c.CR_Cas_Sup_Car_Maintenance_Status == true).ToList();

                        foreach (var item in CarsActive)
                        {
                            item.CR_Cas_Sup_Car_Branch_Status = "H";

                        }

                        ///////////////////////hold  branch Docs //////////////////////////

                        var branch_Doc = db.CR_Cas_Sup_Branch_Documentation.Where(br => br.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode && br.CR_Cas_Sup_Branch_Documentation_Branch_Code == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code);

                        foreach (var doc in branch_Doc)
                        {
                            doc.CR_Cas_Sup_Branch_Status = "H";

                        }
                        ///////////////////////hold  branch Docs ////////////////////////// 



                        ///////////////////////hold  branch sales point  //////////////////////////

                        var branch_Sales = db.CR_Cas_Sup_SalesPoint.Where(br => br.CR_Cas_Sup_SalesPoint_Com_Code == LessorCode && br.CR_Cas_Sup_SalesPoint_Brn_Code == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code);

                        foreach (var sale in branch_Sales)
                        {
                            sale.CR_Cas_Sup_SalesPoint_Branch_Status = "H";
                        }
                        ///////////////////////hold  branch sales point  //////////////////////////

                        ///////////////////////hold  branch User Validate  //////////////////////////

                        var branch_User_Validate = db.CR_Cas_User_Branch_Validity.Where(br => br.CR_Cas_User_Branch_Validity_Lessor == LessorCode && br.CR_Cas_User_Branch_Validity_Branch == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code);

                        foreach (var user_valdity in branch_User_Validate)
                        {
                            user_valdity.CR_Mas_User_Branch_Validity_Branch_Status = "H";
                        }
                        /////////////////////// hold  branch User Validate //////////////////////////

                        db.Entry(cR_Cas_Sup_Branch).State = EntityState.Modified;
                        db.SaveChanges();
                        dbTran.Commit();
                        TempData["TempModel"] = "Holded";
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

                        //////////////////////////Save Tracing/////////////////////////////
                        var UserLogin = Session["UserLogin"].ToString();
                        SaveTracing(LessorCode, "1", "48", "A", UserLogin, cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code, cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Reasons);
                        ///////////////////////////////////////////////////////////////////
                        cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Signature_Director = SignDirector;
                        cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status = "A";


                        // Check And HOld All Cars in this branch/////////////////////////////////////////////////////////////////
                        /*  var CarsActive = db.CR_Cas_Sup_Car_Information.Where(c => c.CR_Cas_Sup_Car_Status == "H" && c.CR_Cas_Sup_Car_Lessor_Code == LessorCode &&
                            c.CR_Cas_Sup_Car_Location_Branch_Code == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code && c.CR_Cas_Sup_Car_Documentation_Status == true && c.CR_Cas_Sup_Car_Price_Status == "1"
                            && c.CR_Cas_Sup_Car_Maintenance_Status == true).ToList();
                          foreach (var item in CarsActive)
                          {
                              item.CR_Cas_Sup_Car_Status = "A";
                          }*/
                        // Check And HOld All Cars in this branch/////////////////////////////////////////////////////////////////

                        ///////////////////////hold  branch Docs //////////////////////////

                        var branch_Doc = db.CR_Cas_Sup_Branch_Documentation.Where(br => br.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode && br.CR_Cas_Sup_Branch_Documentation_Branch_Code == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code);

                        foreach (var doc in branch_Doc)
                        {
                            doc.CR_Cas_Sup_Branch_Status = "A";

                        }
                        ///////////////////////active  branch Docs ////////////////////////// 



                        ///////////////////////active  branch sales point  //////////////////////////

                        var branch_Sales = db.CR_Cas_Sup_SalesPoint.Where(br => br.CR_Cas_Sup_SalesPoint_Com_Code == LessorCode && br.CR_Cas_Sup_SalesPoint_Brn_Code == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code);

                        foreach (var sale in branch_Sales)
                        {
                            sale.CR_Cas_Sup_SalesPoint_Branch_Status = "A";
                        }
                        ///////////////////////active  branch sales point  //////////////////////////

                        ///////////////////////active  branch User Validate  //////////////////////////

                        var branch_User_Validate = db.CR_Cas_User_Branch_Validity.Where(br => br.CR_Cas_User_Branch_Validity_Lessor == LessorCode && br.CR_Cas_User_Branch_Validity_Branch == cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code);

                        foreach (var user_valdity in branch_User_Validate)
                        {
                            user_valdity.CR_Mas_User_Branch_Validity_Branch_Status = "A";
                        }
                        /////////////////////// active  branch User Validate //////////////////////////


                        db.Entry(cR_Cas_Sup_Branch).State = EntityState.Modified;
                        db.SaveChanges();
                        dbTran.Commit();
                        TempData["TempModel"] = "Activated";
                        return RedirectToAction("Index");

                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }

            }
            if (cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status == "A" ||
            cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status == "Activated" ||
            cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status == "1" ||
            cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }
            if ((cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status == "D" ||
                 cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status == "Deleted" ||
                 cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
            }
            if (cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status == "H" ||
                cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status == "Hold" ||
                cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
            }
            if (cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }
            var ListStart = new SelectList(new[] {
                                              new {ID="1",Name="السبت"},
                                              new{ID="2",Name="الأحد"},
                                              new{ID="3",Name="الإثنين"},
                                              new{ID="4",Name="الثلاثاء"},
                                              new{ID="5",Name="الإربعاء"},
                                              new{ID="6",Name="الخميس"},
                                              new{ID="7",Name="الجمعة"},
                                              }, "ID", "Name", cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Start_Day);
            ViewData["listStart"] = ListStart;
            var ListEnd = new SelectList(new[] {
                                              new {ID="1",Name="السبت"},
                                              new{ID="2",Name="الأحد"},
                                              new{ID="3",Name="الإثنين"},
                                              new{ID="4",Name="الثلاثاء"},
                                              new{ID="5",Name="الإربعاء"},
                                              new{ID="6",Name="الخميس"},
                                              new{ID="7",Name="الجمعة"},
                                              }, "ID", "Name", cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_End_Day);
            ViewData["listEnd"] = ListEnd;
            ViewBag.CR_MAS_SUP_City_Code = new SelectList(db.CR_Mas_Sup_City.Where(x => x.CR_Mas_Sup_Regions.CR_Mas_Sup_Regions_Status != "D"), "CR_Mas_Sup_City_Code", "CR_Mas_Sup_City_Ar_Name");

            return View(cR_Cas_Sup_Branch);


        }

        public void DeleteAuthority(string id)
        {
            id = id.Trim();
            var q = db.CR_Cas_User_Branch_Validity.Where(c => c.CR_Cas_User_Branch_Validity_Branch == id);

            foreach (var item in q)
            {
                var auth = db.CR_Cas_User_Branch_Validity.Where(x => x.CR_Cas_User_Branch_Validity_Branch == id).First();
                db.CR_Cas_User_Branch_Validity.Remove(auth);
            }
            db.SaveChanges();
        }

        // GET: CasBranch/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Sup_Branch cR_Cas_Sup_Branch = db.CR_Cas_Sup_Branch.Find(id);
            if (cR_Cas_Sup_Branch == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Sup_Branch);
        }

        // POST: CasBranch/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Cas_Sup_Branch cR_Cas_Sup_Branch = db.CR_Cas_Sup_Branch.Find(id);
            db.CR_Cas_Sup_Branch.Remove(cR_Cas_Sup_Branch);
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
