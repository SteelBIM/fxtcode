using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;


namespace CAS.Common
{
    public class EncryptHelper
    {

        /// <summary>
        /// 进行MD5效验
        /// </summary>
        /// <param name="strmd5"></param>
        /// <returns></returns>
        public static string GetMd5(string strmd5)
        {
            byte[] md5Bytes = ASCIIEncoding.Default.GetBytes(strmd5);
            byte[] encodedBytes;
            MD5 md5;
            md5 = new MD5CryptoServiceProvider();
            //FileStream fs= new FileStream(filepath,FileMode.Open,FileAccess.Read);
            encodedBytes = md5.ComputeHash(md5Bytes);
            string nn = BitConverter.ToString(encodedBytes);
            nn = Regex.Replace(nn, "-", "");//因为转化完的都是34-2d这样的，所以替换掉- 
            nn = nn.ToLower();//根据需要转化成小写
            //fs.Close();
            return nn;
        }

        /// <summary>
        /// 密码加密函数
        /// </summary>
        /// <param name="strpass"></param>
        /// <returns></returns>
        public static string TextToPassword(string strpass)
        {
            return Encrypt(strpass, "fxtsys01");
        }
        /// <summary>
        /// 密码解密函数
        /// </summary>
        /// <param name="strpass"></param>
        /// <returns></returns>
        public static string PasswordToText(string strpass)
        {
            return Decrypt(strpass, "fxtsys01");
        }

        /// <summary>
        /// 加密方法    
        /// </summary>
        /// <param name="pToEncrypt">明文密码</param>
        /// <param name="sKey">加密关键字,8位</param>
        /// <returns>加密后密码</returns>
        public static string Encrypt(string pToEncrypt, string sKey)
        {

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            //把字符串放到byte数组中     

            //原来使用的UTF8编码，我改成Unicode编码了，不行     

            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);

            //byte[]     inputByteArray=Encoding.Unicode.GetBytes(pToEncrypt);     


            //建立加密对象的密钥和偏移量     

            //原文使用ASCIIEncoding.ASCII方法的GetBytes方法     

            //使得输入密码必须输入英文文本     

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);

            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            MemoryStream ms = new MemoryStream();

            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            //Write     the     byte     array     into     the     crypto     stream     

            //(It     will     end     up     in     the     memory     stream)     

            cs.Write(inputByteArray, 0, inputByteArray.Length);

            cs.FlushFinalBlock();

            //Get     the     data     back     from     the     memory     stream,     and     into     a     string     

            StringBuilder ret = new StringBuilder();

            foreach (byte b in ms.ToArray())
            {

                //Format     as     hex     

                ret.AppendFormat("{0:X2} ", b);

            }

            ret.ToString();

            return ret.ToString().Replace(" ", "");

        }

        /// <summary>
        /// 密码解密方法  
        /// </summary>
        /// <param name="pToDecrypt">加密后的密码</param>
        /// <param name="sKey">加密关键字</param>
        /// <returns>密码明文</returns>
        public static string Decrypt(string pToDecrypt, string sKey)
        {

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();


            //Put     the     input     string     into     the     byte     array     

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];

            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {

                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));

                inputByteArray[x] = (byte)i;

            }

            //建立加密对象的密钥和偏移量，此值重要，不能修改     

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);

            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            MemoryStream ms = new MemoryStream();

            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

            //Flush     the     data     through     the     crypto     stream     into     the     memory     stream     

            cs.Write(inputByteArray, 0, inputByteArray.Length);

            cs.FlushFinalBlock();

            //Get     the     decrypted     data     back     from     the     memory     stream     

            //建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象     

            StringBuilder ret = new StringBuilder();
            return System.Text.Encoding.Default.GetString(ms.ToArray());

        }

    }
}
