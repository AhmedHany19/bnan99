using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(JobsMetaData))]
    public partial class CR_Mas_Sup_Jobs
    {
    }
    public class JobsMetaData
    {
        [Display(Name = "الرمز")]
        public string CR_Mas_Sup_Jobs_Code { get; set; }
        [Display(Name = "المجموعة")]
        public string CR_Mas_Sup_Jobs_Group_Code { get; set; }
        [Display(Name = "المهنة عربي")]
        public string CR_Mas_Sup_Jobs_Ar_Name { get; set; }
        [Display(Name = "المهنة إنجليزي")]
        public string CR_Mas_Sup_Jobs_En_Name { get; set; }
        [Display(Name = "المهنة فرنسي")]
        public string CR_Mas_Sup_Jobs_Fr_Name { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Mas_Sup_Jobs_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Mas_Sup_Jobs_Reasons { get; set; }

        public virtual CR_Mas_Sup_Group CR_Mas_Sup_Group { get; set; }
    }
}