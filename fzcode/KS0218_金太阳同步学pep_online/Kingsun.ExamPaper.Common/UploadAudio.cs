using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.IO;

namespace Kingsun.ExamPaper.Common
{
    /// <summary>
    /// 上传音频
    /// </summary>
    public class UploadAudio
    {
        /// <summary>
        /// 上传音频文件
        /// </summary>
        /// <param name="audioUrl">云知声返回的音频地址</param>
        /// <param name="userid">用户ID</param>
        /// <returns>音频保存地址</returns>
        public static string UploadAudioFile(string audioUrl, string userid, int fromAPP = 0)
        {
            WebClient web = new WebClient();
            DateTime dt = DateTime.Now;
            string urlpath = "";
            string filename = dt.ToString("yyyyMMddHHmmssffff") + ".mp3";
            string filepath = GetSavePath(HttpContext.Current, userid);
            try
            {
                web.DownloadFile(audioUrl, filepath + "\\" + filename);
                urlpath = UrlToVirtual(filepath + "\\" + filename, HttpContext.Current, fromAPP);
            }
            catch
            {
                urlpath = audioUrl;
            }
            return urlpath;

        }

        /// <summary>
        /// 合并两个文件
        /// </summary>
        /// <param name="audioUrl"></param>
        /// <param name="lastUrl"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static string MergeAudioFile(string audioUrl, string lastUrl, string userid)
        {
            DateTime dt = DateTime.Now;
            string filename = dt.ToString("yyyyMMddHHmmssffff") + ".mp3";
            string filepath = GetSavePath(HttpContext.Current, userid);
            //新建一个录音存储地址
            FileStream AddStream = new FileStream(VirtualToUrl(lastUrl, HttpContext.Current), FileMode.Append); 
            BinaryWriter AddWriter = new BinaryWriter(AddStream);

            WebClient web = new WebClient();
            try
            {
                web.DownloadFile(audioUrl, filepath + "\\" + filename);
                //这次音频
                FileStream currentStream = new FileStream(filepath + "\\" + filename, FileMode.Open);
                BinaryReader currentReader = new BinaryReader(currentStream);
                AddWriter.Write(currentReader.ReadBytes((int)currentStream.Length));
                currentReader.Close();
                currentStream.Close();

                AddWriter.Close();
                AddStream.Close();              
            }
            catch {
            }

            return lastUrl;
        }

        /// <summary>
        /// 获取服务器上的保存路径
        /// </summary>
        /// <param name="imagesurl"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string VirtualToUrl(string fileUrl, HttpContext context, int fromAPP = 0)
        {
            string tmpRootDir = context.Server.MapPath(context.Request.ApplicationPath.ToString()).Replace("WebAPI", "Web");//获取程序根目录  
            string newFileUrl = fileUrl.Replace(AppSetting.Root + (fromAPP == 0 ? ("/" + AppSetting.Task) : ""),tmpRootDir); //转换成相对路径  
            newFileUrl = newFileUrl.Replace(@"/", @"\");
            return newFileUrl;
        }

        /// <summary>
        /// 获取服务器上的保存路径
        /// </summary>
        /// <param name="imagesurl"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string UrlToVirtual(string fileUrl, HttpContext context, int fromAPP = 0)
        {
            string tmpRootDir = context.Server.MapPath(context.Request.ApplicationPath.ToString()).Replace("WebAPI", "Web");//获取程序根目录  
            string newFileUrl = fileUrl.Replace(tmpRootDir, AppSetting.Root + (fromAPP == 0 ? ("/" + AppSetting.Task) : "")); //转换成相对路径  
            newFileUrl = newFileUrl.Replace(@"\", @"/");
            return newFileUrl;
        }
        /// <summary>
        /// 获取保存路径
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string GetSavePath(HttpContext context,string userid)
        {
            string topDir = context.Server.MapPath("~/QuestionFiles/UserAudio").Replace("WebAPI", "Web") + "/" + userid;//API的录音文件保存到Web路径
            string dayDir = System.IO.Path.Combine(topDir, DateTime.Now.ToString("yyyy/MM/dd"));
            if (!Directory.Exists(dayDir))
            {
                Directory.CreateDirectory(dayDir);
            }
            return dayDir;
        }
    }
}
