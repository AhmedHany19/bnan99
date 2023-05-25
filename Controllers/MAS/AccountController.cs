using RentCar.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using RentCar.Models.ApiModel;
namespace RentCar.Controllers
{
    public class AccountController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();



        // end Authentification varaibles
        // GET: Account

        public void GetValidityContract(string Login,string BranchCode,string LessorCode)
        {
            var validity = db.CR_Cas_User_Validity_Contract.FirstOrDefault(v=>v.CR_Cas_User_Validity_Contract_User_Id==Login);
            if (validity != null)
            {
                if ((bool)(validity.CR_Cas_User_Validity_Contract_Register == true))
                {
                    Session["ContractRegister"] = "True";
                }
                else
                {
                    var doc = db.CR_Cas_Sup_Branch_Documentation.FirstOrDefault(d => d.CR_Cas_Sup_Branch_Documentation_Branch_Code == BranchCode
                    && d.CR_Cas_Sup_Branch_Documentation_Code == "10" && d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode);
                    if (doc != null)
                    {
                        if (doc.CR_Cas_Sup_Branch_Documentation_Status == "A" || doc.CR_Cas_Sup_Branch_Documentation_Status == "X")
                        {
                            Session["ContractRegister"] = "True";
                        }
                        else
                        {
                            Session["ContractRegister"] = "False";
                        }
                    }
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Chamber_Commerce == true))
                {
                    Session["ChamberCommerce"] = "True";
                }
                else
                {
                    var doc = db.CR_Cas_Sup_Branch_Documentation.FirstOrDefault(d => d.CR_Cas_Sup_Branch_Documentation_Branch_Code == BranchCode
                    && d.CR_Cas_Sup_Branch_Documentation_Code == "11" && d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode);
                    if (doc != null)
                    {
                        if (doc.CR_Cas_Sup_Branch_Documentation_Status == "A" || doc.CR_Cas_Sup_Branch_Documentation_Status == "X")
                        {
                            Session["ChamberCommerce"] = "True";
                        }
                        else
                        {
                            Session["ChamberCommerce"] = "False";
                        }
                    }
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Transfer_Permission == true))
                {
                    Session["TransferPermission"] = "True";
                }
                else
                {
                    var doc = db.CR_Cas_Sup_Branch_Documentation.FirstOrDefault(d => d.CR_Cas_Sup_Branch_Documentation_Branch_Code == BranchCode
                    && d.CR_Cas_Sup_Branch_Documentation_Code == "12" && d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode);
                    if (doc != null)
                    {
                        if (doc.CR_Cas_Sup_Branch_Documentation_Status == "A" || doc.CR_Cas_Sup_Branch_Documentation_Status == "X")
                        {
                            Session["TransferPermission"] = "True";
                        }
                        else
                        {
                            Session["TransferPermission"] = "False";
                        }
                    }
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Licence_Municipale == true))
                {
                    Session["LicenceMunicipale"] = "True";
                }
                else
                {
                    var doc = db.CR_Cas_Sup_Branch_Documentation.FirstOrDefault(d => d.CR_Cas_Sup_Branch_Documentation_Branch_Code == BranchCode
                    && d.CR_Cas_Sup_Branch_Documentation_Code == "13" && d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode);
                    if (doc != null)
                    {
                        if (doc.CR_Cas_Sup_Branch_Documentation_Status == "A" || doc.CR_Cas_Sup_Branch_Documentation_Status == "X")
                        {
                            Session["LicenceMunicipale"] = "True";
                        }
                        else
                        {
                            Session["LicenceMunicipale"] = "False";
                        }
                    }
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Company_Address == true))
                {
                    Session["CompanyAddress"] = "True";
                }
                else
                {
                    var doc = db.CR_Cas_Sup_Branch_Documentation.FirstOrDefault(d => d.CR_Cas_Sup_Branch_Documentation_Branch_Code == BranchCode
                    && d.CR_Cas_Sup_Branch_Documentation_Code == "14" && d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode);
                    if (doc != null)
                    {
                        if (doc.CR_Cas_Sup_Branch_Documentation_Status == "A" || doc.CR_Cas_Sup_Branch_Documentation_Status == "X")
                        {
                            Session["CompanyAddress"] = "True";
                        }
                        else
                        {
                            Session["CompanyAddress"] = "False";
                        }
                    }
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Traffic_License == true))
                {
                    Session["TrafficLicense"] = "True";
                }
                else
                {
                    Session["TrafficLicense"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Insurance == true))
                {
                    Session["ContractInsurance"] = "True";
                }
                else
                {
                    Session["ContractInsurance"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Operating_Card == true))
                {
                    Session["ContractOperatingCard"] = "True";
                }
                else
                {
                    Session["ContractOperatingCard"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Chkec_Up == true))
                {
                    Session["ContractChkecUp"] = "True";
                }
                else
                {
                    Session["ContractChkecUp"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Id == true))
                {
                    Session["ContractId"] = "True";
                }
                else
                {
                    Session["ContractId"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Driving_License == true))
                {
                    Session["ContractDrivingLicense"] = "True";
                }
                else
                {
                    Session["ContractDrivingLicense"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Renter_Address == true))
                {
                    Session["ContractRenterAddress"] = "True";
                }
                else
                {
                    Session["ContractRenterAddress"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Employer == true))
                {
                    Session["ContractEmployer"] = "True";
                }
                else
                {
                    Session["ContractEmployer"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Tires == true))
                {
                    Session["ContractTires"] = "True";
                }
                else
                {
                    Session["ContractTires"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Oil == true))
                {
                    Session["ContractOil"] = "True";
                }
                else
                {
                    Session["ContractOil"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Maintenance == true))
                {
                    Session["ContractMaintenance"] = "True";
                }
                else
                {
                    Session["ContractMaintenance"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_FBrake_Maintenance == true))
                {
                    Session["ContractFBrakeMaintenance"] = "True";
                }
                else
                {
                    Session["ContractFBrakeMaintenance"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_BBrake_Maintenance == true))
                {
                    Session["ContractBBrakeMaintenance"] = "True";
                }
                else
                {
                    Session["ContractBBrakeMaintenance"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Extension == true))
                {
                    Session["ContractExtension"] = "True";
                }
                else
                {
                    Session["ContractExtension"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Age == true))
                {
                    Session["ContractAge"] = "True";
                }
                else
                {
                    Session["ContractAge"] = "False";
                }

                Session["ContractOpenAmoutRate"] = validity.CR_Cas_User_Validity_Contract_Open_Amout_Rate;
                Session["ContractDiscountRate"] = validity.CR_Cas_User_Validity_Contract_Discount_Rate;
                Session["ContractKm"] = validity.CR_Cas_User_Validity_Contract_Km;
                Session["ContractHour"] = validity.CR_Cas_User_Validity_Contract_Hour;

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Cancel == true))
                {
                    Session["ContractCancel"] = "True";
                }
                else
                {
                    Session["ContractCancel"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_End == true))
                {
                    Session["ContractEnd"] = "True";
                }
                else
                {
                    Session["ContractEnd"] = "False";
                }

                Session["ContractClose"] = validity.CR_Cas_User_Validity_Contract_Close;
                Session["ContractCloseAmountRate"] = validity.CR_Cas_User_Validity_Contract_Close_Amount_Rate;
            }
            else
            {
                
                Session["ContractRegister"] = "False";
                       
                Session["ChamberCommerce"] = "False";
                      
                Session["TransferPermission"] = "False";
                       
                Session["LicenceMunicipale"] = "False";
                        
                Session["CompanyAddress"] = "False";
                        
                Session["TrafficLicense"] = "False";
                
                Session["ContractInsurance"] = "False";
               
                Session["ContractOperatingCard"] = "False";
                
                Session["ContractChkecUp"] = "False";
               
                Session["ContractId"] = "False";
                
                Session["ContractDrivingLicense"] = "False";
                
                Session["ContractRenterAddress"] = "False";
                
                Session["ContractEmployer"] = "False";
               
                Session["ContractTires"] = "False";
                
                Session["ContractOil"] = "False";
                
                Session["ContractMaintenance"] = "False";
                
                Session["ContractFBrakeMaintenance"] = "False";
                
                Session["ContractBBrakeMaintenance"] = "False";
                
                Session["ContractExtension"] = "False";
                
                Session["ContractAge"] = "False";
                
                Session["ContractOpenAmoutRate"] = "0";
                Session["ContractDiscountRate"] = "0";
                Session["ContractKm"] = "0";
                Session["ContractHour"] = "0";

                
                Session["ContractCancel"] = "False";
               
                Session["ContractEnd"] = "False";
                

                Session["ContractClose"] = "0";
                Session["ContractCloseAmountRate"] = "0";
            }
            
        }

        //////public void GetExpiredDocs(string LessorCode)
        //////{
        //////    int nbr = 0;
        //////    var CompanydocsN = db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode
        //////    && d.CR_Cas_Sup_Branch_Documentation_Status == "N").Count();
        //////    Session["CompanyDocsN"] = CompanydocsN;

        //////    var CompanydocsX = db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode
        //////    && d.CR_Cas_Sup_Branch_Documentation_Status == "X").Count();
        //////    Session["CompanydocsX"] = CompanydocsX;

        //////    var CompanydocsE = db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode
        //////    && d.CR_Cas_Sup_Branch_Documentation_Status == "E").Count();
        //////    Session["CompanydocsE"] = CompanydocsE;

        //////    var Companydocs = CompanydocsE + CompanydocsN + CompanydocsX;




        //////    var CompanyContractsN = db.CR_Cas_Company_Contract.Where(d => d.CR_Cas_Company_Contract_Lessor == LessorCode
        //////    && d.CR_Cas_Company_Contract_Status == "N").Count();
        //////    Session["CompanyContractsN"] = CompanyContractsN;

        //////    var CompanyContractsX = db.CR_Cas_Company_Contract.Where(d => d.CR_Cas_Company_Contract_Lessor == LessorCode
        //////    && d.CR_Cas_Company_Contract_Status == "X").Count();
        //////    Session["CompanyContractsX"] = CompanyContractsX;

        //////    var CompanyContractsE = db.CR_Cas_Company_Contract.Where(d => d.CR_Cas_Company_Contract_Lessor == LessorCode
        //////    && d.CR_Cas_Company_Contract_Status == "E").Count();
        //////    Session["CompanyContractsE"] = CompanyContractsE;

        //////    var CompanyContracts = CompanyContractsN + CompanyContractsX + CompanyContractsE;




        //////    //var PricesN = db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode
        //////    //&& d.CR_Cas_Sup_Branch_Documentation_Status == "N").Count();
        //////    //Session["PricesN"] = PricesN;

        //////    var PricesX = db.CR_Cas_Car_Price_Basic.Where(d => d.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode
        //////    && d.CR_Cas_Car_Price_Basic_Status == "X").Count();
        //////    Session["PricesX"] = PricesX;

        //////    var PricesE = db.CR_Cas_Car_Price_Basic.Where(d => d.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode
        //////    && d.CR_Cas_Car_Price_Basic_Status == "E").Count();
        //////    Session["PricesE"] = PricesE;

        //////    var Prices = PricesX + PricesE;




        //////    var CarsDocN = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "3"
        //////    && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "N").Count();
        //////    Session["CarsDocN"] = CarsDocN;

        //////    var CarsDocX = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "3"
        //////    && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "X").Count();
        //////    Session["CarsDocX"] = CarsDocX;

        //////    var CarsDocE = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "3"
        //////    && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "E").Count();
        //////    Session["CarsDocE"] = CarsDocE;

        //////    var CarsDoc = CarsDocN + CarsDocX + CarsDocE;


        //////    var CarsMaintenanceN = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "4"
        //////    && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "N").Count();
        //////    Session["CarsMaintenanceN"] = CarsMaintenanceN;

        //////    var CarsMaintenanceE = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "4"
        //////    && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "E").Count();
        //////    Session["CarsMaintenanceE"] = CarsMaintenanceE;

        //////    var CarsMaintenanceX = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "4"
        //////    && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "E").Count();
        //////    Session["CarsMaintenanceX"] = CarsMaintenanceX;

        //////    var CarsMaintenance = CarsMaintenanceN + CarsMaintenanceX + CarsMaintenanceE;


        //////    nbr = nbr + Companydocs + CompanyContracts + Prices + CarsDoc + CarsMaintenance;
        //////    Session["AlertsNbr"] = nbr;

        //////}



        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CloseSession()
        {
            Session["UserLogin"] = "";
            Session["UserName"] = "";

            return RedirectToAction("Login", "Account");

        }


       

        [ActionName("Login")]
        [HttpGet]
        public ActionResult Login_GET()
        {
            Session["Language"] = "1";
            Session["Lang"] = "Arabic";

            return View();
        }

        [ActionName("Login")]
        [HttpPost]
        public ActionResult Login_Post(string LogEnter, string txtusername, string txtpassword, string lang)
        {
            Session["Language"] = "1"; // remove after writing languages code
            if (!string.IsNullOrEmpty(lang) && lang == "عربي")
            {
                Session["Language"] = "1";
            }

            if (!string.IsNullOrEmpty(lang) && lang == "English")
            {
                Session["Language"] = "2";

            }
            if (!string.IsNullOrEmpty(lang) && lang == "Français")
            {
                Session["Language"] = "3";
            }
            if (Session["Language"].ToString() == "1")
            {
                if (LogEnter == "الدخول")
                {
                    if (txtusername != "" && txtpassword != "")
                    {
                        var q = db.CR_Mas_User_Information.FirstOrDefault(user => user.CR_Mas_User_Information_Code == txtusername 
                        && user.CR_Mas_User_Information_PassWord == txtpassword && user.CR_Mas_User_Information_Status == "A");

                        if (q != null)
                        {
                            Session["UserLogin"] = q.CR_Mas_User_Information_Code.Trim();
                            Session["UserName"] = q.CR_Mas_User_Information_Ar_Name;
                            Session["SystemCode"] = db.CR_Mas_User_Information.FirstOrDefault(user => user.CR_Mas_User_Information_Code == q.CR_Mas_User_Information_Code.Trim()).CR_Mas_User_Main_Validation.Select(x => x.CR_Mas_Sys_Tasks).First().CR_Mas_Sys_System_Code.ToString();
                            Session["LessorCode"] = "1000";
                            Init();
                            Get_Authority(q.CR_Mas_User_Information_Code.Trim());
                            Session["Hello"] = Session["UserName"];
                            Session["UserId"] = Session["UserLogin"].ToString();

                            return RedirectToAction("index", "home");
                        }
                        else
                        {
                            //////ViewBag.LoginError = "الرجاء التأكد من إسم المستخدم";
                            //////ViewBag.PassError = "الرجاء التأكد من كلمة السر";
                            ///\\\\\\\\\\\\\\\\\\\\\\\\\\CAS\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                            var q1 = db.CR_Cas_User_Information.FirstOrDefault(user => user.CR_Cas_User_Information_Id == txtusername && user.CR_Cas_User_Information_PassWord == txtpassword);
                            if (q1 != null)
                            {
                                if (q1.CR_Cas_User_Information_Status == "A")
                                {
                                    Session["UserLogin"] = q1.CR_Cas_User_Information_Id;
                                    Session["UserName"] = q1.CR_Cas_User_Information_Ar_Name;
                                    Session["LessorCode"] = q1.CR_Cas_User_Information_Lessor_Code;
                                    Session["BranchCode"] = q1.CR_Cas_User_Information_Branch_Code;
                                    if (q1.CR_Cas_User_Information_Image != null)
                                    {
                                        Session["UserImage"] = q1.CR_Cas_User_Information_Image;
                                    }
                                    else
                                    {
                                        Session["UserImage"] = "";
                                    }

                                    //////GetExpiredDocs(q1.CR_Cas_User_Information_Lessor_Code);
                                    Models.CAS.LoadAlerts lAlerts = new Models.CAS.LoadAlerts();
                                    lAlerts.GetExpiredDocs(q1.CR_Cas_User_Information_Lessor_Code);

                                    Session["SystemCode"] = "CAS";
                                    var getlessor = db.CR_Mas_Com_Lessor.FirstOrDefault(l => l.CR_Mas_Com_Lessor_Code == q1.CR_Cas_User_Information_Lessor_Code);
                                    if (getlessor != null)
                                    {
                                        if (getlessor.CR_Mas_Com_Lessor_Logo != null)
                                        {
                                            Session["LessorLogo"] = getlessor.CR_Mas_Com_Lessor_Logo.ToString();
                                        }
                                        else
                                        {
                                            Session["LessorLogo"] = "";
                                        }

                                        Session["CompName"] = getlessor.CR_Mas_Com_Lessor_Ar_Long_Name.ToString();
                                    }
                                    else if (getlessor.CR_Mas_Com_Lessor_Status == "H")
                                    {
                                        ViewBag.LoginError = "عفوا هذه الشركة موقوفة";
                                        return View();
                                    }
                                    else if (getlessor.CR_Mas_Com_Lessor_Status == "D")
                                    {
                                        ViewBag.LoginError = "عفوا هذه الشركة محذوفة";
                                        return View();
                                    }
                                    else
                                    {
                                        ViewBag.LoginError = "الرجاء التأكد من إسم المستخدم و  كلمة السر";
                                        return View();
                                    }

                                    var GetBranch = db.CR_Cas_Sup_Branch.FirstOrDefault(b => b.CR_Cas_Sup_Lessor_Code == q1.CR_Cas_User_Information_Lessor_Code &&
                                    b.CR_Cas_Sup_Branch_Code == q1.CR_Cas_User_Information_Branch_Code);
                                    if (GetBranch != null)
                                    {
                                        if (GetBranch.CR_Cas_Sup_Branch_LogoMap != "" && GetBranch.CR_Cas_Sup_Branch_LogoMap != null)
                                        {
                                            Session["BranchLogo"] = GetBranch.CR_Cas_Sup_Branch_LogoMap.ToString();
                                        }
                                        else
                                        {
                                            Session["BranchLogo"] = "";
                                        }

                                        var brnChk = "<span style=\"color: #ff5b5b \"> (الفرع موقوف)</span>";
                                        var brnName = "<span>" + GetBranch.CR_Cas_Sup_Branch_Ar_Name + "</span>";
                                        if (GetBranch.CR_Cas_Sup_Branch_Status=="H")
                                        {
                                            Session["BranchName"] = brnName + brnChk;
                                        }
                                        else
                                        {
                                            Session["BranchName"] = GetBranch.CR_Cas_Sup_Branch_Ar_Name.ToString();
                                        }
                                        if ((bool)q1.CR_Cas_User_Information_Auth_Branch == true)
                                        {
                                            Session["AuthBranch"] = "true";
                                        }
                                        else
                                        {
                                            Session["AuthBranch"] = "false";
                                        }
                                    }

                                    else
                                    {
                                        var validity = db.CR_Cas_User_Branch_Validity.Where(v => v.CR_Cas_User_Branch_Validity_Id == txtusername);
                                        var nbr = validity.Count();
                                        if (nbr>=1)
                                        {
                                            foreach (var v in validity)
                                            {
                                                GetBranch = db.CR_Cas_Sup_Branch.FirstOrDefault(b => b.CR_Cas_Sup_Lessor_Code == q1.CR_Cas_User_Information_Lessor_Code &&
                                                  b.CR_Cas_Sup_Branch_Status == "A" && b.CR_Cas_Sup_Branch_Code == v.CR_Cas_User_Branch_Validity_Branch);
                                                if (GetBranch != null)
                                                {
                                                    if (GetBranch.CR_Cas_Sup_Branch_LogoMap != "" && GetBranch.CR_Cas_Sup_Branch_LogoMap != null)
                                                    {
                                                        Session["BranchLogo"] = GetBranch.CR_Cas_Sup_Branch_LogoMap.ToString();
                                                    }
                                                    else
                                                    {
                                                        Session["BranchLogo"] = "";
                                                    }
                                                    Session["BranchName"] = GetBranch.CR_Cas_Sup_Branch_Ar_Name.ToString();
                                                    if ((bool)q1.CR_Cas_User_Information_Auth_Branch == true)
                                                    {
                                                        Session["AuthBranch"] = "true";
                                                    }
                                                    else
                                                    {
                                                        Session["AuthBranch"] = "false";
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Session["AuthBranch"] = "false";
                                        }
                                        
                                    }


                                    
                                    
                                    if ((bool)q1.CR_Cas_User_Information_Auth_System == true)
                                    {
                                        Session["AuthSystem"] = "true";
                                    }
                                    else
                                    {
                                        Session["AuthSystem"] = "false";
                                    }

                                   
                                    if (Session["AuthSystem"].ToString() == "true")
                                    {
                                        Session["Hello"] = Session["UserName"];
                                        Session["UserId"] = Session["UserLogin"];
                                        return RedirectToAction("index", "CasHome");
                                    }
                                    else if (Session["AuthBranch"].ToString() == "true" && GetBranch != null)
                                    {
                                        Session["Hello"] = Session["UserName"];
                                        Session["UserId"] = Session["UserLogin"];
                                        GetValidityContract(txtusername, q1.CR_Cas_User_Information_Branch_Code, q1.CR_Cas_User_Information_Lessor_Code);

                                        return RedirectToAction("BranchHome", "BranchHome");
                                    }
                                    else
                                    {
                                        ViewBag.LoginError = "عفواً ..... لايوجد صلاحية للدخول ";
                                        return View();
                                    }
                                }
                                else if (q1.CR_Cas_User_Information_Status == "D")
                                {
                                    ViewBag.LoginError = "عفوا هذا المستخدم محذوف";
                                    return View();
                                }
                                else if (q1.CR_Cas_User_Information_Status == "H")
                                {
                                    ViewBag.LoginError = "عفوا هذا المستخدم موقوف";
                                    return View();
                                }
                            }
                            
                            else
                            {
                                ViewBag.LoginError = "الرجاء التأكد من إسم المستخدم و  كلمة السر";
                                return View();
                            }

                        }

                    }
                    else if (txtusername == "" && txtpassword != "")
                    {
                        ViewBag.Login = "الرجاء إدخال إسم المستخدم";
                        ViewBag.Password = "";
                    }
                    else if (txtpassword == "" && txtusername != "")
                    {
                        ViewBag.Login = "";
                        ViewBag.Password = "الرجاء إدخال كلمة السر ";
                    }
                    else
                    {
                        ViewBag.Login = "الرجاء إدخال إسم المستخدم";
                        ViewBag.Password = "الرجاء إدخال كلمة السر ";
                    }

                }

            }
            if (Session["Language"].ToString() == "3")
            {
                if (LogEnter == "الدخول")
                {
                    if (txtusername != "" && txtpassword != "")
                    {
                        var q = db.CR_Mas_User_Information.FirstOrDefault(user => user.CR_Mas_User_Information_Code == txtusername && user.CR_Mas_User_Information_PassWord == txtpassword && user.CR_Mas_User_Information_Status == "A");
                        if (q != null)
                        {
                            Session["UserLogin"] = q.CR_Mas_User_Information_Code;
                            Session["UserName"] = q.CR_Mas_User_Information_Ar_Name;

                            Init();
                            //Get_Authority();
                            Session["Hello"] = "Bienvenu ...." + Session["UserName"];
                            Session["UserId"] = Session["UserLogin"];
                            return RedirectToAction("index", "home");
                        }
                        else
                        {
                            //////ViewBag.LoginError = "الرجاء التأكد من إسم المستخدم";
                            //////ViewBag.PassError = "الرجاء التأكد من كلمة السر";

                            ViewBag.LoginError = "Veuillez vérifier le nom de l'utilisateur et le mot de passe";
                        }

                    }
                    else if (txtusername == "" && txtpassword != "")
                    {
                        ViewBag.Login = "Veuillez saisir le nom de l'utilisateur";
                        ViewBag.Password = "";
                    }
                    else if (txtpassword == "" && txtusername != "")
                    {
                        ViewBag.Login = "";
                        ViewBag.Password = "Veuillez saisir le mot de passe";
                    }
                    else
                    {
                        ViewBag.Login = "Veuillez saisir le nom de l'utilisateur";
                        ViewBag.Password = "Veuillez saisir le mot de passe";
                    }

                }
            }
            return View();
        }

       

        public void Get_Authority(string UserLogin)
        {
            //IList<CR_Mas_User_Main_Validation> MVALList = new List<CR_Mas_User_Main_Validation>();

            //List<CR_Mas_User_Main_Validation> MainVal = (from r in db.CR_Mas_User_Main_Validation
            //            where r.CR_Mas_User_Main_Validation_Code == UserLogin && r.CR_Mas_User_Main_Validation1 == true
            //            select new CR_Mas_User_Main_Validation { 
            //                CR_Mas_User_Main_Validation_Tasks_Code=r.CR_Mas_User_Main_Validation_Tasks_Code,
            //                CR_Mas_User_Main_Validation1=r.CR_Mas_User_Main_Validation1
            //            }).ToList();


            //foreach(var Authority in MainVal)
            //{
            //    if (Authority.CR_Mas_User_Main_Validation_Tasks_Code == "1004")
            //    {
            //        MT_1001 = true;
            //    }
            //}
            var Lrecord = db.CR_Mas_User_Main_Validation
                               .Where(t => t.CR_Mas_User_Main_Validation_Code.Contains(UserLogin));

            foreach (var record in Lrecord)
            {
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1004" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1004"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1005" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1005"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1006" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1006"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1007" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1007"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1008" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1008"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1009" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1009"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1010" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1010"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1011" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1011"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1101" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1101"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1102" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1102"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1103" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1103"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1104" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1104"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1201" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1201"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1202" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1202"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1203" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1203"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1204" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1204"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1205" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1205"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1301" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1301"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1302" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1302"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1303" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1303"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1304" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1304"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1401" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1401"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1402" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1402"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1501" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1501"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1502" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1502"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1503" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1503"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1504" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1504"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1505" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1505"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1506" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1506"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1507" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1507"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1508" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1508"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1509" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1509"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1510" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1510"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1601" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1601"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1602" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1602"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1603" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1603"] = "true";
                    continue;
                }
                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1604" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1604"] = "true";
                    continue;
                }

                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1605" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1605"] = "true";
                    continue;
                }

                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1701" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1701"] = "true";
                    continue;
                }

                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1702" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1702"] = "true";
                    continue;
                }

                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1801" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1801"] = "true";
                    continue;
                }

                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1802" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1802"] = "true";
                    continue;
                }

                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1803" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1803"] = "true";
                    continue;
                }

                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1901" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1901"] = "true";
                    continue;
                }

                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1902" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1902"] = "true";
                    continue;
                }

                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1903" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1903"] = "true";
                    continue;
                }

                if (record.CR_Mas_User_Main_Validation_Tasks_Code == "1904" && record.CR_Mas_User_Main_Validation1 == true)
                {
                    Session["MT_1904"] = "true";
                    continue;
                }
                //List<CR_Mas_User_Main_Validation> MainVal = (from r in db.CR_Mas_User_Main_Validation
                //                                             where r.CR_Mas_User_Main_Validation_Code == UserLogin && r.CR_Mas_User_Main_Validation1 == true
                //                                             select new CR_Mas_User_Main_Validation
                //                                             {
                //                                                 CR_Mas_User_Main_Validation_Tasks_Code = r.CR_Mas_User_Main_Validation_Tasks_Code,
                //                                                 CR_Mas_User_Main_Validation1 = r.CR_Mas_User_Main_Validation1
                //                                             }).ToList();


                //var query = (from T in db.CR_Mas_Sys_Tasks
                //             from MainV in db.CR_Mas_User_Main_Validation
                //             from UserInfo in db.CR_Mas_User_Information
                //             where UserInfo.CR_Mas_User_Information_Code == UserLogin &&
                //             MainV.CR_Mas_User_Main_Validation_Code == UserInfo.CR_Mas_User_Information_Code &&
                //             MainV.CR_Mas_User_Main_Validation_Tasks_Code == T.CR_Mas_Sys_Tasks_Code
                //             select new CR_Mas_Sys_Tasks
                //             {
                //                 CR_Mas_Sys_Tasks_Code = T.CR_Mas_Sys_Tasks_Code,
                //                 CR_Mas_Sys_Tasks_Main_Validation = T.CR_Mas_Sys_Tasks_Main_Validation,
                //                 CR_Mas_Sys_Tasks_Sub_Validation = T.CR_Mas_Sys_Tasks_Sub_Validation

                //             }).ToList();

                //foreach (var q in query)
                //{
                //    if (q.CR_Mas_Sys_Tasks_Code=="1004")
                //    {
                //        ST_1004 = true;
                //        continue;
                //    }
                //}
            }
            var query = db.CR_Mas_User_Information.FirstOrDefault(user => user.CR_Mas_User_Information_Code == UserLogin.Trim()).CR_Mas_User_Main_Validation.Select(x => x.CR_Mas_Sys_Tasks).ToList();

            foreach (var q in query)
            {

                if (q.CR_Mas_Sys_Tasks_Code == "1004")
                {
                    if (Session["MT_1004"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1004"] = "true";
                            continue;
                        }
                        else
                        {
                            continue;
                        }

                    }
                }
                if (q.CR_Mas_Sys_Tasks_Code == "1005")
                {
                    if (Session["MT_1005"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1005"] = "true";
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }

                if (q.CR_Mas_Sys_Tasks_Code == "1006")
                {
                    if (Session["MT_1006"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1006"] = "true";
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                if (q.CR_Mas_Sys_Tasks_Code == "1007")
                {
                    if (Session["MT_1007"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1007"] = "true";
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                if (q.CR_Mas_Sys_Tasks_Code == "1008")
                {
                    if (Session["MT_1008"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1008"] = "true";
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                if (q.CR_Mas_Sys_Tasks_Code == "1009")
                {
                    if (Session["MT_1009"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1009"] = "true";
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                if (q.CR_Mas_Sys_Tasks_Code == "1010")
                {
                    if (Session["MT_1010"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1010"] = "true";
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                if (q.CR_Mas_Sys_Tasks_Code == "1011")
                {
                    if (Session["MT_1011"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1011"] = "true";
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                if (q.CR_Mas_Sys_Tasks_Code == "1101")
                {
                    if (Session["MT_1101"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1101"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1101");
                            if (SubQuery != null)
                            {

                                if ((bool)SubQuery.CR_Mas_User_Sub_Validation_Insert == true)
                                {
                                    Session["ST_1101_insert"] = "true";
                                }
                                else
                                {
                                    Session["ST_1101_insert"] = "false";
                                }

                                Session["ST_1101_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1101_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1101_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1101_hold "] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1101_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1101_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                if (q.CR_Mas_Sys_Tasks_Code == "1102")
                {
                    if (Session["MT_1102"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1102"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1102");
                            if (SubQuery != null)
                            {
                                Session["ST_1102_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1102_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1102_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1102_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1102_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1102_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1102_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                if (q.CR_Mas_Sys_Tasks_Code == "1103")
                {
                    if (Session["MT_1103"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1103"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1103");
                            if (SubQuery != null)
                            {
                                Session["ST_1103_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1103_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1103_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1103_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1103_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1103_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1103_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                if (q.CR_Mas_Sys_Tasks_Code == "1104")
                {
                    if (Session["MT_1104"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1104"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1104");
                            if (SubQuery != null)
                            {
                                Session["ST_1104_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1104_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1104_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1104_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1104_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1104_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1104_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                if (q.CR_Mas_Sys_Tasks_Code == "1201")
                {
                    if (Session["MT_1201"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1201"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1201");
                            if (SubQuery != null)
                            {
                                Session["ST_1201_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1201_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1201_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1201_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1201_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1201_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1201_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1202")
                {
                    if (Session["MT_1202"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1202"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1202");
                            if (SubQuery != null)
                            {
                                Session["ST_1202_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1202_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1202_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1202_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1202_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1202_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1202_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1203")
                {
                    if (Session["MT_1203"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1203"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1203");
                            if (SubQuery != null)
                            {
                                Session["ST_1203_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1203_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1203_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1203_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1203_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1203_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1203_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                if (q.CR_Mas_Sys_Tasks_Code == "1204")
                {
                    if (Session["MT_1204"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1204"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1204");
                            if (SubQuery != null)
                            {
                                Session["ST_1204_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1204_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1204_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1204_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1204_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1204_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1204_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1205")
                {
                    if (Session["MT_1205"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1205"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1205");
                            if (SubQuery != null)
                            {
                                Session["ST_1205_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1205_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1205_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1205_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1205_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1205_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1205_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1301")
                {
                    if (Session["MT_1301"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1301"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1301");
                            if (SubQuery != null)
                            {
                                Session["ST_1301_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1301_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1301_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1301_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1301_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1301_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1301_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1302")
                {
                    if (Session["MT_1302"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1302"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1302");
                            if (SubQuery != null)
                            {
                                Session["ST_1302_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1302_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1302_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1302_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1302_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1302_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1302_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1303")
                {
                    if (Session["MT_1303"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1303"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1303");
                            if (SubQuery != null)
                            {
                                Session["ST_1303_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1303_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1303_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1303_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1303_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1303_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1303_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1304")
                {
                    if (Session["MT_1304"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1304"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1304");
                            if (SubQuery != null)
                            {
                                Session["ST_1304_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1304_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1304_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1304_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1304_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1304_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1304_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1401")
                {
                    if (Session["MT_1401"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1401"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1401");
                            if (SubQuery != null)
                            {
                                Session["ST_1401_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                if (SubQuery.CR_Mas_User_Sub_Validation_UpDate == true)
                                {
                                    Session["ST_1401_update"] = "true";
                                }
                                else
                                {
                                    Session["ST_1401_update"] = "false";
                                }

                                Session["ST_1401_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1401_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1401_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1401_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1401_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1402")
                {
                    if (Session["MT_1402"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1402"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1402");
                            if (SubQuery != null)
                            {
                                Session["ST_1402_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1402_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1402_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1402_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1402_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1402_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1402_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1501")
                {
                    if (Session["MT_1501"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1501"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1501");
                            if (SubQuery != null)
                            {
                                Session["ST_1501_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1501_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower().ToString().ToLower();
                                Session["ST_1501_delete"] = Convert.ToString(SubQuery.CR_Mas_User_Sub_Validation_Delete).ToString().ToLower();
                                Session["ST_1501_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1501_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1501_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1501_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1502")
                {
                    if (Session["MT_1502"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1502"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1502");
                            if (SubQuery != null)
                            {
                                Session["ST_1502_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1502_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1502_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1502_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1502_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1502_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1502_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1503")
                {
                    if (Session["MT_1503"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1503"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1503");
                            if (SubQuery != null)
                            {
                                Session["ST_1503_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1503_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1503_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1503_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1503_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1503_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1503_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1504")
                {
                    if (Session["MT_1504"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1504"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1504");
                            if (SubQuery != null)
                            {
                                Session["ST_1504_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1504_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1504_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1504_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1504_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1504_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1504_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1505")
                {
                    if (Session["MT_1505"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1505"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1505");
                            if (SubQuery != null)
                            {
                                Session["ST_1505_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1505_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1505_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1505_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1505_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1505_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1505_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1506")
                {
                    if (Session["MT_1506"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1506"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1506");
                            if (SubQuery != null)
                            {
                                Session["ST_1506_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1506_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1506_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1506_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1506_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1506_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1506_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1507")
                {
                    if (Session["MT_1507"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1507"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1507");
                            if (SubQuery != null)
                            {
                                Session["ST_1507_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1507_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1507_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1507_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1507_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1507_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1507_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1508")
                {
                    if (Session["MT_1508"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1508"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1508");
                            if (SubQuery != null)
                            {
                                Session["ST_1508_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1508_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1508_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1508_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1508_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1508_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1508_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1509")
                {
                    if (Session["MT_1509"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1509"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1509");
                            if (SubQuery != null)
                            {
                                Session["ST_1509_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1509_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1509_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1509_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1509_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1509_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1509_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1510")
                {
                    if (Session["MT_1510"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1510"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1510");
                            if (SubQuery != null)
                            {
                                Session["ST_1510_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1510_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1510_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1510_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1510_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1510_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1510_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }

                if (q.CR_Mas_Sys_Tasks_Code == "1601")
                {
                    if (Session["MT_1601"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1601"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1601");
                            if (SubQuery != null)
                            {
                                Session["ST_1601_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1601_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1601_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1601_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1601_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1601_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1601_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1602")
                {
                    if (Session["MT_1602"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1602"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1602");
                            if (SubQuery != null)
                            {
                                Session["ST_1602_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1602_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1602_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1602_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1602_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1602_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1602_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1603")
                {
                    if (Session["MT_1603"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1603"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1603");
                            if (SubQuery != null)
                            {
                                Session["ST_1603_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1603_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1603_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1603_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1603_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1603_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1603_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1604")
                {
                    if (Session["MT_1604"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1604"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1604");
                            if (SubQuery != null)
                            {
                                Session["ST_1604_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1604_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1604_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1604_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1604_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1604_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1604_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1605")
                {
                    if (Session["MT_1605"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1605"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1605");
                            if (SubQuery != null)
                            {
                                Session["ST_1605_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1605_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1605_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1605_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1605_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1605_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1605_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1701")
                {
                    if (Session["MT_1701"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1701"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1701");
                            if (SubQuery != null)
                            {
                                Session["ST_1701_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1701_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1701_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1701_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1701_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1701_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1701_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1702")
                {
                    if (Session["MT_1702"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1702"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1702");
                            if (SubQuery != null)
                            {
                                Session["ST_1702_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1702_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1702_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1702_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1702_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1702_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1702_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1801")
                {
                    if (Session["MT_1801"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1801"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1801");
                            if (SubQuery != null)
                            {
                                Session["ST_1801_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1801_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1801_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1801_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1801_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1801_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1801_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1802")
                {
                    if (Session["MT_1802"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1802"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1802");
                            if (SubQuery != null)
                            {
                                Session["ST_1802_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1802_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1802_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1802_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1802_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1802_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1802_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1803")
                {
                    if (Session["MT_1803"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1803"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1803");
                            if (SubQuery != null)
                            {
                                Session["ST_1803_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1803_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1803_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1803_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1803_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1803_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1803_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1901")
                {
                    if (Session["MT_1901"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1901"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1901");
                            if (SubQuery != null)
                            {
                                Session["ST_1901_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1901_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1901_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1901_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1901_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1901_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1901_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1902")
                {
                    if (Session["MT_1902"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1902"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1902");
                            if (SubQuery != null)
                            {
                                Session["ST_1902_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1902_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1902_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1902_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1902_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1902_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1902_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1903")
                {
                    if (Session["MT_1903"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1903"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1903");
                            if (SubQuery != null)
                            {
                                Session["ST_1903_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1903_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1903_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1903_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1903_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1903_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1903_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


                if (q.CR_Mas_Sys_Tasks_Code == "1904")
                {
                    if (Session["MT_1904"].ToString() == "true")
                    {
                        if (q.CR_Mas_Sys_Tasks_Sub_Validation == true)
                        {
                            Session["ST_1904"] = "true";
                            var SubQuery = db.CR_Mas_User_Sub_Validation.FirstOrDefault(x => x.CR_Mas_User_Sub_Validation_Code == UserLogin && x.CR_Mas_User_Sub_Validation_Tasks_Code == "1904");
                            if (SubQuery != null)
                            {
                                Session["ST_1904_insert"] = SubQuery.CR_Mas_User_Sub_Validation_Insert.ToString().ToLower();
                                Session["ST_1904_update"] = SubQuery.CR_Mas_User_Sub_Validation_UpDate.ToString().ToLower();
                                Session["ST_1904_delete"] = SubQuery.CR_Mas_User_Sub_Validation_Delete.ToString().ToLower();
                                Session["ST_1904_undelete"] = SubQuery.CR_Mas_User_Sub_Validation_UnDelete.ToString().ToLower();
                                Session["ST_1904_hold"] = SubQuery.CR_Mas_User_Sub_Validation_Hold.ToString().ToLower();
                                Session["ST_1904_unhold"] = SubQuery.CR_Mas_User_Sub_Validation_UnHold.ToString().ToLower();
                                Session["ST_1904_print"] = SubQuery.CR_Mas_User_Sub_Validation_Print.ToString().ToLower();
                            }
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }

        }

        public void Init()
        {
            Session["MT_1001"] = "false";  // دخول النظام الإداري للشركة
            Session["MT_1002"] = "false";  //القائمة الرئيسية
            Session["MT_1003"] = "false";  //تغيير كلمة السر
            Session["MT_1004"] = "false";  //عدد الشركات 
            Session["MT_1005"] = "false";  //عدد العقود
            Session["MT_1006"] = "false";  //عدد السيارات
            Session["MT_1007"] = "false";  //عدد العقود المنتهية
            Session["MT_1008"] = "false";  //عدد عقود التأجير
            Session["MT_1009"] = "false";  //عدد العقود على وشك الإنتهاء
            Session["MT_1010"] = "false";  //عدد عقود التأجير المنتهية
            Session["MT_1011"] = "false";  //عدد عقود التأجير القائمة

            Session["MT_1101"] = "false";  //عقد تأجير السيارات
            Session["MT_1102"] = "false";  //عقد تأجير السيارات التفصيلي
            Session["MT_1103"] = "false";  //عقود الشركات المساندات
            Session["MT_1104"] = "false";  //عقد تبادل البيانات

            Session["MT_1201"] = "false";  //تقارير إدارية مختصرة
            Session["MT_1202"] = "false";  //تقارير مالية مختصرة
            Session["MT_1203"] = "false";  //تقارير إدارية تفصيلية
            Session["MT_1204"] = "false";  //تقارير مالية تفصيلية
            Session["MT_1205"] = "false";  //تقارير إحصائية

            Session["MT_1301"] = "false";  //
            Session["MT_1302"] = "false";  //
            Session["MT_1303"] = "false";  //
            Session["MT_1304"] = "false";  //
            Session["MT_1401"] = "false";  //
            Session["MT_1402"] = "false";  //

            Session["MT_1501"] = "false";  //
            Session["MT_1502"] = "false";  //
            Session["MT_1503"] = "false";  //
            Session["MT_1504"] = "false";  //
            Session["MT_1505"] = "false";  //
            Session["MT_1506"] = "false";  //
            Session["MT_1507"] = "false"; //
            Session["MT_1508"] = "false";  //
            Session["MT_1509"] = "false";  //
            Session["MT_1510"] = "false";  //

            Session["MT_1601"] = "false";
            Session["MT_1602"] = "false";
            Session["MT_1603"] = "false";
            Session["MT_1604"] = "false";
            Session["MT_1605"] = "false";

            Session["MT_1701"] = "false";
            Session["MT_1702"] = "false";

            Session["MT_1801"] = "false";
            Session["MT_1802"] = "false";
            Session["MT_1803"] = "false";

            Session["MT_1901"] = "false";
            Session["MT_1902"] = "false";
            Session["MT_1903"] = "false";
            Session["MT_1904"] = "false";




            Session["ST_1001"] = "false";  // دخول النظام الإداري للشركة
            Session["ST_1002"] = "false";  //القائمة الرئيسية
            Session["ST_1003"] = "false";  //تغيير كلمة السر
            Session["ST_1004"] = "false";   //عدد الشركات 
            Session["ST_1005"] = "false";   //عدد العقود
            Session["ST_1006"] = "false";   //عدد السيارات
            Session["ST_1007"] = "false";   //عدد العقود المنتهية
            Session["ST_1008"] = "false";   //عدد عقود التأجير
            Session["ST_1009"] = "false";   //عدد العقود على وشك الإنتهاء
            Session["ST_1010"] = "false";   //عدد عقود التأجير المنتهية
            Session["ST_1011"] = "false";   //عدد عقود التأجير القائمة

            Session["ST_1101"] = "false";   //عقد تأجير السيارات
            Session["ST_1102"] = "false";   //عقد تأجير السيارات التفصيلي
            Session["ST_1103"] = "false";   //عقود الشركات المساندات
            Session["ST_1104"] = "false";   //عقد تبادل البيانات

            Session["ST_1201"] = "false";   //تقارير إدارية مختصرة
            Session["ST_1202"] = "false";   //تقارير مالية مختصرة
            Session["ST_1203"] = "false";   //تقارير إدارية تفصيلية
            Session["ST_1204"] = "false";   //تقارير مالية تفصيلية
            Session["ST_1205"] = "false";   //تقارير إحصائية

            Session["ST_1301"] = "false";   //
            Session["ST_1302"] = "false";   //
            Session["ST_1303"] = "false";   //f
            Session["ST_1304"] = "false";  //f

            Session["ST_1401"] = "false";  //f
            Session["ST_1402"] = "false";  //f

            Session["ST_1501"] = "false";  //
            Session["ST_1502"] = "false";  //
            Session["ST_1503"] = "false";  //
            Session["ST_1504"] = "false";  //
            Session["ST_1505"] = "false";  //
            Session["ST_1506"] = "false";  //
            Session["ST_1507"] = "false";  //
            Session["ST_1508"] = "false";  //
            Session["ST_1509"] = "false";  //
            Session["ST_1510"] = "false";  //

            Session["ST_1601"] = "false";
            Session["ST_1602"] = "false";
            Session["ST_1603"] = "false";
            Session["ST_1604"] = "false";
            Session["ST_1605"] = "false";

            Session["ST_1701"] = "false";
            Session["ST_1702"] = "false";

            Session["ST_1801"] = "false";
            Session["ST_1802"] = "false";
            Session["ST_1803"] = "false";

            Session["ST_1901"] = "false";
            Session["ST_1902"] = "false";
            Session["ST_1903"] = "false";
            Session["ST_1904"] = "false";


            Session["ST_1101_insert"] = "false";
            Session["ST_1101_update"] = "false";
            Session["ST_1101_delete"] = "false";
            Session["ST_1101_undelete"] = "false";
            Session["ST_1101_hold"] = "false";
            Session["ST_1101_unhold"] = "false";
            Session["ST_1101_print"] = "false";

            Session["ST_1102_insert"] = "false";
            Session["ST_1102_update"] = "false";
            Session["ST_1102_delete"] = "false";
            Session["ST_1102_undelete"] = "false";
            Session["ST_1102_hold"] = "false";
            Session["ST_1102_unhold"] = "false";
            Session["ST_1102_print"] = "false";

            Session["ST_1103_insert"] = "false";
            Session["ST_1103_update"] = "false";
            Session["ST_1103_delete"] = "false";
            Session["ST_1103_undelete"] = "false";
            Session["ST_1103_hold"] = "false";
            Session["ST_1103_unhold"] = "false";
            Session["ST_1103_print"] = "false";

            Session["ST_1104_insert"] = "false";
            Session["ST_1104_update"] = "false";
            Session["ST_1104_delete"] = "false";
            Session["ST_1104_undelete"] = "false";
            Session["ST_1104_hold"] = "false";
            Session["ST_1104_unhold"] = "false";
            Session["ST_1104_print"] = "false";

            Session["ST_1201_insert"] = "false";
            Session["ST_1201_update"] = "false";
            Session["ST_1201_delete"] = "false";
            Session["ST_1201_undelete"] = "false";
            Session["ST_1201_hold"] = "false";
            Session["ST_1201_unhold"] = "false";
            Session["ST_1201_print"] = "false";

            Session["ST_1202_insert"] = "false";
            Session["ST_1202_update"] = "false";
            Session["ST_1202_delete"] = "false";
            Session["ST_1202_undelete"] = "false";
            Session["ST_1202_hold"] = "false";
            Session["ST_1202_unhold"] = "false";
            Session["ST_1202_print"] = "false";

            Session["ST_1203_insert"] = "false";
            Session["ST_1203_update"] = "false";
            Session["ST_1203_delete"] = "false";
            Session["ST_1203_undelete"] = "false";
            Session["ST_1203_hold"] = "false";
            Session["ST_1203_unhold"] = "false";
            Session["ST_1203_print"] = "false";

            Session["ST_1204_insert"] = "false";
            Session["ST_1204_update"] = "false";
            Session["ST_1204_delete"] = "false";
            Session["ST_1204_undelete"] = "false";
            Session["ST_1204_hold"] = "false";
            Session["ST_1204_unhold"] = "false";
            Session["ST_1204_print"] = "false";

            Session["ST_1205_insert"] = "false";
            Session["ST_1205_update"] = "false";
            Session["ST_1205_delete"] = "false";
            Session["ST_1205_undelete"] = "false";
            Session["ST_1205_hold"] = "false";
            Session["ST_1205_unhold"] = "false";
            Session["ST_1205_print"] = "false";

            Session["ST_1301_insert"] = "false";
            Session["ST_1301_update"] = "false";
            Session["ST_1301_delete"] = "false";
            Session["ST_1301_undelete"] = "false";
            Session["ST_1301_hold"] = "false";
            Session["ST_1301_unhold"] = "false";
            Session["ST_1301_print"] = "false";

            Session["ST_1302_insert"] = "false";
            Session["ST_1302_update"] = "false";
            Session["ST_1302_delete"] = "false";
            Session["ST_1302_undelete"] = "false";
            Session["ST_1302_hold"] = "false";
            Session["ST_1302_unhold"] = "false";
            Session["ST_1302_print"] = "false";

            Session["ST_1303_insert"] = "false";
            Session["ST_1303_update"] = "false";
            Session["ST_1303_delete"] = "false";
            Session["ST_1303_undelete"] = "false";
            Session["ST_1303_hold"] = "false";
            Session["ST_1303_unhold"] = "false";
            Session["ST_1303_print"] = "false";

            Session["ST_1304_insert"] = "false";
            Session["ST_1304_update"] = "false";
            Session["ST_1304_delete"] = "false";
            Session["ST_1304_undelete"] = "false";
            Session["ST_1304_hold"] = "false";
            Session["ST_1304_unhold"] = "false";
            Session["ST_1304_print"] = "false";


            Session["ST_1401_insert"] = "false";
            Session["ST_1401_update"] = "false";
            Session["ST_1401_delete"] = "false";
            Session["ST_1401_undelete"] = "false";
            Session["ST_1401_hold"] = "false";
            Session["ST_1401_unhold"] = "false";
            Session["ST_1401_print"] = "false";

            Session["ST_1402_insert"] = "false";
            Session["ST_1402_update"] = "false";
            Session["ST_1402_delete"] = "false";
            Session["ST_1402_undelete"] = "false";
            Session["ST_1402_hold"] = "false";
            Session["ST_1402_unhold"] = "false";
            Session["ST_1402_print"] = "false";


            Session["ST_1501_insert"] = "false";
            Session["ST_1501_update"] = "false";
            Session["ST_1501_delete"] = "false";
            Session["ST_1501_undelete"] = "false";
            Session["ST_1501_hold"] = "false";
            Session["ST_1501_unhold"] = "false";
            Session["ST_1501_print"] = "false";

            Session["ST_1502_insert"] = "false";
            Session["ST_1502_update"] = "false";
            Session["ST_1502_delete"] = "false";
            Session["ST_1502_undelete"] = "false";
            Session["ST_1502_hold"] = "false";
            Session["ST_1502_unhold"] = "false";
            Session["ST_1502_print"] = "false";

            Session["ST_1503_insert"] = "false";
            Session["ST_1503_update"] = "false";
            Session["ST_1503_delete"] = "false";
            Session["ST_1503_undelete"] = "false";
            Session["ST_1503_hold"] = "false";
            Session["ST_1503_unhold"] = "false";
            Session["ST_1503_print"] = "false";

            Session["ST_1504_insert"] = "false";
            Session["ST_1504_update"] = "false";
            Session["ST_1504_delete"] = "false";
            Session["ST_1504_undelete"] = "false";
            Session["ST_1504_hold"] = "false";
            Session["ST_1504_unhold"] = "false";
            Session["ST_1504_print"] = "false";

            Session["ST_1505_insert"] = "false";
            Session["ST_1505_update"] = "false";
            Session["ST_1505_delete"] = "false";
            Session["ST_1505_undelete"] = "false";
            Session["ST_1505_hold"] = "false";
            Session["ST_1505_unhold"] = "false";
            Session["ST_1505_print"] = "false";

            Session["ST_1506_insert"] = "false";
            Session["ST_1506_update"] = "false";
            Session["ST_1506_delete"] = "false";
            Session["ST_1506_undelete"] = "false";
            Session["ST_1506_hold"] = "false";
            Session["ST_1506_unhold"] = "false";
            Session["ST_1506_print"] = "false";

            Session["ST_1507_insert"] = "false";
            Session["ST_1507_update"] = "false";
            Session["ST_1507_delete"] = "false";
            Session["ST_1507_undelete"] = "false";
            Session["ST_1507_hold"] = "false";
            Session["ST_1507_unhold"] = "false";
            Session["ST_1507_print"] = "false";

            Session["ST_1508_insert"] = "false";
            Session["ST_1508_update"] = "false";
            Session["ST_1508_delete"] = "false";
            Session["ST_1508_undelete"] = "false";
            Session["ST_1508_hold"] = "false";
            Session["ST_1508_unhold"] = "false";
            Session["ST_1508_print"] = "false";

            Session["ST_1509_insert"] = "false";
            Session["ST_1509_update"] = "false";
            Session["ST_1509_delete"] = "false";
            Session["ST_1509_undelete"] = "false";
            Session["ST_1509_hold"] = "false";
            Session["ST_1509_unhold"] = "false";
            Session["ST_1509_print"] = "false";

            Session["ST_1510_insert"] = "false";
            Session["ST_1510_update"] = "false";
            Session["ST_1510_delete"] = "false";
            Session["ST_1510_undelete"] = "false";
            Session["ST_1510_hold"] = "false";
            Session["ST_1510_unhold"] = "false";
            Session["ST_1510_print"] = "false";

            Session["ST_1601_insert"] = "false";
            Session["ST_1601_update"] = "false";
            Session["ST_1601_delete"] = "false";
            Session["ST_1601_undelete"] = "false";
            Session["ST_1601_hold"] = "false";
            Session["ST_1601_unhold"] = "false";
            Session["ST_1601_print"] = "false";

            Session["ST_1602_insert"] = "false";
            Session["ST_1602_update"] = "false";
            Session["ST_1602_delete"] = "false";
            Session["ST_1602_undelete"] = "false";
            Session["ST_1602_hold"] = "false";
            Session["ST_1602_unhold"] = "false";
            Session["ST_1602_print"] = "false";

            Session["ST_1603_insert"] = "false";
            Session["ST_1603_update"] = "false";
            Session["ST_1603_delete"] = "false";
            Session["ST_1603_undelete"] = "false";
            Session["ST_1603_hold"] = "false";
            Session["ST_1603_unhold"] = "false";
            Session["ST_1603_print"] = "false";

            Session["ST_1604_insert"] = "false";
            Session["ST_1604_update"] = "false";
            Session["ST_1604_delete"] = "false";
            Session["ST_1604_undelete"] = "false";
            Session["ST_1604_hold"] = "false";
            Session["ST_1604_unhold"] = "false";
            Session["ST_1604_print"] = "false";

            Session["ST_1605_insert"] = "false";
            Session["ST_1605_update"] = "false";
            Session["ST_1605_delete"] = "false";
            Session["ST_1605_undelete"] = "false";
            Session["ST_1605_hold"] = "false";
            Session["ST_1605_unhold"] = "false";
            Session["ST_1605_print"] = "false";

            Session["ST_1701_insert"] = "false";
            Session["ST_1701_update"] = "false";
            Session["ST_1701_delete"] = "false";
            Session["ST_1701_undelete"] = "false";
            Session["ST_1701_hold"] = "false";
            Session["ST_1701_unhold"] = "false";
            Session["ST_1701_print"] = "false";

            Session["ST_1702_insert"] = "false";
            Session["ST_1702_update"] = "false";
            Session["ST_1702_delete"] = "false";
            Session["ST_1702_undelete"] = "false";
            Session["ST_1702_hold"] = "false";
            Session["ST_1702_unhold"] = "false";
            Session["ST_1702_print"] = "false";

            Session["ST_1801_insert"] = "false";
            Session["ST_1801_update"] = "false";
            Session["ST_1801_delete"] = "false";
            Session["ST_1801_undelete"] = "false";
            Session["ST_1801_hold"] = "false";
            Session["ST_1801_unhold"] = "false";
            Session["ST_1801_print"] = "false";

            Session["ST_1802_insert"] = "false";
            Session["ST_1802_update"] = "false";
            Session["ST_1802_delete"] = "false";
            Session["ST_1802_undelete"] = "false";
            Session["ST_1802_hold"] = "false";
            Session["ST_1802_unhold"] = "false";
            Session["ST_1802_print"] = "false";

            Session["ST_1803_insert"] = "false";
            Session["ST_1803_update"] = "false";
            Session["ST_1803_delete"] = "false";
            Session["ST_1803_undelete"] = "false";
            Session["ST_1803_hold"] = "false";
            Session["ST_1803_unhold"] = "false";
            Session["ST_1803_print"] = "false";

            Session["ST_1901_insert"] = "false";
            Session["ST_1901_update"] = "false";
            Session["ST_1901_delete"] = "false";
            Session["ST_1901_undelete"] = "false";
            Session["ST_1901_hold"] = "false";
            Session["ST_1901_unhold"] = "false";
            Session["ST_1901_print"] = "false";

            Session["ST_1902_insert"] = "false";
            Session["ST_1902_update"] = "false";
            Session["ST_1902_delete"] = "false";
            Session["ST_1902_undelete"] = "false";
            Session["ST_1902_hold"] = "false";
            Session["ST_1902_unhold"] = "false";
            Session["ST_1902_print"] = "false";

            Session["ST_1903_insert"] = "false";
            Session["ST_1903_update"] = "false";
            Session["ST_1903_delete"] = "false";
            Session["ST_1903_undelete"] = "false";
            Session["ST_1903_hold"] = "false";
            Session["ST_1903_unhold"] = "false";
            Session["ST_1903_print"] = "false";

            Session["ST_1904_insert"] = "false";
            Session["ST_1904_update"] = "false";
            Session["ST_1904_delete"] = "false";
            Session["ST_1904_undelete"] = "false";
            Session["ST_1904_hold"] = "false";
            Session["ST_1904_unhold"] = "false";
            Session["ST_1904_print"] = "false";
        }

    }
}