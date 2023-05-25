using System;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(SubValidationMD))]
    public partial class CR_Mas_User_Sub_Validation
    {
    }

    public partial class SubValidationMD
    {

        [Display(Name = "المستخدم")]
        public string CR_Mas_User_Sub_Validation_Code { get; set; }
        [Display(Name = "الشاشة")]
        public string CR_Mas_User_Sub_Validation_Tasks_Code { get; set; }
        [Display(Name = "إضافة")]
        public Nullable<bool> CR_Mas_User_Sub_Validation_Insert { get; set; }
        [Display(Name = "تعديل")]
        public Nullable<bool> CR_Mas_User_Sub_Validation_UpDate { get; set; }
        [Display(Name = "حذف")]
        public Nullable<bool> CR_Mas_User_Sub_Validation_Delete { get; set; }
        [Display(Name = "إسترجاع")]
        public Nullable<bool> CR_Mas_User_Sub_Validation_UnDelete { get; set; }
        [Display(Name = "إيقاف")]
        public Nullable<bool> CR_Mas_User_Sub_Validation_Hold { get; set; }
        [Display(Name = "تنشيط")]
        public Nullable<bool> CR_Mas_User_Sub_Validation_UnHold { get; set; }
        [Display(Name = "طباعة")]
        public Nullable<bool> CR_Mas_User_Sub_Validation_Print { get; set; }



        public string CR_Mas_User_Sub_Validation_TaskName { get; set; }
        public string CR_Mas_User_Sub_Validation_UserName { get; set; }
        public Nullable<bool> CR_Mas_User_Main_Validation1 { get; set; }

        public virtual CR_Mas_User_Information CR_Mas_User_Information { get; set; }
    }
}