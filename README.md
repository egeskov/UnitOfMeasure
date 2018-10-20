# UnitOfMeasure
Class library for keeping track of values and their units. Makes calculations and displaying measurement data much easier.

## Known units
The following four domains are implemented: Length, Area, Volume and Velocity.

### Length
- Metric:
  - Femtometers, Picometers, Nanometers, Micrometers, Millimeters, Centimeters, Meters (SI), Kilometers, Decimeters, Decameters, Hectometers, Megameters.
- Imperial:
  - Inches, Foot, Yards, Miles, Microinches, Fathom.
- Astronomical:
  - Light-seconds, Light-minutes, Light-hours, Light-days, Light-weeks, Light-years.
- Nautical:
  - Nautical miles.

### Area
- Metric:
  - Square femtometers, Square picometers, Square nanometers, Square micrometers, Square millimeters, Square centimeters, Square meters (SI), Hectares, Square kilometers, Square decimeters, Square decameters, Square megameters.
- Imperial:
  - Square microinches, Square inches, Square foot, Square yards, Acres, Square miles.

### Volume
- Metric (Solid):
  - Cubic femtometers, Cubic picometers, Cubic nanometers, Cubic micrometers, Cubic millimeters, Cubic centimeters, Cubic meters (SI), Cubic kilometers, Cubic decimeters, Cubic decameters, Cubic megameters.
- Metric (Liquid):
  - Microliters, Milliliters, Centiliters, Deciliters, Liters, Hectoliters, Decaliters, Kiloliters, Megaliters, Gigaliters.
- Imperial (Solid):
  - Cubic microinches, Cubic inches, Cubic foot, Cubic yards.
- Imperial (Liquid):
  - Fluid ounces, Pints, Quants, Gallons.
- Special:
  - Barrels

### Velocity
- Metric:
  - Meters per second (SI), Kilometers per hour, Millimeters per hour, Millimeters per minute, Milimeters per second, Centimeters per minute, Centimeters per second, Meters per minute.
- Imperial:
  - Miles per hour, Inches per second, Inches per hour, Foots per hour, Foots per minute, Foots per second, Miles per minute.
- Astronomical:
  - Times the speed of sound, Times the speed of light.
- Nautical:
  - Knots.

## Known Shortcomings
- Missing ability format output to eg. two decimals.
- Missing ability to parse input from string.

## Examples
1) Adding meters to inches and displaying result:

```C#
Length lhs = new Length(2.1, LengthUnit.Meter);
Length rhs = new Length(43.4, LengthUnit.Inch);

Length result = lhs + rhs;
Console.WriteLine($"Adding {lhs} to {rhs} equals {result}.");

// Output: "Adding 2,1 m to 43,4 in equals 3,20236 m."
```

2) The unit of the left hand side operand is used for the result. 
You can change the unit just by setting the Unit property:

```C#
result.Unit = LengthUnit.Foot;
Console.WriteLine($"Result in specific unit: {result}.");

// Output: "Result in specific unit: 10,5064304461942 ft."
```

3) Calculating using units across different domains - Length, Area and Volume:
```C#
var l = new Length(1.45, LengthUnit.Meter);
var a = l * l;
Console.WriteLine($"{l} times {l} equals {a}.");

// Output: "1,45 m times 1,45 m equals 2,1025 m²."

var v = l * a;
Console.WriteLine($"{l} times {a} equals {v}.");

// Output: "1,45 m times 2,1025 m² equals 3,048625 m³."
```

4) Calculating using units across different domains - Length, Time and Velocity:
```C#
var distance = new Length(15.3, LengthUnit.KiloMeter);
var time = new TimeSpan(0, 13, 25);
var averageSpeed = distance / time;
Console.WriteLine($"Travelling {distance} with an average speed of {averageSpeed} takes 13 minutes and 25 seconds.");

// Output: "Travelling 15,3 km with an average speed of 68,4223602484472 kmh takes 13 minutes and 25 seconds."
```