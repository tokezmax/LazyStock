using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

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
        public static string TripleDESEncrypt(string sourceString, string secretKey)
        {
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
        }
    }
}
