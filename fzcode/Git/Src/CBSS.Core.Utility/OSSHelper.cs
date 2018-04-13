using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Aliyun.OSS;

namespace CBSS.Core.Utility
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“OSSHelper”的 XML 注释
    public class OSSHelper
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“OSSHelper”的 XML 注释
    {
        /// <summary>
        /// 获取验证签名
        /// </summary>
        /// <param name="accessKeyId"></param>
        /// <param name="accessKeySecret"></param>
        /// <param name="host">OSS地址(http://fzyouke.oss-cn-shenzhen.aliyuncs.com)</param>
        /// <param name="bucketName">存储空间名称</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetPostObjectSignature(string accessKeyId,string accessKeySecret,string host,string bucketName)
        {
            try
            {
                //string accessKeyId = "LTAIVaiUNugzG5Wm"; 
                //string accessKeySecret = "lEWqqIK4xFKmtgc1ur3gskaiuH7IQC";
                const string endpoint = "http://oss-cn-shenzhen.aliyuncs.com";

                var ossClient = new OssClient(endpoint, accessKeyId, accessKeySecret);
                //第一步，构造policy
                bucketName = bucketName+"/"+ DateTime.Now.Year.ToString()+"/"+Path(DateTime.Now.Month) + "/" + Path(DateTime.Now.Day) +"/";//这个参数是设置用户上传指定存储空间名称，必须以斜线结尾
                var expiration = Convert.ToDateTime("2020-01-01T12:00:00.000Z"); DateTime.Now.AddMinutes(100);//到期时间 暂设2020年
                var policyConds = new PolicyConditions();
                policyConds.AddConditionItem(MatchMode.StartWith, PolicyConditions.CondKey, bucketName);//上传目录
                policyConds.AddConditionItem(PolicyConditions.CondContentLengthRange, 1, 1048576000);//允许上传的文件大小限制1000M
                var postPolicy = ossClient.GeneratePostPolicy(expiration, policyConds);//给policyConds添加过期时间并json序列化（格式iso8601:"yyyy-MM-dd'T'HH:mm:ss.fff'Z'"）
                var base64Policy = Convert.ToBase64String(Encoding.UTF8.GetBytes(postPolicy));
                //以下返回给前端
                TimeSpan ts = expiration - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                var expire = Convert.ToInt64(ts.TotalSeconds);

                Dictionary<string, object> response = new Dictionary<string, object>();
                response["accessid"] = accessKeyId;
                response["accesskey"] = accessKeySecret;
                response["host"] = host;// "http://fzyouke.oss-cn-shenzhen.aliyuncs.com";
                response["policy"] = base64Policy;
                //response["signature"] = signature; 签名在js生成
                response["expire"] = expire;
                response["dir"] = bucketName;  
                return response;
            }
            catch
            {
                return null;
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“OSSHelper.Path(int)”的 XML 注释
        public static string Path(int time)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“OSSHelper.Path(int)”的 XML 注释
        {
            if (time > 9)
                return time.ToString();
            return "0" + time.ToString();
        }
    }
}
