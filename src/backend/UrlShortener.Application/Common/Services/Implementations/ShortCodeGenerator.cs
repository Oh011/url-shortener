using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
 using Shared.Utilities;
using UrlShortener.Application.Common.Interfaces;

namespace UrlShortener.Application.Common.Services.Implementations
{

  

    public class ShortCodeGenerator : IShortCodeGenerator
    {
        private const int Size = 7;
        private const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public string Generate()
        {
            Span<byte> buffer = stackalloc byte[8];
            RandomNumberGenerator.Fill(buffer);

            ulong value = BitConverter.ToUInt64(buffer) & ((1UL << 42) - 1);

            var code = Base62.Encode((long)value);

            return code.PadLeft(Size, '0');
        }
    }
}
