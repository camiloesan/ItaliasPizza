using Microsoft.VisualStudio.TestTools.UnitTesting;
using ItaliasPizza.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizza.Pages.Tests
{
    [TestClass()]
    public class RegistroEmpleadoTests
    {
        [TestMethod()]
        public void GetChargesTest()
        {
            RegistroEmpleado registroEmpleado = new RegistroEmpleado(true);
            var charges = registroEmpleado.GetCharges();
            var expected = 2;
            var actual = charges.Count;
            Assert.AreEqual(expected, actual);
        }
    }
}