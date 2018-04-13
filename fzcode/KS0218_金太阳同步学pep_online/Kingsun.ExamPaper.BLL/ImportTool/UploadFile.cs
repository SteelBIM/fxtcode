using System;
using System.Net;

namespace Kingsun.ExamPaper.BLL.ImportTool
{
    public class UploadFile
    {
        /// <summary>
        /// 上传文件到指定URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static string UploadFileToPath(string url, string filepath)
        {
            WebClient web = new WebClient();
            try
            {
                byte[] bup = web.UploadFile(url, filepath);
                string temp = System.Text.Encoding.UTF8.GetString(bup);
                if (temp.StartsWith("错误："))
                {
                    return "";
                }
                else
                {
                    return temp;
                }
            }
            catch(Exception ex)
            {
                CacheHelper.Instance["UploadErrorMsg"] = CacheHelper.Instance["UploadErrorMsg"] + "\n" + filepath + ":" + ex.Message;
                return "";
            }
        }
    }
}
