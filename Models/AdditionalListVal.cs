using System;

namespace RentCar.Models
{
    public class AdditionalListVal
    {
        public string CR_Mas_Sup_Additional_Code { get; set; }
        public string Name { get; set; }

        public string CR_Cas_Car_Price_Additional_No { get; set; }
        public string CR_Cas_Car_Price_Additional_Code { get; set; }
        public Nullable<decimal> CR_Cas_Car_Price_Additional_Value { get; set; }

        public virtual CR_Mas_Sup_Additional CR_Mas_Sup_Additional { get; set; }
    }
}