using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(ChoicesMetaData))]
    public partial class CR_Mas_Sup_Choices
    {
    }
    public class ChoicesMetaData
    {
        [Display(Name = "الرمز")]
        public string CR_Mas_Sup_Choices_Code { get; set; }
        [Display(Name = "المجموعة")]
        public string CR_Mas_Sup_Choices_Group_Code { get; set; }
        [Display(Name = "الخيار عربي")]
        public string CR_Mas_Sup_Choices_Ar_Name { get; set; }
        [Display(Name = "الخيار إنجليزي")]
        public string CR_Mas_Sup_Choices_En_Name { get; set; }
        [Display(Name = "الخيار فرنسي")]
        public string CR_Mas_Sup_Choices_Fr_Name { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Mas_Sup_Choices_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Mas_Sup_Choices_Reasons { get; set; }

        public virtual CR_Mas_Sup_Group CR_Mas_Sup_Group { get; set; }
    }
}