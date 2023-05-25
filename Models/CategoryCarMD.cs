using System;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(CategoryCarMetaData))]
    public partial class CR_Mas_Sup_Category_Car
    {

    }

    public class CategoryCarMetaData
    {
        [Display(Name = "طرازالسيارة")]
        public string CR_Mas_Sup_Category_Model_Code { get; set; }
        [Display(Name = "سنة الصنع")]
        public int CR_Mas_Sup_Category_Car_Year { get; set; }
        [Display(Name = "الفئة")]
        public string CR_Mas_Sup_Category_Car_Code { get; set; }
        [Display(Name = "الأبواب")]
        public Nullable<int> CR_Mas_Sup_Category_Car_Door_No { get; set; }
        [Display(Name = "حقائب كبيرة")]
        public Nullable<int> CR_Mas_Sup_Category_Car_Bag_Bags { get; set; }
        [Display(Name = "حقائب صغيرة")]
        public Nullable<int> CR_Mas_Sup_Category_Car_Small_Bags { get; set; }
        [Display(Name = "الركاب")]
        public Nullable<int> CR_Mas_Sup_Category_Car_Passengers_No { get; set; }

        [Display(Name = "صورة السيارة")]
        public string CR_Mas_Sup_Category_Car_Picture { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Mas_Sup_Category_Car_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Mas_Sup_Category_Car_Reasons { get; set; }

        public virtual CR_Mas_Sup_Category CR_Mas_Sup_Category { get; set; }
        public virtual CR_Mas_Sup_Model CR_Mas_Sup_Model { get; set; }
    }
}