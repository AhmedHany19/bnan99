using RentCar.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentCar.Reports.BranchReport
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RentCarDBEntities db = new RentCarDBEntities();
            var LessorCode = Session["LessorCode"].ToString();

            var cR_Cas_Sup_Branch = db.CR_Cas_Sup_Branch.Where(x => x.CR_Cas_Sup_Lessor_Code == LessorCode);

            List<BranchCarsModel> l = new List<BranchCarsModel>();
            BranchCarsModel b = new BranchCarsModel();


            foreach (var branch in cR_Cas_Sup_Branch)
            {
                b = new BranchCarsModel();
                var nbr = db.CR_Cas_Sup_Car_Information.Where(x => x.CR_Cas_Sup_Car_Lessor_Code == LessorCode && x.CR_Cas_Sup_Car_Location_Branch_Code == branch.CR_Cas_Sup_Branch_Code).Count();
                var adr = db.CR_Mas_Address.FirstOrDefault(a => a.CR_Mas_Address_Id_Code == branch.CR_Cas_Sup_Branch_Government_No);

                var city = db.CR_Mas_Sup_City.FirstOrDefault(c => c.CR_Mas_Sup_City_Code == adr.CR_Mas_Address_City);

                b.CarsNumber = nbr.ToString();
                b.cityName = city.CR_Mas_Sup_City_Ar_Name;
                b.CR_Cas_Sup_Branch_Ar_Name = branch.CR_Cas_Sup_Branch_Ar_Name;
                b.CR_Cas_Sup_Branch_Ar_Short_Name = branch.CR_Cas_Sup_Branch_Ar_Short_Name;
                b.CR_Cas_Sup_Branch_Code = branch.CR_Cas_Sup_Branch_Code;
                b.CR_Cas_Sup_Branch_Commercial_Registration_No = branch.CR_Cas_Sup_Branch_Commercial_Registration_No;
                b.CR_Cas_Sup_Branch_Documentation = branch.CR_Cas_Sup_Branch_Documentation;
                b.CR_Cas_Sup_Branch_Duration_One_End_Time = branch.CR_Cas_Sup_Branch_Duration_One_End_Time.GetValueOrDefault();
                b.CR_Cas_Sup_Branch_Duration_One_Start_Time = branch.CR_Cas_Sup_Branch_Duration_One_Start_Time.GetValueOrDefault();
                b.CR_Cas_Sup_Branch_Duration_Three_End_Time = branch.CR_Cas_Sup_Branch_Duration_Three_End_Time.GetValueOrDefault();
                b.CR_Cas_Sup_Branch_Duration_Three_Start_Time = branch.CR_Cas_Sup_Branch_Duration_Three_Start_Time.GetValueOrDefault();
                b.CR_Cas_Sup_Branch_Duration_Tow_End_Time = branch.CR_Cas_Sup_Branch_Duration_Tow_End_Time.GetValueOrDefault();
                b.CR_Cas_Sup_Branch_Duration_Tow_Start_Time = branch.CR_Cas_Sup_Branch_Duration_Tow_Start_Time.GetValueOrDefault();
                b.CR_Cas_Sup_Branch_End_Day = branch.CR_Cas_Sup_Branch_End_Day.GetValueOrDefault();
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
                b.CR_Cas_Sup_Branch_Start_Day = branch.CR_Cas_Sup_Branch_Start_Day.GetValueOrDefault();
                b.CR_Cas_Sup_Branch_Status = branch.CR_Cas_Sup_Branch_Status;
                b.CR_Cas_Sup_Branch_Tax_No = branch.CR_Cas_Sup_Branch_Tax_No;
                b.CR_Cas_Sup_Branch_Tel = branch.CR_Cas_Sup_Branch_Tel;
                b.CR_Cas_Sup_Car_Information = branch.CR_Cas_Sup_Car_Information;
                b.CR_Cas_Sup_Lessor_Code = branch.CR_Cas_Sup_Lessor_Code;
                b.CR_Mas_Com_Lessor = branch.CR_Mas_Com_Lessor;
                l.Add(b);
            }

            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            BranchList crystalReport = new BranchList();

            crystalReport.SetDataSource(l);
            var logo = Session["LessorLogo"].ToString();
            var log = logo.Replace("~", "");
            log = log.Replace("/", "\\");
            log = log.Substring(1, log.Length - 1);
            var lm = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + log;
            //crystalReport.SetParameterValue("Logo", System.IO.Directory.GetCurrentDirectory() + logo) ; server side
            crystalReport.SetParameterValue("Logo", lm);
            crystalReport.SetParameterValue("UserName", Session["UserName"].ToString());

            CrystalReportViewer1.ReportSource = crystalReport;

            //CrystalReportViewer1.RefreshReport();
        }
    }
}