using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using CAS.Common;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace FxtCenterService.Logic
{
    /// <summary>
    /// 排序类
    /// </summary>
    public class OrderByHelper
    {
        public static T Clone<T>(T RealObject)
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, RealObject);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        }
        public static List<T> OrderBy<T>(List<T> data, string key)
        {
            List<T> total = new List<T>();//排序后
            List<T> numtextlist = new List<T>();//数字文字
            List<T> lettertextlist = new List<T>();//字母开头
            List<T> textnumlist = new List<T>();//文字数字
            List<T> textlist = new List<T>();//纯文字
            List<T> numlist = new List<T>();//纯数字
            for (int i = 0; i < data.Count; i++)
            {
                SetPropertyValue<T>(data[i], "ob_othername", StringHelper.ChinaNumReplaceToNum(GetPropertyValue<T>(data[i], key).TrimEnd(' ').TrimStart(' ')));
                SetPropertyValue<T>(data[i], "ob_othername", StringHelper.RemoveNumMiddleText(GetPropertyValue<T>(data[i], "ob_othername")));

            }

            //查找纯数字
            for (int i = 0; i < data.Count; i++)
            {
                T dirctModel = data[i];
                //纯数字
                if (Regex.IsMatch(GetPropertyValue<T>(dirctModel, "ob_othername"), "^\\d+$"))
                {
                    SetPropertyValue<T>(data[i], "ob_number", StringHelper.TryGetInt(GetPropertyValue<T>(data[i], "ob_othername")));
                    numlist.Add(Clone<T>(dirctModel));
                }
                else if (Regex.IsMatch(GetPropertyValue<T>(dirctModel, "ob_othername"), "^[0-9]+"))
                {//数字文字
                    Regex r = new Regex("([0-9]+)", RegexOptions.IgnoreCase); //定义一个Regex对象实例            
                    MatchCollection mc = r.Matches(GetPropertyValue<T>(dirctModel, "ob_othername"));
                    SetPropertyValue<T>(dirctModel, "ob_startnum", mc[0].Value);
                    if (mc.Count > 1)
                    {
                        SetPropertyValue<T>(dirctModel, "ob_number", StringHelper.TryGetInt(mc[1].Value));
                    }
                    SetPropertyValue<T>(dirctModel, "ob_text", Regex.Replace(GetPropertyValue<T>(dirctModel, "ob_othername"), "[0-9]+", ""));
                    numtextlist.Add(Clone<T>(dirctModel));
                }
                else if (Regex.IsMatch(GetPropertyValue<T>(dirctModel, "ob_othername"), "^[A-Z,a-z]"))
                {//字母开头
                    Regex r = new Regex("^([A-Z,a-z]+)", RegexOptions.IgnoreCase); //定义一个Regex对象实例            
                    MatchCollection mc = r.Matches(GetPropertyValue<T>(dirctModel, "ob_othername"));
                    SetPropertyValue<T>(dirctModel, "ob_starletter", mc[0].Value);
                    Regex ru = new Regex("([0-9]+)", RegexOptions.IgnoreCase); //定义一个Regex对象实例            
                    MatchCollection mcu = ru.Matches(GetPropertyValue<T>(dirctModel, "ob_othername"));
                    if (mcu.Count > 0)
                    {
                        SetPropertyValue<T>(dirctModel, "ob_number", StringHelper.TryGetInt(mcu[0].Value));
                    }
                    SetPropertyValue<T>(dirctModel, "ob_text", Regex.Replace(GetPropertyValue<T>(dirctModel, "ob_othername"), "^([A-Z,a-z]+)", ""));
                    SetPropertyValue<T>(dirctModel, "ob_text", Regex.Replace(GetPropertyValue<T>(dirctModel, "ob_othername"), "[0-9]+", ""));
                    lettertextlist.Add(Clone<T>(dirctModel));
                }else
                {//文字数字
                    SetPropertyValue<T>(dirctModel, "ob_text", Regex.Replace(GetPropertyValue<T>(dirctModel, "ob_othername"), "[0-9]+", ""));
                    Regex ru = new Regex("([0-9]+)", RegexOptions.IgnoreCase); //定义一个Regex对象实例            
                    MatchCollection mcu = ru.Matches(GetPropertyValue<T>(dirctModel, "ob_othername"));
                    if (mcu.Count > 0)
                    {
                        SetPropertyValue<T>(dirctModel, "ob_number", StringHelper.TryGetInt(mcu[0].Value));
                    }
                    textnumlist.Add(Clone<T>(dirctModel));
                }
                //else if (Regex.IsMatch(GetPropertyValue<T>(dirctModel, "ob_othername"), "^[\u4e00-\u9fa5,（,）,),-,-,—,(]+[0-9]+"))
                //{//文字数字
                //    SetPropertyValue<T>(dirctModel, "ob_text", Regex.Replace(GetPropertyValue<T>(dirctModel, "ob_othername"), "[0-9]+", ""));
                //    Regex ru = new Regex("([0-9]+)", RegexOptions.IgnoreCase); //定义一个Regex对象实例            
                //    MatchCollection mcu = ru.Matches(GetPropertyValue<T>(dirctModel, "ob_othername"));
                //    if (mcu.Count > 0)
                //    {
                //        SetPropertyValue<T>(dirctModel, "ob_number", StringHelper.TryGetInt(mcu[0].Value));
                //    }
                //    textnumlist.Add(Clone<T>(dirctModel));
                //}
                //else
                //{//纯文字
                //    textlist.Add(Clone<T>(dirctModel));
                //}
            }

            //匹配纯数字元素排序
            numlist.Sort(delegate(T s1, T s2)
            {
                return GetPropertyValue<T>(s1, "ob_number").CompareTo(GetPropertyValue<T>(s2, "ob_number"));
            });
            for (int i = 0; i < numlist.Count; i++)
            {
                total.Add(Clone<T>(numlist[i]));
            }
            numlist.Clear();
            //数字加文字
            numtextlist = numtextlist.OrderBy(c => c.GetType().GetProperty("ob_startnum").GetValue(c, null)).ThenBy(c => c.GetType().GetProperty("ob_text").GetValue(c, null)).ThenBy(c => c.GetType().GetProperty("ob_number").GetValue(c, null)).ToList();
            for (int i = 0; i < numtextlist.Count; i++)
            {
                total.Add(Clone<T>(numtextlist[i]));
            }
            numtextlist.Clear();
            //字母开头
            lettertextlist = lettertextlist.OrderBy(c => c.GetType().GetProperty("ob_starletter").GetValue(c, null)).ThenBy(c => c.GetType().GetProperty("ob_text").GetValue(c, null)).ThenBy(c => c.GetType().GetProperty("ob_number").GetValue(c, null)).ToList();
            for (int i = 0; i < lettertextlist.Count; i++)
            {
                total.Add(Clone<T>(lettertextlist[i]));
            }
            lettertextlist.Clear();
            //文字加数字

            textnumlist = textnumlist.OrderBy(c => c.GetType().GetProperty("ob_text").GetValue(c, null)).ThenBy(c => c.GetType().GetProperty("ob_number").GetValue(c, null)).ToList();
            for (int i = 0; i < textnumlist.Count; i++)
            {
                total.Add(Clone<T>(textnumlist[i]));
            }
            textnumlist.Clear();
            //纯文字
            //匹配纯文字元素排序
            textlist.Sort(delegate(T s1, T s2)
            {
                return GetPropertyValue<T>(s1, key).CompareTo(GetPropertyValue<T>(s2, key));
            });
            for (int i = 0; i < textlist.Count; i++)
            {
                total.Add(Clone<T>(textlist[i]));
            }
            textlist.Clear();

            return total;
        }
        /// <summary>
        /// 设置值:反射
        /// </summary>
        /// <typeparam name="T">Class类型</typeparam>
        /// <param name="t">实例化对象</param>
        /// <param name="key">属性名称</param>
        /// <param name="value">值</param>
        private static void SetPropertyValue<T>(T t, string key, object value)
        {
            PropertyInfo propertyInfo = t.GetType().GetProperty(key);
            propertyInfo.SetValue(t, Convert.ChangeType(value, propertyInfo.PropertyType), null);
        }
        /// <summary>
        /// 获取值:反射
        /// </summary>
        /// <typeparam name="T">Class类型</typeparam>
        /// <param name="t">实例化对象</param>
        /// <param name="key">属性名称</param>
        /// <returns></returns>
        private static string GetPropertyValue<T>(T t, string key)
        {
            string result;
            result = t.GetType().GetProperty(key).GetValue(t, null).ToString();
            return result;
        }
    }
}
