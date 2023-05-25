using System;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(ProceduresMD))]
    public partial class CR_Mas_Sup_Procedures
    {
    }

    public class ProceduresMD
    {
        [Display(Name = "الرمز")]
        public string CR_Mas_Sup_Procedures_Code { get; set; }
        [Display(Name = "النوع")]
        public string CR_Mas_Sup_Procedures_Type { get; set; }
        [Display(Name = "الإسم بالعربي")]
        public string CR_Mas_Sup_Procedures_Ar_Name { get; set; }
        [Display(Name = "الإسم بالإنجليزي")]
        public string CR_Mas_Sup_Procedures_En_Name { get; set; }
        [Display(Name = "الإسم بالفرنسي")]
        public string CR_Mas_Sup_Procedures_Fr_Name { get; set; }
        [Display(Name = "الحد الإئتماني")]
        public Nullable<decimal> CR_Mas_Sup_Procedures_FollowUp_Befor_Credit_Limit { get; set; }
        [Display(Name = "عدد أيام التنبيه")]
        public Nullable<int> CR_Mas_Sup_Procedures_FollowUp_Befor_Expire { get; set; }
        [Display(Name = "عدد كيلوات التنبيه")]
        public Nullable<int> CR_Mas_Sup_Procedures_FollowUp_Befor_KM { get; set; }
        [Display(Name = "الأيام الإفتراضية")]
        public Nullable<int> CR_Mas_Sup_Procedures_FollowUp_Befor_Expire_KM { get; set; }
        [Display(Name = "الكيلوات الإفتراضية")]
        public Nullable<int> CR_Mas_Sup_Procedures_FollowUp_Default_KM { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Mas_Sup_Procedures_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Mas_Sup_Procedures_Reasons { get; set; }

    }
}