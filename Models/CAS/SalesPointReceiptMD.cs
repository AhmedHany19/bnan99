using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentCar.Models.CAS
{
    public class SalesPointReceiptMD
    {
        public string CR_Cas_Sup_SalesPoint_Code { get; set; }
        public string CR_Cas_Sup_SalesPoint_Com_Code { get; set; }
        public string CR_Cas_Sup_SalesPoint_Brn_Code { get; set; }
        public string CR_Cas_Sup_SalesPoint_Bank_Code { get; set; }
        public string CR_Cas_Sup_SalesPoint_Bank_No { get; set; }
        public string CR_Cas_Sup_SalesPoint_Ar_Name { get; set; }
        public string CR_Cas_Sup_SalesPoint_En_Name { get; set; }
        public Nullable<decimal> CR_Cas_Sup_SalesPoint_Balance { get; set; }
        public string CR_Cas_Sup_SalesPoint_Status { get; set; }
        public string CR_Cas_Sup_SalesPoint_Reasons { get; set; }

        public int ReceiptDebitCount { get; set; }
        public int ReceiptCreditCount { get; set; }

        public virtual CR_Cas_Sup_Bank CR_Cas_Sup_Bank { get; set; }
        public virtual CR_Cas_Sup_Branch CR_Cas_Sup_Branch { get; set; }
        public virtual CR_Mas_Com_Lessor CR_Mas_Com_Lessor { get; set; }
    }
}