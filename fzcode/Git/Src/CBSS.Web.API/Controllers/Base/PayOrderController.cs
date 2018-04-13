using CBSS.Core.Log;
using CBSS.Core.Pay.AliPay.Util;
using CBSS.Core.Pay.ApplePay;
using CBSS.Core.Pay.WXPay.lib;
using CBSS.Core.Utility;
using CBSS.Framework.Contract.API;
using CBSS.Framework.Contract.Enums;
using CBSS.Pay.Contract;
using CBSS.Tbx.Contract.DataModel;
using CBSS.UserOrder.Contract.DataModel;
using CourseActivate.Web.API.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBSS.Web.API.Controllers
{
    /// <summary>
    /// 支付、订单
    /// </summary>
    public partial class BaseController
    {
        static string filepath = "Config/PayConfig.xml";
        private static string _applePayUrl = XMLHelper.GetAppSetting(filepath, "ApplePay", "ApplePayUrl");

        /// <summary>
        /// 获取支付方式
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public static APIResponse PostPayList(string inputStr)
        {
            List<PayWay> payWayList = new List<PayWay>();
            PayWay py = new PayWay();
            py.PayWayID = (int)PayWayEnum.Alipay;
            py.PayWayName = EnumHelper.GetEnumDesc<PayWayEnum>(PayWayEnum.Alipay);
            py.Description = EnumHelper.GetEnumDesc<PayWayEnum>(PayWayEnum.Alipay);
            payWayList.Add(py);

            py = new PayWay();
            py.PayWayID = (int)PayWayEnum.WeChat;
            py.PayWayName = EnumHelper.GetEnumDesc<PayWayEnum>(PayWayEnum.WeChat);
            py.Description = EnumHelper.GetEnumDesc<PayWayEnum>(PayWayEnum.WeChat);
            payWayList.Add(py);

            string payList = payWayList.ToJson();
            return APIResponse.GetResponse(payList);
        }

        /// <summary>
        /// 获取套餐列表
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        [HttpPost]
        public static APIResponse PostComboNew(string inputStr)
        {
            PostComboNew input;
            var verifyResult = tbxService.VerifyParam<PostComboNew>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }
            List<CommodityDetails> pcn = ipayOrder.GetCommodity(input.AppID, input.ModuleID);

            return APIResponse.GetResponse(pcn);
        }

        /// <summary>
        /// 发起支付,获取支付所需参数
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        [HttpPost]
        public static APIResponse PostOrderID(string inputStr)
        {
            PostOrderID input;
            var verifyResult = tbxService.VerifyParam<PostOrderID>(inputStr, out input, new List<string> { "Remark", "UserPhone" });
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            Good goodInfo = _repository.GetByID<Good>(input.GoodID);
            if (goodInfo == null)
            {
                return APIResponse.GetErrorResponse("找不到套餐数据");
            }

            Guid gid = Guid.NewGuid();
            UserPayOrder order = new UserPayOrder();
            order.UserPayOrderID = gid;
            order.OrderID = gid.ToString().Replace("-", "");
            order.AppVersionID = input.AppID ?? 0;
            order.UserID = input.UserID ?? 0;
            order.UserPhone = input.UserPhone;
            order.PayWay = input.PayWay ?? 0;
            order.TotalPrice = input.TotalPrice ?? 0;
            order.PayMoney = input.PayMoney ?? 0;
            order.Status = 0;
            order.CreateDate = DateTime.Now;

            UserPayOrderGoodItem userOrderInfo = new UserPayOrderGoodItem();
            userOrderInfo.UserPayOrderGoodItemID = Guid.NewGuid();
            userOrderInfo.UserPayOrderID = gid;
            userOrderInfo.GoodID = goodInfo.GoodID;
            userOrderInfo.GoodName = goodInfo.GoodName;
            userOrderInfo.OriginalPrice = input.OriginalPrice ?? 0;
            userOrderInfo.Price = input.PayMoney ?? 0;
            userOrderInfo.Quantity = input.Quantity ?? 0;
            userOrderInfo.TotalPrice = input.TotalPrice ?? 0;
            userOrderInfo.Remark = input.Remark;

            #region 插入订单以及订单详情

            APIResponse aPiResponse = ipayOrder.InsertUserPayOrderAndOrderInfo(gid.ToJson(), order, userOrderInfo, input.channel ?? 0);
            if (!aPiResponse.Success)
            {
                return aPiResponse;
            }

            #endregion

            #region 支付系统创建订单
            try
            {
                if (input.PayWay == (int)PayWayEnum.Alipay)
                {
                    return ipayOrder.OrderByAliPay(order, goodInfo.GoodName, input.TotalPrice ?? 0, input.Quantity ?? 0);
                }

                if (input.PayWay == (int)PayWayEnum.WeChat)
                {
                    return ipayOrder.OrderByWeChatPay(order, goodInfo.GoodName, input.packageName);
                }
                return APIResponse.GetErrorResponse("支付状态异常！");
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ApiExceptionLog, "订单插入异常，order=" + order.ToJson(), ex);
                return APIResponse.GetErrorResponse("支付订单插入异常");
            }
            #endregion
        }

        /// <summary>
        /// 支付确认(APP端支付成功后调用)
        /// </summary>
        [HttpPost]
        public static APIResponse PostPaySuccess(string inputStr)
        {
            PostPaySucessInfo input;
            var verifyResult = tbxService.VerifyParam<PostPaySucessInfo>(inputStr, out input, new List<string> { "Remark", "UserPhone" });
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            try
            {
                UserPayOrder userPayOrder = ipayOrder.GetUserPayOrderByOrderId(input.OrderID);
                if (userPayOrder != null)
                {
                    userPayOrder.Status = 1;

                    switch (userPayOrder.PayWay)
                    {
                        case (int)PayWayEnum.Alipay:
                            return ipayOrder.PaySuccessByAliPay(input.OrderID, input.channel, input.UserID, input.BookID, input.ModuleID, userPayOrder);
                        case (int)PayWayEnum.WeChat:
                            return ipayOrder.PaySuccessByWeChatPay(input.OrderID, input.packageName, input.channel, input.UserID, input.BookID, input.ModuleID, userPayOrder);
                        default:
                            Log4NetHelper.Error(LoggerType.ApiExceptionLog, "支付订单状态异常UserPayOrder=" + userPayOrder.ToJson(), null);
                            return APIResponse.GetErrorResponse("支付订单状态异常！");
                    }
                }
                else
                {
                    return APIResponse.GetErrorResponse("订单信息异常！");
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ApiExceptionLog, "支付确认PaySuccess出错", ex);
                return APIResponse.GetErrorResponse(ex.Message);
            }
        }

        /// <summary>
        /// 苹果支付接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public static APIResponse AddSuccessOrder(string inputStr)
        {
            try
            {
                AppleOrderID input;
                var verifyResult = tbxService.VerifyParam<AppleOrderID>(inputStr, out input);
                if (!verifyResult.Success)
                {
                    return verifyResult;
                }

                Good goodInfo = _repository.GetByID<Good>(input.GoodID);
                if (goodInfo == null)
                {
                    return APIResponse.GetErrorResponse("找不到套餐数据");
                }

                Guid gid = Guid.NewGuid();
                UserPayOrder order = new UserPayOrder();
                order.UserPayOrderID = gid;
                order.OrderID = gid.ToString().Replace("-", "");
                order.AppVersionID = input.AppID ?? 0;
                order.UserID = input.UserID;
                order.UserPhone = input.UserPhone;
                order.PayWay = 3;
                order.TotalPrice = input.TotalPrice ?? 0;
                order.PayMoney = input.PayMoney ?? 0;
                order.Status = 1;
                order.CreateDate = DateTime.Now;

                UserPayOrderGoodItem userOrderInfo = new UserPayOrderGoodItem();
                userOrderInfo.UserPayOrderGoodItemID = Guid.NewGuid();
                userOrderInfo.UserPayOrderID = gid;
                userOrderInfo.GoodID = goodInfo.GoodID;
                userOrderInfo.GoodName = goodInfo.GoodName;
                userOrderInfo.OriginalPrice = input.OriginalPrice ?? 0;
                userOrderInfo.Price = input.PayMoney ?? 0;
                userOrderInfo.Quantity = input.Quantity ?? 0;
                userOrderInfo.TotalPrice = input.TotalPrice ?? 0;
                userOrderInfo.Remark = input.Remark;

                string state = ApplePay.RequestAppleService(input.AppleTicket, _applePayUrl, 0);
                Stateclass sc = state.FromJson<Stateclass>();
                if (sc.Success)
                {
                    #region 创建订单
                    APIResponse aPiResponse = ipayOrder.InsertUserPayOrderAndOrderInfo(gid.ToJson(), order, userOrderInfo, input.channel ?? 0);
                    if (aPiResponse.Success)
                    {
                        return APIResponse.GetResponse(ipayOrder.GetUserModuleItemInfoByUserId(input.UserID, input.BookID, input.ModuleID)); ; //GetResult(accc.QueryCombo(obj.UserID), "支付成功");
                    } 
                    else
                    {
                        return aPiResponse;
                    }
                    #endregion
                }
                else
                {
                    Log4NetHelper.Error(LoggerType.ApiExceptionLog, "苹果支付订单无效!UserPayOrder=" + order.ToJson() + ",订单详情UserPayOrderGoodItem=" + userOrderInfo.ToJson(), null);
                    return APIResponse.GetErrorResponse("苹果支付订单无效");
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ApiExceptionLog, "苹果支付上传订单信息AddSuccessOrder出错", ex);
                return APIResponse.GetErrorResponse(ex.Message);
            }
        }
    }
}
