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
    
    public partial class CR_Mas_Sup_Group
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CR_Mas_Sup_Group()
        {
            this.CR_Mas_Sup_Additional = new HashSet<CR_Mas_Sup_Additional>();
            this.CR_Mas_Sup_Age = new HashSet<CR_Mas_Sup_Age>();
            this.CR_Mas_Sup_Category = new HashSet<CR_Mas_Sup_Category>();
            this.CR_Mas_Sup_Choices = new HashSet<CR_Mas_Sup_Choices>();
            this.CR_Mas_Sup_City = new HashSet<CR_Mas_Sup_City>();
            this.CR_Mas_Sup_Color = new HashSet<CR_Mas_Sup_Color>();
            this.CR_Mas_Sup_Educational_Qualification = new HashSet<CR_Mas_Sup_Educational_Qualification>();
            this.CR_Mas_Sup_Employer = new HashSet<CR_Mas_Sup_Employer>();
            this.CR_Mas_Sup_Gender = new HashSet<CR_Mas_Sup_Gender>();
            this.CR_Mas_Sup_Jobs = new HashSet<CR_Mas_Sup_Jobs>();
            this.CR_Mas_Sup_Made_Year = new HashSet<CR_Mas_Sup_Made_Year>();
            this.CR_Mas_Sup_Membership = new HashSet<CR_Mas_Sup_Membership>();
            this.CR_Mas_Sup_Model = new HashSet<CR_Mas_Sup_Model>();
            this.CR_Mas_Sup_Nationalities = new HashSet<CR_Mas_Sup_Nationalities>();
            this.CR_Mas_Sup_Social = new HashSet<CR_Mas_Sup_Social>();
        }
    
        public string CR_Mas_Sup_Group_Code { get; set; }
        public string CR_Mas_Sup_Group_Ar_Name { get; set; }
        public string CR_Mas_Sup_Group_En_Name { get; set; }
        public string CR_Mas_Sup_Group_Fr_Name { get; set; }
        public Nullable<bool> CR_Mas_Sup_Group_Classified_Data { get; set; }
        public Nullable<bool> CR_Mas_Sup_Group_Independent_Data { get; set; }
        public string CR_Mas_Sup_Group_Status { get; set; }
        public string CR_Mas_Sup_Group_Reasons { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Mas_Sup_Additional> CR_Mas_Sup_Additional { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Mas_Sup_Age> CR_Mas_Sup_Age { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Mas_Sup_Category> CR_Mas_Sup_Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Mas_Sup_Choices> CR_Mas_Sup_Choices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Mas_Sup_City> CR_Mas_Sup_City { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Mas_Sup_Color> CR_Mas_Sup_Color { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Mas_Sup_Educational_Qualification> CR_Mas_Sup_Educational_Qualification { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Mas_Sup_Employer> CR_Mas_Sup_Employer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Mas_Sup_Gender> CR_Mas_Sup_Gender { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Mas_Sup_Jobs> CR_Mas_Sup_Jobs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Mas_Sup_Made_Year> CR_Mas_Sup_Made_Year { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Mas_Sup_Membership> CR_Mas_Sup_Membership { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Mas_Sup_Model> CR_Mas_Sup_Model { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Mas_Sup_Nationalities> CR_Mas_Sup_Nationalities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Mas_Sup_Social> CR_Mas_Sup_Social { get; set; }
    }
}
