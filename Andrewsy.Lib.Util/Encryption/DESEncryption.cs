using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
//using System.Linq;
using System.Text;

namespace Andrewsy.Lib.Util.Encryption
{
    /// <summary>
    /// DES对称加密算法
    /// </summary>
    public class DESEncryption:IEncryption
    {
        /// <summary>
        /// 加解密密钥
        /// </summary>
        private static readonly string DES_Key="Rs@#NG23";

        /// <summary>
        /// 加解密密钥向量
        /// </summary>
        private static readonly byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// DES可逆算法加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns>密文</returns>
        public string Encrypt(string plaintText)
        {
            try
            {
                byte[] encryptKey = Encoding.UTF8.GetBytes(DES_Key.Substring(0, 8));
                byte[] plaintData = Encoding.UTF8.GetBytes(plaintText);

                DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, desProvider.CreateEncryptor(encryptKey, Keys), CryptoStreamMode.Write);
                cStream.Write(plaintData, 0, plaintData.Length);
                cStream.FlushFinalBlock();

                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return plaintText;
            }
        }

        /// <summary>
        /// DES可逆算法解密
        /// </summary>
        /// <param name="cipherText">密文</param>
        /// <returns>明文</returns>
        public string Decrypt(string cipherText)
        {
            try
            {
                byte[] decryptKey = Encoding.UTF8.GetBytes(DES_Key.Substring(0, 8));
                byte[] cipherData = Encoding.UTF8.GetBytes(cipherText);

                DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, desProvider.CreateDecryptor(decryptKey, Keys), CryptoStreamMode.Write);
                cStream.Write(cipherData, 0, cipherData.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return cipherText;
            }
        }
    }
}
