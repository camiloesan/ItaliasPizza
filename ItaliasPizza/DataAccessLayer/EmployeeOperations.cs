using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizza.DataAccessLayer
{
    public class EmployeeOperations
    {
        public static bool IsPhoneRegistered(string phone)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.Employee.Any(e => e.Phone == phone);
            }
        }

        public static bool IsEmailRegistered(string email)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.AccessAccount.Any(a => a.Email == email);
            }
        }

        public static int SaveEmployee(Employee employee, AccessAccount accessAccount)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Employee.Add(employee);
                db.AccessAccount.Add(accessAccount);
                return db.SaveChanges();
            }
        }
    }
}
