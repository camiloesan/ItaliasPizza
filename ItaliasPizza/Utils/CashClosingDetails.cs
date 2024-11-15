using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizza.Utils
{
    public class CashClosingDetails
    {
        public decimal PreviousDayCash { get; set; }
        public decimal TotalSalesCash { get; set; }
        public decimal TotalSpentCash { get; set; }
    }
}
