using System;

namespace RentCar.Models
{
    public class FeaturesListVal
    {
        public string CR_Cas_Sup_Features_Serial_No { get; set; }
        public string CR_Cas_Sup_Features_Code { get; set; }
        public string CR_Cas_Sup_Features_Model_Code { get; set; }
        public string CR_Cas_Sup_Features_Model_Year { get; set; }
        public string CR_Cas_Sup_Features_Lessor_Code { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> CR_Cas_Car_Price_Features_Value { get; set; }

        public virtual CR_Mas_Sup_Features CR_Mas_Sup_Features { get; set; }
    }
}