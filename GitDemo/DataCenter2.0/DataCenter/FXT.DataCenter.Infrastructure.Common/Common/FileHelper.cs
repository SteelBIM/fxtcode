using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FXT.DataCenter.Infrastructure.Common.Common
{
    /// <summary>
    /// 文件相关操作类
    /// </summary>
    public static class FileHelper
    {
        #region static string ReadLine(string fileName) 读取某个文件的一行内容

        /// <summary>
        /// 读取某个文件的一行内容
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>读取的内容</returns>
        public static string ReadLine(string fileName)
        {
            return ReadLine(fileName, Encoding.Default);
        }

        #endregion

        #region static string ReadLine(string fileName, Encoding coding) 用编码读取某个文件的一行内容

        /// <summary>
        /// 用编码读取某个文件的一行内容
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="coding">编码</param>
        /// <returns>读取的内容</returns>
        public static string ReadLine(string fileName, Encoding coding)
        {
            using (StreamReader reader = new StreamReader(fileName, coding))
            {
                return reader.ReadLine();
            }
        }

        #endregion

        #region static string ReadText(string fileName) 读取某个文件的所有内容

        /// <summary>
        /// 读取某个文件的所有内容
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>文件的所有内容</returns>
        public static string ReadText(string fileName)
        {
            return ReadText(fileName, Encoding.Default);
        }

        #endregion

        #region static string ReadText(string fileName, Encoding coding) 用编码读取某个文件的所有内容

        /// <summary>
        /// 用编码读取某个文件的所有内容
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="coding">编码</param>
        /// <returns>文件的所有内容</returns>
        public static string ReadText(string fileName, Encoding coding)
        {
            using (StreamReader reader = new StreamReader(fileName, coding))
            {
                return reader.ReadToEnd();
            }
        }

        #endregion

        #region static void WriteText(string fileName, string text, bool append) 把内容写入某个文件

        /// <summary>
        /// 把内容写入某个文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="text">要写入的内容</param>
        /// <param name="append">是否追加</param>
        public static void WriteText(string fileName, string text, bool append)
        {
            WriteText(fileName, text, append, Encoding.Default);
        }

        #endregion

        #region static void WriteText(string fileName, string text, bool append, Encoding coding) 用编码把内容写入某个文件

        /// <summary>
        /// 用编码把内容写入某个文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="text">要写入的内容</param>
        /// <param name="append">是否追加</param>
        /// <param name="coding">编码</param>
        public static void WriteText(string fileName, string text, bool append, Encoding coding)
        {
            using (StreamWriter writer = new StreamWriter(fileName, append, coding))
            {
                writer.WriteLine(text);
            }
        }

        #endregion

        /// <summary>
        /// 产生新的文件名
        /// </summary>
        /// <param name="fileName">原始文件名(新文件的后缀会以此文件后缀为准)</param>
        /// <returns>新的文件名</returns>
        public static string GenerateFileName(string fileName)
        {
            string extension = Path.GetExtension(fileName);

            Random r = new Random(DateTime.Now.Year + DateTime.Now.Minute + DateTime.Now.Millisecond);

            return DateTime.Now.ToString("yyyyMMddhhmmss") + r.Next(100, 1000) + extension;
        }

        /// <summary>
        /// 产生新的文件名
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="extension">扩展名</param>
        /// <returns>新的文件名</returns>
        public static string GenerateFileName(string path, string extension)
        {
            object o = new object();
            lock (o)
            {
                Random r = new Random(DateTime.Now.Millisecond);

                string fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + r.Next(100, 1000) + extension;
                string temp = path + "/" + fileName;

                while (File.Exists(temp))
                {
                    fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + r.Next(100, 1000) + extension;
                    temp = path + "/" + fileName;
                }

                return fileName;
            }
        }

        /// <summary>
        /// 获取目录名称
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static string GetDirectoryName(string directory)
        {
            return !Directory.Exists(directory) ? string.Empty : new DirectoryInfo(directory).Name;
        }

        /// <summary>
        /// 获取所有的子目录
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static List<string> FindSubDirectories(string directory)
        {
            return Directory.GetDirectories(directory, "*", SearchOption.AllDirectories).ToList();
        }
    }
}
