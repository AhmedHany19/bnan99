using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(MembershipMetaData))]
    public partial class CR_Mas_Sup_Membership
    {
    }
    public class MembershipMetaData
    {
        [Display(Name = "الرمز")]
        public string CR_Mas_Sup_Membership_Code { get; set; }
        public string CR_Mas_Sup_Membership_Group_Code { get; set; }
        [Display(Name = "العضوية عربي")]
        public string CR_Mas_Sup_Membership_Ar_Name { get; set; }
        [Display(Name = "العضوية إنجليزي")]
        public string CR_Mas_Sup_Membership_En_Name { get; set; }
        [Display(Name = "العضوية فرنسي")]
        public string CR_Mas_Sup_Membership_Fr_Name { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Mas_Sup_Membership_Status { get; set; }
        [Display(Name = "الملاحظات")]
        public string CR_Mas_Sup_Membership_Reasons { get; set; }

        public virtual CR_Mas_Sup_Group CR_Mas_Sup_Group { get; set; }
    }
}