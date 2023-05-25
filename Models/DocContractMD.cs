using System;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{

    [MetadataType(typeof(DocContractMD))]
    public partial class CR_Cas_Sup_Branch_Documentation
    {
    }

    public class DocContractMD
    {
        [Display(Name = "رمز الشركة")]
        public string CR_Cas_Sup_Branch_Documentation_Lessor_Code { get; set; }
        [Display(Name = "الرمز")]
        public string CR_Cas_Sup_Branch_Documentation_Branch_Code { get; set; }
        [Display(Name = "رمز المستند")]
        public string CR_Cas_Sup_Branch_Documentation_Code { get; set; }
        [Display(Name = "نوع الإجراء")]
        public string CR_Cas_Sup_Procedures_Type { get; set; }
        [Display(Name = "رقم المستند")]
        public string CR_Cas_Sup_Branch_Documentation_No { get; set; }
        [Display(Name = "تاريخه")]
        public Nullable<System.DateTime> CR_Cas_Sup_Branch_Documentation_Date { get; set; }
        [Display(Name = "بدايته")]
        public Nullable<System.DateTime> CR_Cas_Sup_Branch_Documentation_Start_Date { get; set; }
        [Display(Name = "نهايته")]
        public Nullable<System.DateTime> CR_Cas_Sup_Branch_Documentation_End_Date { get; set; }
        [Display(Name = "التفعيل")]
        public Nullable<bool> CR_Cas_Sup_Branch_Documentation_Activation { get; set; }
        [Display(Name = "السقف الإئتماني")]
        public Nullable<decimal> CR_Cas_Sup_Branch_Documentation_Credit_Limit { get; set; }
        [Display(Name = "تاريخ التنبيه")]
        public Nullable<System.DateTime> CR_Cas_Sup_Branch_Documentation_About_To_Expire { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Cas_Sup_Branch_Documentation_Status { get; set; }

        public virtual CR_Cas_Sup_Branch CR_Cas_Sup_Branch { get; set; }
        public virtual CR_Mas_Sup_Procedures CR_Mas_Sup_Procedures { get; set; }
    }
}