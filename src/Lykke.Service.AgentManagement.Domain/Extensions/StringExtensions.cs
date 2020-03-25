using System;
using System.Security.Cryptography;
using System.Text;

namespace Lykke.Service.AgentManagement.Domain.Extensions
{
    /// <summary>
    /// Contains common string extension methods.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Computes SHA256 hash from base64 string.
        /// </summary>
        /// <param name="base64String">The string encoded in base64.</param>
        /// <returns></returns>
        public static string ToSha256Hash(this string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                throw new ArgumentNullException(nameof(base64String));

            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Convert.FromBase64String(base64String));

                var sb = new StringBuilder();

                foreach (var @byte in bytes)
                    sb.Append(@byte.ToString("x2"));

                return sb.ToString();
            }
        }
    }
}
