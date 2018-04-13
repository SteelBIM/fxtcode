using Kingsun.SynchronousStudy.App.Common;
using Kingsun.SynchronousStudy.Common;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Configuration;
using System.Web.Http;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common.Base;

namespace Kingsun.SynchronousStudy.App.Controllers
{
    public class ExamPaperController : ApiController
    {
        private ExamPaperBLL nlr = new ExamPaperBLL();
        readonly string _getOssFilesUrl = WebConfigurationManager.AppSettings["getOssFiles"];
        readonly string _getFilesUrl = WebConfigurationManager.AppSettings["getFiles"];
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 获取期末模拟测试卷
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetExamPaperListTest()
        {
            try
            {
                ExamPaperListRequset submitData = new ExamPaperListRequset();
                submitData.ClassShortID = "42890438";
                submitData.BookID = "168";
                submitData.pageNumber = 0;
                submitData.Version = "v1";

                HttpResponseMessage htm = new HttpResponseMessage();
                switch (submitData.Version.ToUpper())
                {
                    case "V1":
                        htm = nlr.GetExamPaperList(submitData.BookID, submitData.ClassShortID);
                        break;
                    case "V2":
                        break;
                    case "V3":
                        break;
                    case "V4":
                        break;
                    default:
                        htm = ObjectToJson.GetErrorResult("版本不存在！");
                        break;
                }
                return htm;
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetResult("", "没有更多数据");
        }

        /// <summary>
        /// 获取期末模拟测试卷
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetExamPaperList([FromBody]KingRequest request)
        {
            try
            {
                ExamPaperListRequset submitData = JsonHelper.DecodeJson<ExamPaperListRequset>(request.Data);
                if (submitData == null)
                {
                    return ObjectToJson.GetErrorResult("当前信息为空");
                }
                if (string.IsNullOrEmpty(submitData.BookID))
                {
                    return ObjectToJson.GetErrorResult("书籍ID不能为空");
                }
                if (string.IsNullOrEmpty(submitData.ClassShortID))
                {
                    return ObjectToJson.GetErrorResult("班级ID不能为空！");
                }
                //if (submitData.pageNumber < 0)
                //{
                //    return ObjectToJson.GetErrorResult("页码不能小于0！");
                //}
                if (string.IsNullOrEmpty(submitData.Version))
                {
                    return ObjectToJson.GetErrorResult("版本参数不能为空！");
                }

                HttpResponseMessage htm = new HttpResponseMessage();
                switch (submitData.Version.ToUpper())
                {
                    case "V1":
                        htm = nlr.GetExamPaperList(submitData.BookID, submitData.ClassShortID);
                        break;
                    case "V2":
                        break;
                    case "V3":
                        break;
                    case "V4":
                        break;
                    default:
                        htm = ObjectToJson.GetErrorResult("版本不存在！");
                        break;
                }
                return htm;
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetResult("", "没有更多数据");
        }


        /// <summary>
        /// 获取班级期末测评详细成绩信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetUserExamInfoTest()
        {
            UserInfoList submitData = new UserInfoList();//JsonHelper.DecodeJson<UserInfoList>(request.Data);
            submitData.ClassId = "02976526";
            submitData.CatalogID = "375";
            submitData.pageNumber = 0;
            submitData.Version = "V1";


            HttpResponseMessage htm = new HttpResponseMessage();
            switch (submitData.Version.ToUpper())
            {
                case "V1":
                    htm = nlr.GetUserExamInfo(submitData.ClassId, submitData.CatalogID, submitData.pageNumber);
                    break;
                case "V2":
                    break;
                case "V3":
                    break;
                case "V4":
                    break;
                default:
                    htm = ObjectToJson.GetErrorResult("版本不存在！");
                    break;
            }
            return htm;

        }


        /// <summary>
        /// 获取班级期末测评详细成绩信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetUserExamInfo([FromBody] KingRequest request)
        {
            try
            {
                UserInfoList submitData = JsonHelper.DecodeJson<UserInfoList>(request.Data);
                if (submitData == null)
                {
                    return ObjectToJson.GetErrorResult("当前信息为空");
                }
                if (string.IsNullOrEmpty(submitData.ClassId))
                {
                    return ObjectToJson.GetErrorResult("班级ID不能为空！");
                }
                if (string.IsNullOrEmpty(submitData.CatalogID))
                {
                    return ObjectToJson.GetErrorResult("试卷ID不能为空！");
                }
                if (submitData.pageNumber < 0)
                {
                    return ObjectToJson.GetErrorResult("页码不能小于0！");
                }
                if (string.IsNullOrEmpty(submitData.Version))
                {
                    return ObjectToJson.GetErrorResult("版本参数不能为空！");
                }

                HttpResponseMessage htm = new HttpResponseMessage();
                switch (submitData.Version.ToUpper())
                {
                    case "V1":
                        htm = nlr.GetUserExamInfo(submitData.ClassId, submitData.CatalogID, submitData.pageNumber);
                        break;
                    case "V2":
                        break;
                    case "V3":
                        break;
                    case "V4":
                        break;
                    default:
                        htm = ObjectToJson.GetErrorResult("版本不存在！");
                        break;
                }
                return htm;
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetResult("", "没有更多数据");
        }
    }

    public class UserInfoList
    {
        public int pageNumber { get; set; }
        public string ClassId { get; set; }
        public string CatalogID { get; set; }
        public string Version { get; set; }
    }

    /// <summary>
    /// 获取期末模拟测试卷 请求参数
    /// </summary>
    public class ExamPaperListRequset
    {
        public string BookID { get; set; }
        public string ClassShortID { get; set; }
        public int pageNumber { get; set; }
        public string Version { get; set; }
    }
    /// <summary>
    /// 获取期末模拟测试卷 返回参数
    /// </summary>
    public class ExamPaperListModel
    {
        public int CatalogID { get; set; }
        public string CatalogName { get; set; }
        /// <summary>
        /// 班级总人数
        /// </summary>
        public int ClassNum { get; set; }
        /// <summary>
        /// 做题人数
        /// </summary>
        public int QuestionNum { get; set; }
    }

    public class Tb_StuCatalog
    {
        public string StuCatID { get; set; }
        public string StuID { get; set; }
        public int CatalogID { get; set; }
        public double BestTotalScore { get; set; }
        public DateTime DoDate { get; set; }
        public int AnswerNum { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserImg { get; set; }
        public int IsEnableOss { get; set; }
        public string NickName { get; set; }
        public string TrueName { get; set; }
    }

    public class UserExamInfo
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CreateTime { get; set; }
        public string UserImg { get; set; }
        public string StuCatID { get; set; }
        public double Score { get; set; }
        public int AnswerNum { get; set; }
        public bool IsStudy { get; set; }
    }

}
