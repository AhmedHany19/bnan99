using System;
using System.Data.Entity;
using System.Linq;

namespace BnanFollowUpContracts
{
    public class Program
    {
        public static void Main(string[] args)
        {
            UpdateDatabase();
        }
        public static void UpdateDatabase()
        {
            RentCarDBEntities database = new RentCarDBEntities();
            var rentaldays = 2;

            foreach (var item in database.CR_Cas_Contract_Basic.Include(l => l.CR_Mas_Com_Lessor).Include(l => l.CR_Mas_Renter_Information).Include(l => l.CR_Cas_Sup_Car_Information).Where(d => d.CR_Cas_Contract_Basic_Status == "A").ToList())
            {
                // check if contract will end after 24 hours
                if (item.CR_Cas_Contract_Basic_Day_DateTime_Alert <= DateTime.Now && item.CR_Cas_Contract_Basic_Alert_Status == "0")
                {
                    if (item.CR_Cas_Contract_Basic_Expected_Rental_Days > rentaldays)
                    {
                        SendMailForPatch.SendMailBeforeOneDay(item);
                    }
                    item.CR_Cas_Contract_Basic_Alert_Status = "1";
                    database.SaveChanges();
                }

                // check if contract will end after 4 hours
                if ((item.CR_Cas_Contract_Basic_Hour_DateTime_Alert <= DateTime.Now && item.CR_Cas_Contract_Basic_Alert_Status == "1"))
                {
                    SendMailForPatch.SendMailBeforeFourHours(item);
                    item.CR_Cas_Contract_Basic_Alert_Status = "2";
                    database.SaveChanges();
                }

                var time = Convert.ToDouble(item.CR_Cas_Contract_Basic_Expected_End_Time?.Hours);
                var min = Convert.ToDouble(item.CR_Cas_Contract_Basic_Expected_End_Time?.Minutes);
                TimeSpan timeSpan = TimeSpan.FromHours(time);
                TimeSpan minSpan = TimeSpan.FromMinutes(min);
                var dateOnly = item.CR_Cas_Contract_Basic_Expected_End_Date; // Example date-only DateTime
                if (timeSpan != null && minSpan != null && dateOnly !=null)
                {
                    DateTime result = (DateTime)(dateOnly?.Add(timeSpan).Add(minSpan)); // Concatenate time span and date-only DateTime
                     // check if contract will end Now
                    if (result <= DateTime.Now && item.CR_Cas_Contract_Basic_Alert_Status == "2")
                    {
                        SendMailForPatch.SendMailWhenEnd(item);
                        item.CR_Cas_Contract_Basic_Alert_Status = "3";
                        item.CR_Cas_Contract_Basic_Status = "E";
                        database.SaveChanges();
                    }
                }
            }
        }
    }
}
