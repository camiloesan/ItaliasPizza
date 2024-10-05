using Microsoft.VisualStudio.TestTools.UnitTesting;
using ItaliasPizza.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;
using ItaliasPizza.Pages;

namespace ItaliasPizza.DataAccessLayer.Tests
{
    [TestClass()]
    public class EmployeeOperationsTests
    {
        [TestMethod()]
        public void SaveEmployeeTest()
        {
            var idEmployee = Guid.NewGuid();
            var employee = new Employee
            {
                IdEmployee = idEmployee,
                FirstName = "Test",
                LastName = "Test",
                Phone = "1234567890",
                Status = true,
                IdCharge = 1
            };
            AccessAccount accessAccount = new AccessAccount
            {
                UserName = "robertoll22",
                Password = "shenzhen",
                IdEmployee = idEmployee,
                Email = "roberto@gmail.com",
                Status = true,
            };

            var expected = 2;
            var actual = EmployeeOperations.SaveEmployee(employee, accessAccount);

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Employee.Attach(employee);
                db.AccessAccount.Attach(accessAccount);

                db.Employee.Remove(employee);
                db.AccessAccount.Remove(accessAccount);
                db.SaveChanges();
            }

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void IsPhoneRegisteredIsRegisteredTest()
        {
            var idEmployee = Guid.NewGuid();
            var employee = new Employee
            {
                IdEmployee = idEmployee,
                FirstName = "Test",
                LastName = "Test",
                Phone = "1234567890",
                Status = true,
                IdCharge = 1
            };
            AccessAccount accessAccount = new AccessAccount
            {
                UserName = "robertoll22",
                Password = "shenzhen",
                IdEmployee = idEmployee,
                Email = "roberto@gmail.com",
                Status = true,
            };
            EmployeeOperations.SaveEmployee(employee, accessAccount);

            bool result = EmployeeOperations.IsPhoneRegistered("1234567890");

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Employee.Attach(employee);
                db.AccessAccount.Attach(accessAccount);

                db.Employee.Remove(employee);
                db.AccessAccount.Remove(accessAccount);
                db.SaveChanges();
            }

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsPhoneRegisteredIsNotRegisteredTest()
        {
            EmployeeRegister registroEmpleado = new EmployeeRegister(true);
            var idEmployee = Guid.NewGuid();
            var employee = new Employee
            {
                IdEmployee = idEmployee,
                FirstName = "Test",
                LastName = "Test",
                Phone = "1234567890",
                Status = true,
                IdCharge = 1
            };
            AccessAccount accessAccount = new AccessAccount
            {
                UserName = "robertoll22",
                Password = "shenzhen",
                IdEmployee = idEmployee,
                Email = "roberto@gmail.com",
                Status = true,
            };
            EmployeeOperations.SaveEmployee(employee, accessAccount);

            bool result = EmployeeOperations.IsPhoneRegistered("1111111111");

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Employee.Attach(employee);
                db.AccessAccount.Attach(accessAccount);

                db.Employee.Remove(employee);
                db.AccessAccount.Remove(accessAccount);
                db.SaveChanges();
            }
            Assert.IsFalse(result);
        }
    }
}