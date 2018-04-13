using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using Kingsun.SynchronousStudy.App.Common;
using Kingsun.SynchronousStudy.App.Controllers;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using log4net;

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
    public class PointReadPayService : System.Web.Services.WebService
    {
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        string _logOnOrOff = WebConfigurationManager.AppSettings["LogOnOrOff"];

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }


        [WebMethod]
        public string GetOrderInfo(string tborderid, string orderid)
        {
            if (_logOnOrOff == "1")
            {
                Log4Net.LogHelper.Info("支付回调接口GetOrderInfo开始回调，tborderid=" + tborderid + ";orderid=" + orderid);
            }

            string responseStr = "";
            try
            {                
                OrderManagement o = new OrderManagement();
                TB_Order order = o.Select<TB_Order>(tborderid);
                if (order == null)
                {
                    Log4Net.LogHelper.Error("支付回调接口GetOrderInfo出错，tborderid=" + tborderid + ";orderid=" + orderid);
                    return responseStr;
                }

                order.OrderID = orderid;
                o.Update<TB_Order>(order);
                TB_FeeCombo fc = o.Select<TB_FeeCombo>(order.FeeComboID);
                var token = System.Configuration.ConfigurationManager.AppSettings[fc.AppID];
                KingResponse response = KingResponse.GetResponse(null, JsonHelper.EncodeJson(new
                {
                    ProductName = fc.FeeName,
                    ProductCount = 1,
                    TotalMoney = order.TotalMoney,
                    CreateDate = order.CreateDate,
                    AppToken = token
                }));
                if (_logOnOrOff == "1")
                {
                    Log4Net.LogHelper.Info("支付回调接口GetOrderInfo回调结果，ProductName=" + fc.FeeName + ";AppToken=" + token);
                }
                return JsonHelper.EncodeJson(response); ;
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex,"支付回调接口GetOrderInfo出错,orderid=" + orderid);
                return responseStr;
            }
        }

        [WebMethod]
        public string ModifyOrderState(string ID, string orderid, string State, string payway)
        {
            if (_logOnOrOff == "1")
            {
                Log4Net.LogHelper.Info("支付回调接口ModifyOrderState开始回调，ID=" + ID + ";orderid=" + orderid + ";State=" + State + ";payway=" + payway);
            }
            if(State=="0001")
            {
                PayController pc = new PayController();
                var res = pc.KVpi(orderid);
                if (res.Success)
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
                Data = "失败",
            });
        }
    }
}
