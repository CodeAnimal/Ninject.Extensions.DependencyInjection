using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Integration.Net6.Services.Abstractions;

namespace Integration.Net6.Services
{
    [SuppressMessage("Security", "CA5394:Do not use insecure randomness", Justification = "Only used for testing.")]
    public class ServiceA : IServiceA
    {
        private readonly Random rng = new();
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        
        public string GetRandomString(int size)
        {
            return string.Concat(
                Enumerable.Range(1, size)
                    .Select(_ => Chars[rng.Next(Chars.Length)])
                );
        }
    }
}
