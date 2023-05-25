using System.ComponentModel.DataAnnotations;

namespace RentCar.Models
{
    [MetadataType(typeof(UserInfMD))]
    public partial class CR_Mas_User_Information
    {
    }
    public class UserInfMD
    {
        [Display(Name = "الرمز")]
        public string CR_Mas_User_Information_Code { get; set; }
        [Required(ErrorMessage = "الرقم السري إجباري")]
        [Display(Name = "الرقم السري")]
        public string CR_Mas_User_Information_PassWord { get; set; }
        [Display(Name = "المستخدم عربي")]
        public string CR_Mas_User_Information_Ar_Name { get; set; }
        [Display(Name = "المستخدم إنجليزي")]
        public string CR_Mas_User_Information_En_Name { get; set; }
        [Display(Name = "المستخدم فرنسي")]
        public string CR_Mas_User_Information_Fr_Name { get; set; }
        [Required(ErrorMessage = "رقم الهاتف إجباري")]
        [Display(Name = "رقم الهاتف")]
        public string CR_Mas_User_Information_Mobile_No { get; set; }
        [Display(Name = "الحالة")]
        public string CR_Mas_User_Information_Status { get; set; }
        [Display(Name = "الملاحظات")]

        

        public string CR_Mas_User_Information_Reasons { get; set; }
    }
}