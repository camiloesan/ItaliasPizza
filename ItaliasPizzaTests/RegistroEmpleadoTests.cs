using Microsoft.VisualStudio.TestTools.UnitTesting;
using ItaliasPizza.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;
using ItaliasPizza.DataAccessLayer;

namespace ItaliasPizza.Pages.Tests
{
    [TestClass()]
    public class RegistroEmpleadoTests
    {
        [TestMethod()]
        public void GetChargesCorrectAmountTest()
        {
            var charges = ChargesOperations.GetCharges();
            var expected = 5;
            var actual = charges.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetChargesCorrectEntriesTest()
        {
            EmployeeRegister registroEmpleado = new EmployeeRegister(true);
            var charges = ChargesOperations.GetCharges();

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
        public void IsPasswordMatchMatchesTest()
        {
            EmployeeRegister registroEmpleado = new EmployeeRegister(true);
            bool result = registroEmpleado.IsPasswordMatch("shenzhen", "shenzhen");
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsPasswordMatchDoesNotMatchTest()
        {
            EmployeeRegister registroEmpleado = new EmployeeRegister(true);
            bool result = registroEmpleado.IsPasswordMatch("shenzhen", "pokemon");
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void IsEmailValidIsValidTest()
        {
            EmployeeRegister registroEmpleado = new EmployeeRegister(true);
            bool result = registroEmpleado.IsEmailValid("roberto@gmail.com");
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsEmailValidIsNotValidTest()
        {
            EmployeeRegister registroEmpleado = new EmployeeRegister(true);
            bool result = registroEmpleado.IsEmailValid("robert o@.gmail.com");
            Assert.IsFalse(result);
        }
    }
}