using System;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(ModelMetaData))]
    public partial class CR_Mas_Sup_Model
    {
    }
    public class ModelMetaData
    {
        [Display(Name = "الرمز")]
        public string CR_Mas_Sup_Model_Code { get; set; }
        [Display(Name = "المجموعة")]
        public string CR_Mas_Sup_Model_Group_Code { get; set; }
        [Display(Name = "الماركة")]
        public string CR_Mas_Sup_Model_Brand_Code { get; set; }
        [Display(Name = "الطراز عربي")]
        public string CR_Mas_Sup_Model_Ar_Name { get; set; }
        [Display(Name = "الطراز إنجليزي")]
        public string CR_Mas_Sup_Model_En_Name { get; set; }
        [Display(Name = "الطراز فرنسي")]
        public string CR_Mas_Sup_Model_Fr_Name { get; set; }
        [Display(Name = "رقم العداد")]
        public Nullable<int> CR_Mas_Sup_Model_Counter { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Mas_Sup_Model_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Mas_Sup_Model_Reasons { get; set; }
        public virtual CR_Mas_Sup_Brand CR_Mas_Sup_Brand { get; set; }
        public virtual CR_Mas_Sup_Group CR_Mas_Sup_Group { get; set; }
    }
}