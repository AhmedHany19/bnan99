using RentCar.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentCar.Reports.Admin_Procedures
{
    public partial class FrmTracing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RentCarDBEntities db = new RentCarDBEntities();
            var LessorCode = Session["LessorCode"].ToString();

            List<TracingMD> L = new List<TracingMD>();


            var tracing = db.CR_Cas_Administrative_Procedures.Where(t => t.CR_Cas_Administrative_Procedures_Lessor == LessorCode);
            foreach (var tr in tracing)
            {
                TracingMD T = new TracingMD();
                T.CR_Cas_Administrative_Procedures_Action = (bool)tr.CR_Cas_Administrative_Procedures_Action;
                T.CR_Cas_Administrative_Procedures_Code = tr.CR_Cas_Administrative_Procedures_Code;
                var p = db.CR_Mas_Sup_Procedures.FirstOrDefault(pr => pr.CR_Mas_Sup_Procedures_Code == T.CR_Cas_Administrative_Procedures_Code);
                T.ProcedureName = p.CR_Mas_Sup_Procedures_Ar_Name;
                T.CR_Cas_Administrative_Procedures_Com_Supporting = tr.CR_Cas_Administrative_Procedures_Com_Supporting;
                T.CR_Cas_Administrative_Procedures_Date = tr.CR_Cas_Administrative_Procedures_Date.GetValueOrDefault();
                T.CR_Cas_Administrative_Procedures_Doc_Date = tr.CR_Cas_Administrative_Procedures_Doc_Date.GetValueOrDefault();
                T.CR_Cas_Administrative_Procedures_Doc_End_Date = tr.CR_Cas_Administrative_Procedures_Doc_End_Date.GetValueOrDefault();
                T.CR_Cas_Administrative_Procedures_Doc_No = tr.CR_Cas_Administrative_Procedures_Doc_No;
                T.CR_Cas_Administrative_Procedures_Doc_Start_Date = tr.CR_Cas_Administrative_Procedures_Doc_Start_Date.GetValueOrDefault();
                T.CR_Cas_Administrative_Procedures_From_Branch = tr.CR_Cas_Administrative_Procedures_From_Branch;
                T.CR_Cas_Administrative_Procedures_Lessor = tr.CR_Cas_Administrative_Procedures_Lessor;
                T.CR_Cas_Administrative_Procedures_No = tr.CR_Cas_Administrative_Procedures_No;
                T.CR_Cas_Administrative_Procedures_Reasons = tr.CR_Cas_Administrative_Procedures_Reasons;
                T.CR_Cas_Administrative_Procedures_Sector = tr.CR_Cas_Administrative_Procedures_Sector;
                T.CR_Cas_Administrative_Procedures_Targeted_Action = tr.CR_Cas_Administrative_Procedures_Targeted_Action;
                T.CR_Cas_Administrative_Procedures_Time = tr.CR_Cas_Administrative_Procedures_Time.GetValueOrDefault();
                T.CR_Cas_Administrative_Procedures_To_Branch = tr.CR_Cas_Administrative_Procedures_To_Branch;
                T.CR_Cas_Administrative_Procedures_Type = tr.CR_Cas_Administrative_Procedures_Type;
                if (T.CR_Cas_Administrative_Procedures_Type == "I")
                {
                    T.Type = "إضافة";
                }
                else if (T.CR_Cas_Administrative_Procedures_Type == "M")
                {
                    T.Type = "رسالة";
                }
                else if (T.CR_Cas_Administrative_Procedures_Type == "W")
                {
                    T.Type = "إنتظار";
                }
                T.CR_Cas_Administrative_Procedures_User_Insert = tr.CR_Cas_Administrative_Procedures_User_Insert;
                var user = db.CR_Cas_User_Information.FirstOrDefault(u => u.CR_Cas_User_Information_Id == T.CR_Cas_Administrative_Procedures_User_Insert);
                T.UserUpdate = user.CR_Cas_User_Information_Ar_Name;
                T.CR_Cas_Administrative_Procedures_Value = tr.CR_Cas_Administrative_Procedures_Value.GetValueOrDefault();
                T.CR_Cas_Administrative_Procedures_Year = tr.CR_Cas_Administrative_Procedures_Year;

                L.Add(T);
            }

            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            Tracing crystalReport = new Tracing();

            crystalReport.SetDataSource(L);
            var logo = Session["LessorLogo"].ToString();
            var log = logo.Replace("~", "");
            log = log.Replace("/", "\\");
            log = log.Substring(1, log.Length - 1);
            var lm = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + log;
            //crystalReport.SetParameterValue("Logo", System.IO.Directory.GetCurrentDirectory() + logo) ; server side
            crystalReport.SetParameterValue("Logo", lm);
            crystalReport.SetParameterValue("UserName", Session["UserName"].ToString());

            CrystalReportViewer1.ReportSource = crystalReport;
        }
    }
}