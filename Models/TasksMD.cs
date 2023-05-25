using System;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(TasksMetaData))]
    public partial class CR_Mas_Sys_Tasks
    {
    }
    public class TasksMetaData
    {
        [Display(Name = "الرمز")]
        public string CR_Mas_Sys_Tasks_Code { get; set; }
        [Display(Name = "النظام")]
        public string CR_Mas_Sys_System_Code { get; set; }
        [Display(Name = "الشاشة عربي")]
        public string CR_Mas_Sys_Tasks_Ar_Name { get; set; }
        [Display(Name = "الشاشة إنجليزي")]
        public string CR_Mas_Sys_Tasks_En_Name { get; set; }
        [Display(Name = "الشاشة فرنسي")]
        public string CR_Mas_Sys_Tasks_Fr_Name { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Mas_Sys_Tasks_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Mas_Sys_Tasks_Reasons { get; set; }
        public Nullable<bool> CR_Mas_Sys_Tasks_Main_Validation { get; set; }
        public Nullable<bool> CR_Mas_Sys_Tasks_Sub_Validation { get; set; }

        public virtual CR_Mas_Sys_System_Name CR_Mas_Sys_System_Name { get; set; }
    }
}