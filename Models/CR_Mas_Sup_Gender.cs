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
    
    public partial class CR_Mas_Sup_Gender
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CR_Mas_Sup_Gender()
        {
            this.CR_Mas_Renter_Information = new HashSet<CR_Mas_Renter_Information>();
        }
    
        public string CR_Mas_Sup_Gender_Code { get; set; }
        public string CR_Mas_Sup_Gender_Group_Code { get; set; }
        public string CR_Mas_Sup_Gender_Ar_Name { get; set; }
        public string CR_Mas_Sup_Gender_En_Name { get; set; }
        public string CR_Mas_Sup_Gender_Fr_Name { get; set; }
        public string CR_Mas_Sup_Gender_Status { get; set; }
        public string CR_Mas_Sup_Gender_Reasons { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Mas_Renter_Information> CR_Mas_Renter_Information { get; set; }
        public virtual CR_Mas_Sup_Group CR_Mas_Sup_Group { get; set; }
    }
}
