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
    
    public partial class CR_Cas_Sup_Sub_Com_Account
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CR_Cas_Sup_Sub_Com_Account()
        {
            this.CR_Cas_Account_Restrictions = new HashSet<CR_Cas_Account_Restrictions>();
        }
    
        public string CR_Cas_Sup_Sub_Com_Account_Code { get; set; }
        public string CR_Cas_Sup_Sub_Com_Account_Sub_Code { get; set; }
        public string CR_Cas_Sup_Sub_Com_Account_Com_Code { get; set; }
        public Nullable<decimal> CR_Cas_Sup_Sub_Com_Account_Balance { get; set; }
        public string CR_Cas_Sup_Sub_Com_Account_Status { get; set; }
        public string CR_Cas_Sup_Sub_Com_Account_Reasons { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Cas_Account_Restrictions> CR_Cas_Account_Restrictions { get; set; }
        public virtual CR_Mas_Com_Lessor CR_Mas_Com_Lessor { get; set; }
        public virtual CR_Mas_Sup_Sub_Account CR_Mas_Sup_Sub_Account { get; set; }
    }
}
