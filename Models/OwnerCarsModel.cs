using System.Collections.Generic;

namespace RentCar.Models
{
    public class OwnerCarsModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OwnerCarsModel()
        {
            this.CR_Cas_Sup_Car_Information = new HashSet<CR_Cas_Sup_Car_Information>();
        }

        public string CR_Cas_Sup_Owners_Code { get; set; }
        public string CR_Cas_Sup_Owners_Lessor_Code { get; set; }
        public string CR_Cas_Sup_Owners_Commercial_Registration_No { get; set; }
        public string CR_Cas_Sup_Owners_Sector { get; set; }
        public string CR_Cas_Sup_Owners_Ar_Long_Name { get; set; }
        public string CR_Cas_Sup_Owners_En_Long_Name { get; set; }
        public string CR_Cas_Sup_Owners_Fr_Long_Name { get; set; }
        public string CR_Cas_Sup_Owners_Status { get; set; }
        public string CR_Cas_Sup_Owners_Reasons { get; set; }
        public int CarsNumber { get; set; }

        public virtual CR_Mas_Com_Lessor CR_Mas_Com_Lessor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Cas_Sup_Car_Information> CR_Cas_Sup_Car_Information { get; set; }
    }
}