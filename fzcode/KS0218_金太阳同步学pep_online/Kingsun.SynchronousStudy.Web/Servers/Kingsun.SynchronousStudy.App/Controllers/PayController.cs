using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web.Configuration;
using System.Web.Http;
using System.Xml.Serialization;
using Kingsun.SynchronousStudy.App.Common;
using Kingsun.SynchronousStudy.App.Filter;
using Kingsun.SynchronousStudy.App.KSWFWebService;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;
using log4net;
using Newtonsoft.Json;
using System.Data.SqlClient;
using Kingsun.IBS.BLL;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.Model;

namespace Kingsun.SynchronousStudy.App.Controllers
{
    /// <summary>
    /// 支付相关接口
    /// </summary>
    public class PayController : ApiController
    {
        static RedisListHelper listRedis = new RedisListHelper();
        IIBSData_AreaSchRelationBLL areaBLL = new IBSData_AreaSchRelationBLL();
        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();
        IIBSData_SchClassRelationBLL schBLL = new IBSData_SchClassRelationBLL();

        FZPayService.FZPayService service = new FZPayService.FZPayService();
        string _kswfUserName = WebConfigurationManager.AppSettings["kswfUserName"];
        string _kswfPassWord = WebConfigurationManager.AppSettings["kswfPassWord"];
        string _logOnOrOff = WebConfigurationManager.AppSettings["LogOnOrOff"];
        BaseManagement bm = new BaseManagement();
        OrderManagement o = new OrderManagement();

        ///// <summary>
        ///// 获取支付方式
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[ShowApi]
        //public ApiResponse GetPayList()
        //{
        //    string payList = service.GetPayWayByToken(ProjectConstant.PayToken);
        //    return GetResult(payList);
        //}

        /// <summary>
        /// 获取支付方式
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ShowApi]
        public ApiResponse GetPayList(string AppID = "")
        {
            var token = ProjectConstant.PayToken;

            if (AppID != "")
            {
                token = System.Configuration.ConfigurationManager.AppSettings[AppID];
            }

            if (string.IsNullOrEmpty(token))
            {
                return GetErrorResult("未找到此AppID:" + AppID);
            }
            string payList = service.GetPayWayByToken(token);
            return GetResult(payList);
        }


        /// <summary>
        /// 获取套餐列表
        /// </summary>
        /// <param name="AppID">课程ID(测试用ID 1 )</param>
        /// <param name="SyetemID">系统ID(安卓1 苹果2 )</param>
        /// <returns></returns>
        [HttpGet]
        [ShowApi]
        public ApiResponse GetCombo(string AppID, string SyetemID)
        {
            if (string.IsNullOrEmpty(AppID))
            {
                return GetErrorResult("课程ID不能为空");
            }

            string where = "State='1' and AppID='" + AppID + "' and (Type='" + SyetemID + "' or Type='3')";
            string sql = string.Format(@"SELECT  [ID]
                                                ,[FeeName]
                                                ,[FeePrice]
                                                ,[State]
                                                ,[CreateDate]
                                                ,[CreateUser]
                                                ,[ModifyDate]
                                                ,[ModifyUser]
                                                ,[AppID]
                                                ,[Month]
                                                ,[Discount]
                                                ,[Type]
                                                ,[AppleID]
                                                ,[ComboType]
                                                ,[ImageUrl]
                                            FROM [dbo].[TB_FeeCombo] WHERE {0} ", where);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            List<TBFeeCombo> list = JsonHelper.DataSetToIList<TBFeeCombo>(ds, 0);
            //IList<TB_FeeCombo> list = bm.Search<TB_FeeCombo>("State='1' and AppID='" + AppID + "' and (Type='" + SyetemID + "' or Type='3')");

            if (list == null || list.Count == 0)
            {
                return GetErrorResult("没有找到套餐信息");
            }

            for (int i = 0; i < list.Count; i++)
            {
                var orlist = bm.Search<TB_Order>("FeeComboID='" + list[i].ID + "' and State='0001'");
                if (orlist != null)
                {
                    list[i].PayCount = orlist.Count;
                }
            }
            return GetResult(list);

        }


        /// <summary>
        /// 获取套餐列表
        /// </summary>
        /// <param name="AppID">课程ID(测试用ID 1 )</param>
        /// <param name="SyetemID">系统ID(安卓1 苹果2 )</param>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        [ShowApi]
        public ApiResponse GetComboNew(string AppID, string SyetemID, string UserID)
        {
            if (string.IsNullOrEmpty(AppID))
            {
                return GetErrorResult("课程ID不能为空");
            }
            string where = "State='1' and AppID='" + AppID + "' and (Type='" + SyetemID + "' or Type='3')";
            string sql = string.Format(@"SELECT  [ID]
                                                ,[FeeName]
                                                ,[FeePrice]
                                                ,[State]
                                                ,[CreateDate]
                                                ,[CreateUser]
                                                ,[ModifyDate]
                                                ,[ModifyUser]
                                                ,[AppID]
                                                ,[Month]
                                                ,[Discount]
                                                ,[Type]
                                                ,[AppleID]
                                                ,[ComboType]
                                                ,[ImageUrl]
                                            FROM [dbo].[TB_FeeCombo] WHERE {0} ", where);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            List<TBFeeCombo> list = JsonHelper.DataSetToIList<TBFeeCombo>(ds, 0);
            //var list = bm.Search<TB_FeeCombo>("State='1' and AppID='" + AppID + "' and (Type='" + SyetemID + "' or Type='3')");
            if (list == null || list.Count == 0)
            {
                return GetErrorResult("没有找到套餐信息");
            }

            for (int i = 0; i < list.Count; i++)
            {
                list[i].Time = "1";
            }
            int Status = 0;
            var couList = bm.Search<TB_Coupon>("UserID='" + UserID + "'");
            if (couList != null && couList.Count > 0)
            {
                Status = couList[0].Status.Value;
            }
            Temp1 t = new Temp1();

            t.List = list;
            t.Status = Status;

            return GetResult(t);

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
        [ShowApi]
        public ApiResponse GetOrderID(int PayWayID, string PayWay, string CourseID, string FeeComboID, string UserID, decimal TotalMoney)
        {
            #region 校验相应的数据有效性

            if (PayWayID == 0)
            {
                return GetErrorResult("支付方式不能为空");
            }

            if (string.IsNullOrEmpty(PayWay))
            {
                return GetErrorResult("支付方式不能为空");
            }
            Guid FID;
            if (!Guid.TryParse(FeeComboID, out FID))
            {
                return GetErrorResult("套餐编不正确");
            }

            if (string.IsNullOrEmpty(UserID))
            {
                return GetErrorResult("用户编号不能为空");
            }

            int? IsDiscount = 0;
            TB_FeeCombo feecombo = bm.Select<TB_FeeCombo>(FID);
            if (feecombo == null)
            {
                return GetErrorResult("找不到套餐数据");
            }
            else
            {
                if (TotalMoney < feecombo.FeePrice)
                {
                    IsDiscount = 1;
                }
            }

            #endregion

            if (_logOnOrOff == "1")
            {
                Log4Net.LogHelper.Error("PayWayID:" + PayWayID + ";PayWay:" + PayWay + ";FeeComboID：" + FeeComboID + ";CourseID：" + CourseID);
            }
            var token = System.Configuration.ConfigurationManager.AppSettings[feecombo.AppID];
            if (string.IsNullOrEmpty(token))
            {
                return GetErrorResult("未找到此AppID:" + feecombo.AppID);
            }

            #region 订单生成
            TB_Order order = new TB_Order();
            order.ID = Guid.NewGuid();
            order.CreateDate = DateTime.Now;
            order.TotalMoney = TotalMoney;
            order.OrderID = Guid.NewGuid().ToString().Replace("-", "");
            order.UserID = UserID;
            order.State = "0000";
            order.PayWay = PayWay;
            order.CourseID = CourseID;
            order.FeeComboID = FID;
            order.IsDiscount = IsDiscount ?? 0;

            try
            {
                bool success = bm.Insert<TB_Order>(order);
                if (!success)
                {
                    Log4Net.LogHelper.Error("订单插入失败，order=" + JsonHelper.EncodeJson(order));
                    return GetErrorResult("订单插入失败");
                }
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 2627:  //重复插入
                        Log4Net.LogHelper.Error(ex, "订单重复插入，order=" + JsonHelper.EncodeJson(order));
                        return GetErrorResult("订单重复插入");
                    default:
                        Log4Net.LogHelper.Error(ex, "订单插入脚本执行失败，order=" + JsonHelper.EncodeJson(order));
                        return GetErrorResult("订单插入脚本执行失败");
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "订单插入异常，order=" + JsonHelper.EncodeJson(order));
                return GetErrorResult("订单插入异常");
            }
            #endregion

            #region 支付系统创建订单
            if (_logOnOrOff == "1")
            {
                Log4Net.LogHelper.Info("GetOrderID支付接口插入订单的UserID:" + ";token:" + token + ";order:" + JsonHelper.EncodeJson(order));
            }
            try
            {
                string resultStr = service.GetOrderID(order.ID.ToString(), token);
                TempClass tc = JsonHelper.DecodeJson<TempClass>(resultStr);
                if (tc == null)
                {
                    Log4Net.LogHelper.Error("GetOrderID应用编号生成失败,订单号为:" + order.ID + "；该数据库连接为：" + bm.bmConnection + "；resultStr：" + resultStr);
                    return GetErrorResult("应用编号生成失败");
                }
                order.OrderID = tc.OrderID;
                string responStr = service.SaveOrderInfoFromTemp(order.OrderID, PayWayID);
                if (string.IsNullOrEmpty(responStr))
                {
                    Log4Net.LogHelper.Error("GetOrderID接口支付系统返回为空，responStr:" + responStr);
                    return GetErrorResult("支付系统返回为空");
                }
                else
                {
                    return GetResult(responStr);
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "订单插入异常，order=" + JsonHelper.EncodeJson(order));
                return GetErrorResult("订单插入异常");
            }
            #endregion
        }

        [HttpPost]
        [ShowApi]
        public ApiResponse GetOrderID([FromBody]KingRequest request)
        {
            #region 校验相应的数据有效性
            GetOrderList submitData = JsonHelper.DecodeJson<GetOrderList>(request.Data);
            if (submitData.PayWayID == 0)
            {
                return GetErrorResult("支付方式不能为空");
            }

            if (string.IsNullOrEmpty(submitData.PayWay))
            {
                return GetErrorResult("支付方式不能为空");
            }
            Guid FID;
            if (!Guid.TryParse(submitData.FeeComboID, out FID))
            {
                return GetErrorResult("套餐编不正确");
            }

            if (string.IsNullOrEmpty(submitData.UserID))
            {
                return GetErrorResult("用户编号不能为空");
            }

            int? IsDiscount = 0;
            TB_FeeCombo feecombo = bm.Select<TB_FeeCombo>(FID);
            if (feecombo == null)
            {
                return GetErrorResult("找不到套餐数据");
            }
            else
            {
                if (submitData.TotalMoney < feecombo.FeePrice)
                {
                    IsDiscount = 1;
                }
            }
            if (submitData.channel <= 0)
            {
                submitData.channel = 303;
            }

            #endregion

            if (_logOnOrOff == "1")
            {
                Log4Net.LogHelper.Error("PayWayID:" + submitData.PayWayID + ";PayWay:" + submitData.PayWay + ";FeeComboID：" + submitData.FeeComboID + ";CourseID：" + submitData.CourseID);
            }
            var token = System.Configuration.ConfigurationManager.AppSettings[feecombo.AppID];
            if (string.IsNullOrEmpty(token))
            {
                return GetErrorResult("未找到此AppID:" + feecombo.AppID);
            }

            #region 订单生成
            TB_Order order = new TB_Order();
            order.ID = Guid.NewGuid();
            order.CreateDate = DateTime.Now;
            order.TotalMoney = submitData.TotalMoney;
            order.OrderID = Guid.NewGuid().ToString().Replace("-", "");
            order.UserID = submitData.UserID;
            order.State = "0000";
            order.PayWay = submitData.PayWay;
            order.CourseID = submitData.CourseID;
            order.FeeComboID = FID;
            order.IsDiscount = IsDiscount ?? 0;
            order.SourceType = submitData.channel == 301 ? 1 : 0;
            try
            {
                if (submitData.channel == 301)
                {
                    string sql = string.Format(@"INSERT  INTO dbo.TB_Order ( ID ,OrderID ,TotalMoney ,CreateDate ,State ,UserID ,PayWay ,FeeComboID ,CourseID ,IsDiscount ,channel,sourcetype)
                                                                   VALUES  ( '{0}' ,'{1}' ,'{2}' ,GETDATE() ,'0000' ,'{3}' ,'{4}' ,'{5}' ,'{6}' ,'{7}' ,'{8}','{9}')",
                                                                            Guid.NewGuid(), Guid.NewGuid().ToString(), submitData.TotalMoney, submitData.UserID, submitData.PayWay, FID, submitData.CourseID, IsDiscount ?? 0, submitData.channel, order.SourceType);
                    int i = SqlHelper.ExecuteNonQuery(SqlHelper.RJTBXConnectionString, CommandType.Text, sql);
                    if (i <= 0)
                    {
                        Log4Net.LogHelper.Error("优学订单插入失败，order=" + JsonHelper.EncodeJson(order));
                    }
                }

                bool success = bm.Insert<TB_Order>(order);
                if (!success)
                {
                    Log4Net.LogHelper.Error("同步学订单插入失败，order=" + JsonHelper.EncodeJson(order));
                    return GetErrorResult("订单插入失败");
                }

            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 2627:  //重复插入
                        Log4Net.LogHelper.Error(ex, "订单重复插入，order=" + JsonHelper.EncodeJson(order));
                        return GetErrorResult("订单重复插入");
                    default:
                        Log4Net.LogHelper.Error(ex, "订单插入脚本执行失败，order=" + JsonHelper.EncodeJson(order));
                        return GetErrorResult("订单插入脚本执行失败");
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "订单插入异常，order=" + JsonHelper.EncodeJson(order));
                return GetErrorResult("订单插入异常");
            }
            #endregion

            #region 支付系统创建订单
            if (_logOnOrOff == "1")
            {
                Log4Net.LogHelper.Info("GetOrderID支付接口插入订单的UserID:" + ";token:" + token + ";order:" + JsonHelper.EncodeJson(order));
            }
            try
            {
                string resultStr = service.GetOrderID(order.ID.ToString(), token);
                TempClass tc = JsonHelper.DecodeJson<TempClass>(resultStr);
                if (tc == null)
                {
                    Log4Net.LogHelper.Error("GetOrderID应用编号生成失败,订单号为:" + order.ID + "；该数据库连接为：" + bm.bmConnection + "；resultStr：" + resultStr);
                    return GetErrorResult("应用编号生成失败");
                }
                order.OrderID = tc.OrderID;
                string responStr = service.SaveOrderInfoFromTemp(order.OrderID, submitData.PayWayID);
                if (string.IsNullOrEmpty(responStr))
                {
                    Log4Net.LogHelper.Error("GetOrderID接口支付系统返回为空，responStr:" + responStr);
                    return GetErrorResult("支付系统返回为空");
                }
                else
                {
                    return GetResult(responStr);
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "订单插入异常，order=" + JsonHelper.EncodeJson(order));
                return GetErrorResult("订单插入异常");
            }
            #endregion
        }



        /// <summary>
        /// 支付确认(APP端支付成功后调用)
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        [HttpGet]
        [ShowApi]
        public ApiResponse PaySuccess(string OrderID)
        {

            PaySucessInfo submitData = new PaySucessInfo();
            submitData.OrderID = OrderID;

            if (submitData == null)
            {
                return GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.OrderID))
            {
                return GetErrorResult("订单ID不能为空");
            }

            try
            {
                FZPayService.FZPayService service = new FZPayService.FZPayService();
                string state = service.GetOrderState(submitData.OrderID, null);
                if (state == "0001") //用户支付成功但订单状态未更新
                {
                    return KVpi(submitData.OrderID);
                }
                else
                {
                    return GetErrorResult("支付系统订单状态不正确");
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error("支付确认PaySuccess出错，ex=" + ex.Message);
                return GetErrorResult(ex.Message);
            }

        }

        /// <summary>
        /// 支付确认(APP端支付成功后调用)
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        [HttpPost]
        [ShowApi]
        public ApiResponse PaySuccess([FromBody]KingRequest request)
        {
            PaySucessInfo submitData = JsonHelper.DecodeJson<PaySucessInfo>(request.Data);
            if (submitData == null)
            {
                return GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.OrderID))
            {
                return GetErrorResult("订单ID不能为空");
            }

            try
            {
                FZPayService.FZPayService service = new FZPayService.FZPayService();
                string state = service.GetOrderState(submitData.OrderID, null);
                if (state == "0001") //用户支付成功但订单状态未更新
                {
                    return KVpi(submitData.OrderID);
                }
                else
                {
                    return GetErrorResult("支付系统订单状态不正确");
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error("支付确认PaySuccess出错，ex=" + ex.Message);
                return GetErrorResult(ex.Message);
            }

        }
        /// <summary>
        /// 支付确认(APP端支付成功后调用)
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        [HttpGet]
        public ApiResponse PaySuccessTest()
        {
            PaySucessInfo submitData=new PaySucessInfo();
            submitData.OrderID = "148350548220180123180842391";
            var orderinfo = o.GetOrderByOrderID(submitData.OrderID);
            if (submitData == null)
            {
                return GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.OrderID))
            {
                return GetErrorResult("订单ID不能为空");
            }

            try
            {
                #region 更新订单
                try
                {
                    orderinfo.State = "0001";
                    bool success = bm.Update<TB_Order>(orderinfo);
                    if (!success)
                    {
                        Log4Net.LogHelper.Error("更新订单状态失败，OrderID=" + orderinfo.OrderID);
                        return GetErrorResult("更新订单状态失败");
                    }
                    #region 分库订单写入队列
                    listRedis.LPush("Order2BaseDBQueue", orderinfo.ToJson());
                    #endregion
                    return GetResult("");
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "更新订单状态出错，OrderID=" + orderinfo.OrderID);
                    return GetErrorResult("更新订单状态出错");
                }
                #endregion
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error("支付确认PaySuccess出错，ex=" + ex.Message);
                return GetErrorResult(ex.Message);
            }

        }


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
                var obj = JsonConvert.DeserializeAnonymousType(request.Data, new
                {
                    AppleOrderID = "",
                    AppleTicket = "",
                    CourseID = "",
                    FeeComboID = Guid.Empty,
                    UserID = "",
                    TotalMoney = 0.01m,
                    IsDiscount = 0,
                    UserTicketID = Guid.Empty
                });

                string state = RequestAppleService(obj.AppleTicket, ConfigurationManager.AppSettings["ApplePayUrl"].ToString(), 0);
                Stateclass sc = JsonHelper.DecodeJson<Stateclass>(state);
                if (sc.Success)
                {
                    TB_Order order = new TB_Order();
                    order.ID = Guid.NewGuid();
                    order.OrderID = obj.AppleOrderID;
                    order.FeeComboID = obj.FeeComboID;
                    order.CourseID = obj.CourseID;
                    order.PayWay = "苹果支付";
                    int SanBoxTest = 0;
                    if (sc.isSanBox == "1")
                    {
                        SanBoxTest = 1;//表示沙盒订单
                    }
                    order.CreateDate = DateTime.Now;
                    order.TotalMoney = obj.TotalMoney;
                    order.UserID = obj.UserID;
                    order.IsDiscount = obj.IsDiscount;

                    #region 创建订单
                    try
                    {
                        o.Insert<TB_Order>(order);
                        return KVpi(order.OrderID, SanBoxTest);
                    }
                    catch (SqlException ex)
                    {
                        switch (ex.Number)
                        {
                            case 2627:  //重复插入
                                Log4Net.LogHelper.Error(ex, "苹果支付插入订单重复,order=" + JsonHelper.EncodeJson(order));
                                return KVpi(order.OrderID, SanBoxTest);
                            default:
                                Log4Net.LogHelper.Error(ex, "苹果支付插入订单脚本执行失败,order=" + JsonHelper.EncodeJson(order));
                                return GetErrorResult("苹果支付插入订单脚本执行失败");
                        }
                    }
                    catch (Exception ex)
                    {
                        Log4Net.LogHelper.Error(ex, "苹果支付插入订单失败,order=" + JsonHelper.EncodeJson(order));
                        return GetErrorResult("苹果支付插入订单失败");
                    }
                    #endregion
                }
                else
                {
                    return GetErrorResult("苹果支付订单无效");
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error("苹果支付上传订单信息AddSuccessOrder出错，ex=" + ex.Message);
                return GetErrorResult(ex.Message);
            }
        }

        /// <summary>
        /// 上传订单信息
        /// </summary>
        /// <param name="info">Json串</param>
        /// <returns></returns>
        [HttpGet]
        [ShowApi]
        public ApiResponse AddSuccessOrderTest()
        {
            try
            {
                var obj = new
                {
                    AppleOrderID = "1000000332468003",
                    AppleTicket = "123",
                    CourseID = "169",
                    FeeComboID = new Guid("6d3bfd00-f45c-4b11-ba00-0d03209228f1"),
                    UserID = "1007400082",
                    TotalMoney = 60,
                    IsDiscount = 1,
                    UserTicketID = new Guid("00000000-0000-0000-0000-000000000000")
                };

                string state = RequestAppleService(obj.AppleTicket, ConfigurationManager.AppSettings["ApplePayUrl"].ToString(), 0);
                Stateclass sc = JsonHelper.DecodeJson<Stateclass>(state);
                if (sc.Success)
                {
                    TB_Order order = new TB_Order();
                    order.ID = Guid.NewGuid();
                    order.OrderID = obj.AppleOrderID;
                    order.FeeComboID = obj.FeeComboID;
                    order.CourseID = obj.CourseID;
                    order.PayWay = "苹果支付";
                    int SanBoxTest = 0;
                    if (sc.isSanBox == "1")
                    {
                        SanBoxTest = 1;//表示沙盒订单
                    }
                    order.CreateDate = DateTime.Now;
                    order.TotalMoney = obj.TotalMoney;
                    order.UserID = obj.UserID;
                    order.IsDiscount = obj.IsDiscount;

                    try
                    {
                        o.Insert<TB_Order>(order);
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number != 2627) //未重复插入数据库
                        {
                            Log4Net.LogHelper.Error(ex, "ios插入订单失败，OrderID=" + order.OrderID);
                            return GetErrorResult("ios插入订单失败");
                        }
                    }
                    return KVpi(order.OrderID, SanBoxTest);
                }
                else
                {
                    return GetErrorResult("订单无效");
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error("上传订单信息AddSuccessOrder出错，ex=" + ex.Message);
                return GetErrorResult(ex.Message);
            }
        }

        /// <summary>
        /// 获取购买记录
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="AppID">应用ID</param>
        /// <returns></returns>
        [HttpGet]
        [ShowApi]
        public ApiResponse GetOrderList(string UserID, string AppID)
        {

            var app = bm.Select<TB_APPManagement>(AppID);
            if (app == null)
            {
                return GetErrorResult("没有此应用");
            }
            var edList = bm.Search<TB_CurriculumManage>(" EditionID='" + app.VersionID + "'");
            var cids = "";
            for (int i = 0; i < edList.Count; i++)
            {
                if (string.IsNullOrEmpty(cids))
                    cids += "'" + edList[i].BookID + "'";
                else
                    cids += ",'" + edList[i].BookID + "'";
            }

            var vip = bm.Search<TB_UserMember>("UserID='" + UserID + "' and CourseID in (" + cids + ")", " StartDate");
            if (vip == null)
            {
                return GetErrorResult("没有购买记录");
            }
            ArrayList list = new ArrayList();
            foreach (var item in vip)
            {
                var olist = bm.Select<TB_Order>(item.TbOrderID.Value);
                if (olist == null)
                {
                    continue;
                }
                var courseList = bm.Search<TB_CurriculumManage>("BookID='" + item.CourseID + "'");
                if (courseList == null)
                {
                    return GetErrorResult("所购买的课程不存在");
                }
                var course = courseList[0];
                list.Add(new
                {
                    TotalMoney = olist.TotalMoney,
                    CourseName = course.TeachingNaterialName.Replace("小学", "").Replace("初中", "").Replace("高中", "").Replace("学期", "").Replace("英语", ""),
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    Months = item.Months
                });
            }

            return GetResult(list);
        }

        /// <summary>
        /// 获取指定套餐支付人数
        /// </summary>
        /// <param name="FeeComboID">套餐ID</param>
        /// <returns></returns>
        [HttpGet]
        [ShowApi]
        public ApiResponse PayCount(string FeeComboID)
        {
            if (string.IsNullOrEmpty(FeeComboID))
            {
                GetErrorResult("套餐ID不能为空");
            }

            var list = bm.Search<TB_Order>("FeeComboID='" + FeeComboID + "' and State='0001'");
            if (list == null)
            {
                return GetResult(new
                {
                    PayCount = 0
                });
            }
            return GetResult(new
            {
                PayCount = list.Count
            });
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

        public ApiResponse KVpi(string OrderID, int SanBoxTest = 0)
        {
            TB_Order orderinfo = o.GetOrderByOrderID(OrderID);
            if (orderinfo == null)
            {
                Log4Net.LogHelper.Error("找不到订单信息，OrderID=" + OrderID);
                return GetErrorResult("找不到订单信息");
            }

            TB_FeeCombo fc = bm.Select<TB_FeeCombo>(orderinfo.FeeComboID.Value);
            if (fc == null)
            {
                Log4Net.LogHelper.Error("找不到套餐信息，OrderID=" + OrderID);
                return GetErrorResult("找不到套餐信息");
            }

            #region 更新订单
            try
            {
                orderinfo.State = "0001";
                bool success = bm.Update<TB_Order>(orderinfo);
                if (!success)
                {
                    Log4Net.LogHelper.Error("更新订单状态失败，OrderID=" + orderinfo.OrderID);
                    return GetErrorResult("更新订单状态失败");
                }
                #region 分库订单写入队列
                listRedis.LPush("Order2BaseDBQueue", orderinfo.ToJson());
                #endregion
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "更新订单状态出错，OrderID=" + orderinfo.OrderID);
                return GetErrorResult("更新订单状态出错");
            }
            #endregion

            #region 同步到财富分账
            if (SanBoxTest != 1)
            {
                bool success = ConcurrentOrder(orderinfo);
                if (!success)
                {
                    return GetErrorResult("同步到财富分账失败");
                }
            }
            #endregion


            #region 用户已经开通权限，返回用户权限
            AccountController accc = new AccountController();
            try
            {
                var usermember = bm.Search<TB_UserMember>("TbOrderID='" + orderinfo.ID.Value + "'");
                if (usermember != null && usermember.Count > 0)
                {
                    return GetResult(accc.QueryCombo(orderinfo.UserID), "支付成功");
                }
                else
                {
                    Log4Net.LogHelper.Error("用户开通权限失败，TbOrderID=" + orderinfo.ID.Value);
                    return GetErrorResult("用户开通权限失败");
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "用户已经开通权限,获取失败，TbOrderID=" + orderinfo.ID.Value);
                return GetErrorResult("用户已经开通权限,获取失败");
            }
            #endregion
        }

        private static string RequestAppleService(string strCorona, string url, int isSanBox)
        {
            ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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

                        if (bundle_id.Contains("com.kingsunsoft"))
                        {
                            var obj = new { Success = true, isSanBox = isSanBox };
                            return JsonHelper.EncodeJson(obj);
                        }
                    }
                    else if (value.ToString() == "21007")
                    {
                        if (!url.Contains("sandbox"))
                        {
                            isSanBox = 1;
                            url = System.Configuration.ConfigurationManager.AppSettings["ApplePaySandboxUrl"].ToString();
                            return RequestAppleService(strCorona, url, 1);
                        }
                        else
                        {
                            var obj = new { Success = false, isSanBox = isSanBox };
                            return JsonHelper.EncodeJson(obj);
                        }
                    }
                    else
                    {
                        var obj = new { Success = false, isSanBox = isSanBox };
                        return JsonHelper.EncodeJson(obj);
                    }
                }
                var obj1 = new { Success = false, isSanBox = isSanBox };
                return JsonHelper.EncodeJson(obj1);
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error("错误：" + ex.Message);
                var obj = new { Success = false, isSanBox = isSanBox };
                return JsonHelper.EncodeJson(obj);
            }
        }

        /// <summary>
        /// 同步订单
        /// </summary>
        [HttpGet]
        public HttpResponseMessage testFN(string orderId, string YearMonth)
        {
            int i = 0;
            try
            {
                if (string.IsNullOrEmpty(orderId) && string.IsNullOrEmpty(YearMonth))//orderId和earMonth都为空（同步所有）
                {
                    IList<TB_Order> order = bm.Search<TB_Order>("State=0001");
                    if (order != null)
                    {
                        foreach (var item in order)
                        {
                            Fn(new Guid(item.ID.ToString()));
                            i++;
                        }
                        return JsonHelper.GetResult("本次同步数据条数为：" + i);
                    }
                    return JsonHelper.GetResult("本次同步数据条数为" + i);
                }
                else if (string.IsNullOrEmpty(orderId) && !string.IsNullOrEmpty(YearMonth))//orderId为空，YearMonth不为空(同步月份)
                {
                    IList<TB_Order> order = bm.Search<TB_Order>(string.Format("convert(varchar(7),CreateDate,120)=convert(varchar(7),'{0}',120) AND State=0001", YearMonth));
                    if (order != null)
                    {
                        foreach (var item in order)
                        {
                            Fn(new Guid(item.ID.ToString()));
                            i++;
                        }
                        return JsonHelper.GetResult("本次同步数据条数为：" + i);
                    }
                    return JsonHelper.GetResult("本次同步数据条数为" + i);
                }
                else//orderId不为空（同步订单）
                {
                    Fn(new Guid(orderId));
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "同步订单出错");
            }
            return JsonHelper.GetResult("本次同步数据条数为：" + i);

        }

        /// <summary>
        /// 同步商品
        /// </summary>
        [HttpGet]
        public void testProduct()
        {
            IList<TB_FeeCombo> fc = bm.SelectAll<TB_FeeCombo>();
            foreach (var fcitem in fc)
            {
                IList<TB_APPManagement> am = bm.Search<TB_APPManagement>("ID='" + fcitem.AppID + "'");
                KSWFWebService.KSWFWebService kswf = new KSWFWebService.KSWFWebService();

                KSWFWebService.Product pd = new KSWFWebService.Product();
                if (am != null)
                {
                    foreach (var item in am)
                    {
                        pd.VersionID = item.VersionID;
                        pd.Version = item.VersionName;
                    }
                }
                pd.Channel = 1;
                pd.ProductNo = "TBX_" + fcitem.ID;
                pd.ProductName = fcitem.FeeName;
                pd.Subject = "英语";
                pd.SubjectID = 3;
                pd.Category = "E-BOOK";
                pd.CategoryKey = 101;
                pd.Price = fcitem.FeePrice ?? 0;
                pd.Isshevel = fcitem.State ?? 0;

                CertficateSoapHeader header = new CertficateSoapHeader();
                header.UserName = _kswfUserName;
                header.PassWord = _kswfPassWord;
                kswf.CertficateSoapHeaderValue = header;

                KSWFWebService.ReturnInfo ri = kswf.AddProduct(pd);
            }
        }


        [HttpGet]
        [ShowApi]
        public void Fn(Guid TbOrderID)
        {
            try
            {
                TB_Order orderinfo = bm.Select<TB_Order>(TbOrderID);
                if (orderinfo == null)
                {
                    return;
                }

                bool co = ConcurrentOrder(orderinfo);
                if (co)
                {
                    Log4Net.LogHelper.Error("同步订单状态：true");
                }
                else
                {
                    Log4Net.LogHelper.Error("同步订单状态：false");
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "error");
            }
        }

        /// <summary>
        /// 同步订单
        /// </summary>
        /// <param name="tbo"></param>
        public bool ConcurrentOrder(TB_Order tbo)
        {
            try
            {
                var user = userBLL.GetUserAllInfoByUserId(Convert.ToInt32(tbo.UserID));

                if (user != null)
                {
                    Order on = new Order();
                    if (user.ClassSchDetailList != null && user.ClassSchDetailList.Count > 0)
                    {
                        user.ClassSchDetailList.ForEach(a =>
                        {
                            var classInfo = classBLL.GetClassUserRelationByClassId(a.ClassID);
                            var tea = classInfo.ClassTchList.Where(x => x.SubjectID == 3).FirstOrDefault();
                            if (tea != null)
                            {
                                on.SchoolID = classInfo.SchID;
                            }
                            on.SchoolName = "";
                            on.GradeID = classInfo.GradeID;
                            on.GradeName = "";
                            on.ClassID = new Guid(classInfo.ClassID);
                            on.ClassName = classInfo.ClassName;
                            on.TeacherUserID = Convert.ToInt32(tea.TchID);
                            on.TeacherUserName = tea.TchName;
                        });
                    }
                    int type = 4;
                    if (tbo.PayWay.Contains("微信"))
                    {
                        type = 0;
                    }
                    else if (tbo.PayWay.Contains("支付宝"))
                    {
                        type = 1;
                    }
                    else if (tbo.PayWay.Contains("苹果"))
                    {
                        type = 2;
                    }
                    on.OrderID = tbo.OrderID;
                    on.OrderDate = tbo.CreateDate;
                    on.ProductNO = "TBX_" + tbo.FeeComboID;
                    on.PayType = type;
                    on.Channel = 1;
                    on.UserClientIP = "";
                    on.PayAmount = tbo.TotalMoney ?? 0;
                    on.BuyUserID = user.iBS_UserInfo.UserID.ToString();
                    on.BuyUserPhone = user.iBS_UserInfo.TelePhone;
                    KSWFWebService.KSWFWebService kswf = new KSWFWebService.KSWFWebService();
                    CertficateSoapHeader header = new CertficateSoapHeader();
                    header.UserName = _kswfUserName;
                    header.PassWord = _kswfPassWord;
                    kswf.CertficateSoapHeaderValue = header;

                    KSWFWebService.ReturnInfo ri = kswf.AddOrderInfo(on);
                    if (!ri.Success)
                    {
                        Log4Net.LogHelper.Error("同步到财富分账失败,ErrorMsg=" + ri.ErrorMsg + ",BuyUserID=" + on.BuyUserID + ",OrderID=" + on.OrderID);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "同步到财富分账出错失败");
            }
            return true;
        }

        private static Thread BgThread { set; get; }
    }

    public class PaySucessInfo
    {
        public string OrderID { get; set; }
        public string TicketID { get; set; }
        public string AppID { get; set; }
        public int PayWayID { get; set; }
    }

    public class TempClass
    {
        public string OrderID { get; set; }
        public IList<PayWay> PayWayList { get; set; }
    }

    class TempClass3
    {
        public int State { get; set; }
        public string Msg { get; set; }
    }
    public class PayWay
    {
        public int PayWayID { get; set; }
        public string PayWayName { get; set; }
        public string Description { get; set; }
    }

    public class teaInfo
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string SchoolName { get; set; }
        public string TrueName { get; set; }
    }

    public class riUser
    {
        public int SchoolID { get; set; }
    }

    public class Tb_OrderNet
    {
        /// <summary>
        /// 订单编号（支付系统的）
        /// </summary>
        public string OrderID { get; set; }

        /// <summary>
        /// 订单时间
        /// </summary>
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 区域编号(选填)
        /// </summary>
        public int? AreaID { get; set; }

        /// <summary>
        /// 区域路径
        /// </summary>
        public string AreaPath { get; set; }

        /// <summary>
        /// 学校编号(选填)
        /// </summary>
        public int? SchoolID { get; set; }

        /// <summary>
        /// 学校名称
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 年级编号
        /// </summary>
        public int? GradeID { get; set; }

        /// <summary>
        /// 年级名称
        /// </summary>
        public string GradeName { get; set; }

        /// <summary>
        /// 班级编号（GUID）
        /// </summary>
        public Guid? ClassID { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 产品编号（使用“,”分开多个产品编号，不允许为空）
        /// </summary>
        public string ProductNO { get; set; }

        /// <summary>
        /// 老师编号
        /// </summary>
        public int? TeacherUserID { get; set; }

        /// <summary>
        /// 老师名称
        /// </summary>
        public string TeacherUserName { get; set; }

        /// <summary>
        /// 购买用户编号
        /// </summary>
        public int? BuyUserID { get; set; }

        /// <summary>
        /// 购买用户手机号
        /// </summary>
        public string BuyUserPhone { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal? PayAmount { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayType { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public int? Channel { get; set; }

        /// <summary>
        /// 用户购买IP地址（如果区域编号存在或者购买用户手机号存在，可以不填）
        /// </summary>
        public string UserClientIP { get; set; }
    }

    public class XmlUtil
    {
        #region 反序列化
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static object Deserialize(Type type, string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(type);
            return xmldes.Deserialize(stream);
        }
        #endregion

        #region 序列化
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serializer(Type type, object obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                //序列化对象
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }

        #endregion
    }

    public class Temp1
    {
        public List<TBFeeCombo> List { get; set; }

        public int Status { get; set; }
    }

    public class TBFeeCombo
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? FeePrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ModifyUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AppID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Month { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AppleID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? ComboType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FeeName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        public int PayCount { get; set; }
        public string Time { get; set; }
    }

    public class Stateclass
    {
        public bool Success { get; set; }
        public string isSanBox { get; set; }
    }

    public class GetOrderList
    {
        public int PayWayID { get; set; }
        public string PayWay { get; set; }
        public string CourseID { get; set; }
        public string FeeComboID { get; set; }
        public string UserID { get; set; }
        public decimal TotalMoney { get; set; }
        public int channel { get; set; }
    }
}