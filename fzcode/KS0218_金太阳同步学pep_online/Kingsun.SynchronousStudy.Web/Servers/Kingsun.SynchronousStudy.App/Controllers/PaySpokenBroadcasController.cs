using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web.Configuration;
using System.Web.Http;
using System.Xml.Serialization;
using Kingsun.SynchronousStudy.App.Filter;
using Kingsun.SynchronousStudy.App.KSWFWebService;
using log4net;
using Newtonsoft.Json;
using Kingsun.SpokenBroadcas.Model;
using Kingsun.SpokenBroadcas.Common;
using Kingsun.SpokenBroadcas.BLL;

namespace Kingsun.SynchronousStudy.App.Controllers
{
    /// <summary>
    /// 支付相关接口
    /// </summary>
    public class PaySpokenBroadcasController : ApiController
    {
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        BaseManagement bm = new BaseManagement();
        FZPayService.FZPayService service = new FZPayService.FZPayService();
        OrderManagement o = new OrderManagement();

        /// <summary>
        /// 上传订单信息
        /// </summary>
        /// <param name="info">Json串</param>
        /// <returns></returns>
        [HttpPost]
        [ShowApi]
        public ApiResponse AddSuccessOrder([FromBody] KingRequest request)
        {
            try
            {
                log.Info("苹果支付接口AddSuccessOrder开始");
                var obj = JsonConvert.DeserializeAnonymousType(request.Data, new
                {
                    AppleOrderID = "",
                    AppleTicket = "",
                    FeeComboID = "",
                    UserID = "",
                    TotalMoney = 0.01m,
                    UserTicketID = ""
                });
                log.Info("苹果支付接口AddSuccessOrder得到参数");
                var oList = o.Search<TB_Order>("OrderID='" + obj.AppleOrderID + "'");
                if (oList != null && oList.Count > 0)
                {
                    return GetErrorResult("重复订单");
                } 
                bool state = RequestAppleService(obj.AppleTicket, ConfigurationManager.AppSettings["ApplePayUrl"].ToString());
                log.Info("苹果支付接口AddSuccessOrder验证确认"+state);
                //state = true;
                if (state)
                {
                    TB_Order order = new TB_Order();
                    order.ID = Guid.NewGuid();
                    order.UserID = obj.UserID;
                    order.CoursePeriodTimeID = Convert.ToInt32(obj.FeeComboID);
                    order.OrderID = Guid.NewGuid().ToString();
                    order.Count = 1;
                    order.TotalMoney = obj.TotalMoney;
                    order.CompleteDate = DateTime.Now;
                    order.State = "0001";
                    order.PayWay = "苹果支付";
                    order.CreateDate = DateTime.Now;
                    if (bm.Insert<TB_Order>(order))
                    {
                        //string sql = string.Format(@" update Tb_UserAppoint set State=1 where UserID='{0}' and CoursePeriodTimeID='{1}'", obj.UserID, obj.FeeComboID);
                        //int bl = Kingsun.SpokenBroadcas.Common.SqlHelper.ExecuteNonQuery(Kingsun.SpokenBroadcas.Common.SqlHelper.SpokenConnectionString, CommandType.Text, sql);
                        //if (bl == 0)
                        //{
                        //    log.Info(string.Format("修改预约表失败"));
                        //}
                        bool flag = UpdateUserAppointState(order.UserID, order.CoursePeriodTimeID.ToString());
                        if (flag == false)
                        {
                            return GetErrorResult("修改预约表状态失败");
                        }
                        return GetResult(new
                        {

                        });
                    }
                    else
                    {
                        return GetErrorResult("订单插入失败" + bm._operatorError);
                    }
                }
                else
                {
                    return GetErrorResult("订单无效");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return GetErrorResult(ex.Message);
            }
        }
        /// <summary>
        /// 发起支付,获取支付所需参数
        /// </summary>
        /// <param name="PayWayID">支付方式ID</param>
        /// <param name="PayWay">支付方式</param>
        /// <param name="FeeComboID">套餐ID</param>
        /// <param name="UserID">用户ID</param>
        /// <param name="CourseID">用户ID</param>
        /// <param name="TotalMoney">实际付款额</param>
        /// <returns></returns>
        [HttpGet]
        //[ShowApi]
        public ApiResponse GetOrderID(int PayWayID, string PayWay, string CoursePeriodTimeID, string UserID, decimal TotalMoney)
        {
            try
            {
                log.Info("口语直播开始调用GetOrderID");
                //int PayWayID = 0; string PayWay = ""; string CoursePeriodTimeID = ""; string UserID = ""; decimal TotalMoney=0
                #region 校验相应的数据有效性

                if (PayWayID == 0)
                {
                    return GetErrorResult("支付方式不能为空");
                }

                if (string.IsNullOrEmpty(PayWay))
                {
                    return GetErrorResult("支付方式不能为空");
                }
                if (string.IsNullOrEmpty(CoursePeriodTimeID))
                {
                    return GetErrorResult("课时时间ID不能为空");
                }
                //Guid FID;
                //if (!Guid.TryParse(FeeComboID, out FID))
                //{
                //    return GetErrorResult("套餐编不正确");
                //}

                if (string.IsNullOrEmpty(UserID))
                {
                    return GetErrorResult("用户编号不能为空");
                }


                #endregion

                var token = System.Configuration.ConfigurationManager.AppSettings["SpokenBroadcasPayToken"];
                if (string.IsNullOrEmpty(token))
                {
                    return GetErrorResult("未找到此AppID:" + "AppID");
                }
                TB_Order order = new TB_Order();
                order.ID = Guid.NewGuid();
                order.UserID = UserID;
                order.CoursePeriodTimeID = Convert.ToInt32(CoursePeriodTimeID);
                order.OrderID = Guid.NewGuid().ToString();
                order.Count = 1;
                order.TotalMoney = TotalMoney;
                order.CompleteDate = DateTime.Now;
                order.State = "0000";
                order.PayWay = PayWay;
                order.CreateDate = DateTime.Now;
                log.Info("口语直播开始调用GetOrderID(TB_Order)");
                if (bm.Insert<TB_Order>(order))
                {
                    log.Info("口语直播开始调用GetOrderID(bm.Insert<TB_Order>(order))");
                    string resultStr = service.GetOrderID(order.ID.ToString(), token);
                    TempClass tc = JsonHelper.DecodeJson<TempClass>(resultStr);
                    if (tc == null)
                    {
                        bm.Delete<TB_Order>(order.ID);
                        return GetErrorResult("应用编号生成失败");
                    }
                    order.OrderID = tc.OrderID;

                    string responStr = service.SaveOrderInfoFromTemp(order.OrderID, PayWayID);
                    log.Info("口语直播开始调用GetOrderID(responStr)" + responStr);
                    if (string.IsNullOrEmpty(responStr))
                    {
                        bm.Delete<TB_Order>(order.ID);
                        return GetErrorResult("支付系统返回为空");
                    }
                    return GetResult(responStr);
                }
                else
                {
                    return GetErrorResult("订单插入失败" + bm._operatorError);
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return GetErrorResult(ex.Message);
            }
        }

        /// <summary>
        /// 支付确认(APP端支付成功后调用)
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        [HttpGet]
        [ShowApi]
        public ApiResponse PaySuccess(string OrderID, string UserID, string CoursePeriodTimeID)
        {
            #region 校验相应的数据有效性

            if (string.IsNullOrEmpty(OrderID))
            {
                return GetErrorResult("订单编号不能为空");
            }
            if (string.IsNullOrEmpty(UserID))
            {
                return GetErrorResult("用户编号不能为空");
            }
            if (string.IsNullOrEmpty(CoursePeriodTimeID))
            {
                return GetErrorResult("课时时间ID不能为空");
            }
            #endregion
            TB_Order orderinfo = o.GetOrderByOrderID(OrderID);
            if (orderinfo == null)
            {
                return GetErrorResult("找不到订单信息");
            }
            try
            {
                FZPayService.FZPayService service = new FZPayService.FZPayService();
                string state = service.GetOrderState(OrderID, null);
                if (state == "0001")
                {
                    if (orderinfo.State == "0001")
                    {
                        if (!string.IsNullOrEmpty(UserID) && !string.IsNullOrEmpty(CoursePeriodTimeID))
                        { 
                            if (!bm.Update<TB_Order>(orderinfo))
                            {
                                return GetErrorResult("订单更新失败_" + bm._operatorError);
                            }
                        }
                    }
                    //bool flag = UpdateUserAppointState(UserID, CoursePeriodTimeID);
                    //if (flag==false)
                    //{
                    //    return GetErrorResult("修改预约表状态失败");
                    //}
                    return GetResult("支付成功");
                }
                else
                {
                    return GetErrorResult("支付系统订单状态不正确");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return GetErrorResult(ex.Message);
            }
        }
        CourseBLL bll = new CourseBLL();
        /// <summary>
        /// 更新预约表状态
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CoursePeriodTimeID"></param>
        /// <returns></returns>
        public bool UpdateUserAppointState(string UserID, string CoursePeriodTimeID)
        {
            try
            {
                string sql = string.Format(" select CoursePeriodID from Tb_CoursePeriodTime where ID='{0}'", CoursePeriodTimeID);
                string CoursePeriodID = Kingsun.SpokenBroadcas.Common.SqlHelper.ExecuteScalar(Kingsun.SpokenBroadcas.Common.SqlHelper.SpokenConnectionString, CommandType.Text, sql).ToString();
                string CoursePeriodTimeState = bll.GetCoursePeriodTimeState(UserID, CoursePeriodID, CoursePeriodTimeID);
                if (CoursePeriodTimeState == "可预约")
                {
                    bool flag = bll.UpdateUserAppointState(UserID, CoursePeriodID, CoursePeriodTimeID);
                    if (flag)
                    {
                        return true;
                    }
                }
                log.Info(string.Format("修改预约表状态失败(口语直播SpokenBroadcasPayService下面的UpdateUserAppointState)"));
                return false;
            }
            catch (Exception ex)
            {
                log.Info(string.Format("修改预约表状态失败(口语直播SpokenBroadcasPayService下面的UpdateUserAppointState)" + ex.Message));
                return false;
            }
        }

        private static bool RequestAppleService(string strCorona, string url)
        {
            try
            {
                string postDataStr = strCorona;
                //提交服务器
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postDataStr.Length;

                Stream myRequestStream = request.GetRequestStream();
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream);
                myStreamWriter.Write(postDataStr);
                myStreamWriter.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                if (!string.IsNullOrEmpty(retString))
                {
                    Dictionary<string, object> json = (Dictionary<string, object>)JsonHelper.DecodeJson<object>(retString);
                    object value;

                    json.TryGetValue("status", out value);
                    if (value.ToString() == "0")
                    {
                        object receipt;
                        json.TryGetValue("receipt", out receipt);
                        Dictionary<string, object> receiptData = (Dictionary<string, object>)receipt;

                        string bundle_id = receiptData["bundle_id"].ToString();
                        if (bundle_id.Contains("com.kingsunsoft.sunnyclass"))
                        {
                            return true;

                        }
                    }
                    else if (value.ToString() == "21007")
                    {
                        if (!url.Contains("sandbox"))
                        {
                            url = System.Configuration.ConfigurationManager.AppSettings["ApplePaySandboxUrl"].ToString();
                            return RequestAppleService(strCorona, url);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch
            {

                return false;
            }
        }
        private ApiResponse GetErrorResult(string message)
        {
            return new ApiResponse
            {
                Success = false,
                data = null,
                Message = message
            };
        }

        private ApiResponse GetResult(object Data, string message = "")
        {

            return new ApiResponse
            {
                Success = true,
                data = Data,
                Message = message
            };
        }
    }
    public class ApiResponse
    {

        public bool Success { get; set; }
        public object data { get; set; }
        public string Message { get; set; }
    }
}