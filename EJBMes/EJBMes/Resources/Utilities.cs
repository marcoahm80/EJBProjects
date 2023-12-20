using System.Security.Cryptography;
using System.Text;

namespace EJBMes.Resources
{
    public class Utilities
    {
        public static string EncriptKey(string key)
        {
            if (key == null)
            {
                return "null";
            }
            StringBuilder sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;

                byte[] result = hash.ComputeHash(enc.GetBytes(key));

                foreach (byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
            }
            return sb.ToString();
        }

        public static string DecodeString(string inputString)
        {
            string outputString = string.Empty;

            byte[] data = Convert.FromBase64String(inputString);
            outputString = Encoding.UTF8.GetString(data);

            return outputString;
        }
    }
}
