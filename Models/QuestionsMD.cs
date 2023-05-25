using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(QuestionsMetaData))]
    public partial class CR_Mas_Msg_Questions_Answer
    {
    }
    public class QuestionsMetaData
    {
        [Display(Name = "الرمز")]
        public string CR_Mas_Msg_Questions_Answer_No { get; set; }

        public string CR_Mas_Msg_Tasks_Code { get; set; }
        [Display(Name = "السؤال عربي")]
        public string CR_Mas_Msg_Ar_Questions { get; set; }
        [Display(Name = "الجواب عربي")]
        public string CR_Mas_Msg_Ar_Answer { get; set; }
        [Display(Name = "السؤال إنجليزي")]
        public string CR_Mas_Msg_En_Questions { get; set; }
        [Display(Name = "الجواب إنجليزي")]
        public string CR_Mas_Msg_En_Answer { get; set; }
        [Display(Name = "السؤال فرنسي")]
        public string CR_Mas_Msg_Fr_Questions { get; set; }
        [Display(Name = "الجواب فرنسي")]
        public string CR_Mas_Msg_Fr_Answer { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Mas_Msg_Questions_Answer_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Mas_Msg_Questions_Answer_Reasons { get; set; }

        public virtual CR_Mas_Sys_Tasks CR_Mas_Sys_Tasks { get; set; }
    }
}