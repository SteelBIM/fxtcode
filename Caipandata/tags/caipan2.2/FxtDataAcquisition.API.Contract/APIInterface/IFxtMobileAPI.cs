using FxtDataAcquisition.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace FxtDataAcquisition.API.Contract.APIInterface
{
    [ServiceContract()]
    public interface  IFxtMobileAPI
    {
        #region 公共入口
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/{type}",
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest
            )]
        ResultData Entrance(string type, string sinfo, string info);//(string type, string name, string date, string code, string parameter);
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/{type}/upload?sinfo={sinfo}&info={info}",
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest
            )]
        ResultData UpLoadFile(System.IO.Stream stream, string type, string sinfo, string info);//(string type, string name, string date, string code, string parameter);


        //[OperationContract]
        //[WebInvoke(
        //    UriTemplate = "/UploadFile?aa={filename}",
        //    Method = "POST",
        //    RequestFormat=WebMessageFormat.Json,
        //    ResponseFormat=WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest
        //    )]
        //string UploadFile_bak(System.IO.Stream stream,string filename);

        //[OperationContract]
        //[WebInvoke(
        //    UriTemplate = "/GetFileSeries",
        //    Method = "POST",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest
        //    )]
        //ResultData GetFileSeries_bak(string filename);


        //[OperationContract]
        //[WebInvoke(
        //    UriTemplate = "/UpLoadFileSeries?filename={filename}&npos={npos}",
        //    Method = "POST",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest
        //    )]
        //ResultData UpLoadFileSeries_bak(System.IO.Stream stream, string filename, string npos);


        #endregion
    }
}
