using Database;
using System;
using System.Linq;

namespace ItaliasPizza.DataAccessLayer
{
    public class AccessAccountOperations
    {
        public static bool AreCredentialsValid(string email, string password)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.AccessAccount.Any(a => a.Email == email && a.Password == password);
            }
        }

        public static AccessAccount GetAccessAccountByEmail(string email)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.AccessAccount.FirstOrDefault(a => a.Email == email);
            }
        }

        public static string GetEmployeeCharge(string email)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                var accessAccount = db.AccessAccount.FirstOrDefault(a => a.Email == email);
                return accessAccount.Employee.Charge.Name;
            }
        }

        public static System.Guid GetEmployeeId(string email)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                var accessAccount = db.AccessAccount.FirstOrDefault(a => a.Email == email);
                return accessAccount.IdEmployee;
            }
        }
    }
}
