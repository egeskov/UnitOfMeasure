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

        [TestMethod]
        public void ExampleForReadMe()
        {
            Length lhs = new Length(2.1, LengthUnit.Meter);
            Length rhs = new Length(43.4, LengthUnit.Inch);

            Length result = lhs + rhs;
            Console.WriteLine($"Adding {lhs} to {rhs} equals {result}.");

            result.Unit = LengthUnit.Foot;
            Console.WriteLine($"Result in specific unit: {result}.");


            var l = new Length(1.45, LengthUnit.Meter);
            var a = l * l;
            Console.WriteLine($"{l} times {l} equals {a}.");

            var v = l * a;
            Console.WriteLine($"{l} times {a} equals {v}.");


            var distance = new Length(15.3, LengthUnit.KiloMeter);
            var time = new TimeSpan(0, 13, 25);
            var averageSpeed = distance / time;
            Console.WriteLine($"Travelling {distance} with an average speed of {averageSpeed} takes 13 minutes and 25 seconds.");
        }
    }
}
