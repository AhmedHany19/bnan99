using System;

namespace RentCar.Models.BranchSys
{
    public class ElmInfoModel
    {
        public string CR_Elm_ID { get; set; }
        public string CR_Elm_Ar_Name { get; set; }
        public string CR_Elm_En_Name { get; set; }
        public string CR_Elm_Sector { get; set; }
        public Nullable<System.DateTime> CR_Elm_BirthDate { get; set; }
        public Nullable<System.DateTime> CR_Elm_Issue_ID_Date { get; set; }
        public Nullable<System.DateTime> CR_Elm_Expiry_ID_Date { get; set; }
        public Nullable<System.DateTime> CR_Elm_Expiry_Driver_Date { get; set; }
        public string CR_Elm_Ar_Nationality { get; set; }
        public string CR_Elm_En_Nationality { get; set; }
        public string CR_Elm_Ar_Gender { get; set; }
        public string CR_Elm_En_Gender { get; set; }
        public string CR_Elm_Ar_Jobs { get; set; }
        public string CR_Elm_En_Jobs { get; set; }
        public string CR_Elm_Ar_Workplace_Subscription { get; set; }
        public string CR_Elm_En_Workplace_Subscription { get; set; }
        public string CR_Elm_Mobile { get; set; }
        public string CR_Elm_Email { get; set; }
        public string CR_Elm_Ar_Social { get; set; }
        public string CR_Elm_En_Social { get; set; }
        public string CR_Elm_Ar_Educational_Qualification { get; set; }
        public string CR_Elm_En_Educational_Qualification { get; set; }

        public string OurSectorCode { get; set; }
        public string TracingNo { get; set; }
        public string Address { get; set; }
        public int Code { get; set; }
        public int Age { get; set; }
        public string OurNationalityCode { get; set; }
        public string OurGenderCode { get; set; }
        public string OurJobsCode { get; set; }
        public string OurSocialCode { get; set; }
        public string OurQualificationCode { get; set; }
        public string OurEmployerCode { get; set; }
        public string OurRegionCode { get; set; }
        public string OurCityCode { get; set; }
        public string IdErrorMessage { get; set; }
        public decimal PreviousBalance { get; set; }
        public bool PassRenter { get; set; }
        public string AddressErrorMessage { get; set; }
        public string EmployerErrorMessage { get; set; }

    }
}