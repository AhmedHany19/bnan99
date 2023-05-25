using System;

namespace RentCar.Models
{
    public class AdditionalPrice
    {

        public string CR_Mas_Sup_Additional_Code { get; set; }
        public string CR_Mas_Sup_Additional_Group_Code { get; set; }
        public string CR_Mas_Sup_Additional_Ar_Name { get; set; }
        public string CR_Mas_Sup_Additional_En_Name { get; set; }
        public string CR_Mas_Sup_Additional_Fr_Name { get; set; }
        public string CR_Mas_Sup_Additional_Status { get; set; }
        public string CR_Mas_Sup_Additional_Reasons { get; set; }

        public virtual CR_Mas_Sup_Group CR_Mas_Sup_Group { get; set; }
        public Nullable<bool> check { get; set; }
    }
}