using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(LessorMetaData))]
    public partial class CR_Mas_Com_Lessor
    {
    }

    public partial class LessorMetaData
    {

        [Display(Name = "الرمز")]
        public string CR_Mas_Com_Lessor_Code { get; set; }
        [Display(Name = "الشعار")]
        public string CR_Mas_Com_Lessor_Logo { get; set; }
        [Display(Name = "إسم الشركة عربي")]
        public string CR_Mas_Com_Lessor_Ar_Long_Name { get; set; }
        [Display(Name = "إسم الشركة مختصر")]
        public string CR_Mas_Com_Lessor_Ar_Short_Name { get; set; }
        [Display(Name = "إسم الشركة إنجليزي")]
        public string CR_Mas_Com_Lessor_En_Long_Name { get; set; }
        [Display(Name = "إسم الشركة مختصر")]
        public string CR_Mas_Com_Lessor_En_Short_Name { get; set; }
        [Display(Name = "إسم الشركة فرنسي")]
        public string CR_Mas_Com_Lessor_Fr_Long_Name { get; set; }
        [Display(Name = "إسم الشركة مختصر")]
        public string CR_Mas_Com_Lessor_Fr_Short_Name { get; set; }
        [Display(Name = "الموقع")]
        public string CR_Mas_Com_Lessor_Location_Coordinates { get; set; }
        [Display(Name = "رقم السجل التجاري")]
        public string CR_Mas_Com_Lessor_Commercial_Registration_No { get; set; }
        [Display(Name = "القطاع")]
        public string CR_Mas_Com_Lessor_Sector { get; set; }
        [Display(Name = "الرقم الحكومي")]
        public string CR_Mas_Com_Lessor_Government_No { get; set; }
        [Display(Name = "الرقم الضريبي")]
        public string CR_Mas_Com_Lessor_Tax_Number { get; set; }
        [Display(Name = "إسم موظف الإتصال عربي")]
        public string CR_Mas_Com_Lessor_Communication_Ar_Officer_Name { get; set; }
        [Display(Name = "إسم موظف الإتصال إنجليزي")]
        public string CR_Mas_Com_Lessor_Communication_En_Officer_Name { get; set; }
        [Display(Name = "إسم موظف الإتصال فرنسي")]
        public string CR_Mas_Com_Lessor_Communication_Fr_Officer_Name { get; set; }
        [Display(Name = " البريد الإلكتروني للموظف")]
        public string CR_Mas_Com_Lessor_Communication_Officer_Emaile { get; set; }
        [Display(Name = "رقم جوال الموظف")]
        public string CR_Mas_Com_Lessor_Communication_Officer_Mobile { get; set; }
        [Display(Name = "رقم التواصل المجاني")]
        public string CR_Mas_Com_Lessor_Tolk_Free { get; set; }
        [Display(Name = "البريد الإلكتروني للشركة")]
        public string CR_Mas_Com_Lessor_Email { get; set; }
        [Display(Name = "تويتر ")]
        public string CR_Mas_Com_Lessor_Twiter { get; set; }
        [Display(Name = "فايسبوك")]
        public string CR_Mas_Com_Lessor_FaceBook { get; set; }
        [Display(Name = "إنستجرام")]
        public string CR_Mas_Com_Lessor_Instagram { get; set; }
        [Display(Name = "الختم")]
        public string CR_Mas_Com_Lessor_Stamp { get; set; }
        [Display(Name = "توقيع المدير")]
        public string CR_Mas_Com_Lessor_Signature_Director { get; set; }
        [Display(Name = "إسم المدير عربي")]
        public string CR_Mas_Com_Lessor_Signature_Ar_Director_Name { get; set; }
        [Display(Name = "إسم المدير إنجليزي")]
        public string CR_Mas_Com_Lessor_Signature_En_Director_Name { get; set; }
        [Display(Name = "إسم المدير فرنسي")]
        public string CR_Mas_Com_Lessor_Signature_Fr_Director_Name { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Mas_Com_Lessor_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Mas_Com_Lessor_Reasons { get; set; }
    }
}