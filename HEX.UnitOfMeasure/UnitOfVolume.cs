using System;

namespace HEX.UnitOfMeasure
{
    /// <summary>
    /// Enumeration of units of measure for area.
    /// </summary>
    public enum VolumeUnit
    {
        // Metric solid common
        CubicFemtoMeter, CubicPicoMeter, CubicNanoMeter, CubicMicroMeter, CubicMilliMeter, CubicCentiMeter, CubicMeter, CubicKiloMeter,
        // Metric solid rare
        CubicDeciMeter, CubicDecaMeter, CubicHectoMeter,

        // Metric liquid commin
        MicroLiter, MilliLiter, CentiLiter, DeciLiter, Liter, HectoLiter,
        // Metric liquid rare
        DecaLiter, KiloLiter, MegaLiter, GigaLiter,

        // Imperial solid common
        CubicMicroInch, CubicInch, CubicFoot, CubicYard,

        // Imperial liquid common
        FluidOunce, Pint, Quant, Gallon,

        // Special
        Barrel
    }

    public enum VolumeSystem
    {
        MetricSolid,
        MetricLiquid,
        ImperialSolid,
        ImperialLiquid,
        Special
    }

    /// <summary>
    /// Class for holding measurement of volume.
    /// </summary>
    public class Volume : IUnitOfMeasure
    {
        #region Constants

        private static readonly string[] unitTextShort = {
            "fm³", "pm³", "nm³", "µm³", "mm³", "cm³", "m³", "km³",
            "dm³", "da³", "ha³",
            "µl", "ml", "cl", "dl", "l", "hl",
            "dal", "kl", "Ml", "Gl",
            "µin³", "in³", "ft³", "yd³",
            "fl.o", "pt", "qt", "gal"
        };
        private static readonly string[] unitText = {
            "cubic femtometers", "cubic picometers", "cubic nanometers", "cubic micrometers", "cubic millimeters", "cubic centimeters", "cubic meters", "cubic kilometers",
            "cubic decimeters", "cubic decameters", "cubic megameters",
            "microliters", "milliliters", "centiliters", "deciliters", "liters", "hectoliters",
            "decaliters", "kiloliters", "megaliters", "gigaliters",
            "cubic microinches", "cubic inches", "cubic foot", "cubic yards",
            "fluid ounces", "pints", "quants", "gallons",
            "barrels"
        };

        const VolumeUnit UNIT_IDX_METRIC_SOLID_MIN = VolumeUnit.CubicFemtoMeter;
        const VolumeUnit UNIT_IDX_METRIC_SOLID_MAX_STD = VolumeUnit.CubicKiloMeter;
        const VolumeUnit UNIT_IDX_METRIC_SOLID_MAX = VolumeUnit.CubicHectoMeter;

        const VolumeUnit UNIT_IDX_METRIC_LIQUID_MIN = VolumeUnit.MicroLiter;
        const VolumeUnit UNIT_IDX_METRIC_LIQUID_MAX_STD = VolumeUnit.HectoLiter;
        const VolumeUnit UNIT_IDX_METRIC_LIQUID_MAX = VolumeUnit.GigaLiter;

        const VolumeUnit UNIT_IDX_IMPERIAL_SOLID_MIN = VolumeUnit.CubicMicroInch;
        const VolumeUnit UNIT_IDX_IMPERIAL_SOLID_MAX_STD = VolumeUnit.CubicYard;
        const VolumeUnit UNIT_IDX_IMPERIAL_SOLID_MAX = VolumeUnit.CubicYard;

        const VolumeUnit UNIT_IDX_IMPERIAL_LIQUID_MIN = VolumeUnit.FluidOunce;
        const VolumeUnit UNIT_IDX_IMPERIAL_LIQUID_MAX_STD = VolumeUnit.Gallon;
        const VolumeUnit UNIT_IDX_IMPERIAL_LIQUID_MAX = VolumeUnit.Gallon;

        public const VolumeUnit SI = VolumeUnit.CubicMeter;

        #endregion Constants


        VolumeUnit unit = VolumeUnit.CubicMeter;
        double internalValue = 0;


        #region Constructors

        public Volume()
        { }

        public Volume(double value)
        {
            internalValue = value;
        }

        public Volume(double value, VolumeUnit unit)
        {
            this.unit = unit;
            internalValue = value;
        }

        public Volume(Volume other)
        {
            unit = other.unit;
            internalValue = other.internalValue;
        }

        #endregion Constructors


        #region Text

        public string UnitToString()
        {
            throw new NotImplementedException();
            //return unitText[(int)unit];
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
        /// Generate Volume from string.
        /// </summary>
        /// <param name="s">String composed of number and unit.</param>
        /// <returns>Volume.</returns>
        public static Volume Parse(string s)
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
                    return new Volume(number, (VolumeUnit)idx);
                else
                    throw new FormatException("Unknown unit (" + unit + ")");
            }
            else
                throw new ArgumentNullException();
        }

        /// <summary>
        /// Generate Volume from string.
        /// </summary>
        /// <param name="s">String composed of number and unit.</param>
        /// <param name="result">Volume.</param>
        /// <returns>True if string parsed succesfully.</returns>
        public static bool TryParse(string s, out Volume result)
        {
            try
            {
                result = Parse(s);
                return true;
            }
            catch (Exception)
            {
                result = new Volume();
                return false;
            }
        }

        #endregion Text


        #region UnitSystem

        public VolumeSystem System
        {
            get { return GetSystem(unit); }
        }

        private static VolumeSystem GetSystem(VolumeUnit unit)
        {
            if (UNIT_IDX_METRIC_SOLID_MIN <= unit && unit <= UNIT_IDX_METRIC_SOLID_MAX)
                return VolumeSystem.MetricSolid;
            if (UNIT_IDX_METRIC_LIQUID_MIN <= unit && unit <= UNIT_IDX_METRIC_LIQUID_MAX)
                return VolumeSystem.MetricLiquid;
            if (UNIT_IDX_IMPERIAL_SOLID_MIN <= unit && unit <= UNIT_IDX_IMPERIAL_SOLID_MAX)
                return VolumeSystem.ImperialSolid;
            if (UNIT_IDX_IMPERIAL_LIQUID_MIN <= unit && unit <= UNIT_IDX_IMPERIAL_LIQUID_MAX)
                return VolumeSystem.ImperialLiquid;
            else
                return VolumeSystem.Special;
        }

        public bool IsMetric
        {
            get
            {
                VolumeSystem s = GetSystem(unit);
                return s == VolumeSystem.MetricSolid || s == VolumeSystem.MetricLiquid;
            }
        }

        public bool IsMetricSolid
        {
            get { return GetSystem(unit) == VolumeSystem.ImperialSolid; }
        }

        public bool IsMetricLiquid
        {
            get { return GetSystem(unit) == VolumeSystem.MetricLiquid; }
        }

        public bool IsImperial
        {
            get
            {
                VolumeSystem s = GetSystem(unit);
                return s == VolumeSystem.ImperialSolid || s == VolumeSystem.ImperialLiquid;
            }
        }

        public bool IsImperialSolid
        {
            get { return GetSystem(unit) == VolumeSystem.ImperialSolid; }
        }

        public bool IsImperialLiquid
        {
            get { return GetSystem(unit) == VolumeSystem.ImperialLiquid; }
        }

        #endregion UnitSystem


        #region Reshape

        /// <summary>
        /// Calculate the factor from one input unit to one cubic meter.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        private static double Factor(VolumeUnit unit)
        {
            switch (unit)
            {
                case VolumeUnit.CubicFemtoMeter:
                    return 1e-45;
                case VolumeUnit.CubicPicoMeter:
                    return 1e-36;
                case VolumeUnit.CubicNanoMeter:
                    return 1e-27;
                case VolumeUnit.CubicMicroMeter:
                    return 1e-18;
                case VolumeUnit.CubicMilliMeter:
                    return 1e-9;
                case VolumeUnit.CubicCentiMeter:
                    return 1e-6;
                case VolumeUnit.CubicDeciMeter:
                    return 1e-3;
                case VolumeUnit.CubicMeter:
                    return 1;
                case VolumeUnit.CubicDecaMeter:
                    return 1e3;
                case VolumeUnit.CubicHectoMeter:
                    return 1e6;
                case VolumeUnit.CubicKiloMeter:
                    return 1e9;

                case VolumeUnit.MicroLiter:
                    return 1e-9;
                case VolumeUnit.MilliLiter:
                    return 1e-6;
                case VolumeUnit.CentiLiter:
                    return 1e-5;
                case VolumeUnit.DeciLiter:
                    return 1e-4;
                case VolumeUnit.Liter:
                    return 1e-3;
                case VolumeUnit.DecaLiter:
                    return 1e-2;
                case VolumeUnit.HectoLiter:
                    return 1e-1;
                case VolumeUnit.KiloLiter:
                    return 1;
                case VolumeUnit.MegaLiter:
                    return 1e3;
                case VolumeUnit.GigaLiter:
                    return 1e6;

                case VolumeUnit.CubicMicroInch:
                    return 1.6387064e-23;
                case VolumeUnit.CubicInch:
                    return 1.6387064e-5;
                case VolumeUnit.CubicFoot:
                    return 2.8316846592e-2;
                case VolumeUnit.CubicYard:
                    return 7.64554857984e-1;

                case VolumeUnit.FluidOunce:
                    return 2.8413e-5;
                case VolumeUnit.Pint:
                    return 5.68261e-4;
                case VolumeUnit.Quant:
                    return 1.136523e-3;
                case VolumeUnit.Gallon:
                    return 4.54609e-3;

                case VolumeUnit.Barrel:
                    return 1.58987294928e-1;
                default:
                    throw new NotImplementedException();
            }
        }

        public double As(VolumeUnit unit)
        {
            if (this.unit == unit)
                return internalValue;
            else
            {
                double f = Factor(this.unit) / Factor(unit);
                return internalValue * f;
            }
        }

        public VolumeUnit Unit
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
            if (System != VolumeSystem.Special)
            {
                VolumeUnit min, max;
                switch (System)
                {
                    case VolumeSystem.MetricSolid:
                        min = UNIT_IDX_METRIC_SOLID_MIN;
                        max = UNIT_IDX_METRIC_SOLID_MAX_STD;
                        break;
                    case VolumeSystem.MetricLiquid:
                        min = UNIT_IDX_METRIC_LIQUID_MIN;
                        max = UNIT_IDX_METRIC_LIQUID_MAX_STD;
                        break;
                    case VolumeSystem.ImperialSolid:
                        min = UNIT_IDX_IMPERIAL_SOLID_MIN;
                        max = UNIT_IDX_IMPERIAL_SOLID_MAX_STD;
                        break;
                    case VolumeSystem.ImperialLiquid:
                        min = UNIT_IDX_IMPERIAL_LIQUID_MIN;
                        max = UNIT_IDX_IMPERIAL_LIQUID_MAX_STD;
                        break;
                    default:
                        return;
                }
                if (internalValue != 0)
                {
                    double log = Math.Log10(internalValue);
                    double f = Factor(unit);
                    for (VolumeUnit u = max; u >= min; u--)
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
        }

        #endregion Reshape


        #region Operaters

        public static Volume operator +(Volume ls, Volume rs)
        {
            return new Volume(ls.internalValue + rs.As(ls.unit), ls.unit);
        }

        public static Volume operator -(Volume ls, Volume rs)
        {
            return new Volume(ls.internalValue - rs.As(ls.unit), ls.unit);
        }

        public static Volume operator *(Volume ls, double rs)
        {
            return new Volume(ls.internalValue * rs, ls.unit);
        }

        public static Volume operator *(double ls, Volume rs)
        {
            return rs * ls;
        }

        public static Volume operator /(Volume ls, double rs)
        {
            return new Volume(ls.internalValue / rs, ls.unit);
        }

        public static Area operator /(Volume ls, Length rs)
        {
            VolumeSystem s = ls.System;
            if (s == VolumeSystem.MetricSolid || s == VolumeSystem.ImperialSolid)
            {
                Area res;
                switch (s)
                {
                    case VolumeSystem.MetricSolid:
                        res = new Area(ls.As(VolumeUnit.CubicMeter) / rs.As(LengthUnit.Meter), AreaUnit.SqrMeter);
                        break;
                    case VolumeSystem.ImperialSolid:
                        res = new Area(ls.As(VolumeUnit.CubicFoot) / rs.As(LengthUnit.Foot), AreaUnit.SqrFoot);
                        break;
                    default:
                        throw new NotImplementedException();
                }
                return res;
            }
            else
                throw new InvalidOperationException();
        }

        public static bool operator ==(Volume lhs, Volume rhs)
        {
            return lhs.internalValue == rhs.As(lhs.Unit);
        }

        public static bool operator !=(Volume lhs, Volume rhs)
        {
            return lhs.internalValue != rhs.As(lhs.Unit);
        }

        #endregion Operaters


        #region Object overrides

        public override bool Equals(object other)
        {
            return other is Volume && this == (Volume)other;
        }

        public override int GetHashCode()
        {
            return internalValue.GetHashCode() ^ Unit.GetHashCode();
        }

        #endregion Object overrides


        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj != null && obj is Volume)
            {
                double o = ((Volume)obj).As(unit);
                return internalValue.CompareTo(o);
            }
            else
                throw new ArgumentException("Object of Volume expected!");
        }

        #endregion IComparable Members


        #region ICloneable Members

        public object Clone()
        {
            return new Volume(this);
        }

        #endregion
    }
}