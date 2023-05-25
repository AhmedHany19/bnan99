using System;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(ModelCategoryMetaData))]
    public partial class CR_Mas_Sup_Car_Model_Category
    {

    }

    public class ModelCategoryMetaData
    {
        [Display(Name = "طرازالسيارة")]
        public string CR_Mas_Sup_Car_Model_Category_Code { get; set; }
        [Display(Name = "سنة الصنع")]
        public int CR_Mas_Sup_Car_Model_Category_Year { get; set; }
        [Display(Name = "الفئة")]
        public string CR_Mas_Sup_Car_Category_Code { get; set; }
        [Display(Name = "الأبواب")]
        public Nullable<int> CR_Mas_Sup_Car_Model_Category_Door_No { get; set; }
        [Display(Name = "حقائب كبيرة")]
        public Nullable<int> CR_Mas_Sup_Car_Model_Category_Bag_Bags { get; set; }
        [Display(Name = "حقائب صغيرة")]
        public Nullable<int> CR_Mas_Sup_Car_Model_Category_Small_Bags { get; set; }
        [Display(Name = "الركاب")]
        public Nullable<int> CR_Mas_Sup_Car_Model_Category_Passengers_No { get; set; }
        [Display(Name = "وزن السيارة")]
        public Nullable<int> CR_Mas_Sup_Car_Model_Category_Weight { get; set; }
        [Display(Name = "كم سلندر")]
        public Nullable<int> CR_Mas_Sup_Car_Model_Category_Clinder { get; set; }
        [Display(Name = "كم حصان")]
        public Nullable<int> CR_Mas_Sup_Car_Model_Category_Hourses { get; set; }
        [Display(Name = "حمولة المركبة")]
        public Nullable<int> CR_Mas_Sup_Car_Model_Category_Payload { get; set; }
        [Display(Name = "صورة السيارة")]
        public string CR_Mas_Sup_Car_Model_Category_Picture { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Mas_Sup_Car_Model_Category_Status { get; set; }
        [Display(Name = "المرجع")]
        public string CR_Mas_Sup_Car_Model_Category_Reasons { get; set; }

        public virtual CR_Mas_Sup_Category_Car CR_Mas_Sup_Category_Car { get; set; }
        public virtual CR_Mas_Sup_Model CR_Mas_Sup_Model { get; set; }
    }
}