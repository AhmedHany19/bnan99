using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(CategoryMetaData))]
    public partial class CR_Mas_Sup_Category
    {
    }
    public class CategoryMetaData
    {
        [Display(Name = "الرمز")]
        public string CR_Mas_Sup_Category_Code { get; set; }
        [Display(Name = "المجموعة")]
        public string CR_Mas_Sup_Category_Group_Code { get; set; }
        [Display(Name = "الفئة عربي")]
        public string CR_Mas_Sup_Category_Ar_Name { get; set; }
        [Display(Name = "الفئة إنجليزي")]
        public string CR_Mas_Sup_Category_En_Name { get; set; }
        [Display(Name = "الفئة فرنسي")]
        public string CR_Mas_Sup_Category_Fr_Name { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Mas_Sup_Category_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Mas_Sup_Category_Reasons { get; set; }
        public virtual CR_Mas_Sup_Group CR_Mas_Sup_Group { get; set; }
    }
}