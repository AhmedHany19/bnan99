using System;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(CarInformationMD))]
    public partial class CR_Cas_Sup_Car_Information
    {
    }
    public class CarInformationMD
    {
        [Display(Name = "الرقم التسلسلي")]
        public string CR_Cas_Sup_Car_Serail_No { get; set; }
        [Display(Name = "شركة تأجير")]
        public string CR_Cas_Sup_Car_Collect_Ar_Name { get; set; }
        [Display(Name = "السيارة")]
        public string CR_Cas_Sup_Car_Collect_Ar_Short_Name { get; set; }
        [Display(Name = "السيارة")]
        public string CR_Cas_Sup_Car_Collect_En_Name { get; set; }
        public string CR_Cas_Sup_Car_Collect_En_Short_Name { get; set; }
        public string CR_Cas_Sup_Car_Collect_Fr_Name { get; set; }
        public string CR_Cas_Sup_Car_Collect_Fr_Short_Name { get; set; }
        public string CR_Cas_Sup_Car_Lessor_Code { get; set; }
        [Display(Name = "الفرع المالك لسيارة")]
        public string CR_Cas_Sup_Car_Owner_Branch_Code { get; set; }
        [Display(Name = "فرع تواجد السيارة")]
        public string CR_Cas_Sup_Car_Location_Branch_Code { get; set; }
        [Display(Name = "المالك")]
        public string CR_Cas_Sup_Car_Owner_Code { get; set; }
        [Display(Name = "المستفيد")]
        public string CR_Cas_Sup_Car_Beneficiary_Code { get; set; }
        [Display(Name = "الرقم الجمركي")]
        public string CR_Cas_Sup_Car_Customs_No { get; set; }
        [Display(Name = "رقم الهيكل")]
        public string CR_Cas_Sup_Car_Structure_No { get; set; }
        [Display(Name = "نوع التسجيل")]
        public string CR_Cas_Sup_Car_Registration_Code { get; set; }
        [Display(Name = "الماركة")]
        public string CR_Cas_Sup_Car_Brand_Code { get; set; }
        [Display(Name = "الطراز")]
        public string CR_Cas_Sup_Car_Model_Code { get; set; }
        [Display(Name = "سنة الصنع")]
        public Nullable<int> CR_Cas_Sup_Car_Year { get; set; }
        [Display(Name = "الفئة")]
        public string CR_Cas_Sup_Car_Category_Code { get; set; }
        [Display(Name = "اللون الأساسي")]
        public string CR_Cas_Sup_Car_Out_Main_Color_Code { get; set; }
        [Display(Name = "اللون الثانوي")]
        public string CR_Cas_Sup_Car_Out_Secondary_Color_Code { get; set; }
        [Display(Name = "لون المراتب")]
        public string CR_Cas_Sup_Car_In_Main_Color_Code { get; set; }
        [Display(Name = "لون الأرضية")]
        public string CR_Cas_Sup_Car_In_Secondary_Color_Code { get; set; }
        [Display(Name = "رقم اللوحة عربي")]
        public string CR_Cas_Sup_Car_Plate_Ar_No { get; set; }
        [Display(Name = "رقم اللوحة إنجليزي")]
        public string CR_Cas_Sup_Car_Plate_En_No { get; set; }
        [Display(Name = "العداد")]
        public Nullable<int> CR_Cas_Sup_Car_No_Current_Meter { get; set; }
        [Display(Name = "رخصة السير")]
        public string CR_Cas_Sup_Car_Traffic_License_Img { get; set; }
        [Display(Name = "تاريخ الإنظمام")]
        public Nullable<System.DateTime> CR_Cas_Sup_Car_Joined_Fleet_Date { get; set; }
        [Display(Name = "تاريخ الخروج")]
        public Nullable<System.DateTime> CR_Cas_Sup_Car_Left_Fleet_Date { get; set; }
        [Display(Name = "صور السيارة")]
        public string CR_Cas_Sup_Car_Last_Pictures { get; set; }
        [Display(Name = "حالة الوثائق")]
        public Nullable<bool> CR_Cas_Sup_Car_Documentation_Status { get; set; }
        [Display(Name = "حالة الصيانة")]
        public Nullable<bool> CR_Cas_Sup_Car_Maintenance_Status { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Cas_Sup_Car_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Cas_Sup_Car_Reasons { get; set; }
    }
}