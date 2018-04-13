using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kingsun.SynchronousStudy.Common
{
    public class PublicHelp
    {
        /// <summary>
        /// 公司唯一id
        /// </summary>
        /// <returns></returns>
        public static string OrgId
        {
            get
            {
                return "CourseActivate";
            }
        }

        public static string AgentId()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssffff");
        }

        #region  过滤sql特殊字符  public static string DelSQLStr(string str)
        /// <summary>
        /// 过滤sql特殊字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DelSQLStr(string str)
        {
            if (str == null || str == "")
                return "";
            str = str.Replace(";", "");
            str = str.Replace("'", "");
            str = str.Replace("&", "");
            str = str.Replace("%20", "");
            str = str.Replace("--", "");
            str = str.Replace("==", "");
            str = str.Replace("<", "");
            str = str.Replace(">", "");
            str = str.Replace("%", "");
            str = str.Replace("+", "");
            str = str.Replace("-", "");
            str = str.Replace("=", "");
            str = str.Replace(",", "");
            return str;
        }
        #endregion

        #region  MD5加密  string pswToSecurity(string strpsw)
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="strpsw">要加密的字符串</param>
        /// <returns>加密结果</returns>
        public static string pswToSecurity(string strpsw)
        {
            if (!string.IsNullOrEmpty(strpsw) && strpsw.Length != 0)
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strpsw, "MD5");
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        #region 字符串加密

        /// <summary>   
        /// 利用DES加密算法加密字符串（可解密）   
        /// </summary>   
        /// <param name="plaintext">被加密的字符串</param>   
        /// <param name="key">密钥（只支持8个字节的密钥）</param>   
        /// <returns>加密后的字符串</returns>   
        public static string EncryptString(string plaintext, string key)
        {
            //访问数据加密标准(DES)算法的加密服务提供程序 (CSP) 版本的包装对象   
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = ASCIIEncoding.ASCII.GetBytes(key);　//建立加密对象的密钥和偏移量   
            des.IV = ASCIIEncoding.ASCII.GetBytes(key);　 //原文使用ASCIIEncoding.ASCII方法的GetBytes方法   

            byte[] inputByteArray = Encoding.Default.GetBytes(plaintext);//把字符串放到byte数组中   

            MemoryStream ms = new MemoryStream();//创建其支持存储区为内存的流　   
            //定义将数据流链接到加密转换的流   
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //上面已经完成了把加密后的结果放到内存中去   
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        /// <summary>   
        /// 利用DES解密算法解密密文（可解密）   
        /// </summary>   
        /// <param name="ciphertext">被解密的字符串</param>   
        /// <param name="key">密钥（只支持8个字节的密钥，同前面的加密密钥相同）</param>   
        /// <returns>返回被解密的字符串</returns>   
        public static string DecryptString(string ciphertext, string key)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                byte[] inputByteArray = new byte[ciphertext.Length / 2];
                for (int x = 0; x < ciphertext.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(ciphertext.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }

                des.Key = ASCIIEncoding.ASCII.GetBytes(key);　//建立加密对象的密钥和偏移量，此值重要，不能修改   
                des.IV = ASCIIEncoding.ASCII.GetBytes(key);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

                cs.Write(inputByteArray, 0, inputByteArray.Length);

                cs.FlushFinalBlock();

                //建立StringBuild对象，createDecrypt使用的是流对象，必须把解密后的文本变成流对象   
                StringBuilder ret = new StringBuilder();

                return System.Text.Encoding.Default.GetString(ms.ToArray());
            }
            catch (Exception)
            {
                return "error";
            }
        }

        #endregion

        /// <summary>
        /// 将数字转成日期类型
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static DateTime ConvertIntDateTime(double d)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddSeconds(d);
            return time;
        }
        ///
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        ///
        /// 时间
        /// double
        public static double ConvertDateTimeInt(System.DateTime time)
        {
            double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = (time - startTime).TotalSeconds;
            return intResult;
        }
        public static string Percentage(decimal? number)
        {
            if (number > 0)
            {
                return number * 100 + "%";
            }
            return "";
        }
        /// <summary>
        /// 产品渠道来源
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public static string ProductName(int? channel)
        {
            if (channel == 1)
            {
                return "同步学";
            }
            if (channel == 2)
            {
                return "C++客户端";
            }
            return channel.ToString();
        }

        /// <summary> 
        /// DataSet装换为泛型集合 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="ds">DataSet</param> 
        /// <param name="tableIndex">待转换数据表索引</param> 
        /// <returns></returns> 
        public static List<T> DataSetToIList<T>(DataSet ds, int tableIndex)
        {
            if (ds == null || ds.Tables.Count < 0)
                return null;
            if (tableIndex > ds.Tables.Count - 1)
                return null;
            if (tableIndex < 0)
                tableIndex = 0;

            DataTable p_Data = ds.Tables[tableIndex];
            // 返回值初始化 
            List<T> result = new List<T>();
            for (int j = 0; j < p_Data.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < p_Data.Columns.Count; i++)
                    {
                        // 属性与字段名称一致的进行赋值 
                        if (pi.Name.Equals(p_Data.Columns[i].ColumnName))
                        {
                            // 数据库NULL值单独处理 
                            if (p_Data.Rows[j][i] != DBNull.Value)
                                pi.SetValue(_t, p_Data.Rows[j][i], null);
                            else
                                pi.SetValue(_t, null, null);
                            break;
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }

        /// <summary>
        /// 是否是手机号码
        /// </summary>
        /// <param name="val"></param>
        public static bool IsMobile(string val)
        {
            return Regex.IsMatch(val, @"^(1[358]\d{9}|17[0678]\d{8})$", RegexOptions.IgnoreCase);
            //  return Regex.IsMatch(val, @"^1[34578]\d{9}$", RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <returns></returns>
        public static string Number(int Length)
        {
            string result = "";
            Random random = new Random();
            for (int i = 0; i < Length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }
        #region 判断是否为空
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool JudgmentIsEmpty(object obj)
        {
            //判断是否为空
            if (obj != null)
            {
                if (!string.IsNullOrEmpty(obj.ToString()))
                    return false;
            }
            return true;
        }
        #endregion
    }
}
