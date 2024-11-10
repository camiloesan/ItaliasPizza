using Database;
using ItaliasPizza.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ItaliasPizza.DataAccessLayer
{
    public class CashReconciliationOperations
    {
        public static CashClosingDetails GetCashClosingDetails()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                var today = DateTime.Today;

                var previousDayCash = db.CashReconciliation
                    .OrderByDescending(cr => cr.ClosingDate)
                    .Select(cr => cr.FinishingAmount)
                    .FirstOrDefault();

                var totalSalesCash = db.Transaction
                    .Where(t => DbFunctions.TruncateTime(t.Date) == today && t.IdTransactionType == 1)
                    .Sum(t => (decimal?)t.Amount) ?? 0;

                var totalSpentCash = db.Transaction
                    .Where(t => DbFunctions.TruncateTime(t.Date) == today && t.IdTransactionType == 2)
                    .Sum(t => (decimal?)t.Amount) ?? 0;

                return new CashClosingDetails
                {
                    PreviousDayCash = previousDayCash,
                    TotalSalesCash = totalSalesCash,
                    TotalSpentCash = totalSpentCash
                };
            }
        }

    }
}
