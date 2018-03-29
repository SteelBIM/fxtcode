using System;
using System.IO;
using System.Net;
using System.Web;

namespace CAS.Common
{
    public class FileEntity
    {
        /// <summary>
        /// 文件名
        /// </summary>
        private string _filename;
        public string filename
        {
            get { return _filename; }
            set { _filename = value; }
        }

        /// <summary>
        /// 路径
        /// </summary>
        private string _filepath;
        public string filepath
        {
            get { return _filepath; }
            set { _filepath = value; }
        }
        /// <summary>
        /// 路径
        /// </summary>
        private string _filesmallpath;
        public string filesmallpath
        {
            get { return _filesmallpath; }
            set { _filesmallpath = value; }
        }
        /// <summary>
        /// 大小
        /// </summary>
        private int _filesize;
        public int filesize
        {
            get { return _filesize; }
            set { _filesize = value; }
        }
        public int width { get; set; }
        public int height { get; set; }
    }
    /// <summary>
    /// Oss阿里云云存储
    /// </summary>
    public class OssHelper
    {
        /// <summary>
        /// oss云存储的文件上传根路径
        /// </summary>
        public static string ossUpload = WebCommon.GetConfigSetting("ossUpload");
        /// <summary>
        /// oss云存储的文件下载根路径 
        /// </summary>
        public static string ossDownload
        {
            get
            {
                if (!string.IsNullOrEmpty(WebCommon.GetConfigSetting("ossUpload")))
                {
                    return  WebCommon.GetConfigSetting("ossFileDownload");
                }
                else
                {
                    return "/";
                }
            }
        }
        /// <summary>
        /// oss云存储的图片下载根路径
        /// </summary>
        public static string ossImgDownload
        {
            get
            {
                if (!string.IsNullOrEmpty(WebCommon.GetConfigSetting("ossUpload")))
                {
                    return WebCommon.GetConfigSetting("ossImgDownload");
                }
                else
                {
                    return "/";
                }
            }
        }
        /// <summary>
        /// 返回当前路径是否为本地部署
        /// </summary>
        public static bool IsLocalPath
        {
            get
            {
                if (string.IsNullOrEmpty(WebCommon.GetConfigSetting("ossUpload")))
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 本地服务器临时存储oss云存储下载下来的文件路径
        /// </summary>
        public static string ossTempFileDownloadPath = "/tempfiles/OA_OssFiles/Download/";
        /// <summary>
        /// 本地服务器临时存储要上传到oss云存储的文件路径
        /// </summary>
        public static string ossTempFileUploadPath = "/tempfiles/OA_OssFiles/Upload/";

        #region 根据配置情况返回oss的文件地址或者本地服务器的文件地址
        /// <summary>
        /// 根据配置情况返回oss的文件地址或者本地服务器的文件地址
        /// </summary>
        /// <param name="roolurl">根目录</param>
        /// <param name="companyid">公司id</param>
        /// <param name="osspath">oss的路径</param>
        /// <param name="directoryProductType">文件夹名称</param>
        /// <param name="needfilename">是否含有文件名,如否,则不需要下面两个参数</param>
        /// <param name="filename">文件名</param>
        /// <param name="localpath">如果是本地服务器,且配有该地址 ,直接使用此地址而不去生成一个地址</param>
        /// <param name="DateFormatType">如果配有日期格式, 则按日期格式来</param>
        /// <returns></returns>
        public static string GetServerPath(string roolurl, string companyid, string osspath, string directoryProductType, bool needfilename = true, string filename = "", string localpath = "",string DateFormatType="")
        {
            string path = "";
            string dateType = string.IsNullOrEmpty(DateFormatType) ? "yyyy/MM/dd" : DateFormatType;
            if (!string.IsNullOrEmpty(WebCommon.GetConfigSetting("ossUpload")))//是否为oss存储方式
            {
                if (needfilename)//传入路径是否有文件名
                {
                    string tempPath = osspath + companyid + "/" + directoryProductType + "/" + DateTime.Now.AddDays(-1).ToString(dateType, System.Globalization.DateTimeFormatInfo.InvariantInfo) + "/" + filename;
                    string newPath = osspath + companyid + "/" + directoryProductType + "/" + DateTime.Now.ToString(dateType, System.Globalization.DateTimeFormatInfo.InvariantInfo) + "/" + filename; ;
                    if (File.Exists(HttpContext.Current.Server.MapPath(tempPath)) && !File.Exists(HttpContext.Current.Server.MapPath(newPath))) //防止在凌晨整点前打开的文件保存失败
                    {
                        path = tempPath;
                    }
                    else
                    {
                        path = newPath;
                    }
                }
                else
                {
                    path = osspath + companyid + "/" + directoryProductType + "/" + DateTime.Now.ToString(dateType, System.Globalization.DateTimeFormatInfo.InvariantInfo) + "/";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(localpath))
                {
                    string uploadroot = "upload";
                    //修改上传路径 kevin 2013-9-24
                    string uploadpath = companyid + "/" + directoryProductType + "/" + DateTime.Now.ToString(dateType, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    string savepath = HttpContext.Current.Server.MapPath(roolurl + uploadroot + "/" + uploadpath + "/");
                    if (!Directory.Exists(savepath))
                        Directory.CreateDirectory(savepath);
                    if (needfilename)
                    {
                        path = roolurl + uploadroot + "/" + uploadpath + "/" + filename;
                    }
                    else
                    {
                        path = roolurl + uploadroot + "/" + uploadpath + "/";
                    }
                }
                else
                {
                    if (needfilename)
                    {
                        path = roolurl + localpath;
                    }
                    else
                    {
                        path = roolurl + localpath.Replace(filename, string.Empty);
                    }
                }
            }
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(Path.GetDirectoryName(path))))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(Path.GetDirectoryName(path)));
            }
            return path;
        } 
        #endregion
        

        #region 上传文件
        /***cas项目引用***/
        /// <summary>
        /// 在指定文件夹中将文件上传到oss
        /// </summary>
        /// <param name="filepath">存放在阿里云的文件路径(虚拟路径:"upload/365/..")</param>
        /// <param name="companyid">公司id</param>
        /// <param name="DirectoryPath">要上传的文件的路径(全虚拟路径)</param>
        /// <param name="isDel">是否需要删除要上传的文件</param>
        /// <returns>0:失败，1：成功</returns>
        public static int UploadFile(string filepath, string companyid, string DirectoryPath, bool isDel)
        {
            int result = 0;
            string path = "";
            try
            {
                //如果没有配置oss 则使用本地部署
                if (string.IsNullOrEmpty(WebCommon.GetConfigSetting("ossUpload")))
                {
                    #region 本地部署
                    if (File.Exists(DirectoryPath)) //是否存在文件
                    {
                        path=HttpContext.Current.Server.MapPath("/" + filepath);
                        if (!Directory.Exists(Path.GetDirectoryName(path)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(path));
                        }
                        if (File.Exists(path)) //如果附件已经存在 则取消复制
                        {
                            return 1;
                        }
                        File.Copy(DirectoryPath, path);
                        if (isDel)
                        {
                            File.Delete(DirectoryPath);
                        }
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                    #endregion
                }
                else
                {
                    #region 阿里云部署
                    if (!string.IsNullOrEmpty(DirectoryPath))
                    {
                        path = DirectoryPath;
                    }
                    else
                    {
                        path = HttpContext.Current.Server.MapPath(GetServerPath("/", companyid.ToString(), ossTempFileUploadPath, "OA", true, Path.GetFileName(filepath)));
                    }
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(ossUpload + filepath); //创建请求
                    request.Method = "POST";
                    request.Timeout = 600000; //上传时间调整为10分钟

                    if (File.Exists(path)) //是否存在文件
                    {
                        using (FileStream tempfile = new FileStream(path, FileMode.Open, FileAccess.Read))//读取文件
                        {
                            //request.ContentLength = tempfile.Length;
                            using (Stream newStream = request.GetRequestStream()) //为请求流写入数据
                            {
                                int readResult;
                                while ((readResult = tempfile.ReadByte()) != -1)//遍历文件流,将所有字节写入其中
                                {
                                    newStream.WriteByte((Byte)readResult);
                                }
                            }
                        }
                        using (HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse())
                        {
                            if (myResponse.StatusCode == HttpStatusCode.OK) //判断状态
                            {
                                if (isDel)//如果需要删除
                                {
                                    File.Delete(path);
                                }
                                result = 1; // "上传成功!";
                            }
                            else
                            {
                                result = 0; // "上传失败!";
                            }
                        }
                    }
                    else
                    {
                        LogHelper.Info("阿里云服务器文件上传失败 ! 文件路径 :" + filepath + " .错误原因 : 文件不存在 !");
                        result = 0; // "上传失败!";
                    }
                    return result; 
                    #endregion
                }
            }
            catch (WebException ex)
            {
                var rsp = ex.Response as HttpWebResponse;
                int httpStatusCode = rsp == null ? 0 : (int)rsp.StatusCode;
                switch (httpStatusCode)
                {
                    case 403:
                    case 404:
                        LogHelper.Error(ex);
                        LogHelper.Info("文件上传失败 ! 文件路径 :" + filepath + " .错误原因 :" + ex.Message);
                        break;
                    default:
                        LogHelper.Info("文件上传失败 ! 文件路径 :" + filepath + " .错误原因 :" + ex.Message);
                        throw ex;
                }
                return 0;
            }
        }
        #endregion

        #region 下载文件
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filepath">找到存放在阿里云的文件路径(虚拟目录)</param>
        /// <param name="companyid">公司id</param>
        /// <param name="directoryProductType">文件夹名称</param>
        /// <param name="DirectoryPath">需要存放的文件夹目录(虚拟目录) ,为空则默认存放在临时文件夹中的/OA_OssFiles/Download/中</param>
        /// <param name="isNew">是否需要下载最新</param>
        /// <param name="randomFileName">文件名是取随机还是取原文件名,默认是随机. 取原名赋值false,用于缓存文件路径等,如logo的缓存</param>
        /// <returns></returns>
        public static string DownLoadFile(string filepath, string companyid,string directoryProductType,string DirectoryPath = "", bool isNew = true, bool randomFileName = true)
        {
            string result = "";
            string path = "";
            try
            {
                //如果没有配置oss 则使用本地部署
                if (string.IsNullOrEmpty(WebCommon.GetConfigSetting("ossUpload")))
                {
                    return OssHelper.GetServerPath("", companyid, OssHelper.ossTempFileDownloadPath, directoryProductType, true, Path.GetFileName(filepath), filepath);//获取附件地址虚拟目录;
                }
                if (!string.IsNullOrEmpty(DirectoryPath))
                {
                    path = DirectoryPath;
                }
                else
                {
                    path = GetServerPath("/", companyid.ToString(), ossTempFileDownloadPath, directoryProductType, false);
                }
                //判断是否存在download文件夹
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(path)))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(path));
                }
                if (File.Exists(HttpContext.Current.Server.MapPath(path + Path.GetFileName(filepath))))
                {
                    //如果存在文件 则不下载
                    if (isNew == false)
                    {
                        return path + Path.GetFileName(filepath);
                    }
                }
                if (string.IsNullOrEmpty(filepath) || filepath == "/")
                {
                    return "";
                }
                string writepath = path + (randomFileName ? (WebCommon.GetRndString(20) + Path.GetExtension(filepath)) : Path.GetFileName(filepath));
                WebClient wc = new WebClient();
                wc.DownloadFile(ossDownload + filepath, HttpContext.Current.Server.MapPath(writepath));
                result = writepath;
                return result;
            }
            catch (WebException ex)
            {
                var rsp = ex.Response as HttpWebResponse;
                int httpStatusCode = rsp == null ? 0 : (int)rsp.StatusCode;
                switch (httpStatusCode)
                {
                    case 403:
                    case 404:
                        LogHelper.Error(ex);
                        LogHelper.Info("阿里云服务器文件下载失败 ! 文件路径 :" + filepath + " .错误原因 :" + ex.Message);
                        break;
                    default:
                        LogHelper.Info("阿里云服务器文件下载失败 ! 文件路径 :" + filepath + " .错误原因 :" + ex.Message);
                        throw ex;
                }
                result = "";
            }
            return result;
        }
        #endregion

        #region 创建服务器上传路径
        /// <summary>
        /// 创建服务器上传路径，只适用于存放到数据库中的路径。不用于保存时获取路径
        /// 潘锦发 20160708
        /// </summary>
        /// <param name="companyid">公司id</param>
        /// <param name="filename">文件名带后缀</param>
        /// <returns>返回物理文件路径</returns>
        public static string CreateUploadPath(string companyid,string filename=null)
        {
            string uploadroot = "upload";
            string uploadpath = companyid + "/OA/" + DateTime.Now.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            string webPath = uploadroot + "/" + uploadpath + "/";
            if (!string.IsNullOrEmpty(filename))
            {
                webPath += filename;
            }
            return webPath;
        } 
        #endregion
    }
}
