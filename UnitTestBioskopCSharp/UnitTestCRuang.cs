using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BioskopCSharp.Controllers;
using BioskopCSharp.Models;

namespace UnitTestBioskopCSharp
{
    [TestClass]
    public class UnitTestCRuang
    {
        [TestMethod]
        public void TestReadRuang()
        {
            const bool expected = true;
            var tmp = CRuang.GetInstance.Read();
            var actual = (tmp.Rows.Count < 1) ? false : true;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestCreateRuang()
        {
            const bool expected = true;
            var actual = false;
            var ent = new MRuang();
            try
            {
                ent.Nama = "Test";
                CRuang.GetInstance.Create(ent);
                actual = true;
            }
            catch { }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestDeleteRuang()
        {
            const bool expected = true;
            var actual = false;
            try
            {
                CRuang.GetInstance.Code = "1";
                CRuang.GetInstance.Delete();
                actual = true;
            }
            catch { }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestUpdateRuang()
        {
            const bool expected = true;
            var actual = false;
            var ent = new MRuang();
            try
            {
                ent.Nama = "Test2";
                CRuang.GetInstance.Update(ent);
                actual = true;
            }
            catch { }
            Assert.AreEqual(expected, actual);
        }
    }
}
