using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BioskopCSharp.Controllers;
using BioskopCSharp.Models;

namespace UnitTestBioskopCSharp
{
    [TestClass]
    public class UnitTestCFilm
    {
        [TestMethod]
        public void TestFilmRead()
        {
            const bool expected = true;
            var tmp = CFilm.GetInstance.Read();
            var actual = (tmp.Rows.Count < 1) ? false : true;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestFilmCreate()
        {
            const bool expected = true;
            var actual = false;
            var ent = new MFilm();
            try
            {
                ent.Judul = "Test";
                ent.Harga = 1000 as int? ?? 0;
                CFilm.GetInstance.Create(ent);
                actual = true;
            }
            catch { }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestFilmUpdate()
        {
            const bool expected = true;
            var actual = false;
            var ent = new MFilm();
            try
            {
                ent.Judul = "Test";
                ent.Harga = 1000 as int? ?? 0;
                CFilm.GetInstance.Code = "12";
                CFilm.GetInstance.Update(ent);
                actual = true;
            }
            catch { }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestFilmDelete()
        {
            const bool expected = true;
            var actual = false;
            try
            {

                CFilm.GetInstance.Code = "12";
                CFilm.GetInstance.Delete();
                actual = true;
            }
            catch { }
            Assert.AreEqual(expected, actual);
        }
    }
}
