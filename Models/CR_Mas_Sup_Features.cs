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
    
    public partial class CR_Mas_Sup_Features
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CR_Mas_Sup_Features()
        {
            this.CR_Cas_Car_Price_Features = new HashSet<CR_Cas_Car_Price_Features>();
            this.CR_Cas_Sup_Features = new HashSet<CR_Cas_Sup_Features>();
        }
    
        public string CR_Mas_Sup_Features_Code { get; set; }
        public string CR_Mas_Sup_Features_Ar_Name { get; set; }
        public string CR_Mas_Sup_Features_En_Name { get; set; }
        public string CR_Mas_Sup_Features_Fr_Name { get; set; }
        public string CR_Mas_Sup_Features_Status { get; set; }
        public string CR_Mas_Sup_Features_Reasons { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Cas_Car_Price_Features> CR_Cas_Car_Price_Features { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Cas_Sup_Features> CR_Cas_Sup_Features { get; set; }
    }
}
