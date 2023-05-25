using System;
using System.ComponentModel.DataAnnotations;



namespace RentCar.Models
{
    [MetadataType(typeof(ContractTammMD))]
    public partial class CR_Mas_Basic_Contract
    {
    }

    public class ContractTammMD
    {
        [Display(Name = "رقم العقد")]
        public string CR_Mas_Basic_Contract_No { get; set; }
        public string CR_Mas_Basic_Contract_Year { get; set; }
        [Display(Name = "القطاع")]
        public string CR_Mas_Basic_Contract_Sector { get; set; }
        [Display(Name = "رمز الإجراء")]
        public string CR_Mas_Basic_Contract_Code { get; set; }
        [Display(Name = "رمز الشركة")]
        public string CR_Mas_Basic_Contract_Lessor { get; set; }
        [Display(Name = "رقم الشركة المساندة")]
        public Nullable<int> CR_Mas_Basic_Contract_Com_Code { get; set; }
        [Display(Name = "تاريخه")]
        public Nullable<System.DateTime> CR_Mas_Basic_Contract_Date { get; set; }
        [Display(Name = "بدايته")]
        public Nullable<System.DateTime> CR_Mas_Basic_Contract_Start_Date { get; set; }
        [Display(Name = "نهايته")]
        public Nullable<System.DateTime> CR_Mas_Basic_Contract_End_Date { get; set; }
        [Display(Name = "رسوم الإشتراك")]
        public Nullable<decimal> CR_Mas_Basic_Contract_Annual_Fees { get; set; }
        [Display(Name = "رسوم الخدمة")]
        public Nullable<decimal> CR_Mas_Basic_Contract_Service_Fees { get; set; }
        [Display(Name = "نسبة الخصم")]
        public Nullable<decimal> CR_Mas_Basic_Contract_Discount_Rate { get; set; }
        [Display(Name = "نسبة الضريبة")]
        public Nullable<decimal> CR_Mas_Basic_Contract_Tax_Rate { get; set; }
        [Display(Name = "المستخدم")]
        public string CR_Mas_Basic_Contract_Tamm_User_Id { get; set; }
        [Display(Name = "كلمة السر")]
        public string CR_Mas_Basic_Contract_Tamm_User_PassWord { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Mas_Basic_Contract_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Mas_Basic_Contract_Reasons { get; set; }
    }
}