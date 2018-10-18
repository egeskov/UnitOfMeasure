using System;

namespace HEX.UnitOfMeasure
{
    /// <summary>
    /// Enumeration of units of measure for volume.
    /// </summary>
    public enum AreaUnit
    {
        // Metric common
        SqrFemtoMeter, SqrPicoMeter, SqrNanoMeter, SqrMicroMeter, SqrMilliMeter, SqrCentiMeter, SqrMeter, Hectare, SqrKiloMeter,
        // Metric rare
        SqrDeciMeter, SqrDecaMeter, SqrMegaMeter,

        // Imperial common
        SqrMicroInch, SqrInch, SqrFoot, SqrYard, Acre, SqrMile,
    }

    public enum AreaSystem
    {
        Metric,
        Imperial
    }

    /// <summary>
    /// Class for holding measurement of area.
    /// </summary>
    public class Area : IUnitOfMeasure
    {
        #region Constants

        private static readonly string[] unitTextShort = {
            "fm²", "pm²", "nm²", "µm²", "mm²", "cm²", "m²", "ha", "km²",
            "dm²", "da²", "hm²", "Mm²",
            "µin²", "in²", "ft²", "yd²", "ac", "mi²"
        };
        private static readonly string[] unitText = {
            "square femtometers", "square picometers", "square nanometers", "square micrometers", "square millimeters", "square centimeters", "square meters", "hectares", "square kilometers",
            "square decimeters", "square decameters", "square megameters",
            "square microinches", "square inches", "square foot", "square yards", "acres", "square miles"
        };

        const AreaUnit UNIT_IDX_METRIC_MIN = AreaUnit.SqrFemtoMeter;
        const AreaUnit UNIT_IDX_METRIC_MAX_STD = AreaUnit.SqrKiloMeter;
        const AreaUnit UNIT_IDX_METRIC_MAX = AreaUnit.SqrMegaMeter;

        const AreaUnit UNIT_IDX_IMPERIAL_MIN = AreaUnit.SqrMicroInch;
        const AreaUnit UNIT_IDX_IMPERIAL_MAX_STD = AreaUnit.SqrMile;
        const AreaUnit UNIT_IDX_IMPERIAL_MAX = AreaUnit.SqrMile;

        public static AreaUnit SI = AreaUnit.SqrMeter;

        #endregion Constants


        AreaUnit unit = AreaUnit.SqrMeter;
        double internalValue = 0;


        #region Constructors

        public Area()
        { }

        public Area(double value)
        {
            internalValue = value;
        }

        public Area(double value, AreaUnit unit)
        {
            this.unit = unit;
            internalValue = value;
        }

        public Area(Area other)
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
        /// Generate Area from string.
        /// </summary>
        /// <param name="s">String composed of number and unit.</param>
        /// <returns>Area.</returns>
        public static Area Parse(string s)
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
                    return new Area(number, (AreaUnit)idx);
                else
                    throw new FormatException("Unknown unit (" + unit + ")");
            }
            else
                throw new ArgumentNullException();
        }

        /// <summary>
        /// Generate Area from string.
        /// </summary>
        /// <param name="s">String composed of number and unit.</param>
        /// <param name="result">Area.</param>
        /// <returns>True if string parsed succesfully.</returns>
        public static bool TryParse(string s, out Area result)
        {
            try
            {
                result = Parse(s);
                return true;
            }
            catch (Exception)
            {
                result = new Area();
                return false;
            }
        }

        #endregion Text


        #region UnitSystem

        public AreaSystem System
        {
            get { return GetSystem(unit); }
        }

        private static AreaSystem GetSystem(AreaUnit unit)
        {
            if (UNIT_IDX_METRIC_MIN <= unit && unit <= UNIT_IDX_METRIC_MAX)
                return AreaSystem.Metric;
            else //if (UNIT_IDX_IMPERIAL_MIN <= unit && unit <= UNIT_IDX_IMPERIAL_MAX)
                return AreaSystem.Imperial;
        }

        public bool IsMetric
        {
            get { return GetSystem(unit) == AreaSystem.Metric; }
        }

        public bool IsImperial
        {
            get { return GetSystem(unit) == AreaSystem.Imperial; }
        }

        #endregion UnitSystem


        #region Reshape

        /// <summary>
        /// Calculate the factor from one input unit to one square meter.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        private static double Factor(AreaUnit unit)
        {
            switch (unit)
            {
                case AreaUnit.SqrFemtoMeter:
                    return 1e-30;
                case AreaUnit.SqrPicoMeter:
                    return 1e-24;
                case AreaUnit.SqrNanoMeter:
                    return 1e-18;
                case AreaUnit.SqrMicroMeter:
                    return 1e-12;
                case AreaUnit.SqrMilliMeter:
                    return 1e-6;
                case AreaUnit.SqrCentiMeter:
                    return 1e-4;
                case AreaUnit.SqrDeciMeter:
                    return 1e-2;
                case AreaUnit.SqrMeter:
                    return 1;
                case AreaUnit.SqrDecaMeter:
                    return 1e2;
                case AreaUnit.Hectare:
                    return 1e4;
                case AreaUnit.SqrKiloMeter:
                    return 1e6;
                case AreaUnit.SqrMegaMeter:
                    return 1e12;

                case AreaUnit.SqrMicroInch:
                    return 6.4516e-8;
                case AreaUnit.SqrInch:
                    return 6.4516e-4;
                case AreaUnit.SqrFoot:
                    return 9.2903e-2;
                case AreaUnit.SqrYard:
                    return 0.836127;
                case AreaUnit.Acre:
                    return 4.0468564224e3;
                case AreaUnit.SqrMile:
                    return 2.589988110336e6;
                default:
                    throw new NotImplementedException();
            }
        }

        public double As(AreaUnit unit)
        {
            if (this.unit == unit)
                return internalValue;
            else
            {
                double f = Factor(this.unit) / Factor(unit);
                return internalValue * f;
            }
        }

        public AreaUnit Unit
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
        /// Example:    1034 mm  ->  1.034 m
        ///             34.5 in  ->  2.875 ft
        /// </summary>
        public void Normalize()
        {
            AreaUnit min, max;
            switch (System)
            {
                case AreaSystem.Metric:
                    min = UNIT_IDX_METRIC_MIN;
                    max = UNIT_IDX_METRIC_MAX_STD;
                    break;
                case AreaSystem.Imperial:
                    min = UNIT_IDX_IMPERIAL_MIN;
                    max = UNIT_IDX_IMPERIAL_MAX_STD;
                    break;
                default:
                    return;
            }
            if (internalValue != 0)
            {
                double log = Math.Log10(internalValue);
                double f = Factor(unit);
                for (AreaUnit u = max; u >= min; u--)
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

        public static Area operator +(Area ls, Area rs)
        {
            return new Area(ls.internalValue + rs.As(ls.unit), ls.unit);
        }

        public static Area operator -(Area ls, Area rs)
        {
            return new Area(ls.internalValue - rs.As(ls.unit), ls.unit);
        }

        public static Area operator *(Area ls, double rs)
        {
            return new Area(ls.internalValue * rs, ls.unit);
        }

        public static Area operator *(double ls, Area rs)
        {
            return rs * ls;
        }

        public static Area operator /(Area ls, double rs)
        {
            return new Area(ls.internalValue / rs, ls.unit);
        }

        public static Length operator /(Area ls, Length rs)
        {
            Length res;
            switch (ls.System)
            {
                case AreaSystem.Metric:
                    res = new Length(ls.As(Area.SI) / rs.As(Length.SI), Length.SI);
                    break;
                case AreaSystem.Imperial:
                    res = new Length(ls.As(AreaUnit.SqrFoot) / rs.As(LengthUnit.Foot), LengthUnit.Foot);
                    break;
                default:
                    throw new InvalidOperationException("Missing implementation for AreaSystem." + ls.System);
            }
            return res;
        }

        public static Volume operator *(Area ls, Length rs)
        {
            Volume res;
            switch (ls.System)
            {
                case AreaSystem.Metric:
                    res = new Volume(ls.As(Area.SI) * rs.As(Length.SI), Volume.SI);
                    break;
                case AreaSystem.Imperial:
                    res = new Volume(ls.As(AreaUnit.SqrFoot) * rs.As(LengthUnit.Foot), VolumeUnit.CubicFoot);
                    break;
                default:
                    throw new InvalidOperationException("Missing implementation of AreaSystem." + ls.System);
            }
            return res;
        }

        public static Volume operator *(Length ls, Area rs)
        {
            // Call overload operator wirh swapped parameters.
            return rs * ls;
        }

        #endregion Operaters


        #region Object overrides

        public override bool Equals(object other)
        {
            return other is Area && this == (Area)other;
        }

        public override int GetHashCode()
        {
            return internalValue.GetHashCode() ^ Unit.GetHashCode();
        }

        #endregion Object overrides


        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj != null && obj is Area)
            {
                double o = ((Area)obj).As(unit);
                return internalValue.CompareTo(o);
            }
            else
                throw new ArgumentException("Object of Area expected!");
        }

        #endregion IComparable Members


        #region ICloneable Members

        public object Clone()
        {
            return new Area(this);
        }

        #endregion
    }
}