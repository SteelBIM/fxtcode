using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.IO;
using System.Reflection;
using FxtSpider.Bll.SpiderCommon.Interface;
using FxtSpider.RunSource;
using System.Net;
using System.Runtime.InteropServices;
using FxtSpider.RunSource.IP代理;
using System.Threading;
using System.Collections;
using FxtSpider.Bll;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.Bll.SpiderCommon;
using FxtSpider.Bll.SpiderCommon.Models;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
[assembly: log4net.Config.Repository]
namespace FxtSpiderConsoleApplication
{
    /// <summary>
    /// 控制台主程序执行入口代码
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string[] runType = args;
            if (args == null || args.Length < 1)
            {
                string paraStr = Console.ReadLine();
                runType = paraStr.Split(' ');
            }
            if (runType[0].Contains("1"))
            {
                WorkItemManager.SetAllStop(1);
                Console.Write("爬取服务器暂停成功");
                return;
            }
            else if (runType[0].Contains("0"))
            {
                WorkItemManager.SetAllStop(0);
                Console.Write("爬取服务器启动成功");
                return;
            }
        work:
            if (!WorkItemManager.CheckPassSpider())//****检查数据库是否有维护程序在执行******//
            {
                System.Threading.Thread.Sleep(60000);
                goto work;
            }
            if (!runType[0].Contains("IP代理"))
            {
                #region (设置日志名称)
                Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
                //将参数添加到字典,用于生成日志文件名
                foreach (string str in runType)
                {
                    string strIndex1 = str.Split('.')[0];
                    string strIndex2 = str.Split('.')[1];
                    List<string> strList = new List<string>();
                    if (!dic.ContainsKey(strIndex1))
                    {
                        dic.Add(strIndex1, new List<string>());
                    }
                    strList = dic[strIndex1];
                    strList.Add(strIndex2);
                    dic[strIndex1] = strList;
                }
                StringBuilder sb = new StringBuilder();
                foreach (KeyValuePair<string, List<string>> kvp in dic)
                {
                    string key = kvp.Key;
                    List<string> list = kvp.Value;
                    sb.Append(key).Append("_");
                    for(int i=0;i<list.Count;i++)
                    {
                        string str = list[i];
                        if (i < list.Count - 1)
                        {
                            sb.Append(str).Append(".");
                        }
                        else
                        {
                            sb.Append(str).Append(";");
                        }
                    }
                }
                InitLogger("(" + sb.ToString() + ")");
                #endregion
                foreach (string str in runType)
                {
                    //运行操作
                    INewDataRum rum = null;
                    string directory = GetConfigDire();
                    string dllFill = directory + "\\FxtSpider.RunSource.dll";
                    Type type = Assembly.LoadFile(dllFill).GetType("FxtSpider.RunSource." + str);
                    if (type != null)
                    {
                        rum = Activator.CreateInstance(type) as INewDataRum;
                    }
                    else
                    {
                        type = Assembly.LoadFile(dllFill).GetType("FxtSpider.RunSource." + str.Split('.')[0] + ".其他城市");
                        rum = Activator.CreateInstance(type) as INewDataRum;
                        rum.CityName = str.Split('.')[1];
                    }
                    rum.start();
                }
            }
            else
            {
                #region (设置日志名称)
                Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
                StringBuilder sb = new StringBuilder();
                //将参数添加到字典,用于生成日志文件名
                foreach (string str in runType)
                {
                    sb.Append(str).Append(";");
                }
                InitLogger("(" + sb.ToString() + ")");
                #endregion
                foreach (string str in runType)
                {
                    string directory = GetConfigDire();
                    string dllFill = directory + "\\FxtSpider.RunSource.dll";
                    string className = str.Split('.')[0] + "." + str.Split('.')[1];
                    object objClass = Assembly.LoadFile(dllFill).CreateInstance("FxtSpider.RunSource." + className);
                    if (objClass != null)
                    {
                        MethodInfo method = objClass.GetType().GetMethod(str.Split('.')[2]);
                        ArrayList al = new ArrayList();
                        al.Add(method);
                        al.Add(objClass);
                        Thread m_thread = new Thread(new ParameterizedThreadStart(ExecIpSpider));
                        m_thread.Start(al);
                    }
                }
 
            }
        }
        private static void InitLogger(string name)
        {
            log4net.GlobalContext.Properties["dynamicName"] = name;
        }
        public static void ExecIpSpider(object param)
        {
            ArrayList al=(ArrayList)param;
            MethodInfo method = al[0] as MethodInfo;
            object objClass = al[1];
            method.Invoke(objClass, null);
        }
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
