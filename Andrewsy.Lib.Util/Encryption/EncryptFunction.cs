using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Andrewsy.Lib.Util.Encryption
{
    /// <summary>
    /// 简单工厂模式 创建加密对象
    /// </summary>
    public abstract class EncryptFunction
    {
        /// <summary>
        /// 创建指定的加密实例
        /// </summary>
        /// <param name="encryptMode">加密模式</param>
        /// <returns></returns>
        public static IEncryption CreateInstance(EncryptionMode encryptMode)
        {
            switch (encryptMode)
            {
                case EncryptionMode.SHA1:
                    return new SHA1Encryption();
                case EncryptionMode.MD5:
                    return new MD5Encryption();
                case EncryptionMode.AES:
                    return new AESEncryption();
                case EncryptionMode.DES:
                    return new DESEncryption();
                case EncryptionMode.Base64:
                    return new Base64Encryption();
                default:
                    return new DESEncryption();

            }
        }
    }
}
