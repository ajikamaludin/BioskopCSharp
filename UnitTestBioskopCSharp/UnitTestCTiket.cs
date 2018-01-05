using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BioskopCSharp.Controllers;

namespace UnitTestBioskopCSharp
{
    [TestClass]
    public class UnitTestCTiket
    {
        [TestMethod]
        public void TestTiketRead()
        {
            const bool expected = true;
            var tmp = CTiket.GetInstance.Read();
            var actual = (tmp.Rows.Count < 1) ? false : true;
            Assert.AreEqual(expected, actual);
        }
    }
}
