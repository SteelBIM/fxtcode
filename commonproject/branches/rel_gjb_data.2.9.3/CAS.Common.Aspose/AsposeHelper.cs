﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.International.Formatters;
using System.Globalization;
using System.Drawing;
using System.Text.RegularExpressions;

namespace CAS.Common.Aspose
{
    public class AsposeHelper
    {
        #region 对于”2015年9月6日至2015年9月28日“这种多个日期的经行大写转换
        public static string ConvertMoreDate(string date)
        {
            return ConvertMoreDate(date, "");
        }
        public static string ConvertMoreDate(string date, string dbnum)
        {
            if (date.Contains("年") && date.Contains("月") && date.Contains("日"))
            {
                Regex regex1 =new Regex(@"[0-9]{4}年(((0[13578]|(10|12)|[13578])月(0[1-9]|[1-9]|[1-2][0-9]|3[0-1]))|((02|2)月(0[1-9]|[1-9]|[1-2][0-9]))|((0[469]|11|[469])月(0[1-9]|[1-9]|[1-2][0-9]|30)))日");
                foreach (Match m in regex1.Matches(date))
                {
                    date = date.Replace(m.Value, ConvertDate(m.Value, dbnum));
                }
            }
            else
            {
                date = ConvertDate(date, dbnum);
            }
            return date;
        }
        #endregion

        #region 时间格式转换

        public static string ConvertDate(string date)
        {
            return ConvertDate(date, "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dbnum">dbnum1表示大写，mm表示小写的格式</param>
        /// <returns></returns>
        public static string ConvertDate(string date, string dbnum)
        {
            string result = string.Empty;
            try
            {
                if (!TryGetDateTime(date.Replace("年", "-").Replace("月", "-").Replace("日", ""))) return date;
                if (date.IndexOf("年", StringComparison.Ordinal) < 0)
                {
                    date = Convert.ToDateTime(date).ToString(dbnum == "mm" ? "yyyy年MM月dd日" : "yyyy年M月d日");
                }
                //2015年1月2日
                //年份
                string year = date.Substring(0, 4);

                int yearIndex = date.IndexOf("年");
                int monthIndex = date.IndexOf("月");
                int dayIndex = date.IndexOf("日");
                string month = "", day = "";
                if (monthIndex > 0)
                {
                    //月
                    month = date.Substring(yearIndex + 1, monthIndex - yearIndex - 1);
                }
                if (monthIndex > 0 && dayIndex > 0)
                {
                    //日
                    day = date.Substring(monthIndex + 1, dayIndex - monthIndex - 1);
                }
                result = dbnum == "dbnum1"
                    ? string.Format("{0}{1}{2}", year != "" ? GetCnDate(year, true) + "年" : "",
                        month != "" ? GetCnDate(month) + "月" : "", day != "" ? GetCnDate(day) + "日" : "")
                    : string.Format("{0}{1}{2}", year != "" ? year + "年" : "", month != "" ? month + "月" : "",
                        day != "" ? day + "日" : "");
            }
            catch (Exception ex)
            {
                result = date;
            }
            return result;
        }

        #endregion

        /// <summary>
        /// 获取中文年份
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        private static string GetCnDate(string num, bool isSpecial)
        {
            string result = string.Empty;
            if (isSpecial)
            {
                char[] yearChar = num.ToCharArray();
                result = yearChar.Aggregate(result, (current, item) => current + InternationalNumericFormatter.FormatWithCulture("Ln", Convert.ToInt32(item.ToString()), null, new CultureInfo("zh-CHS")));
            }
            else
            {
                result = InternationalNumericFormatter.FormatWithCulture("Ln", Convert.ToInt32(num), null, new CultureInfo("zh-CHS"));
            }
            return result;
        }

        /// <summary>
        /// 获取中文年份
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        private static string GetCnDate(string num)
        {
            return GetCnDate(num, false);
        }

        public static bool TryGetDecimal(string value)
        {
            decimal i = 0;
            return decimal.TryParse(value, out i);
        }
        public static decimal TryGetDecimalValue(string value)
        {
            decimal result = default(decimal);
            decimal.TryParse(value, out result);
            return result;
        }
        public static bool TryGetDateTime(string value)
        {
            DateTime i = DateTime.Today;
            return DateTime.TryParse(value, out i);
        }
        public static bool TryGetbool(string value)
        {
            bool i = false;
            return bool.TryParse(value, out i);
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

        public static bool RegularMatch(string match,string text)
        {
            Regex rg = new Regex(match);
            return rg.IsMatch(text);
        }

        /// <summary>
        /// string转化为int, false:返回 0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int TryGetInt(string value)
        {
            int n = 0;
            int result = 0;
            if (int.TryParse(value, out n))
            {
                result = Convert.ToInt32(n);
            }
            return result;
        }

        /// <summary>
        /// 数据模版替换：判断当前需要添加进dicExcel对象的名称是否存在，如果不存在则添加，如果存在原始值不为空则替换
        /// </summary>
        /// <param name="dicExcel">引用传递</param>
        /// <param name="showName"></param>
        /// <param name="value"></param>
        public static void AddItem(Dictionary<string, string> dicExcel, string showName, object value)
        {
            if (null != value)
            {
                if (dicExcel.ContainsKey(showName))
                {
                    if (!string.IsNullOrEmpty(dicExcel[showName])) //在数据模版中获取数据 当原表单中不为空才替换否则不进行替换也不取默认值
                    {
                        if (value.ToString().ToLower() == "#value!")
                            dicExcel[showName] = "";
                        else
                            dicExcel[showName] = value.ToString();
                    }
                }
                else
                {
                    if (value.ToString() == "一八九九年十二月三十一日" || value.ToString() == "1899年12月31日" || value.ToString().ToLower() == "#value!")
                        dicExcel.Add(showName, "");
                    else
                        dicExcel.Add(showName, value.ToString());
                }
            }
            else
            {
                //对象为null，且不存在该键值则替换为""空
                if (!dicExcel.ContainsKey(showName))
                {
                    dicExcel.Add(showName, "");
                }
            }
        }

        #region  将数字转化成小写（〇一二三四五六七八九）

        /// <summary>
        /// 正则匹配将多个数字转化成小写
        /// </summary>
        /// <param name="stringNum"></param>
        /// <param name="type">1:（〇一二三四五六七八九） 3:（一十二亿三千四百五十六万七千八百九十）</param>
        /// <returns></returns>
        public static string MoreFormatWithCulture(string stringNum, int type)
        {

            if (TryGetDecimal(stringNum))
            {
                if (type == 1)
                {
                    stringNum = stringNum.Replace(stringNum, FormatWithCulture(stringNum));
                }
                else if (type == 3)
                {
                    stringNum = stringNum.Replace(stringNum, SplitFormatWithCulture(stringNum));
                }
            }
            else
            {
                Regex regex1 = new Regex(@"[0-9]{1,}");
                foreach (Match m in regex1.Matches(stringNum))
                {
                    if (type == 1)
                    {
                        stringNum = regex1.Replace(stringNum, FormatWithCulture(m.Value), 1);
                    }
                    else if (type == 3)
                    {
                        stringNum = regex1.Replace(stringNum, SplitFormatWithCulture(m.Value), 1);
                    }
                }
            }
            return stringNum;
        }

        /// <summary>
        /// 将数字转化成小写（〇一二三四五六七八九）
        /// </summary>
        /// <param name="stringNum"></param>
        /// <returns></returns>
        public static string SplitFormatWithCulture(string stringNum)
        {
            string value = "";
            for (int i = 0; i < stringNum.Length; i++)
            {
                value += FormatWithCulture(stringNum.Substring(i, 1));
            }
            return value;
        }

        /// <summary>
        /// 将数字转化成小写（一十二亿三千四百五十六万七千八百九十）
        /// </summary>
        /// <param name="stringNum"></param>
        /// <returns></returns>
        public static string FormatWithCulture(string stringNum)
        {
            return InternationalNumericFormatter.FormatWithCulture("Ln", Convert.ToDecimal(stringNum), null,new CultureInfo("zh-CHS"));
        }

        #endregion

        #region 将数字转化成人民币大写格式 （零壹贰叁肆伍陆柒捌玖）

        /// <summary>
        /// 将一个字符串中的多个数字统一转换格式（例如：26万5千5佰元整）
        /// </summary>
        /// <param name="stringNum"></param>
        /// <returns>贰拾陆万伍仟伍佰元整</returns>
        public static string MoreCmycurD(string stringNum)
        {
            if (TryGetDecimal(stringNum))
                stringNum = CmycurD(stringNum);
            else
            {
                Regex regex1 = new Regex(@"[0-9]{1,}");
                foreach (Match m in regex1.Matches(stringNum))
                {
                    //stringNum = stringNum.Replace(m.Value, CmycurD(m.Value));
                    stringNum=regex1.Replace(stringNum, CmycurD(m.Value), 1);
                }
            }
            return stringNum;
        }

        /// <summary> 
        /// 转换人民币大小金额 
        /// </summary> 
        /// <param name="num">金额</param> 
        /// <returns>返回大写形式</returns> 
        public static string CmycurD(string stringNum)
        {
            string str5 = ""; //人民币大写金额形式 
            try
            {
                if (stringNum == "#NAME?")
                    return "零";
                string changeStringNum =
                    stringNum.Replace("万", "")
                        .Replace("零", "")
                        .Replace("元", "")
                        .Replace("角", "")
                        .Replace("分", "")
                        .Replace("整", "")
                        .Replace("点", ".")
                        .Replace("负", "");
                if (changeStringNum == "" || !TryGetDecimal(changeStringNum))
                    return stringNum;
                decimal num = Convert.ToDecimal(changeStringNum);
                if (stringNum.Contains("分"))
                    num = num/100;
                else if (stringNum.Contains("角"))
                    num = num/10;
                string str1 = "零壹贰叁肆伍陆柒捌玖"; //0-9所对应的汉字 
                string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字 
                string str3 = ""; //从原num值中取出的值 
                string str4 = ""; //数字的字符串形式 
                int i; //循环变量 
                int j; //num的值乘以100的字符串长度 
                string ch1 = ""; //数字的汉语读法 
                string ch2 = ""; //数字位的汉字读法 
                int nzero = 0; //用来计算连续的零值是几个 
                int temp; //从原num值中取出的值 

                num = Math.Round(Math.Abs(num), 2); //将num取绝对值并四舍五入取2位小数 
                str4 = ((long) (num*100)).ToString(); //将num乘100并转换成字符串形式 
                j = str4.Length; //找出最高位 
                if (j > 15)
                {
                    return "溢出";
                }
                str2 = str2.Substring(15 - j); //取出对应位数的str2的值。如：200.55,j为5所以str2=佰拾元角分 

                //循环取出每一位需要转换的值 
                for (i = 0; i < j; i++)
                {
                    str3 = str4.Substring(i, 1); //取出需转换的某一位的值 
                    temp = Convert.ToInt32(str3); //转换为数字 
                    if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                    {
                        //当所取位数不为元、万、亿、万亿上的数字时 
                        if (str3 == "0")
                        {
                            ch1 = "";
                            ch2 = "";
                            nzero = nzero + 1;
                        }
                        else
                        {
                            if (str3 != "0" && nzero != 0)
                            {
                                ch1 = "零" + str1.Substring(temp*1, 1);
                                ch2 = str2.Substring(i, 1);
                                nzero = 0;
                            }
                            else
                            {
                                ch1 = str1.Substring(temp*1, 1);
                                ch2 = str2.Substring(i, 1);
                                nzero = 0;
                            }
                        }
                    }
                    else
                    {
                        //该位是万亿，亿，万，元位等关键位 
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + str1.Substring(temp*1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 != "0" && nzero == 0)
                            {
                                ch1 = str1.Substring(temp*1, 1);
                                ch2 = str2.Substring(i, 1);
                                nzero = 0;
                            }
                            else
                            {
                                if (str3 == "0" && nzero >= 3)
                                {
                                    ch1 = "";
                                    ch2 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    //if (j >= 11)
                                    //{
                                    //    ch1 = "";
                                    //    nzero = nzero + 1;
                                    //}
                                    //else
                                    //{
                                    ch1 = "";
                                    //ch2 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                    //}
                                }
                            }
                        }
                    }
                    if (i == (j - 11) || i == (j - 3))
                    {
                        //如果该位是亿位或元位，则必须写上 
                        ch2 = str2.Substring(i, 1);
                    }
                    str5 = str5 + ch1 + ch2;
                    if (i == j - 1 && str3 == "0")
                    {
                        //最后一位（分）为0时，加上“整” 
                        str5 = str5 + '整';
                    }
                }
                if (num == 0)
                {
                    str5 = "零";
                }
                if (stringNum.Contains("元") || stringNum.Contains("整"))
                {
                }
                else
                {
                    if (str5.Contains("角") || str5.Contains("分"))
                        str5 = str5.Replace("整", "");
                    else
                        str5 = str5.Replace("元", "").Replace("整", "");
                    if (stringNum.Contains("点"))
                    {
                        int sp = stringNum.Split(new[] {"点"}, StringSplitOptions.RemoveEmptyEntries)[1].Length;
                        str5 = str5.Replace("角", "").Replace("分", "");
                        str5 = str5.Replace(str5.Substring(str5.Length - sp, sp),
                            "点" + str5.Substring(str5.Length - sp, sp));
                    }
                }
                if (stringNum.Contains("负"))
                    str5 = "负" + str5;
            }
            catch (Exception ex)
            {
                str5 = stringNum;
            }
            return str5;
        }

        #endregion

        #region 验证输入的数据是不是正整数
        ///<summary>
        ///验证输入的数据是不是正整数
        ///</summary>
        ///<param name="str">传入字符串</param>
        ///<returns>返回true或者false</returns>
        public static bool IsNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[0-9]\d*$");
            return reg1.IsMatch(str);
        }
        #endregion

        /// <summary>
        /// 基于指定的文件路径 等比例缩放照片 宽度或高度为零的话 则表示不根据该宽度或高度进行缩放 20160429  zhangl
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <param name="Width">最大宽度</param>
        /// <param name="Height">最大高度</param>
        /// <param name="isTransition">文本框生成图片时需要按比例转换宽度和高度</param>
        /// <returns>等比例缩放后的图片对象</returns>
        public static Image ScalingImage(string path, double Width, double Height, bool isTransition=false)
        {
            Image scalingimage = null;
            if (File.Exists(path))
            {
                Image image = Image.FromFile(path);
                scalingimage = ScalingImage(image, Width, Height, isTransition);
                image.Dispose();
            }
            return scalingimage;
        }
        /// <summary>
        /// 基于指定的字节数组  等比例缩放照片 宽度或高度为零的话 则表示不根据该宽度或高度进行缩放 20160429  zhangl 
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="Width">最大宽度</param>
        /// <param name="Height">最大高度</param>
        /// <param name="isTransition">文本框生成图片时需要按比例转换宽度和高度</param>
        /// <returns>等比例缩放后的图片对象</returns>
        public static Image ScalingImage(byte[] bytes, double Width, double Height, bool isTransition = false)
        {
            Image scalingimage = null;
            Stream stream = new MemoryStream(bytes);
            Image image = Image.FromStream(stream);
            scalingimage = ScalingImage(image, Width, Height, isTransition);
            image.Dispose();
            return scalingimage;
        }
        /// <summary>
        ///  基于指定的数据流 等比例缩放照片 宽度或高度为零的话 则表示不根据该宽度或高度进行缩放 20160429  zhangl
       /// </summary>
        /// <param name="stream">数据流</param>
        /// <param name="Width">最大宽度</param>
        /// <param name="Height">最大高度</param>
        /// <param name="isTransition">文本框生成图片时需要按比例转换宽度和高度</param>
        /// <returns>等比例缩放后的图片对象</returns>
        public static Image ScalingImage(Stream stream, double Width, double Height, bool isTransition = false)
        {
            Image scalingimage = null;
            Image image = Image.FromStream(stream);
            scalingimage = ScalingImage(image, Width, Height, isTransition);
            image.Dispose();
            return scalingimage;
        }

        /// <summary>
        ///  等比例缩放照片 宽度或高度为零的话 则表示不根据该宽度或高度进行缩放 20160429  zhangl
        /// </summary>
        /// <param name="image">图片对象</param>
        /// <param name="Width">最大宽度</param>
        /// <param name="Height">最大高度</param>
        /// <param name="isTransition">文本框生成图片时需要按比例转换宽度和高度</param>
        /// <returns>等比例缩放后的图片对象</returns>
        public static Image ScalingImage(Image image, double Width, double Height, bool isTransition = false)
        {
            //Image scalingimage = null;
            if (Width > 0 && Height > 0 && (Width < image.Width || Height < image.Height))
            {
                double ScalingHeight = Height/image.Height;
                double ScalingWidth = Width/image.Width;
                if (ScalingHeight > ScalingWidth)
                    Height = image.Height*ScalingWidth;
                else
                    Width = image.Width*ScalingHeight;
            }
            else if (Width > 0 && Height <= 0 && Width < image.Width)
            {
                double ScalingWidth = Width/image.Width;
                Height = image.Height*ScalingWidth;

            }
            else if (Width <= 0 && Height > 0 && Width < image.Width)
            {
                double ScalingHeight = Height/image.Height;
                Width = image.Width*ScalingHeight;
            }
            else
            {
                Width = image.Width;
                Height = image.Height;
            }
            //0.83是因为获取到的shape的宽度和照片的宽度像素单位不一致  所以转换一次
            if (isTransition)
            {
                Height = Height / 0.8;
                Width = Width / 0.83;
            }

            #region 图片不压缩代码

            Bitmap scalingimage = new Bitmap(Convert.ToInt32(Width), Convert.ToInt32(Height));
            Graphics graphic = GetGraphic(image, scalingimage);
            graphic.DrawImage(image, 0, 0, Convert.ToInt32(Width), Convert.ToInt32(Height));
            graphic.Dispose();

            #endregion


            //scalingimage = image.GetThumbnailImage(Convert.ToInt32(Width), Convert.ToInt32(Height), () => { return false; }, IntPtr.Zero);
            image.Dispose();
            return scalingimage;
        }

        public static Graphics GetGraphic(Image originImage, Bitmap newImage)
        {
            newImage.SetResolution(originImage.HorizontalResolution, originImage.VerticalResolution);
            Graphics graphic = Graphics.FromImage(newImage);
            graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            return graphic;
        }
        public static log4net.ILog AsposeLog
        {
            get
            {
                log4net.Config.XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log.config"));
                return log4net.LogManager.GetLogger("CASLog");
            }
        }

        /// <summary>
        /// 写简单日志
        /// </summary>
        /// <param name="info"></param>
        public static void Info(string info)
        {
            AsposeLog.Info(info);
        }
    }
}