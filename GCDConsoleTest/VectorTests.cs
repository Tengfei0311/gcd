﻿using GCDConsoleLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using GCDConsoleTest.Helpers;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace GCDConsoleLib.Tests
{
    [TestClass()]
    public class VectorTests
    {
        [TestMethod()]
        [TestCategory("Unit")]
        public void VectorTest()
        {
            string sFilepath = DirHelpers.GetTestVectorPath("StressTest.shp");
            Vector rVector = new Vector(new FileInfo(sFilepath));
            Assert.IsTrue(rVector.Features.Count > 0);
            Assert.IsTrue(rVector.Fields.Count > 0);
            Assert.AreEqual(rVector.LayerName, "StressTest");
            Assert.AreEqual(rVector.GISFileInfo.FullName, sFilepath);
            Assert.IsNotNull(rVector.Proj);

            Assert.AreEqual(rVector.GeometryType.SimpleType, GDalGeometryType.SimpleTypes.LineString);

            // Now let's make sure we can load verious broken shape files
            string sMPMG = DirHelpers.GetTestVectorPath(@"MultiPart_MultiGeometry.shp");
            Vector rMPMG = new Vector(new FileInfo(sMPMG));
            Assert.IsTrue(rMPMG.Features.Count > 0);
            Assert.IsTrue(rMPMG.Fields.Count > 0);

            string sMPSG = DirHelpers.GetTestVectorPath(@"MultiPart_SingleGeometry.shp");
            Vector rMPSG = new Vector(new FileInfo(sMPSG));
            Assert.IsTrue(rMPSG.Features.Count > 0);
            Assert.IsTrue(rMPSG.Fields.Count > 0);

            string sSPMG = DirHelpers.GetTestVectorPath(@"SinglePart_MultiGeometry.shp");
            Vector rSPMG = new Vector(new FileInfo(sSPMG));
            Assert.IsTrue(rSPMG.Features.Count > 0);
            Assert.IsTrue(rSPMG.Fields.Count > 0);

            try
            {
                string sMPL = DirHelpers.GetTestVectorPath(@"MultiPart_Line.shp");
                Vector rMPL = new Vector(new FileInfo(sMPL));
                Assert.Fail();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            try
            {
                string sMPP = DirHelpers.GetTestVectorPath(@"MultiPart_Point.shp");
                Vector rMPP = new Vector(new FileInfo(sMPP));
                Assert.Fail();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            string sSPSG = DirHelpers.GetTestVectorPath(@"SinglePart_SingleGeometry.shp");
            Vector rSPSG = new Vector(new FileInfo(sSPSG));
            Assert.IsTrue(rSPSG.Features.Count > 0);
            Assert.IsTrue(rSPSG.Fields.Count > 0);

            string sNullPoint = DirHelpers.GetTestVectorPath(@"Null_Point.shp");
            Vector rNullPoint = new Vector(new FileInfo(sNullPoint));
            Assert.IsTrue(rNullPoint.Features.Count > 0);
            Assert.IsTrue(rNullPoint.Fields.Count > 0);

            string sNullLine = DirHelpers.GetTestVectorPath(@"Null_Line.shp");
            Vector rNullLine = new Vector(new FileInfo(sNullLine));
            Assert.IsTrue(rNullLine.Features.Count > 0);
            Assert.IsTrue(rNullLine.Fields.Count > 0);

            string sNullPolygon = DirHelpers.GetTestVectorPath(@"Null_Polygon.shp");
            Vector rNullPolygon = new Vector(new FileInfo(sNullPolygon));
            Assert.IsTrue(rNullPolygon.Features.Count > 0);
            Assert.IsTrue(rNullPolygon.Fields.Count > 0);

            string sEmptyNull = DirHelpers.GetTestVectorPath(@"Emply_Null_Polygon.shp");
            Vector rEmptyNull = new Vector(new FileInfo(sEmptyNull));
            Assert.IsTrue(rEmptyNull.Features.Count > 0);
            Assert.IsTrue(rEmptyNull.Fields.Count > 0);

            try
            {
                string sEmptyFile = DirHelpers.GetTestVectorPath(@"Empty_File.shp");
                Vector vEmptyFile = new Vector(new FileInfo(sEmptyFile));
                Assert.Fail();
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void VectorCopyTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                Vector rVector1 = new Vector(new FileInfo(DirHelpers.GetTestRootPath("SulphurGCDMASK/Sulphur_ComplexGCDMask.shp")));
                FileInfo rV1Copy = new FileInfo(Path.Combine(tmp.Name, "COPYSulphur_ComplexGCDMask.shp"));
                rVector1.Copy(rV1Copy);

                // Now verify that everything got created correctly
                Vector rVector1Copy = new Vector(rV1Copy);

                Assert.IsFalse(rVector1.Fields.ContainsKey(Vector.CGDMASKFIELD));
                Assert.IsTrue(rVector1Copy.Fields.ContainsKey(Vector.CGDMASKFIELD));

                // Make sure our GCDFID is getting set properly
                foreach (KeyValuePair<long, VectorFeature> kvp in rVector1Copy.Features)
                {
                    int fieldID = rVector1Copy.Fields[Vector.CGDMASKFIELD].FieldID;
                    Assert.AreEqual(kvp.Value.Feat.GetFieldAsInteger(fieldID), kvp.Key);
                }

                Vector rVector = new Vector(new FileInfo(DirHelpers.GetTestVectorPath("StressTest.shp")));
                rVector.Copy(new FileInfo(Path.Combine(tmp.Name, "CopyShapefile.shp")));

                // Make sure we're good.
                Assert.IsTrue(File.Exists(Path.Combine(tmp.Name, "CopyShapefile.shp")));
                Assert.IsTrue(File.Exists(Path.Combine(tmp.Name, "CopyShapefile.dbf")));
                Assert.IsTrue(File.Exists(Path.Combine(tmp.Name, "CopyShapefile.prj")));
                Assert.IsTrue(File.Exists(Path.Combine(tmp.Name, "CopyShapefile.shx")));


            }
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void FileTypes()
        {
            Vector shp = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"vectors\StressTest.shp")));
            Assert.IsTrue(shp.Features.Count > 10);

            Vector geojson = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"geojson\3squares.geojson")));
            Assert.IsTrue(geojson.Features.Count > 2);

        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void VectorDeleteTest()
        {
            using (ITempDir tmp = TempDir.Create())
            {
                string sOrigPath = DirHelpers.GetTestVectorPath("StressTest.shp");
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


        [TestMethod()]
        [TestCategory("Unit")]
        public void StringVector()
        {
            string geoJSON = @"
{
    'type': 'FeatureCollection',
    'crs': {
        'type': 'name',
        'properties': {
            'name': 'urn:ogc:def:crs:OGC:1.3:CRS84'
        }
    },
    'features': [{
            'type': 'Feature',
            'properties': {
                'id': 1,
                'name': 'FeatA'
            },
            'geometry': {
                'type': 'Polygon',
                'coordinates': [
                    [
                        [-114.252403253361436, 30.888533802761863],
                        [-114.252403223569075, 30.888531408978231],
                        [-114.252400449711814, 30.888531507935365],
                        [-114.252400393960841, 30.888533878831986],
                        [-114.252403253361436, 30.888533802761863]
                    ]
                ]
            }
        },
        {
            'type': 'Feature',
            'properties': {
                'id': 2,
                'name': 'FeatB'
            },
            'geometry': {
                'type': 'Polygon',
                'coordinates': [
                    [
                        [-114.2524031592272, 30.888531092526087],
                        [-114.252403186661326, 30.888528722140837],
                        [-114.252400238159936, 30.888528628797079],
                        [-114.252400356460214, 30.888531167573458],
                        [-114.2524031592272, 30.888531092526087]
                    ]
                ]
            }
        },
        {
            'type': 'Feature',
            'properties': {
                'id': 3,
                'name': 'FeatC'
            },
            'geometry': {
                'type': 'Polygon',
                'coordinates': [
                    [
                        [-114.25240312113354, 30.888528356846415],
                        [-114.252403033521759, 30.888525915243243],
                        [-114.252400202438167, 30.88852599080198],
                        [-114.252400343125714, 30.888528284855536],
                        [-114.25240312113354, 30.888528356846415]
                    ]
                ]
            }
        }
    ]
}  ";
            // First we test using it on its own. Just a temporary file all alone in the world
            string filepath1 = "";
            using (var tmp = new TempGeoJSONFile(geoJSON))
            {
                filepath1 = tmp.fInfo.FullName;
                Vector stringVector = new Vector(tmp.fInfo);
                Assert.IsTrue(stringVector.Features.Count > 2);
                Assert.IsTrue(new FileInfo(filepath1).Exists);
            }
            // Usage is done now file is cleaned up
            Assert.IsFalse(new FileInfo(filepath1).Exists);

            // Now let's test using it as part of a temporary directory 
            string filepath2 = "";
            using (ITempDir tmpfolder = TempDir.Create())
            {
                using (var tmpfile = new TempGeoJSONFile(geoJSON, Path.Combine(tmpfolder.Name, "geojson.json")))
                {
                    filepath2 = tmpfile.fInfo.FullName;
                    Vector stringVector = new Vector(tmpfile.fInfo);
                    Assert.IsTrue(stringVector.Features.Count > 2);
                    Assert.IsTrue(new FileInfo(filepath2).Exists);
                }
                // Still here
                Assert.IsTrue(new FileInfo(filepath2).Exists);
            }
            // Now it's gone 
            Assert.IsFalse(new FileInfo(filepath2).Exists);

        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void VectorMultiPartTest()
        {
            try
            {
                Vector vPolyMask = new Vector(new FileInfo(DirHelpers.GetTestRootPath(@"vectors\MultiPart_Polygons.shp")));
                // Should not get here because the multi-part ShapeFile should produce an exception.
                Assert.Fail();
            }
            catch
            {
                // Opening multi-part ShapeFile should cause an exception!
                Assert.IsTrue(true);
            }
        }
    }
}