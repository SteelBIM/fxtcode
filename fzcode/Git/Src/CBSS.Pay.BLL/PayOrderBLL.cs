using CBSS.Framework.DAL;
using CBSS.Framework.Redis;
using CBSS.Pay.Contract;
using CBSS.Pay.IBLL;
using CBSS.Tbx.Contract.DataModel;
using CBSS.UserOrder.Contract.DataModel;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSS.Core.Log;
using CBSS.Core.Pay.AliPay.Util;
using CBSS.Core.Pay.WXPay.lib;
using CBSS.Core.Utility;
using CBSS.Framework.Contract.API;

namespace CBSS.Pay.BLL
{
    public partial class PayOrderBLL : IPayOrder
    {
        Repository _repository = new Repository("Tbx");
        RedisListHelper redisList = new RedisListHelper("Tbx");

        static string filepath = "Config/PayConfig.xml";
        static string _aliaccountid = XMLHelper.GetAppSetting(filepath, "AliPayConfig", "ali_accountid");
        static string _alipartnerid = XMLHelper.GetAppSetting(filepath, "AliPayConfig", "ali_partnerid");
        static string _aliprivatekey = XMLHelper.GetAppSetting(filepath, "AliPayConfig", "ali_private_key");
        static string _alinotifyurl = XMLHelper.GetAppSetting(filepath, "AliPayConfig", "ali_notify_url");

        private static string _szWxAppid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "sz_wx_appid");
        private static string _szWxMchid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "sz_wx_mchid");
        private static string _bjWxAppid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "bj_wx_appid");
        private static string _bjWxMchid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "bj_wx_mchid");
        private static string _shbdAppid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "shbd_appid");
        private static string _shbdMchid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "shbd_mchid");
        private static string _shqgWxAppid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "shqg_wx_appid");
        private static string _shqgWxMchid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "shqg_wx_mchid");
        private static string _gdAppid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "gd_appid");
        private static string _gdMchid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "gd_mchid");
        private static string _gzAppid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "gz_appid");
        private static string _gzMchid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "gz_mchid");
        private static string _rjpepWxAppid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "rjpep_wx_appid");
        private static string _rjpepWxMchid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "rjpep_wx_mchid");

        /// <summary>
        /// 支付宝订单参数
        /// </summary>
        /// <param name="order"></param>
        /// <param name="goodName"></param>
        /// <param name="totalPrice"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public APIResponse OrderByAliPay(UserPayOrder order, string goodName, double totalPrice, int quantity)
        {
            var obj = new
            {
                ID = Guid.NewGuid(),
                SeqNo = Guid.NewGuid(),
                orderid = order.OrderID,
                ProductName = goodName,
                ProductCount = quantity,
                TotalMoney = totalPrice,
                AccountID = _aliaccountid,
                PartnerID = _alipartnerid,
                AppPrivateSecert = _aliprivatekey,
                NotifyUrl = _alinotifyurl
            };
            return APIResponse.GetResponse(obj);
        }

        /// <summary>
        /// 微信订单参数
        /// </summary>
        /// <param name="order"></param>
        /// <param name="packageName"></param>
        /// <param name="goodName"></param>
        /// <returns></returns>
        public APIResponse OrderByWeChatPay(UserPayOrder order, string goodName, string packageName)
        {
            string appid = "";
            string mchid = "";
            switch (packageName)
            {
                case "com.sz.syslearning":
                    appid = _szWxAppid;
                    mchid = _szWxMchid;
                    break;
                case "com.bj.syslearning":
                    appid = _bjWxAppid;
                    mchid = _bjWxMchid;
                    break;
                case "com.shqg.syslearning":
                    appid = _shqgWxAppid;
                    mchid = _shqgWxMchid;
                    break;
                case "com.shbd.syslearning":
                    appid = _shbdAppid;
                    mchid = _shbdMchid;
                    break;
                case "com.gd.syslearning":
                    appid = _gdAppid;
                    mchid = _gdMchid;
                    break;
                case "com.gz.syslearning":
                    appid = _gzAppid;
                    mchid = _gzMchid;
                    break;
                case "com.rj.syslearning":
                    appid = _rjpepWxAppid;
                    mchid = _rjpepWxMchid;
                    break;
                default:
                    appid = _rjpepWxAppid;
                    mchid = _rjpepWxMchid;
                    break;
            }

            WxPayData result = WxPayApi.GetPrePayInfo(order.OrderID, (int)(order.TotalPrice * 100), goodName, packageName);//调用统一下单接口
            if (result != null)
            {
                var obj = new
                {
                    appid = appid,
                    partnerid = mchid,
                    prepayid = result.GetValue("prepay_id").ToString(),
                    package = "Sign=WXPay",
                    noncestr = result.GetValue("nonce_str").ToString(),
                    timestamp = result.GetValue("timestamp").ToString(),
                    sign = result.GetValue("sign").ToString(),
                    orderid = order.OrderID
                };

                return APIResponse.GetResponse(obj);
            }
            else
            {
                return APIResponse.GetErrorResponse("订单失败！");
            }
        }

        /// <summary>
        /// 修改支付宝订单状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="channel"></param>
        /// <param name="moduleId"></param>
        /// <param name="userPayOrder"></param>
        /// <param name="userId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public APIResponse PaySuccessByAliPay(string orderId, int channel, int userId, int bookId, int moduleId, UserPayOrder userPayOrder)
        {
            AliPay ali = new AliPay();
            bool state = ali.Query_Order(orderId);
            if (state)
            {
                if (UpdateUserPayOrder(userPayOrder))
                {
                    //301:优学 1：同步学
                    if (channel == 301)
                    {
                        #region 修改优学订单状态
                        redisList.LPush("CBSSUpdateOrderState", userPayOrder.ToJson());
                        #endregion
                    }
                    return APIResponse.GetResponse(GetUserModuleItemInfoByUserId(userId, bookId, moduleId));
                }
                else
                {
                    Log4NetHelper.Error(LoggerType.ApiExceptionLog, "订单插入异常，userPayOrder=" + userPayOrder.ToJson(), null);
                    return APIResponse.GetErrorResponse("订单状态修改失败");
                }
            }
            else
            {
                Log4NetHelper.Error(LoggerType.ApiExceptionLog, "微信支付状态异常，userPayOrder=" + userPayOrder.ToJson(), null);
                return APIResponse.GetErrorResponse("微信支付状态异常！");
            }
        }

        /// <summary>
        /// 修改微信订单支付状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="packageName"></param>
        /// <param name="channel"></param>
        /// <param name="moduleId"></param>
        /// <param name="userPayOrder"></param>
        /// <param name="userId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public APIResponse PaySuccessByWeChatPay(string orderId, string packageName, int channel, int userId, int bookId, int moduleId, UserPayOrder userPayOrder)
        {
            WxPayData datas = new WxPayData();
            datas.SetValue("out_trade_no", orderId);
            datas.SetValue("attach", packageName);//附加数据

            WxPayData results = WxPayApi.OrderQuery(datas); //调用查询订单接口
            if (results.GetValue("return_code").ToString() == "SUCCESS")
            {
                if (results.GetValue("result_code").ToString() == "SUCCESS")
                {
                    if (results.GetValue("trade_state").ToString() == "SUCCESS")
                    {
                        if (UpdateUserPayOrder(userPayOrder))
                        {
                            //301:优学 1：同步学
                            if (channel == 301)
                            {
                                userPayOrder.Type = 2;
                                #region 修改优学订单状态
                                redisList.LPush("CBSSUserPayOrder", userPayOrder.ToJson());
                                #endregion
                            }
                            return APIResponse.GetResponse(GetUserModuleItemInfoByUserId(userId, bookId, moduleId));
                        }
                        else
                        {
                            Log4NetHelper.Error(LoggerType.ApiExceptionLog, "订单插入异常，userPayOrder=" + userPayOrder.ToJson(), null);
                            return APIResponse.GetErrorResponse("订单状态修改失败");
                        }
                    }
                    else
                    {
                        return APIResponse.GetErrorResponse("支付系统订单状态不正确！");
                    }
                }
                else
                {
                    return APIResponse.GetErrorResponse("支付系统订单状态不正确！");
                }
            }
            else
            {
                return APIResponse.GetErrorResponse("支付系统订单状态不正确！");
            }
        }

        /// <summary>
        /// 插入订单以及订单详情
        /// </summary>
        /// <param name="userPayOrderId"></param>
        /// <param name="order"></param>
        /// <param name="userOrderInfo"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public APIResponse InsertUserPayOrderAndOrderInfo(string userPayOrderId, UserPayOrder order, UserPayOrderGoodItem userOrderInfo, int channel)
        {
            try
            {
                UserPayOrder userPayOrder = _repository.GetByID<UserPayOrder>(userPayOrderId);
                if (userPayOrder == null)
                {
                    if ((int)InsertUserPayOrder(order) > 0)
                    {
                        if ((int)InsertUserPayOrderGoodItem(userOrderInfo) < 0)
                        {
                            Log4NetHelper.Error(LoggerType.ApiExceptionLog, "插入订单详情数据失败，userOrderInfo=" + userOrderInfo.ToJson(), null);
                            return APIResponse.GetErrorResponse("插入订单详情数据失败！");
                        }
                        else
                        {
                            //301:优学 1：同步学
                            if (channel == 301)
                            {
                                order.Type = 1;
                                redisList.LPush("CBSSUserPayOrder", order.ToJson());
                                redisList.LPush("CBSSUserPayOrderInfo", userOrderInfo.ToJson());
                            }
                            return APIResponse.GetResponse("订单插入成功！");
                        }
                    }
                    else
                    {
                        Log4NetHelper.Error(LoggerType.ApiExceptionLog, "插入订单详情数据失败，userOrderInfo=" + order.ToJson(), null);
                        return APIResponse.GetErrorResponse("插入订单数据失败！");
                    }
                }
                else
                {
                    return APIResponse.GetErrorResponse("订单已存在！");
                }
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 2627:  //重复插入
                        Log4NetHelper.Error(LoggerType.ApiExceptionLog, "订单重复插入，order=" + order.ToJson(), ex);
                        return APIResponse.GetErrorResponse("订单重复插入");
                    default:
                        Log4NetHelper.Error(LoggerType.ApiExceptionLog, "订单插入脚本执行失败，order=" + order.ToJson(), ex);
                        return APIResponse.GetErrorResponse("订单插入脚本执行失败");
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ApiExceptionLog, "订单插入异常，order=" + order.ToJson(), ex);
                return APIResponse.GetErrorResponse("订单插入异常");
            }
        }

        /// <summary>
        /// 获取商品套餐详情
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public List<CommodityDetails> GetCommodity(string appId, int moduleId)
        {
            List<CommodityDetails> list2 = null;
            using (var db = _repository.GetInstance())
            {
                list2 = db.Queryable<AppGoodItem, Good, GoodModuleItem, GoodPrice>((ag, gd, gm, gp) => new object[] {
                   JoinType.Left,ag.GoodID==gd.GoodID,
                   JoinType.Left,ag.GoodID==gm.GoodID,
                   JoinType.Left,gp.GoodID==gm.GoodID
                }).Where((ag, gd, gm, gp) => gd.Status == 1 && ag.AppID == appId && gm.ModuleID == moduleId)
                .Select((ag, gd, gm, gp) => new CommodityDetails
                {
                    AppID = ag.AppID,
                    GoodID = gd.GoodID,
                    GoodName = gd.GoodName,
                    Describe = gd.Describe,
                    ModuleID = gm.ModuleID,
                    GoodsBpolicyMonths = gp.GoodsBpolicyMonths,
                    GoodsOriginalPrice = gp.GoodsOriginalPrice,
                    AndroidPrice = gp.AndroidPrice,
                    IOSCommodityID = gp.IOSCommodityID,
                    IOSPrice = gp.IOSPrice
                }).ToList();
            }
            return list2;
        }

        /// <summary>
        /// 根据订单ID获取订单信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public UserPayOrder GetUserPayOrderByOrderId(string orderId)
        {
            return _repository.SelectSearch<UserPayOrder>(i => i.OrderID == orderId).FirstOrDefault();
        }

        /// <summary>
        /// 插入订单信息
        /// </summary>
        /// <param name="userPayOrder"></param>
        /// <returns></returns>
        public object InsertUserPayOrder(UserPayOrder userPayOrder)
        {
            return _repository.Insert<UserPayOrder>(userPayOrder);
        }

        /// <summary>
        /// 插入订单详情信息
        /// </summary>
        /// <param name="userPayOrder"></param>
        /// <returns></returns>
        public object InsertUserPayOrderGoodItem(UserPayOrderGoodItem userOrderInfo)
        {
            return _repository.Insert<UserPayOrderGoodItem>(userOrderInfo);
        }

        /// <summary>
        /// 修改订单状态
        /// </summary>
        /// <param name="userPayOrder"></param>
        /// <returns></returns>
        public bool UpdateUserPayOrder(UserPayOrder userPayOrder)
        {
            return _repository.UpdateColumns<UserPayOrder>(userPayOrder, i => i.Status);
        }

        /// <summary>
        /// 根据用户ID，书籍ID,模块ID,获取权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bookId"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public List<UserModuleItem> GetUserModuleItemInfoByUserId(int userId, int bookId, int moduleId)
        {
            List<UserModuleItem> userModuleItems = null;
            using (var db = _repository.GetInstance())
            {
                userModuleItems = db.Queryable<UserModuleItem>().Select(it => new UserModuleItem { UserID = it.UserID, MarketBookID = it.MarketBookID, ModuleID = it.ModuleID, Months = it.Months, StartDate = it.StartDate, EndDate = it.EndDate }).ToList();
            }
            //IEnumerable<UserModuleItem> userModuleItems = _repository.SelectSearch<UserModuleItem>(i => i.UserID == userId && i.MarketBookID == bookId && i.ModuleID == moduleId && i.States == 1);
            return userModuleItems.ToList();
        }

    }
}
