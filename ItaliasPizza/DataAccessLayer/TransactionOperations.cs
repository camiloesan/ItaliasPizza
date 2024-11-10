using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizza.DataAccessLayer
{
    public class TransactionOperations
    {
        public static int SaveTransaction(Transaction transaction)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Transaction.Add(transaction);

                return db.SaveChanges();
            }
        }
    }
}
