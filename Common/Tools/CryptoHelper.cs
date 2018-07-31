using System;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public static class CryptoHelper
    {
        /// <summary>
        /// 對字串做TripleDES加密
        /// </summary>
        /// <param name="sourceString">原始字串</param>
        /// <param name="secretKey">TripleDES加密用的金鑰字串</param>
        /// <returns></returns>
        public static string Encrypt3DES(string sourceString, string secretKey)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            DES.Key = UTF8Encoding.UTF8.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(secretKey.PadRight(24, '0'), "md5").Substring(0, 24));
            DES.Mode = CipherMode.ECB;
            ICryptoTransform DESEncrypt = DES.CreateEncryptor();
            byte[] Buffer = UTF8Encoding.UTF8.GetBytes(sourceString);
            return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
            /*
            var provider = new TripleDESCryptoServiceProvider();

            provider.Key = Encoding.UTF8.GetBytes(secretKey);
            provider.Mode = CipherMode.ECB;
            provider.Padding = PaddingMode.Zeros;

            //取得UTF8編碼的byte陣列
            byte[] sourceBytes = Encoding.UTF8.GetBytes(sourceString);
            //加密
            var encryptor = provider.CreateEncryptor();
            byte[] encryptedBytes = encryptor.TransformFinalBlock(sourceBytes, 0, sourceBytes.Length);
            //將加密後的byte陣列轉換成Hex字串
            string encodeString = BitConverter.ToString(encryptedBytes).Replace("-", "").ToLower();

            return encodeString;
            */
        }

        /// <summary>
        /// 對字串做TripleDES加密
        /// </summary>
        /// <param name="sourceString">原始字串</param>
        /// <param name="secretKey">TripleDES加密用的金鑰字串</param>
        /// <returns></returns>
        public static string Decrypt3DES(string strEncryptData, string secretKey)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            DES.Key = UTF8Encoding.UTF8.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(secretKey.PadRight(24, '0'), "md5").Substring(0, 24));
            DES.Mode = CipherMode.ECB;
            DES.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            ICryptoTransform DESDecrypt = DES.CreateDecryptor();
            string result = "";
            byte[] Buffer = Convert.FromBase64String(strEncryptData);
            result = UTF8Encoding.UTF8.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
            return result;
        }
    }
}