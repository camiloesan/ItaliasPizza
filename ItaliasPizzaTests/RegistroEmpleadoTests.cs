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
            var expected = 5;
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