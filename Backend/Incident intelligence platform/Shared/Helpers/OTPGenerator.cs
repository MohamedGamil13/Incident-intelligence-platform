using System.Security.Cryptography;


namespace Shared.Helpers
{
    public static class OtpGenerator
    {
        public static string Generate(int length = 6)
        {
            if (length < 6 || length > 8)
                throw new ArgumentOutOfRangeException(nameof(length));

            int max = (int)Math.Pow(10, length);

            int otp = RandomNumberGenerator.GetInt32(0, max);

            return otp.ToString($"D{length}");
        }
    }
}
