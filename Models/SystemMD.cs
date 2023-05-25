using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(SystemMetaData))]
    public partial class CR_Mas_Sys_System_Name
    {
    }
    public class SystemMetaData
    {
        [Display(Name = "الرمز")]
        public string CR_Mas_Sys_System_Code { get; set; }
        [Display(Name = "الإسم بالعربي")]
        public string CR_Mas_Sys_System_Ar_Name { get; set; }
        [Display(Name = "الإسم بالإنجليزي")]
        public string CR_Mas_Sys_System_En_Name { get; set; }
        [Display(Name = "الإسم بالفرنسي")]
        public string CR_Mas_Sys_System_Fr_Name { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Mas_Sys_System_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Mas_Sys_System_Reasons { get; set; }
    }
}