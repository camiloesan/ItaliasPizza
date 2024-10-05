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

        public static Employee GetEmployeeById(Guid idEmployee)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.Employee.Find(idEmployee);
            }
        }

        public static string GetEmployeeEmail(Guid idEmployee)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                return db.AccessAccount.Find(idEmployee).Email;
            }
        }

        public static int UpdateEmployee(Employee employee)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                Employee employeeToUpdate = db.Employee.Find(employee.IdEmployee);
                employeeToUpdate.FirstName = employee.FirstName;
                employeeToUpdate.LastName = employee.LastName;
                employeeToUpdate.Phone = employee.Phone;
                employeeToUpdate.Status = employee.Status;
                employeeToUpdate.IdCharge = employee.IdCharge;
                return db.SaveChanges();
            }
        }

        public static int UpdateEmployeePasswordAndEmail(Guid idEmployee, string password, string email)
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                AccessAccount accessAccount = db.AccessAccount.Find(idEmployee);
                accessAccount.Password = password;
                accessAccount.Email = email;
                return db.SaveChanges();
            }
        }
    }
}
