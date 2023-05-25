using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(SpecificationsMetaData))]
    public partial class CR_Mas_Sup_Specifications
    {
    }
    public class SpecificationsMetaData
    {
        [Display(Name = "الرمز")]
        public string CR_Mas_Sup_Specifications_Code { get; set; }
        [Display(Name = "المواصفة عربي")]
        public string CR_Mas_Sup_Specifications_Ar_Name { get; set; }
        [Display(Name = "المواصفة إنجليزي")]
        public string CR_Mas_Sup_Specifications_En_Name { get; set; }
        [Display(Name = "المواصفة فرنسي")]
        public string CR_Mas_Sup_Specifications_Fr_Name { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Mas_Sup_Specifications_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Mas_Sup_Specifications_Reasons { get; set; }
    }
}