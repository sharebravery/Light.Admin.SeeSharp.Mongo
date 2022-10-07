using System.Security.Cryptography;
using System.Text;

namespace Light.Admin.Mongo.Utils
{
    public static class Hash
    {
        /// <summary>
        /// Hash Sha256加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Sha256(string input)
        {
            if (String.IsNullOrEmpty(input)) return string.Empty;
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
