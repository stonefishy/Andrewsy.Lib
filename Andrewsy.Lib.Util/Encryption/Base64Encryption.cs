using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Andrewsy.Lib.Util.Encryption
{
    /// <summary>
    /// Base64加解密算法
    /// </summary>
    public class Base64Encryption:IEncryption
    {
        /// <summary>
        /// 采用64Base算法加密
        /// </summary>
        /// <param name="plaintText">明文</param>
        /// <returns>密文</returns>
        public string Encrypt(string plaintText)
        {
            byte[] encryptData = Encoding.UTF8.GetBytes(plaintText);
            return Convert.ToBase64String(encryptData);
        }

        /// <summary>
        /// 采用64Base算法解密
        /// </summary>
        /// <param name="cipherText">密文</param>
        /// <returns>明文</returns>
        public string Decrypt(string cipherText)
        {
            byte[] decryptData = Convert.FromBase64String(cipherText);
            return Encoding.UTF8.GetString(decryptData);
        }
    }
}
