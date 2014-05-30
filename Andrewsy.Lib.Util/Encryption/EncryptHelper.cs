using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using Andrewsy.Lib.Util.Encryption;

namespace Andrewsy.Lib.Util.Encryption
{
    /// <summary>
    /// 加解密辅助类
    /// </summary>
    public class EncryptHelper
    {
        /// <summary>
        /// 加密模式，默认为DES加解密算法
        /// </summary>
        public static EncryptionMode EncryptMode = EncryptionMode.DES;

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="plaintString">明文</param>
        /// <returns>密文</returns>
        public static string Encrypt(string plaintString)
        {
            IEncryption encrypt = EncryptFunction.CreateInstance(EncryptMode);
            return encrypt.Encrypt(plaintString);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="cipherString">密文</param>
        /// <returns>明文</returns>
        public static string Decrypt(string cipherString)
        {
            IEncryption encrypt = EncryptFunction.CreateInstance(EncryptMode);
            return encrypt.Decrypt(cipherString);
        }
    }
}
