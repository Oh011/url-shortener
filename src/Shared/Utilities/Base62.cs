using System.Text;

namespace Shared.Utilities
{
    public static class Base62
    {

        private const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";




        public static string Encode(long value)
        {


            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Value must be non-negative.");

            if (value == 0) return Alphabet[0].ToString();



            StringBuilder sb = new StringBuilder();

            while (value > 0)
            {

                var remainder = (int)value % 62;
                sb.Insert(0, Alphabet[remainder]);
                value = value / 62;

                //--> read digits backwards
                //Dividend=Divisor×Quotient+Remainder

            }

            return sb.ToString();
        }



        private static int CharToNumber(char c)
        {
            if (c >= '0' && c <= '9') return c - '0';
            if (c >= 'A' && c <= 'Z') return c - 'A' + 10;
            if (c >= 'a' && c <= 'z') return c - 'a' + 36;
            throw new ArgumentException("Invalid character for Base62");
        }
        public static long Decode(string base62)
        {

            if (string.IsNullOrEmpty(base62))
                throw new ArgumentNullException(nameof(base62));

            long result = 0;
            int counter = base62.Length - 1;
            foreach (var c in base62)
            {
                if (!Alphabet.Contains(c))
                    throw new ArgumentException($"Invalid character '{c}' in Base62 string.");

                int val = c;
                result = result + (int)Math.Pow(62, counter) * CharToNumber(c);
                --counter;
            }

            return result;
        }

    }



}
