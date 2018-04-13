using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aliyun.OSS;
using Aliyun.OSS.Common;

namespace Kingsun.SynchronousStudyHopeChina.Web
{
    public partial class FileUpload : System.Web.UI.Page
    {
        const string AccessKeyId = "LTAIfHqHXr5z8PiK";
        const string AccessKeySecret = "BeyjGMSufJ2SZb2IZd3TZJl5xTduh2";
        const string Endpoint = "http://synchronousstudy.oss-cn-shenzhen.aliyuncs.com";

        /// <summary>
        /// 由用户指定的OSS访问地址、阿里云颁发的AccessKeyId/AccessKeySecret构造一个新的OssClient实例。
        /// </summary>
        OssClient ossClient = new OssClient(Endpoint, AccessKeyId, AccessKeySecret);

        ClientConfiguration conf = new ClientConfiguration();
       
        protected void Page_Load(object sender, EventArgs e)
        {
            conf.MaxErrorRetry = 3;     //设置请求发生错误时最大的重试次数
            conf.ConnectionTimeout = 300;  //设置连接超时时间
            conf.SetCustomEpochTicks(1000);        //设置自定义基准时间
        }
    }
}