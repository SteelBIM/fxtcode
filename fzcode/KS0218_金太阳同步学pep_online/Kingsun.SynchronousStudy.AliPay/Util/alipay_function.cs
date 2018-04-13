using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using System;
using System.Collections;
using System.Xml;

namespace Kingsun.SynchronousStudy.AliPay.Util
{
    /// <summary>
    /// 功能：支付宝接口公用函数类
    /// 详细：该类是请求、通知返回两个文件所调用的公用函数核心处理文件，不需要修改
    /// 版本：3.0
    /// 修改日期：2010-06-13
    /// 说明：
    /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
    /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
    /// </summary>
    public class AlipayFunction
    {
        public AlipayFunction()
        {
        }

        /// <summary>
        /// 生成签名结果
        /// </summary>
        /// <param name="sArray">要加密的数组</param>
        /// <param name="key">安全校验码</param>
        /// <param name="sign_type">加密类型</param>
        /// <param name="_input_charset">编码格式</param>
        /// <returns>签名结果字符串</returns>
        public static string Build_mysign(ArrayList sArray, string key, string sign_type, string _input_charset)
        {
            string prestr = Create_linkstring(sArray);  //把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr = prestr.Substring(0, nLen - 1);

            prestr = prestr + key;                      //把拼接后的字符串再与安全校验码直接连接起来
            string mysign = Sign(prestr, sign_type, _input_charset);	//把最终的字符串加密，获得签名结果

            return mysign;
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string Create_linkstring(ArrayList sArray)
        {
            int nCount = sArray.Count;
            int i = 0;
            StringBuilder prestr = new StringBuilder();
            for (i = 0; i < nCount; i++)
            {
                prestr.Append(sArray[i].ToString() + "&");
            }

            return prestr.ToString();
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// 使用场景：GET方式请求时，对URL的中文进行编码
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string Create_linkstring_urlencode(ArrayList sArray)
        {
            int nCount = sArray.Count;
            int i = 0;
            StringBuilder prestr = new StringBuilder();
            for (i = 0; i < nCount; i++)
            {
                //把sArray的数组里的元素格式：变量名=值，分割开来
                int nPos = sArray[i].ToString().IndexOf('=');              //获得=字符的位置
                int nLen = sArray[i].ToString().Length;                    //获得字符串长度
                string itemName = sArray[i].ToString().Substring(0, nPos); //获得变量名
                string itemValue = "";                                     //获得变量的值
                if (nPos + 1 < nLen)
                {
                    itemValue = sArray[i].ToString().Substring(nPos + 1);
                }

                if (itemName != "service" && itemName != "_input_charset")
                {
                    prestr.Append(itemName + "=" + HttpUtility.UrlEncode(itemValue) + "&");
                }
                else
                {
                    prestr.Append(sArray[i].ToString() + "&");
                }
            }

            return prestr.ToString();
        }

        /// <summary>
        /// 除去数组中的空值和签名参数
        /// </summary>
        /// <param name="sArray">加密参数组</param>
        /// <returns>去掉空值与签名参数后的新加密参数组</returns>
        public static ArrayList Para_filter(ArrayList sArray)
        {
            int nCount = sArray.Count;
            int i;
            for (i = 0; i < nCount; i++)
            {
                //把sArray的数组里的元素格式：变量名=值，分割开来
                int nPos = sArray[i].ToString().IndexOf('=');              //获得=字符的位置
                int nLen = sArray[i].ToString().Length;                    //获得字符串长度
                string itemName = sArray[i].ToString().Substring(0, nPos); //获得变量名
                string itemValue = "";                                     //获得变量的值
                if (nPos + 1 < nLen)
                {
                    itemValue = sArray[i].ToString().Substring(nPos + 1);
                }

                if (itemName.ToLower() == "sign" || itemName.ToLower() == "sign_type" || itemValue == "" || itemName.ToLower() == "signtype")
                {
                    sArray.RemoveAt(i);
                    nCount--;
                    i--;
                }
            }

            return sArray;
        }

        /// <summary>
        /// 对数组排序
        /// </summary>
        /// <param name="sArray">排序前的数组</param>
        /// <returns>排序后的数组</returns>
        public static string[] Arg_sort(string[] sArray)
        {
            int i, j; //交换标志 
            string temp;

            bool exchange;

            for (i = 0; i < sArray.Length; i++) //最多做R.Length-1趟排序 
            {
                exchange = false; //本趟排序开始前，交换标志应为假

                for (j = sArray.Length - 2; j >= i; j--)
                {//交换条件
                    if (System.String.CompareOrdinal(sArray[j + 1], sArray[j]) < 0)
                    {
                        temp = sArray[j + 1];
                        sArray[j + 1] = sArray[j];
                        sArray[j] = temp;

                        exchange = true; //发生了交换，故将交换标志置为真 
                    }
                }

                if (!exchange) //本趟排序未发生交换，提前终止算法 
                {
                    break;
                }
            }
            return sArray;
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="prestr">需要加密的字符串</param>
        /// <param name="sign_type">加密类型</param>
        /// <param name="_input_charset">编码格式</param>
        /// <returns>加密结果</returns>
        public static string Sign(string prestr, string sign_type, string _input_charset)
        {
            StringBuilder sb = new StringBuilder(32);
            if (sign_type.ToUpper() == "MD5")
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(prestr));
                for (int i = 0; i < t.Length; i++)
                {
                    sb.Append(t[i].ToString("x").PadLeft(2, '0'));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 写日志，方便测试（看网站需求，也可以改成把记录存入数据库）
        /// </summary>
        /// <param name="sPath">日志的本地绝对路径</param>
        /// <param name="sWord">要写入日志里的文本内容</param>
        public static void log_result(string sPath, string sWord)
        {
            StreamWriter fs = new StreamWriter(sPath, false, System.Text.Encoding.Default);
            fs.Write(sWord);
            fs.Close();
        }

        /// <summary>
        /// 用于防钓鱼，调用接口query_timestamp来获取时间戳的处理函数
        /// 注意：远程解析XML出错，与IIS服务器配置有关
        /// </summary>
        /// <param name="partner">合作身份者ID</param>
        /// <returns>时间戳字符串</returns>
        public static string Query_timestamp(string partner)
        {
            string url = "https://mapi.alipay.com/gateway.do?service=query_timestamp&partner=" + partner;
            string encrypt_key = "";

            XmlTextReader Reader = new XmlTextReader(url);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Reader);

            encrypt_key = xmlDoc.SelectSingleNode("/alipay/response/timestamp/encrypt_key").InnerText;

            return encrypt_key;
        }

        
    }
}