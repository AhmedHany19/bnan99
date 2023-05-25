using System.Collections.Generic;

namespace RentCar.Models
{
    public class BranchCarsModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BranchCarsModel()
        {
            this.CR_Cas_Sup_Branch_Documentation = new HashSet<CR_Cas_Sup_Branch_Documentation>();
            this.CR_Cas_Sup_Car_Information = new HashSet<CR_Cas_Sup_Car_Information>();
        }

        public string CR_Cas_Sup_Branch_Code { get; set; }
        public string CR_Cas_Sup_Lessor_Code { get; set; }
        public string CR_Cas_Sup_Branch_Ar_Name { get; set; }
        public string CR_Cas_Sup_Branch_Ar_Short_Name { get; set; }
        public string CR_Cas_Sup_Branch_En_Name { get; set; }
        public string CR_Cas_Sup_Branch_En_Short_Name { get; set; }
        public string CR_Cas_Sup_Branch_Fr_Name { get; set; }
        public string CR_Cas_Sup_Branch_Fr_Short_Name { get; set; }
        public string CR_Cas_Sup_Branch_Commercial_Registration_No { get; set; }
        public string CR_Cas_Sup_Branch_Government_No { get; set; }
        public string CR_Cas_Sup_Branch_Tax_No { get; set; }
        public string CR_Cas_Sup_Branch_Tel { get; set; }
        public string CR_Cas_Sup_Branch_Mobile { get; set; }
        public string CR_Cas_Sup_Branch_Stamp { get; set; }
        public string CR_Cas_Sup_Branch_Signature_Director { get; set; }
        public string CR_Cas_Sup_Branch_Signature_Ar_Director_Name { get; set; }
        public string CR_Cas_Sup_Branch_Signature_En_Director_Name { get; set; }
        public string CR_Cas_Sup_Branch_Signature_Fr_Director_Name { get; set; }
        public int CR_Cas_Sup_Branch_Start_Day { get; set; }
        public int CR_Cas_Sup_Branch_End_Day { get; set; }
        public System.TimeSpan CR_Cas_Sup_Branch_Duration_One_Start_Time { get; set; }
        public System.TimeSpan CR_Cas_Sup_Branch_Duration_One_End_Time { get; set; }
        public System.TimeSpan CR_Cas_Sup_Branch_Duration_Tow_Start_Time { get; set; }
        public System.TimeSpan CR_Cas_Sup_Branch_Duration_Tow_End_Time { get; set; }
        public System.TimeSpan CR_Cas_Sup_Branch_Duration_Three_Start_Time { get; set; }
        public System.TimeSpan CR_Cas_Sup_Branch_Duration_Three_End_Time { get; set; }
        public string CR_Cas_Sup_Branch_LogoMap { get; set; }
        public string CR_Cas_Sup_Branch_Status { get; set; }
        public string CR_Cas_Sup_Branch_Reasons { get; set; }
        public string CarsNumber { get; set; }
        public string cityName { get; set; }
        public string Region { get; set; }
        public virtual CR_Mas_Com_Lessor CR_Mas_Com_Lessor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Cas_Sup_Branch_Documentation> CR_Cas_Sup_Branch_Documentation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Cas_Sup_Car_Information> CR_Cas_Sup_Car_Information { get; set; }
    }
}