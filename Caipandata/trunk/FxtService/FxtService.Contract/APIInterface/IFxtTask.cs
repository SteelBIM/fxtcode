using FxtNHibernate.DTODomain.APIActualizeDTO;
using FxtNHibernate.FxtLoanDomain.Entities;
using FxtService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

/*作者:李晓东
 * 时间:2014.05.23
 * 摘要:新建
 * **/
namespace FxtService.Contract.APIInterface
{
    [ServiceContract]
    public interface IFxtTask
    {
        /// <summary>
        /// 任务修改
        /// </summary>
        /// <param name="task">任务模型</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "TU/",
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest
            )]
        ResultData TaskUpdate(string data);

        /// <summary>
        /// 修改任务成功及失败条数
        /// </summary>
        /// <param name="data">任务模型</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "TUSF/",
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest
            )]
        ResultData TaskSuccessFail(string data);

        /// <summary>
        /// 根据任务ID获得任务
        /// </summary>
        /// <param name="id">任务ID</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "TaskById/{id}",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest
            )]
        ResultData GetTaskById(string id);
        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "TList/",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest
            )]
        ResultData TaskList();

        /// <summary>
        /// 新增任务日志
        /// </summary>
        /// <param name="data">任务日志实体</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "TLC/",
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest
            )]
        ResultData TaskLogAdd(string data);

         /// 获取要复估的押品列表
        /// </summary>
        /// <param name="id">文件ID</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "CList/{id}",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest
            )]
        ResultData GetDataCollateral(string id);

        /// <summary>
        /// 押品拆分保存
        /// </summary>
        /// <param name="dataCollateral">押品对象</param>
        /// <param name="uploadFileId">文件ID</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "CreateCollateral/",
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest
            )]
        ResultData DataCollateralAdd(string data);

        /// <summary>
        /// 复估押品
        /// </summary>
        /// <param name="id">押品编号</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "Calculation/{id}",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest
            )]
        ResultData RunCalculation(string id);
    }
}
