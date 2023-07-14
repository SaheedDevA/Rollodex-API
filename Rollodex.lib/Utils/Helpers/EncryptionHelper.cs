using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Rolodex.Lib.Utils.Helpers
{
    public class EncryptionHelper
    {
        private static string secret_key;
        private static string secret_iv;

        public EncryptionHelper(string inputKey, string inputIv)
        {
            secret_key = inputKey;
            secret_iv = inputIv;
        }

        /// <summary>
        ///     MD5
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToMD5(string str)
        {
            var encode = Encoding.UTF8;

            if (string.IsNullOrWhiteSpace(str)) return "";

            using (var md5 = MD5.Create())
            {
                var bytes = md5.ComputeHash(encode.GetBytes(str));
                var sb = new StringBuilder();
                foreach (var i in bytes) sb.Append(i.ToString("x2"));
                return sb.ToString();
            }
        }

        /// <summary>
        ///     SHA1
        /// </summary>
        /// <param name="content"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string SHA1(string content, Encoding encode)
        {
            try
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                var bytes_in = encode.GetBytes(content);
                var bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Dispose();
                var result = BitConverter.ToString(bytes_out);
                result = result.Replace("-", "");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("SHA1：" + ex.Message);
            }
        }

        /// <summary>
        ///     AES- ECB - PKCS7
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <returns></returns>
        public string AESEncrypt(string toEncrypt)
        {
            if (string.IsNullOrWhiteSpace(toEncrypt)) return "";

            var keyArray = Encoding.UTF8.GetBytes(secret_key);
            var ivArray = Encoding.UTF8.GetBytes(secret_iv);
            var toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            var rDel = new RijndaelManaged
            {
                Key = keyArray,
                IV = ivArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            var cTransform = rDel.CreateEncryptor();
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        ///     AES- ECB - PKCS7
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <returns></returns>
        public string AESDecrypt(string toDecrypt)
        {
            if (string.IsNullOrWhiteSpace(toDecrypt)) return "";

            var keyArray = Encoding.UTF8.GetBytes(secret_key);
            var ivArray = Encoding.UTF8.GetBytes(secret_iv);
            var toEncryptArray = Convert.FromBase64String(toDecrypt);

            var rDel = new RijndaelManaged
            {
                Key = keyArray,
                IV = ivArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            var cTransform = rDel.CreateDecryptor();
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        ///     AESkey
        /// </summary>
        /// <returns></returns>
        public static string GeyRandomAESKey()
        {
            var str = string.Empty;
            var rnd1 = new Random();
            var r = rnd1.Next(10, 100);
            var num2 = DateTime.Now.Ticks + r;
            var random = new Random((int)((ulong)num2 & 0xffffffffL) | (int)(num2 >> r));
            for (var i = 0; i < 16; i++)
            {
                char ch;
                var num = random.Next();
                if (num % 2 == 0)
                    ch = (char)(0x30 + (ushort)(num % 10));
                else
                    ch = (char)(0x41 + (ushort)(num % 0x1a));
                str = str + ch;
            }

            return str;
        }

        public static string GetMD5HashFromFile(Stream stream)
        {
            try
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                var retVal = md5.ComputeHash(stream);
                var sb = new StringBuilder();
                for (var i = 0; i < retVal.Length; i++) sb.Append(retVal[i].ToString("x2"));
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }
    }
}