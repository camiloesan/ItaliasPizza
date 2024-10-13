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

        [TestMethod()]
        public void IsEmailRegisteredTest()
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

            bool result = EmployeeOperations.IsEmailRegistered("roberto@gmail.com");

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
        public void IsEmailRegisteredDoesNotExistTest()
        {
            bool result = EmployeeOperations.IsEmailRegistered("roberto@gmail.com");

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void UpdateEmployeeSuccessTest()
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

            var modifiedEmployee = new Employee
            {
                IdEmployee = idEmployee,
                FirstName = "jose",
                LastName = "perez",
                Phone = "0000000000",
                Status = true,
                IdCharge = 2
            };

            bool result = false;
            int rows = EmployeeOperations.UpdateEmployee(modifiedEmployee);
            if (rows > 0)
            {
                result = true;
            }

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
        public void UpdateEmployeeDoesNotExistTest()
        {
            var idEmployee = Guid.NewGuid();
            var modifiedEmployee = new Employee
            {
                IdEmployee = idEmployee,
                FirstName = "jose",
                LastName = "perez",
                Phone = "0000000000",
                Status = true,
                IdCharge = 2
            };

            bool result = false;
            int rows = 0;
            try
            {
                rows = EmployeeOperations.UpdateEmployee(modifiedEmployee);
            }
            catch
            {
                result = false;
            }
            if (rows > 0)
            {
                result = true;
            }

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void GetEmployeeEmailEmpDoesNotExistTest()
        {
            var result = EmployeeOperations.GetEmployeeEmail(Guid.NewGuid());
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void GetEmployeeEmailSuccessTest()
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

            var result = EmployeeOperations.GetEmployeeEmail(idEmployee);

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Employee.Attach(employee);
                db.AccessAccount.Attach(accessAccount);

                db.Employee.Remove(employee);
                db.AccessAccount.Remove(accessAccount);
                db.SaveChanges();
            }

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void GetEmployeeByIdDoesNotExistTest()
        {
            var result = EmployeeOperations.GetEmployeeById(Guid.NewGuid());
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void GetEmployeeByIdSuccessTest()
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

            var result = EmployeeOperations.GetEmployeeById(idEmployee);

            using (var db = new ItaliasPizzaDBEntities())
            {
                db.Employee.Attach(employee);
                db.AccessAccount.Attach(accessAccount);

                db.Employee.Remove(employee);
                db.AccessAccount.Remove(accessAccount);
                db.SaveChanges();
            }

            Assert.IsNotNull(result);
        }
    }
}