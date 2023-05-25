using System;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(CityMetaData))]
    public partial class CR_Mas_Sup_City
    {
    }
    public class CityMetaData
    {
        [Display(Name = "الرمز")]
        public string CR_Mas_Sup_City_Code { get; set; }
        [Display(Name = "المجموعة")]
        public string CR_Mas_Sup_City_Group_Code { get; set; }
        [Display(Name = "المدينة عربي")]
        public string CR_Mas_Sup_City_Ar_Name { get; set; }
        [Display(Name = "المدينة إنجليزي")]
        public string CR_Mas_Sup_City_En_Name { get; set; }
        [Display(Name = "المدينة فرنسي")]
        public string CR_Mas_Sup_City_Fr_Name { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Mas_Sup_City_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Mas_Sup_City_Reasons { get; set; }
        public string CR_Mas_Sup_City_Regions_Code { get; set; }
        [Display(Name = "إحداثيات الموقع")]
        public string CR_Mas_Sup_City_Location_Coordinates { get; set; }
        public Nullable<int> CR_Mas_Sup_City_Counter { get; set; }
        [Display(Name = "المنطقة")]
        public virtual CR_Mas_Sup_Regions CR_Mas_Sup_Regions { get; set; }
        public virtual CR_Mas_Sup_Group CR_Mas_Sup_Group { get; set; }
    }
}