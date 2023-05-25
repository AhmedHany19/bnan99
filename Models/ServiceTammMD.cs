using System;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{

    [MetadataType(typeof(ServiceTammMD))]
    public partial class CR_Mas_Sup_Service_Fee_Tamm
    {
    }

    public class ServiceTammMD
    {
        [Display(Name = "الرمز")]
        public string CR_Mas_Sup_Service_Fee_Tamm_Code { get; set; }
        [Display(Name = "الإسم بالعربي")]
        public string CR_Mas_Sup_Service_Fee_Tamm_Ar_Name { get; set; }
        [Display(Name = "الإسم بالفرنسي")]
        public string CR_Mas_Sup_Service_Fee_Tamm_En_Name { get; set; }
        [Display(Name = "الإسم بالإنجليزي")]
        public string CR_Mas_Sup_Service_Fee_Tamm_Fr_Name { get; set; }
        [Display(Name = "القيمة")]
        public Nullable<decimal> CR_Mas_Sup_Service_Fee_Tamm_Value { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Mas_Sup_Service_Fee_Tamm_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Mas_Sup_Service_Fee_Tamm_Reasons { get; set; }
    }
}