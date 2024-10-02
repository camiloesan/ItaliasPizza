using Microsoft.VisualStudio.TestTools.UnitTesting;
using ItaliasPizza.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace ItaliasPizza.Pages.Tests
{
    [TestClass()]
    public class RegistroEmpleadoTests
    {
        [TestMethod()]
        public void GetChargesCorrectAmountTest()
        {
            RegistroEmpleado registroEmpleado = new RegistroEmpleado(true);
            var charges = registroEmpleado.GetCharges();
            var expected = 4;
            var actual = charges.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetChargesCorrectEntriesTest()
        {
            RegistroEmpleado registroEmpleado = new RegistroEmpleado(true);
            var charges = registroEmpleado.GetCharges();

            var expected = new List<string> { "Mesero", "Cajero", "Repartidor", "Cocinero" };

            bool result = false;
            for (int i = 0; i < expected.Count; i++)
            {
                if (charges.FirstOrDefault(c => c.Name == expected[i]) != null)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void SaveEmployeeTest()
        {
            RegistroEmpleado registroEmpleado = new RegistroEmpleado(true);
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
            var actual = registroEmpleado.SaveEmployee(employee, accessAccount);

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
        public void IsPasswordMatchMatchesTest()
        {
            RegistroEmpleado registroEmpleado = new RegistroEmpleado(true);
            bool result = registroEmpleado.IsPasswordMatch("shenzhen", "shenzhen");
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsPasswordMatchDoesNotMatchTest()
        {
            RegistroEmpleado registroEmpleado = new RegistroEmpleado(true);
            bool result = registroEmpleado.IsPasswordMatch("shenzhen", "pokemon");
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void IsPhoneRegisteredIsRegisteredTest()
        {
            RegistroEmpleado registroEmpleado = new RegistroEmpleado(true);
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
            registroEmpleado.SaveEmployee(employee, accessAccount);
            
            bool result = registroEmpleado.IsPhoneRegistered("1234567890");

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
            RegistroEmpleado registroEmpleado = new RegistroEmpleado(true);
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
            registroEmpleado.SaveEmployee(employee, accessAccount);

            bool result = registroEmpleado.IsPhoneRegistered("1111111111");

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
        public void IsEmailValidIsValidTest()
        {
            RegistroEmpleado registroEmpleado = new RegistroEmpleado(true);
            bool result = registroEmpleado.IsEmailValid("roberto@gmail.com");
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsEmailValidIsNotValidTest()
        {
            RegistroEmpleado registroEmpleado = new RegistroEmpleado(true);
            bool result = registroEmpleado.IsEmailValid("robert o@.gmail.com");
            Assert.IsFalse(result);
        }
    }
}