using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(BrandMetaData))]
    public partial class CR_Mas_Sup_Brand
    {
    }
    public class BrandMetaData
    {
        [Display(Name = "الرمز")]
        public string CR_Mas_Sup_Brand_Code { get; set; }
        [Display(Name = "الماركة عربي")]
        public string CR_Mas_Sup_Brand_Ar_Name { get; set; }
        [Display(Name = "الماركة إنجليزي")]
        public string CR_Mas_Sup_Brand_En_Name { get; set; }
        [Display(Name = "الماركة فرنسي")]
        public string CR_Mas_Sup_Brand_Fr_Name { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Mas_Sup_Brand_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Mas_Sup_Brand_Reasons { get; set; }
    }
}