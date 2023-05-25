using RentCar.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace RentCar.Controllers
{
    public class LessorController : Controller
    {
        private static string regioncode = "";
        private RentCarDBEntities db = new RentCarDBEntities();
        public static string TaskCode;
        // GET: Lessor
        [ActionName("Index")]
        public ActionResult Index_Get()
        {
            TaskCode = "1401";
            Session["POS"] = "1401";
            return View(db.CR_Mas_Com_Lessor.ToList());
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

            //if (!string.IsNullOrEmpty(excelCall))
            //{
            //    var LessorTable = new System.Data.DataTable("teste");

            //    LessorTable.Columns.Add("الملاحظات", typeof(string));
            //    LessorTable.Columns.Add("الحالة", typeof(string));
            //    LessorTable.Columns.Add("الإسم العربي", typeof(string));
            //    LessorTable.Columns.Add("الرمز", typeof(string));
            //    var Lrecord = db.CR_Mas_Sup_Brand.ToList();
            //    if (Lrecord != null)
            //    {
            //        foreach (var i in Lrecord)
            //        {
            //            LessorTable.Rows.Add(i.CR_Mas_Sup_Brand_Reasons, i.CR_Mas_Sup_Brand_Status, i.CR_Mas_Sup_Brand_Ar_Name,
            //                                i.CR_Mas_Sup_Brand_Code);
            //        }
            //    }
            //    var grid = new System.Web.UI.WebControls.GridView();
            //    grid.DataSource = LessorTable;
            //    grid.DataBind();

            //    Response.ClearContent();
            //    Response.Buffer = true;
            //    Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
            //    Response.ContentType = "application/ms-excel";

            //    Response.Charset = "";
            //    StringWriter sw = new StringWriter();
            //    HtmlTextWriter htw = new HtmlTextWriter(sw);

            //    grid.RenderControl(htw);

            //    Response.Output.Write(sw.ToString());
            //    Response.Flush();
            //    Response.End();
            //}

            return View(db.CR_Mas_Com_Lessor.ToList());
        }

        // GET: Lessor/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CR_Mas_Com_Lessor cR_Mas_Com_Lessor = db.CR_Mas_Com_Lessor.Find(id);
        //    if (cR_Mas_Com_Lessor == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cR_Mas_Com_Lessor);
        //}



        public CR_Mas_Com_Lessor GetLastRecord()
        {
            var Lrecord = db.CR_Mas_Com_Lessor.Max(Lr => Lr.CR_Mas_Com_Lessor_Code);
            CR_Mas_Com_Lessor l = new CR_Mas_Com_Lessor();
            if (Lrecord != null && Lrecord!="1000")
            {
                int val = int.Parse(Lrecord) + 1;
                l.CR_Mas_Com_Lessor_Code = val.ToString();
            }
            else
            {
                l.CR_Mas_Com_Lessor_Code = "5001";
            }
            return l;
        }

        //public CR_Cas_Sup_Branch getBranchLastCode()
        //{
        //    var Lrecord = db.CR_Cas_Sup_Branch.Max(Lr => Lr.CR_Cas_Sup_Branch_Code);
        //    CR_Cas_Sup_Branch b = new CR_Cas_Sup_Branch();
        //    if (Lrecord != null)
        //    {
        //        int val = int.Parse(Lrecord) + 1;
        //        b.CR_Cas_Sup_Branch_Code = val.ToString();
        //    }
        //    else
        //    {
        //        b.CR_Cas_Sup_Branch_Code = "100";
        //    }
        //    return b;
        //}


        //////public CR_Cas_User_Information getCASUserLastCode()
        //////{
        //////    var Lrecord = db.CR_Cas_User_Information.Max(Lr => Lr.CR_Cas_User_Information_Id);
        //////    CR_Cas_User_Information user = new CR_Cas_User_Information();
        //////    if (Lrecord != null)
        //////    {
        //////        int val = int.Parse(Lrecord) + 1;
        //////        user.CR_Cas_User_Information_Id = val.ToString();
        //////    }
        //////    else
        //////    {
        //////        user.CR_Cas_User_Information_Id = "1001";
        //////    }
        //////    return user;
        //////}




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


        // GET: Lessor/Create
        public ActionResult Create()
        {
            CR_Mas_Com_Lessor lessor = new CR_Mas_Com_Lessor();
            lessor = GetLastRecord();
            lessor.CR_Mas_Com_Lessor_Status = "A";
            //ViewBag.CR_MAS_SUP_City_Code = new SelectList(db.CR_Mas_Sup_City, "CR_Mas_Sup_City_Code", "CR_Mas_Sup_City_Ar_Name");
            ViewBag.CR_MAS_SUP_City_Code = new SelectList(db.CR_Mas_Sup_City.Where(x => x.CR_Mas_Sup_Regions.CR_Mas_Sup_Regions_Status != "D"), "CR_Mas_Sup_City_Code", "CR_Mas_Sup_City_Ar_Name");
            return View(lessor);
        }


        // POST: Lessor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Mas_Com_Lessor_Code,CR_Mas_Com_Lessor_Logo,CR_Mas_Com_Lessor_Ar_Long_Name," +
            "CR_Mas_Com_Lessor_Ar_Short_Name,CR_Mas_Com_Lessor_En_Long_Name,CR_Mas_Com_Lessor_En_Short_Name,CR_Mas_Com_Lessor_Fr_Long_Name," +
            "CR_Mas_Com_Lessor_Fr_Short_Name,CR_Mas_Com_Lessor_Location_Coordinates,CR_Mas_Com_Lessor_Commercial_Registration_No," +
            "CR_Mas_Com_Lessor_Sector,CR_Mas_Com_Lessor_Government_No,CR_Mas_Com_Lessor_Tax_Number," +
            "CR_Mas_Com_Lessor_Communication_Ar_Officer_Name,CR_Mas_Com_Lessor_Communication_En_Officer_Name," +
            "CR_Mas_Com_Lessor_Communication_Fr_Officer_Name,CR_Mas_Com_Lessor_Communication_Officer_Emaile," +
            "CR_Mas_Com_Lessor_Communication_Officer_Mobile,CR_Mas_Com_Lessor_Tolk_Free,CR_Mas_Com_Lessor_Email," +
            "CR_Mas_Com_Lessor_Twiter,CR_Mas_Com_Lessor_FaceBook,CR_Mas_Com_Lessor_Instagram,CR_Mas_Com_Lessor_Stamp," +
            "CR_Mas_Com_Lessor_Signature_Director,CR_Mas_Com_Lessor_Signature_Ar_Director_Name,CR_Mas_Com_Lessor_Signature_En_Director_Name," +
            "CR_Mas_Com_Lessor_Signature_Fr_Director_Name,CR_Mas_Com_Lessor_Status,CR_Mas_Com_Lessor_Reasons")] CR_Mas_Com_Lessor cR_Mas_Com_Lessor, HttpPostedFileBase img,
            HttpPostedFileBase imgStamp, HttpPostedFileBase imgDirector, string CR_MAS_SUP_City_Code, string CR_MAS_SUP_Region_Code, string CR_Mas_Address_Ar_District,
            string CR_Mas_Address_En_District, string CR_Mas_Address_Fr_District, string CR_Mas_Address_Ar_Street, string CR_Mas_Address_En_Street,
            string CR_Mas_Address_Fr_Street, string CR_Mas_Address_Building, string CR_Mas_Address_Unit_No, string CR_Mas_Address_Zip_Code,
            string CR_Mas_Address_Additional_Numbers, string CR_MAS_SUP_Region_Ar_Name,HttpPostedFileBase imgContractCard, HttpPostedFileBase ImgContractConditions)
        {
            if (ModelState.IsValid)
            {
                var LrecordExitArabeLong = db.CR_Mas_Com_Lessor.Any(Lr => Lr.CR_Mas_Com_Lessor_Ar_Long_Name == cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Long_Name);
                var LrecordExitArabeShort = db.CR_Mas_Com_Lessor.Any(Lr => Lr.CR_Mas_Com_Lessor_Ar_Short_Name == cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Short_Name);

                var LrecordExitEnglishLong = db.CR_Mas_Com_Lessor.Any(Lr => Lr.CR_Mas_Com_Lessor_En_Long_Name == cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Long_Name);
                var LrecordExitEnglishShort = db.CR_Mas_Com_Lessor.Any(Lr => Lr.CR_Mas_Com_Lessor_En_Short_Name == cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Short_Name);

                var LrecordExitFrenchLong = db.CR_Mas_Com_Lessor.Any(Lr => Lr.CR_Mas_Com_Lessor_Fr_Long_Name == cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Long_Name);
                var LrecordExitFrenchShort = db.CR_Mas_Com_Lessor.Any(Lr => Lr.CR_Mas_Com_Lessor_Fr_Short_Name == cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Short_Name);
                var gov = db.CR_Mas_Com_Lessor.Any(Lr => Lr.CR_Mas_Com_Lessor_Government_No == cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Government_No);


                string branchcode = "100";



                //var addr = new System.Net.Mail.MailAddress(cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Email);



                if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Long_Name != null && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Short_Name != null &&
                    cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Long_Name != null && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Short_Name != null &&
                    cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Long_Name != null && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Short_Name != null && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Government_No != null
                    && !LrecordExitArabeLong && !LrecordExitArabeShort && !LrecordExitEnglishLong && !LrecordExitEnglishShort && !LrecordExitFrenchLong &&
                    !LrecordExitFrenchShort && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Long_Name.Length >= 3 && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Short_Name.Length >= 2 &&
                        cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Long_Name.Length >= 3 && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Short_Name.Length >= 2 &&
                        cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Long_Name.Length >= 3 && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Short_Name.Length >= 2 &&
                        cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Government_No != null && CR_Mas_Address_Ar_District != "" && CR_Mas_Address_En_District != "" && CR_Mas_Address_Fr_District != ""
                        && CR_Mas_Address_Ar_Street != "" && CR_Mas_Address_En_Street != "" && CR_Mas_Address_Fr_Street != "" && CR_Mas_Address_Building != "" && !gov)
                {
                    string logopath = "";
                    string stamppath = "";
                    string signaturepath = "";
                    string CardPath = "";
                    string ConditionsPath = "";
                    string UserID = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code;
                    //if (img != null && imgDirector != null && imgStamp != null && imgContractCard!=null && ImgContractConditions!=null)
                    //{

                        string folderimages = Server.MapPath(string.Format("~/{0}/", "/images"));
                        string folderCompany = Server.MapPath(string.Format("~/{0}/", "/images/Company"));
                        string FolderLessor = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code));
                        string FolderUsers = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code + "/" + "Users"));

                        string FolderBranchCode = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                            + "/" + branchcode));

                        string FolderLOGO = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                            + "/" + "Logo"));

                        string FolderStamp = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                            + "/" + "Stamp"));

                        string FolderSignature = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                            + "/" + "ManagerSignature"));


                        string FolderCard = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                            + "/" + "ContractCard"));


                        string FolderCondition = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                            + "/" + "‏‏‏‏‏‏‏‏ContractConditions"));
                        string FolderMembership = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                            + "/" + "Membership"));


                        string FolderUsersID = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                           + "/" + "Users" + "/" + UserID));

                        

                        if (!Directory.Exists(folderimages))
                        {
                            Directory.CreateDirectory(folderimages);
                        }
                        if (!Directory.Exists(FolderUsers))
                        {
                            Directory.CreateDirectory(FolderUsers);
                        }
                        if (!Directory.Exists(folderCompany))
                        {
                            Directory.CreateDirectory(folderCompany);
                        }
                        if (!Directory.Exists(FolderLessor))
                        {
                            Directory.CreateDirectory(FolderLessor);
                        }
                        if (!Directory.Exists(FolderBranchCode))
                        {
                            Directory.CreateDirectory(FolderBranchCode);
                            if (imgDirector.FileName.Length > 0)
                            {
                                signaturepath = "~/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code + "/" + branchcode + "/" + Path.GetFileName(imgDirector.FileName);
                                imgDirector.SaveAs(HttpContext.Server.MapPath(signaturepath));
                                cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Logo = signaturepath;
                            }
                           
                        }
                        if (!Directory.Exists(FolderLOGO))
                        {
                            Directory.CreateDirectory(FolderLOGO);
                        }
                        if (!Directory.Exists(FolderStamp))
                        {
                            Directory.CreateDirectory(FolderStamp);
                        }
                        if (!Directory.Exists(FolderSignature))
                        {
                            Directory.CreateDirectory(FolderSignature);
                        }

                        if (!Directory.Exists(FolderCard))
                        {
                            Directory.CreateDirectory(FolderCard);
                        }

                        //if (!Directory.Exists(FolderCarLicense))
                        //{
                        //    Directory.CreateDirectory(FolderCarLicense);
                        //}

                        if (!Directory.Exists(FolderCondition))
                        {
                            Directory.CreateDirectory(FolderCondition);
                        }

                        if (!Directory.Exists(FolderMembership))
                        {
                            Directory.CreateDirectory(FolderMembership);
                        }


                        if (!Directory.Exists(FolderUsersID))
                        {
                            Directory.CreateDirectory(FolderUsersID);
                            if (imgDirector.FileName.Length > 0)
                            {
                                signaturepath = "~/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code + "/" + "Users" + "/" + UserID + "/" + Path.GetFileName(imgDirector.FileName);
                                imgDirector.SaveAs(HttpContext.Server.MapPath(signaturepath));
                                cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Logo = signaturepath;
                            }
                            else
                            {
                                cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Logo = "~/images/common/DefualtUserSignature.png";
                            }
                        }

                        if (Directory.Exists(FolderLOGO))
                        {
                            if (img.FileName.Length > 0)
                            {
                                logopath = "~/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code + "/" + "LOGO" + "/" + Path.GetFileName(img.FileName);

                                img.SaveAs(HttpContext.Server.MapPath(logopath));
                                cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Logo = logopath;
                            }
                        }
                        if (Directory.Exists(FolderStamp))
                        {
                            if (imgStamp.FileName.Length > 0)
                            {
                                stamppath = "~/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code + "/" + "Stamp" + "/" + Path.GetFileName(imgStamp.FileName);
                                imgStamp.SaveAs(HttpContext.Server.MapPath(stamppath));
                                cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Stamp = stamppath;
                            }
                            else
                            {
                                cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Stamp = "~/images/common/stamp-5.png";
                            }
                        }

                        if (Directory.Exists(FolderSignature))
                        {
                            if (imgDirector.FileName.Length > 0)
                            {
                                signaturepath = "~/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code + "/" + "ManagerSignature" + "/" + Path.GetFileName(imgDirector.FileName);
                                imgDirector.SaveAs(HttpContext.Server.MapPath(signaturepath));
                                cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Signature_Director = signaturepath; 
                            }
                            else
                            {
                                cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Signature_Director = "~/images/common/DefualtUserSignature.png";
                            }
                        }
                        if (Directory.Exists(FolderCard))
                        {
                            if (imgContractCard.FileName.Length > 0)
                            {
                                CardPath = "~/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code + "/" + "ContractCard" + "/" + Path.GetFileName(imgContractCard.FileName);
                                imgContractCard.SaveAs(HttpContext.Server.MapPath(CardPath));
                                cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Contract_Card = CardPath;
                            }
                            else
                            {
                                cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Contract_Card = "~/images/common/ContractCard.png";
                            }
                        }
                        if (Directory.Exists(FolderCondition))
                        {
                            if (ImgContractConditions.FileName.Length > 0)
                            {
                                ConditionsPath = "~/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code + "/" + "‏‏‏‏‏‏‏‏ContractConditions" + "/" + Path.GetFileName(ImgContractConditions.FileName);
                                ImgContractConditions.SaveAs(HttpContext.Server.MapPath(ConditionsPath));
                                cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Contract_Conditions = ConditionsPath;
                            }
                            else
                            {
                                cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Contract_Conditions = "~/images/common/cont-1.png";
                            }
                        }
                    //}
                    //else
                    //{
                    //    string folderimages = Server.MapPath(string.Format("~/{0}/", "/images"));
                    //    string folderCompany = Server.MapPath(string.Format("~/{0}/", "/images/Company"));
                    //    string FolderLessor = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code));


                    //    string FolderBranchCode = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                    //        + "/" + branchcode));

                    //    string FolderLOGO = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                    //        + "/" + "LOGO"));

                    //    string FolderStamp = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                    //        + "/" + "Stamp"));

                    //    string FolderSignature = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                    //        + "/" + "ManagerSignature"));

                    //    string FolderCars = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                    //       + "/" + "Cars"));

                    //    string FolderUsers = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                    //       + "/" + "Users"));

                    //    string FolderCard = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                    //        + "/" + "ContractCard"));


                    //    string FolderCondition = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                    //        + "/" + "ContractConditions"));

                    //    if (!Directory.Exists(folderimages))
                    //    {
                    //        Directory.CreateDirectory(folderimages);
                    //    }
                    //    if (!Directory.Exists(folderCompany))
                    //    {
                    //        Directory.CreateDirectory(folderCompany);
                    //    }
                    //    if (!Directory.Exists(FolderLessor))
                    //    {
                    //        Directory.CreateDirectory(FolderLessor);
                    //    }
                    //    if (!Directory.Exists(FolderBranchCode))
                    //    {
                    //        Directory.CreateDirectory(FolderBranchCode);
                    //    }
                    //    if (!Directory.Exists(FolderLOGO))
                    //    {
                    //        Directory.CreateDirectory(FolderLOGO);
                    //    }
                    //    if (!Directory.Exists(FolderStamp))
                    //    {
                    //        Directory.CreateDirectory(FolderStamp);
                    //    }
                    //    if (!Directory.Exists(FolderSignature))
                    //    {
                    //        Directory.CreateDirectory(FolderSignature);
                    //    }
                    //    if (!Directory.Exists(FolderCars))
                    //    {
                    //        Directory.CreateDirectory(FolderCars);
                    //    }
                    //    if (!Directory.Exists(FolderUsers))
                    //    {
                    //        Directory.CreateDirectory(FolderUsers);
                    //    }

                    //    if (!Directory.Exists(FolderCard))
                    //    {
                    //        Directory.CreateDirectory(FolderCard);
                    //    }

                       
                    //    if (!Directory.Exists(FolderCondition))
                    //    {
                    //        Directory.CreateDirectory(FolderCondition);
                    //    }
                    //}


                    ////////////////////////////save lessor//////////////////////////////////////////
                    ///
                    if (logopath.Length <= 100)
                    {
                        cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Logo = logopath;
                    }
                    if (stamppath.Length <= 100)
                    {
                        cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Stamp = stamppath;
                    }
                    if (signaturepath.Length <= 100)
                    {
                        cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Signature_Director = signaturepath;
                    }

                    cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Sector = "2";
                    cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code = GetLastRecord().CR_Mas_Com_Lessor_Code;
                    db.CR_Mas_Com_Lessor.Add(cR_Mas_Com_Lessor);
                    /////////////////////////////////////////////////////////////////////////////////
                    ///
                    ////////////////////////////Save beneficiary////////////////////////////////////
                    CR_Cas_Sup_Beneficiary beneficiary = new CR_Cas_Sup_Beneficiary();
                    beneficiary.CR_Cas_Sup_Beneficiary_Lessor = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code;
                    beneficiary.CR_Cas_Sup_Beneficiary_Code = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Government_No;
                    beneficiary.CR_Cas_Sup_Beneficiary_Ar_Long_Name = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Long_Name;
                    beneficiary.CR_Cas_Sup_Beneficiary_En_Long_Name = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Long_Name;
                    beneficiary.CR_Cas_Sup_Beneficiary_Fr_Long_Name = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Long_Name;
                    beneficiary.CR_Cas_Sup_Beneficiary_Commercial_Registration_No = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Commercial_Registration_No;
                    beneficiary.CR_Cas_Sup_Beneficiary_Sector = "2";
                    beneficiary.CR_Cas_Sup_Beneficiary_Status = "A";
                    db.CR_Cas_Sup_Beneficiary.Add(beneficiary);
                    ////////////////////////////////////////////////////////////////////////////////
                    ///
                    ////////////////////////////Save Owners////////////////////////////////////
                    CR_Cas_Sup_Owners Owners = new CR_Cas_Sup_Owners();
                    Owners.CR_Cas_Sup_Owners_Lessor_Code = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code;
                    Owners.CR_Cas_Sup_Owners_Code = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Government_No;
                    Owners.CR_Cas_Sup_Owners_Ar_Long_Name = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Long_Name;
                    Owners.CR_Cas_Sup_Owners_En_Long_Name = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Long_Name;
                    Owners.CR_Cas_Sup_Owners_Fr_Long_Name = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Long_Name;
                    Owners.CR_Cas_Sup_Owners_Commercial_Registration_No = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Commercial_Registration_No;
                    Owners.CR_Cas_Sup_Owners_Sector = "2";
                    Owners.CR_Cas_Sup_Owners_Status = "A";
                    db.CR_Cas_Sup_Owners.Add(Owners);
                    ////////////////////////////////////////////////////////////////////////////////
                    ///
                    ////////////////////////////save branch/////////////////////////////////////////
                    CR_Cas_Sup_Branch cR_Cas_Sup_Branch = new CR_Cas_Sup_Branch();
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Ar_Name = "الفرع الرئيسي";// cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Long_Name;
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Ar_Short_Name = "الرئيسي";// cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Short_Name;
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_En_Name = "Main branch";// cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Long_Name;
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_En_Short_Name = "Main";// cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Short_Name;
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Fr_Name = "Filiale Principale";// cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Long_Name;
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Fr_Short_Name = "Principale";// cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Short_Name;
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code = branchcode.ToString();
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Commercial_Registration_No = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Commercial_Registration_No;
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Government_No = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Government_No;
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Mobile = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Communication_Officer_Mobile;
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Signature_Ar_Director_Name = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Signature_Ar_Director_Name;
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Signature_En_Director_Name = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Signature_En_Director_Name;
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Signature_Fr_Director_Name = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Signature_Fr_Director_Name;
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_LogoMap = logopath;
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Signature_Director = signaturepath;
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Stamp = stamppath;
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Status = "A";
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Tax_No = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Tax_Number;
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Tel = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Tolk_Free;
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Lessor_Code = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code;
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Duration_One_Start_Time = TimeSpan.Parse("09:00");
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Duration_One_End_Time = TimeSpan.Parse("17:00");
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Duration_Tow_Start_Time = TimeSpan.Parse("09:00");
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Duration_Tow_End_Time = TimeSpan.Parse("17:00");
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Duration_Three_Start_Time = TimeSpan.Parse("09:00");
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Duration_Three_End_Time = TimeSpan.Parse("17:00");
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_End_Day = 30;
                    cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Start_Day = 1;
                    db.CR_Cas_Sup_Branch.Add(cR_Cas_Sup_Branch);
                    db.SaveChanges();
                    /////////////////////////////////////////////////////////////////////////////////
                    ///
                    /////////////////////////////Save User////////////////////////////////////////
                    CR_Cas_User_Information userinfo = new CR_Cas_User_Information();
                    userinfo.CR_Cas_User_Information_Id = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code;
                    userinfo.CR_Cas_User_Information_PassWord = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code;
                    userinfo.CR_Cas_User_Information_Lessor_Code = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code;
                    userinfo.CR_Cas_User_Information_Auth_Branch = true;
                    userinfo.CR_Cas_User_Information_Auth_System = true;
                    userinfo.CR_Cas_User_Information_Branch_Code = branchcode;
                    userinfo.CR_Cas_User_Information_Ar_Name = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Communication_Ar_Officer_Name;
                    userinfo.CR_Cas_User_Information_En_Name = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Communication_En_Officer_Name;
                    userinfo.CR_Cas_User_Information_Fr_Name = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Communication_Fr_Officer_Name;
                    userinfo.CR_Cas_User_Information_Emaile = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Communication_Officer_Emaile;
                    userinfo.CR_Cas_User_Information_Mobile = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Communication_Officer_Mobile;
                    userinfo.CR_Cas_User_Information_Signature = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Signature_Director;
                    var userimg = "~/images/common/user.jpg";
                    var UserSignature= "~/images/common/DefualtUserSignature.png";
                    userinfo.CR_Cas_User_Information_Image = userimg;
                    userinfo.CR_Cas_User_Information_Signature = UserSignature;
                    userinfo.CR_Cas_User_Information_Status = "A";
                    db.CR_Cas_User_Information.Add(userinfo);
                    //////////////////////////////////////////////////////////////////////////////


                    ////////////////////////////////Save Documents////////////////////////////////
                    var procedure = db.CR_Mas_Sup_Procedures.Where(x => x.CR_Mas_Sup_Procedures_Type == "1" && x.CR_Mas_Sup_Procedures_Status != "D");
                    //int nbr = procedure.Count();
                    List<CR_Cas_Sup_Branch_Documentation> ldoc = new List<CR_Cas_Sup_Branch_Documentation>();
                    CR_Cas_Sup_Branch_Documentation doc;
                    foreach (var p in procedure)
                    {
                        doc = new CR_Cas_Sup_Branch_Documentation();
                        doc.CR_Cas_Sup_Branch_Documentation_Lessor_Code = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code;
                        doc.CR_Cas_Sup_Branch_Documentation_Branch_Code = branchcode;
                        doc.CR_Cas_Sup_Branch_Documentation_Code = p.CR_Mas_Sup_Procedures_Code;
                        doc.CR_Cas_Sup_Branch_Documentation_Activation = true;
                        doc.CR_Cas_Sup_Procedures_Type = p.CR_Mas_Sup_Procedures_Type;
                        doc.CR_Cas_Sup_Branch_Documentation_Credit_Limit = 0;

                        doc.CR_Cas_Sup_Branch_Documentation_Status = "N";
                        ldoc.Add(doc);
                    }
                    ldoc.ForEach(d => db.CR_Cas_Sup_Branch_Documentation.Add(d));
                    /// //////////////////////////////////////////////////////////////////////////
                    /// 
                    ////////////////////////////////Save Cas Contracts////////////////////////////////
                    var proc = db.CR_Mas_Sup_Procedures.Where(x => x.CR_Mas_Sup_Procedures_Type == "2" && x.CR_Mas_Sup_Procedures_Status != "D");
                    
                    List<CR_Cas_Company_Contract> lCasContract = new List<CR_Cas_Company_Contract>();
                    CR_Cas_Company_Contract cont;
                    foreach (var p in proc)
                    {
                        DateTime year = DateTime.Now;
                        var y = year.ToString("yy");
                        cont = new CR_Cas_Company_Contract();
                        cont.CR_Cas_Company_Contract_No = y + "-1-" + p.CR_Mas_Sup_Procedures_Code + "-" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code + "-0000001";
                        cont.CR_Cas_Company_Contract_Lessor = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code;
                       
                        cont.CR_Cas_Company_Contract_Code = p.CR_Mas_Sup_Procedures_Code;
                        cont.CR_Cas_Company_Contract_Year = y;
                        cont.CR_Cas_Company_Contract_Sector = "1";
                        
                        cont.CR_Cas_Company_Contract_Number = null;
                        cont.CR_Cas_Company_Contract_Date = null;
                        cont.CR_Cas_Company_Contract_Start_Date = null;
                        cont.CR_Cas_Company_Contract_End_Date = null;
                        cont.CR_Cas_Company_Contract_Status = "N";
                        lCasContract.Add(cont);
                    }
                    lCasContract.ForEach(c => db.CR_Cas_Company_Contract.Add(c));
                    /// //////////////////////////////////////////////////////////////////////////
                    /// 
                    /// ////////////////////////////////Save mechanism///////////////////////////
                    var procedures = db.CR_Mas_Sup_Procedures.Where(x => (x.CR_Mas_Sup_Procedures_Type == "1" || x.CR_Mas_Sup_Procedures_Type == "2"
                    || x.CR_Mas_Sup_Procedures_Type == "3" || x.CR_Mas_Sup_Procedures_Type == "4" || x.CR_Mas_Sup_Procedures_Type == "B") && x.CR_Mas_Sup_Procedures_Status != "D");
                    //int q = procedures.Count();
                    List<CR_Cas_Sup_Follow_Up_Mechanism> lmech = new List<CR_Cas_Sup_Follow_Up_Mechanism>();
                    CR_Cas_Sup_Follow_Up_Mechanism mech;
                    foreach (var p in procedures)
                    {
                        mech = new CR_Cas_Sup_Follow_Up_Mechanism();
                        mech.CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code;
                        mech.CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code = p.CR_Mas_Sup_Procedures_Code;
                        mech.CR_Cas_Sup_Follow_Up_Mechanism_Activate_Service = true;

                        mech.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Credit_Limit = p.CR_Mas_Sup_Procedures_FollowUp_Befor_Credit_Limit;
                        mech.CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire = p.CR_Mas_Sup_Procedures_FollowUp_Befor_Expire;
                        mech.CR_Cas_Sup_Follow_Up_Mechanism_Default_KM = p.CR_Mas_Sup_Procedures_FollowUp_Default_KM;
                        mech.CR_Cas_Sup_Follow_Up_Mechanism_Befor_KM = p.CR_Mas_Sup_Procedures_FollowUp_Befor_KM;
                        mech.CR_Cas_Sup_Follow_Up_Mechanism_After_Expire = p.CR_Mas_Sup_Procedures_FollowUp_Befor_Expire_KM;
                        mech.CR_Cas_Sup_Procedures_Type = p.CR_Mas_Sup_Procedures_Type;
                        lmech.Add(mech);
                    }
                    lmech.ForEach(m => db.CR_Cas_Sup_Follow_Up_Mechanism.Add(m));
                    /////////////////////////////////////////////////////////////////////////////////
                    // تعديل عروض فترة الى 10 ايام
                    var pc = db.CR_Mas_Sup_Procedures.Single(p=>p.CR_Mas_Sup_Procedures_Code=="33");
                    if (pc != null)
                    {
                        pc.CR_Mas_Sup_Procedures_FollowUp_Befor_Expire = 10;
                        db.Entry(pc).State = EntityState.Modified;
                    }
                    /////////////////////////////////Save membership/////////////////////////////////
                    var membership = db.CR_Mas_Sup_Membership.Where(x => x.CR_Mas_Sup_Membership_Status != "D");

                    List<CR_Cas_Sup_Membership_Conditions> lmember = new List<CR_Cas_Sup_Membership_Conditions>();
                    CR_Cas_Sup_Membership_Conditions member;
                    foreach (var m in membership)
                    {
                        member = new CR_Cas_Sup_Membership_Conditions();
                        member.CR_Cas_Sup_Membership_Conditions_Lessor_Code = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code;
                        member.CR_Cas_Sup_Membership_Conditions_Code = m.CR_Mas_Sup_Membership_Code;
                        member.CR_Cas_Sup_Membership_Conditions_Insert = false;
                        member.CR_Cas_Sup_Membership_Conditions_KM = 0;
                        member.CR_Cas_Sup_Membership_Conditions_Link_1 = 0;
                        member.CR_Cas_Sup_Membership_Conditions_Link_2 = 0;
                        member.CR_Cas_Sup_Membership_Conditions_Amount = 0;
                        member.CR_Cas_Sup_Membership_Conditions_Contract_No = 0;
                        member.CR_Cas_Sup_Membership_Conditions_Insert_No = 0;
                        lmember.Add(member);
                    }
                    lmember.ForEach(m => db.CR_Cas_Sup_Membership_Conditions.Add(m));
                    //////////////////////////////////////////////////////////////////////////////////
                    ///
                    /// //////////////////////////////////save Address////////////////////////////////
                    /// 
                    CR_Mas_Address adr = new CR_Mas_Address();
                    adr.CR_Mas_Address_Id_Code = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Government_No;
                    adr.CR_Mas_Address_Regions = regioncode;
                    adr.CR_Mas_Address_City = CR_MAS_SUP_City_Code;
                    if (CR_Mas_Address_Additional_Numbers != null && CR_Mas_Address_Additional_Numbers != "")
                    {
                        adr.CR_Mas_Address_Additional_Numbers = int.Parse(CR_Mas_Address_Additional_Numbers);
                    }

                    adr.CR_Mas_Address_Ar_District = CR_Mas_Address_Ar_District;
                    adr.CR_Mas_Address_Ar_Street = CR_Mas_Address_Ar_Street;
                    if (CR_Mas_Address_Building != null && CR_Mas_Address_Building != "")
                    {
                        adr.CR_Mas_Address_Building = int.Parse(CR_Mas_Address_Building);
                    }

                    adr.CR_Mas_Address_En_District = CR_Mas_Address_En_District;
                    adr.CR_Mas_Address_En_Street = CR_Mas_Address_En_Street;
                    adr.CR_Mas_Address_Fr_District = CR_Mas_Address_Fr_District;
                    adr.CR_Mas_Address_Fr_Street = CR_Mas_Address_Fr_Street;
                    adr.CR_Mas_Address_Status = "A";
                    adr.CR_Mas_Address_Unit_No = CR_Mas_Address_Unit_No;
                    if (CR_Mas_Address_Zip_Code != null && CR_Mas_Address_Zip_Code != "")
                    {
                        adr.CR_Mas_Address_Zip_Code = int.Parse(CR_Mas_Address_Zip_Code);
                    }

                    //adr.CR_Mas_Address_Additional_Numbers = int.Parse(CR_Mas_Address_Additional_Numbers);
                    adr.CR_Mas_Address_UpDate_Post = DateTime.Now;
                    db.CR_Mas_Address.Add(adr);
                    /////////////////////////////////////////////////////////////////////////////////

                    ////////////////////////////////Save MainAccount////////////////////////////////
                    var MainAccount = db.CR_Mas_Sup_Main_Account.Where(x => x.CR_Mas_Sup_Main_Account_Status != "D");
                    //int nbr = procedure.Count();
                    List<CR_Cas_Sup_Main_Com_Account> lMainAccount = new List<CR_Cas_Sup_Main_Com_Account>();
                    CR_Cas_Sup_Main_Com_Account CasAccount;
                    foreach (var a in MainAccount)
                    {
                        CasAccount = new CR_Cas_Sup_Main_Com_Account();
                        CasAccount.CR_Cas_Sup_Main_Com_Account_Code = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code + a.CR_Mas_Sup_Main_Account_Code;
                        CasAccount.CR_Cas_Sup_Main_Com_Account_Main_Code = a.CR_Mas_Sup_Main_Account_Code;
                        CasAccount.CR_Cas_Sup_Main_Com_Account_Com_Code = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code;
                        CasAccount.CR_Cas_Sup_Main_Com_Account_Balance = 0;
                        CasAccount.CR_Cas_Sup_Main_Com_Account_Status = "A";
                        lMainAccount.Add(CasAccount);
                    }
                    lMainAccount.ForEach(d => db.CR_Cas_Sup_Main_Com_Account.Add(d));
                    /// //////////////////////////////////////////////////////////////////////////
                    ////////////////////////////////Save MainAccount into branch////////////////////////////////
                    var BrnMainAccount = db.CR_Mas_Sup_Main_Account.Where(x => x.CR_Mas_Sup_Main_Account_Status != "D");
                    //int nbr = procedure.Count();
                    List<CR_Cas_Sup_Main_Brn_Account> lBrnMainAccount = new List<CR_Cas_Sup_Main_Brn_Account>();
                    CR_Cas_Sup_Main_Brn_Account MasBrnAccount;
                    foreach (var a in BrnMainAccount)
                    {
                        MasBrnAccount = new CR_Cas_Sup_Main_Brn_Account();
                        MasBrnAccount.CR_Cas_Sup_Main_Brn_Account_Code = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code + "100" + a.CR_Mas_Sup_Main_Account_Code;
                        MasBrnAccount.CR_Cas_Sup_Main_Brn_Account_Main_Code = a.CR_Mas_Sup_Main_Account_Code;
                        MasBrnAccount.CR_Cas_Sup_Main_Brn_Account_Com_Code = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code;
                        MasBrnAccount.CR_Cas_Sup_Main_Brn_Account_Brn_Code = "100";
                        MasBrnAccount.CR_Cas_Sup_Main_Brn_Account_Balance = 0;
                        MasBrnAccount.CR_Cas_Sup_Main_Brn_Account_Status = "A";
                        lBrnMainAccount.Add(MasBrnAccount);
                    }
                    lBrnMainAccount.ForEach(d => db.CR_Cas_Sup_Main_Brn_Account.Add(d));
                    /// //////////////////////////////////////////////////////////////////////////
                    ////////////////////////////////Save SubAccount////////////////////////////////
                    var SubAccount = db.CR_Mas_Sup_Sub_Account.Where(x => x.CR_Mas_Sup_Sub_Account_Status != "D");
                    //int nbr = procedure.Count();
                    List<CR_Cas_Sup_Sub_Com_Account> lSubAccount = new List<CR_Cas_Sup_Sub_Com_Account>();
                    CR_Cas_Sup_Sub_Com_Account CasSubAccount;
                    foreach (var a in SubAccount)
                    {
                        CasSubAccount = new CR_Cas_Sup_Sub_Com_Account();
                        CasSubAccount.CR_Cas_Sup_Sub_Com_Account_Code = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code + a.CR_Mas_Sup_Sub_Account_Code;
                        CasSubAccount.CR_Cas_Sup_Sub_Com_Account_Sub_Code = a.CR_Mas_Sup_Sub_Account_Code;
                        CasSubAccount.CR_Cas_Sup_Sub_Com_Account_Com_Code = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code;
                        CasSubAccount.CR_Cas_Sup_Sub_Com_Account_Balance = 0;
                        CasSubAccount.CR_Cas_Sup_Sub_Com_Account_Status = "A";
                        lSubAccount.Add(CasSubAccount);
                    }
                    lSubAccount.ForEach(d => db.CR_Cas_Sup_Sub_Com_Account.Add(d));
                    /// //////////////////////////////////////////////////////////////////////////
                    ////////////////////////////////Save BrnSubAccount////////////////////////////////
                    var BrnSubAccount = db.CR_Mas_Sup_Sub_Account.Where(x => x.CR_Mas_Sup_Sub_Account_Status != "D");
                    //int nbr = procedure.Count();
                    List<CR_Cas_Sup_Sub_Brn_Account> lBrnSubAccount = new List<CR_Cas_Sup_Sub_Brn_Account>();
                    CR_Cas_Sup_Sub_Brn_Account CasSubBrnAccount;
                    foreach (var a in BrnSubAccount)
                    {
                        CasSubBrnAccount = new CR_Cas_Sup_Sub_Brn_Account();
                        CasSubBrnAccount.CR_Cas_Sup_Sub_Brn_Account_Code = cR_Cas_Sup_Branch.CR_Cas_Sup_Lessor_Code + "100" + a.CR_Mas_Sup_Sub_Account_Code;
                        CasSubBrnAccount.CR_Cas_Sup_Sub_Brn_Account_Sub_Code = a.CR_Mas_Sup_Sub_Account_Code;
                        CasSubBrnAccount.CR_Cas_Sup_Sub_Brn_Account_Com_Code = cR_Cas_Sup_Branch.CR_Cas_Sup_Lessor_Code;
                        CasSubBrnAccount.CR_Cas_Sup_Sub_Brn_Account_Brn_Code = cR_Cas_Sup_Branch.CR_Cas_Sup_Branch_Code;
                        CasSubBrnAccount.CR_Cas_Sup_Sub_Brn_Account_Balance = 0;
                        CasSubBrnAccount.CR_Cas_Sup_Sub_Brn_Account_Status = "A";
                        lBrnSubAccount.Add(CasSubBrnAccount);
                    }
                    lBrnSubAccount.ForEach(d => db.CR_Cas_Sup_Sub_Brn_Account.Add(d));
                    /// //////////////////////////////////////////////////////////////////////////
                    ////////////////////////////////Save bank cash////////////////////////////////
                    CR_Cas_Sup_Bank b = new CR_Cas_Sup_Bank();
                    b.CR_Cas_Sup_Bank_Code = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code+"0000";
                    b.CR_Cas_Sup_Bank_Com_Code = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code;
                    b.CR_Cas_Sup_Bank_Mas_Code = "00";
                    b.CR_Cas_Sup_Bank_Account_No = null;
                    b.CR_Cas_Sup_Bank_Ar_Name = "الصندوق" + "/" + "الفرع الرئيسي";
                    b.CR_Cas_Sup_Bank_En_Name = "Fund";
                    b.CR_Cas_Sup_Bank_Status = "A";
                    db.CR_Cas_Sup_Bank.Add(b);
                    /// //////////////////////////////////////////////////////////////////////////
                    /// 
                    ////////////////////////////////Save Sales Point cash////////////////////////////////
                    CR_Cas_Sup_SalesPoint SalesPoint = new CR_Cas_Sup_SalesPoint();
                    SalesPoint.CR_Cas_Sup_SalesPoint_Code = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code + "100" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code + "000000";
                    SalesPoint.CR_Cas_Sup_SalesPoint_Com_Code = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code;
                    SalesPoint.CR_Cas_Sup_SalesPoint_Brn_Code = "100";
                    SalesPoint.CR_Cas_Sup_SalesPoint_Bank_Code = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code + "0000";
                    SalesPoint.CR_Cas_Sup_SalesPoint_Bank_No = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code + "0000";
                    SalesPoint.CR_Cas_Sup_SalesPoint_Ar_Name = "الصندوق" + "/" + "الفرع الرئيسي";
                    SalesPoint.CR_Cas_Sup_SalesPoint_En_Name = "Fund";
                    SalesPoint.CR_Cas_Sup_SalesPoint_Balance = 0;
                    SalesPoint.CR_Cas_Sup_SalesPoint_Status = "A";
                    db.CR_Cas_Sup_SalesPoint.Add(SalesPoint);
                    /// //////////////////////////////////////////////////////////////////////////

                    db.SaveChanges();
                    cR_Mas_Com_Lessor = new CR_Mas_Com_Lessor();
                    cR_Mas_Com_Lessor = GetLastRecord();
                    cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status = "A";
                    TempData["TempModel"] = "تم الحفظ بنجاح";
                    return RedirectToAction("Create", "Lessor");
                    //////////////////////////////////////////////////////////////////////////////////
                    ///


                }
                else
                {
                    if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Long_Name == null)
                        ViewBag.LRExistArLng = "الرجاء إدخال بيانات الحقل";
                    if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Short_Name == null)
                        ViewBag.LRExistArSh = "الرجاء إدخال بيانات الحقل";

                    if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Long_Name == null)
                        ViewBag.LRExistEnLng = "الرجاء إدخال بيانات الحقل";
                    if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Short_Name == null)
                        ViewBag.LRExistEnSh = "الرجاء إدخال بيانات الحقل";

                    if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Long_Name == null)
                        ViewBag.LRExistFrLng = "الرجاء إدخال بيانات الحقل";
                    if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Short_Name == null)
                        ViewBag.LRExistFrSh = "الرجاء إدخال بيانات الحقل";


                    if (LrecordExitArabeLong)
                        ViewBag.LRExistArLng = "عفوا هذه الشركة موجودة";
                    if (LrecordExitArabeShort)
                        ViewBag.LRExistArSh = "عفوا هذه الشركة موجودة";


                    if (LrecordExitEnglishLong)
                        ViewBag.LRExistEnLng = "عفوا هذه الشركة موجودة";
                    if (LrecordExitEnglishShort)
                        ViewBag.LRExistEnSh = "عفوا هذه الشركة موجودة";


                    if (LrecordExitFrenchLong)
                        ViewBag.LRExistFrLng = "عفوا هذه الشركة موجودة";
                    if (LrecordExitFrenchShort)
                        ViewBag.LRExistFrSh = "عفوا هذه الشركة موجودة";

                    if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Government_No == null)
                        ViewBag.GovNumber = "الرجاء إدخال بيانات الحقل";
                    if (gov == true)
                        ViewBag.GovNumber = "الرقم الحكومي متكرر";

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


                    if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Long_Name != null && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Long_Name.Length < 3)
                        ViewBag.LRExistArLng = "عفوا الحد الأدنى للحروف 3";
                    if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Short_Name != null && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Short_Name.Length < 2)
                        ViewBag.LRExistArSh = "عفوا الحد الأدنى للحروف 2";


                    if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Long_Name != null && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Long_Name.Length < 3)
                        ViewBag.LRExistEnLng = "عفوا الحد الأدنى للحروف 3";
                    if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Short_Name != null && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Short_Name.Length < 2)
                        ViewBag.LRExistEnSh = "عفوا الحد الأدنى للحروف 2";


                    if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Long_Name != null && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Long_Name.Length < 2)
                        ViewBag.LRExistFrSh = "عفوا الحد الأدنى للحروف 2";
                    if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Short_Name != null && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Short_Name.Length < 3)
                        ViewBag.LRExistFrLng = "عفوا الحد الأدنى للحروف 3";
                }
            }
            ViewBag.CR_MAS_SUP_City_Code = new SelectList(db.CR_Mas_Sup_City.Where(x => x.CR_Mas_Sup_Regions.CR_Mas_Sup_Regions_Status != "D"), "CR_Mas_Sup_City_Code", "CR_Mas_Sup_City_Ar_Name");
            return View(cR_Mas_Com_Lessor);
        }

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

        // GET: Lessor/Edit/5
        public ActionResult Edit(string id1, string id2)
        {
            if (id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Com_Lessor cR_Mas_Com_Lessor = db.CR_Mas_Com_Lessor.Find(id1);
            if (cR_Mas_Com_Lessor == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status == "A" ||
                    cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status == "Activated" ||
                    cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status == "1" ||
                    cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status == "Undeleted")
                {
                    ViewBag.stat = "حذف";
                    ViewBag.h = "إيقاف";
                }

                if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status == "D" ||
                    cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status == "Deleted" ||
                    cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status == "0")
                {
                    ViewBag.stat = "إسترجاع";
                    ViewBag.h = "إيقاف";
                    ViewData["ReadOnly"] = "true";
                }

                if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status == "H" ||
                    cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status == "Hold" ||
                    cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status == "2")
                {
                    ViewBag.h = "تنشيط";
                    ViewBag.stat = "حذف";
                    ViewData["ReadOnly"] = "true";
                }

                if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status == null)
                {
                    ViewBag.h = "إيقاف";
                    ViewBag.stat = "حذف";
                }


                if (id2 != "")
                {
                    CR_Mas_Address cR_Mas_Address = db.CR_Mas_Address.FirstOrDefault(x => x.CR_Mas_Address_Id_Code == id2);
                    if (String.IsNullOrEmpty(cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Logo))
                    {
                        cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Logo = null;
                    }
                    if (String.IsNullOrEmpty(cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Signature_Director))
                    {
                        cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Signature_Director = null;
                    }
                    if (String.IsNullOrEmpty(cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Stamp))
                    {
                        cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Stamp = null;
                    }
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

                    var q = db.CR_Mas_Sup_Regions.FirstOrDefault(x => x.CR_Mas_Sup_Regions_Code == cR_Mas_Address.CR_Mas_Address_Regions);
                    ViewBag.Region = q.CR_Mas_Sup_Regions_Ar_Name;
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                //ViewBag.Region = db.CR_Mas_Sup_Regions.FirstOrDefault(r => r.CR_Mas_Sup_Regions_Code == cR_Mas_Address.CR_Mas_Address_Regions).CR_Mas_Sup_Regions_Ar_Name.ToString();

                ViewBag.delete = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status;

            }
            var CityCode = "";
            var Address = db.CR_Mas_Address.FirstOrDefault(a=>a.CR_Mas_Address_Id_Code==cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Government_No);
            if (Address != null)
            {
                CityCode = Address.CR_Mas_Address_City;
            }
            ViewBag.CR_MAS_SUP_City_Code = new SelectList(db.CR_Mas_Sup_City.Where(x => x.CR_Mas_Sup_Regions.CR_Mas_Sup_Regions_Status != "D"),
                "CR_Mas_Sup_City_Code", "CR_Mas_Sup_City_Ar_Name",CityCode);
            return View(cR_Mas_Com_Lessor);
        }

        // POST: Lessor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Mas_Com_Lessor_Code,CR_Mas_Com_Lessor_Logo,CR_Mas_Com_Lessor_Ar_Long_Name,CR_Mas_Com_Lessor_Ar_Short_Name" +
            ",CR_Mas_Com_Lessor_En_Long_Name,CR_Mas_Com_Lessor_En_Short_Name,CR_Mas_Com_Lessor_Fr_Long_Name,CR_Mas_Com_Lessor_Fr_Short_Name," +
            "CR_Mas_Com_Lessor_Location_Coordinates,CR_Mas_Com_Lessor_Commercial_Registration_No,CR_Mas_Com_Lessor_Sector," +
            "CR_Mas_Com_Lessor_Government_No,CR_Mas_Com_Lessor_Tax_Number,CR_Mas_Com_Lessor_Communication_Ar_Officer_Name," +
            "CR_Mas_Com_Lessor_Communication_En_Officer_Name,CR_Mas_Com_Lessor_Communication_Fr_Officer_Name" +
            ",CR_Mas_Com_Lessor__Communication_Officer_Emaile,CR_Mas_Com_Lessor_Communication_Officer_Mobile" +
            ",CR_Mas_Com_Lessor_Tolk_Free,CR_Mas_Com_Lessor_Email,CR_Mas_Com_Lessor_Twiter,CR_Mas_Com_Lessor_FaceBook,CR_Mas_Com_Lessor_Instagram" +
            ",CR_Mas_Com_Lessor_Stamp,CR_Mas_Com_Lessor_Signature_Director,CR_Mas_Com_Lessor_Signature_Ar_Director_Name" +
            ",CR_Mas_Com_Lessor_Signature_En_Director_Name,CR_Mas_Com_Lessor_Signature_Fr_Director_Name," +
            "CR_Mas_Com_Lessor_Status,CR_Mas_Com_Lessor_Reasons")] CR_Mas_Com_Lessor cR_Mas_Com_Lessor, HttpPostedFileBase img,
            HttpPostedFileBase imgStamp, HttpPostedFileBase imgDirector, string CR_MAS_SUP_City_Code, string CR_MAS_SUP_Region_Code, string CR_Mas_Address_Ar_District,
            string CR_Mas_Address_En_District, string CR_Mas_Address_Fr_District, string CR_Mas_Address_Ar_Street, string CR_Mas_Address_En_Street,
            string CR_Mas_Address_Fr_Street, string CR_Mas_Address_Building, string CR_Mas_Address_Unit_No, string CR_Mas_Address_Zip_Code,
            string CR_Mas_Address_Additional_Numbers, string save, string delete, string hold, HttpPostedFileBase imgContractCard, HttpPostedFileBase ImgContractConditions)
        {
            if (!string.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    var LrecordExitArabeLong = db.CR_Mas_Com_Lessor.Any(Lr => Lr.CR_Mas_Com_Lessor_Ar_Long_Name == cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Long_Name);
                    var LrecordExitArabeShort = db.CR_Mas_Com_Lessor.Any(Lr => Lr.CR_Mas_Com_Lessor_Ar_Short_Name == cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Short_Name);

                    var LrecordExitEnglishLong = db.CR_Mas_Com_Lessor.Any(Lr => Lr.CR_Mas_Com_Lessor_En_Long_Name == cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Long_Name);
                    var LrecordExitEnglishShort = db.CR_Mas_Com_Lessor.Any(Lr => Lr.CR_Mas_Com_Lessor_En_Short_Name == cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Short_Name);

                    var LrecordExitFrenchLong = db.CR_Mas_Com_Lessor.Any(Lr => Lr.CR_Mas_Com_Lessor_Fr_Long_Name == cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Long_Name);
                    var LrecordExitFrenchShort = db.CR_Mas_Com_Lessor.Any(Lr => Lr.CR_Mas_Com_Lessor_Fr_Short_Name == cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Short_Name);

                    var gov = db.CR_Mas_Com_Lessor.Any(Lr => Lr.CR_Mas_Com_Lessor_Government_No == cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Government_No &&
                        Lr.CR_Mas_Com_Lessor_Code != cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code);

                    string branchcode = "100";

                    if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Long_Name != null && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Short_Name != null &&
                    cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Long_Name != null && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Short_Name != null &&
                    cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Long_Name != null && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Short_Name != null && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Government_No != null &&
                    cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Long_Name.Length >= 3 && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Short_Name.Length >= 2 &&
                        cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Long_Name.Length >= 3 && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Short_Name.Length >= 2 &&
                        cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Long_Name.Length >= 3 && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Short_Name.Length >= 2 &&
                        cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Government_No != null && CR_Mas_Address_Ar_District != "" && CR_Mas_Address_En_District != "" && CR_Mas_Address_Fr_District != ""
                        && CR_Mas_Address_Ar_Street != "" && CR_Mas_Address_En_Street != "" && CR_Mas_Address_Fr_Street != "" && CR_Mas_Address_Building != "" && !gov)
                    {
                       

                        string logopath = "";
                        string stamppath = "";
                        string signaturepath = "";
                        string CardPath = "";
                        string ConditionsPath = "";
                        string UserID = cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code;
                       
                            string folderimages = Server.MapPath(string.Format("~/{0}/", "/images"));
                            string folderCompany = Server.MapPath(string.Format("~/{0}/", "/images/Company"));
                            string FolderLessor = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code));
                            string FolderUsers = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code + "/" + "Users"));

                            string FolderBranchCode = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                                + "/" + branchcode));

                            string FolderLOGO = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                                + "/" + "Logo"));

                            string FolderStamp = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                                + "/" + "Stamp"));

                            string FolderSignature = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                                + "/" + "ManagerSignature"));


                            string FolderCarLicense = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                                + "/" + "CarLicense"));

                            string FolderCard = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                                + "/" + "ContractCard"));


                            string FolderCondition = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                                + "/" + "ContractConditions"));
                            string FolderMembership = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                                + "/" + "Membership"));


                            string FolderUsersID = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code
                               + "/" + "Users" + "/" + UserID));

                            if (!Directory.Exists(folderimages))
                            {
                                Directory.CreateDirectory(folderimages);
                            }
                            if (!Directory.Exists(FolderUsers))
                            {
                                Directory.CreateDirectory(FolderUsers);
                            }
                            if (!Directory.Exists(folderCompany))
                            {
                                Directory.CreateDirectory(folderCompany);
                            }
                            if (!Directory.Exists(FolderLessor))
                            {
                                Directory.CreateDirectory(FolderLessor);
                            }
                            
                            
                            if (!Directory.Exists(FolderLOGO))
                            {
                                Directory.CreateDirectory(FolderLOGO);
                            }
                            if (!Directory.Exists(FolderStamp))
                            {
                                Directory.CreateDirectory(FolderStamp);
                            }
                            if (!Directory.Exists(FolderSignature))
                            {
                                Directory.CreateDirectory(FolderSignature);
                            }

                            if (!Directory.Exists(FolderCard))
                            {
                                Directory.CreateDirectory(FolderCard);
                            }

                            if (!Directory.Exists(FolderCarLicense))
                            {
                                Directory.CreateDirectory(FolderCarLicense);
                            }

                            if (!Directory.Exists(FolderCondition))
                            {
                                Directory.CreateDirectory(FolderCondition);
                            }

                            if (!Directory.Exists(FolderMembership))
                            {
                                Directory.CreateDirectory(FolderMembership);
                            }

                            if (imgDirector != null)
                            {
                                if (!Directory.Exists(FolderBranchCode))
                                {
                                    Directory.CreateDirectory(FolderBranchCode);
                                    if (imgDirector.FileName.Length > 0)
                                    {
                                        signaturepath = "~/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code + "/" + branchcode + "/" + Path.GetFileName(imgDirector.FileName);
                                        imgDirector.SaveAs(HttpContext.Server.MapPath(signaturepath));
                                        cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Logo = signaturepath;
                                    }
                                }
                                if (!Directory.Exists(FolderUsersID))
                                {
                                    Directory.CreateDirectory(FolderUsersID);
                                    if (imgDirector.FileName.Length > 0)
                                    {
                                        signaturepath = "~/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code + "/" + "Users" + "/" + UserID + "/" + Path.GetFileName(imgDirector.FileName);
                                        imgDirector.SaveAs(HttpContext.Server.MapPath(signaturepath));
                                        cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Logo = signaturepath;
                                    }
                                }
                                if (Directory.Exists(FolderSignature))
                                {
                                    if (imgDirector.FileName.Length > 0)
                                    {
                                        signaturepath = "~/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code + "/" + "ManagerSignature" + "/" + Path.GetFileName(imgDirector.FileName);
                                        imgDirector.SaveAs(HttpContext.Server.MapPath(signaturepath));
                                        cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Signature_Director = signaturepath;
                                    }
                                }
                            }
                            if (img != null)
                            {
                                if (Directory.Exists(FolderLOGO))
                                {
                                    if (img.FileName.Length > 0)
                                    {
                                        logopath = "~/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code + "/" + "LOGO" + "/" + Path.GetFileName(img.FileName);

                                        img.SaveAs(HttpContext.Server.MapPath(logopath));
                                        cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Logo = logopath;
                                    }
                                }
                            }
                            if(imgStamp != null)
                            {
                                if (Directory.Exists(FolderStamp))
                                {
                                    if (imgStamp.FileName.Length > 0)
                                    {
                                        stamppath = "~/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code + "/" + "Stamp" + "/" + Path.GetFileName(imgStamp.FileName);
                                        imgStamp.SaveAs(HttpContext.Server.MapPath(stamppath));
                                        cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Stamp = stamppath;
                                    }
                                }
                            }


                            if (imgContractCard != null)
                            {
                                if (Directory.Exists(FolderCard))
                                {
                                    if (imgContractCard.FileName.Length > 0)
                                    {
                                        CardPath = "~/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code + "/" + "ContractCard" + "/" + Path.GetFileName(imgContractCard.FileName);
                                        imgContractCard.SaveAs(HttpContext.Server.MapPath(CardPath));
                                        cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Contract_Card = CardPath;
                                    }
                                }
                            }
                            if (ImgContractConditions != null)
                            {
                                if (Directory.Exists(FolderCondition))
                                {
                                    if (ImgContractConditions.FileName.Length > 0)
                                    {
                                        ConditionsPath = "~/images/Company/" + cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Code + "/" + "ContractConditions" + "/" + Path.GetFileName(ImgContractConditions.FileName);
                                        ImgContractConditions.SaveAs(HttpContext.Server.MapPath(ConditionsPath));
                                        cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Contract_Conditions = ConditionsPath;
                                    }
                                }
                            }

                        db.Entry(cR_Mas_Com_Lessor).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Long_Name == null)
                            ViewBag.LRExistArLng = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Short_Name == null)
                            ViewBag.LRExistArSh = "الرجاء إدخال بيانات الحقل";

                        if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Long_Name == null)
                            ViewBag.LRExistEnLng = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Short_Name == null)
                            ViewBag.LRExistEnSh = "الرجاء إدخال بيانات الحقل";

                        if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Long_Name == null)
                            ViewBag.LRExistFrLng = "الرجاء إدخال بيانات الحقل";
                        if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Short_Name == null)
                            ViewBag.LRExistFrSh = "الرجاء إدخال بيانات الحقل";

                        if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Government_No == null)
                            ViewBag.GovNumber = "الرجاء إدخال بيانات الحقل";
                        if (gov == true)
                            ViewBag.GovNumber = "الرقم الحكومي متكرر";

                        if (LrecordExitArabeLong)
                            ViewBag.LRExistArLng = "عفوا هذه الشركة موجودة";
                        if (LrecordExitArabeShort)
                            ViewBag.LRExistArSh = "عفوا هذه الشركة موجودة";


                        if (LrecordExitEnglishLong)
                            ViewBag.LRExistEnLng = "عفوا هذه الشركة موجودة";
                        if (LrecordExitEnglishShort)
                            ViewBag.LRExistEnSh = "عفوا هذه الشركة موجودة";


                        if (LrecordExitFrenchLong)
                            ViewBag.LRExistFrSh = "عفوا هذه الشركة موجودة";
                        if (LrecordExitFrenchShort)
                            ViewBag.LRExistFrLng = "عفوا هذه الشركة موجودة";


                        if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Long_Name != null && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Long_Name.Length < 3)
                            ViewBag.LRExistArLng = "عفوا الحد الأدنى للحروف 3";
                        if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Short_Name != null && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Short_Name.Length < 2)
                            ViewBag.LRExistArSh = "عفوا الحد الأدنى للحروف 2";


                        if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Long_Name != null && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Long_Name.Length < 3)
                            ViewBag.LRExistEnLng = "عفوا الحد الأدنى للحروف 3";
                        if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Short_Name != null && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_En_Short_Name.Length < 2)
                            ViewBag.LRExistEnSh = "عفوا الحد الأدنى للحروف 2";


                        if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Long_Name != null && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Long_Name.Length < 2)
                            ViewBag.LRExistFrSh = "عفوا الحد الأدنى للحروف 2";
                        if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Short_Name != null && cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Fr_Short_Name.Length < 3)
                            ViewBag.LRExistFrLng = "عفوا الحد الأدنى للحروف 3";
                    }




                }
            }
            if (delete == "Delete" || delete == "حذف")
            {
                cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status = "D";
                db.Entry(cR_Mas_Com_Lessor).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            if (delete == "Activate" || delete == "إسترجاع")
            {
                cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status = "A";
                db.Entry(cR_Mas_Com_Lessor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "إيقاف" || hold == "hold")
            {
                cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status = "H";
                db.Entry(cR_Mas_Com_Lessor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (hold == "تنشيط" || hold == "Activate")
            {
                cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status = "A";
                db.Entry(cR_Mas_Com_Lessor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status == "A" ||
            cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status == "Activated" ||
            cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status == "1" ||
            cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status == "Undeleted")
            {
                ViewBag.stat = "حذف";
                ViewBag.h = "إيقاف";
            }
            if ((cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status == "D" ||
                 cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status == "Deleted" ||
                 cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status == "0"))
            {
                ViewBag.stat = "إسترجاع";
                ViewBag.h = "إيقاف";
            }
            if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status == "H" ||
                cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status == "Hold" ||
                cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status == "2")
            {
                ViewBag.h = "تنشيط";
                ViewBag.stat = "حذف";
            }
            if (cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Status == null)
            {
                ViewBag.h = "إيقاف";
                ViewBag.stat = "حذف";
            }
            return View(cR_Mas_Com_Lessor);
        }

        // GET: Lessor/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Mas_Com_Lessor cR_Mas_Com_Lessor = db.CR_Mas_Com_Lessor.Find(id);
            if (cR_Mas_Com_Lessor == null)
            {
                return HttpNotFound();
            }
            return View(cR_Mas_Com_Lessor);
        }

        // POST: Lessor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Mas_Com_Lessor cR_Mas_Com_Lessor = db.CR_Mas_Com_Lessor.Find(id);
            db.CR_Mas_Com_Lessor.Remove(cR_Mas_Com_Lessor);
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
