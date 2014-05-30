using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Andrewsy.Lib.Util.Encryption
{
    /// <summary>
    /// 加解密接口
    /// </summary>
    public interface IEncryption
    {
        /// <summary>
        /// 将明文字符串加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns>密文</returns>
        string Encrypt(string plainText);

        /// <summary>
        /// 将密文字符串解密
        /// </summary>
        /// <param name="cipherText">密文</param>
        /// <returns>明文</returns>
        string Decrypt(string cipherText);
    }
}
