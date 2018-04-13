using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aliyun.OSS;
using System.Threading;
using System.IO;

namespace Kingsun.SynchronousStudy.Common
{
    public class OSSHelper
    {
        static string accessKeyId = System.Web.Configuration.WebConfigurationManager.AppSettings["accessKeyId"];//"LTAIfHqHXr5z8PiK";
        static string accessKeySecret = System.Web.Configuration.WebConfigurationManager.AppSettings["accessKeySecret"];// "BeyjGMSufJ2SZb2IZd3TZJl5xTduh2";
        static string endpoint = System.Web.Configuration.WebConfigurationManager.AppSettings["endpoint"];// "http://oss-cn-shenzhen.aliyuncs.com";
        static OssClient client = new OssClient(endpoint, accessKeyId, accessKeySecret);
        static ObjectMetadata metadata = new ObjectMetadata() { ContentEncoding = "utf-8" };

        #region 查看，创建，删除，检测存储空间(管理Bucket)

        /// <summary>
        /// 判断存储空间是否存在
        /// </summary>
        /// <param name="bucketName">存储空间的名称</param>
        public static bool DoesBucketExist(string bucketName)
        {
            try
            {
                return client.DoesBucketExist(bucketName);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 列出账户下所有的存储空间信息
        /// </summary>
        /// <returns>List<Bucket></returns>
        public static List<Bucket> ListBuckets()
        {
            try
            {
                List<Bucket> list = null;
                var buckets = client.ListBuckets();
                foreach (var bucket in buckets)
                {
                    list.Add(new Bucket()
                    {
                        CreationDate = bucket.CreationDate,
                        Location = bucket.Location,
                        Name = bucket.Name,
                        Owner = bucket.Owner
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 在OSS中创建一个新的存储空间。
        /// </summary>
        /// <param name="bucketName">要创建的存储空间的名称</param>
        public static bool CreateBucket(string bucketName)
        {
            try
            {
                bool flag = DoesBucketExist(bucketName);
                if (flag)
                {
                    return false;
                }
                var bucket = client.CreateBucket(bucketName);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 在OSS中创建一个新的存储空间。(访问权限)
        /// </summary>
        /// <param name="bucketName">要创建的存储空间的名称</param>
        /// <param name="acl">用户访问权限</param>
        /// <returns></returns>
        public static bool CreateBucket(string bucketName, CannedAccessControlList acl)
        {
            try
            {
                bool flag = DoesBucketExist(bucketName);
                if (flag)
                {
                    return false;
                }
                var bucket = client.CreateBucket(bucketName);
                client.SetBucketAcl(bucketName, acl);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 删除存储空间
        /// </summary>
        /// <param name="bucketName">存储空间的名称</param>
        public static bool DeleteBucket(string bucketName)
        {
            try
            {
                client.DeleteBucket(bucketName);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region 上传
        /// <summary>
        /// 上传指定的文件到指定的OSS的存储空间(如果key存在则会覆盖)
        /// </summary>
        /// <param name="bucketName">指定的存储空间名称</param>
        /// <param name="key">文件的在OSS上保存的名称</param>
        /// <param name="fileToUpload">指定上传文件的本地路径</param>
        public static bool PutObject(string bucketName, string key, string fileToUpload)
        {
            try
            {
                var result = client.PutObject(bucketName, key, fileToUpload, metadata);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        
        /// <summary>
        /// 上传指定的文件到指定的OSS的存储空间(如果key存在则会覆盖)
        /// </summary>
        /// <param name="bucketName">指定的存储空间名称</param>
        /// <param name="key">文件的在OSS上保存的名称</param>
        /// <param name="content">要上传的文件流</param>
     
        public static KingResponse PutObject(string bucketName, string key, Stream content)
        {
            try
            {             
                var result = client.PutObject(bucketName, key, content,metadata);
                return new KingResponse {  Success=true, Data=result.HttpStatusCode};
            }
            catch (Exception ex)
            {
                return new KingResponse {  Success=false, Data=ex.Message };
            }
        }
        /// <summary>
        /// 上传指定的文件到指定的OSS的存储空间(返回状态,0:失败，1:成功，2:key已存在)
        /// </summary>
        /// <param name="bucketName">指定的存储空间名称</param>
        /// <param name="key">文件的在OSS上保存的名称</param>
        /// <param name="fileToUpload">指定上传文件的本地路径</param>
        /// <returns>0:失败，1:成功，2:key已存在</returns>
        public static int PutObjectState(string bucketName, string key, string fileToUpload)
        {
            try
            {
                bool flag = CheckKeyExists(bucketName, key);
                if (flag)
                {
                    return 2;
                }
                var result = client.PutObject(bucketName, key, fileToUpload, metadata);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        static AutoResetEvent _event = new AutoResetEvent(false);

        /// <summary>
        /// 上传指定的文件到指定的OSS的存储空间(异步上传)
        /// </summary>
        /// <param name="bucketName">指定的存储空间名称</param>
        /// <param name="key">文件的在OSS上保存的名称</param>
        /// <param name="fileToUpload">指定上传文件的本地路径</param>
        public static bool AsyncPutObject(string bucketName, string key, string fileToUpload)
        {
            try
            {
                using (var fs = File.Open(fileToUpload, FileMode.Open))
                {
                    var metadata = new ObjectMetadata();
                    metadata.CacheControl = "No-Cache";
                    //metadata.ContentType = "image/jpg";
                    client.BeginPutObject(bucketName, key, fs, metadata, PutObjectCallback, new string('a', 8));
                    _event.WaitOne();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        private static void PutObjectCallback(IAsyncResult ar)
        {
            try
            {
                var result = client.EndPutObject(ar);
                Console.WriteLine("ETag:{0}", result.ETag);
                Console.WriteLine("User Parameter:{0}", ar.AsyncState as string);
                Console.WriteLine("Put object succeeded");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Put object failed, {0}", ex.Message);
            }
            finally
            {
                _event.Set();
            }
        }

        /// <summary>
        /// 断点续传
        /// </summary>
        /// <param name="bucketName">指定的存储空间名称。</param>
        /// <param name="key">保存到OSS上的名称。</param>
        /// <param name="fileToUpload">指定上传文件的路径。</param>
        /// <param name="checkpointDir">保存断点续传中间状态文件的目录，如果指定了，则会具有断点续传功能，否则会重新上传</param>
        public static bool ResumableUploadObject(string bucketName, string key, string fileToUpload, string checkpointDir)
        {
            try
            {
                client.ResumableUploadObject(bucketName, key, fileToUpload, null, checkpointDir);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region 下载
        /// <summary>
        /// 从指定的OSS存储空间中获取指定的文件
        /// </summary>
        /// <param name="bucketName">要获取的文件所在的存储空间的名称</param>
        /// <param name="key">要获取的文件的名称</param>
        /// <param name="fileToDownload">文件保存的本地路径</param>
        public static bool GetObject(string bucketName, string key, string fileToDownload)
        {
            try
            {
                var obj = client.GetObject(bucketName, key);
                using (var requestStream = obj.Content)
                {
                    byte[] buf = new byte[1024];
                    var fs = File.Open(fileToDownload, FileMode.OpenOrCreate);
                    var len = 0;
                    while ((len = requestStream.Read(buf, 0, 1024)) != 0)
                    {
                        fs.Write(buf, 0, len);
                    }
                    fs.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        #endregion

        #region 管理文件
        /// <summary>
        /// 列出指定存储空间下的文件的摘要信息OssObjectSummary列表
        /// </summary>
        /// <param name="bucketName">存储空间的名称</param>
        public static void ListObjects(string bucketName)
        {
            try
            {
                var listObjectsRequest = new ListObjectsRequest(bucketName);
                var result = client.ListObjects(listObjectsRequest);
                Console.WriteLine("List objects succeeded");
                foreach (var summary in result.ObjectSummaries)
                {
                    Console.WriteLine("File name:{0}", summary.Key);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("List objects failed. {0}", ex.Message);
            }
        }
        /// <summary>
        /// 判断某个存储空间下的key是否存在
        /// </summary>
        /// <param name="bucketName">存储空间的名称</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool CheckKeyExists(string bucketName, string key)
        {
            try
            {
                var obj = client.GetObject(bucketName, key);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static string GenerateIamgeUri(string bucketName, string key)
        {
            try
            {
                var req = new GeneratePresignedUriRequest(bucketName, key);
                //req.ContentType = "image/jpg";
                // 产生带有签名的URI
                var url = client.GeneratePresignedUri(req);
                if (url != null)
                {
                    return url.ToString().Split('?')[0];
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 获取Url链接
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string getUrl(string bucketName, string key)
        {
            // 设置URL过期时间 
            DateTime expiration = DateTime.Now.AddSeconds(100);
            // 生成URL
            Uri url = client.GeneratePresignedUri(bucketName, key, SignHttpMethod.Get);
            if (url != null)
            {
                return url.ToString().Split('?')[0];
            }
            return null;
        }

        /// <summary>
        /// 删除指定存储空间下的特定文件
        /// </summary>
        /// <param name="bucketName">存储空间的名称</param>
        /// <param name="key">文件的名称</param>
        /// <returns></returns>
        public static bool DeleteObject(string bucketName, string key)
        {
            try
            {
                client.DeleteObject(bucketName, key);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 删除指定存储空间下的特定文件
        /// </summary>
        /// <param name="bucketName">存储空间的名称</param> 
        /// <returns></returns>
        public static bool DeleteObjects(string bucketName)
        {
            try
            {
                var keys = new List<string>();
                var listResult = client.ListObjects(bucketName);
                foreach (var summary in listResult.ObjectSummaries)
                {
                    keys.Add(summary.Key);
                }
                var request = new DeleteObjectsRequest(bucketName, keys, false);
                client.DeleteObjects(request);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }

    #region Bucket

    // 摘要: 
    //     Bucket是OSS上的命名空间，可以理解为存储空间
    //
    // 备注: 
    //      Bucket名在整个 OSS 中具有全局唯一性，且不能修改；存储在OSS上的每个Object必须都包含在某个Bucket中。 一个应用，例如图片分享网站，可以对应一个或多个
    //     Bucket。一个用户最多可创建 10 个Bucket， 但每个Bucket 中存放的Object的数量和大小总和没有限制，用户不需要考虑数据的可扩展性。
    //     Bucket 命名规范 只能包括小写字母，数字和短横线（-） 必须以小写字母或者数字开头 长度必须在 3-63 字节之间
    public class Bucket
    {
        // 摘要: 
        //     获取/设置Bucket的创建时间。
        public DateTime CreationDate { get; set; }
        //
        // 摘要: 
        //     获取/设置Bucket的Location。
        public string Location { get; set; }
        //
        // 摘要: 
        //     获取/设置Bucket的名称。
        public string Name { get; set; }
        //
        // 摘要: 
        //     获取/设置Bucket的Aliyun.OSS.Bucket.Owner
        public Owner Owner { get; set; }
    }
    #endregion
}
