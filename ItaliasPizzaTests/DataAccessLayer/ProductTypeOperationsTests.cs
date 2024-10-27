using Microsoft.VisualStudio.TestTools.UnitTesting;
using ItaliasPizza.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliasPizza.DataAccessLayer.Tests
{
    [TestClass()]
    public class ProductTypeOperationsTests
    {
        [TestMethod()]
        public void GetProductTypesTest()
        {
            var result = ProductTypeOperations.GetProductTypes();
            Assert.IsNotNull(result);
        }
    }
}