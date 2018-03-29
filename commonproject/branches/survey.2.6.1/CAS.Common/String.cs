﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using CAS.Entity.DBEntity;
using System.IO;
using System.Collections;

namespace CAS.Common
{
    public class StringHelper
    {
        public static string EncryptString = "d&52-M0@x*";
        /// <summary>
        /// string转化为int, false:返回 0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int TryGetInt(string value)
        {
            int n = 0;
            int result = 0;
            if (int.TryParse(value, out n) && n <= Int32.MaxValue)
            {
                result = Convert.ToInt32(n);
            }
            return result;
        }
        /// <summary>
        /// 获取int类型 返回object
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object TryGetIntObject(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                int n = 0;
                int result = 0;
                if (int.TryParse(value, out n) && n <= Int32.MaxValue)
                {
                    result = Convert.ToInt32(n);
                }
                return result;
            }
            else
            {
                return null;
            }
        }


        public static short TryGetShort(string value)
        {
            short n = 0;
            short result = 0;
            if (short.TryParse(value, out n) && n <= Int16.MaxValue)
            {
                result = Convert.ToInt16(n);
            }
            return result;
        }

        /// <summary>
        /// 获取short类型返回object
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object TryGetShortObject(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                short n = 0;
                short result = 0;
                if (short.TryParse(value, out n) && n <= Int16.MaxValue)
                {
                    result = Convert.ToInt16(n);
                }
                return result;
            }
            else
            {
                return null;
            }
        }

        public static byte TryGetByte(string value)
        {
            byte result = 0;
            byte.TryParse(value, out result);
            return result;
        }

        public static bool TryGetBool(string value)
        {
            bool result = false;
            if (value == "1") return true;
            bool.TryParse(value, out result);
            return result;
        }
        /// <summary>
        /// 转换布尔
        /// </summary>
        /// <param name="value">判断的值</param>
        /// <param name="defaultValue">默认值</param>
        /// <remarks>侯湘岳</remarks>
        /// <returns></returns>
        public static bool TryGetBool(string value, bool defaultValue)
        {
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            value = value.ToUpper();
            return (value == "YES" || value == "TRUE" || value == "1");
        }

        public static long TryGetLong(string value)
        {
            long n = 0;
            long result = 0;
            if (long.TryParse(value, out n))
            {
                result = Convert.ToInt64(n);
            }
            return result;
        }

        public static object TryGetLongObject(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                long n = 0;
                long result = 0;
                if (long.TryParse(value, out n))
                {
                    result = Convert.ToInt64(n);
                }
                return result;
            }
            else
            {
                return null;
            }
        }

        public static float TryGetFloat(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace(",", "");
            }

            float result = 0;
            float.TryParse(value, out result);
            return result;
        }

        /// <summary>
        /// 获取Float类型返回Object类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object TryGetFloatObject(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace(",", "");
                float result = 0;
                float.TryParse(value, out result);
                return result;
            }
            else
            {
                return null;
            }
        }

        public static double TryGetDouble(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace(",", "");
            }
            double result = 0;
            double.TryParse(value, out result);
            return result;
        }
        /// <summary>
        /// 获取double类型返回object类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object TryGetDoubleObject(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace(",", "");
                double result = 0;
                double.TryParse(value, out result);
                return result;
            }
            else
            {
                return null;
            }
        }

        public static DateTime TryGetDateTime(string value)
        {
            DateTime result = default(DateTime);
            DateTime.TryParse(value, out result);
            return result;
        }

        /// <summary>
        /// 保留小数位数
        /// byte
        /// </summary>
        /// <param name="d">数值</param>
        /// <param name="decimals">保留几位小数位数</param>
        /// <param name="roundOff">是否四舍五入</param>
        /// <returns></returns>
        public static string RoundCorrect(double d, int decimals, bool roundOff)
        {
            double multiplier = Math.Pow(10, decimals), result;

            if (d < 0)
                multiplier *= -1;
            if (roundOff)
                result = Math.Floor((d * multiplier) + 0.51) / multiplier;
            else
                result = Math.Floor((d * multiplier)) / multiplier;
            return result.ToString("f" + decimals.ToString());
        }

        public class DateTimeFormat
        {
            public static string yyyyMMdd = "yyyyMMdd";
            public static string yyyy_MM_dd = "yyyy-MM-dd";
            public static string yyyyMM = "yyyyMM";
            public static string yyyy_MM_dd_hh_mm_ss = "yyyy-MM-dd HH:mm:ss";
        }

        /// <summary>
        /// 将datetime转化为string,格式为：yyyy-MM-dd
        /// </summary>
        public static string TryGetDateTimeFormat(object value, DateTime dt)
        {
            string format = DateTimeFormat.yyyy_MM_dd;
            return TryGetDateTimeFormat(value, format, dt);
        }
        /// <summary>
        /// 获取当前天的最后时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetCurDayLastTime(DateTime dt)
        {
            string str = " 23:59:59";
            str = dt.ToString("yyyy-MM-dd") + str;
            return Convert.ToDateTime(str);
        }
        /// <summary>
        /// 将datetime转化为string可以自定义格,格式类：DateTimeFormat
        /// </summary>
        public static string TryGetDateTimeFormat(object value, string format, DateTime dt)
        {
            string result;
            if (value == null || Convert.ToString(value) == "")
            {
                result = dt.ToString(format);
            }
            else
            {
                try
                {
                    result = Convert.ToDateTime(value).ToString(format);
                }
                catch { result = ""; }
            }
            return result;
        }

        public static decimal TryGetDecimal(string value)
        {
            decimal result = default(decimal);
            decimal.TryParse(value, out result);
            return result;
        }
        /// <summary>
        /// 获取Decimal类型 
        /// </summary>
        /// <param name="value">输入值</param>
        /// <returns>如果没有值返回null</returns>
        public static object TryGetDecimalObject(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            else
            {
                decimal result = default(decimal);
                decimal.TryParse(value, out result);
                return result;
            }
        }

        /// <summary>
        /// 获得第几周：0-当前日期是第几周。n-相隔N周是第几周
        /// </summary>
        public static string TryGetFewWeeks(object value, DateTime dt, int apart)
        {
            string result;
            if (value == null)
            {
                DateTime time = dt.AddDays(-(apart * 7));
                string year = time.Year.ToString();
                int weeknow = Convert.ToInt32(time.DayOfWeek);//今天星期几
                int daydiff = (-1) * (weeknow + 1);//今日与上周末的天数差
                int days = time.AddDays(daydiff).DayOfYear;//上周末是本年第几天
                int weeks = days / 7;
                if (days % 7 != 0)
                {
                    weeks++;
                }
                string week = Convert.ToString(weeks + 1);
                if (week.Length == 1)
                {
                    week = "0" + week;
                }
                result = year + week;
            }
            else
            {
                result = value.ToString();
            }
            return result;
        }

        /// <summary>
        /// 获得处于第几季：0-上季是第几季。n-相隔N季
        /// </summary>
        public static string TryGetFewQuarter(object value, DateTime dt)
        {
            string result;
            if (value == null)
            {
                string quarter = "";
                int spare = Convert.ToInt32(dt.Month) % 3;
                int divide = Convert.ToInt32(dt.Month) / 3;
                if ((spare != 0 && divide == 0) || (spare == 0 && divide == 1))
                {
                    quarter = dt.AddYears(-1).Year + "04";
                }
                else if (spare == 0 && divide > 1)
                {
                    quarter = dt.Year + "0" + Convert.ToString(divide - 1);
                }
                else if (spare != 0 && divide > 1)
                {
                    quarter = dt.Year + "0" + Convert.ToString(divide);
                }
                result = quarter;
            }
            else
            {
                result = value.ToString();
            }

            return result;
        }

        /// <summary>
        /// 获取转码的文字信息
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string TryGetDecodeStr(object o)
        {
            if (o == null)
            {
                return "";
            }
            else
            {
                return HttpUtility.UrlDecode(o.ToString());
            }
        }
        /// <summary>
        /// 获取相似楼盘名
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static string LikeProjectName(string projectName)
        {
            string projectname = projectName;
            if (projectName.Length < 3) return projectname;
            int index_1 = projectName.IndexOf("·", 2);
            int index_2 = projectName.IndexOf(".", 2);
            int index_3 = projectName.IndexOf("第", 2);
            int index_4 = projectName.IndexOf("(", 2);
            int index_5 = projectName.IndexOf("（", 2);
            int index_6 = projectName.IndexOf("[", 2);
            int index_7 = projectName.IndexOf("一", 2);
            int index_8 = projectName.IndexOf("二", 2);
            int index_9 = projectName.IndexOf("三", 2);
            int index_10 = projectName.IndexOf("四", 2);
            int index_11 = projectName.IndexOf("五", 2);
            int index_12 = projectName.IndexOf("六", 2);
            int index_13 = projectName.IndexOf("七", 2);
            int index_14 = projectName.IndexOf("八", 2);
            int index_15 = projectName.IndexOf("九", 2);
            int index_16 = projectName.IndexOf("十", 2);
            int index_17 = projectName.IndexOf("1", 2);
            int index_18 = projectName.IndexOf("2", 2);
            int index_19 = projectName.IndexOf("3", 2);
            int index_20 = projectName.IndexOf("4", 2);
            int index_21 = projectName.IndexOf("5", 2);
            int index_22 = projectName.IndexOf("6", 2);
            int index_23 = projectName.IndexOf("7", 2);
            int index_24 = projectName.IndexOf("8", 2);
            int index_25 = projectName.IndexOf("9", 2);
            int index_26 = projectName.IndexOf("_", 2);
            int index_27 = projectName.IndexOf("-", 2);
            int index = projectName.Length;
            if (index_1 > 0) index = index_1;
            if (index_2 > 0 && index_2 < index) index = index_2;
            if (index_3 > 0 && index_3 < index) index = index_3;
            if (index_4 > 0 && index_4 < index) index = index_4;
            if (index_5 > 0 && index_5 < index) index = index_5;
            if (index_6 > 0 && index_6 < index) index = index_6;
            if (index_7 > 0 && index_7 < index) index = index_7;
            if (index_8 > 0 && index_8 < index) index = index_8;
            if (index_9 > 0 && index_9 < index) index = index_9;
            if (index_10 > 0 && index_10 < index) index = index_10;
            if (index_11 > 0 && index_11 < index) index = index_11;
            if (index_12 > 0 && index_12 < index) index = index_12;
            if (index_13 > 0 && index_13 < index) index = index_13;
            if (index_14 > 0 && index_14 < index) index = index_14;
            if (index_15 > 0 && index_15 < index) index = index_15;
            if (index_16 > 0 && index_16 < index) index = index_16;
            if (index_17 > 0 && index_17 < index) index = index_17;
            if (index_18 > 0 && index_18 < index) index = index_18;
            if (index_19 > 0 && index_19 < index) index = index_19;
            if (index_20 > 0 && index_20 < index) index = index_20;
            if (index_21 > 0 && index_21 < index) index = index_21;
            if (index_22 > 0 && index_22 < index) index = index_22;
            if (index_23 > 0 && index_23 < index) index = index_23;
            if (index_24 > 0 && index_24 < index) index = index_24;
            if (index_25 > 0 && index_25 < index) index = index_25;
            if (index_26 > 0 && index_26 < index) index = index_26;
            if (index_27 > 0 && index_27 < index) index = index_27;
            projectname = projectName.Substring(0, index);
            return projectname;
        }

        /// <summary>
        /// 获取税费条件
        /// </summary>
        /// <param name="PurposeCode">物业用途</param>
        /// <param name="RightTypeCode">产权形式</param>
        /// <param name="fullFiveYears">是否满五年</param>
        /// <param name="onlySpace">是否唯一用房</param>
        /// <param name="firstBuy">是否首次购买</param>
        /// <returns></returns>
        public static string GetTaxSelect(string purposeCode, string rightTypeCode, string fullFiveYears, string onlySpace, string firstBuy)
        {
            string TaxSelect = null;
            Dictionary<string, string> houseUse = new Dictionary<string, string>();
            houseUse.Add("1002001", "0"); houseUse.Add("1002003", "0"); houseUse.Add("1002012", "0");
            houseUse.Add("1002013", "0"); houseUse.Add("1002023", "0"); houseUse.Add("1002024", "0");
            houseUse.Add("1002002", "1"); houseUse.Add("1002004", "1"); houseUse.Add("1002005", "1");
            houseUse.Add("1002006", "1"); houseUse.Add("1002007", "1"); houseUse.Add("1002008", "1");
            houseUse.Add("1002009", "1"); houseUse.Add("1002010", "1"); houseUse.Add("1002011", "1");
            houseUse.Add("1002014", "2"); houseUse.Add("1002015", "2"); houseUse.Add("1002016", "2");
            houseUse.Add("1002017", "2"); houseUse.Add("1002018", "2"); houseUse.Add("1002019", "2");
            houseUse.Add("1002020", "2"); houseUse.Add("1002021", "2");
            houseUse.Add("1002022", "2"); houseUse.Add("1002025", "2");
            if (purposeCode != null || purposeCode.Length > 0)
            {
                if (houseUse.ContainsKey(purposeCode))
                {
                    TaxSelect = houseUse[purposeCode];
                }
                else
                {
                    TaxSelect = purposeCode;
                }
            }
            TaxSelect += rightTypeCode + fullFiveYears + onlySpace + firstBuy;
            return TaxSelect;
        }


        /// <summary>
        /// 指定长度随机数字字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RndNumberString(int length)
        {
            Random rnd = new Random();
            string returnstring = "";
            int i = 1;
            while (i <= length)
            {
                returnstring += rnd.Next(0, 9).ToString();
                i++;
            }
            return returnstring;
        }

        /// <summary>
        /// 获取询价单编号
        /// </summary>
        /// <returns></returns>
        public static string GetFileNo(string fileno)
        {
            string sguid = Guid.NewGuid().ToString().ToUpper();
            sguid = sguid.Substring(sguid.Length - 8, 8);
            DateTime dt = DateTime.Now;
            sguid = fileno + "[" + dt.Year + "]" + dt.Month + sguid;
            return sguid;
        }

        /// <summary>
        /// 获取4位验证码
        /// </summary>
        /// <returns></returns>
        public static string GetCode()
        {
            string veriCode = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
            return veriCode = veriCode.Substring(veriCode.Length - 6, 4);
        }

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

        public static string GetMD5(string str)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "md5");
        }

        public static string GetHttpUrl(string url)
        {
            return "http://" + url + "/";
        }

        /// <summary>
        /// 时间差
        /// </summary>
        /// <param name="DateTime1"></param>
        /// <param name="DateTime2"></param>
        /// <returns></returns>
        public static string DateDiff(DateTime DateTime1, DateTime DateTime2, string type)
        {
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            string val = "";
            switch (type)
            {
                case "d":
                    if ((ts.TotalDays / 365) > 1)
                    {
                        val = (ts.TotalDays % 365).ToString("0") + "月";
                    }
                    else if ((ts.TotalDays / 30) > 1)
                    {
                        val = (ts.TotalDays % 30).ToString("0") + "月";
                    }
                    else if (ts.TotalDays > 1)
                        val = ts.TotalDays.ToString("0") + "天";
                    else if (ts.TotalHours > 1)
                        val = ts.TotalHours.ToString("0") + "小时";
                    else if (ts.TotalMinutes > 1)
                        val = ts.TotalMinutes.ToString("0") + "分钟";
                    else if (ts.TotalSeconds > 1)
                        val = ts.TotalSeconds.ToString("0") + "秒";
                    break;
                case "tm": //totalminutes
                    val = ts.TotalMinutes.ToString("0");
                    break;
                default:
                    val = ts.Days.ToString() + "天"
                 + ts.Hours.ToString() + "小时"
                 + ts.Minutes.ToString() + "分钟"
                 + ts.Seconds.ToString() + "秒";
                    break;
            }
            return val;
        }
        /// <summary>
        /// 将table转为string数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fieldname"></param>
        /// <returns></returns>
        public static string[] tableReturnString(DataTable dt, string fieldname)
        {
            string[] arrString = new string[dt.Rows.Count];
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    arrString[i] = dt.Rows[i][fieldname].ToString();
                }
                return arrString;
            }
            else
            {
                return arrString;
            }
        }
        /// <summary>
        /// 从url中移除指定参数 kevin
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string RemoveUrlParameters(string url, params string[] parameters)
        {
            foreach (string para in parameters)
            {
                Regex reg = new Regex(@"(?:^|\?|&)" + para + @"=([^&]*)", RegexOptions.IgnoreCase);
                url = reg.Replace(url, "");
            }
            if (url.IndexOf("&") >= 0 && url.IndexOf("?") < 0)
            {
                url = url.Substring(0, url.IndexOf("&")) + "?" + url.Substring(url.IndexOf("&") + 1);
            }
            return url;
        }

        public static string LetterReplace(string letter)
        {

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["A"] = "原购价"; dic["B"] = "现评估总值"; dic["C"] = "建筑面积"; dic["D"] = "营业税";
            dic["E"] = "城建税"; dic["F"] = "教育附加税"; dic["G"] = "印花税"; dic["H"] = "契税";
            dic["I"] = "处置费用"; dic["J"] = "交易手续费"; dic["K"] = "土地增值税"; dic["L"] = "所得税";
            foreach (KeyValuePair<string, string> item in dic)
            {
                letter = letter.Replace(item.Key, item.Value);
            }
            if (letter == "0")
            {
                letter = "免征";
            }
            return letter;
        }

        public static string Html2Text(string content)
        {
            return Html2Text(content, 0);
        }

        public static string Html2Text(string content, int length)
        {
            string result = string.Empty;
            RegexOptions common = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline;
            Regex imgRegex = new Regex(@"<img[^<]*\/?>?$|<img[^<]*>", common);
            content = imgRegex.Replace(content, "[图片]");
            Regex textRegex = new Regex(@">[^>]+<|>[^>]+<?$", common);//取><之间的文本值
            Regex tagRegex = new Regex(@"<", common);
            MatchCollection mc = textRegex.Matches(content);
            if (0 < mc.Count)
            {
                StringBuilder temp = new StringBuilder();
                Regex bracketsRegex = new Regex(@">|<", common);
                foreach (Match match in mc)
                {
                    temp.Append(match.Value);
                }
                result = bracketsRegex.Replace(temp.ToString(), "");
            }
            else if (0 < tagRegex.Matches(result).Count)
                result = "";  //全部为html标签
            else
                result = content;
            return 0 < length && result.Length > length ? result.Substring(0, 50) : result;
        }

        public static string Transform(string condition)
        {
            char[] condit = new char[5];
            condit = condition.ToCharArray();
            string[] str = new string[5];

            switch (condit[0])
            {
                case '0':
                    str[0] = "普通住宅;"; break;
                case '1':
                    str[0] = "非普通住宅;"; break;
                default:
                    str[0] = "非住宅;"; break;
            }
            switch (condit[1])
            {
                case '0':
                    str[1] = "个人;"; break;
                case '1':
                    str[1] = "企业;"; break;
                default:
                    str[1] = ""; break;
            }
            switch (condit[2])
            {
                case '0':
                    str[2] = "未满五年;"; break;
                case '1':
                    str[2] = "已满五年;"; break;
                default:
                    str[2] = ""; break;
            }
            switch (condit[3])
            {
                case '0':
                    str[3] = "非首次购房;"; break;
                case '1':
                    str[3] = "首次购房;"; break;
                default:
                    str[3] = ""; break;
            }
            switch (condit[4])
            {
                case '0':
                    str[4] = "非唯一生活用房;"; break;
                case '1':
                    str[4] = "唯一生活用房;"; break;
                default:
                    str[4] = ""; break;
            }
            return str[0] + str[1] + str[2] + str[3] + str[4];
        }

        /// <summary>
        /// 数字转千分为货币数字(保留小数点后数字)
        /// </summary>
        /// <param name="strBumeric"></param>
        /// <returns></returns>
        public static string NumericToPrice(string strBumeric)
        {
            return NumericToPrice(strBumeric, true);
        }

        /// <summary>
        /// 数字转千分为货币数字（caoq 2013-8-21）
        /// </summary>
        /// <param name="strBumeric"></param>
        /// <param name="containDecimal">是否保留小数</param>
        /// <returns></returns>
        public static string NumericToPrice(string strBumeric, bool containDecimal)
        {
            string num = strBumeric;
            if (strBumeric.IndexOf(".") != -1)
            {
                num = strBumeric.Split('.')[0];
            }
            if (num.Length > 3)
            {
                int ilen = num.Length - 3;
                for (int i = ilen; i > 0; i++)
                {
                    num = num.Insert(ilen, ",");
                    ilen -= 3;
                    if (ilen <= 0) break;
                }
            }
            if (strBumeric.IndexOf(".") != -1 && containDecimal)
            {
                if (TryGetInt(strBumeric.Split('.')[1]) != 0)//如果为0不显示小数点后面数字
                    num = num + "." + TryGetInt(strBumeric.Split('.')[1]);//数字转字符，去掉小数点后无效0
            }
            return num;
        }
        /// <summary>
        /// 数字转千分为货币数字 (gch 2014-06-27)
        /// </summary>
        /// <param name="strBumeric">字符串数据</param>
        /// <param name="decimals">保留小数位数</param>
        /// <param name="roundOff">是否四舍五入</param>
        /// <returns></returns>
        public static string NumericToPrice(string strBumeric, int decimals, bool roundOff)
        {
            string num = strBumeric;
            if (strBumeric.IndexOf(".") != -1)
            {
                num = strBumeric.Split('.')[0];
            }
            if (num.Length > 3)
            {
                int ilen = num.Length - 3;
                for (int i = ilen; i > 0; i++)
                {
                    num = num.Insert(ilen, ",");
                    ilen -= 3;
                    if (ilen <= 0) break;
                }
            }
            if (strBumeric.IndexOf(".") != -1)
            {
                //if (TryGetInt(strBumeric.Split('.')[1]) != 0)//如果为0不显示小数点后面数字
                num = num + "." + strBumeric.Split('.')[1];//数字转字符，去掉小数点后无效0
            }
            return num;
        }
        private static char[] constant = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        /// <summary>
        /// 取随机字符串。只含数字及小写字母
        /// </summary>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string GenerateRandomString(int length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(74);
            Random rd = new Random();
            int len = constant.Length;
            for (int i = 0; i < length; i++)
            {
                newRandom.Append(constant[rd.Next(len)]);
            }
            return newRandom.ToString();
        }

        /// <summary>
        /// 字符是否以数字或字母结尾
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool EndWithNumOrAlphabet(string value)
        {
            Regex myRegex = new Regex("[A-Za-z0-9]$");
            if (value.Length == 0)
            {
                return false;
            }
            return myRegex.IsMatch(value);
        }

        /// <summary>
        /// 文件名不能有特殊符号,去掉特殊字符后的文件名 \/:*?"<>|@#&[]
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetFileName(string filename)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            //特殊字符 \/:*?"<>|@#&[]
            dic["\\"] = ""; dic["/"] = ""; dic[":"] = ""; dic["*"] = ""; dic["?"] = "";
            dic["\""] = ""; dic["<"] = ""; dic[">"] = ""; dic["|"] = ""; dic["@"] = "";
            dic["#"] = ""; dic["&"] = ""; dic["["] = ""; dic["]"] = "";
            foreach (KeyValuePair<string, string> item in dic)
            {
                filename = filename.Replace(item.Key, item.Value);
            }
            return filename;
        }

        /// <summary>
        /// 获得Appwd
        /// </summary>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static string GetAppwdMd5(string pwd)
        {
            string strmd5 = pwd + ConstCommon.WcfAppwdMd5Key;
            strmd5 = EncryptHelper.GetMd5(strmd5);
            return strmd5;
        }

        /// <summary>
        /// 获得登录Api中的Code,即对时间加密
        /// </summary>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static string GetLoginCodeMd5(string pwd)
        {
            string strmd5 = pwd + ConstCommon.WcfLoginMd5Key;
            strmd5 = EncryptHelper.GetMd5(strmd5);
            return strmd5;
        }

        /// <summary>
        /// 获得加密后的密码
        /// </summary>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static string GetPassWordMd5(string pwd)
        {
            string strmd5 = pwd + ConstCommon.WcfPassWordMd5Key;
            strmd5 = EncryptHelper.GetMd5(strmd5);
            return strmd5;
        }

        /// <summary>
        /// 获得Guid，不包含-；例：ece4f4a60b764339b94a07c84e338a27
        /// </summary>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static string GetPasCode()
        {
            string pasCode = Guid.NewGuid().ToString("N");

            return pasCode;
        }

        /// <summary>
        /// 货币显示，千分位
        /// </summary>
        /// <param name="o">金额</param>
        /// <param name="unit">单位</param>
        /// <param name="isnullResult">传入值为null,显示结果</param>
        /// <returns></returns>
        public static string DisplayCurrency(Object o, string unit, string isnullResult)
        {
            string result = Convert.ToString(o);
            if (string.IsNullOrEmpty(result))
            {
                return isnullResult;
            }
            else
            {
                return NumericToPrice(result, true) + unit;
            }
        }
        /// <summary>
        /// Html转为实体
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlToString(string html)
        {
            html = html.Replace("&amp;", "&");
            html = html.Replace("&lt;", "<");
            html = html.Replace("&gt;", ">");
            html = html.Replace("&quot;", "\"");
            html = html.Replace("&nbsp;", " ");
            html = html.Replace("&copy;", "©");
            html = html.Replace("&reg;", "®");
            html = html.Replace("&qult;", "“");
            html = html.Replace("&qurt;", "”");

            return html;
        }


        /// <summary>
        /// <![CDATA[
        /// 侯湘岳]]>
        /// </summary>
        public static string GetGridColModel(List<DatDisplayColumn> list)
        {
            StringBuilder result = new StringBuilder();
            result.Append("[\n");
            int currentIndex = 0;
            foreach (var obj in list)
            {
                if (0 == currentIndex)
                {
                    result.Append("{");
                    result.Append(" display: \"" + obj.displayname + "\"");
                    result.Append(" ,name: \"" + obj.propertyname + "\"");
                    result.Append(" ,width: " + obj.displaywidth.ToString() + "");
                    result.Append(" ,sortable: true");
                    result.Append(" ,align: \"" + obj.displayalign + "\"");
                    if (!string.IsNullOrEmpty(obj.process))
                    {
                        result.Append(" ,process: function(t, p, r){\n" +
                            obj.process +
                        "\n}");
                    }
                    if (!string.IsNullOrEmpty(obj.format))
                    {
                        result.Append(" format: " + obj.format + "");
                    }
                    result.Append("}");
                }
                else
                {
                    result.Append(",\n{");
                    result.Append(" display: \"" + obj.displayname + "\"");
                    result.Append(" ,name: \"" + obj.propertyname + "\"");
                    result.Append(" ,width: " + obj.displaywidth.ToString() + "");
                    result.Append(" ,sortable: true");
                    result.Append(" ,align: \"" + obj.displayalign + "\"");
                    if (!string.IsNullOrEmpty(obj.process))
                    {
                        result.Append(" ,process: function(t, p, r){\n" +
                            obj.process +
                        "\n}");
                    }
                    if (!string.IsNullOrEmpty(obj.format))
                    {
                        result.Append(" ,format: " + obj.format + "");
                    }
                    result.Append("}");
                }
                currentIndex++;
            }
            result.Append("\n]");
            return result.ToString();
        }

        /// <summary>
        /// 汉字转拼音
        /// caoq 2014-08-13
        /// </summary>
        /// <param name="hzString"></param>
        /// <returns></returns>
        public static string Hz2Py(string hzString)
        {
            int[] pyValue = new int[]    {
                #region pyValue
                -20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,
                -20032,-20026,-20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,
                -19756,-19751,-19746,-19741,-19739,-19728,-19725,-19715,-19540,-19531,-19525,-19515,
                -19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263,-19261,-19249,
                -19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,
                -19003,-18996,-18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,
                -18731,-18722,-18710,-18697,-18696,-18526,-18518,-18501,-18490,-18478,-18463,-18448,
                -18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183, -18181,-18012,
                -17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,
                -17733,-17730,-17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,
                -17468,-17454,-17433,-17427,-17417,-17202,-17185,-16983,-16970,-16942,-16915,-16733,
                -16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459,-16452,-16448,
                -16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,
                -16212,-16205,-16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,
                -15933,-15920,-15915,-15903,-15889,-15878,-15707,-15701,-15681,-15667,-15661,-15659,
                -15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416,-15408,-15394,
                -15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,
                -15149,-15144,-15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,
                -14941,-14937,-14933,-14930,-14929,-14928,-14926,-14922,-14921,-14914,-14908,-14902,
                -14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668,-14663,-14654,
                -14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,
                -14170,-14159,-14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,
                -14109,-14099,-14097,-14094,-14092,-14090,-14087,-14083,-13917,-13914,-13910,-13907,
                -13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658,-13611,-13601,
                -13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,
                -13340,-13329,-13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,
                -13068,-13063,-13060,-12888,-12875,-12871,-12860,-12858,-12852,-12849,-12838,-12831,
                -12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346,-12320,-12300,
                -12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,
                -11781,-11604,-11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,
                -11055,-11052,-11045,-11041,-11038,-11024,-11020,-11019,-11018,-11014,-10838,-10832,
                -10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331,-10329,-10328,
                -10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254
                #endregion
            };

            string[] pyName = new string[]    {
                #region pyName
                "A","Ai","An","Ang","Ao","Ba","Bai","Ban","Bang","Bao","Bei","Ben",
                "Beng","Bi","Bian","Biao","Bie","Bin","Bing","Bo","Bu","Ba","Cai","Can",
                "Cang","Cao","Ce","Ceng","Cha","Chai","Chan","Chang","Chao","Che","Chen","Cheng",
                "Chi","Chong","Chou","Chu","Chuai","Chuan","Chuang","Chui","Chun","Chuo","Ci","Cong",
                "Cou","Cu","Cuan","Cui","Cun","Cuo","Da","Dai","Dan","Dang","Dao","De",
                "Deng","Di","Dian","Diao","Die","Ding","Diu","Dong","Dou","Du","Duan","Dui",
                "Dun","Duo","E","En","Er","Fa","Fan","Fang","Fei","Fen","Feng","Fo",
                "Fou","Fu","Ga","Gai","Gan","Gang","Gao","Ge","Gei","Gen","Geng","Gong",
                "Gou","Gu","Gua","Guai","Guan","Guang","Gui","Gun","Guo","Ha","Hai","Han",
                "Hang","Hao","He","Hei","Hen","Heng","Hong","Hou","Hu","Hua","Huai","Huan",
                "Huang","Hui","Hun","Huo","Ji","Jia","Jian","Jiang","Jiao","Jie","Jin","Jing",
                "Jiong","Jiu","Ju","Juan","Jue","Jun","Ka","Kai","Kan","Kang","Kao","Ke",
                "Ken","Keng","Kong","Kou","Ku","Kua","Kuai","Kuan","Kuang","Kui","Kun","Kuo",
                "La","Lai","Lan","Lang","Lao","Le","Lei","Leng","Li","Lia","Lian","Liang",
                "Liao","Lie","Lin","Ling","Liu","Long","Lou","Lu","Lv","Luan","Lue","Lun",
                "Luo","Ma","Mai","Man","Mang","Mao","Me","Mei","Men","Meng","Mi","Mian",
                "Miao","Mie","Min","Ming","Miu","Mo","Mou","Mu","Na","Nai","Nan","Nang",
                "Nao","Ne","Nei","Nen","Neng","Ni","Nian","Niang","Niao","Nie","Nin","Ning",
                "Niu","Nong","Nu","Nv","Nuan","Nue","Nuo","O","Ou","Pa","Pai","Pan",
                "Pang","Pao","Pei","Pen","Peng","Pi","Pian","Piao","Pie","Pin","Ping","Po",
                "Pu","Qi","Qia","Qian","Qiang","Qiao","Qie","Qin","Qing","Qiong","Qiu","Qu",
                "Quan","Que","Qun","Ran","Rang","Rao","Re","Ren","Reng","Ri","Rong","Rou",
                "Ru","Ruan","Rui","Run","Ruo","Sa","Sai","San","Sang","Sao","Se","Sen",
                "Seng","Sha","Shai","Shan","Shang","Shao","She","Shen","Sheng","Shi","Shou","Shu",
                "Shua","Shuai","Shuan","Shuang","Shui","Shun","Shuo","Si","Song","Sou","Su","Suan",
                "Sui","Sun","Suo","Ta","Tai","Tan","Tang","Tao","Te","Teng","Ti","Tian",
                "Tiao","Tie","Ting","Tong","Tou","Tu","Tuan","Tui","Tun","Tuo","Wa","Wai",
                "Wan","Wang","Wei","Wen","Weng","Wo","Wu","Xi","Xia","Xian","Xiang","Xiao",
                "Xie","Xin","Xing","Xiong","Xiu","Xu","Xuan","Xue","Xun","Ya","Yan","Yang",
                "Yao","Ye","Yi","Yin","Ying","Yo","Yong","You","Yu","Yuan","Yue","Yun",
                "Za", "Zai","Zan","Zang","Zao","Ze","Zei","Zen","Zeng","Zha","Zhai","Zhan",
                "Zhang","Zhao","Zhe","Zhen","Zheng","Zhi","Zhong","Zhou","Zhu","Zhua","Zhuai","Zhuan",
                "Zhuang","Zhui","Zhun","Zhuo","Zi","Zong","Zou","Zu","Zuan","Zui","Zun","Zuo"
                #endregion
            };

            // 匹配中文字符
            Regex regex = new Regex(@"^[\u4e00-\u9fa5]$");
            byte[] array = new byte[2];
            string pyString = "";
            int chrAsc = 0;
            int i1 = 0;
            int i2 = 0;
            char[] noWChar = hzString.ToCharArray();

            for (int j = 0; j < noWChar.Length; j++)
            {
                // 中文字符
                if (regex.IsMatch(noWChar[j].ToString()))
                {
                    array = System.Text.Encoding.Default.GetBytes(noWChar[j].ToString());
                    i1 = (short)(array[0]);
                    i2 = (short)(array[1]);
                    chrAsc = i1 * 256 + i2 - 65536;
                    if (chrAsc > 0 && chrAsc < 160)
                    {
                        pyString += noWChar[j];
                    }
                    else
                    {
                        // 修正部分文字
                        if (chrAsc == -9254) // 修正“圳”字
                            pyString += "Zhen";
                        else
                        {
                            for (int i = (pyValue.Length - 1); i >= 0; i--)
                            {
                                if (pyValue[i] <= chrAsc)
                                {
                                    pyString += pyName[i];
                                    break;
                                }
                            }
                        }
                    }
                }
                // 非中文字符
                else
                {
                    pyString += noWChar[j].ToString();
                }
            }
            return pyString;
        }

        /// <summary>
        /// 将Json序列化的时间由/Date(1294499956278+0800)转为字符串    
        /// </summary>
        /// <param name="jsonDate">Json序列化的时间，例如：/Date(1411729199000)/</param>
        /// <returns></returns>
        public static DateTime ConvertJsonDateToDateString(string jsonDate)
        {
            string date = @"(?<=\()(\S+)(?=\)/)";
            Match m = Regex.Match(jsonDate, date);
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Value));
            dt = dt.ToLocalTime();
            return dt;
        }

        /// <summary>
        /// 获取汉字字符串的拼音的首字母
        /// caoq 2014-08-13
        /// </summary>
        /// <param name="ChineseStr"></param>
        /// <returns></returns>
        public static string ToCap(string ChineseStr)
        {
            string Capstr = "";
            byte[] ZW = new byte[2];
            long ChineseStr_int;
            string CharStr, ChinaStr = "";
            for (int i = 0; i <= ChineseStr.Length - 1; i++)
            {
                CharStr = ChineseStr.Substring(i, 1).ToString();
                ZW = System.Text.Encoding.Default.GetBytes(CharStr);
                // 得到汉字符的字节数组
                if (ZW.Length == 2)
                {
                    int i1 = (short)(ZW[0]);
                    int i2 = (short)(ZW[1]);
                    ChineseStr_int = i1 * 256 + i2;
                    if ((ChineseStr_int >= 45217) && (ChineseStr_int <= 45252))
                    {
                        ChinaStr = "A";
                    }
                    else if ((ChineseStr_int >= 45253) && (ChineseStr_int <= 45760))
                    {
                        ChinaStr = "B";
                    }

                    else if ((ChineseStr_int >= 45761) && (ChineseStr_int <= 46317))
                    {
                        ChinaStr = "C";
                    }
                    else if ((ChineseStr_int >= 46318) && (ChineseStr_int <= 46825))
                    {
                        ChinaStr = "D";
                    }
                    else if ((ChineseStr_int >= 46826) && (ChineseStr_int <= 47009))
                    {
                        ChinaStr = "E";
                    }
                    else if ((ChineseStr_int >= 47010) && (ChineseStr_int <= 47296))
                    {
                        ChinaStr = "F";
                    }
                    else if ((ChineseStr_int >= 47297) && (ChineseStr_int <= 47613))
                    {
                        ChinaStr = "G";
                    }
                    else if ((ChineseStr_int >= 47614) && (ChineseStr_int <= 48118))
                    {
                        ChinaStr = "H";
                    }
                    else if ((ChineseStr_int >= 48119) && (ChineseStr_int <= 49061))
                    {
                        ChinaStr = "J";
                    }
                    else if ((ChineseStr_int >= 49062) && (ChineseStr_int <= 49323))
                    {
                        ChinaStr = "K";
                    }
                    else if ((ChineseStr_int >= 49324) && (ChineseStr_int <= 49895))
                    {
                        ChinaStr = "L";
                    }
                    else if ((ChineseStr_int >= 49896) && (ChineseStr_int <= 50370))
                    {
                        ChinaStr = "M";
                    }
                    else if ((ChineseStr_int >= 50371) && (ChineseStr_int <= 50613))
                    {
                        ChinaStr = "N";
                    }
                    else if ((ChineseStr_int >= 50614) && (ChineseStr_int <= 50621))
                    {
                        ChinaStr = "O";
                    }
                    else if ((ChineseStr_int >= 50622) && (ChineseStr_int <= 50905))
                    {
                        ChinaStr = "P";
                    }
                    else if ((ChineseStr_int >= 50906) && (ChineseStr_int <= 51386))
                    {
                        ChinaStr = "Q";
                    }
                    else if ((ChineseStr_int >= 51387) && (ChineseStr_int <= 51445))
                    {
                        ChinaStr = "R";
                    }
                    else if ((ChineseStr_int >= 51446) && (ChineseStr_int <= 52217))
                    {
                        ChinaStr = "S";
                    }
                    else if ((ChineseStr_int >= 52218) && (ChineseStr_int <= 52697))
                    {
                        ChinaStr = "T";
                    }
                    else if ((ChineseStr_int >= 52698) && (ChineseStr_int <= 52979))
                    {
                        ChinaStr = "W";
                    }
                    else if ((ChineseStr_int >= 52980) && (ChineseStr_int <= 53640))
                    {
                        ChinaStr = "X";
                    }
                    else if ((ChineseStr_int >= 53689) && (ChineseStr_int <= 54480))
                    {
                        ChinaStr = "Y";
                    }
                    else if ((ChineseStr_int >= 54481) && (ChineseStr_int <= 55289))
                    {
                        ChinaStr = "Z";
                    }
                }
                else
                {
                    Capstr = ChineseStr;
                    break;
                }
                Capstr = Capstr + ChinaStr;
            }
            return Capstr;
        }

        /// <summary>
        /// 将int数组转换成","分隔的字符串
        /// </summary>
        /// <param name="ints"></param>
        /// <returns></returns>
        public static string ConvertToString(int[] ints)
        {
            if (ints == null)
            {
                return null;
            }
            if (ints.Length < 1)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            foreach (int str in ints)
            {
                sb.Append(str).Append(",");
            }
            return sb.ToString().TrimEnd(',');
        }
        /// <summary>
        /// 将string数组转换成","分隔的字符串
        /// </summary>
        /// <param name="ints"></param>
        /// <returns></returns>
        public static string ConvertToString(string[] strings)
        {
            if (strings == null)
            {
                return null;
            }
            if (strings.Length < 1)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            foreach (string str in strings)
            {
                sb.Append(str).Append(",");
            }
            return sb.ToString().TrimEnd(',');
        }

        /// <summary>
        /// 字符串数组转换成int数组
        /// 修改人：曾智磊，2014-11-06，新增是否去重参数
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="isDistinct">是否去重</param>
        /// <returns></returns>
        public static int[] ConvertToInts(string[] strings, bool isDistinct = false)
        {
            int[] ints = null;
            if (strings == null)
            {
                return null;
            }
            List<int> list = new List<int>();
            foreach (string id in strings)
            {
                if (CheckInteger(id))
                {
                    if (isDistinct && list.Where(_obj => _obj == Convert.ToInt32(id)).Count() > 0)
                    {
                        continue;
                    }
                    list.Add(Convert.ToInt32(id));
                }
            }
            if (list != null && list.Count > 0)
            {
                ints = list.ToArray();
            }
            return ints;
        }
        /// <summary>
        /// 字符串数组转换成int数组
        /// 修改人：曾智磊，2014-11-06，新增是否去重参数
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="isDistinct">是否去重</param>
        /// <returns></returns>
        public static long[] ConvertToLongs(string[] strings, bool isDistinct = false)
        {
            long[] longs = null;
            if (strings == null)
            {
                return null;
            }
            List<long> list = new List<long>();
            foreach (string id in strings)
            {
                if (CheckInteger(id))
                {
                    if (isDistinct && list.Where(_obj => _obj == Convert.ToInt64(id)).Count() > 0)
                    {
                        continue;
                    }
                    list.Add(Convert.ToInt64(id));
                }
            }
            if (list != null && list.Count > 0)
            {
                longs = list.ToArray();
            }
            return longs;
        }
        /// <summary>
        /// 将字符串数组转换成list
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static List<int> ConvertToIntList(string[] strings)
        {
            int[] ints = ConvertToInts(strings);
            if (ints == null || ints.Length < 1)
            {
                return new List<int>();
            }
            return ints.ToList();
        }
        /// <summary>
        /// 将字符串数组转换成list
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static List<long> ConvertToLongList(string[] strings)
        {
            long[] longs = ConvertToLongs(strings);
            if (longs == null || longs.Length < 1)
            {
                return new List<long>();
            }
            return longs.ToList();
        }
        /// <summary>
        /// 将字符串转换成list(逗号分隔)
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static List<int> ConvertToIntList(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new List<int>();
            }
            return ConvertToIntList(str.Split(','));
        }
        /// <summary>
        /// 判断一个字符串是否为合法整数(不限制长度)
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns></returns>
        public static bool CheckInteger(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
            string pattern = @"^\d+$";
            return Regex.IsMatch(s, pattern);
        }
        /// <summary>
        /// 判断一个字符串是否为整数或小数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumeric(string str)
        {
            Regex reg = new System.Text.RegularExpressions.Regex(@"^[-]?\d+[.]?\d*$");
            return reg.IsMatch(str);
        }
        /// <summary>
        /// 字符串转换成int数组
        /// 修改人：曾智磊，2014-11-06，新增是否去重参数
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="_char">数组分隔符</param>
        /// <param name="isDistinct">是否去重</param>
        /// <returns></returns>
        public static int[] ConvertToInts(string strings, char _char, bool isDistinct = false)
        {
            if (string.IsNullOrEmpty(strings))
            {
                return null;
            }
            return ConvertToInts(strings.Split(_char), isDistinct);
        }
        /// <summary>
        /// 去除字符串所有空格
        /// 创建人：曾智磊，2014-11-10
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TrimBlank(string str)
        {
            if (str == null)
            {
                return null;
            }
            return Regex.Replace(str, @"\s", "");
        }
        /// <summary>
        /// 获得间隔时间，返回结果格式：1天2小时23分钟
        /// hody，2014-12-05
        /// </summary>
        /// <param name="startdate">起始时间</param>
        /// <param name="enddate">截止时间</param>
        /// <param name="isShowSeconds">是否显示秒</param>
        /// <returns></returns>
        public static string Interval(DateTime startdate, DateTime enddate,bool isShowSeconds)
        {
            string result = string.Empty;
            TimeSpan ts = enddate - startdate;
            if (ts.Days>0)
            {
                result += ts.Days+"天";
            }

            if (ts.Hours>=0)
            {
                result += ts.Hours + "小时";
            }

            if (ts.Minutes>=0)
            {
                result += ts.Minutes + "分钟";
            }

            if (ts.Seconds >= 0 && isShowSeconds)
            {
                result += ts.Seconds + "秒";
            }
            if (!isShowSeconds && ts.Days==0 && ts.Hours==0 && ts.Minutes==0)
            {
                 result += "1分钟";
            }
            return result;
        }
        /// <summary>
        /// 获得过期时间，返回结果格式：1天前或2小时前或23分钟前
        /// hody，2014-12-05
        /// </summary>
        /// <param name="startdate">起始时间</param>
        /// <param name="enddate">截止时间</param>
        /// <param name="isShowSeconds">是否显示秒</param>
        /// <returns></returns>
        public static string GetExpireTime(DateTime startdate, DateTime enddate)
        {
            string result = string.Empty;
            TimeSpan ts = enddate - startdate;
            if (ts.Days > 0)
            {
                result = ts.Days + "天前";
            }else if (ts.Hours > 0)
            {
                result = ts.Hours + "小时前";
            }else if (ts.Minutes >= 0)
            {
                result = ts.Minutes + "分钟前";
            }else 
            {
                result = "1分钟前";
            }
            return result;
        }
        /// <summary>
        /// 字符为空的情况，返回代替Value
        /// hody，2015-02-03
        /// </summary>
        /// <param name="value">字符</param>
        /// <param name="replace">代替字符</param>
        /// <returns></returns>
        public static string StringIsNuLLReturnValue(string value, string replace)
        {
            if (string.IsNullOrEmpty(value))
            {
                return replace;
            }
            else {
                return value;
            }
        }

        /// <summary>
        /// 字符为空的情况，返回代替Value
        /// hody，2015-02-03
        /// </summary>
        /// <param name="value">字符</param>
        /// <param name="replace">代替字符</param>
        /// <returns></returns>
        public static string yyyymmddformat(string date)
        {
            string result = "";
            if (!string.IsNullOrEmpty(date))
            {
                result = string.Format("{0}-{1}-{2}", date.Substring(0, 4), date.Substring(4, 2), date.Substring(6, 2));
                return result;
            }
            else
            {
                return result;
            }
        }


        /// <summary>
        /// 根据正则表达式获取字符串
        /// </summary>
        /// <param name="str">要监测的字符</param>
        /// <param name="regex">正则表达式</param>
        /// <param name="index">要获取符合正则的字符索引例如:$1</param>
        /// <returns></returns>
        public static List<string> GetStrByRegexByIndex(string str, string regex, string index)
        {
            List<string> resultList = new List<string>();
            if (string.IsNullOrEmpty(regex))
            {
                return resultList;
            }
            Regex r = new Regex(regex, RegexOptions.IgnoreCase); //定义一个Regex对象实例            
            MatchCollection mc = r.Matches(str);
            if (mc.Count > 0)
            {
                for (int i = 0; i < mc.Count; i++)
                {
                    if (string.IsNullOrEmpty(index))
                    {
                        resultList.Add(mc[i].Value);
                        continue;
                    }
                    string result = "";
                    if (Regex.IsMatch(str, regex, RegexOptions.IgnoreCase))
                    {
                        result = Regex.Replace(mc[i].Value, regex, index, RegexOptions.IgnoreCase);
                    }
                    resultList.Add(result);


                }
            }
            return resultList;
        }

       /// <summary>
        /// 获得省份简称
       /// </summary>
       /// <param name="provinceid">省份ID</param>
       /// <returns></returns>
        public static string GetProviceShortName(int provinceid)
        {
            Dictionary<int, string> provice = new Dictionary<int, string> ();
            provice.Add(1, "北京");
            provice.Add(2, "上海");
            provice.Add(3, "天津");
            provice.Add(4, "重庆");
            provice.Add(5, "广东");
            provice.Add(6, "黑龙江");
            provice.Add(7, "吉林");
            provice.Add(8, "辽宁");
            provice.Add(9, "河北");
            provice.Add(10, "河南");
            provice.Add(11, "湖北");
            provice.Add(12, "湖南");
            provice.Add(13, "浙江");
            provice.Add(14, "江苏");
            provice.Add(15, "安徽");
            provice.Add(16, "贵州");
            provice.Add(17, "福建");
            provice.Add(18, "四川");
            provice.Add(19, "山东");
            provice.Add(20, "山西");
            provice.Add(21, "新疆");
            provice.Add(22, "内蒙古");
            provice.Add(23, "西藏");
            provice.Add(24, "青海");
            provice.Add(25, "宁夏");
            provice.Add(26, "陕西");
            provice.Add(27, "甘肃");
            provice.Add(28, "江西");
            provice.Add(29, "云南");
            provice.Add(30, "广西");
            provice.Add(31, "海南");
            provice.Add(32, "香港");
            provice.Add(33, "澳门");
            provice.Add(34, "台湾");

            return provinceid>0? provice[provinceid]:"";
        }
        /// <summary>
        /// url解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlDecode(string str)
        {
            if (str == null)
            {
                return null;
            }
            return HttpUtility.UrlDecode(str).Replace("%20", "+");
        }        
        /// <summary>
        /// url编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlEncode(string str)
        {
            if (str == null)
            {
                return null;
            }
            return HttpUtility.UrlEncode(str).Replace("+", "%20");
        }
        /// <summary>
        /// 去除数字中间的字符
        /// </summary>
        /// <param name="rersult"></param>
        /// <returns></returns>
        public static string RemoveNumMiddleText(string result) 
        {
       
            string pattern = @"(?<=[^0-9]+\d+)[^\d]+(?=\d+)";
            Regex r = new Regex(@"\d+([0-9]+)", RegexOptions.IgnoreCase | RegexOptions.Multiline); //定义一个Regex对象实例            
            MatchCollection mc = r.Matches(result);
            MatchEvaluator secondEvaluator = new MatchEvaluator(SecondGroupReplace);
            MatchEvaluator thirdGroupReplace = new MatchEvaluator(ThirdGroupReplace); 
            if (Regex.IsMatch(result, "^\\d+"))
            {
                if (mc.Count>2)
                {
                    result = r.Replace(result, thirdGroupReplace); 
                }
            }
            else
            {
                if (mc.Count > 1)
                {
                    result = r.Replace(result, secondEvaluator); 
                }
            }
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            result = reg.Replace(result, "");
            return result;
        }
        /// <summary>
        /// 替换第三组匹配字符
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private static string ThirdGroupReplace(Match m)
        {
            string result = "";
            string groupValue = m.Groups[2].Value;
            switch (groupValue.Length)
            {
                case 1:
                    result = string.Format("0000{0}", groupValue);
                    break;
                case 2:
                    result = string.Format("000{0}", groupValue);
                    break;
                case 3:
                    result = string.Format("00{0}", groupValue);
                    break;
                case 4:
                    result = string.Format("0{0}", groupValue);
                    break;
                default:
                    result = groupValue;
                    break;
            }
            return result; //直接返回分组1
        }
        /// <summary>
        /// 替换第二组匹配字符
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private static string SecondGroupReplace(Match m)
        {
            string result = "";
            string groupValue = m.Groups[1].Value;
            switch (groupValue.Length)
            {
                case 1:
                    result = string.Format("0000{0}", groupValue);
                    break;
                case 2:
                    result = string.Format("000{0}", groupValue);
                    break;
                case 3:
                    result = string.Format("00{0}", groupValue);
                    break;
                case 4:
                    result = string.Format("0{0}", groupValue);
                    break;
                default:
                    result = groupValue;
                    break;
            }
            return result; //直接返回分组1
        }


        /// <summary>
        /// 中文数字转阿拉伯数字
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string ChinaNumReplaceToNum(string result)
        {
            List<string> list = StringHelper.GetStrByRegexByIndex(result, "([东,西,南,北,中,一,二,三,四,五,六,七,八,九,零,千,百,十,万,亿,甲,乙,丙,丁,戊,己,庚,辛,壬,癸]+)", "$1");
            Dictionary<string, string> keyDic = new Dictionary<string, string>();
            for (int i = 0; i < list.Count; i++)
            {
                keyDic[list[i]] = list[i];
            }
            List<KeyValuePair<string, string>> lst = new List<KeyValuePair<string, string>>(keyDic);
            lst.Sort(delegate(KeyValuePair<string, string> s1, KeyValuePair<string, string> s2)
            {
                return s2.Value.Length.CompareTo(s1.Value.Length);
            });
            foreach (KeyValuePair<string, string> kvp in lst)
            {
                if (Regex.IsMatch(kvp.Key, "[一,二,三,四,五,六,七,八,九,零,千,百,十,万,亿]"))
                {
                    result = result.Replace(kvp.Key, ParseCnToInt(kvp.Key).ToString());
                }
               
            }
             foreach (KeyValuePair<string, string> kvp in lst)
            {
                if (Regex.IsMatch(kvp.Key, "[甲,乙,丙,丁,戊,己,庚,辛,壬,癸]"))
                {
                    result = result.Replace(kvp.Key, TGDParseCnToInt(kvp.Key,false).ToString());
                }
                
            }
             foreach (KeyValuePair<string, string> kvp in lst)
             {
                 if (Regex.IsMatch(kvp.Key, "[东,西,南,北,中]"))
                 {
                     result = result.Replace(kvp.Key, TGDParseCnToInt(kvp.Key, true).ToString());
                 }

             }
            return result;
        }
        /// <summary>  
        /// 将中文数字转换阿拉伯数字  
        /// </summary>  
        /// <param name="cnum">汉字数字</param>  
        /// <returns>长整型阿拉伯数字</returns>  
        public static long TGDParseCnToInt(string cnum, bool isdirection)
        {
            cnum = Regex.Replace(cnum, "\\s+", "");
            long result = 0;//结果  
            for (int i = cnum.Length - 1; i > -1; --i)//从低到高位依次处理  
            {
                if (isdirection)
                {
                    result += DireCharToNumber(cnum[i]);//取出此位对应的单位  
                }
                else 
                {
                    result += TGCharToNumber(cnum[i]);//取出此位对应的单位  
                }
               
               
            }
            return result;
        }
        /// <summary>  
        /// 方向转换数字  
        /// </summary>  
        protected static long DireCharToNumber(char c)
        {
            switch (c)
            {
                case '东': return 1;
                case '西': return 2;
                case '南': return 3;
                case '北': return 4;
                case '中': return 5;
                default: return -1;
            }
        }
        /// <summary>  
        /// 天干转换数字  
        /// </summary>  
        protected static long TGCharToNumber(char c)
        {
            switch (c)
            {
                case '甲': return 1;
                case '乙': return 2;
                case '丙': return 3;
                case '丁': return 4;
                case '戊': return 5;
                case '己': return 6;
                case '庚': return 7;
                case '辛': return 8;
                case '壬': return 9;
                case '癸': return 10;
                default: return -1;
            }
        }
        /// <summary>  
        /// 转换数字  
        /// </summary>  
        protected static long CharToNumber(char c)
        {
            switch (c)
            {
                case '一': return 1;
                case '二': return 2;
                case '三': return 3;
                case '四': return 4;
                case '五': return 5;
                case '六': return 6;
                case '七': return 7;
                case '八': return 8;
                case '九': return 9;
                case '零': return 0;
                default: return -1;
            }
        }

        /// <summary>  
        /// 转换单位  
        /// </summary>  
        protected static long CharToUnit(char c)
        {
            switch (c)
            {
                case '十': return 10;
                case '百': return 100;
                case '千': return 1000;
                case '万': return 10000;
                case '亿': return 100000000;
                default: return -1;
            }
        }
        /// <summary>  
        /// 将中文数字转换阿拉伯数字  
        /// </summary>  
        /// <param name="cnum">汉字数字</param>  
        /// <returns>长整型阿拉伯数字</returns>  
        public static long ParseCnToInt(string cnum)
        {
            
            cnum = Regex.Replace(cnum, "\\s+", "");
            long firstUnit = 1;//一级单位                  
            long secondUnit = 1;//二级单位   
            long tmpUnit = 1;//临时单位变量  
            long result = 0;//结果  
            for (int i = cnum.Length - 1; i > -1; --i)//从低到高位依次处理  
            {
                tmpUnit = CharToUnit(cnum[i]);//取出此位对应的单位  
                if (tmpUnit == -1)
                {

                }
                if (tmpUnit > firstUnit)//判断此位是数字还是单位  
                {
                    firstUnit = tmpUnit;//是的话就赋值,以备下次循环使用  
                    secondUnit = 1;
                    if (i == 0)//处理如果是"十","十一"这样的开头的  
                    {
                        result += firstUnit * secondUnit;
                    }
                    continue;//结束本次循环  
                }
                else if (tmpUnit > secondUnit)
                {
                    secondUnit = tmpUnit;
                    continue;
                }
                result += firstUnit * secondUnit * CharToNumber(cnum[i]);//如果是数字,则和单位想乘然后存到结果里  
            }
            return result;
        } 
        /// <summary>
        /// 处理银行账号每四个数分开显示
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertBankAccount(string str)
        {
            if (str == null)
            {
                return str;
            }
            str = TrimBlank(str);
            var j = 1;
            var result = "";
            for (var i = 0; i < str.Length; i++)
            {
                if (j == 5)
                {
                    result = result + " " + str[i];
                    j = 2;
                }
                else
                {
                    result = result + str[i];
                    j = j + 1;
                }
            }
            return result;
        }
    }
}

