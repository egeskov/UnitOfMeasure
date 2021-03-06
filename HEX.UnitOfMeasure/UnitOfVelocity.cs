﻿using System;

namespace HEX.UnitOfMeasure
{
    /// <summary>
    /// Enumeration of units of measure for velicity.
    /// </summary>
    public enum VelocityUnit
    {
        // Metric common
        MeterPerSecond, KiloMeterPerHour,
        // Metric rare
        MilliMeterPerHour, MilliMeterPerMinute, MilliMeterPerSecond, CentiMeterPerMinute, CentiMeterPerSecond, MeterPerMinute,

        // Imperial common
        MilePerHour,
        // Imperial rare
        InchPerSecond, InchPerHour, FootPerHour, FootPerMinute, FootPerSecond, MilePerMinute,

        // Astronimical
        SpeedOfSound, SpeedOfLight,

        // Nautical
        Knot
    }

    public enum VelocitySystem
    {
        Metric,
        Imperial,
        Astronomical,
        Nautical
    }

    /// <summary>
    /// Class for holding measurement of velocity. 
    /// By binding a unit to the number, convertion error and misscomparing of numbers can be avoided.
    /// </summary>
    public class Velocity : IUnitOfMeasure
    {
        #region Constants

        private static readonly string[] unitTextShort = {
            "ms", "kmh",
            "mm/h", "mm/min", "mm/s", "cm/min", "cm/s", "m/min",
            "mph",
            "in/s", "in/h", "ft/h", "ft/min", "ft/s", "mi/min",
            "x speed of sound", "x speed of light",
            "kn"
        };
        private static readonly string[] unitText = {
            "meters per second", "kilometers per hour",
            "millimeters per hour", "millimeters per minute", "milimeters per second", "centimeters per minute", "centimeters per second", "meters per minute",
            "miles per hour",
            "inches per second", "inches per hour", "foots per hour", "foots per minute", "foots per second", "miles per minute",
            "times the speed of sound", "times the speed of light",
            "knots"
        };

        const VelocityUnit UNIT_IDX_METRIC_MIN = VelocityUnit.MeterPerSecond;
        const VelocityUnit UNIT_IDX_METRIC_MAX_STD = VelocityUnit.KiloMeterPerHour;
        const VelocityUnit UNIT_IDX_METRIC_MAX = VelocityUnit.MeterPerMinute;

        const VelocityUnit UNIT_IDX_IMPERIAL_MIN = VelocityUnit.MilePerHour;
        const VelocityUnit UNIT_IDX_IMPERIAL_MAX_STD = VelocityUnit.MilePerHour;
        const VelocityUnit UNIT_IDX_IMPERIAL_MAX = VelocityUnit.MilePerMinute;

        const VelocityUnit UNIT_IDX_ASTRONIMICAL_MIN = VelocityUnit.SpeedOfLight;
        const VelocityUnit UNIT_IDX_ASTRONIMICAL_MAX_STD = VelocityUnit.SpeedOfLight;
        const VelocityUnit UNIT_IDX_ASTRONIMICAL_MAX = VelocityUnit.SpeedOfLight;

        const VelocityUnit UNIT_IDX_NAUTICAL = VelocityUnit.Knot;

        public const VelocityUnit SI = VelocityUnit.MeterPerSecond;

        #endregion Constants


        VelocityUnit unit = VelocityUnit.MeterPerSecond;
        double internalValue = 0;


        #region Constructors

        public Velocity()
        { }

        public Velocity(double value)
        {
            internalValue = value;
        }

        public Velocity(double value, VelocityUnit unit)
        {
            this.unit = unit;
            internalValue = value;
        }

        public Velocity(Velocity other)
        {
            unit = other.unit;
            internalValue = other.internalValue;
        }

        #endregion Constructors


        #region Text

        public string UnitToString()
        {
            return unitText[(int)unit];
        }

        public string UnitToStringShort()
        {
            return unitTextShort[(int)unit];
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", internalValue, UnitToStringShort());
        }

        public string ToString(string format)
        {
            format = format.ToLower();
            if (format.Contains("l"))
                return internalValue.ToString(format.Replace("l", "")) + " " + UnitToString();
            else
                return internalValue.ToString(format) + " " + UnitToStringShort();
        }

        /// <summary>
        /// Generate Velocity from string.
        /// </summary>
        /// <param name="s">String composed of number and unit.</param>
        /// <returns>Velocity.</returns>
        public static Velocity Parse(string s)
        {
            if (s != null)
            {
                s.Trim();
                int idx = 0;
                while (idx < s.Length && !char.IsLetter(s, idx))
                    idx++;
                double number = double.Parse(s.Substring(0, idx - 1));
                string unit = s.Substring(idx).Trim();
                idx = Array.IndexOf(unitTextShort, unit);
                if (idx >= 0)
                    return new Velocity(number, (VelocityUnit)idx);
                else
                    throw new FormatException("Unknown unit (" + unit + ")");
            }
            else
                throw new ArgumentNullException();
        }

        /// <summary>
        /// Generate Velocity from string.
        /// </summary>
        /// <param name="s">String composed of number and unit.</param>
        /// <param name="result">Velocity.</param>
        /// <returns>True if string parsed succesfully.</returns>
        public static bool TryParse(string s, out Velocity result)
        {
            try
            {
                result = Parse(s);
                return true;
            }
            catch (Exception)
            {
                result = new Velocity();
                return false;
            }
        }

        #endregion Text


        #region UnitSystem

        public VelocitySystem System
        {
            get { return GetSystem(unit); }
        }

        private static VelocitySystem GetSystem(VelocityUnit unit)
        {
            if (UNIT_IDX_METRIC_MIN <= unit && unit <= UNIT_IDX_METRIC_MAX)
                return VelocitySystem.Metric;
            else if (UNIT_IDX_IMPERIAL_MIN <= unit && unit <= UNIT_IDX_IMPERIAL_MAX)
                return VelocitySystem.Imperial;
            else
                return VelocitySystem.Astronomical;
        }

        public bool IsMetric
        {
            get { return GetSystem(unit) == VelocitySystem.Metric; }
        }

        public bool IsImperial
        {
            get { return GetSystem(unit) == VelocitySystem.Imperial; }
        }

        public bool IsAstronimical
        {
            get { return GetSystem(unit) == VelocitySystem.Astronomical; }
        }

        #endregion UnitSystem


        #region Reshape

        /// <summary>
        /// Calculate the factor from the input unit to one meter per second.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        private static double Factor(VelocityUnit unit)
        {
            switch (unit)
            {
                case VelocityUnit.MilliMeterPerHour:
                    return 2.8e-7;               // http://convert-to.com/conversion/speed/convert-mm-per-hour-to-m-per-second.html
                case VelocityUnit.MilliMeterPerMinute:
                    return 1.66667e-5;
                case VelocityUnit.CentiMeterPerMinute:
                    return 1.66667e-4;
                case VelocityUnit.MilliMeterPerSecond:
                    return 1e-3;
                case VelocityUnit.CentiMeterPerSecond:
                    return 1e-2;
                case VelocityUnit.MeterPerMinute:
                    return 1.66667e-2;
                case VelocityUnit.KiloMeterPerHour:
                    return 0.277778;
                case VelocityUnit.MeterPerSecond:
                    return 1;

                case VelocityUnit.InchPerHour:
                    return 7.1e-6;               // http://convert-to.com/conversion/speed/convert-in-per-hour-to-m-per-second.html
                case VelocityUnit.FootPerHour:
                    return 8.46667e-5;
                case VelocityUnit.FootPerMinute:
                    return 5.08e-3;
                case VelocityUnit.InchPerSecond:
                    return 2.54e-2;
                case VelocityUnit.FootPerSecond:
                    return 0.3048;
                case VelocityUnit.MilePerHour:
                    return 0.44704;
                case VelocityUnit.MilePerMinute:
                    return 26.8224;

                case VelocityUnit.SpeedOfSound:
                    return 3.432e2;
                case VelocityUnit.SpeedOfLight:
                    return 2.99792e8;

                case VelocityUnit.Knot:
                    return 0.514444;
                default:
                    throw new NotImplementedException();
            }
        }

        public double As(VelocityUnit unit)
        {
            if (this.unit == unit)
                return internalValue;
            else
            {
                double f = Factor(this.unit) / Factor(unit);
                return internalValue * f;
            }
        }

        public VelocityUnit Unit
        {
            get
            {
                return unit;
            }
            set
            {
                if (unit != value)
                {
                    double f = Factor(unit) / Factor(value);
                    internalValue *= f;
                    unit = value;
                }
            }
        }

        /// <summary>
        /// Normalize by changing the unit.
        /// This method will only change to a unit in the same system and only to commonly used units.
        /// </summary>
        public void Normalize()
        {
            VelocityUnit min, max;
            switch (System)
            {
                case VelocitySystem.Metric:
                    min = UNIT_IDX_METRIC_MIN;
                    max = UNIT_IDX_METRIC_MAX_STD;
                    break;
                case VelocitySystem.Imperial:
                    min = UNIT_IDX_IMPERIAL_MIN;
                    max = UNIT_IDX_IMPERIAL_MAX_STD;
                    break;
                case VelocitySystem.Astronomical:
                    min = UNIT_IDX_ASTRONIMICAL_MIN;
                    max = UNIT_IDX_ASTRONIMICAL_MAX_STD;
                    return;
                default:
                    return;
            }
            if (internalValue != 0)
            {
                double log = Math.Log10(internalValue);
                double f = Factor(unit);
                for (VelocityUnit u = max; u >= min; u--)
                {
                    double log_new = Math.Log10(f / Factor(u));
                    if (log + log_new >= 0)
                    {
                        Unit = u;
                        break;
                    }
                }
            }
            else if (IsMetric)
                unit = SI;
        }

        #endregion Reshape


        #region Operaters

        public static Velocity operator +(Velocity lhs, Velocity rhs)
        {
            return new Velocity(lhs.internalValue + rhs.As(lhs.unit), lhs.unit);
        }

        public static Velocity operator -(Velocity lhs, Velocity rhs)
        {
            return new Velocity(lhs.internalValue - rhs.As(lhs.unit), lhs.unit);
        }

        public static Velocity operator *(Velocity lhs, double rhs)
        {
            return new Velocity(lhs.internalValue * rhs, lhs.unit);
        }

        public static Velocity operator *(double lhs, Velocity rhs)
        {
            return rhs * lhs;
        }

        public static Velocity operator /(Velocity lhs, double rhs)
        {
            return new Velocity(lhs.internalValue / rhs, lhs.unit);
        }

        public static bool operator ==(Velocity lhs, Velocity rhs)
        {
            return lhs.internalValue == rhs.As(lhs.Unit);
        }

        public static bool operator !=(Velocity lhs, Velocity rhs)
        {
            return lhs.internalValue != rhs.As(lhs.Unit);
        }

        public static Length operator *(Velocity lhs, TimeSpan rhs)
        {
            switch (lhs.System)
            {
                case VelocitySystem.Metric:
                    if (lhs.unit == VelocityUnit.KiloMeterPerHour)
                        return new Length(lhs.As(VelocityUnit.KiloMeterPerHour) / rhs.TotalHours, LengthUnit.KiloMeter);
                    else
                        return new Length(lhs.As(VelocityUnit.MeterPerSecond) / rhs.TotalSeconds, LengthUnit.Meter);
                case VelocitySystem.Imperial:
                    return new Length(lhs.As(VelocityUnit.MilePerHour) / rhs.TotalHours, LengthUnit.Mile);
                case VelocitySystem.Nautical:
                    return new Length(lhs.As(VelocityUnit.Knot) / rhs.TotalHours, LengthUnit.NauticalMile);
                default:
                    throw new NotImplementedException();
            }
        }

        public static Length operator *(TimeSpan lhs, Velocity rhs)
        {
            return rhs * lhs;
        }

        #endregion Operaters


        #region Object overrides

        public override bool Equals(object other)
        {
            return other is Velocity && this == (Velocity)other;
        }

        public override int GetHashCode()
        {
            return internalValue.GetHashCode() ^ Unit.GetHashCode();
        }

        #endregion Object overrides


        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj != null && obj is Velocity)
            {
                double o = ((Velocity)obj).As(unit);
                return internalValue.CompareTo(o);
            }
            else
                throw new ArgumentException("Object of Velocity expected!");
        }

        #endregion IComparable Members


        #region ICloneable Members

        public object Clone()
        {
            return new Velocity(this);
        }

        #endregion
    }
}