using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;


namespace Kingsun.ExamPaper.Common
{
    public  class Common
    {
        #region 获取文件的md5值
        /// <summary>
        /// 获取文件的md5值
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="ToUpper">是否转为大写</param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(string fileName, bool ToUpper = true)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return ToUpper ? sb.ToString().ToUpper() : sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }
        #endregion

        #region 转换为对Excel标题有效的文本
        /// <summary>
        /// 转换为对Excel标题有效的文本
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertToLegalText(string str)
        {
            string tmpStr = str;
            char[] illegalChars = new char[] { '\\', '/', ':', '*', '?', '\"', '<', '>', '|' };
            foreach (char item in illegalChars)
            {
                tmpStr = tmpStr.Replace(item, ' ');
            }
            return tmpStr;
        }
        #endregion

        #region "检测是否为有效邮件地址格式"
        /// <summary>
        /// 检测是否为有效邮件地址格式
        /// </summary>
        /// <param name="strIn">输入邮件地址</param>
        /// <returns></returns>
        public static bool IsValidEmail(string strIn)
        {
            return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        #endregion

        #region "检测是否为有效的电话"
        public static bool IsValidPhone(string phone)
        {
            Regex rcode = new Regex(@"^[1][3-8]+\d{9}$|^(0\d{2,3}-?|\(0\d{2,3}\))?[1-9]\d{4,7}(-\d{1,8})?$");
            if (!rcode.IsMatch(phone))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region "检测是否为有效的手机号码"
        public static bool IsValidCellPhone(string phone)
        {
            Regex rcode = new Regex(@"^[1][3-8]+\d{9}$");
            if (!rcode.IsMatch(phone))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region 获取当前日期是周几
        public static string GetWeekday(DateTime dt)
        {
            string[] weekdays = { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
            return weekdays[Convert.ToInt32(dt.DayOfWeek)];
        }
        #endregion

        #region 封装人名（是否有冒号）
        /// <summary>
        /// 封装人名（是否有冒号）
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string PackagePersonName(string content)
        {
            bool hasName = true;
            if (content.IndexOf(":") > 0)
            {
                string[] arrayC = content.Split(':');
                //判断人名：第一个冒号前的所有单词的首字母是否大写
                foreach (string item in arrayC[0].Split(' '))
                {
                    if (string.IsNullOrEmpty(item) || item == "&")
                    {
                        continue;
                    }
                    if (!Regex.IsMatch((item.Trim())[0].ToString(), "[A-Z]"))
                    {
                        hasName = false;
                        break;
                    }
                }
                if (hasName)
                {
                    return "【" + arrayC[0] + "】" + content.Substring(arrayC[0].Length + 1);
                }
            }
            return content;
        }
        #endregion

        #region 根据是否为S16转换字母为数字
        /// <summary>
        /// 根据是否为S16转换字母为数字
        /// </summary>
        /// <param name="input"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string CharToNum(string input, string model)
        {
            if (model == "S16")
            {
                string highChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                return (highChars.IndexOf(input.ToUpper()) + 1).ToString();
            }
            else
            {
                return input;
            }
        }
        #endregion

        #region 数字转换成字母
        /// <summary>
        /// 数字转换成字母
        /// </summary>
        /// <param name="inputNum"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string NumToChar(int inputNum, string model)
        {
            if (model == "S16")
            {
                return ((char)(64 + inputNum)).ToString();
            }
            else
            {
                return inputNum.ToString();
            }
        }
        #endregion

        #region 按Section拼接排序规则
        /// <summary>
        /// 按Section拼接排序规则
        /// </summary>
        /// <returns></returns>
        public static string OrderBySection()
        {
            string orderby = "case SUBSTRING(Section,4,LEN(Section)) when '一' then '01' when '二' then '02' when '三' then '03' when '四' then '04' "
            + " when '五' then '05' when '六' then '06' when '七' then '07' when '八' then '08' when '九' then '09'  when '十' then '10' "
            + " when '十一' then '11' when '十二' then '12' when '十三' then '13' when '十四' then '14' when '十五' then '15' "
            + " when '十六' then '16' when '十七' then '17' when '十八' then '18' when '十九' then '19'  when '二十' then '20' "
            + " when '二十一' then '21' when '二十二' then '22' when '二十三' then '23' when '二十四' then '24' when '二十五' then '25' "
            + " when '二十六' then '26' when '二十七' then '27' when '二十八' then '28' when '二十九' then '29' when '三十' then '30' else Section end";
            return orderby;
        }
        #endregion

        #region 匹配拼音
        /// <summary>
        /// 匹配拼音
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string MatchPinYin(string content)
        {
            string newContent = "";
            if (!String.IsNullOrEmpty(content))
            {
                content = content.Replace("/n", "<br/>").Replace("\n", "").Replace("<u>", "<#>").Replace("</u>", "</#>");
                string strPinYinReg = @"([a-uA-Uw-zW-Zāáǎàēéěèīíǐìōóǒòūúǔùüǖǘǚǜ]{1,6}[\s]{0,1})";
                string strCharReg = @"[A-Z]+";
                string strHtmlReg = @"(?=<).*?(?=>)";
                Regex regPinYin = new Regex(strPinYinReg);
                Regex regChar = new Regex(strCharReg);
                Regex regHtml = new Regex(strHtmlReg);
                string[] arrayContent = new string[] { };
                arrayContent = regHtml.Split(content);
                MatchCollection mcHtml = regHtml.Matches(content);
                int h = 0;
                for (int i = 0; i < arrayContent.Length; i++)
                {
                    IList<string> listPinYin = new List<string>();
                    if (regPinYin.IsMatch(arrayContent[i]))
                    {
                        MatchCollection mcPinYin = regPinYin.Matches(arrayContent[i]);                        
                        for (int j = 0; j < mcPinYin.Count; j++)
                        {
                            if (mcPinYin[j].ToString() != "br" && mcPinYin[j].ToString() != "nbsp"
                                && !regChar.IsMatch(mcPinYin[j].ToString()))
                            {
                                if (!listPinYin.Contains(mcPinYin[j].ToString().Replace(" ", "")))
                                {
                                    listPinYin.Add(mcPinYin[j].ToString().Replace(" ",""));
                                }
                            }
                        }                       
                    }
                    for (int k = 0; k < listPinYin.Count; k++)
                    {
                        arrayContent[i] = arrayContent[i].Replace(listPinYin[k].ToString(), "<$>" + listPinYin[k].ToString() + "</$>");
                    }
                    if (!String.IsNullOrEmpty(arrayContent[i]) && arrayContent[i].Substring(0, 1) == ">")
                    {
                        arrayContent[i] = mcHtml[h].ToString() + arrayContent[i];
                        h++;
                    }
                    newContent += arrayContent[i];
                }
            }
            return newContent.Replace("<#>", "<u>").Replace("</#>", "</u>");
        }
        #endregion

        #region 匹配分式
        /// <summary>
        /// 匹配分式
        /// </summary>
        /// <param name="content"></param>
        /// <param name="questionModel"></param>
        /// <returns></returns>
        public static string MatchFenShi(string content,string questionModel,bool isContentOrAnswer,int isForWeb=0)
        {
            string requireModel = "S12,S18";//题干和答案均匹配
            if (isContentOrAnswer)
            {
                requireModel += ",S7,S13,S15,S16,S23,S25";//仅匹配题干
            }
            if (!String.IsNullOrEmpty(content) && requireModel.Contains(questionModel))
            {
                content = MatchKuoHao(content);
                string strFenShiReg = @"(\d+\/\d+)|(\（\）\/\（\）)";
                Regex regFenShi = new Regex(strFenShiReg);
                if (regFenShi.IsMatch(content))
                {
                    MatchCollection mcFenShi = regFenShi.Matches(content);
                    IList<string> listFenShi = new List<string>();
                    for (int i = 0; i < mcFenShi.Count; i++)
                    {
                        if (!listFenShi.Contains(mcFenShi[i].ToString()))
                        {
                            listFenShi.Add(mcFenShi[i].ToString());
                        }
                    }
                    for (int j = 0; j < listFenShi.Count; j++)
                    {
                        if (isForWeb == 0)
                        {
                            content = content.Replace(listFenShi[j], "<fs>" + listFenShi[j] + "</fs>");
                        }
                        else {
                            content = content.Replace(listFenShi[j], "<span class=\"MathJye\"><table class=\"table\" style=\"margin-right:1px\" cellpadding=\"0\" cellspacing=\"0\"><tbody>"
                        + "<tr><td class=\"fsline\">" + (listFenShi[j].Split('/'))[0].Replace("（）","（   ）") + "</td></tr>"
                        + "<tr><td>" + (listFenShi[j].Split('/'))[1].Replace("（）", "（   ）") + "</td></tr></tbody></table></span>");
                        }
                    }
                    content = content.Replace("{", "").Replace("}", "");
                }
            }
            return content;
        }
        #endregion

        #region 匹配中文标点
        /// <summary>
        /// 匹配中文标点
        /// </summary>
        /// <param name="content"></param>
        /// <param name="questionModel"></param>
        /// <returns></returns>
        public static string MatchSign(string content)
        {
            string strSign = @"([。？！，、；：“”‘'（）《》〈〉【】『』「」﹃﹄〔〕…—～﹏￥/\n]+)";
            Regex regSign = new Regex(strSign);
            content = regSign.Replace(content,"");
                //if (regSign.IsMatch(content))
                //{
                //    MatchCollection mcSign = regSign.Matches(content);
                //    IList<string> listSign = new List<string>();
                //    for (int i = 0; i < mcSign.Count; i++)
                //    {
                //        if (!listSign.Contains(mcSign[i].ToString()))
                //        {
                //            listSign.Add(mcSign[i].ToString());
                //        }
                //    }
                //    for (int j = 0; j < listSign.Count; j++)
                //    {
                //        content = content.Replace(listSign[j], "");
                //    }
                //}
            
            return content;
        }
        #endregion

        #region 匹配括号
        /// <summary>
        /// 匹配括号
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string MatchKuoHao(string content)
        {
            string strKuoHaoReg = @"（\s+）";
            Regex regKuoHao = new Regex(strKuoHaoReg);
            if (regKuoHao.IsMatch(content))
            {
                MatchCollection mcKuoHao = regKuoHao.Matches(content);
                for (int i = 0; i < mcKuoHao.Count; i++)
                {
                    content = content.Replace(mcKuoHao[i].ToString(), "（）");
                }
            }
            return content;
        }
        #endregion

        #region 写日志到指定目录文件
        /// <summary>
        /// 写日志到指定目录文件
        /// </summary>
        /// <param name="folderName">日志文件目录</param>
        /// <param name="log">日志内容</param>
        public static void WriteLog(string folderName, string log)
        {
            string path = AppSetting.Root;
            path += "/QuestionFiles/" + folderName + "/" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            string newpath = VirtualToUrl(path);
            FileStream fs = new FileStream(newpath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            if (File.Exists(newpath))
            {
                fs.Position = fs.Length;
            }
            sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " : " + log, 0, log.Length);
            sw.Flush();
            sw.Close();
        }
        #endregion

        /// <summary>
        /// 获取服务器上的保存路径
        /// </summary>
        /// <param name="imagesurl"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string VirtualToUrl(string fileUrl)
        {
            string tmpRootDir = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath.ToString());//获取程序根目录  
            string newFileUrl = fileUrl.Replace(AppSetting.Root, tmpRootDir); //转换成相对路径  
            newFileUrl = newFileUrl.Replace(@"/", @"\");
            return newFileUrl;
        }
    }

    public static class ExtensionMethods
    {
        /// <summary>
        /// 用于前端alert
        /// </summary>
        /// <param name="m"></param>
        /// <param name="msg"></param>
        public static void Alert(this ClientScriptManager m, string msg)
        {
            m.RegisterStartupScript(typeof(Page), "tishi", string.Format("<script type=\"text/javascript\">alert('{0}！');</script>", msg));
        }
    } 
}
