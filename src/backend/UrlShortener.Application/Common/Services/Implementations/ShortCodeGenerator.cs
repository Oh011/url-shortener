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
        private const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const int Size = 7;

        public string Generate()
        {
            Span<char> result = stackalloc char[Size];
            Span<byte> buffer = stackalloc byte[Size];

            RandomNumberGenerator.Fill(buffer);

            for (int i = 0; i < Size; i++)
            {
                result[i] = Alphabet[buffer[i] % Alphabet.Length];
            }

            return new string(result);
        }
    }


}