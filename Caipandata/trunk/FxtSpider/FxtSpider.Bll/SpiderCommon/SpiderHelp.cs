using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FxtSpider.Bll.SpiderCommon.Models;
using System.Net;
using System.IO;
using System.IO.Compression;
using log4net;
using FxtSpider.Common;
using FxtSpider.DAL.LinqToSql;

namespace FxtSpider.Bll.SpiderCommon
{
    /// <summary>
    /// object扩展类。
    /// </summary>
    [Serializable]
    public static class SpiderHelp
    {
        /// <summary>
        /// 赶集网需要验证码判断正则
        /// </summary>
        private static RegexInfo regex_ganji_checkcode = new RegexInfo("(<[^<>]*class=\"error[^\"]*\"[^<>]*>[^<>]*验证码[^<>]*</[^<>]*>)", "$1");
        /// <summary>
        /// 58同城需要验证码判断正则
        /// </summary>
        private static RegexInfo regex_58_checkcode = new RegexInfo("(<div class=\"search_tips_info\">[^<>]*验证码[^<>]*</div>)", "$1");
        /// <summary>
        /// 存储所有网站需要验证码判断正则
        /// </summary>
        private static RegexInfo regex_checkcode = null;
        private static readonly ILog log = LogManager.GetLogger(typeof(SpiderHelp));

        static SpiderHelp()
        {
            regex_checkcode = regex_ganji_checkcode;
            regex_checkcode.RegexInfoList.Add(regex_58_checkcode);
        }
        #region 页面抓取和正则验证

        /// <summary>
        /// 根据页面url,页面编码,正则表达式;获取相应字符串
        /// </summary>
        /// <param name="url">页面url</param>
        /// <param name="encoding">页面编码</param>
        /// <param name="dic">正则表达式规则</param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> GetHtmlByRegex(string url, string encoding, Dictionary<string, RegexInfo> dic, 网站表 webObj, int cityId, string referer = null, bool keepAlive = false, int timeout=60000)
        {
            string NowProxyIp = null;
            int 网络异常重试次数 = 0;
            int 网络异常时代理ip更换次数 = 0;
            int 验证码异常时代理ip更换次数 = 0;
            string resultHtml = "";
            string ipStr = NowProxyIp == null ? "" : NowProxyIp + ",";
        begin:
            Dictionary<string, List<string>> resultDic = new Dictionary<string, List<string>>();
            if (dic == null)
            {
                return resultDic;
            } 
            try
            {
                resultHtml = GetHtml(url, encoding, proxyIp: NowProxyIp, referer: referer, keepAlive: keepAlive,timeout:timeout);
            }
            catch (Exception ex)
            {
                if (网络异常重试次数 < 2)   
                {
                    System.Threading.Thread.Sleep(3000);
                    网络异常重试次数++;
                    goto begin;
                }
                if (webObj.BlockadeOfIP)
                {
                    if (网络异常时代理ip更换次数 < 3)
                    {
                        System.Threading.Thread.Sleep(2000);
                        网络异常时代理ip更换次数++;

                        work:
                        if (!WorkItemManager.CheckPassSpider())//检查数据库是否有维护程序在执行
                        {
                            System.Threading.Thread.Sleep(60000);
                            goto work;
                        }
                        ProxyIpManager.SetNotEffectiveProxyIp(webObj.ID, NowProxyIp);//设置当前代理ip为不可用
                        NowProxyIp = ProxyIpManager.GetEffectiveProxyIp(webObj.ID);//获取新代理ip
                        ipStr = ipStr + NowProxyIp + ",";
                        goto begin;
                    }
                }
                log.Error(string.Format("SpiderHouse:(requestUrl:{0}--请求异常)", url), ex);
                //记录爬取失败原因和信息(网络异常)
                work2:
                if (!WorkItemManager.CheckPassSpider())//检查数据库是否有维护程序在执行
                {
                    System.Threading.Thread.Sleep(60000);
                    goto work2;
                }
                DatSpiderErrorLogManager.InsertError(cityId, webObj.ID, url, SysCodeManager.Code_1_1, "所用ip:" + ipStr);
                goto end;
            }
        end:
            resultHtml = Regex.Replace(resultHtml, @"(([\r\n])[\s]+|[\r\n]+|[\t]+)", "", RegexOptions.IgnoreCase);
            //检测是否需要验证码
            bool checkError = false;//记录是否为验证码错误
            if (webObj.BlockadeOfIPType == WebsiteManager.BlockadeOfIPType1)
            {
                List<string> checkList = GetStrByRegexByIndex(resultHtml, regex_checkcode);
                if (checkList != null && checkList.Count > 0)
                {
                    网络异常时代理ip更换次数 = 0;
                    if (验证码异常时代理ip更换次数 < 3)
                    {
                        验证码异常时代理ip更换次数++;
                    //记录爬取失败原因和信息(网络异常)
                    work3:
                        if (!WorkItemManager.CheckPassSpider())//检查数据库是否有维护程序在执行
                        {
                            System.Threading.Thread.Sleep(60000);
                            goto work3;
                        }
                        ProxyIpManager.SetNotEffectiveProxyIp(webObj.ID, NowProxyIp);//设置当前代理ip为不可用
                        NowProxyIp = ProxyIpManager.GetEffectiveProxyIp(webObj.ID);//获取新代理ip
                        ipStr = ipStr + NowProxyIp + ",";
                        goto begin;
                    }
                    else
                    {
                        checkError = true;
                    //记录爬取时需要验证码
                    //记录爬取失败原因和信息(网络异常)
                    work4:
                        if (!WorkItemManager.CheckPassSpider())//检查数据库是否有维护程序在执行
                        {
                            System.Threading.Thread.Sleep(60000);
                            goto work4;
                        }
                        DatSpiderErrorLogManager.InsertError(cityId, webObj.ID, url, SysCodeManager.Code_1_3,
                            string.Format("所用ip:{0}", ipStr)
                            );
                    }
                    log.Debug(string.Format("SpiderHouse:(requestUrl:{0}--请求异常:需输入验证码)", url));
                }
            }
            foreach (KeyValuePair<string, RegexInfo> kvp in dic)
            {
                string key = kvp.Key;
                List<string> list = GetStrByRegexByIndex(resultHtml, kvp.Value);
                resultDic.Add(key, list);
                if (webObj != null && webObj.ID == WebsiteManager.搜房网_ID && resultHtml.Contains(">此房源已售出!<") && resultHtml.Contains(">您可以选择查看：<"))
                {
                    continue;
                }
                //正则表达式为重要字段&&无网络异常&&无验证码异常&&正则表达式不为null
                if (key.Contains("*") && !string.IsNullOrEmpty(resultHtml) && !checkError && kvp.Value != null && !string.IsNullOrEmpty(kvp.Value.RegexStr))
                {
                    //通过规则未获取到信息&&不为验证码异常
                    if (list == null || list.Count < 1)
                    {
                        //记录爬取失败原因和信息(通过规则未获取到字符)
                        DatSpiderErrorLogManager.InsertError(cityId, webObj.ID, url, SysCodeManager.Code_1_2,
                            string.Format("描述:{0},规则:{1},索引:{2},其他规则个数:{3},所用ip:{4}",
                                          key,
                                          kvp.Value.RegexStr,
                                          kvp.Value.RegexIndex,
                                          kvp.Value.RegexInfoList == null ? 0 : kvp.Value.RegexInfoList.Count,
                                          ipStr
                                          )
                            );

                    }
                }
            }
            if (webObj != null && webObj.ID == WebsiteManager.搜房网_ID&&resultHtml.Contains(">此房源已售出!<") && resultHtml.Contains(">您可以选择查看：<"))
            {
                resultDic.Add("NotData", new List<string> { "1" });
            }
            return resultDic;
        }
        /// <summary>
        /// 根据页面url,页面编码,正则表达式;获取相应字符串(未叫代理ip功能)
        /// </summary>
        /// <param name="url">页面url</param>
        /// <param name="encoding">页面编码</param>
        /// <param name="dic">正则表达式规则</param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> GetHtmlByRegexNotProxyIp(string url, string encoding, Dictionary<string, RegexInfo> dic)
        {
            int 网络异常重试次数 = 0;
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
                if (网络异常重试次数 < 3)
                {
                    System.Threading.Thread.Sleep(5000);
                    网络异常重试次数++;
                    goto begin;
                }
                log.Error(string.Format("GetHtmlByRegexNotProxyIp:(requestUrl:{0}--请求异常)", url), ex);
            }
            resultHtml = Regex.Replace(resultHtml, @"(([\r\n])[\s]+|[\r\n]+)", "", RegexOptions.IgnoreCase);
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
            resultHtml = Regex.Replace(resultHtml, @"(([\r\n])[\s]+|[\r\n]+)", "", RegexOptions.IgnoreCase);
            foreach (KeyValuePair<string, RegexInfo> kvp in dic)
            {
                string key = kvp.Key;
                List<string> list = GetStrByRegexByIndex(resultHtml, kvp.Value);
                resultDic.Add(key, list);
            }
            return resultDic;
        }
        /// <summary>
        /// 根据页面url获取页面html字符
        /// </summary>
        /// <param name="url">页面url</param>
        /// <param name="encoding">页面编码</param>
        /// <param name="proxyIp">代理ip地址</param>
        /// <returns></returns>
        public static string GetHtml(string url, string encoding, string proxyIp = null, string referer = null, bool keepAlive = false, int timeout=60000)
        {
            //encoding = "GBK";
            //**用于处理异常：使用HttpWebRequest进行请求时发生错误:基础连接已关闭,发送时发生错误处理*//
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            //proxyIp = "14.18.17.166:80";
            WebProxy wp = new WebProxy();
            if (!string.IsNullOrEmpty(proxyIp))
            {
                wp.Address = new Uri(proxyIp.Contains("http://") ? proxyIp : "http://" + proxyIp);
            }
            string resultHtml = "";
            HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(new Uri(url, false));
            if (!string.IsNullOrEmpty(proxyIp))
            {
                request.Proxy = wp;
            }
            request.Method = "get";
            request.Timeout = timeout;
            request.KeepAlive = keepAlive;
            request.AllowAutoRedirect = true;
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";

            if (!string.IsNullOrEmpty(referer))
            {
                request.Referer = referer;
            }
            CookieContainer cc = new CookieContainer();
            request.CookieContainer = cc;

            System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.Stream stream = response.GetResponseStream();
            string aaa = response.Headers["Content-Encoding"];
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
            read.Close();
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
        #endregion

        #region 计算各字段值
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
            int _sumL = Convert.ToInt32(sumL); 
            if (_sumL >=18)
            {
                return 建筑类型Manager.高层;
            }
            if (_sumL >= 9)
            {
                return 建筑类型Manager.小高层;
            }
            if (_sumL > 3)
            {
                return 建筑类型Manager.多层;
            }
            return 建筑类型Manager.低层;

        }
        /// <summary>
        /// 获取计算住宅用途
        /// </summary>
        /// <param name="purposes">当前用途</param>
        /// <param name="cityName">当前城市</param>
        /// <param name="price">单价</param>
        /// <param name="area">建筑面积</param>
        /// <param name="sumL">总楼层</param>
        /// <param name="suozaiL">所在层</param>
        /// <param name="jzxs">建筑形式</param>
        /// <param name="houseType">建筑类型</param>
        /// <returns></returns>
        public static string GetHousePurposes(string purposes,string cityName,string price,string area, string sumL,string suozaiL,string jzxs, string buildingType)
        {
            string result = 用途Manager.普通住宅;
            area = StringHelp.TrimBlank(area);
            sumL = StringHelp.TrimBlank(sumL);
            suozaiL = StringHelp.TrimBlank(suozaiL);
            jzxs = StringHelp.TrimBlank(jzxs);
            price = StringHelp.TrimBlank(price);
            if (cityName != null && cityName.Contains("重庆") && StringHelp.IsInteger(area) && StringHelp.IsInteger(suozaiL) && StringHelp.IsInteger(price))
            {
                int _area = Convert.ToInt32(area);
                int _suozaiL = Convert.ToInt32(suozaiL);
                double _price = Convert.ToDouble(price);
                if (_suozaiL <= 1 && _price >= 30000 && _area <= 50)
                {
                    return 用途Manager.商业;
                }
            }
            if (!string.IsNullOrEmpty(purposes))
            {
                return purposes;
            }
            if (!StringHelp.IsInteger(area) || !StringHelp.IsInteger(sumL))
            {
                result = "";
            }
            else
            {
                int _area = Convert.ToInt32(area);
                int _sumL = Convert.ToInt32(sumL);
                if (_area >= 180 && _sumL <= 4)
                {
                    result = 用途Manager.别墅;
                }
                else if (_area < 144 || (_sumL >= 5 && _area >= 144))
                {
                    result = 用途Manager.普通住宅;
                }
                else
                {
                    result = 用途Manager.非普通住宅;
                }
            }
            if (!string.IsNullOrEmpty(jzxs) && StringHelp.IsInteger(area) && Convert.ToInt32(area) > 150)
            {
                if (jzxs.Contains("独"))
                {
                    result = 用途Manager.独立别墅;
                }
                else if (jzxs.Contains("联"))
                {
                    result = 用途Manager.联排别墅;
                }
                else if (jzxs.Contains("叠"))
                {
                    result = 用途Manager.叠加别墅;
                }
                else if (jzxs.Contains("双"))
                {
                    result = 用途Manager.双拼别墅;
                }
            }
            return result;
            

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
            if (!houseType.Contains("房") && !houseType.Contains("厅"))
            {
                return houseType;
            }
            Dictionary<string, RegexInfo> regDic = new Dictionary<string, RegexInfo>();
            string _houseType = StringHelp.ChineseConvertToNumber(houseType);
            regDic.Add("房", new RegexInfo("(.+)房", "$1"));
            regDic.Add("厅", new RegexInfo("(\\d+)厅", "$1"));
            Dictionary<string, List<string>> regDicResult = GetStrByRegex(_houseType, regDic);
            string _f = regDicResult["房"].Count < 1 ? "" : regDicResult["房"][0];
            string _t = regDicResult["厅"].Count < 1 ? "" : regDicResult["厅"][0];
            if (string.IsNullOrEmpty(_f))
            {
                _f = "0";
            }
            if (string.IsNullOrEmpty(_t))
            {
                _t = "0";
            }
            if (!StringHelp.IsInteger(_f) || !StringHelp.IsInteger(_t))
            {
                return "";
            }
            int f = Convert.ToInt32(_f);//房
            int t = Convert.ToInt32(_t);//厅
            //4+房0厅
            if (t < 1 && f > 3)
            {
                return "";
            }
            //0厅,1房2厅
            //房3+
            if (f > 2)
            {
                //厅>=房&&房4-
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

        public static string 获取户型结构(string jg)
        {
            jg = StringHelp.TrimBlank(jg);
            jg = string.IsNullOrEmpty(jg) ? 结构Manager.平面 : jg;
            if (jg.Contains("平"))
            {
                jg = 结构Manager.平面;
            }
            else if(jg.Contains("跃"))
            {
                jg = 结构Manager.跃式;
            }
            else if (jg.Contains("复"))
            {
                jg = 结构Manager.复式;
            }
            else if (jg.Contains("错"))
            {
                jg = 结构Manager.错层;
            }
            return jg;
        }

        public static string 获取装修类型(string zx)
        {
            if (zx == null)
            {
                return "";
            }
            if (zx.Contains("毛") || zx.Contains("坯") || zx.Contains("未装修"))
            {
                zx = 装修Manager.毛坯;
            }
            else if (zx.Contains("普通") || zx.Contains("新装修"))
            {
                zx = 装修Manager.普通;
            }
            else if (zx.Contains("简"))
            {
                zx = 装修Manager.简易;
            }
            else if (zx.Contains("中"))
            {
                zx = 装修Manager.中档;
            }
            else if (zx.Contains("精"))
            {
                zx = 装修Manager.高档;
            }
            else if (zx.Contains("豪"))
            {
                zx = 装修Manager.豪华;
            }
            return zx;
        }
        public static string 处理朝向字符(string cx)
        {

            if (cx == null)
            {
                return null;
            }
            cx = cx.Replace("朝", "");
            cx = cx.Replace("向", "");
            if (cx.Contains("南北"))
            {
                return "南北";
            }
            return cx;
        }
        #endregion

        #region 验证各字段值
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
            if (_unitPrice > 100 && _unitPrice < 200000)
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
        public static bool CheckHouseAll(案例信息 newHouse,string cityName,out string message)
        {
            message = "";
            if (!SpiderHelp.CheckHouseArea(newHouse.面积String))
            {
                message = string.Format("面积不合格,下一个:{0}-(value_mj:{1},url:{2}--)", cityName, newHouse.面积, newHouse.URL);
                return false;
            }
            if (!SpiderHelp.CheckHouseUnitPrice(newHouse.单价String))
            {
                message = string.Format("单价不合格,下一个:{0}-(value_dj:{1},url:{2}--)", cityName, newHouse.单价, newHouse.URL);
                return false;
            }
            if (!SpiderHelp.CheckHouseFloor(newHouse.总楼层String, newHouse.所在楼层String))
            {
                message = string.Format("所在楼层或总楼层不合格,下一个:{0}-(value_zlc:{1},value_szlc:{1},url:{2}--)", cityName, newHouse.总楼层, newHouse.所在楼层, newHouse.URL);
                return false;
            }
            return true;
        }
        #endregion
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
