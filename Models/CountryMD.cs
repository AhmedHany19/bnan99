using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(CountryMetaData))]
    public partial class CR_Mas_Sup_Country
    {
    }
    public class CountryMetaData
    {
        [Display(Name = "الرمز")]
        public string CR_Mas_Sup_Country_Code { get; set; }
        [Display(Name = "الإسم بالعربي")]
        public string CR_Mas_Sup_Country_Ar_Name { get; set; }
        [Display(Name = "الإسم بالإنجليزي")]
        public string CR_Mas_Sup_Country_En_Name { get; set; }
        [Display(Name = "الإسم بالفرنسي")]
        public string CR_Mas_Sup_Country_Fr_Name { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Mas_Sup_Country_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Mas_Sup_Country_Reasons { get; set; }
    }
}