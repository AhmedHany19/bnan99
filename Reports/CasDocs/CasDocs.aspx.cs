using RentCar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RentCar.Reports.CasDocs
{
    public partial class CasDocs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RentCarDBEntities db = new RentCarDBEntities();
            var LessorCode = Session["LessorCode"].ToString();

            var docs = db.CR_Cas_Sup_Car_Doc_Mainten.Where(x => x.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode);

            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CasDocsReport crystalReport = new CasDocsReport();

            crystalReport.SetDataSource(docs);
            

            CrystalReportViewer1.ReportSource = crystalReport;
        }
    }
}