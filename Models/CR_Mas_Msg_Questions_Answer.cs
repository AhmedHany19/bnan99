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
    
    public partial class CR_Mas_Msg_Questions_Answer
    {
        public string CR_Mas_Msg_Questions_Answer_No { get; set; }
        public string CR_Mas_Msg_Tasks_Code { get; set; }
        public string CR_Mas_Msg_Ar_Questions { get; set; }
        public string CR_Mas_Msg_Ar_Answer { get; set; }
        public string CR_Mas_Msg_En_Questions { get; set; }
        public string CR_Mas_Msg_En_Answer { get; set; }
        public string CR_Mas_Msg_Fr_Questions { get; set; }
        public string CR_Mas_Msg_Fr_Answer { get; set; }
        public string CR_Mas_Msg_Questions_Answer_Status { get; set; }
        public string CR_Mas_Msg_Questions_Answer_Reasons { get; set; }
    
        public virtual CR_Mas_Sys_Tasks CR_Mas_Sys_Tasks { get; set; }
    }
}
