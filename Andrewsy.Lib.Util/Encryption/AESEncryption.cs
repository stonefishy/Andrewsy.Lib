using System;
using System.Collections.Generic;
using System.Security.Cryptography;
//using System.Linq;
using System.Text;

namespace Andrewsy.Lib.Util.Encryption
{
    /// <summary>
    /// 对称加密算法AES RijndaelManaged加密解密
    /// </summary>
    public class AESEncryption:IEncryption
    {
        /// <summary>
        /// 加解密密钥
        /// </summary>
        private static readonly string AES_Key = "Rs@#NG23";

        /// <summary>
        /// 密钥向量
        /// </summary>
        private static readonly byte[] Keys = { 0x6F, 0x79, 0x3F, 0x6E, 0x61, 0x6D, 0x77, 0x6D, 0x75,
                                                  0x6F, 0x65, 0x41, 0x53, 0x6E, 0x72, 0x79 };
        /// <summary>
        /// AES对称加密算法加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns>密文</returns>
        public string Encrypt(string plainText)
        {
            return AES_Encrypt(plainText);
        }

        /// <summary>
        /// AES对称加密算法解密
        /// </summary>
        /// <param name="cipherText">密文</param>
        /// <returns>明文</returns>
        public string Decrypt(string cipherText)
        {
            return AES_Decrypt(cipherText);
        }

        /// <summary>
        /// 对称加密算法AES RijndaelManaged加密(RijndaelManaged（AES）算法是块式加密算法)
        /// </summary>
        /// <param name="encryptString">明文<param>
        /// <returns>密文</returns>
        private string AES_Encrypt(string plainText)
        {
            try
            {
                string encryptKey = GetSubString(AES_Key, 32, "");
                encryptKey = encryptKey.PadRight(32, ' ');

                RijndaelManaged rijndaelProvider = new RijndaelManaged();
                rijndaelProvider.Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 32));
                rijndaelProvider.IV = Keys;
                ICryptoTransform rijndaelEncrypt = rijndaelProvider.CreateEncryptor();

                byte[] plaintData = Encoding.UTF8.GetBytes(plainText);
                byte[] encryptData = rijndaelEncrypt.TransformFinalBlock(plaintData, 0, plaintData.Length);

                return Convert.ToBase64String(encryptData);
            }
            catch
            {
                return plainText;
            }

        }

        /// <summary>
        /// 对称加密算法AES RijndaelManaged解密字符串
        /// </summary>
        /// <param name="decryptString">密文</param>
        /// <returns>明文</returns>
        private string AES_Decrypt(string cipherText)
        {
            try
            {
                string decryptKey = GetSubString(AES_Key, 32, "");
                decryptKey = decryptKey.PadRight(32, ' ');

                RijndaelManaged rijndaelProvider = new RijndaelManaged();
                rijndaelProvider.Key = Encoding.UTF8.GetBytes(decryptKey);
                rijndaelProvider.IV = Keys;
                ICryptoTransform rijndaelDecrypt = rijndaelProvider.CreateDecryptor();

                byte[] cipherData = Encoding.UTF8.GetBytes(cipherText);
                byte[] decryptData = rijndaelDecrypt.TransformFinalBlock(cipherData, 0, cipherData.Length);

                return Encoding.UTF8.GetString(decryptData);
            }
            catch
            {
                return cipherText;
            }
        }

        /// <summary>
        /// 按字节长度(按字节,一个汉字为2个字节)取得某字符串的一部分
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="length">所取字符串字节长度</param>
        /// <param name="tailString">附加字符串(当字符串不够长时，尾部所添加的字符串，一般为"..")</param>
        /// <returns>某字符串的一部分</returns
        private string GetSubString(string srcString, int length, string tailString)
        {
            return GetSubString(srcString, 0, length, tailString);
        }

        /// <summary>
        /// 按字节长度(按字节,一个汉字为2个字节)取得某字符串的一部分
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="startIndex">索引位置，以0开始</param>
        /// <param name="length">所取字符串字节长度</param>
        /// <param name="tailString">附加字符串(当字符串不够长时，尾部所添加的字符串，一般为"..")</param>
        /// <returns>某字符串的一部分</returns>
        private string GetSubString(string sourceString, int startIndex, int length, string tailString)
        {
            string myResult = sourceString;

            //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
            if (System.Text.RegularExpressions.Regex.IsMatch(sourceString, "[\u0800-\u4e00]+") ||
            System.Text.RegularExpressions.Regex.IsMatch(sourceString, "[\xAC00-\xD7A3]+"))
            {
                //当截取的起始位置超出字段串长度时
                if (startIndex >= sourceString.Length)
                {
                    return string.Empty;
                }
                else
                {
                    return sourceString.Substring(startIndex,
                    ((length + startIndex) > sourceString.Length) ? (sourceString.Length - startIndex) : length);
                }
            }

            //中文字符，如"中国人民abcd123"
            if (length <= 0)
            {
                return string.Empty;
            }
            byte[] bytesSource = Encoding.Default.GetBytes(sourceString);

            //当字符串长度大于起始位置
            if (bytesSource.Length > startIndex)
            {
                int endIndex = bytesSource.Length;

                //当要截取的长度在字符串的有效长度范围内
                if (bytesSource.Length > (startIndex + length))
                {
                    endIndex = length + startIndex;
                }
                else
                { //当不在有效范围内时,只取到字符串的结尾
                    length = bytesSource.Length - startIndex;
                    tailString = "";
                }

                int[] anResultFlag = new int[length];
                int nFlag = 0;
                //字节大于127为双字节字符
                for (int i = startIndex; i < endIndex; i++)
                {
                    if (bytesSource[i] > 127)
                    {
                        nFlag++;
                        if (nFlag == 3)
                        {
                            nFlag = 1;
                        }
                    }
                    else
                    {
                        nFlag = 0;
                    }
                    anResultFlag[i] = nFlag;
                }
                //最后一个字节为双字节字符的一半
                if ((bytesSource[endIndex - 1] > 127) && (anResultFlag[length - 1] == 1))
                {
                    length = length + 1;
                }

                byte[] bsResult = new byte[length];
                Array.Copy(bytesSource, startIndex, bsResult, 0, length);
                myResult = Encoding.Default.GetString(bsResult);
                myResult = myResult + tailString;

                return myResult;
            }

            return string.Empty;

        }

    }
}
