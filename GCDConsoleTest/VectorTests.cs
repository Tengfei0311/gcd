﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace GCDConsoleLib.Tests
{
    [TestClass()]
    public class VectorTests
    {
        [TestMethod()]
        public void VectorTest()
        {
            string sFilepath = TestHelpers.GetTestVectorPath("StressTest.shp");
            Vector rVector = new Vector(new FileInfo(sFilepath));
            Assert.IsTrue(rVector.Features.Count > 0);
            Assert.IsTrue(rVector.Fields.Count > 0);
            Assert.AreEqual(rVector.LayerName, "StressTest");
            Assert.AreEqual(rVector.GISFileInfo.FullName, sFilepath);
            Assert.IsNotNull(rVector.Proj);
        }

        [TestMethod()]
        public void VectorCopyTest()
        {
            using (Utility.ITempDir tmp = Utility.TempDir.Create())
            {
                Vector rVector = new Vector(new FileInfo(TestHelpers.GetTestVectorPath("StressTest.shp")));
                rVector.Copy(new FileInfo(Path.Combine(tmp.Name, "CopyShapefile.shp")));

                // Make sure we're good.
                Assert.IsTrue(File.Exists(Path.Combine(tmp.Name, "CopyShapefile.shp")));
                Assert.IsTrue(File.Exists(Path.Combine(tmp.Name, "CopyShapefile.dbf")));
                Assert.IsTrue(File.Exists(Path.Combine(tmp.Name, "CopyShapefile.prj")));
                Assert.IsTrue(File.Exists(Path.Combine(tmp.Name, "CopyShapefile.shx")));
            }
        }

        [TestMethod()]
        public void FileTypes()
        {
            Vector shp = new Vector(new FileInfo(TestHelpers.GetTestRootPath(@"vectors\StressTest.shp")));
            Assert.IsTrue(shp.Features.Count > 10);

            Vector geojson = new Vector(new FileInfo(TestHelpers.GetTestRootPath(@"geojson\3squares.json")));
            Assert.IsTrue(geojson.Features.Count > 2);

        }

        [TestMethod()]
        public void VectorDeleteTest()
        {
            using (Utility.ITempDir tmp = Utility.TempDir.Create())
            {
                string sOrigPath = TestHelpers.GetTestVectorPath("StressTest.shp");
                string sDeletePath = Path.Combine(tmp.Name, "DeleteShapefile.shp");

                Vector rVector = new Vector(new FileInfo(sOrigPath));
                rVector.Copy(new FileInfo(sDeletePath));
                //Make sure our setup is good
                Assert.IsTrue(File.Exists(sDeletePath));

                // Now delete what we just copied
                Vector rVectorCopy = new Vector(new FileInfo(sDeletePath));
                rVectorCopy.Delete();

                // Make sure we're good.
                Assert.IsFalse(File.Exists(sDeletePath));
                Assert.IsFalse(File.Exists(Path.Combine(tmp.Name, "DeleteShapefile.dbf")));
                Assert.IsFalse(File.Exists(Path.Combine(tmp.Name, "DeleteShapefile.prj")));
                Assert.IsFalse(File.Exists(Path.Combine(tmp.Name, "DeleteShapefile.shx")));
            }
        }

    }
}