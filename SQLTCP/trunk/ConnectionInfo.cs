using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace SqlTCP
{
	/// <summary>
	/// 提供加密、解密数据库连接字符串和MD5加密密码的方法
	/// </summary>
	public class ConnectionInfo 
	{

		public static string Encrypt(string pToEncrypt, string sKey)
		{
			try
			{
				DESCryptoServiceProvider des = new	DESCryptoServiceProvider();
				byte[] inputByteArray =  Encoding.Default.GetBytes (pToEncrypt);
				//建立加密对象的密钥和偏移量
				//原文使用ASCIIEncoding.ASCII方法的GetBytes方法
				//使得输入密码必须输入英文文本
				des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
				des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
				MemoryStream ms = new MemoryStream();
				CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(),CryptoStreamMode.Write);
				cs.Write(inputByteArray, 0,
					inputByteArray.Length);
				cs.FlushFinalBlock();
				StringBuilder ret = new StringBuilder();
				foreach(byte b in ms.ToArray())
				{
					ret.AppendFormat("{0:X2}", b);
				}
				ret.ToString();
				return ret.ToString();
			}
			catch{return "";}
		}

		public static string Decrypt(string pToDecrypt, string sKey)
		{
			try
			{
				DESCryptoServiceProvider des = new
					DESCryptoServiceProvider();
				byte[] inputByteArray = new byte
					[pToDecrypt.Length / 2];
				for(int x = 0; x < pToDecrypt.Length / 2; x++)
				{
					int i = (Convert.ToInt32
						(pToDecrypt.Substring(x * 2, 2), 16));
					inputByteArray[x] = (byte)i;
				}
				//建立加密对象的密钥和偏移量，此值重要，不能修改
				des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
				des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
				MemoryStream ms = new MemoryStream();
				CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(),CryptoStreamMode.Write);
				cs.Write(inputByteArray, 0,	inputByteArray.Length);
				cs.FlushFinalBlock();

				//建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象
				StringBuilder ret = new StringBuilder();
				return System.Text.Encoding.Default.GetString(ms.ToArray());
			}
			catch{return "";}
		}
		/// <summary>
		/// 将密码进行MD5 16位的方式加密
		/// </summary>
		/// <param name="Pwd">待加密字符</param>
		/// <returns>返回加密后的字符</returns>
		public static string MD5(string toCryString)
		{
			//return   FormsAuthentication.HashPasswordForStoringInConfigFile(Pwd,"md5").Substring(8,16).ToLower();		
			MD5CryptoServiceProvider hashmd5;
			hashmd5 = new MD5CryptoServiceProvider();
			return BitConverter.ToString(hashmd5.ComputeHash(Encoding.Default.GetBytes(toCryString))).Replace("-","").ToLower();//asp是小写,把所有字符
		}
	}
}
