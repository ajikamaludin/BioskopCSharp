using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BioskopCSharp.Controllers;

namespace UnitTestBioskopCSharp
{
    [TestClass]
    public class UnitTestCJadwal
    {
        [TestMethod]
        public void TestJadwalRead()
        {
            const bool expected = true;
            var tmp = CJadwal.GetInstance.Read();
            var actual = (tmp.Rows.Count < 1) ? false : true;
            Assert.AreEqual(expected, actual);
        }
    }
}
