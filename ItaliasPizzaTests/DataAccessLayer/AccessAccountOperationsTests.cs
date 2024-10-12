using ItaliasPizza.DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Database;
using ItaliasPizza.Pages;

namespace ItaliasPizza.DataAccessLayer.Tests
{
    [TestClass()]
    public class AccessAccountOperationsTests
    {
        [TestMethod()]
        public void AreCredentialsValidSuccessTest()
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
                Password = "123456",
                IdEmployee = idEmployee,
                Email = "roberto@gmail.com",
                Status = true,
            };
            EmployeeOperations.SaveEmployee(employee, accessAccount);

            var result = AccessAccountOperations.AreCredentialsValid("roberto@gmail.com", "123456");

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
        public void AreCredentialsValidFailTest()
        {
            var result = AccessAccountOperations.AreCredentialsValid("martinn@gmail.com", "123456");
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void GetEmployeeChargeTest()
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
                Password = "123456",
                IdEmployee = idEmployee,
                Email = "roberto@gmail.com",
                Status = true,
            };
            EmployeeOperations.SaveEmployee(employee, accessAccount);

            var result = AccessAccountOperations.GetEmployeeCharge("camiloesan@gmail.com");

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Employee.Attach(employee);
                db.AccessAccount.Attach(accessAccount);

                db.Employee.Remove(employee);
                db.AccessAccount.Remove(accessAccount);
                db.SaveChanges();
            }

            Assert.AreEqual("Gerente", result);
        }

        [TestMethod()]
        public void GetAccessAccountByEmailTest()
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
                Password = "123456",
                IdEmployee = idEmployee,
                Email = "roberto@gmail.com",
                Status = true,
            };
            EmployeeOperations.SaveEmployee(employee, accessAccount);

            var result = AccessAccountOperations.GetAccessAccountByEmail("roberto@gmail.com");

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Employee.Attach(employee);
                db.AccessAccount.Attach(accessAccount);

                db.Employee.Remove(employee);
                db.AccessAccount.Remove(accessAccount);
                db.SaveChanges();
            }

            Assert.AreEqual(result.Status, accessAccount.Status);
        }

        [TestMethod()]
        public void GetAccessAccountByEmailDoesNotExistTest()
        {
            var result = AccessAccountOperations.GetAccessAccountByEmail("roberto@gmail.com");

            Assert.IsNull(result);
        }

        [TestMethod()]
        public void GetEmployeeIdDoesNotExistTest()
        {
            var result = AccessAccountOperations.GetEmployeeId("roberto@gmail.com");
            Assert.AreEqual(result, Guid.Empty);
        }

        [TestMethod()]
        public void GetEmployeeIdDoesExistTest()
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
                Password = "123456",
                IdEmployee = idEmployee,
                Email = "roberto@gmail.com",
                Status = true,
            };
            EmployeeOperations.SaveEmployee(employee, accessAccount);

            var result = AccessAccountOperations.GetEmployeeId("roberto@gmail.com");

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Employee.Attach(employee);
                db.AccessAccount.Attach(accessAccount);

                db.Employee.Remove(employee);
                db.AccessAccount.Remove(accessAccount);
                db.SaveChanges();
            }

            Assert.AreEqual(result, idEmployee);
        }
    }
}