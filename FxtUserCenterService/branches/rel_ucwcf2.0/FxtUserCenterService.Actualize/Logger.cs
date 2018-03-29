using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FxtUserCenterService.Actualize
{
    public class Logger
    {
        public static void WriteLog(string sinfo, string info, string createtime)
        {
            var file = "C:\\UserCenterLog\\" + createtime + ".txt";

            if (!File.Exists(file))
            {

                FileStream fs1 = new FileStream(file, FileMode.Create, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sw.WriteLine(sinfo + "+" + info);//开始写入值

                sw.Close();
                fs1.Close();

            }
            else
            {
                StreamWriter sw = File.AppendText(file);
                sw.WriteLine("********************************");
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sw.WriteLine(sinfo + "+" + info);//开始写入值
                sw.Flush();
                sw.Close();
            }
        }
    }
}
