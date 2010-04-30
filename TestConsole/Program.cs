using System;
using HEX.UnitOfMeasure;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var l = new Length(1.45, LengthUnit.Meter);
            var a = l * l;
            var v = l * a;
            Console.WriteLine(l);
            Console.WriteLine(a);
            Console.WriteLine(v);

            var f = new Length(2.4, LengthUnit.Fathom);
            Console.WriteLine(f);
            Console.ReadKey();
        }
    }
}
