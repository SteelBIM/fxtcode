using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Kingsun.SynchronousStudy.App.Common;
using Kingsun.SynchronousStudy.App.Controllers;
using Kingsun.SpokenBroadcas.BLL;
using Kingsun.SpokenBroadcas.Model;
using log4net;
using System.Reflection;
using Kingsun.SpokenBroadcas.Common;
using System.Data;

namespace Kingsun.SynchronousStudy.App
{
    /// <summary>
    /// PointReadPayService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class SpokenBroadcasPayService : System.Web.Services.WebService
    {
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tborderid">Tb_Order表ID</param>
        /// <param name="orderid"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetOrderInfo(string tborderid, string orderid)
        {
            try
            {
                log.Info("口语直播开始调用GetOrderInfo");
                Kingsun.SpokenBroadcas.BLL.OrderManagement o = new Kingsun.SpokenBroadcas.BLL.OrderManagement();
                TB_Order order = o.Select<TB_Order>(tborderid);
                order.OrderID = orderid;
                o.Update<TB_Order>(order);
                log.Info("口语直播开始调用GetOrderInfo（update_TB_Order）");
                var token = System.Configuration.ConfigurationManager.AppSettings["SpokenBroadcasPayToken"];
                if (string.IsNullOrEmpty(token))
                {
                    token = "找不到token";
                }
                string CourseName = o.GetCourseName(tborderid);
                KingResponse response = KingResponse.GetResponse(null, JsonHelper.EncodeJson(new
                {
                    ProductName = CourseName,
                    ProductCount = 1,
                    TotalMoney = order.TotalMoney,
                    CreateDate = order.CreateDate,
                    AppToken = token
                }));
                string responseStr = JsonHelper.EncodeJson(response);
                log.Info("口语直播开始调用GetOrderInfo下responseStr" + responseStr);
                return responseStr;
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return "";
        }

        [WebMethod]
        public string ModifyOrderState(string ID, string orderid, string State, string payway)
        {
            log.Info("口语直播开始调用ModifyOrderState");
            OrderManagement o = new OrderManagement();
            TB_Order order = o.Select<TB_Order>(ID);
            if (order.State == "0001")
            {
                return JsonHelper.EncodeJson(new KingResponse
                {
                    Success = true,
                    Data = "成功",
                });
            } 
            order.OrderID = orderid;
            order.State = State;
            if (o.Update<TB_Order>(order))
            {
                bool flag = UpdateUserAppointState(order.UserID.ToString(), order.CoursePeriodTimeID.ToString());
                if (flag == true)
                {
                    return JsonHelper.EncodeJson(new KingResponse
                    {
                        Success = true,
                        Data = "成功",
                    }); 
                } 
            }
            return JsonHelper.EncodeJson(new KingResponse
            {
                Success = false,
                Data = "失败" + o._operatorError,
            });
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
    }
}
