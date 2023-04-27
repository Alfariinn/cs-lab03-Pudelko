using PudelkoLib;

namespace Program
{

    public static class Utility
    {
        public static Pudelko Kompresuj(this Pudelko box) => new Pudelko(Math.Pow(box.Objetosc, 1.0 / 3.0), Math.Pow(box.Objetosc, 1.0 / 3.0), Math.Pow(box.Objetosc, 1.0 / 3.0));


        public static int Comparision(Pudelko box1, Pudelko box2)
        {
            int compare = box1.Objetosc.CompareTo(box2.Objetosc);
            if (compare != 0)
                return compare;

            compare = box1.Pole.CompareTo(box2.Pole);
            if (compare != 0)
                return compare;
            double sum1 = box1.A + box1.B + box1.C;
            double sum2 = box2.A + box2.B + box2.C;
            return sum2.CompareTo(sum1);
        }
    }
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("   Kompresja:\n");
            Pudelko box1 = new Pudelko(3, 5, 7);
            Pudelko box2 = box1.Kompresuj();

            Console.WriteLine(" "+box2);

            Console.WriteLine("\n   Lista:\n");
            var Lista = new List<Pudelko>();
            Lista.Add(new Pudelko());
            Lista.Add(new Pudelko(1, 1, 1));
            Lista.Add(new Pudelko(1, 3, 3, UnitOfMeasure.meter));
            Lista.Add(new Pudelko(150, 301, 304, UnitOfMeasure.centimeter));
            Lista.Add(new Pudelko(777, 6969, unit: UnitOfMeasure.milimeter));
            foreach (var box in Lista)
                Console.WriteLine(" "+box);
            Console.WriteLine("\n   Sortowanie \n   od najmniejszego:\n");
            Lista.Sort(Utility.Comparision);

            foreach (var box in Lista)
                Console.WriteLine(" "+box);
        }
    }
}