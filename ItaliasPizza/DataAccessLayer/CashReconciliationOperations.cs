using Database;
using ItaliasPizza.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;

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
                    .OrderByDescending(cr => cr.OpeningDate)
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

        public static bool IsCashReconciliationRegisteredToday()
        {
            var today = DateTime.Today;

            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.CashReconciliation
                    .Any(cr => DbFunctions.TruncateTime(cr.OpeningDate) == today);
            }
        }

        public static int SaveCashReconciliation(CashReconciliation cashReconciliation)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                int result = 0;

                db.CashReconciliation.Add(cashReconciliation);

                result += db.SaveChanges();

                return result;
            }
        }

    }
}
