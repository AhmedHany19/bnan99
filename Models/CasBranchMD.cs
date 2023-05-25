using System;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(CasBranchMD))]
    public partial class CR_Cas_Sup_Branch
    {

    }

    public class CasBranchMD
    {
        [Display(Name = "رمز الفرع")]
        public string CR_Cas_Sup_Branch_Code { get; set; }
        [Display(Name = "رمز الشركة")]
        public string CR_Cas_Sup_Lessor_Code { get; set; }
        [Display(Name = "إسم الفرع عربي")]
        public string CR_Cas_Sup_Branch_Ar_Name { get; set; }
        [Display(Name = "إسم الفرع مختصر")]
        public string CR_Cas_Sup_Branch_Ar_Short_Name { get; set; }
        [Display(Name = "إسم الفرع أنجليزي")]
        public string CR_Cas_Sup_Branch_En_Name { get; set; }
        [Display(Name = "إسم الفرع مختصر")]
        public string CR_Cas_Sup_Branch_En_Short_Name { get; set; }
        [Display(Name = "إسم الفرع فرنسي")]
        public string CR_Cas_Sup_Branch_Fr_Name { get; set; }
        [Display(Name = "إسم البفرع مختصر")]
        public string CR_Cas_Sup_Branch_Fr_Short_Name { get; set; }
        [Display(Name = "السجل التجاري")]
        public string CR_Cas_Sup_Branch_Commercial_Registration_No { get; set; }
        [Display(Name = "الرقم الحكومي")]
        public string CR_Cas_Sup_Branch_Government_No { get; set; }
        [Display(Name = "الرقم الضريبي")]
        public string CR_Cas_Sup_Branch_Tax_No { get; set; }
        [Display(Name = "رقم الهاتف")]
        public string CR_Cas_Sup_Branch_Tel { get; set; }
        [Display(Name = "رقم الجوال")]
        public string CR_Cas_Sup_Branch_Mobile { get; set; }
        [Display(Name = "الختم")]
        public string CR_Cas_Sup_Branch_Stamp { get; set; }
        [Display(Name = "توقيع المدير")]
        public string CR_Cas_Sup_Branch_Signature_Director { get; set; }
        [Display(Name = "إسم المدير عربي")]
        public string CR_Cas_Sup_Branch_Signature_Ar_Director_Name { get; set; }
        [Display(Name = "أسم المدير إنجليزي")]
        public string CR_Cas_Sup_Branch_Signature_En_Director_Name { get; set; }
        [Display(Name = "إسم المدير فرنسي")]
        public string CR_Cas_Sup_Branch_Signature_Fr_Director_Name { get; set; }

        public Nullable<int> CR_Cas_Sup_Branch_Start_Day { get; set; }
        public Nullable<int> CR_Cas_Sup_Branch_End_Day { get; set; }
        public Nullable<System.TimeSpan> CR_Cas_Sup_Branch_Duration_One_Start_Time { get; set; }
        public Nullable<System.TimeSpan> CR_Cas_Sup_Branch_Duration_One_End_Time { get; set; }
        public Nullable<System.TimeSpan> CR_Cas_Sup_Branch_Duration_Tow_Start_Time { get; set; }
        public Nullable<System.TimeSpan> CR_Cas_Sup_Branch_Duration_Tow_End_Time { get; set; }
        public Nullable<System.TimeSpan> CR_Cas_Sup_Branch_Duration_Three_Start_Time { get; set; }
        public Nullable<System.TimeSpan> CR_Cas_Sup_Branch_Duration_Three_End_Time { get; set; }
        public string CR_Cas_Sup_Branch_LogoMap { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Cas_Sup_Branch_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Cas_Sup_Branch_Reasons { get; set; }
    }
}