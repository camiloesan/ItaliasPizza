using ItaliasPizza.DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Database;

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
            var result = AccessAccountOperations.GetEmployeeCharge("camiloesan@gmail.com");
            Assert.AreEqual("Gerente", result);
        }
    }
}