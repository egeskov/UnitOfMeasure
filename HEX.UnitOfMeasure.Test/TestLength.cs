using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HEX.UnitOfMeasure.Test
{
    [TestClass]
    public class TestLength
    {
        [TestMethod]
        public void TestAddition()
        {
            Length lhs = new Length(1.25, LengthUnit.Meter);
            Length rhs = new Length(10, LengthUnit.CentiMeter);

            Length expected = new Length(1.35, LengthUnit.Meter);
            Length actual = (lhs + rhs);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMultiplication()
        {
            var l = new Length(1.45, LengthUnit.Meter);

            var a = l * l;
            var expectedArea = new Area(1.45 * 1.45, AreaUnit.SqrMeter);
            Assert.AreEqual(expectedArea.ToString(), a.ToString());

            var v = l * a;
            Volume expectedVolume = new Volume(1.45 * 1.45 * 1.45, VolumeUnit.CubicMeter);
            Assert.AreEqual(expectedVolume.ToString(), v.ToString());
        }
    }
}
