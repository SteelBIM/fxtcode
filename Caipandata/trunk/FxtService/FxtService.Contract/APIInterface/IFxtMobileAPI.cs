using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using System.Web.Mvc;
using FxtNHibernate.DTODomain.DATProjectDTO;
using FxtNHibernate.DTODomain.APIActualizeDTO;

/**
 * 作者: 曾智磊
 * 时间:2014-03-03
 * 摘要:新建手机端API公共wcf入口APIActualize.FxtAPI契约(接口)
 * **/
namespace FxtService.Contract.APIInterface
{
    [ServiceContract()]

    public interface IFxtMobileAPI
    {
        #region 公共入口
        [OperationContract]
        [WebInvoke(
            UriTemplate = "Entrance/{type}/{name}",
            Method = "POST", 
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest
            )]
        ResultData Entrance(string type, string name, string date, string code, string parameter);
        //[OperationContract]
        //[WebInvoke(
        //    UriTemplate = "GetFile",
        //    Method = "POST",
        //    ResponseFormat = WebMessageFormat.Json
        //    )]
        //ResultData GetFile(System.IO.Stream stream);

        [OperationContract]
        [WebInvoke(
            UriTemplate = "cart",
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest
            )]
        string aa(string b);
        #endregion
    }
}
