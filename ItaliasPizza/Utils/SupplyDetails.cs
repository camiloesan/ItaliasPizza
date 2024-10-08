using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizza.Utils
{
    public sealed class SupplyDetails
    {
        private SupplyDetails()
        {

        }

        public static Guid IdSupply { get; set; }
        public static string Name { get; set; }
        public static string Amount { get; set; }
        public static string ExpirationDate { get; set; }
        
        public static void CleanSupplyDetails()
        {
            IdSupply = Guid.Empty;
            Name = "";
            Amount = "";
            ExpirationDate = "";
        }
    }
}
