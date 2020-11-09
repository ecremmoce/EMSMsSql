using System.Security.Cryptography;
using System.Text;

namespace ShopeeManagement.DTOs
{
    public abstract class ReqBase
    {
        protected string HashString(string StringToHash, string HashKey)
        {
            UTF8Encoding myEncoder = new UTF8Encoding();
            byte[] key = myEncoder.GetBytes(HashKey);
            byte[] Text = myEncoder.GetBytes(StringToHash);
            HMACSHA256 myHMACSHA256 = new HMACSHA256(key);
            byte[] HashCode = myHMACSHA256.ComputeHash(Text);
            string hash = ByteToString(HashCode);
            return hash.ToLower();
        }

        private string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }
    }
}
