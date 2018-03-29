using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace SqlTCP
{
	/// <summary>
	/// �ṩ���ܡ��������ݿ������ַ�����MD5��������ķ���
	/// </summary>
	public class ConnectionInfo 
	{

		public static string Encrypt(string pToEncrypt, string sKey)
		{
			try
			{
				DESCryptoServiceProvider des = new	DESCryptoServiceProvider();
				byte[] inputByteArray =  Encoding.Default.GetBytes (pToEncrypt);
				//�������ܶ������Կ��ƫ����
				//ԭ��ʹ��ASCIIEncoding.ASCII������GetBytes����
				//ʹ�����������������Ӣ���ı�
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
				//�������ܶ������Կ��ƫ��������ֵ��Ҫ�������޸�
				des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
				des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
				MemoryStream ms = new MemoryStream();
				CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(),CryptoStreamMode.Write);
				cs.Write(inputByteArray, 0,	inputByteArray.Length);
				cs.FlushFinalBlock();

				//����StringBuild����CreateDecryptʹ�õ��������󣬱���ѽ��ܺ���ı����������
				StringBuilder ret = new StringBuilder();
				return System.Text.Encoding.Default.GetString(ms.ToArray());
			}
			catch{return "";}
		}
		/// <summary>
		/// ���������MD5 16λ�ķ�ʽ����
		/// </summary>
		/// <param name="Pwd">�������ַ�</param>
		/// <returns>���ؼ��ܺ���ַ�</returns>
		public static string MD5(string toCryString)
		{
			//return   FormsAuthentication.HashPasswordForStoringInConfigFile(Pwd,"md5").Substring(8,16).ToLower();		
			MD5CryptoServiceProvider hashmd5;
			hashmd5 = new MD5CryptoServiceProvider();
			return BitConverter.ToString(hashmd5.ComputeHash(Encoding.Default.GetBytes(toCryString))).Replace("-","").ToLower();//asp��Сд,�������ַ�
		}
	}
}
