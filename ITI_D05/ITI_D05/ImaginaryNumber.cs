namespace ITI_D05
{
    internal struct ImaginaryNumber
    {
        public double Real { get; set; }
        public double Imaginary { get; set; }

        public ImaginaryNumber(double real, double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public override string ToString()
        {
            if (Imaginary >= 0)
                return $"{Real} + {Imaginary}i";
            else
                return $"{Real} - {Math.Abs(Imaginary)}i";
        }
    }
}
