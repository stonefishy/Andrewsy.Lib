using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Andrewsy.Lib.Util.Encryption
{
    /// <summary>
    /// 加密算法枚举
    /// </summary>
    public enum EncryptionMode
    {
        /// <summary>
        /// SHA1加密算法，不可逆
        /// </summary>
        SHA1,

        /// <summary>
        /// MD5加密算法，不可逆
        /// </summary>
        MD5,

        /// <summary>
        /// AES对称加密算法，可逆
        /// </summary>
        AES,

        /// <summary>
        /// DES加密算法， 可逆
        /// </summary>
        DES,

        /// <summary>
        /// Base64加密算法，可逆
        /// </summary>
        Base64
    }
}
