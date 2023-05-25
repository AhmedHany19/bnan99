using System;

namespace RentCar.Models
{
    public class ChoicesListVal
    {


        public string CR_Mas_Sup_Choices_Code { get; set; }

        public string CR_Mas_Sup_Choices_Group_Code { get; set; }

        public string CR_Mas_Sup_Choices_Ar_Name { get; set; }

        public string CR_Mas_Sup_Choices_En_Name { get; set; }

        public string CR_Mas_Sup_Choices_Fr_Name { get; set; }

        public string CR_Mas_Sup_Choices_Status { get; set; }

        public string CR_Mas_Sup_Choices_Reasons { get; set; }

        public Nullable<decimal> CR_Cas_Car_Price_Choices_Value { get; set; }

        public virtual CR_Mas_Sup_Group CR_Mas_Sup_Group { get; set; }
    }
}