using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(AdditionalMD))]
    public partial class CR_Mas_Sup_Additional
    {
    }

    public class AdditionalMD
    {
        [Display(Name = "الرمز")]
        public string CR_Mas_Sup_Additional_Code { get; set; }
        public string CR_Mas_Sup_Additional_Group_Code { get; set; }
        [Display(Name = "الإظافة عربي")]
        public string CR_Mas_Sup_Additional_Ar_Name { get; set; }
        [Display(Name = "الإظافة إنجليزي")]
        public string CR_Mas_Sup_Additional_En_Name { get; set; }
        [Display(Name = "الإظافة فرنسي")]
        public string CR_Mas_Sup_Additional_Fr_Name { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Mas_Sup_Additional_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Mas_Sup_Additional_Reasons { get; set; }
        [Display(Name = "المجموعة")]
        public virtual CR_Mas_Sup_Group CR_Mas_Sup_Group { get; set; }
    }
}