using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

/*作者:李晓东
 * 摘要:
 *      2014.02.20  修改人:李晓东
 *                  新建  EntranceHelper 
 * 
 * */
namespace FxtService.Common
{
    /// <summary>
    /// API入口帮助类
    /// </summary>
    public class EntranceHelper
    {
        /// <summary>
        /// 得到验证码
        /// </summary>
        /// <returns></returns>
        public string GetCode(string strCode)
        {
            string strDay = DateTime.Now.ToString("yyyy-MM-dd");
            string strDayReverse = string.Empty;
            IEnumerable<char> iableReverse = strDay.Reverse();
            foreach (char chReverse in iableReverse) {
                strDayReverse += chReverse;
            }
            return GetMd5(string.Format("{0}{1}{2}", strDay, strCode, strDayReverse));
        }
        /// <summary>
        /// 进行MD5效验
        /// </summary>
        /// <param name="strmd5"></param>
        /// <returns></returns>
        string GetMd5(string strmd5)
        {
            byte[] md5Bytes = ASCIIEncoding.Default.GetBytes(strmd5);
            byte[] encodedBytes;
            MD5 md5 = new MD5CryptoServiceProvider();
            encodedBytes = md5.ComputeHash(md5Bytes);
            string nn = BitConverter.ToString(encodedBytes);
            nn = Regex.Replace(nn, "-", "");//因为转化完的都是34-2d这样的，所以替换掉- 
            nn = nn.ToLower();//根据需要转化成小写
            return nn;
        }

        public MatchClass GetMatchClass(string key)
        {
           XmlNode xml = Utility.ApiConfig.SelectSingleNode("/Match/Class[@Key='" + key + "']");
            string library = xml.Attributes["Library"].Value;
            string className = xml.Attributes["ClassName"].Value;
            return new MatchClass() { Key = key, ClassName = className, Library = library };
        }

    }
    public class MatchClass
    {
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 类库名称
        /// </summary>
        public string Library { get; set; }
        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }
    }


    //公司角色
    public enum EnumCustomerType
    {
        /// <summary>
        /// 房迅通
        /// </summary>
        Company_Fxt = 1,
        /// <summary>
        /// 银行
        /// </summary>
        Company_Bank = 2001013,
        /// <summary>
        /// 评估机构
        /// </summary>
        Company_Soa = 2001010,
    }

}
