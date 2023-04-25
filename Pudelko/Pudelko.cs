using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Metrics;
using System.Drawing;

namespace PudelkoLib
{
    public sealed class Pudelko : IFormattable, IEquatable<Pudelko>
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
        //public double ToCentimeter(double value, UnitOfMeasure unitOfMeasure)
        //{
        //    switch (unitOfMeasure)
        //    {
        //        case UnitOfMeasure.milimeter: return value / 10;
        //        case UnitOfMeasure.centimeter: return value;
        //        case UnitOfMeasure.meter: return value * 100;
        //        default: throw new ArgumentException();
        //    }
        //}
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
            if (this.a <= 0 || this.b <= 0 || this.c <= 0|| this.a > 10000 || this.b > 10000 || this.c > 10000) throw new ArgumentOutOfRangeException();
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

        public double Objetosc { get { return Math.Round(ToMeter(a, UnitOfMeasure.milimeter) * ToMeter(b, UnitOfMeasure.milimeter) * ToMeter(c, UnitOfMeasure.milimeter), 9, MidpointRounding.AwayFromZero); }  }

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
            double[] boxy1 = {box1.a, box1.b, box1.c};
            double[] boxy2 = {box2.a, box2.b, box2.c};
            Array.Sort(boxy1, boxy2);

            return new Pudelko
                (
                a: box1.a + box2.a,
                b: Math.Max(box1.b, box2.b),
                c: Math.Max(box1.c, box2.c),
                unit: UnitOfMeasure.meter
                );
        }



    }
}
