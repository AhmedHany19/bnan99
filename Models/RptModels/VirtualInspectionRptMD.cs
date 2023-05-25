using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentCar.Models.RptModels
{
    public class VirtualInspectionRptMD
    {
        public int CR_Cas_Contract_Virtual_Inspection_Code { get; set; }
        public string CR_Mas_Sup_Virtual_Inspection_Ar_Name { get; set; }
        public string CR_Mas_Sup_Virtual_Inspection_En_Name { get; set; }
        public string CR_Mas_Sup_Virtual_Inspection_Fr_Name { get; set; }
        public  bool CR_Cas_Contract_Virtual_Inspection_Action { get; set; }
        public string CR_Cas_Contract_Virtual_Inspection_Remarks { get; set; }
    }
}