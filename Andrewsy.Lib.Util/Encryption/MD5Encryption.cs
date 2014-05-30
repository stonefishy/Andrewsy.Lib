using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.Configuration;
//using System.Linq;
using System.Text;

namespace Andrewsy.Lib.Util.Encryption
{
    /// <summary>
    /// MD5加密算法，此算法为不可逆算法
    /// </summary>
    public class MD5Encryption:IEncryption
    {

        /// <summary>
        /// 采用MD5算法加密明文
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns>密文</returns>
        public string Encrypt(string plainText)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(plainText, FormsAuthPasswordFormat.MD5.ToString());
        }

        /// <summary>
        /// MD5算法不可逆算法，不可解密
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public string Decrypt(string cipherText)
        {
            string strMsg = "MD5加密算法为不可逆算法，不能解密!";
            throw new NotImplementedException(strMsg);
        }
    }
}
