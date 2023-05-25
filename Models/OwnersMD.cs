using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(OwnersMD))]
    public partial class CR_Cas_Sup_Owners
    {
    }

    public partial class OwnersMD
    {
        [Display(Name = "الرقم الحكومي")]
        public string CR_Cas_Sup_Owners_Code { get; set; }
        [Display(Name = "الشركة")]
        public string CR_Cas_Sup_Owners_Lessor_Code { get; set; }
        [Display(Name = "السجل التجاري")]
        public string CR_Cas_Sup_Owners_Commercial_Registration_No { get; set; }
        [Display(Name = "القطاع")]
        public string CR_Cas_Sup_Owners_Sector { get; set; }
        [Display(Name = "الإسم بالعربي")]
        public string CR_Cas_Sup_Owners_Ar_Long_Name { get; set; }
        [Display(Name = "الإسم بالإنجليزي")]
        public string CR_Cas_Sup_Owners_En_Long_Name { get; set; }

        public string CR_Cas_Sup_Owners_Fr_Long_Name { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Cas_Sup_Owners_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Cas_Sup_Owners_Reasons { get; set; }


    }
}