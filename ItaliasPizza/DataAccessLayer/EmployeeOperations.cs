using Database;
using ItaliasPizza.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;

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
                return db.AccessAccount.Where(a => a.IdEmployee == idEmployee).Select(a => a.Email).FirstOrDefault();
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
                AccessAccount accessAccount = db.AccessAccount.Where(a => a.IdEmployee == idEmployee).FirstOrDefault();
                accessAccount.Password = password;
                accessAccount.Email = email;
                return db.SaveChanges();
            }
        }

        public static List<EmployeeDetails> GetEmployeeInfo()
        {
            using (var db = new ItaliasPizzaDBEntities())
            {
                var employeeDetailsList = (from e in db.Employee
                                           join a in db.AccessAccount
                                           on e.IdEmployee equals a.IdEmployee
                                           join c in db.Charge
                                           on e.IdCharge equals c.IdCharge
                                           select new EmployeeDetails
                                           {
                                               IdEmployee = e.IdEmployee,
                                               FirstName = e.FirstName,
                                               LastName = e.LastName,
                                               Email = a.Email, // Associated email
                                               Phone = e.Phone,
                                               Charge = c.Name // Charge name from the Charge table
                                           }).ToList();

                return employeeDetailsList;
            }
        }
    }
}
