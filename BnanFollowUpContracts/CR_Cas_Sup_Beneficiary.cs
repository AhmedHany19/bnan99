//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BnanFollowUpContracts
{
    using System;
    using System.Collections.Generic;
    
    public partial class CR_Cas_Sup_Beneficiary
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CR_Cas_Sup_Beneficiary()
        {
            this.CR_Cas_Sup_Car_Information = new HashSet<CR_Cas_Sup_Car_Information>();
        }
    
        public string CR_Cas_Sup_Beneficiary_Code { get; set; }
        public string CR_Cas_Sup_Beneficiary_Lessor { get; set; }
        public string CR_Cas_Sup_Beneficiary_Commercial_Registration_No { get; set; }
        public string CR_Cas_Sup_Beneficiary_Sector { get; set; }
        public string CR_Cas_Sup_Beneficiary_Ar_Long_Name { get; set; }
        public string CR_Cas_Sup_Beneficiary_En_Long_Name { get; set; }
        public string CR_Cas_Sup_Beneficiary_Fr_Long_Name { get; set; }
        public string CR_Cas_Sup_Beneficiary_Status { get; set; }
        public string CR_Cas_Sup_Beneficiary_Reasons { get; set; }
    
        public virtual CR_Mas_Com_Lessor CR_Mas_Com_Lessor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Cas_Sup_Car_Information> CR_Cas_Sup_Car_Information { get; set; }
    }
}
