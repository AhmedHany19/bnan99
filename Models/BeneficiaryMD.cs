using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(BeneficiaryMD))]
    public partial class CR_Cas_Sup_Beneficiary
    {
    }

    public partial class BeneficiaryMD
    {
        [Display(Name = "الرقم الحكومي")]
        public string CR_Cas_Sup_Beneficiary_Code { get; set; }
        [Display(Name = "الشركة")]
        public string CR_Cas_Sup_Beneficiary_Lessor { get; set; }
        [Display(Name = "السجل التجاري")]
        public string CR_Cas_Sup_Beneficiary_Commercial_Registration_No { get; set; }
        [Display(Name = "القطاع")]
        public string CR_Cas_Sup_Beneficiary_Sector { get; set; }
        [Display(Name = "الإسم بالعربي")]
        public string CR_Cas_Sup_Beneficiary_Ar_Long_Name { get; set; }
        [Display(Name = "الإسم بالإنجليزي")]
        public string CR_Cas_Sup_Beneficiary_En_Long_Name { get; set; }
        public string CR_Cas_Sup_Beneficiary_Fr_Long_Name { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Cas_Sup_Beneficiary_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Cas_Sup_Beneficiary_Reasons { get; set; }

        public virtual CR_Mas_Com_Lessor CR_Mas_Com_Lessor { get; set; }
    }
}