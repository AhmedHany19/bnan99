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
    
    public partial class CR_Mas_Sup_Procedures
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CR_Mas_Sup_Procedures()
        {
            this.CR_Cas_Company_Contract = new HashSet<CR_Cas_Company_Contract>();
            this.CR_Cas_Sup_Branch_Documentation = new HashSet<CR_Cas_Sup_Branch_Documentation>();
            this.CR_Cas_Sup_Car_Doc_Mainten = new HashSet<CR_Cas_Sup_Car_Doc_Mainten>();
            this.CR_Cas_Sup_Follow_Up_Mechanism = new HashSet<CR_Cas_Sup_Follow_Up_Mechanism>();
        }
    
        public string CR_Mas_Sup_Procedures_Code { get; set; }
        public string CR_Mas_Sup_Procedures_Type { get; set; }
        public string CR_Mas_Sup_Procedures_Ar_Name { get; set; }
        public string CR_Mas_Sup_Procedures_En_Name { get; set; }
        public string CR_Mas_Sup_Procedures_Fr_Name { get; set; }
        public Nullable<decimal> CR_Mas_Sup_Procedures_BnanFollowUpContracts_Befor_Credit_Limit { get; set; }
        public Nullable<int> CR_Mas_Sup_Procedures_BnanFollowUpContracts_Befor_Expire { get; set; }
        public Nullable<int> CR_Mas_Sup_Procedures_BnanFollowUpContracts_Befor_KM { get; set; }
        public Nullable<int> CR_Mas_Sup_Procedures_BnanFollowUpContracts_Befor_Expire_KM { get; set; }
        public Nullable<int> CR_Mas_Sup_Procedures_BnanFollowUpContracts_Default_KM { get; set; }
        public string CR_Mas_Sup_Procedures_Status { get; set; }
        public string CR_Mas_Sup_Procedures_Reasons { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Cas_Company_Contract> CR_Cas_Company_Contract { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Cas_Sup_Branch_Documentation> CR_Cas_Sup_Branch_Documentation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Cas_Sup_Car_Doc_Mainten> CR_Cas_Sup_Car_Doc_Mainten { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Cas_Sup_Follow_Up_Mechanism> CR_Cas_Sup_Follow_Up_Mechanism { get; set; }
    }
}
