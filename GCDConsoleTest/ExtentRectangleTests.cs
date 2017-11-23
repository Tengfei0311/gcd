﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib;
using System;
using System.Collections.Generic;

namespace GCDConsoleLib.Tests
{
    [TestClass()]
    public class ExtentRectangleTests
    {
        [TestMethod()]
        public void ExtentRectangleTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void BufferTest()
        {
            ExtentRectangle rA1 = new ExtentRectangle(5, 6, -1, 1, 100, 100);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            ExtentRectangle rA1 = new ExtentRectangle(5, 6, -1, 1, 100, 100);
            Assert.AreEqual(rA1.ToString(), "6 -95 106 5");
        }

        [TestMethod()]
        public void UnionTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void IntersectTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void GetTranslationTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void IsConcurrentTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void IsOrthogonalTest()
        {
            ExtentRectangle rA1 = new ExtentRectangle(5, 6, -1, 1, 100, 100);
            ExtentRectangle rB1 = new ExtentRectangle(1, 3.0, -1, 1, 100, 100);
            ExtentRectangle rC1 = new ExtentRectangle(2, 7, -1, 1, 100, 100);
            ExtentRectangle rD1 = new ExtentRectangle(4.1, 6.0, -1, 1, 100, 100);

            Assert.AreEqual(rA1.MaxArrID, 9999);

            // Positive Tests
            Assert.IsTrue(rA1.IsOrthogonal(rA1));
            Assert.IsTrue(rA1.IsOrthogonal(rB1) && rB1.IsOrthogonal(rA1));
            Assert.IsTrue(rA1.IsOrthogonal(rC1) && rC1.IsOrthogonal(rA1));

            // Negative Tests
            Assert.IsFalse(rA1.IsOrthogonal(rD1) || rD1.IsOrthogonal(rA1));
        }

        [TestMethod()]
        public void IsDivisibleTest()
        {
            // Positive test
            ExtentRectangle rDivisible1 = new ExtentRectangle(5, 6, -1, 1, 100, 100);
            Assert.IsTrue(rDivisible1.IsDivisible());

            //Negative Test
            ExtentRectangle rNotDivisible1 = new ExtentRectangle(5, 6, -2, 2, 100, 100);
            Assert.IsFalse(rNotDivisible1.IsDivisible());

            // Positive test
            ExtentRectangle rDivisible2 = new ExtentRectangle(5.1, 6.1, -0.1, 0.1, 100, 100);
            Assert.IsTrue(rDivisible2.IsDivisible());

            //Negative Test
            ExtentRectangle rNotDivisible2 = new ExtentRectangle(5.1, 6.1, -0.1, 0.2, 100, 100);
            Assert.IsFalse(rNotDivisible2.IsDivisible());

            //Negative Test
            ExtentRectangle rNotDivisible3 = new ExtentRectangle(5.1, 6.1, -0.2, 0.1, 100, 100);
            Assert.IsFalse(rNotDivisible3.IsDivisible());

            // Positive Test for floating point weirdness
            ExtentRectangle rDivisible4 = new ExtentRectangle(5.0999999999999999999999999999999999999999999, 6.1, -0.1, 0.1, 100, 100);
            Assert.IsTrue(rDivisible4.IsDivisible());

            // Negative Test for floating point weirdness
            ExtentRectangle rNotDivisible4 = new ExtentRectangle(5.0999999999999999999999999999999999999999999, 6.1, -0.2, 0.1, 100, 100);
            Assert.IsFalse(rNotDivisible4.IsDivisible());
        }

        [TestMethod()]
        public void MakeDivisibleTest()
        {
            ///
            /// Here's an already divisible raster. Make sure we don't change it.
            ///
            ExtentRectangle eA1 = new ExtentRectangle(5.1, 6.4, 0.1, 0.2, 100, 100);

            // This should do nothing
            ExtentRectangle eA1result = eA1.GetDivisibleExtent();
            Assert.AreEqual(eA1result.Top, eA1.Top);
            Assert.AreEqual(eA1result.Bottom, eA1.Bottom);
            Assert.AreEqual(eA1result.Left, eA1.Left);
            Assert.AreEqual(eA1result.Right, eA1.Right);
            Assert.AreEqual(eA1result.rows, eA1.rows);
            Assert.AreEqual(eA1result.cols, eA1.cols);

            ///
            /// NEGStep 1: Negative Cell Height, Positive Cell Width
            ///
            ExtentRectangle eNotDivisible = new ExtentRectangle(5.09, 6.5, -0.1, 0.2, 100, 100);
            Assert.IsFalse(eNotDivisible.IsDivisible());

            // Now Make it divisible in the right way
            ExtentRectangle eDivisible = eNotDivisible.GetDivisibleExtent();
            Assert.AreEqual(eDivisible.Top, 5.1m);
            Assert.AreEqual(eDivisible.Bottom, -5.0m);
            Assert.AreEqual(eDivisible.Left, 6.4m);
            Assert.AreEqual(eDivisible.Right, 26.6m);
            Assert.AreEqual(eDivisible.rows, 101);
            Assert.AreEqual(eDivisible.cols, 101);

            ///
            /// NEGStep 1: Negative Cell Height, Positive Cell Width
            ///
            ExtentRectangle eNegStep1NonDivisible = new ExtentRectangle(5.09, 6.39, -0.1, 0.2, 100, 100);
            Assert.IsFalse(eNegStep1NonDivisible.IsDivisible());

            //Now Make it divisible in the right way
            ExtentRectangle eNegStep1 = eNegStep1NonDivisible.GetDivisibleExtent();
            Assert.AreEqual(eNegStep1.Top, 5.1m);
            Assert.AreEqual(eNegStep1.Bottom, -5.0m);
            Assert.AreEqual(eNegStep1.Left, 6.2m);
            Assert.AreEqual(eNegStep1.Right, 26.4m);
            Assert.AreEqual(eNegStep1.rows, 101);
            Assert.AreEqual(eNegStep1.cols, 101);

            ///
            /// NEGStep 2: Positive Cell Height, Negative Cell Width
            ///
            ExtentRectangle eNegStep2NonDivisible = new ExtentRectangle(5.09, 6.39, 0.1, -0.2, 100, 100);
            Assert.IsFalse(eNegStep2NonDivisible.IsDivisible());

            //Now Make it divisible in the right way
            ExtentRectangle eNegStep2 = eNegStep2NonDivisible.GetDivisibleExtent();
            Assert.AreEqual(eNegStep2.Top, 5.0m);
            Assert.AreEqual(eNegStep2.Bottom, 15.1m);
            Assert.AreEqual(eNegStep2.Left, 6.4m);
            Assert.AreEqual(eNegStep2.Right, -13.8m);
            Assert.AreEqual(eNegStep2.rows, 101);
            Assert.AreEqual(eNegStep2.cols, 101);

            ///
            /// NegWorldNEGStep 1: Negative Coordinates, Negative Cell Height, Positive Cell Width
            ///
            ExtentRectangle eNWNegStep1NonDivisible = new ExtentRectangle(-5.09, -6.39, -0.1, 0.2, 100, 100);
            Assert.IsFalse(eNWNegStep1NonDivisible.IsDivisible());

            //Now Make it divisible in the right way
            ExtentRectangle eNWNegStep1 = eNWNegStep1NonDivisible.GetDivisibleExtent();
            Assert.AreEqual(eNWNegStep1.Top, -5.0m);
            Assert.AreEqual(eNWNegStep1.Bottom, -15.1m);
            Assert.AreEqual(eNWNegStep1.Left, -6.4m);
            Assert.AreEqual(eNWNegStep1.Right, 13.8m);
            Assert.AreEqual(eNWNegStep1.rows, 101);
            Assert.AreEqual(eNWNegStep1.cols, 101);

            ///
            /// NegWorldNEGStep 2: Negative Coordinates, Positive Cell Height, Negative Cell Width
            ///
            ExtentRectangle eNWNegStep2NonDivisible = new ExtentRectangle(-5.09, -6.39, 0.1, -0.2, 100, 100);
            Assert.IsFalse(eNWNegStep2NonDivisible.IsDivisible());

            //Now Make it divisible in the right way
            ExtentRectangle eNWNegStep2 = eNWNegStep2NonDivisible.GetDivisibleExtent();
            Assert.AreEqual(eNWNegStep2.Top, -5.1m);
            Assert.AreEqual(eNWNegStep2.Bottom, 5.0m);
            Assert.AreEqual(eNWNegStep2.Left, -6.2m);
            Assert.AreEqual(eNWNegStep2.Right, -26.4m);
            Assert.AreEqual(eNWNegStep2.rows, 101);
            Assert.AreEqual(eNWNegStep2.cols, 101);

        }

        [TestMethod()]
        public void HasOverlapTest()
        {
            Assert.Inconclusive();
            // TODO: TEST THIS THOROUGHLY, ESPECIALLY -/+ widht heights
            Assert.Fail();
            ExtentRectangle eA1 = new ExtentRectangle(5.1, 6.4, -0.1, 0.2, 100, 100);
            ExtentRectangle eInside = new ExtentRectangle(3, 9, -0.1, 0.2, 50, 50);
            ExtentRectangle eOverlap1 = new ExtentRectangle(9.1, 0.4, -0.1, 0.2, 100, 100);

            ExtentRectangle eEdgesOverlap = new ExtentRectangle(5.1, 6.4, 0.1, 0.2, 100, 100);

            ExtentRectangle eNoOverlap = new ExtentRectangle(5.1, 6.4, 0.1, 0.2, 100, 100);

            // Some positive Tests
            Assert.IsTrue(eA1.HasOverlap(eA1));
            Assert.IsTrue(eA1.HasOverlap(eInside));
            Assert.IsTrue(eA1.HasOverlap(eOverlap1));

            // Some border cases
            Assert.IsFalse(eA1.HasOverlap(eEdgesOverlap));

            // Some negative tests
            Assert.IsFalse(eA1.HasOverlap(eNoOverlap));
        }


        [TestMethod()]
        public void RowCol2IdTest()
        {
            ExtentRectangle rTest1 = new ExtentRectangle(5, 6, -1, 1, 100, 100);
            Assert.AreEqual(rTest1.RowCol2Id(2, 3), 102);
            Assert.AreEqual(rTest1.RowCol2Id(1, 1), 0);
            Assert.AreEqual(rTest1.RowCol2Id(1, 2), 1);
            Assert.AreEqual(rTest1.RowCol2Id(2, 1), 100);
            Assert.AreEqual(rTest1.RowCol2Id(100, 100), 9999);
        }

        [TestMethod()]
        public void Id2RowColTest()
        {
            ExtentRectangle rTest1 = new ExtentRectangle(5, 6, -1, 1, 100, 50);
            Assert.AreEqual(rTest1.Id2RowCol(52), new Tuple<int, int>(2, 3));
            Assert.AreEqual(rTest1.Id2RowCol(0), new Tuple<int, int>(1, 1));
            Assert.AreEqual(rTest1.Id2RowCol(1), new Tuple<int, int>(1, 2));
            Assert.AreEqual(rTest1.Id2RowCol(100), new Tuple<int, int>(3, 1));
            Assert.AreEqual(rTest1.Id2RowCol(4999), new Tuple<int, int>(100, 50));
        }

        [TestMethod()]
        public void RelativeId()
        {
            ExtentRectangle rTest1 = new ExtentRectangle(4, 6, -1, 1, 10, 10);
            ExtentRectangle rTest2 = new ExtentRectangle(5, 3, -1, 1, 8, 8);

            Assert.AreEqual(rTest1.RelativeId(0, rTest1), 0);
            Assert.AreEqual(rTest1.RelativeId(0, rTest2), 11);
            Assert.AreEqual(rTest2.RelativeId(0, rTest1), -1);
            Assert.AreEqual(rTest2.RelativeId(0, rTest2), 0);

            Assert.AreEqual(rTest1.RelativeId(1, rTest1), 1);
            Assert.AreEqual(rTest1.RelativeId(1, rTest2), 12);
            Assert.AreEqual(rTest2.RelativeId(1, rTest1), -1);
            Assert.AreEqual(rTest2.RelativeId(1, rTest2), 1);

            Assert.AreEqual(rTest1.RelativeId(24, rTest1), 24);
            Assert.AreEqual(rTest1.RelativeId(24, rTest2), 31);
            Assert.AreEqual(rTest2.RelativeId(24, rTest1), -1);
            Assert.AreEqual(rTest2.RelativeId(24, rTest2), 24);

            Assert.AreEqual(rTest1.RelativeId(25, rTest1), 25);
            Assert.AreEqual(rTest1.RelativeId(25, rTest2), -1);
            Assert.AreEqual(rTest2.RelativeId(25, rTest1), -1);
            Assert.AreEqual(rTest2.RelativeId(25, rTest2), 25);

            Assert.AreEqual(rTest1.RelativeId(99, rTest1), 99);
            Assert.AreEqual(rTest1.RelativeId(99, rTest2), -1);
            Assert.AreEqual(rTest2.RelativeId(99, rTest1), -1);
            Assert.AreEqual(rTest2.RelativeId(99, rTest2), -1);
        }

    }
}