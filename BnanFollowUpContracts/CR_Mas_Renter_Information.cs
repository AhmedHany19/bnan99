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
    
    public partial class CR_Mas_Renter_Information
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CR_Mas_Renter_Information()
        {
            this.CR_Cas_Contract_Basic = new HashSet<CR_Cas_Contract_Basic>();
            this.CR_Cas_Renter_Lessor = new HashSet<CR_Cas_Renter_Lessor>();
        }
    
        public string CR_Mas_Renter_Information_Id { get; set; }
        public Nullable<int> CR_Mas_Renter_Information_CopyID { get; set; }
        public string CR_Mas_Renter_Information_Sector { get; set; }
        public string CR_Mas_Renter_Information_Ar_Name { get; set; }
        public string CR_Mas_Renter_Information_En_Name { get; set; }
        public Nullable<System.DateTime> CR_Mas_Renter_Information_BirthDate { get; set; }
        public Nullable<System.DateTime> CR_Mas_Renter_Information_Issue_Id_Date { get; set; }
        public Nullable<System.DateTime> CR_Mas_Renter_Information_Expiry_Id_Date { get; set; }
        public Nullable<System.DateTime> CR_Mas_Renter_Information_Expiry_Driving_License_Date { get; set; }
        public string CR_Mas_Renter_Information_Workplace_Subscription { get; set; }
        public string CR_Mas_Renter_Information_Nationality { get; set; }
        public string CR_Mas_Renter_Information_Gender { get; set; }
        public string CR_Mas_Renter_Information_Jobs { get; set; }
        public string CR_Mas_Renter_Information_Educational_Qualification { get; set; }
        public string CR_Mas_Renter_Information_Social { get; set; }
        public string CR_Mas_Renter_Information_Membership { get; set; }
        public string CR_Mas_Renter_Information_Mobile { get; set; }
        public string CR_Mas_Renter_Information_Email { get; set; }
        public Nullable<long> CR_Mas_Renter_Information_Passport_No { get; set; }
        public Nullable<System.DateTime> CR_Mas_Renter_Information_Pasport_Date { get; set; }
        public Nullable<System.DateTime> CR_Mas_Renter_Information_Pasport_Expiry_Date { get; set; }
        public string CR_Mas_Renter_Information_Iban { get; set; }
        public string CR_Mas_Renter_Information_Bank { get; set; }
        public Nullable<System.DateTime> CR_Mas_Renter_Information_UpDate_Personal { get; set; }
        public Nullable<System.DateTime> CR_Mas_Renter_Information_UpDate_Post { get; set; }
        public Nullable<System.DateTime> CR_Mas_Renter_Information_UpDate_Workplace { get; set; }
        public Nullable<System.DateTime> CR_Mas_Renter_Information_UpDate_License { get; set; }
        public Nullable<System.DateTime> CR_Mas_Renter_Information_Date_First_Interaction { get; set; }
        public Nullable<System.DateTime> CR_Mas_Renter_Information_Date_Last_Interaction { get; set; }
        public Nullable<int> CR_Mas_Renter_Information_Visits_Number { get; set; }
        public Nullable<int> CR_Mas_Renter_Information_Requests_Number { get; set; }
        public Nullable<int> CR_Mas_Renter_Information_Contract_Number { get; set; }
        public Nullable<decimal> CR_Mas_Renter_Information_Value { get; set; }
        public Nullable<int> CR_Mas_Renter_Information_Days { get; set; }
        public Nullable<int> CR_Mas_Renter_Information_Traveled_Distance { get; set; }
        public Nullable<int> CR_Mas_Renter_Information_Reservations_Number { get; set; }
        public Nullable<int> CR_Mas_Renter_Information_Reservations_Executed_Number { get; set; }
        public Nullable<int> CR_Mas_Renter_Information_Accidents { get; set; }
        public Nullable<int> CR_Mas_Renter_Information_Violations { get; set; }
        public Nullable<int> CR_Mas_Renter_Information_Evaluation_Count { get; set; }
        public Nullable<int> CR_Mas_Renter_Information_Evaluation_Value { get; set; }
        public string CR_Mas_Renter_Information_Tax_No { get; set; }
        public string CR_Mas_Renter_Information_Signature { get; set; }
        public string CR_Mas_Renter_Information_Renter_Id_Image { get; set; }
        public string CR_Mas_Renter_Information_Renter_License_Image { get; set; }
        public string CR_Mas_Renter_Information_Renter_Image { get; set; }
        public string CR_Mas_Renter_Information_Status { get; set; }
        public string CR_Mas_Renter_Information_Reasons { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Cas_Contract_Basic> CR_Cas_Contract_Basic { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Cas_Renter_Lessor> CR_Cas_Renter_Lessor { get; set; }
        public virtual CR_Mas_Sup_Educational_Qualification CR_Mas_Sup_Educational_Qualification { get; set; }
        public virtual CR_Mas_Sup_Employer CR_Mas_Sup_Employer { get; set; }
        public virtual CR_Mas_Sup_Gender CR_Mas_Sup_Gender { get; set; }
        public virtual CR_Mas_Sup_Jobs CR_Mas_Sup_Jobs { get; set; }
        public virtual CR_Mas_Sup_Nationalities CR_Mas_Sup_Nationalities { get; set; }
        public virtual CR_Mas_Sup_Sector CR_Mas_Sup_Sector { get; set; }
        public virtual CR_Mas_Sup_Social CR_Mas_Sup_Social { get; set; }
    }
}
