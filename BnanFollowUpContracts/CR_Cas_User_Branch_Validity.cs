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
    
    public partial class CR_Cas_User_Branch_Validity
    {
        public string CR_Cas_User_Branch_Validity_Id { get; set; }
        public string CR_Cas_User_Branch_Validity_Branch { get; set; }
        public string CR_Mas_User_Branch_Validity_Branch_Status { get; set; }
        public string CR_Cas_User_Branch_Validity_Lessor { get; set; }
    
        public virtual CR_Cas_User_Information CR_Cas_User_Information { get; set; }
    }
}
