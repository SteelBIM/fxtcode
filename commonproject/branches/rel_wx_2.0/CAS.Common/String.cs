using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using CAS.Entity.DBEntity;

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

        public static float TryGetFloat(string value)
        {
            value = value.Replace(",", "");
            float result = 0;
            float.TryParse(value, out result);
            return result;
        }

        public static double TryGetDouble(string value)
        {
            value = value.Replace(",", "");
            double result = 0;
            double.TryParse(value, out result);
            return result;
        }

        public static DateTime TryGetDateTime(string value)
        {
            DateTime result = default(DateTime);
            DateTime.TryParse(value, out result);
            return result;
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
            if (value == null)
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
                    num = num + "." + strBumeric.Split('.')[1];
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
    }
}


