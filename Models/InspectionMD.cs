using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(InspectionMD))]
    public partial class CR_Mas_Sup_Virtual_Inspection
    {
    }

    public partial class InspectionMD
    {
        [Display(Name = "الرمز")]
        public string CR_Mas_Sup_Virtual_Inspection_Code { get; set; }
        [Display(Name = "النوع")]
        public string CR_Mas_Sup_Virtual_Inspection_Type { get; set; }
        [Display(Name = "الإسم بالعربي")]
        public string CR_Mas_Sup_Virtual_Inspection_Ar_Name { get; set; }
        [Display(Name = "الإسم بالإنجليزي")]
        public string CR_Mas_Sup_Virtual_Inspection_En_Name { get; set; }
        [Display(Name = "الإسم بالفرنسي")]
        public string CR_Mas_Sup_Virtual_Inspection_Fr_Name { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Mas_Sup_Virtual_Inspection_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Mas_Sup_Virtual_Inspection_Reasons { get; set; }
    }
}