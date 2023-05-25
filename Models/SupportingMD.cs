using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(SupportingMetaData))]
    public partial class CR_Mas_Com_Supporting
    {
    }
    public class SupportingMetaData
    {
        [Display(Name = "الرمز")]
        public string CR_Mas_Com_Supporting_Code { get; set; }
        [Display(Name = "النوع")]
        public string CR_Mas_Com_Supporting_Type { get; set; }
        [Display(Name = "الإسم بالعربي")]
        public string CR_Mas_Com_Supporting_Ar_Long_Name { get; set; }
        [Display(Name = "الإسم المختصر بالعربي")]
        public string CR_Mas_Com_Supporting_Ar_Short_Name { get; set; }
        [Display(Name = "الإسم بالإنجليزي")]
        public string CR_Mas_Com_Supporting_En_Long_Name { get; set; }
        [Display(Name = "الإسم المختصر بالإنجليزي")]
        public string CR_Mas_Com_Supporting_En_Short_Name { get; set; }
        [Display(Name = "الإسم بالفرنسي")]
        public string CR_Mas_Com_Supporting_Fr_Long_Name { get; set; }
        [Display(Name = "الإسم المختصر بالفرنسي")]
        public string CR_Mas_Com_Supporting_Fr_Short_Name { get; set; }
        [Display(Name = "رقم الجوال")]
        public string CR_Mas_Com_Supporting_Mobile_Number { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Mas_Com_Supporting_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Mas_Com_Supporting_Reasons { get; set; }
    }
}