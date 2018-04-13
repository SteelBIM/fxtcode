using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace CourseActivate.Core.Utility
{
    /// <summary>
    /// DES加密操作 
    /// </summary>
    public class DESEncrypt
    {
        private const string _iv = "RFV%$BGT";
        private const string _key = "Z9ZUO";
        private const string iv = "12345678";

        TripleDESCryptoServiceProvider _des = new TripleDESCryptoServiceProvider();
        private Encoding _encoding = Encoding.UTF8;

        /// <summary>
        /// 
        /// </summary>
        /// <value>The encoding mode.</value>
        public Encoding EncodingMode
        {
            get
            {
                return _encoding;
            }
            set
            {
                _encoding = value;
            }
        }

        /// <summary>
        /// ADES加密，
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>返回加密字符串</returns>
        public string EncryptToString(string str, string encryptyKey)
        {
            encryptyKey = encryptyKey.Replace("-", "");
            if (encryptyKey.Length < 19)
            {
                encryptyKey = encryptyKey.PadRight(19, '0');
            }
            string iv = _iv;
            string key = _key + encryptyKey.Substring(0, 19);
            if (String.IsNullOrEmpty(key))
                return null;
            var ivb = Encoding.ASCII.GetBytes(iv);
            var keyb = Encoding.ASCII.GetBytes(key);
            var tob = EncodingMode.GetBytes(str);
            byte[] encrypted;

            try
            {
                // Create a MemoryStream.
                MemoryStream mStream = new MemoryStream();

                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.CBC;             //默认值
                tdsp.Padding = PaddingMode.PKCS7;       //默认值

                // Create a CryptoStream using the MemoryStream 
                // and the passed key and initialization vector (IV).
                CryptoStream cStream = new CryptoStream(mStream,
                    _des.CreateEncryptor(keyb, ivb),
                    CryptoStreamMode.Write);

                // Write the byte array to the crypto stream and flush it.
                cStream.Write(tob, 0, tob.Length);
                cStream.FlushFinalBlock();

                // Get an array of bytes from the 
                // MemoryStream that holds the 
                // encrypted data.
                encrypted = mStream.ToArray();

                // Close the streams.
                cStream.Close();
                mStream.Close();

                return Convert.ToBase64String(encrypted);
                // Return the encrypted buffer.
                //return EncodingMode.GetString(encrypted);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// ADES加密，返回加密字符串
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>返回加密二进制数组</returns>
        public byte[] Encrypt(string str, string encryptyKey)
        {
            encryptyKey = encryptyKey.Replace("-", "");
            if (encryptyKey.Length < 19)
            {
                encryptyKey = encryptyKey.PadRight(19, '0');
            }
            string iv = _iv;
            string key = _key + encryptyKey.Substring(0, 19);
            if (String.IsNullOrEmpty(key))
                return null;
            var ivb = Encoding.ASCII.GetBytes(iv);
            var keyb = Encoding.ASCII.GetBytes(key);
            var tob = EncodingMode.GetBytes(str);
            byte[] encrypted;

            try
            {
                // Create a MemoryStream.
                MemoryStream mStream = new MemoryStream();

                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.CBC;             //默认值
                tdsp.Padding = PaddingMode.PKCS7;       //默认值

                // Create a CryptoStream using the MemoryStream 
                // and the passed key and initialization vector (IV).
                CryptoStream cStream = new CryptoStream(mStream,
                    _des.CreateEncryptor(keyb, ivb),
                    CryptoStreamMode.Write);

                // Write the byte array to the crypto stream and flush it.
                cStream.Write(tob, 0, tob.Length);
                cStream.FlushFinalBlock();

                // Get an array of bytes from the 
                // MemoryStream that holds the 
                // encrypted data.
                encrypted = mStream.ToArray();

                // Close the streams.
                cStream.Close();
                mStream.Close();

                return encrypted;
                // Return the encrypted buffer.
                //return EncodingMode.GetString(encrypted);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }

        /// <summary>
        /// ADes解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Decrypt(byte[] tob, string encryptyKey)
        {
            encryptyKey = encryptyKey.Replace("-", "");
            if (encryptyKey.Length < 19)
            {
                encryptyKey = encryptyKey.PadRight(19, '0');
            }
            string iv = _iv;
            string key = _key + encryptyKey.Substring(0, 19);

            if (String.IsNullOrEmpty(key))
                return null;

            var ivb = Encoding.ASCII.GetBytes(iv);
            var keyb = Encoding.ASCII.GetBytes(key);
            // var tob = EncodingMode.GetBytes(str);
            byte[] encrypted;
            try
            {
                // Create a new MemoryStream using the passed 
                // array of encrypted data.
                if (tob == null)
                    return null;
                MemoryStream msDecrypt = new MemoryStream();

                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.CBC;
                tdsp.Padding = PaddingMode.PKCS7;

                // Create a CryptoStream using the MemoryStream 
                // and the passed key and initialization vector (IV).
                CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                    tdsp.CreateDecryptor(keyb, ivb),
                    CryptoStreamMode.Read);

                ///////////
                ICryptoTransform desdecrypt123 = tdsp.CreateDecryptor(keyb, ivb);
                MemoryStream writerS123 = new MemoryStream();
                CryptoStream cryptostreamDecr123 = new CryptoStream(writerS123, desdecrypt123, CryptoStreamMode.Write);
                cryptostreamDecr123.Write(tob, 0, tob.Length);
                cryptostreamDecr123.FlushFinalBlock();
                encrypted = writerS123.ToArray();
                /////////////

                //// Create buffer to hold the decrypted data.
                //encrypted = new byte[msDecrypt.Length];

                //// Read the decrypted data out of the crypto stream
                //// and place it into the temporary buffer.
                //csDecrypt.Read(encrypted, 0, encrypted.Length);
                //// csDecrypt.FlushFinalBlock();

                ////Convert the buffer into a string and return it.
                return EncodingMode.GetString(encrypted);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }
       
        /// <summary>
        /// ADes解密
        /// </summary>
        /// <param name="inputStr">密文串</param>
        /// <param name="encryptyKey"></param>
        /// <returns>解密后的字符串</returns>
        public string Decrypt(string inputStr, string encryptyKey)
        {
            char[] charBuffer = inputStr.ToCharArray();
            byte[] tob = Convert.FromBase64CharArray(charBuffer, 0, charBuffer.Length);
            //byte[] tob = EncodingMode.GetBytes(inputStr);
            encryptyKey = encryptyKey.Replace("-", "");
            if (encryptyKey.Length < 19)
            {
                encryptyKey = encryptyKey.PadRight(19, '0');
            }
            string iv = _iv;
            string key = _key + encryptyKey.Substring(0, 19);

            if (String.IsNullOrEmpty(key))
                return null;

            var ivb = Encoding.ASCII.GetBytes(iv);
            var keyb = Encoding.ASCII.GetBytes(key);
            // var tob = EncodingMode.GetBytes(str);
            byte[] encrypted;
            try
            {
                // Create a new MemoryStream using the passed 
                // array of encrypted data.
                if (tob == null)
                    return null;
                MemoryStream msDecrypt = new MemoryStream();

                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.CBC;
                tdsp.Padding = PaddingMode.PKCS7;

                // Create a CryptoStream using the MemoryStream 
                // and the passed key and initialization vector (IV).
                CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                    tdsp.CreateDecryptor(keyb, ivb),
                    CryptoStreamMode.Read);

                ///////////
                ICryptoTransform desdecrypt123 = tdsp.CreateDecryptor(keyb, ivb);
                MemoryStream writerS123 = new MemoryStream();
                CryptoStream cryptostreamDecr123 = new CryptoStream(writerS123, desdecrypt123, CryptoStreamMode.Write);
                cryptostreamDecr123.Write(tob, 0, tob.Length);
                cryptostreamDecr123.FlushFinalBlock();
                encrypted = writerS123.ToArray();
                /////////////

                //// Create buffer to hold the decrypted data.
                //encrypted = new byte[msDecrypt.Length];

                //// Read the decrypted data out of the crypto stream
                //// and place it into the temporary buffer.
                //csDecrypt.Read(encrypted, 0, encrypted.Length);
                //// csDecrypt.FlushFinalBlock();

                ////Convert the buffer into a string and return it.
                return EncodingMode.GetString(encrypted);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string Encrypt(string toEncrypt, string key, string iv)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] ivArray = UTF8Encoding.UTF8.GetBytes(iv);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.BlockSize = 128;
            rDel.KeySize = 256;
            rDel.FeedbackSize = 128;
            rDel.Padding = PaddingMode.PKCS7;
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string Decrypt(string toDecrypt, string key, string iv)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] ivArray = UTF8Encoding.UTF8.GetBytes(iv);
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.Zeros;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey);
                byte[] rgbIV = Encoding.UTF8.GetBytes(iv);
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                dCSP.Mode = CipherMode.CBC;
                dCSP.Padding = PaddingMode.PKCS7;
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();

                return Convert.ToBase64String(mStream.ToArray());
            }
            catch (Exception ex)
            {
                return encryptString;
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Encoding.UTF8.GetBytes(iv);
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                dCSP.Mode = CipherMode.CBC;
                dCSP.Padding = PaddingMode.PKCS7;
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch (Exception ex)
            {
                return decryptString;
            }
        }



    }
}
