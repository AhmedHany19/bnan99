﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class RentCarDBEntities : DbContext
    {
        public RentCarDBEntities()
            : base("name=RentCarDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CR_Cas_Account_Bnan_Owed> CR_Cas_Account_Bnan_Owed { get; set; }
        public virtual DbSet<CR_Cas_Account_Pay_Owed> CR_Cas_Account_Pay_Owed { get; set; }
        public virtual DbSet<CR_Cas_Account_Receipt> CR_Cas_Account_Receipt { get; set; }
        public virtual DbSet<CR_Cas_Account_Restrictions> CR_Cas_Account_Restrictions { get; set; }
        public virtual DbSet<CR_Cas_Account_TAMM_Owed> CR_Cas_Account_TAMM_Owed { get; set; }
        public virtual DbSet<CR_Cas_Account_Tax_Owed> CR_Cas_Account_Tax_Owed { get; set; }
        public virtual DbSet<CR_Cas_Account_Tga_Owed> CR_Cas_Account_Tga_Owed { get; set; }
        public virtual DbSet<CR_Cas_Account_Transfers_Tenants> CR_Cas_Account_Transfers_Tenants { get; set; }
        public virtual DbSet<CR_Cas_Administrative_Procedures> CR_Cas_Administrative_Procedures { get; set; }
        public virtual DbSet<CR_Cas_Car_Price_Additional> CR_Cas_Car_Price_Additional { get; set; }
        public virtual DbSet<CR_Cas_Car_Price_Basic> CR_Cas_Car_Price_Basic { get; set; }
        public virtual DbSet<CR_Cas_Car_Price_Choices> CR_Cas_Car_Price_Choices { get; set; }
        public virtual DbSet<CR_Cas_Car_Price_Features> CR_Cas_Car_Price_Features { get; set; }
        public virtual DbSet<CR_Cas_Company_Contract> CR_Cas_Company_Contract { get; set; }
        public virtual DbSet<CR_Cas_Contract_Additional> CR_Cas_Contract_Additional { get; set; }
        public virtual DbSet<CR_Cas_Contract_Basic> CR_Cas_Contract_Basic { get; set; }
        public virtual DbSet<CR_Cas_Contract_Choices> CR_Cas_Contract_Choices { get; set; }
        public virtual DbSet<CR_Cas_Contract_Virtual_Inspection> CR_Cas_Contract_Virtual_Inspection { get; set; }
        public virtual DbSet<CR_Cas_Renter_Lessor> CR_Cas_Renter_Lessor { get; set; }
        public virtual DbSet<CR_Cas_Sup_Bank> CR_Cas_Sup_Bank { get; set; }
        public virtual DbSet<CR_Cas_Sup_Beneficiary> CR_Cas_Sup_Beneficiary { get; set; }
        public virtual DbSet<CR_Cas_Sup_Branch> CR_Cas_Sup_Branch { get; set; }
        public virtual DbSet<CR_Cas_Sup_Branch_Documentation> CR_Cas_Sup_Branch_Documentation { get; set; }
        public virtual DbSet<CR_Cas_Sup_Car_Doc_Mainten> CR_Cas_Sup_Car_Doc_Mainten { get; set; }
        public virtual DbSet<CR_Cas_Sup_Car_Information> CR_Cas_Sup_Car_Information { get; set; }
        public virtual DbSet<CR_Cas_Sup_Features> CR_Cas_Sup_Features { get; set; }
        public virtual DbSet<CR_Cas_Sup_Follow_Up_Mechanism> CR_Cas_Sup_Follow_Up_Mechanism { get; set; }
        public virtual DbSet<CR_Cas_Sup_Main_Brn_Account> CR_Cas_Sup_Main_Brn_Account { get; set; }
        public virtual DbSet<CR_Cas_Sup_Main_Com_Account> CR_Cas_Sup_Main_Com_Account { get; set; }
        public virtual DbSet<CR_Cas_Sup_Membership_Conditions> CR_Cas_Sup_Membership_Conditions { get; set; }
        public virtual DbSet<CR_Cas_Sup_Owners> CR_Cas_Sup_Owners { get; set; }
        public virtual DbSet<CR_Cas_Sup_SalesPoint> CR_Cas_Sup_SalesPoint { get; set; }
        public virtual DbSet<CR_Cas_Sup_Sub_Brn_Account> CR_Cas_Sup_Sub_Brn_Account { get; set; }
        public virtual DbSet<CR_Cas_Sup_Sub_Com_Account> CR_Cas_Sup_Sub_Com_Account { get; set; }
        public virtual DbSet<CR_Cas_User_Branch_Validity> CR_Cas_User_Branch_Validity { get; set; }
        public virtual DbSet<CR_Cas_User_Information> CR_Cas_User_Information { get; set; }
        public virtual DbSet<CR_Cas_User_Validity_Contract> CR_Cas_User_Validity_Contract { get; set; }
        public virtual DbSet<CR_ELM_Address> CR_ELM_Address { get; set; }
        public virtual DbSet<CR_Elm_Personal> CR_Elm_Personal { get; set; }
        public virtual DbSet<CR_Mas_Account_Data_Owed> CR_Mas_Account_Data_Owed { get; set; }
        public virtual DbSet<CR_Mas_Account_License_Owed> CR_Mas_Account_License_Owed { get; set; }
        public virtual DbSet<CR_Mas_Account_Msg_Owed> CR_Mas_Account_Msg_Owed { get; set; }
        public virtual DbSet<CR_Mas_Account_Post_Owed> CR_Mas_Account_Post_Owed { get; set; }
        public virtual DbSet<CR_Mas_Address> CR_Mas_Address { get; set; }
        public virtual DbSet<CR_Mas_Basic_Contract> CR_Mas_Basic_Contract { get; set; }
        public virtual DbSet<CR_Mas_Com_Lessor> CR_Mas_Com_Lessor { get; set; }
        public virtual DbSet<CR_Mas_Com_Supporting> CR_Mas_Com_Supporting { get; set; }
        public virtual DbSet<CR_Mas_Msg_Questions_Answer> CR_Mas_Msg_Questions_Answer { get; set; }
        public virtual DbSet<CR_MAS_Renter_Addre> CR_MAS_Renter_Addre { get; set; }
        public virtual DbSet<CR_Mas_Renter_Information> CR_Mas_Renter_Information { get; set; }
        public virtual DbSet<CR_Mas_Service_Bnan_Contract> CR_Mas_Service_Bnan_Contract { get; set; }
        public virtual DbSet<CR_Mas_Service_Tamm_Contract> CR_Mas_Service_Tamm_Contract { get; set; }
        public virtual DbSet<CR_Mas_Sup_Additional> CR_Mas_Sup_Additional { get; set; }
        public virtual DbSet<CR_Mas_Sup_Age> CR_Mas_Sup_Age { get; set; }
        public virtual DbSet<CR_Mas_Sup_Bank> CR_Mas_Sup_Bank { get; set; }
        public virtual DbSet<CR_Mas_Sup_Brand> CR_Mas_Sup_Brand { get; set; }
        public virtual DbSet<CR_Mas_Sup_Category> CR_Mas_Sup_Category { get; set; }
        public virtual DbSet<CR_Mas_Sup_Category_Car> CR_Mas_Sup_Category_Car { get; set; }
        public virtual DbSet<CR_Mas_Sup_Choices> CR_Mas_Sup_Choices { get; set; }
        public virtual DbSet<CR_Mas_Sup_City> CR_Mas_Sup_City { get; set; }
        public virtual DbSet<CR_Mas_Sup_Color> CR_Mas_Sup_Color { get; set; }
        public virtual DbSet<CR_Mas_Sup_Country> CR_Mas_Sup_Country { get; set; }
        public virtual DbSet<CR_Mas_Sup_Educational_Qualification> CR_Mas_Sup_Educational_Qualification { get; set; }
        public virtual DbSet<CR_Mas_Sup_Employer> CR_Mas_Sup_Employer { get; set; }
        public virtual DbSet<CR_Mas_Sup_Features> CR_Mas_Sup_Features { get; set; }
        public virtual DbSet<CR_Mas_Sup_Gender> CR_Mas_Sup_Gender { get; set; }
        public virtual DbSet<CR_Mas_Sup_Group> CR_Mas_Sup_Group { get; set; }
        public virtual DbSet<CR_Mas_Sup_Jobs> CR_Mas_Sup_Jobs { get; set; }
        public virtual DbSet<CR_Mas_Sup_Made_Year> CR_Mas_Sup_Made_Year { get; set; }
        public virtual DbSet<CR_Mas_Sup_Main_Account> CR_Mas_Sup_Main_Account { get; set; }
        public virtual DbSet<CR_Mas_Sup_Membership> CR_Mas_Sup_Membership { get; set; }
        public virtual DbSet<CR_Mas_Sup_Membership_Group> CR_Mas_Sup_Membership_Group { get; set; }
        public virtual DbSet<CR_Mas_Sup_Model> CR_Mas_Sup_Model { get; set; }
        public virtual DbSet<CR_Mas_Sup_Nationalities> CR_Mas_Sup_Nationalities { get; set; }
        public virtual DbSet<CR_Mas_Sup_Payment_Method> CR_Mas_Sup_Payment_Method { get; set; }
        public virtual DbSet<CR_Mas_Sup_Procedures> CR_Mas_Sup_Procedures { get; set; }
        public virtual DbSet<CR_Mas_Sup_Regions> CR_Mas_Sup_Regions { get; set; }
        public virtual DbSet<CR_Mas_Sup_Registration_Car> CR_Mas_Sup_Registration_Car { get; set; }
        public virtual DbSet<CR_Mas_Sup_Sector> CR_Mas_Sup_Sector { get; set; }
        public virtual DbSet<CR_Mas_Sup_Service_Fee_Bnan> CR_Mas_Sup_Service_Fee_Bnan { get; set; }
        public virtual DbSet<CR_Mas_Sup_Service_Fee_Data> CR_Mas_Sup_Service_Fee_Data { get; set; }
        public virtual DbSet<CR_Mas_Sup_Service_Fee_Tamm> CR_Mas_Sup_Service_Fee_Tamm { get; set; }
        public virtual DbSet<CR_Mas_Sup_Service_Fee_Tga> CR_Mas_Sup_Service_Fee_Tga { get; set; }
        public virtual DbSet<CR_Mas_Sup_Social> CR_Mas_Sup_Social { get; set; }
        public virtual DbSet<CR_Mas_Sup_Specifications> CR_Mas_Sup_Specifications { get; set; }
        public virtual DbSet<CR_Mas_Sup_Sub_Account> CR_Mas_Sup_Sub_Account { get; set; }
        public virtual DbSet<CR_Mas_Sup_Virtual_Inspection> CR_Mas_Sup_Virtual_Inspection { get; set; }
        public virtual DbSet<CR_Mas_Sys_System_Name> CR_Mas_Sys_System_Name { get; set; }
        public virtual DbSet<CR_Mas_Sys_Tasks> CR_Mas_Sys_Tasks { get; set; }
        public virtual DbSet<CR_Mas_User_Information> CR_Mas_User_Information { get; set; }
        public virtual DbSet<CR_Mas_User_Main_Validation> CR_Mas_User_Main_Validation { get; set; }
        public virtual DbSet<CR_Mas_User_Sub_Validation> CR_Mas_User_Sub_Validation { get; set; }
    
        public virtual int CheckAndUpdateTable()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CheckAndUpdateTable");
        }
    }
}
