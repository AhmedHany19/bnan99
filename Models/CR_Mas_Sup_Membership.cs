//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RentCar.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CR_Mas_Sup_Membership
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CR_Mas_Sup_Membership()
        {
            this.CR_Cas_Sup_Membership_Conditions = new HashSet<CR_Cas_Sup_Membership_Conditions>();
        }
    
        public string CR_Mas_Sup_Membership_Code { get; set; }
        public string CR_Mas_Sup_Membership_Group_Code { get; set; }
        public string CR_Mas_Sup_Membership_Ar_Name { get; set; }
        public string CR_Mas_Sup_Membership_En_Name { get; set; }
        public string CR_Mas_Sup_Membership_Fr_Name { get; set; }
        public string CR_Mas_Sup_Membership_Status { get; set; }
        public string CR_Mas_Sup_Membership_Reasons { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Cas_Sup_Membership_Conditions> CR_Cas_Sup_Membership_Conditions { get; set; }
        public virtual CR_Mas_Sup_Group CR_Mas_Sup_Group { get; set; }
    }
}
