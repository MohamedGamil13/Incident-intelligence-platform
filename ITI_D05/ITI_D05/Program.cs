using ITI_D05;

namespace ComplexNumberApp
{
    internal class Program
    {
        static void Main(string[] args)
        {



            Console.WriteLine("Enter First Complex Number:");
            ImaginaryNumber c1 = ReadComplexNumber();


            Console.WriteLine("\nEnter Second Complex Number:");
            ImaginaryNumber c2 = ReadComplexNumber();


            ImaginaryNumber sum = Add(c1, c2);
            ImaginaryNumber diff = Subtract(c1, c2);


            Console.WriteLine("\n--------------------------------------");
            Console.WriteLine($"Sum         : {sum}");
            Console.WriteLine($"Difference  : {diff}");
            Console.WriteLine("-------------------------------------------");

            Console.ReadKey();
        }


        static ImaginaryNumber Add(ImaginaryNumber c1, ImaginaryNumber c2)
        {
            ImaginaryNumber result = new ImaginaryNumber();
            result.Real = c1.Real + c2.Real;
            result.Imaginary = c1.Imaginary + c2.Imaginary;
            return result;
        }


        static ImaginaryNumber Subtract(ImaginaryNumber c1, ImaginaryNumber c2)
        {
            ImaginaryNumber result = new ImaginaryNumber();
            result.Real = c1.Real - c2.Real;
            result.Imaginary = c1.Imaginary - c2.Imaginary;
            return result;
        }


        static ImaginaryNumber ReadComplexNumber()
        {
            Console.Write("Enter Real Part: ");
            double.TryParse(Console.ReadLine(), out double real);

            Console.Write("Enter Imaginary Part: ");
            double.TryParse(Console.ReadLine(), out double imaginary);

            return new ImaginaryNumber(real, imaginary);
        }
    }
}