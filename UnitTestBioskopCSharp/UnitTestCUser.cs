using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BioskopCSharp.Controllers;
using BioskopCSharp.Models;

namespace UnitTestBioskopCSharp
{
    [TestClass]
    public class UnitTestCUser
    {
        [TestMethod]
        public void TestReadUser()
        {
            const bool expected = true;
            var tmp = CUser.GetInstance.Read();
            var actual = (tmp.Rows.Count < 1) ? false : true;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestCreateUser()
        {
            const bool expected = true;
            var actual = false;
            var ent = new MUser();
            try
            {
                ent.Nama = "Test";
                ent.Username = "Test";
                ent.Password = "Test";
                CUser.GetInstance.Create(ent);
                actual = true;
            }
            catch { }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestUpdateUser()
        {
            const bool expected = true;
            var actual = false;
            var ent = new MUser();
            try
            {
                ent.Nama = "Test2";
                ent.Username = "Test2";
                ent.Password = "Test2";
                CUser.GetInstance.Code = "9";
                CUser.GetInstance.Update(ent);
                actual = true;
            }
            catch { }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestDeleteUser()
        {
            const bool expected = true;
            var actual = false;
            try
            {
                CUser.GetInstance.Code = "9";
                CUser.GetInstance.Delete();
                actual = true;
            }
            catch { }
            Assert.AreEqual(expected, actual);
        }
    }
}
