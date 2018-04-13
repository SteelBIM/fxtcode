using CourseActivate.Core.Utility;
using CourseActivate.Framework.BLL;
using CourseActivate.Framework.DAL;
using CourseActivate.Resource.Constract.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestLog4Net;

namespace CourseActivate.Resource.BLL
{
    public class ResourceBLL : Manage
    {
        public tb_res_resource GetResourceByModulerID(int modulerid, int CourseID)
        {
            DoRedisString redis = new DoRedisString();
            tb_res_resource resinfo = redis.Get<tb_res_resource>(RedisConfiguration.ResourceKey + CourseID + ":" + modulerid);
            if (resinfo == null)
            {
                resinfo = SelectSearch<tb_res_resource>(i => i.ModularID == modulerid && i.BookID == CourseID).FirstOrDefault();
                if (resinfo != null)
                {
                    redis.Set<tb_res_resource>(RedisConfiguration.ResourceKey + CourseID + ":" + modulerid, resinfo);
                }
            }
            return resinfo;
        }

        /// <summary>
        /// 获取最新的APP版本信息
        /// </summary>
        /// <param name="AppID"></param>
        /// <returns></returns>
        public tb_res_appversion GetAppNewVersionByAppID(Guid AppID)
        {
            DoRedisString redis = new DoRedisString();
            tb_res_appversion app = redis.Get<tb_res_appversion>(RedisConfiguration.AppVersionKey + AppID);
            if (app == null)
            {
                app = SelectSearch<tb_res_appversion>(i => i.AppID == AppID && i.Status == 1).OrderByDescending(i => i.CreateDate).FirstOrDefault();
                if (app != null)
                {
                    redis.Set<tb_res_appversion>(RedisConfiguration.AppVersionKey + AppID, app);
                }
            }
            return app;
        }


        public bool AddResource(tb_res_resource res)
        {
            var existRes = SelectSearch<tb_res_resource>(o => o.BookID == res.BookID && o.ModularID == res.ModularID);

            if (existRes != null && existRes.Any())
            {//update
                res.ResID = existRes[0].ResID;
                return UpdateResource(res);
            }

            res.ResVersion = "V1.0.0";

            var modular = Select<tb_res_modular>(res.ModularID);
            if (modular != null)
                res.ModularName = modular.ModularName;
            res.CreateTime = DateTime.Now;
            res.ResKey = MD5Helper.GetMD5HashFromString(res.BookID + "-" + res.ModularID + "-" + res.ResVersion);
            res.ResKey = "kingsoft";
            int r = Add<tb_res_resource>(res);
            res.ResID = r;
            if (r > 0)
            {
                ///添加资源到缓存redis
                ResetRedisData(res);

                //同步到api服务器
                string resRoot = ConfigurationManager.AppSettings["Webhost"];
                CopyFileRequest(res.ResUrl.Replace("@/", resRoot + "/"));
            }
            return r > 0;
        }

        public  string HttpGet(string url)
        {
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
        /// <summary>
        /// 同步资源到目标服务器
        /// </summary>
        /// <param name="fileUrl">源服务器的文件,如:http://http://183.47.42.221/Upload/BookResource/1.json </param>
        public void CopyFileRequest(string fileUrl)
        {
            try
            {
                string copyRoot = ConfigurationManager.AppSettings["ResourceCopy"];
                if (copyRoot == null)
                {
                    throw new Exception("配置结点ResourceCopy未找到");
                }
                copyRoot = copyRoot.Trim();
                if (!string.IsNullOrEmpty(copyRoot))
                {
                    string[] copyRoots = copyRoot.Split(';');
                    WebClient client = new WebClient();

                    foreach (string root in copyRoots)
                    {
                        if (root.Any())
                        {
                            LogHelper.Info("同步文件到站点:" + root);
                            LogHelper.Info("源文件:" + fileUrl);
                            string address = root + "/api/" + "CopyFile/CopyFile?fileUrl=" + fileUrl;
                            var success = client.DownloadString(address);//发送请求
                            LogHelper.Info("结束同步文件到站点:" + root + ",执行结果:" + success);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info("同步文件到目标服务器出现异常:" + ex.Message + ",源服务器文件:" + fileUrl);
            }
        }

        /// <summary>
        /// 删除目标服务器文件(目前主要是删除.json,资源不删)
        /// </summary>
        /// <param name="fileUrl">源服务器的文件,如:http://http://183.47.42.221/Upload/BookResource/1.json </param>
        public void DeleteFileRequest(string fileUrl)
        {            
                try
                {
                    string copyRoot = ConfigurationManager.AppSettings["ResourceCopy"];
                    if (copyRoot == null)
                    {
                        throw new Exception("配置结点ResourceCopy未找到");
                    }
                    copyRoot = copyRoot.Trim();
                    if (!string.IsNullOrEmpty(copyRoot))
                    {
                        string[] copyRoots = copyRoot.Split(';');
                        WebClient client = new WebClient();
                        foreach (string root in copyRoots)
                        {
                            if (root.Any())
                            {
                                LogHelper.Info("开始删除站点:" + root + "的文件");
                                LogHelper.Info("源文件:" + fileUrl);
                                string address = root + "/api/" + "CopyFile/CopyFile?fileUrl=" + fileUrl;
                                var success = client.DownloadString(address);//发送请求
                                LogHelper.Info("结束删除站点:" + root + "的文件,执行结果:" + success);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Info("删除目标服务器文件出现异常:" + ex.Message + ",源服务器文件:" + fileUrl);
                }
            
        }

        public void GetResJson(int bookid, string path)
        {
            var resources = SelectSearch<tb_res_resource>(o => o.BookID == bookid);
            var modularIds = resources.Select(o => o.ModularID).ToArray();
            //   var modulars = SelectSearch<tb_res_modular>(o => modularIds.Contains(o.ModularID));
            var catalogs = SelectSearch<tb_res_catalog>(o => o.BookID == bookid);
            var book = Select<tb_res_book>(bookid);
            if (book == null || book.Status != 1)//不是启用状态,删除json
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                DeleteFileRequest(ConfigurationManager.AppSettings["Webhost"] + "/Upload/BookResource/" + bookid + ".json");
                return;
            }
            //启用状态,产生json
            string json = JsonHelper.EncodeJson(
                new
                {
                    book.BookID,
                    book.BookName,
                    book.BookCover,
                    book.PeriodName,
                    book.GradeName,
                    book.ReelName,
                    book.SubjectName,
                    book.EditionName,
                    book.PublishidName,
                    book.EPlate,
                    Modulars = resources.Where(o => o.Status == 1).Select(o => new { ModularID = o.ModularID, ModularName = o.ModularName, ResUrl = o.ResUrl, ResMD5 = o.ResMD5, ResVersion = o.ResVersion, ResKey = o.ResKey }).ToArray(),
                    Catalogs = catalogs.Select(o => new { CatalogID = o.CatalogID, CatalogName = o.CatalogName, CatalogLevel = o.CatalogLevel, PageNoStart = o.PageNoStart, PageNoEnd = o.PageNoEnd, ParentID = o.ParentID, Sort = o.Sort })
                });
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            FileOperate.WriteFile(path, json);
            //同步.json
            string resRoot = ConfigurationManager.AppSettings["Webhost"] + "/Upload/BookResource/";
            CopyFileRequest(resRoot + "/" + bookid + ".json");
        }

        public bool UpdateResource(tb_res_resource res)
        {
            var ver = Select<tb_res_resource>(res.ResID.Value).ResVersion.Replace("V", "").Replace(".", "");
            string newVersion = Convert.ToString(int.Parse(ver) + 1);
            newVersion = "V" + newVersion[0] + "." + newVersion[1] + "." + newVersion[2];
            var modular = Select<tb_res_modular>(res.ModularID);
            if (modular != null)
                res.ModularName = modular.ModularName;
            res.ResVersion = newVersion;
            res.UpdateTime = DateTime.Now;
            res.ResKey = MD5Helper.GetMD5HashFromString(res.BookID + "-" + res.ModularID + "-" + res.ResVersion);

            res.ResKey = "kingsoft";
            bool r = Update<tb_res_resource>(res);
            if (r)
            {
                //修改资源到缓存
                ResetRedisData(res);

                //同步到api服务器
                string resRoot = ConfigurationManager.AppSettings["Webhost"];
                CopyFileRequest(res.ResUrl.Replace("@/",resRoot+"/"));
            }
            return r;
        }

        /// <summary>
        /// 重置redis的资源信息缓存
        /// </summary>
        /// <param name="resinfo"></param>
        public void ResetRedisData(tb_res_resource resinfo)
        {
            new DoRedisString().Set<tb_res_resource>(RedisConfiguration.ResourceKey + resinfo.BookID + ":" + resinfo.ModularID, resinfo);
        }

        /// <summary>
        /// 移除redis资源缓存信息
        /// </summary>
        /// <param name="resinfo"></param>
        public void RemoveRedisData(tb_res_resource resinfo)
        {
            if (resinfo != null)
                new DoRedisString().Remove(RedisConfiguration.GetResourceKey(resinfo.BookID.Value, resinfo.ModularID.Value));
        }
    }
}
