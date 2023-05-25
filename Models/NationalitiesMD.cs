using System;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(NationalitiesMetaData))]
    public partial class CR_Mas_Sup_Nationalities
    {
    }
    public class NationalitiesMetaData
    {
        [Display(Name = "الرمز")]
        public string CR_Mas_Sup_Nationalities_Code { get; set; }
        [Display(Name = "المجموعة")]
        public string CR_Mas_Sup_Nationalities_Group_Code { get; set; }
        public string CR_Mas_Sup_Nationalities_Country_Code { get; set; }
        [Display(Name = "الجنسية عربي")]
        public string CR_Mas_Sup_Nationalities_Ar_Name { get; set; }
        [Display(Name = "الجنسية إنجليزي")]
        public string CR_Mas_Sup_Nationalities_En_Name { get; set; }
        [Display(Name = "الجنسية فرنسي")]
        public string CR_Mas_Sup_Nationalities_Fr_Name { get; set; }
        [Display(Name = "رقم العداد")]
        public Nullable<int> CR_Mas_Sup_Nationalities_Counter { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Mas_Sup_Nationalities_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Mas_Sup_Nationalities_Reasons { get; set; }
        [Display(Name = "الدولة")]
        public virtual CR_Mas_Sup_Country CR_Mas_Sup_Country { get; set; }
        [Display(Name = "المجموعة")]
        public virtual CR_Mas_Sup_Group CR_Mas_Sup_Group { get; set; }
    }
}