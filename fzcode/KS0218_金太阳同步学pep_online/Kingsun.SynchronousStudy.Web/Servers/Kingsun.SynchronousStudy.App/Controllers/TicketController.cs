using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Configuration;
using System.Web.Http;
using Kingsun.SynchronousStudy.App.Common;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using Kingsun.SynchronousStudy.BLL;
using System.Web.Script.Serialization;
using log4net;
using Kingsun.SynchronousStudy.Common.Base;

namespace Kingsun.SynchronousStudy.App.Controllers
{
    public class TicketController : ApiController
    {
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private TicketBLL nlr = new TicketBLL();

        /// <summary>
        /// 根据书籍ID获取优惠券信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetTicketInfoByBookId([FromBody]KingRequest request)
        {
            try
            {
                GetUserTicketList submitData = JsonHelper.DecodeJson<GetUserTicketList>(request.Data);
                if (submitData == null)
                {
                    return ObjectToJson.GetErrorResult("当前信息为空");
                }
                if (string.IsNullOrEmpty(submitData.CourseID))
                {
                    return ObjectToJson.GetErrorResult("书籍ID不能为空");
                }
                if (string.IsNullOrEmpty(submitData.Version))
                {
                    return ObjectToJson.GetErrorResult("版本参数不能为空！");
                }
                HttpResponseMessage htm = new HttpResponseMessage();
                switch (submitData.Version.ToUpper())
                {
                    case "V1":
                        htm = nlr.GetTicketInfoByBookId(submitData.CourseID);
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
                return ObjectToJson.GetErrorResult("没有更多数据");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetTicketInfoByBookIdTest()
        {
            try
            {
                GetUserTicketList submitData = new GetUserTicketList();//JsonHelper.DecodeJson<GetUserTicketList>(request.Data);
                submitData.CourseID = "265";
                submitData.AppId = "1548d0a3-ca8e-4702-9c2c-f0ba0cacd385";
                submitData.Version = "v1";
                HttpResponseMessage htm = new HttpResponseMessage();
                switch (submitData.Version.ToUpper())
                {
                    case "V1":
                        htm = nlr.GetTicketInfoByBookId(submitData.CourseID);
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
                return ObjectToJson.GetErrorResult("没有更多数据");
            }
        }

        /// <summary>
        /// 根据书籍ID获取优惠券信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetUserTicketInfoByUserId([FromBody]KingRequest request)
        {
            try
            {
                GetUserTicketList submitData = JsonHelper.DecodeJson<GetUserTicketList>(request.Data);
                if (submitData == null)
                {
                    return ObjectToJson.GetErrorResult("当前信息为空");
                }
                if (string.IsNullOrEmpty(submitData.UserID))
                {
                    return ObjectToJson.GetErrorResult("用户ID不能为空");
                }
                if (string.IsNullOrEmpty(submitData.Version))
                {
                    return ObjectToJson.GetErrorResult("版本参数不能为空！");
                }
                HttpResponseMessage htm = new HttpResponseMessage();
                switch (submitData.Version.ToUpper())
                {
                    case "V1":
                        htm = nlr.GetUserTicketInfoByUserId(submitData.UserID, submitData.CourseID);
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
                return ObjectToJson.GetErrorResult("没有更多数据");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetUserTicketInfoByUserIdTest()
        {
            try
            {
                GetUserTicketList submitData = new GetUserTicketList();//JsonHelper.DecodeJson<GetUserTicketList>(request.Data);
                submitData.UserID = "1000137999";
                submitData.AppId = "1548d0a3-ca8e-4702-9c2c-f0ba0cacd385";
                submitData.Version = "v1";
                HttpResponseMessage htm = new HttpResponseMessage();
                switch (submitData.Version.ToUpper())
                {
                    case "V1":
                        htm = nlr.GetUserTicketInfoByUserId(submitData.UserID, submitData.CourseID);
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
                return ObjectToJson.GetErrorResult("没有更多数据");
            }
        }

        /// <summary>
        /// 添加用户优惠券信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage InsertUserTicketInfo([FromBody]KingRequest request)
        {
            try
            {
                InsertUserTicket submitData = JsonHelper.DecodeJson<InsertUserTicket>(request.Data);
                if (submitData == null)
                {
                    return ObjectToJson.GetErrorResult("当前信息为空");
                }
                if (string.IsNullOrEmpty(submitData.UserID.ToString()))
                {
                    return ObjectToJson.GetErrorResult("用户ID不能为空");
                }
                if (string.IsNullOrEmpty(submitData.TicketID.ToString()))
                {
                    return ObjectToJson.GetErrorResult("优惠券ID不能为空");
                }
                if (string.IsNullOrEmpty(submitData.Version))
                {
                    return ObjectToJson.GetErrorResult("版本参数不能为空！");
                }
                HttpResponseMessage htm = new HttpResponseMessage();
                TB_UserTicket userticket = new TB_UserTicket();
                userticket.Status = 0;
                userticket.TicketID = submitData.TicketID;
                userticket.CreateTime = DateTime.Now;
                userticket.UserID = submitData.UserID;
                switch (submitData.Version.ToUpper())
                {
                    case "V1":
                        htm = nlr.InsertUserTicketInfo(userticket);
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
                return ObjectToJson.GetErrorResult("没有更多数据");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage InsertUserTicketInfoTest()
        {
            try
            {
                InsertUserTicket submitData = new InsertUserTicket();//JsonHelper.DecodeJson<GetUserTicketList>(request.Data);
                submitData.UserID = 1000137999;
                submitData.TicketID = 45;
                submitData.Version = "v1";
                HttpResponseMessage htm = new HttpResponseMessage();
                TB_UserTicket userticket = new TB_UserTicket();
                userticket.Status = 0;
                userticket.TicketID = submitData.TicketID;
                userticket.CreateTime = DateTime.Now;
                userticket.UserID = submitData.UserID;
                switch (submitData.Version.ToUpper())
                {
                    case "V1":
                        htm = nlr.InsertUserTicketInfo(userticket);
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
                return ObjectToJson.GetErrorResult("没有更多数据");
            }
        }

        /// <summary>
        /// 根据课程id获取优惠券信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetTicketByCourseId([FromBody]KingRequest request)
        {
            try
            {
                GetUserTicketList submitData = JsonHelper.DecodeJson<GetUserTicketList>(request.Data);
                if (submitData == null)
                {
                    return ObjectToJson.GetErrorResult("当前信息为空");
                }
                if (string.IsNullOrEmpty(submitData.CourseID))
                {
                    return ObjectToJson.GetErrorResult("书籍ID不能为空");
                }
                if (string.IsNullOrEmpty(submitData.Version))
                {
                    return ObjectToJson.GetErrorResult("版本参数不能为空！");
                }
                HttpResponseMessage htm = new HttpResponseMessage();
                switch (submitData.Version.ToUpper())
                {
                    case "V1":
                        htm = nlr.GetTicketByCourseId(submitData.CourseID.ToString());
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
                return ObjectToJson.GetErrorResult("没有更多数据");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetTicketByCourseIdTest()
        {
            try
            {
                GetUserTicketList submitData = new GetUserTicketList();//JsonHelper.DecodeJson<TB_Ticket>(request.Data);
                submitData.CourseID = "168";
                submitData.Version = "V1";
                HttpResponseMessage htm = new HttpResponseMessage();
                switch (submitData.Version.ToUpper())
                {
                    case "V1":
                        htm = nlr.GetTicketByCourseId(submitData.CourseID.ToString());
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
                return ObjectToJson.GetErrorResult("没有更多数据");
            }
        }


        /// <summary>
        /// 根据UserID获取用户优惠券
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetUserTicketByUserId([FromBody]KingRequest request)
        {
            try
            {
                GetUserTicketList submitData = JsonHelper.DecodeJson<GetUserTicketList>(request.Data);
                if (submitData == null)
                {
                    return ObjectToJson.GetErrorResult("当前信息为空");
                }
                if (string.IsNullOrEmpty(submitData.UserID))
                {
                    return ObjectToJson.GetErrorResult("用户ID不能为空");
                }
                if (string.IsNullOrEmpty(submitData.AppId))
                {
                    return ObjectToJson.GetErrorResult("AppID不能为空");
                }
                if (string.IsNullOrEmpty(submitData.Version))
                {
                    return ObjectToJson.GetErrorResult("版本参数不能为空！");
                }
                HttpResponseMessage htm = new HttpResponseMessage();
                switch (submitData.Version.ToUpper())
                {
                    case "V1":
                        htm = nlr.GetUserTicketByUserId(submitData.UserID.ToString(), submitData.AppId);
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
                return ObjectToJson.GetErrorResult("没有更多数据");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetUserTicketByUserIdTest()
        {
            try
            {
                GetUserTicketList submitData = new GetUserTicketList();//JsonHelper.DecodeJson<TB_UserTicket>(request.Data);
                //if (submitData == null)
                //{
                //    return ObjectToJson.GetErrorResult("当前信息为空");
                //}
                //if (submitData.UserID <= 0)
                //{
                //    return ObjectToJson.GetErrorResult("书籍ID不能为空");
                //}
                //if (string.IsNullOrEmpty(submitData.Version))
                //{
                //    return ObjectToJson.GetErrorResult("版本参数不能为空！");
                //}
                submitData.UserID = "537118551";
                submitData.Version = "V1";
                submitData.AppId = "1528d0a3-ca8e-4702-9c2c-f0ba0cacd385";
                HttpResponseMessage htm = new HttpResponseMessage();
                switch (submitData.Version.ToUpper())
                {
                    case "V1":
                        htm = nlr.GetUserTicketByUserId(submitData.UserID.ToString(), submitData.AppId);
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
                return ObjectToJson.GetErrorResult("没有更多数据");
            }
        }

        /// <summary>
        /// 根据UserID,totalScore获取用户是否拥有优惠券
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage IsUserTicketByUserIdAndTotalscore([FromBody]KingRequest request)
        {
            try
            {
                GetUserTicketList submitData = JsonHelper.DecodeJson<GetUserTicketList>(request.Data);
                if (submitData == null)
                {
                    return ObjectToJson.GetErrorResult("当前信息为空");
                }
                if (string.IsNullOrEmpty(submitData.UserID))
                {
                    return ObjectToJson.GetErrorResult("用户ID不能为空");
                }
                if (string.IsNullOrEmpty(submitData.TotalScore))
                {
                    return ObjectToJson.GetErrorResult("成绩不能为空");
                }
                if (string.IsNullOrEmpty(submitData.AppId))
                {
                    return ObjectToJson.GetErrorResult("AppID不能为空");
                }
                if (string.IsNullOrEmpty(submitData.CourseID))
                {
                    return ObjectToJson.GetErrorResult("书籍ID不能为空");
                }
                if (string.IsNullOrEmpty(submitData.Version))
                {
                    return ObjectToJson.GetErrorResult("版本参数不能为空！");
                }
                HttpResponseMessage htm = new HttpResponseMessage();
                switch (submitData.Version.ToUpper())
                {
                    case "V1":
                        htm = nlr.IsUserTicketByUserIdAndTotalscore(submitData.UserID.ToString(), submitData.TotalScore, submitData.CourseID, submitData.AppId);
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
                return ObjectToJson.GetErrorResult("没有更多数据");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage IsUserTicketByUserIdAndTotalscoreTest()
        {
            try
            {
                GetUserTicketList submitData = new GetUserTicketList();//JsonHelper.DecodeJson<GetUserTicketList>(request.Data);
                submitData.UserID = "756090643";
                submitData.CourseID = "171";
                submitData.TotalScore = "100.0";
                submitData.AppId = "241ea176-fce7-4bd7-a65f-a7978aac1cd2";
                submitData.Version = "V1";
                HttpResponseMessage htm = new HttpResponseMessage();
                switch (submitData.Version.ToUpper())
                {
                    case "V1":
                        htm = nlr.IsUserTicketByUserIdAndTotalscore(submitData.UserID.ToString(), submitData.TotalScore, submitData.CourseID, submitData.AppId);
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
                return ObjectToJson.GetErrorResult("没有更多数据");
            }
        }

        /// <summary>
        /// 根据UserID获取用户所有状态优惠券使用情况
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetUserTicketListByUserId([FromBody]KingRequest request)
        {
            try
            {
                GetUserTicketList submitData = JsonHelper.DecodeJson<GetUserTicketList>(request.Data);
                if (submitData == null)
                {
                    return ObjectToJson.GetErrorResult("当前信息为空");
                }
                if (string.IsNullOrEmpty(submitData.UserID))
                {
                    return ObjectToJson.GetErrorResult("用户ID不能为空");
                }
                if (string.IsNullOrEmpty(submitData.Version))
                {
                    return ObjectToJson.GetErrorResult("版本参数不能为空！");
                }
                HttpResponseMessage htm = new HttpResponseMessage();
                switch (submitData.Version.ToUpper())
                {
                    case "V1":
                        htm = nlr.GetUserTicketListByUserId(submitData.UserID.ToString(), submitData.AppId);
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
                return ObjectToJson.GetErrorResult("没有更多数据");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetUserTicketListByUserIdTest()
        {
            try
            {
                GetUserTicketList submitData = new GetUserTicketList();//JsonHelper.DecodeJson<GetUserTicketList>(request.Data);
                submitData.UserID = "537118551";
                submitData.AppId = "241ea176-fce7-4bd7-a65f-a7978aac1cd2";
                submitData.Version = "v1";
                HttpResponseMessage htm = new HttpResponseMessage();
                switch (submitData.Version.ToUpper())
                {
                    case "V1":
                        htm = nlr.GetUserTicketListByUserId(submitData.UserID.ToString(), submitData.AppId);
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
                return ObjectToJson.GetErrorResult("没有更多数据");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class GetUserTicketList
        {
            public string UserID { get; set; }
            public string TotalScore { get; set; }
            public string CourseID { get; set; }
            public string Version { get; set; }
            public string AppId { get; set; }


        }

        /// <summary>
        /// 
        /// </summary>
        public class InsertUserTicket
        {
            public Guid ID { get; set; }
            public int UserID { get; set; }
            public Guid OrderID { get; set; }
            public int TicketID { get; set; }
            public int Status { get; set; }
            public DateTime CreateTime { get; set; }
            public string Version { get; set; }
        }

    }

}