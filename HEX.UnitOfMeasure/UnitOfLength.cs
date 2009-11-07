using System;

namespace HEX.UnitOfMeasure
{
    /// <summary>
    /// Enumeration of units of measure for length or distance.
    /// </summary>
    public enum LengthUnit
    {
        // Metric common
        FemtoMeter, PicoMeter, NanoMeter, MicroMeter, MilliMeter, CentiMeter, Meter, KiloMeter,
        // Metric rare
        DeciMeter, DecaMeter, HectoMeter, MegaMeter,

        // Imperial common
        Inch, Foot, Yard, Mile,
        // Imperial rare
        MicroInch, 

        // Astronimical
        LightSecond, LightMinute, LightHour, LightDay, LightWeek, LightYear,

        // Nautical
        NauticalMile
    }

    public enum LengthSystem
    {
        Metric,
        Imperial,
        Astronomical,
        Nautical
    }

    /// <summary>
    /// Class for holding measurement of length or distance. 
    /// By binding a unit to the number, convertion error and misscomparing of numbers can be avoided.
    /// </summary>
    public class Length : IUnitOfMeasure
    {
        #region Constants

        static string[] unitTextShort = {"fm", "pm", "nm", "µm", "mm", "cm", "m", "km",
                                            "dm", "da", "hm", "Mm",
                                            "in", "ft", "yd", "mi",
                                            "µin",                                            
                                            "lhs", "lm", "lh", "ld", "lw", "ly",
                                            "Nm",};
        static string[] unitText = {"femtometers", "picometers", "nanometers", "micrometers", "millimeters", "centimeters", "meters", "kilometers",
                                       "decimeters", "decameters", "hectometers", "megameters",
                                       "inches", "foot", "yards", "miles", 
                                       "microinches",  
                                       "light-seconds", "light-minutes", "light-hours", "light-days", "light-weeks", "light-years",
                                       "nautical miles"};

        const LengthUnit UNIT_IDX_METRIC_MIN = LengthUnit.FemtoMeter;
        const LengthUnit UNIT_IDX_METRIC_MAX_STD = LengthUnit.KiloMeter;
        const LengthUnit UNIT_IDX_METRIC_MAX = LengthUnit.MegaMeter;

        const LengthUnit UNIT_IDX_IMPERIAL_MIN = LengthUnit.Inch;
        const LengthUnit UNIT_IDX_IMPERIAL_MAX_STD = LengthUnit.Mile;
        const LengthUnit UNIT_IDX_IMPERIAL_MAX = LengthUnit.MicroInch;

        const LengthUnit UNIT_IDX_ASTRONIMICAL_MIN = LengthUnit.LightSecond;
        const LengthUnit UNIT_IDX_ASTRONIMICAL_MAX_STD = LengthUnit.LightYear;
        const LengthUnit UNIT_IDX_ASTRONIMICAL_MAX = LengthUnit.LightYear;

        public const LengthUnit SI = LengthUnit.Meter;

        #endregion Constants


        LengthUnit unit = LengthUnit.Meter;
        double internalValue = 0;


        #region Constructors

        public Length()
        { }

        public Length(double value)
        {
            this.internalValue = value;
        }

        public Length(double value, LengthUnit unit)
        {
            this.unit = unit;
            this.internalValue = value;
        }

        public Length(Length other)
        {
            this.unit = other.unit;
            this.internalValue = other.internalValue;
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
            return string.Format("{0} {1}", this.internalValue, this.UnitToStringShort());
        }

        public string ToString(string format)
        {
            format = format.ToLower();
            if (format.Contains("l"))
                return this.internalValue.ToString(format.Replace("l", "")) + " " + this.UnitToString();
            else
                return this.internalValue.ToString(format) + " " + this.UnitToStringShort();
        }

        /// <summary>
        /// Generate Length from string.
        /// </summary>
        /// <param name="s">String composed of number and unit.</param>
        /// <returns>Length.</returns>
        public static Length Parse(string s)
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
                    return new Length(number, (LengthUnit)idx);
                else
                    throw new FormatException("Unknown unit (" + unit + ")");
            }
            else
                throw new ArgumentNullException();
        }

        /// <summary>
        /// Generate Length from string.
        /// </summary>
        /// <param name="s">String composed of number and unit.</param>
        /// <param name="result">Length.</param>
        /// <returns>True if string parsed succesfully.</returns>
        public static bool TryParse(string s, out Length result)
        {
            try
            {
                result = Parse(s);
                return true;
            }
            catch(Exception)
            {
                result = new Length();
                return false;
            }
        }

        #endregion Text


        #region UnitSystem

        public LengthSystem System
        {
            get { return getSystem(this.unit); }
        }

        static LengthSystem getSystem(LengthUnit unit)
        {
            if (UNIT_IDX_METRIC_MIN <= unit && unit <= UNIT_IDX_METRIC_MAX)
                return LengthSystem.Metric;
            else if (UNIT_IDX_IMPERIAL_MIN <= unit && unit <= UNIT_IDX_IMPERIAL_MAX)
                return LengthSystem.Imperial;
            else if(UNIT_IDX_ASTRONIMICAL_MIN <= unit && unit <= UNIT_IDX_ASTRONIMICAL_MAX)
                return LengthSystem.Astronomical;
            else
                return LengthSystem.Nautical;
        }

        public bool IsMetric
        {
            get { return getSystem(unit) == LengthSystem.Metric; }
        }

        public bool IsImperial
        {
            get { return getSystem(unit) == LengthSystem.Imperial; }
        }

        public bool IsAstronimical
        {
            get { return getSystem(unit) == LengthSystem.Astronomical; }
        }

        public bool IsNautical
        {
            get { return getSystem(unit) == LengthSystem.Nautical;}
        }

        #endregion UnitSystem


        #region Reshape

        /// <summary>
        /// Calculate the factor from the input unit to one meter.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        static double factor(LengthUnit unit)
        {
            switch (unit)
            {
                case LengthUnit.FemtoMeter: return 1e-15;
                case LengthUnit.PicoMeter: return 1e-12;
                case LengthUnit.NanoMeter: return 1e-9;
                case LengthUnit.MicroMeter: return 1e-6;
                case LengthUnit.MilliMeter: return 1e-3;
                case LengthUnit.CentiMeter: return 1e-2;
                case LengthUnit.DeciMeter: return 1e-1;
                case LengthUnit.Meter: return 1;
                case LengthUnit.DecaMeter: return 1e1;
                case LengthUnit.HectoMeter: return 1e2;
                case LengthUnit.KiloMeter: return 1e3;
                case LengthUnit.MegaMeter: return 1e6;

                case LengthUnit.MicroInch: return 2.54e-8;
                case LengthUnit.Inch: return 2.54e-2;
                case LengthUnit.Foot: return 3.048e-1;
                case LengthUnit.Yard: return 9.144e-1;
                case LengthUnit.Mile: return 1.609344e3;
                case LengthUnit.NauticalMile: return 1.852e3;

                case LengthUnit.LightSecond: return 2.99792458e8;
                case LengthUnit.LightMinute: return 1.798754748e10;
                case LengthUnit.LightHour: return 1.0792528488e12;
                case LengthUnit.LightDay: return 2.59020684e13;
                case LengthUnit.LightWeek: return 1.81314478598e14;
                case LengthUnit.LightYear: return 9.460730472580e15;
                default:
                    throw new NotImplementedException();
            }
        }

        public double As(LengthUnit unit)
        {
            if (this.unit == unit)
                return this.internalValue;
            else
            {
                double f = factor(this.unit) / factor(unit);
                return this.internalValue * f;
            }
        }

        public LengthUnit Unit
        {
            get
            {
                return this.unit;
            }
            set
            {
                if (this.unit != value)
                {
                    double f = factor(this.unit) / factor(value);
                    this.internalValue *= f;
                    this.unit = value;
                }
            }
        }

        /// <summary>
        /// Normalize by changing the unit.
        /// This method will only change to a unit in the same system and only to commonly used units.
        /// Example:    1034 mm  ->  1.034 m
        ///             34.5 in  ->  2.875 ft
        /// </summary>
        public void Normalize()
        {
            LengthUnit min, max;
            switch (System)
            {
                case LengthSystem.Metric:
                    min = UNIT_IDX_METRIC_MIN;
                    max = UNIT_IDX_METRIC_MAX_STD;
                    break;
                case LengthSystem.Imperial:
                    min = UNIT_IDX_IMPERIAL_MIN;
                    max = UNIT_IDX_IMPERIAL_MAX_STD;
                    break;
                case LengthSystem.Astronomical:
                    min = UNIT_IDX_ASTRONIMICAL_MIN;
                    max = UNIT_IDX_ASTRONIMICAL_MAX_STD;
                    break;
                default:
                    return;
            }
            if (this.internalValue != 0)
            {
                double log = Math.Log10(this.internalValue);
                double f = factor(this.unit);
                for (LengthUnit u = max; u >= min; u--)
                {
                    double log_new = Math.Log10(f / factor(u));
                    if (log + log_new >= 0)
                    {
                        this.Unit = u;
                        break;
                    }
                }
            }
            else if (IsMetric)
                this.unit = SI;
        }

        #endregion Reshape


        #region Operaters

        public static Length operator +(Length lhs, Length rhs)
        {
            return new Length(lhs.internalValue + rhs.As(lhs.unit), lhs.unit);
        }

        public static Length operator -(Length lhs, Length rhs)
        {
            return new Length(lhs.internalValue - rhs.As(lhs.unit), lhs.unit);
        }

        public static Length operator *(Length lhs, double rhs)
        {
            return new Length(lhs.internalValue * rhs, lhs.unit);
        }

        public static Length operator *(double lhs, Length rhs)
        {
            return rhs * lhs;
        }

        public static Length operator /(Length lhs, double rhs)
        {
            return new Length(lhs.internalValue / rhs, lhs.unit);
        }

        public static Area operator *(Length lhs, Length rhs)
        {
            Area res;
            switch (lhs.System)
            {
                case LengthSystem.Imperial:
                    res = new Area(lhs.As(LengthUnit.Foot) * rhs.As(LengthUnit.Foot), AreaUnit.SqrFoot);
                    break;
                default:
                    res = new Area(lhs.As(SI) * rhs.As(SI), Area.SI);
                    break;
            }
            return res;
        }

        public static bool operator ==(Length lhs, Length rhs)
        {
            return lhs.internalValue == rhs.As(lhs.Unit);
        }

        public static bool operator !=(Length lhs, Length rhs)
        {
            return lhs.internalValue != rhs.As(lhs.Unit);
        }

        public static Velocity operator /(Length lhs, TimeSpan rhs)
        {
            switch (lhs.System)
            {
                case LengthSystem.Metric:
                    {
                        if (lhs.unit == LengthUnit.KiloMeter)
                            return new Velocity(lhs.internalValue / rhs.TotalHours, VelocityUnit.KiloMeterPerHour);
                        else
                            return new Velocity(lhs.As(LengthUnit.Meter) / rhs.TotalSeconds, VelocityUnit.MeterPerSecond);
                    }
                case LengthSystem.Imperial:
                    return new Velocity(lhs.As(LengthUnit.Mile) / rhs.TotalHours, VelocityUnit.MilePerHour);
                case LengthSystem.Astronomical:
                    return new Velocity(lhs.As(LengthUnit.LightSecond) / rhs.TotalSeconds, VelocityUnit.SpeedOfLight);
                case LengthSystem.Nautical:
                    return new Velocity(lhs.As(LengthUnit.NauticalMile) / rhs.TotalHours, VelocityUnit.Knot);
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion Operaters


        #region Object overrides

        public override bool Equals(object other)
        {
            return (other is Length) && (this == (Length)other);
        }

        public override int GetHashCode()
        {
            return this.internalValue.GetHashCode() ^ this.Unit.GetHashCode();
        }

        #endregion Object overrides


        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj != null && obj is Length)
            {
                double o = ((Length)obj).As(this.unit);
                return this.internalValue.CompareTo(o);
            }
            else
                throw new ArgumentException("Object of Length expected!");
        }

        #endregion IComparable Members


        #region ICloneable Members

        public object Clone()
        {
            return new Length(this);
        }

        #endregion
    }
}
