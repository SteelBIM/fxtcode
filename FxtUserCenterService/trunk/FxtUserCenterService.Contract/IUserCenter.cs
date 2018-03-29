using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using CAS.Entity;

namespace FxtUserCenterService.Contract
{
    [ServiceContract]
    public interface IUserCenter
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "active"
            , Method = "POST"
            , RequestFormat = WebMessageFormat.Json
            , ResponseFormat = WebMessageFormat.Json
            , BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        WCFJsonData Entrance(string sinfo, string info);

        [OperationContract]
        [WebInvoke(UriTemplate = "start"
            , Method = "POST"
            , RequestFormat = WebMessageFormat.Json
            , ResponseFormat = WebMessageFormat.Json
            , BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        WCFJsonData UserLogin(string sinfo, string info);

        [OperationContract]
        [WebInvoke(UriTemplate = "cmbstart"
            , Method = "POST"
            , RequestFormat = WebMessageFormat.Json
            , ResponseFormat = WebMessageFormat.Json
            , BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        WCFJsonData CmbUserLogin(string sinfo, string info);

        /// <summary>
        /// 联系我们
        /// </summary>
        /// <param name="tl">标题</param>
        /// <param name="ct">内容</param>
        /// <param name="to">发送邮件的类型</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "cu?callback={callback}&tl={tl}&ct={ct}&to={to}"
            , Method = "GET"
            , RequestFormat = WebMessageFormat.Json
            , ResponseFormat = WebMessageFormat.Json
            , BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        WCFJsonData ConnectUs(string tl, string ct, string to, string callback);
    }
}
