using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Collections;

namespace PudelkoLib
{
    public sealed class Pudelko : IFormattable, IEquatable<Pudelko>, IEnumerable<double>
    {
        private double a { get; init; }
        private double b { get; init; }
        private double c { get; init; }
        public double A { get { return Math.Round(ToMeter(a,UnitOfMeasure.milimeter), 3, MidpointRounding.AwayFromZero); } }
        public double B { get { return Math.Round(ToMeter(b, UnitOfMeasure.milimeter), 3, MidpointRounding.AwayFromZero); } }
        public double C { get { return Math.Round(ToMeter(c, UnitOfMeasure.milimeter), 3, MidpointRounding.AwayFromZero); } }

        public double ToMilimeter(double value, UnitOfMeasure unitOfMeasure)
        {
            switch (unitOfMeasure)
            {
                case UnitOfMeasure.milimeter: return value;
                case UnitOfMeasure.centimeter: return value * 10;
                case UnitOfMeasure.meter: return value * 1000;
                default: throw new ArgumentException();
            }
        }
        public double ToMeter(double value, UnitOfMeasure unitOfMeasure)
        {
            switch (unitOfMeasure)
            {
                case UnitOfMeasure.milimeter: return value / 1000;
                case UnitOfMeasure.centimeter: return value / 100;
                case UnitOfMeasure.meter: return value;
                default: throw new ArgumentException();
            }
        }

        public Pudelko(double? a = null, double? b = null, double? c = null, UnitOfMeasure unit = UnitOfMeasure.meter)
        {
           
            this.a = Math.Floor((a != null) ? ToMilimeter(a.Value, unit) : 100);
            this.b = Math.Floor((b != null) ? ToMilimeter(b.Value, unit) : 100);
            this.c = Math.Floor((c != null) ? ToMilimeter(c.Value, unit) : 100);
            if (this.a <= 0 || this.b <= 0 || this.c <= 0 || this.a > 10000 || this.b > 10000 || this.c > 10000)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public string ToString(string Unit = "m")
        {
            if (Unit == null) Unit = "m";

            switch (Unit)
            {
                case "m":
                    return string.Format("{1:0.000} {0} × {2:0.000} {0} × {3:0.000} {0}", Unit, a / 1000, b / 1000, c / 1000);
                case "cm":
                    return string.Format("{1:0.0} {0} × {2:0.0} {0} × {3:0.0} {0}", Unit, a / 10, b / 10, c / 10);
                case "mm":
                    return string.Format("{1:0} {0} × {2:0} {0} × {3:0} {0}", Unit, a, b, c);
                default:
                    throw new FormatException();
            }
        }

        public override string ToString() 
            => string.Format("{1:0.000} {0} × {2:0.000} {0} × {3:0.000} {0}","m", A, B, C);
        public string ToString(string format, IFormatProvider formatProvider) 
            => string.Format("{1:0.000} {0} × {2:0.000} {0} × {3:0.000} {0}", "m", A, B, C);

        public double Objetosc { get =>  Math.Round(ToMeter(a, UnitOfMeasure.milimeter) * ToMeter(b, UnitOfMeasure.milimeter) * ToMeter(c, UnitOfMeasure.milimeter), 9);   }



        public double Pole { get {return Math.Round(
            2*(ToMeter(a, UnitOfMeasure.milimeter)* ToMeter(b, UnitOfMeasure.milimeter)
            + ToMeter(a, UnitOfMeasure.milimeter)* ToMeter(c, UnitOfMeasure.milimeter)
            + ToMeter(b, UnitOfMeasure.milimeter)* ToMeter(c, UnitOfMeasure.milimeter))
            ,6,MidpointRounding.AwayFromZero); } }

        public override bool Equals(object other)
        {
            if (other is null) 
                return false;

            if (ReferenceEquals(this, other)) 
                return true;

            if (other is Pudelko pudelko) Equals(pudelko);
                return false;
        }
        public bool Equals(Pudelko other)
        {
            if (other is null) 
                return false;

            if (ReferenceEquals(this, other)) 
                return true;

            var otherSizes = new List<double> { other.a, other.b, other.c };

            int temp = otherSizes.FindIndex(x => x == a);
            if (temp != -1)
            {
                otherSizes.RemoveAt(temp);
                temp = otherSizes.FindIndex(x => x == b);
                if (temp != -1)
                {
                    otherSizes.RemoveAt(temp);
                    return otherSizes[0] == c;
                }
            }
            return false;
        }
        public override int GetHashCode() => (a, b, c).GetHashCode();


        public static bool operator ==(Pudelko box1, Pudelko box2) => box1.Equals(box2);
        public static bool operator !=(Pudelko box1, Pudelko box2) => !box1.Equals(box2);

        public static Pudelko operator +(Pudelko box1, Pudelko box2)
        {
            double[] boxy1 = {box1.A, box1.B, box1.C};
            double[] boxy2 = {box2.A, box2.B, box2.C};
            Array.Sort(boxy1, boxy2);

            return new Pudelko
                (
                a: boxy1[0] + boxy2[0],
                b: Math.Max(boxy1[1], boxy2[1]),
                c: Math.Max(boxy1[2], boxy2[2]),
                unit: UnitOfMeasure.meter
                );
        }

        public static explicit operator double[](Pudelko box) => new double[3] { box.A, box.B, box.C };

        public static implicit operator Pudelko(ValueTuple<int, int, int> value) => new Pudelko(value.Item1, value.Item2, value.Item3, UnitOfMeasure.milimeter);
        public double this[int idx]
        {
            get
            {
                switch (idx)
                {
                    case 0: 
                        return A;
                    case 1: 
                        return B;
                    case 2: 
                        return C;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public static Pudelko Parse(string str)
        {
            string[] dane = str.Split('×');
            if (dane.Length != 3 || 
               (dane[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length != 2 &&
                dane[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length != 2 &&
                dane[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length != 2)) 
                throw new FormatException();

            UnitOfMeasure unit;
            switch (dane[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1].Trim().ToLower())
            {
                case "m":
                    unit = UnitOfMeasure.meter;
                    break;
                case "cm":
                    unit = UnitOfMeasure.centimeter;
                    break;
                case "mm":
                    unit = UnitOfMeasure.milimeter;
                    break;
                default:
                    throw new FormatException();
            }
            double a = double.Parse(dane[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[0].Trim());
            double b = double.Parse(dane[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)[0].Trim());
            double c = double.Parse(dane[2].Split(' ', StringSplitOptions.RemoveEmptyEntries)[0].Trim());

            return new Pudelko(a, b, c,unit);
        }

        public IEnumerator<double> GetEnumerator()
        {
            yield return A;
            yield return B;
            yield return C;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
