using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.IO;

namespace CDI.Common
{
    public class EncryptHelper
    {

        #region TripleDES
        //8 byte initialization vector for TripleDES encryption.
        private static byte[] ivs = { 36, 232, 175, 181, 239, 22, 21, 158 };
        //24 byte key for TripleDES encryption.
        private static byte[] keys = {123, 112, 94, 236, 240, 77, 61, 18, 125, 224, 155, 72, 18, 181, 48, 101, 77, 217, 58, 141, 111, 240, 117, 114 };

        private static byte[] Encrypt(byte[] plainTextArray, byte[] keys, byte[] ivs)
        {
            using (MemoryStream ms = new MemoryStream())
            using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
            using (CryptoStream encStream = new CryptoStream(ms, tdes.CreateEncryptor(keys, ivs), CryptoStreamMode.Write))
            {
                encStream.Write(plainTextArray, 0, plainTextArray.Length);
                encStream.FlushFinalBlock();
                return ms.ToArray();
            }
        }

        private static byte[] Decrypt(byte[] encryptedData, byte[] keys, byte[] ivs)
        {
            using (MemoryStream ms = new MemoryStream())
            using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
            using (CryptoStream encStream = new CryptoStream(ms, tdes.CreateDecryptor(keys, ivs), CryptoStreamMode.Write))
            {
                encStream.Write(encryptedData, 0, encryptedData.Length);
                encStream.FlushFinalBlock();
                return ms.ToArray();
            }
        }

        public static byte[] Encrypt(byte[] plainTextArray)
        {
            return Encrypt(plainTextArray, keys, ivs);
        }
        public static byte[] Encrypt(string s)
        {
            var buffer = Encoding.UTF8.GetBytes(s);
            return Encrypt(buffer);
        }

        public static byte[] Decrypt(byte[] encryptedData)
        {
            return Decrypt(encryptedData, keys, ivs);
        }
        public static string DecryptToText(byte[] encryptedData)
        {
            var buffer = Decrypt(encryptedData);
            return Encoding.UTF8.GetString(buffer);
        }


        #endregion


    }
}
