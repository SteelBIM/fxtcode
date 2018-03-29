using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;

namespace FxtCommonLibrary.LibraryLucene
{
    /// <summary>
    /// 词库类，生成树形词库
    /// </summary>
    public class WordTree
    {
        /// <summary>
        /// 字典文件的路径
        /// </summary>
        //private static string DictPath = Application.StartupPath + "\\data\\sDict.txt";
        private static string DictPath = "dict\\sDict.txt";
        /// <summary>
        /// 缓存字典的对象
        /// </summary>
        public static Hashtable chartable = new Hashtable();

        /// <summary>
        /// 字典文件读取的状态
        /// </summary>
        private static bool DictLoaded = false;
        /// <summary>
        /// 读取字典文件所用的时间
        /// </summary>
        public static double DictLoad_Span = 0;

        /// <summary>
        /// 正则表达式
        /// </summary>
        public string strChinese = "[\u4e00-\u9fa5]";
        public string strNumber = "[0-9]";
        public string strEnglish = "[a-zA-Z]";


        /// <summary>
        /// 获取字符类型
        /// </summary>
        /// <param name="Char"></param>
        /// <returns>
        /// 0: 中文,1:英文,2:数字
        ///</returns>
        public int GetCharType(string Char)
        {
            if (new Regex(strChinese).IsMatch(Char))
                return 0;
            if (new Regex(strEnglish).IsMatch(Char))
                return 1;
            if (new Regex(strNumber).IsMatch(Char))
                return 2;
            return -1;
        }

        /// <summary>
        /// 读取字典文件
        /// </summary>
        public void LoadDict()
        {
            if (DictLoaded) return;
            BuidDictTree();
            DictLoaded = true;
            return;
        }

        /// <summary>
        /// 建立树
        /// </summary>
        private void BuidDictTree()
        {
            long dt_s = DateTime.Now.Ticks;
            string char_s;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DictPath);
            StreamReader reader = new StreamReader(path, System.Text.Encoding.UTF8);
            string word = reader.ReadLine();
            while (word != null && word.Trim() != "")
            {
                Hashtable t_chartable = chartable;
                for (int i = 0; i < word.Length; i++)
                {
                    char_s = word.Substring(i, 1);
                    if (!t_chartable.Contains(char_s))
                    {
                        t_chartable.Add(char_s, new Hashtable());
                    }
                    t_chartable = (Hashtable)t_chartable[char_s];
                }
                word = reader.ReadLine();
            }
            reader.Close();
            //DictLoad_Span = (double)(DateTime.Now.Ticks - dt_s) / (1000 * 10000);
            //System.Console.Out.WriteLine("读取字典文件所用的时间: " + DictLoad_Span + "s");
        }

    }
}
