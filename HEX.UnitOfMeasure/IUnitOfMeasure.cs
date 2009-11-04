using System;

namespace HEX.UnitOfMeasure
{
    // By binding a unit to the number, convertion error and misscomparing of numbers can be avoided.

    // The following model is lacking the ablity to modulate a offset.
    // This is only a problem in relation to units of temperature, where
    // Celsius, Kelvin and Fahrenheit do not share a common zero.

    interface IUnitOfMeasure : IComparable, ICloneable
    {
        string UnitToString();
        string UnitToStringShort();
        string ToString();
        string ToString(string format);
        void Normalize();
        bool IsMetric {get;}
        bool Equals(object other);
        int GetHashCode();
    }
}
