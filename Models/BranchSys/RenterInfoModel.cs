using System;
using System.Collections.Generic;

namespace RentCar.Models.BranchSys
{
    public class RenterInfoModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]

        public string CR_Mas_Renter_Information_Id { get; set; }
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
        public string CR_Mas_Renter_Information_Signature { get; set; }
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
        public string CR_Mas_Renter_Information_Status { get; set; }
        public string CR_Mas_Renter_Information_Reasons { get; set; }

        public string TracingNo { get; set; }
        public string address { get; set; }
        public int RenterMessageCode { get; set; }

        public string DrivingLicenceErrorMessage { get; set; }
        public string IdErrorMessage { get; set; }
        public string AddressErrorMessage { get; set; }
        public string EmployerErrorMessage { get; set; }

        public string Nationality { get; set; }
        public string Gender { get; set; }
        public string Job { get; set; }         
        public string Employer { get; set; }

        public int Age { get; set; }
        public decimal PreviousBalance { get; set; }
        public bool PassRenter { get; set; }

        public bool HaveContract { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Cas_Renter_Lessor> CR_Cas_Renter_Lessor { get; set; }
    }
}