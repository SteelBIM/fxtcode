using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FxtSpider.Common.Models;
using System.Net;
using System.IO;
using System.IO.Compression;
using log4net;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace FxtSpider.Common
{
    /// <summary>
    /// object扩展类。
    /// </summary>
    [Serializable]
    public static class SpiderHelp
    {
        /// <summary>
        /// 高层
        /// </summary>
        private static readonly string buildingType1 = "高层";
        /// <summary>
        /// 小高层
        /// </summary>
        private static readonly string buildingType2 = "小高层";
        /// <summary>
        /// 多层
        /// </summary>
        private static readonly string buildingType3 = "多层";
        /// <summary>
        /// 低层
        /// </summary>
        private static readonly string buildingType4 = "低层";

        /// <summary>
        /// 独立别墅
        /// </summary>
        private static readonly string housePurposes1 = "非普通住宅"; //"独立别墅";
        /// <summary>
        /// 联排别墅
        /// </summary>
        private static readonly string housePurposes2 ="非普通住宅";// "联排别墅";
        /// <summary>
        /// 非普通住宅
        /// </summary>
        private static readonly string housePurposes3 ="非普通住宅";
        /// <summary>
        /// 普通住宅
        /// </summary>
        private static readonly string housePurposes4 = "普通住宅";

        private static readonly ILog log = LogManager.GetLogger(typeof(SpiderHelp));
        /// <summary>
        /// 根据页面url,页面编码,正则表达式;获取相应字符串
        /// </summary>
        /// <param name="url">页面url</param>
        /// <param name="encoding">页面编码</param>
        /// <param name="dic">正则表达式规则</param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> GetHtmlByRegex(string url, string encoding, Dictionary<string, RegexInfo> dic)
        {
            int 重试次数 = 0;
            string resultHtml = "";
         begin:
            Dictionary<string, List<string>> resultDic = new Dictionary<string, List<string>>();
            if (dic == null)
            {
                return resultDic;
            }
            try
            {
                resultHtml = GetHtml(url, encoding);
            }
            catch (Exception ex)
            {
                System.Threading.Thread.Sleep(2000);
                重试次数++;
                if (重试次数 < 3)
                {
                    goto begin;
                }
                log.Error(string.Format("SpiderHouse:(requestUrl:{0}--请求异常)", url), ex);
                goto end;
                //return GetHtmlByRegex(url, encoding, dic);
            }
         end:
            resultHtml = Regex.Replace(resultHtml, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            foreach (KeyValuePair<string, RegexInfo> kvp in dic)
            {
                string key = kvp.Key;
                List<string> list = GetStrByRegexByIndex(resultHtml, kvp.Value);
                resultDic.Add(key, list);
            }
            return resultDic;
        }
        /// <summary>
        /// 根据正则表达式;获取相应字符串
        /// </summary>
        /// <param name="str">要监测的字符串</param>
        /// <param name="dic">正则表达式规则</param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> GetStrByRegex(string str, Dictionary<string, RegexInfo> dic)
        {
            Dictionary<string, List<string>> resultDic = new Dictionary<string, List<string>>();
            if (dic == null)
            {
                return resultDic;
            }
            string resultHtml = str == null ? "" : str;
            resultHtml = Regex.Replace(resultHtml, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            foreach (KeyValuePair<string, RegexInfo> kvp in dic)
            {
                string key = kvp.Key;
                List<string> list = GetStrByRegexByIndex(resultHtml, kvp.Value.RegexStr, kvp.Value.RegexIndex);
                resultDic.Add(key, list);
            }
            return resultDic;
        }
        /// <summary>
        /// 根据页面url获取页面html字符
        /// </summary>
        /// <param name="url">页面url</param>
        /// <param name="encoding">页面编码</param>
        /// <returns></returns>
        public static string GetHtml(string url, string encoding)
        {

                WebProxy wp = new WebProxy();
                string resultHtml = "";
                HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(new Uri(url, false));
                request.Method = "get";
                request.Timeout = 10000;
                request.KeepAlive = false;
                request.AllowAutoRedirect = true;
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";

                CookieContainer cc = new CookieContainer();
                request.CookieContainer = cc;

                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.Stream stream = response.GetResponseStream();
                if (response.Headers["Content-Encoding"] == "gzip")//gzip解压处理
                {
                    MemoryStream msTemp = new MemoryStream();
                    GZipStream gzs = new GZipStream(stream, CompressionMode.Decompress);
                    byte[] buf = new byte[1024];
                    int len;
                    while ((len = gzs.Read(buf, 0, buf.Length)) > 0)
                    {
                        msTemp.Write(buf, 0, len);
                    }
                    msTemp.Position = 0;
                    stream = msTemp;
                }
                System.IO.StreamReader read = new System.IO.StreamReader(stream, Encoding.GetEncoding(encoding));
                resultHtml = read.ReadToEnd();
                response.Close();
            
            return resultHtml;
        }
        /// <summary>
        /// 根据正则表达式获取字符串
        /// </summary>
        /// <param name="str">要监测的字符</param>
        /// <param name="regex">正则表达式</param>
        /// <param name="index">要获取符合正则的字符索引例如:$1</param>
        /// <returns></returns>
        public static List<string> GetStrByRegexByIndex(string str, RegexInfo regexInfo)
        {
            List<string> resultList = new List<string>();
            if (regexInfo == null)
            {
                return resultList;
            }
            string regex = regexInfo.RegexStr;
            if (string.IsNullOrEmpty(regex))
            {
                return resultList;
            }
            string index = regexInfo.RegexIndex;
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
            else if (regexInfo.RegexInfoList != null && regexInfo.RegexInfoList.Count > 0)
            {
                foreach (RegexInfo _regexInfo in regexInfo.RegexInfoList)
                {
                    Regex _r = new Regex(_regexInfo.RegexStr, RegexOptions.IgnoreCase); //定义一个Regex对象实例            
                    MatchCollection _mc = _r.Matches(str);
                    if (_mc.Count > 0)
                    {
                        for (int i = 0; i < _mc.Count; i++)
                        {
                            if (string.IsNullOrEmpty(_regexInfo.RegexIndex))
                            {
                                resultList.Add(_mc[i].Value);
                                continue;
                            }
                            string result = "";
                            if (Regex.IsMatch(str, _regexInfo.RegexStr, RegexOptions.IgnoreCase))
                            {
                                result = Regex.Replace(_mc[i].Value, _regexInfo.RegexStr, _regexInfo.RegexIndex, RegexOptions.IgnoreCase);
                            }
                            resultList.Add(result);
                        }
                        break;
                    }
                }
            }
            return resultList;
        }/// <summary>
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
        /// 获取建筑类型
        /// </summary>
        /// <param name="sumL">总楼层</param>
        /// <returns></returns>
        public static string GetBuildingType (string sumL)
        {
            sumL = StringHelp.TrimBlank(sumL);
            if (!StringHelp.IsInteger(sumL))
            {
                return "";
            }
            int _sumL = Convert.ToInt32(sumL); ;
            if (_sumL > 17)
            {
                return buildingType1;//高层
            }
            if (_sumL > 9)
            {
                return buildingType2;//小高层
            }
            if (_sumL > 3)
            {
                return buildingType3;//多层
            }
            return buildingType4;//低层

        }
        /// <summary>
        /// 获取住宅用途
        /// </summary>
        /// <param name="area">建筑面积</param>
        /// <param name="sumL">总楼层</param>
        /// <returns></returns>
        public static string GetHousePurposes2(string area, string sumL)
        {
            string buildingType = GetBuildingType(sumL);
            return GetHousePurposes(area, buildingType);
        }
        /// <summary>
        /// 获取计算住宅用途
        /// </summary>
        /// <param name="area">建筑面积</param>
        /// <param name="houseType">建筑类型</param>
        /// <returns></returns>
        public static string GetHousePurposes(string area, string buildingType)
        {

            area = StringHelp.TrimBlank(area);
            if (!StringHelp.IsInteger(area))
            {
                return "";
            }
            int _area = Convert.ToInt32(area);
            if (_area > 299 && buildingType.Equals(buildingType4))
            {
                return housePurposes1;//独立别墅
            }
            if (_area > 199 && buildingType.Equals(buildingType4))
            {
                return housePurposes2;//联排别墅
            }
            if (_area > 144)
            {
                return housePurposes3;//非普通住宅
            }
            return housePurposes4;//普通住宅
        }
        /// <summary>
        /// 获取计算户型
        /// </summary>
        /// <param name="houseType"></param>
        /// <returns></returns>
        public static string GetHouseType(string houseType)
        {
            if (houseType == null)
            {
                return "";
            }
            houseType = StringHelp.TrimBlank(houseType);
            if(string.IsNullOrEmpty(houseType))
            {
                return "";
            }
            Dictionary<string, RegexInfo> regDic = new Dictionary<string, RegexInfo>();
            string _houseType = StringHelp.ChineseConvertToNumber(houseType);
            regDic.Add("房", new RegexInfo("(.+)房", "$1"));
            regDic.Add("厅", new RegexInfo("(\\d+)厅", "$1"));
            Dictionary<string, List<string>> regDicResult = GetStrByRegex(_houseType, regDic);
            string _f = regDicResult["房"].Count < 1 ? "" : regDicResult["房"][0];
            string _t = regDicResult["厅"].Count < 1 ? "" : regDicResult["厅"][0];
            if (!StringHelp.IsInteger(_f) || !StringHelp.IsInteger(_t))
            {
                return "";
            }
            int f = Convert.ToInt32(_f);//房
            int t = Convert.ToInt32(_t);//厅
            //房>2
            if (f > 2)
            {
                //厅>=房&&房<5
                if (t >= f && f < 5)
                {
                    t = f - 1;
                }
            }
            else
            {
                //厅>房
                if (t > f)
                {
                    t = f;
                }
            }
            if (f >= 7)
            {
                return "七房及以上";
            }
            if (f > 4 && f < 7)
            {
                return StringHelp.NumberConvertToChinese(f.ToString()) + "房";
            }
            if (t < 1)
            {
                return "单身公寓";
            }
            _houseType = string.Format("{0}房{1}厅", f.ToString(), t.ToString());
            return StringHelp.NumberConvertToChinese(_houseType);

        }
        /// <summary>
        /// 检查房屋面积(1000>area>15)
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static bool CheckHouseArea(string area)
        {
            if (string.IsNullOrEmpty(area))
            {
                return false;
            }
            area = StringHelp.TrimBlank(area);
            if (!StringHelp.IsInteger(area))
            {
                return false;
            }
            int _area = Convert.ToInt32(area);
            if (_area > 15 && _area < 1000)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 检查单价范围(100000>unitPrice>3000)
        /// </summary>
        /// <param name="unitPrice"></param>
        /// <returns></returns>
        public static bool CheckHouseUnitPrice(string unitPrice)
        {

            if (string.IsNullOrEmpty(unitPrice))
            {
                return false;
            }
            unitPrice = StringHelp.TrimBlank(unitPrice);
            if (!StringHelp.IsInteger(unitPrice))
            {
                return false;
            }
            int _unitPrice = Convert.ToInt32(unitPrice);
            if (_unitPrice > 3000 && _unitPrice < 100000)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 检查楼层
        /// </summary>
        /// <param name="sumFloor">总楼层</param>
        /// <param name="nowFloor">当前楼层</param>
        /// <returns></returns>
        public static bool CheckHouseFloor(string sumFloor, string nowFloor)
        {
            sumFloor = StringHelp.TrimBlank(sumFloor);
            if (string.IsNullOrEmpty(sumFloor))
            {
                return true;
            }
            nowFloor = StringHelp.TrimBlank(nowFloor);
            if (string.IsNullOrEmpty(nowFloor))
            {
                return true;
            }
            if (!StringHelp.IsInteger(sumFloor))
            {
                return false;
            }
            if (!StringHelp.IsInteger(nowFloor))
            {
                return false;
            }
            int _nowFloor = Convert.ToInt32(nowFloor);
            int _sumFloor = Convert.ToInt32(sumFloor);
            if (_nowFloor > _sumFloor)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 检查所有数据
        /// </summary>
        /// <param name="newHouse"></param>
        /// <param name="cityName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool CheckHouseAll(NewHouse newHouse,string cityName,out string message)
        {
            message = "";
            if (!SpiderHelp.CheckHouseArea(newHouse.Mj))
            {
                message = string.Format("面积不合格,下一个:{0}-(value_mj:{1},url:{2}--)", cityName, newHouse.Mj, newHouse.Url);
                return false;
            }
            if (!SpiderHelp.CheckHouseUnitPrice(newHouse.Dj))
            {
                message = string.Format("单价不合格,下一个:{0}-(value_dj:{1},url:{2}--)", cityName, newHouse.Dj, newHouse.Url);
                return false;
            }
            if (!SpiderHelp.CheckHouseFloor(newHouse.Zlc, newHouse.Szlc))
            {
                message = string.Format("所在楼层或总楼层不合格,下一个:{0}-(value_zlc:{1},value_szlc:{1},url:{2}--)", cityName, newHouse.Zlc, newHouse.Szlc, newHouse.Url);
                return false;
            }
            return true;
        }

        private static Dictionary<string, string> dicNowUrls = new Dictionary<string, string>();
        /// <summary>
        /// 获取上次爬取的当前列表页页面的最新一条url
        /// </summary>
        /// <param name="cityName">当前城市</param>
        /// <param name="wzly">网站来源</param>
        /// <returns></returns>
        public static string GetNowUrl(string cityName, string wzly)
        {
            string key = wzly + "_" + cityName + "_NewDataSpide";
            if (dicNowUrls.ContainsKey(key))
            {
                return dicNowUrls[key];
            }
            string nowUrl = "";
            string filaName = key + ".txt";
            if (!File.Exists(GetConfigDire() + filaName))
            {
                FileStream fs1 = new FileStream(GetConfigDire() + filaName, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs1, Encoding.UTF8);
                sw.Flush();
                fs1.Flush();
                sw.Close();
                fs1.Close();
            }
            FileStream fs2 = new FileStream(GetConfigDire() + filaName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs2, Encoding.UTF8);

            for (; ; )
            {
                string str = sr.ReadLine();
                if (str == null)
                {
                    break;
                }
                else
                {
                    nowUrl = str;
                }
            }
            fs2.Flush();
            sr.Close();
            fs2.Close();
            dicNowUrls.Add(key, nowUrl);
            return nowUrl;
        }
        /// <summary>
        /// 记录爬取的当前列表页页面的最新一条url
        /// </summary>
        /// <param name="cityName">当前城市</param>
        /// <param name="nowUrl">当前url</param>
        public static void SetNowUrl(string cityName, string wzly, string nowUrl)
        {
            string key = wzly + "_" + cityName + "_NewDataSpide";
            dicNowUrls[key] = nowUrl;
            string filaName = key + ".txt"; ;
            string str = nowUrl;
            StreamWriter m_streamWriter = new StreamWriter(GetConfigDire() + filaName, false, Encoding.UTF8);
            m_streamWriter.Flush();
            m_streamWriter.BaseStream.Seek(0, SeekOrigin.Begin);
            m_streamWriter.WriteLine(str);
            m_streamWriter.Close();
        }
        /// <summary>
        /// 获取当前程序根目录
        /// </summary>
        /// <returns></returns>
        public static string GetConfigDire()
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            string resultPath = "";
            if (path.EndsWith("bin"))
            {
                if (path.ToLower().StartsWith("file:"))
                    resultPath = (path.Substring(6) + "\\..\\");
                else
                    resultPath = (path + "\\..\\");
            }
            else
            {
                if (path.ToLower().StartsWith("file:"))
                    resultPath = (path.Substring(6) + "\\");
                else
                    resultPath = (path + "\\");
            }
            if (!Directory.Exists(resultPath))
            {
                Directory.CreateDirectory(resultPath);
            }
            return resultPath;
        }

    }
}
