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
    
    public partial class CR_Mas_Sup_Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CR_Mas_Sup_Category()
        {
            this.CR_Cas_Car_Price_Basic = new HashSet<CR_Cas_Car_Price_Basic>();
            this.CR_Mas_Sup_Category_Car = new HashSet<CR_Mas_Sup_Category_Car>();
        }
    
        public string CR_Mas_Sup_Category_Code { get; set; }
        public string CR_Mas_Sup_Category_Group_Code { get; set; }
        public string CR_Mas_Sup_Category_Ar_Name { get; set; }
        public string CR_Mas_Sup_Category_En_Name { get; set; }
        public string CR_Mas_Sup_Category_Fr_Name { get; set; }
        public string CR_Mas_Sup_Category_Status { get; set; }
        public string CR_Mas_Sup_Category_Reasons { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Cas_Car_Price_Basic> CR_Cas_Car_Price_Basic { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Mas_Sup_Category_Car> CR_Mas_Sup_Category_Car { get; set; }
        public virtual CR_Mas_Sup_Group CR_Mas_Sup_Group { get; set; }
    }
}
