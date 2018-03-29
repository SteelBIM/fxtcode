using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FxtSpider.Bll.SpiderCommon;
using System.Xml;

namespace FxtSpider.RunSource.任务计划
{
    public static class Help
    {
        public static readonly string 任务计划txt;
        static Help()
        {
            任务计划txt = get_任务计划txt();
        }
        public static void get_配置任务计划文件字符串(string fileName,DateTime 开始运行时间, int 间隔天数, int 运行超时时间_小时,string 运行内容)
        {
            DateTime date = DateTime.Now;
            string str = 任务计划txt;
            str = str.Replace("{$当前日期}", date.ToString("yyyy-MM-dd"));
            str = str.Replace("{$当前时间}", date.ToString("HH:mm:ss.fffffff"));
            str = str.Replace("{$计算机名}", "fxt00010");
            str = str.Replace("{$计算机用户}", "Administrator");
            str = str.Replace("{$域名}", "FXTCN");
            str = str.Replace("{$域用户}", "zengzl");
            str = str.Replace("{$开始运行日期}", 开始运行时间.ToString("yyyy-MM-dd"));
            str = str.Replace("{$开始运行时间}", 开始运行时间.ToString("HH:mm:ss"));
            str = str.Replace("{$运行超时时间}", "T" + 运行超时时间_小时.ToString() + "H");
            str = str.Replace("{$相隔天数}", 间隔天数.ToString());
            str = str.Replace("{$运行内容}", 运行内容);
            XmlDocument strxml = new XmlDocument();
            strxml.LoadXml(str);
            strxml.Save(SpiderHelp.GetConfigDire() + fileName);
        }
        public static void get_配置任务计划文件字符串2(string fileName, DateTime 开始运行时间, int 间隔天数, int 运行超时时间_天, string 运行内容)
        {
            DateTime date = DateTime.Now;
            string str = 任务计划txt;
            str = str.Replace("{$当前日期}", date.ToString("yyyy-MM-dd"));
            str = str.Replace("{$当前日期}", date.ToString("HH:mm:ss.fffffff"));
            str = str.Replace("{$计算机名}", "fxt00010");
            str = str.Replace("{$计算机用户}", "Administrator");
            str = str.Replace("{$开始运行日期}", 开始运行时间.ToString("yyyy-MM-dd"));
            str = str.Replace("{$开始运行时间}", 开始运行时间.ToString("HH:mm:ss"));
            str = str.Replace("{$运行超时时间}", 运行超时时间_天.ToString() + "D");
            str = str.Replace("{$相隔天数}", 间隔天数.ToString());
            str = str.Replace("{$运行内容}", 运行内容);
            XmlDocument strxml = new XmlDocument();
            strxml.LoadXml(str);
            strxml.Save(SpiderHelp.GetConfigDire() + fileName);
        }
        public static string get_任务计划txt()
        {
            StringBuilder sb = new StringBuilder();
            string filaName = "任务计划\\计划任务.txt";
            FileStream fs2 = new FileStream(SpiderHelp.GetConfigDire() + filaName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
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
                    sb.Append(str);
                }
            }
            fs2.Flush();
            sr.Close();
            fs2.Close();
            return sb.ToString();
        }
    }
}
