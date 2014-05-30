using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.Configuration;
//using System.Linq;
using System.Text;

namespace Andrewsy.Lib.Util.Encryption
{
    /// <summary>
    /// SHA1加密算法，为不可逆算法
    /// </summary>
    public class SHA1Encryption:IEncryption
    {
        /// <summary>
        /// SHA1加密算法加密明文
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns>密文</returns>
        public string Encrypt(string plainText)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(plainText, FormsAuthPasswordFormat.SHA1.ToString());
        }

        /// <summary>
        /// SHA1算法 不可解密，无实现
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public string Decrypt(string cipherText)
        {
            string strMsg = "SHA1加密算法为不可逆算法，不能解密!";
            throw new NotImplementedException(strMsg);
        }
    }
}
