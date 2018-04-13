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
using System.Web;
using Kingsun.IBS.BLL;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.Model;
using Kingsun.SynchronousStudy.AliPay.Util;
using Kingsun.SynchronousStudy.App.FZPayService;
using Kingsun.SynchronousStudy.Pay.business;
using Kingsun.SynchronousStudy.Pay.lib;
using System.Threading.Tasks;

namespace Kingsun.SynchronousStudy.App.Controllers
{
    /// <summary>
    /// 支付相关接口
    /// </summary>
    public class PayActionController : ApiController
    {
        IIBSData_AreaSchRelationBLL areaBLL = new IBSData_AreaSchRelationBLL();
        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();
        IIBSData_SchClassRelationBLL schBLL = new IBSData_SchClassRelationBLL();
        private RedisListHelper listredis = new RedisListHelper();

        FZPayService.FZPayService service = new FZPayService.FZPayService();
        string _kswfUserName = WebConfigurationManager.AppSettings["kswfUserName"];
        string _kswfPassWord = WebConfigurationManager.AppSettings["kswfPassWord"];
        string _logOnOrOff = WebConfigurationManager.AppSettings["LogOnOrOff"];
        string _aliaccountid = WebConfigurationManager.AppSettings["ali_accountid"];
        string _alipartnerid = WebConfigurationManager.AppSettings["ali_partnerid"];
        string _aliprivatekey = WebConfigurationManager.AppSettings["ali_private_key"];
        string _alinotifyurl = WebConfigurationManager.AppSettings["ali_notify_url"];
        string _alikeycode = WebConfigurationManager.AppSettings["ali_keycode"];
        string _alipayway = WebConfigurationManager.AppSettings["ali_payway"];

        private string bj = ConfigurationManager.ConnectionStrings["BJDBConnectionStr"].ConnectionString;
        private string sz = ConfigurationManager.ConnectionStrings["SZDBConnectionStr"].ConnectionString;
        private string ot = ConfigurationManager.ConnectionStrings["OTDBConnectionStr"].ConnectionString;
        private string rjpep = ConfigurationManager.ConnectionStrings["RJPEPDBConnectionStr"].ConnectionString;
        private string rjyx = ConfigurationManager.ConnectionStrings["RJYXDBConnectionStr"].ConnectionString;

        private string _szWxAppid = WebConfigurationManager.AppSettings["sz_wx_appid"];
        private string _szWxMchid = WebConfigurationManager.AppSettings["sz_wx_mchid"];
        private string _bjWxAppid = WebConfigurationManager.AppSettings["bj_wx_appid"];
        private string _bjWxMchid = WebConfigurationManager.AppSettings["bj_wx_mchid"];
        private string _shbdAppid = WebConfigurationManager.AppSettings["shbd_appid"];
        private string _shbdMchid = WebConfigurationManager.AppSettings["shbd_mchid"];
        private string _shqgWxAppid = WebConfigurationManager.AppSettings["shqg_wx_appid"];
        private string _shqgWxMchid = WebConfigurationManager.AppSettings["shqg_wx_mchid"];
        private string _gdAppid = WebConfigurationManager.AppSettings["gd_appid"];
        private string _gdMchid = WebConfigurationManager.AppSettings["gd_mchid"];
        private string _gzAppid = WebConfigurationManager.AppSettings["gz_appid"];
        private string _gzMchid = WebConfigurationManager.AppSettings["gz_mchid"];
        private string _rjpepWxAppid = WebConfigurationManager.AppSettings["rjpep_wx_appid"];
        private string _rjpepWxMchid = WebConfigurationManager.AppSettings["rjpep_wx_mchid"];


        private string _rjpepWxMchid1 = System.Configuration.ConfigurationManager.AppSettings["123456"];

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
        [HttpPost]
        public ApiResponse PostPayList([FromBody] KingRequest request)//string AppID = "")
        {
            PostPayList submitData = JsonHelper.DecodeJson<PostPayList>(request.Data);

            List<PayWay> payWayList = new List<PayWay>();
            PayWay py = new PayWay();
            py.PayWayID = 1;
            py.PayWayName = "支付宝支付";
            py.Description = "支付宝支付";
            payWayList.Add(py);

            py = new PayWay();
            py.PayWayID = 18;
            py.PayWayName = "微信支付";
            py.Description = "微信支付";
            payWayList.Add(py);

            string payList = JsonHelper.EncodeJson(payWayList);

            return GetResult(payList);
        }


        /// <summary>
        /// 获取套餐列表
        /// </summary>
        /// <param name="AppID">课程ID(测试用ID 1 )</param>
        /// <param name="SyetemID">系统ID(安卓1 苹果2 )</param>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse PostCombo([FromBody] KingRequest request)//string AppID, string SyetemID)
        {
            PostCombo submitData = JsonHelper.DecodeJson<PostCombo>(request.Data);
            if (string.IsNullOrEmpty(submitData.AppID))
            {
                return GetErrorResult("课程ID不能为空");
            }

            string where = "State='1' and AppID='" + submitData.AppID + "' and (Type='" + submitData.SyetemID + "' or Type='3')";
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
            DataSet ds = SelectOrderSql(submitData.AppID, sql);//SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
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
        [HttpPost]
        public ApiResponse PostComboNew([FromBody]KingRequest request)//string AppID, string SyetemID, string UserID)
        {
            PostComboNew submitData = JsonHelper.DecodeJson<PostComboNew>(request.Data);
            if (string.IsNullOrEmpty(submitData.AppID))
            {
                return GetErrorResult("课程ID不能为空");
            }
            string where = "State='1' and AppID='" + submitData.AppID + "' and (Type='" + submitData.SyetemID + "' or Type='3')";
            string sql;
            if (string.IsNullOrEmpty(submitData.ModuleID) || submitData.ModuleID == "1")
            {
                sql = string.Format(@"SELECT  [ID]
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
                                                ,[ModuleID]
                                            FROM [dbo].[TB_FeeCombo] WHERE {0} and (ModuleID is null or ModuleID='')", where);
            }
            else
            {
                sql = string.Format(@"SELECT  [ID]
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
                                                ,[ModuleID]
                                            FROM [dbo].[TB_FeeCombo] WHERE {0} and  ModuleID ='" + submitData.ModuleID + "'", where);
            }

            DataSet ds = SelectOrderSql(submitData.AppID, sql);//SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
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
            var couList = bm.Search<TB_Coupon>("UserID='" + submitData.UserID + "'");
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
        /// 获取套餐列表
        /// </summary>
        /// <param name="AppID">课程ID(测试用ID 1 )</param>
        /// <param name="SyetemID">系统ID(安卓1 苹果2 )</param>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResponse GetComboNew()//string AppID, string SyetemID, string UserID)
        {
            PostComboNew submitData = new PostComboNew();//JsonHelper.DecodeJson<PostComboNew>(request.Data);
            submitData.AppID = "333d7cfc-cb4f-49d2-8ded-025e7d0fe766";
            submitData.SyetemID = "1";
            if (string.IsNullOrEmpty(submitData.AppID))
            {
                return GetErrorResult("课程ID不能为空");
            }
            string where = "State='1' and AppID='" + submitData.AppID + "' and (Type='" + submitData.SyetemID + "' or Type='3')";
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
            var couList = bm.Search<TB_Coupon>("UserID='" + submitData.UserID + "'");
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
        [HttpPost]
        public ApiResponse PostOrderID([FromBody]KingRequest request)//int PayWayID, string PayWay, string CourseID, string FeeComboID, string UserID, decimal TotalMoney)
        {
            PostOrderID submitData = JsonHelper.DecodeJson<PostOrderID>(request.Data);
            #region 校验相应的数据有效性

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
            string FCSql = string.Format(@"SELECT * FROM dbo.TB_FeeCombo WHERE id='{0}'", FID);
            DataSet FCDS = SelectOrderSql(submitData.AppID, FCSql);
            List<TB_FeeCombo> feecombo = JsonHelper.DataSetToIList<TB_FeeCombo>(FCDS, 0);//bm.Select<TB_FeeCombo>(FID);
            if (feecombo.Count <= 0)
            {
                return GetErrorResult("找不到套餐数据");
            }
            else
            {
                if (submitData.TotalMoney < feecombo[0].FeePrice)
                {
                    IsDiscount = 1;
                }
            }

            if (string.IsNullOrEmpty(submitData.packageName))
            {
                submitData.packageName = "com.rj.syslearning";
            }

            #endregion

            if (_logOnOrOff == "1")
            {
                Log4Net.LogHelper.Error("PayWayID:" + submitData.PayWayID + ";PayWay:" + submitData.PayWay + ";FeeComboID：" + submitData.FeeComboID + ";CourseID：" + submitData.CourseID);
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
            order.ModuleID = submitData.ModuleID;

            string ss = "";
            try
            {
                string ssql = string.Format(@"SELECT * FROM dbo.TB_Order WHERE OrderID='{0}'", order.OrderID);
                DataSet ds = SelectOrderSql(submitData.AppID, ssql);
                ss = ds.Tables[0].Rows.Count.ToString();
                if ((ds == null) || (ds.Tables.Count == 0) || (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0))
                {
                    //301:优学 1：同步学
                    if (submitData.channel == 301)
                    {
                        string sqlstr = string.Format(@"INSERT INTO dbo.TB_Order ( ID ,OrderID ,TotalMoney ,CreateDate ,State ,UserID ,PayWay ,FeeComboID ,CourseID ,IsDiscount ,SourceType ,ModuleID)
                                                                                  VALUES  ( '{0}' , '{1}' , '{2}' , '{3}' , '{4}' , '{5}' , '{6}' , '{7}' , '{8}' , '{9}' , '{10}', '{11}'   )",
                                                                               order.ID, order.OrderID, order.TotalMoney, order.CreateDate, order.State, order.UserID, order.PayWay, order.FeeComboID, order.CourseID, order.IsDiscount, order.SourceType, order.ModuleID);
                        int s = SqlHelper.ExecuteNonQuery(rjyx, CommandType.Text, sqlstr);
                        if (s <= 0)
                        {
                            Log4Net.LogHelper.Error("订单插入失败，order=" + JsonHelper.EncodeJson(order) + "APPID:" + submitData.AppID);
                            return GetErrorResult("订单插入失败1");
                        }
                    }

                    string sql = string.Format(@"INSERT INTO dbo.TB_Order ( ID ,OrderID ,TotalMoney ,CreateDate ,State ,UserID ,PayWay ,FeeComboID ,CourseID ,IsDiscount ,SourceType ,ModuleID)
                                                              VALUES  ( '{0}' , '{1}' , '{2}' , '{3}' , '{4}' , '{5}' , '{6}' , '{7}' , '{8}' , '{9}' , '{10}' , '{11}'  )",
                                                                           order.ID, order.OrderID, order.TotalMoney, order.CreateDate, order.State, order.UserID, order.PayWay, order.FeeComboID, order.CourseID, order.IsDiscount, order.SourceType, order.ModuleID);
                    var i = OrderSql(submitData.AppID, sql);
                    if (i <= 0)
                    {
                        Log4Net.LogHelper.Error("订单插入失败2，order=" + JsonHelper.EncodeJson(order) + "；APPID:" + submitData.AppID + ";ss" + ss);
                        return GetErrorResult("订单插入失败");
                    }
                }
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 2627:  //重复插入
                        Log4Net.LogHelper.Error(ex, "订单重复插入，order=" + JsonHelper.EncodeJson(order) + ";重复：" + ss);
                        return GetErrorResult("订单重复插入");
                    default:
                        Log4Net.LogHelper.Error(ex, "订单插入脚本执行失败，order=" + JsonHelper.EncodeJson(order) + "；APPID:" + submitData.AppID);
                        return GetErrorResult("订单插入脚本执行失败");
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "订单插入异常，order=" + JsonHelper.EncodeJson(order) + ";ds=" + ss);
                return GetErrorResult("订单插入异常");
            }
            #endregion

            #region 支付系统创建订单
            if (_logOnOrOff == "1")
            {
                Log4Net.LogHelper.Info("GetOrderID支付接口插入订单的order:" + JsonHelper.EncodeJson(order));
            }
            try
            {
                if (submitData.PayWay.Contains("支付宝支付"))
                {
                    var obj = new
                    {
                        ID = Guid.NewGuid(),
                        SeqNo = Guid.NewGuid(),
                        orderid = order.OrderID,
                        ProductName = feecombo[0].FeeName,
                        ProductCount = 1,
                        TotalMoney = submitData.TotalMoney,
                        AccountID = _aliaccountid,
                        PartnerID = _alipartnerid,
                        AppPrivateSecert = _aliprivatekey,
                        NotifyUrl = _alinotifyurl
                    };
                    return GetResult(obj);
                }

                if (submitData.PayWay.Contains("微信支付"))
                {
                    string APPID = "";
                    string MCHID = "";
                    switch (submitData.packageName)
                    {
                        case "com.sz.syslearning":
                            APPID = _szWxAppid;
                            MCHID = _szWxMchid;
                            break;
                        case "com.bj.syslearning":
                            APPID = _bjWxAppid;
                            MCHID = _bjWxMchid;
                            break;
                        case "com.shqg.syslearning":
                            APPID = _shqgWxAppid;
                            MCHID = _shqgWxMchid;
                            break;
                        case "com.shbd.syslearning":
                            APPID = _shbdAppid;
                            MCHID = _shbdMchid;
                            break;
                        case "com.gd.syslearning":
                            APPID = _gdAppid;
                            MCHID = _gdMchid;
                            break;
                        case "com.gz.syslearning":
                            APPID = _gzAppid;
                            MCHID = _gzMchid;
                            break;
                        case "com.rj.syslearning":
                            APPID = _rjpepWxAppid;
                            MCHID = _rjpepWxMchid;
                            break;
                        default:
                            APPID = _rjpepWxAppid;
                            MCHID = _rjpepWxMchid;
                            break;
                    }

                    //WxPayData data = new WxPayData();
                    //data.SetValue("body", feecombo.FeeName);//商品描述
                    ////data.SetValue("attach", "test");//附加数据
                    //data.SetValue("out_trade_no", order.OrderID);//随机字符串
                    //data.SetValue("total_fee", submitData.TotalMoney);//总金额
                    //data.SetValue("trade_type", "APP");//交易类型
                    ////data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
                    ////data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
                    ////data.SetValue("goods_tag", "jjj");//商品标记
                    ////data.SetValue("trade_type", "NATIVE");//交易类型
                    ////data.SetValue("product_id", productId);//商品ID
                    WxPayData result = WxPayApi.GetPrePayInfo(order.OrderID, (int)(order.TotalMoney * 100), feecombo[0].FeeName.ToString(), submitData.packageName);//调用统一下单接口
                    if (result != null)
                    {
                        var obj = new
                        {
                            appid = APPID,
                            partnerid = MCHID,
                            prepayid = result.GetValue("prepay_id").ToString(),
                            package = "Sign=WXPay",
                            noncestr = result.GetValue("nonce_str").ToString(),
                            timestamp = result.GetValue("timestamp").ToString(),
                            sign = result.GetValue("sign").ToString(),
                            orderid = order.OrderID
                        };

                        return GetResult(obj);
                    }
                    else
                    {
                        return GetErrorResult("订单失败！" + result);
                    }
                }
                return GetErrorResult("支付状态异常！");
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "订单插入异常，order=" + JsonHelper.EncodeJson(order));
                return GetErrorResult("支付订单插入异常");
            }
            #endregion
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
        public ApiResponse GetOrderID()//int PayWayID, string PayWay, string CourseID, string FeeComboID, string UserID, decimal TotalMoney)
        {

            PostOrderID submitData = new PostOrderID();//JsonHelper.DecodeJson<PostOrderID>(request.Data);
            //{"PayWayID":"18","PayWay":"微信支付","UserID":"1242345889","TotalMoney":"0.01","CourseID":"104","FeeComboID":"882b432d-4010-46bc-a12d-42b1dd17a3a9","channel":"301","AppID":"5373bbc9-49d4-47df-b5b5-ae196dc23d6d"}
            submitData.PayWayID = 18;
            submitData.PayWay = "微信支付";
            submitData.UserID = "1121059182";
            submitData.TotalMoney = Convert.ToDecimal("0.02");
            submitData.CourseID = "263";
            submitData.FeeComboID = "882b48ed-4010-46bc-a12d-42b4dda5a3a9";
            submitData.channel = 1;
            submitData.AppID = "241ea176-fce7-4bd7-a65f-a7978aac1cd2";

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


            if (string.IsNullOrEmpty(submitData.packageName))
            {
                submitData.packageName = "com.rj.syslearning";
            }

            if (_logOnOrOff == "1")
            {
                Log4Net.LogHelper.Error("PayWayID:" + submitData.PayWayID + ";PayWay:" + submitData.PayWay + ";FeeComboID：" + submitData.FeeComboID + ";CourseID：" + submitData.CourseID);
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

            string ss = "";
            try
            {
                string ssql = string.Format(@"SELECT * FROM dbo.TB_Order WHERE OrderID='{0}'", order.OrderID);
                DataSet ds = SelectOrderSql(submitData.AppID, ssql);
                ss = ds.Tables[0].Rows.Count.ToString();
                if ((ds == null) || (ds.Tables.Count == 0) || (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0))
                {
                    //301:优学 1：同步学
                    if (submitData.channel == 301)
                    {
                        string sqlstr = string.Format(@"INSERT INTO dbo.TB_Order ( ID ,OrderID ,TotalMoney ,CreateDate ,State ,UserID ,PayWay ,FeeComboID ,CourseID ,IsDiscount ,SourceType)
                                                                                  VALUES  ( '{0}' , '{1}' , '{2}' , '{3}' , '{4}' , '{5}' , '{6}' , '{7}' , '{8}' , '{9}' , '{10}'  )",
                                                                               order.ID, order.OrderID, order.TotalMoney, order.CreateDate, order.State, order.UserID, order.PayWay, order.FeeComboID, order.CourseID, order.IsDiscount, order.SourceType);
                        int s = SqlHelper.ExecuteNonQuery(rjyx, CommandType.Text, sqlstr);
                        if (s <= 0)
                        {
                            Log4Net.LogHelper.Error("订单插入失败，order=" + JsonHelper.EncodeJson(order) + "APPID:" + submitData.AppID);
                            return GetErrorResult("订单插入失败1");
                        }
                    }

                    string sql = string.Format(@"INSERT INTO dbo.TB_Order ( ID ,OrderID ,TotalMoney ,CreateDate ,State ,UserID ,PayWay ,FeeComboID ,CourseID ,IsDiscount ,SourceType)
                                                              VALUES  ( '{0}' , '{1}' , '{2}' , '{3}' , '{4}' , '{5}' , '{6}' , '{7}' , '{8}' , '{9}' , '{10}'  )",
                                                                           order.ID, order.OrderID, order.TotalMoney, order.CreateDate, order.State, order.UserID, order.PayWay, order.FeeComboID, order.CourseID, order.IsDiscount, order.SourceType);
                    var i = OrderSql(submitData.AppID, sql);
                    if (i <= 0)
                    {
                        Log4Net.LogHelper.Error("订单插入失败2，order=" + JsonHelper.EncodeJson(order) + "；APPID:" + submitData.AppID + ";ss" + ss);
                        return GetErrorResult("订单插入失败");
                    }
                }
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 2627:  //重复插入
                        Log4Net.LogHelper.Error(ex, "订单重复插入，order=" + JsonHelper.EncodeJson(order) + ";重复：" + ss);
                        return GetErrorResult("订单重复插入");
                    default:
                        Log4Net.LogHelper.Error(ex, "订单插入脚本执行失败，order=" + JsonHelper.EncodeJson(order) + "；APPID:" + submitData.AppID);
                        return GetErrorResult("订单插入脚本执行失败");
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "订单插入异常，order=" + JsonHelper.EncodeJson(order) + ";ds=" + ss);
                return GetErrorResult("订单插入异常");
            }
            #endregion

            #region 支付系统创建订单
            if (_logOnOrOff == "1")
            {
                Log4Net.LogHelper.Info("GetOrderID支付接口插入订单的order:" + JsonHelper.EncodeJson(order));
            }
            try
            {
                if (submitData.PayWay.Contains("支付宝支付"))
                {
                    var obj = new
                    {
                        ID = Guid.NewGuid(),
                        SeqNo = Guid.NewGuid(),
                        orderid = order.OrderID,
                        ProductName = feecombo.FeeName,
                        ProductCount = 1,
                        TotalMoney = submitData.TotalMoney,
                        AccountID = _aliaccountid,
                        PartnerID = _alipartnerid,
                        AppPrivateSecert = _aliprivatekey,
                        NotifyUrl = _alinotifyurl
                    };
                    return GetResult(obj);
                }

                if (submitData.PayWay.Contains("微信支付"))
                {
                    string APPID = "";
                    string MCHID = "";
                    switch (submitData.packageName)
                    {
                        case "com.sz.syslearning":
                            APPID = _szWxAppid;
                            MCHID = _szWxMchid;
                            break;
                        case "com.bj.syslearning":
                            APPID = _bjWxAppid;
                            MCHID = _bjWxMchid;
                            break;
                        case "com.shqg.syslearning":
                            APPID = _shqgWxAppid;
                            MCHID = _shqgWxMchid;
                            break;
                        case "com.shbd.syslearning":
                            APPID = _shbdAppid;
                            MCHID = _shbdMchid;
                            break;
                        case "com.gd.syslearning":
                            APPID = _gdAppid;
                            MCHID = _gdMchid;
                            break;
                        case "com.gz.syslearning":
                            APPID = _gzAppid;
                            MCHID = _gzMchid;
                            break;
                        case "com.rj.syslearning":
                            APPID = _rjpepWxAppid;
                            MCHID = _rjpepWxMchid;
                            break;
                        default:
                            APPID = _rjpepWxAppid;
                            MCHID = _rjpepWxMchid;
                            break;
                    }

                    //WxPayData data = new WxPayData();
                    //data.SetValue("body", feecombo.FeeName);//商品描述
                    ////data.SetValue("attach", "test");//附加数据
                    //data.SetValue("out_trade_no", order.OrderID);//随机字符串
                    //data.SetValue("total_fee", submitData.TotalMoney);//总金额
                    //data.SetValue("trade_type", "APP");//交易类型
                    ////data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
                    ////data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
                    ////data.SetValue("goods_tag", "jjj");//商品标记
                    ////data.SetValue("trade_type", "NATIVE");//交易类型
                    ////data.SetValue("product_id", productId);//商品ID
                    WxPayData result = WxPayApi.GetPrePayInfo(order.OrderID, (int)(order.TotalMoney * 100), feecombo.FeeName.ToString(), submitData.packageName);//调用统一下单接口
                    if (result != null)
                    {
                        var obj = new
                        {
                            appid = APPID,
                            partnerid = MCHID,
                            prepayid = result.GetValue("prepay_id").ToString(),
                            package = "Sign=WXPay",
                            noncestr = result.GetValue("nonce_str").ToString(),
                            timestamp = result.GetValue("timestamp").ToString(),
                            sign = result.GetValue("sign").ToString(),
                            orderid = order.OrderID
                        };

                        return GetResult(obj);
                    }
                    else
                    {
                        return GetErrorResult("订单失败！" + result);
                    }
                }
                return GetErrorResult("支付状态异常！");
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "订单插入异常，order=" + JsonHelper.EncodeJson(order));
                return GetErrorResult("支付订单插入异常");
            }
            #endregion
        }



        private int OrderSql(string AppID, string sql)
        {
            int i = 0;
            switch (AppID)
            {
                case "0a94ceaf-8747-4266-bc05-ed8ae2e7e410": //北京版
                    i = SqlHelper.ExecuteNonQuery(bj, CommandType.Text, sql);
                    break;
                case "1548d0a3-ca8e-4702-9c2c-f0ba0cacd385": //广州版
                    i = SqlHelper.ExecuteNonQuery(ot, CommandType.Text, sql);
                    break;
                case "241ea176-fce7-4bd7-a65f-a7978aac1cd2": //牛津深圳版
                    i = SqlHelper.ExecuteNonQuery(sz, CommandType.Text, sql);
                    break;
                case "37ca795d-42a6-4117-84f3-f4f856e03c62": //广东版
                    i = SqlHelper.ExecuteNonQuery(ot, CommandType.Text, sql);
                    break;
                case "41efcd18-ad8c-4585-8b6c-e7b61f49914c": //新课标标准实验版
                    i = SqlHelper.ExecuteNonQuery(ot, CommandType.Text, sql);
                    break;
                case "43716a9b-7ade-4137-bdc4-6362c9e1c999": //牛津上海本地版
                    i = SqlHelper.ExecuteNonQuery(ot, CommandType.Text, sql);
                    break;
                case "5373bbc9-49d4-47df-b5b5-ae196dc23d6d": //人教PEP版(同步学)
                    i = SqlHelper.ExecuteNonQuery(rjpep, CommandType.Text, sql);
                    break;
                case "64a8de22-cea0-4026-ab36-5a70f94dd6e4": //人教版新起点
                    i = SqlHelper.ExecuteNonQuery(rjpep, CommandType.Text, sql);
                    break;
                case "333d7cfc-cb4f-49d2-8ded-025e7d0fe766": //江苏译林
                    i = SqlHelper.ExecuteNonQuery(ot, CommandType.Text, sql);
                    break;
                case "8170b2bf-82a8-4c2d-9458-ae9d43cac5e3": //人教版
                    i = SqlHelper.ExecuteNonQuery(rjpep, CommandType.Text, sql);
                    break;
                case "9426808e-da8e-488c-9827-b082c19b62a7": //牛津上海全国版
                    i = SqlHelper.ExecuteNonQuery(ot, CommandType.Text, sql);
                    break;
                case "f0a9e1a7-b4cf-4a37-8fd1-932a66070afa": //山东版
                    i = SqlHelper.ExecuteNonQuery(ot, CommandType.Text, sql);
                    break;
                default:
                    break;
            }
            return i;
        }

        public DataSet SelectOrderSql(string AppID, string sql)
        {
            DataSet ds = new DataSet();
            switch (AppID)
            {
                case "0a94ceaf-8747-4266-bc05-ed8ae2e7e410": //北京版
                    ds = SqlHelper.ExecuteDataset(bj, CommandType.Text, sql);
                    break;
                case "1548d0a3-ca8e-4702-9c2c-f0ba0cacd385": //广州版
                    ds = SqlHelper.ExecuteDataset(ot, CommandType.Text, sql);
                    break;
                case "241ea176-fce7-4bd7-a65f-a7978aac1cd2": //牛津深圳版
                    ds = SqlHelper.ExecuteDataset(sz, CommandType.Text, sql);
                    break;
                case "37ca795d-42a6-4117-84f3-f4f856e03c62": //广东版
                    ds = SqlHelper.ExecuteDataset(ot, CommandType.Text, sql);
                    break;
                case "41efcd18-ad8c-4585-8b6c-e7b61f49914c": //新课标标准实验版
                    ds = SqlHelper.ExecuteDataset(ot, CommandType.Text, sql);
                    break;
                case "43716a9b-7ade-4137-bdc4-6362c9e1c999": //牛津上海本地版
                    ds = SqlHelper.ExecuteDataset(ot, CommandType.Text, sql);
                    break;
                case "5373bbc9-49d4-47df-b5b5-ae196dc23d6d": //人教PEP版(同步学)
                    ds = SqlHelper.ExecuteDataset(rjpep, CommandType.Text, sql);
                    break;
                case "64a8de22-cea0-4026-ab36-5a70f94dd6e4": //人教版新起点
                    ds = SqlHelper.ExecuteDataset(rjpep, CommandType.Text, sql);
                    break;
                case "333d7cfc-cb4f-49d2-8ded-025e7d0fe766": //江苏译林
                    ds = SqlHelper.ExecuteDataset(ot, CommandType.Text, sql);
                    break;
                case "8170b2bf-82a8-4c2d-9458-ae9d43cac5e3": //人教版
                    ds = SqlHelper.ExecuteDataset(rjpep, CommandType.Text, sql);
                    break;
                case "9426808e-da8e-488c-9827-b082c19b62a7": //牛津上海全国版
                    ds = SqlHelper.ExecuteDataset(ot, CommandType.Text, sql);
                    break;
                case "f0a9e1a7-b4cf-4a37-8fd1-932a66070afa": //山东版
                    ds = SqlHelper.ExecuteDataset(ot, CommandType.Text, sql);
                    break;
                default:
                    break;
            }
            return ds;
        }

        /// <summary>
        /// 支付确认(APP端支付成功后调用)
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        [HttpPost]
        public ApiResponse PostPaySuccess([FromBody] KingRequest request)
        {
            PostPaySucessInfo submitData = JsonHelper.DecodeJson<PostPaySucessInfo>(request.Data);
            if (submitData == null)
            {
                return GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.OrderID))
            {
                return GetErrorResult("订单ID不能为空");
            }


            if (string.IsNullOrEmpty(submitData.packageName))
            {
                submitData.packageName = "com.rj.syslearning";
            }

            try
            {
                string strsql = string.Format(@"SELECT [ID]
                                              ,[OrderID]
                                              ,[TotalMoney]
                                              ,[CreateDate]
                                              ,[State]
                                              ,[UserID]
                                              ,[PayWay]
                                              ,[FeeComboID]
                                              ,[CourseID]
                                              ,[IsDiscount]
                                              ,[SourceType] 
                                              ,[ModuleID] FROM dbo.TB_Order WHERE OrderID='{0}'", submitData.OrderID);
                DataSet ds = SelectOrderSql(submitData.AppID, strsql);

                if (ds.Tables.Count > 0)
                {
                    List<TB_Order> orderList = JsonHelper.DataSetToIList<TB_Order>(ds, 0);
                    if (orderList != null)
                    {
                        var order = orderList.FirstOrDefault();
                        if (order != null)
                        {
                            if (order.PayWay.Contains("支付宝支付"))
                            {
                                AliPay.Util.AliPay ali = new AliPay.Util.AliPay();
                                bool state = ali.Query_Order(submitData.OrderID);
                                if (state)
                                {
                                    string sql = string.Format(@"UPDATE	dbo.TB_Order SET State='0001' WHERE OrderID='{0}'", submitData.OrderID);
                                    var i = OrderSql(submitData.AppID, sql);
                                    if (i > 0)
                                    {
                                        order.State = "0001";
                                        bool success = ConcurrentOrder(order);
                                        if (!success)
                                        {
                                            Log4Net.LogHelper.Error("订单同步到财富分账失败：" + JsonHelper.EncodeJson(order));
                                        }
                                        #region 分库订单写入队列
                                        listredis.LPush("Order2BaseDBQueue", order.ToJson());
                                        #endregion
                                        return GetResult(NewQueryCombo(submitData.AppID, order.UserID.ToString(), submitData.HasModule), "支付成功");
                                    }
                                }
                            }

                            if (order.PayWay.Contains("微信支付"))
                            {
                                WxPayData datas = new WxPayData();
                                datas.SetValue("out_trade_no", submitData.OrderID);
                                datas.SetValue("attach", submitData.packageName);//附加数据

                                WxPayData results = WxPayApi.OrderQuery(datas); //调用查询订单接口
                                Log4Net.LogHelper.Info("查询订单接口，return_code=" + results.GetValue("return_code").ToString() + ";result_code=" + results.GetValue("result_code").ToString());
                                if (results.GetValue("return_code").ToString() == "SUCCESS")
                                {
                                    if (results.GetValue("result_code").ToString() == "SUCCESS")
                                    {
                                        if (results.GetValue("trade_state").ToString() == "SUCCESS")
                                        {
                                            string sql = string.Format(@"UPDATE	dbo.TB_Order SET State='0001' WHERE OrderID='{0}'", submitData.OrderID);
                                            var i = OrderSql(submitData.AppID, sql);
                                            if (i > 0)
                                            {
                                                order.State = "0001";
                                                bool success = ConcurrentOrder(order);
                                                if (!success)
                                                {
                                                    Log4Net.LogHelper.Error("订单同步到财富分账失败：" + JsonHelper.EncodeJson(order));
                                                }
                                                #region 分库订单写入队列
                                                listredis.LPush("Order2BaseDBQueue", order.ToJson());
                                                #endregion
                                                return GetResult(NewQueryCombo(submitData.AppID, order.UserID,submitData.HasModule), "支付成功");
                                            }
                                        }
                                        else
                                        {
                                            return GetErrorResult("支付系统订单状态不正确！");
                                        }
                                    }
                                    else
                                    {
                                        return GetErrorResult("支付系统订单状态不正确！");
                                    }
                                }
                                else
                                {
                                    return GetErrorResult("支付系统订单状态不正确！");
                                }
                            }
                            return GetErrorResult("支付订单状态异常！");
                        }
                        else
                        {
                            Log4Net.LogHelper.Info("订单信息：" + JsonHelper.EncodeJson(orderList));
                            return GetErrorResult("订单数据信息异常！");
                        }
                    }
                    else
                    {
                        return GetErrorResult("订单信息异常！");
                    }
                }
                else
                {
                    return GetErrorResult("订单不存在！");
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
        public ApiResponse GetPaySuccessTest()
        {
            PaySucessInfo submitData = new PaySucessInfo();//JsonHelper.DecodeJson<PaySucessInfo>(request.Data);
            submitData.AppID = "241ea176-fce7-4bd7-a65f-a7978aac1cd2";
            submitData.OrderID = "148350548220180123175846364";
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
                string strsql = string.Format(@"SELECT [ID]
                                              ,[OrderID]
                                              ,[TotalMoney]
                                              ,[CreateDate]
                                              ,[State]
                                              ,[UserID]
                                              ,[PayWay]
                                              ,[FeeComboID]
                                              ,[CourseID]
                                              ,[IsDiscount]
                                              ,[SourceType] FROM dbo.TB_Order WHERE OrderID='{0}'", submitData.OrderID);
                DataSet ds = SelectOrderSql(submitData.AppID, strsql);

                if (ds.Tables.Count > 0)
                {
                    List<TB_Order> orderList = Extension.Convert2Object<TB_Order>(ds.Tables[0]);
                    if (orderList != null)
                    {
                        var order = orderList.FirstOrDefault();
                        if (order != null)
                        {
                            AccountController accc = new AccountController();
                            if (order.PayWay.Contains("支付宝支付"))
                            {
                                AliPay.Util.AliPay ali = new AliPay.Util.AliPay();
                                bool state = ali.Query_Order(submitData.OrderID);
                                if (state)
                                {
                                    string sql = string.Format(@"UPDATE	dbo.TB_Order SET State='0001' WHERE OrderID='{0}'", submitData.OrderID);
                                    var i = OrderSql(submitData.AppID, sql);
                                    if (i > 0)
                                    {
                                        #region 分库订单写入队列
                                        listredis.LPush("Order2BaseDBQueue", order.ToJson());
                                        #endregion
                                        return GetResult(NewQueryCombo(submitData.AppID, ds.Tables[0].Rows[0]["UserID"].ToString(), null), "支付成功");
                                    }
                                }
                            }

                            if (order.PayWay.Contains("微信支付"))
                            {
                                WxPayData datas = new WxPayData();
                                datas.SetValue("out_trade_no", submitData.OrderID);

                                WxPayData results = WxPayApi.OrderQuery(datas); //调用查询订单接口
                                if (results.GetValue("return_code").ToString() == "SUCCESS")
                                {
                                    if (results.GetValue("result_code").ToString() == "SUCCESS")
                                    {
                                        if (results.GetValue("trade_state").ToString() == "SUCCESS")
                                        {
                                            string sql = string.Format(@"UPDATE	dbo.TB_Order SET State='0001' WHERE OrderID='{0}'", submitData.OrderID);
                                            var i = OrderSql(submitData.AppID, sql);
                                            if (i > 0)
                                            {
                                                return GetResult(NewQueryCombo(submitData.AppID, order.UserID, null), "支付成功");
                                            }
                                        }
                                        else
                                        {
                                            return GetErrorResult("支付系统订单状态不正确！");
                                        }
                                    }
                                    else
                                    {
                                        return GetErrorResult("支付系统订单状态不正确！");
                                    }
                                }
                            }
                        }
                    }

                }

                return GetErrorResult("支付状态异常！");

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
                    UserTicketID = Guid.Empty,
                    IsYX = 0,
                    AppID = "",
                    ModuleID = ""
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
                    order.SourceType = obj.IsYX;
                    int SanBoxTest = 0;
                    if (sc.isSanBox == "1")
                    {
                        SanBoxTest = 1;//表示沙盒订单
                    }
                    order.CreateDate = DateTime.Now;
                    order.TotalMoney = obj.TotalMoney;
                    order.UserID = obj.UserID;
                    order.State = "0001";
                    order.IsDiscount = obj.IsDiscount;
                    AccountController accc = new AccountController();

                    #region 创建订单
                    try
                    {
                        UserManagement usermanager = new UserManagement();
                        string sql = string.Format(@"INSERT INTO dbo.TB_Order ( ID ,OrderID ,TotalMoney ,CreateDate ,State ,UserID ,PayWay ,FeeComboID ,CourseID ,IsDiscount ,SourceType, ModuleID)
                                                              VALUES  ( '{0}' , '{1}' , '{2}' , '{3}' , '{4}' , '{5}' , '{6}' , '{7}' , '{8}' , '{9}' , '{10}', '{11}'  )",
                                                                      order.ID, order.OrderID, order.TotalMoney, order.CreateDate, order.State, order.UserID, order.PayWay, order.FeeComboID, order.CourseID, order.IsDiscount, order.SourceType, order.ModuleID);
                        var j = OrderSql(obj.AppID, sql);
                        if (j > 0)
                        {
                            #region 分库订单写入队列
                            listredis.LPush("Order2BaseDBQueue", order.ToJson());
                            #endregion
                            return GetResult(accc.QueryCombo(obj.UserID), "支付成功");
                        }
                        else
                        {
                            Log4Net.LogHelper.Info("苹果支付插入订单失败:" + JsonHelper.EncodeJson(order));
                            return GetErrorResult("苹果支付插入订单失败");
                        }
                        //o.Insert<TB_Order>(order);
                        //
                    }
                    catch (SqlException ex)
                    {
                        switch (ex.Number)
                        {
                            case 2627:  //重复插入
                                Log4Net.LogHelper.Error(ex, "苹果支付插入订单重复,order=" + JsonHelper.EncodeJson(order));
                                return GetResult(accc.QueryCombo(obj.UserID), "支付成功");
                            //return KVpi(order.OrderID, SanBoxTest);
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
                    Log4Net.LogHelper.Info("苹果支付订单无效");
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
        public ApiResponse AddSuccessOrderTest()
        {
            try
            {
                var obj = new
                {
                    AppleOrderID = "1000000368438537",
                    AppleTicket = "{\"receipt-data\":\"MIIknwYJKoZIhvcNAQcCoIIkkDCCJIwCAQExCzAJBgUrDgMCGgUAMIIUQAYJKoZIhvcNAQcBoIIUMQSCFC0xghQpMAoCAQgCAQEEAhYAMAoCARQCAQEEAgwAMAsCAQECAQEEAwIBADALAgELAgEBBAMCAQAwCwIBDgIBAQQDAgFrMAsCAQ8CAQEEAwIBADALAgEQAgEBBAMCAQAwCwIBGQIBAQQDAgEDMAwCAQMCAQEEBAwCMjIwDAIBCgIBAQQEFgI0KzANAgENAgEBBAUCAwGuejANAgETAgEBBAUMAzEuMDAOAgEJAgEBBAYCBFAyNTAwGAIBBAIBAgQQbnVB5EAjgX8f2VKeRCCyqDAbAgEAAgEBBBMMEVByb2R1Y3Rpb25TYW5kYm94MBwCAQUCAQEEFD3lL17Av4z1wvMoFQu41CZ8mvW\\/MB4CAQwCAQEEFhYUMjAxOC0wMS0yNFQwOTo0MzozNFowHgIBEgIBAQQWFhQyMDEzLTA4LTAxVDA3OjAwOjAwWjAoAgECAgEBBCAMHmNvbS5raW5nc3Vuc29mdC5zdW5ueWNsYXNzLnBlcDBPAgEHAgEBBEfRBp2\\/\\/qyz+SXbQmcljmA+FtP4M1Q1h4UyzgnIZxEsDlSGiMHCf1AGisHJOlE9\\/D2HblJ+muksI+\\/yKAAI9y5R9\\/p7XqdokDBTAgEGAgEBBEuPNHHE5J4pvm4+qu7KIM1OfPTbIb1pvGDYy87MWK8dhD+iNRmusXrZlj+3eJB8VBrFaV5OBzM3MlAIZSwjoAQ9\\/eGwVLtBIb+Oo+AwggFgAgERAgEBBIIBVjGCAVIwCwICBqwCAQEEAhYAMAsCAgatAgEBBAIMADALAgIGsAIBAQQCFgAwCwICBrICAQEEAgwAMAsCAgazAgEBBAIMADALAgIGtAIBAQQCDAAwCwICBrUCAQEEAgwAMAsCAga2AgEBBAIMADAMAgIGpQIBAQQDAgEBMAwCAgarAgEBBAMCAQIwDAICBq4CAQEEAwIBADAMAgIGrwIBAQQDAgEAMAwCAgaxAgEBBAMCAQAwGwICBqcCAQEEEgwQMTAwMDAwMDMzMDE3MDIxMDAbAgIGqQIBAQQSDBAxMDAwMDAwMzMwMTcwMjEwMB8CAgaoAgEBBBYWFDIwMTctMDktMDJUMTA6MDk6MzBaMB8CAgaqAgEBBBYWFDIwMTctMDktMDJUMTA6MDk6MzBaMCYCAgamAgEBBB0MG3N1bm55Y2xhc3NfMV8xMjE2NjI2NTk2XzEwNzCCAWACARECAQEEggFWMYIBUjALAgIGrAIBAQQCFgAwCwICBq0CAQEEAgwAMAsCAgawAgEBBAIWADALAgIGsgIBAQQCDAAwCwICBrMCAQEEAgwAMAsCAga0AgEBBAIMADALAgIGtQIBAQQCDAAwCwICBrYCAQEEAgwAMAwCAgalAgEBBAMCAQEwDAICBqsCAQEEAwIBAjAMAgIGrgIBAQQDAgEAMAwCAgavAgEBBAMCAQAwDAICBrECAQEEAwIBADAbAgIGpwIBAQQSDBAxMDAwMDAwMzMwMTcwODU2MBsCAgapAgEBBBIMEDEwMDAwMDAzMzAxNzA4NTYwHwICBqgCAQEEFhYUMjAxNy0wOS0wMlQxMDoxNjo1MFowHwICBqoCAQEEFhYUMjAxNy0wOS0wMlQxMDoxNjo1MFowJgICBqYCAQEEHQwbc3VubnljbGFzc18xXzEyMTY2MjY1OTZfMTA3MIIBYAIBEQIBAQSCAVYxggFSMAsCAgasAgEBBAIWADALAgIGrQIBAQQCDAAwCwICBrACAQEEAhYAMAsCAgayAgEBBAIMADALAgIGswIBAQQCDAAwCwICBrQCAQEEAgwAMAsCAga1AgEBBAIMADALAgIGtgIBAQQCDAAwDAICBqUCAQEEAwIBATAMAgIGqwIBAQQDAgECMAwCAgauAgEBBAMCAQAwDAICBq8CAQEEAwIBADAMAgIGsQIBAQQDAgEAMBsCAganAgEBBBIMEDEwMDAwMDAzMzAxODk1MDIwGwICBqkCAQEEEgwQMTAwMDAwMDMzMDE4OTUwMjAfAgIGqAIBAQQWFhQyMDE3LTA5LTAyVDE2OjQ2OjIzWjAfAgIGqgIBAQQWFhQyMDE3LTA5LTAyVDE2OjQ2OjIzWjAmAgIGpgIBAQQdDBtzdW5ueWNsYXNzXzFfMTIxNjYyNjU5Nl8xMDYwggFgAgERAgEBBIIBVjGCAVIwCwICBqwCAQEEAhYAMAsCAgatAgEBBAIMADALAgIGsAIBAQQCFgAwCwICBrICAQEEAgwAMAsCAgazAgEBBAIMADALAgIGtAIBAQQCDAAwCwICBrUCAQEEAgwAMAsCAga2AgEBBAIMADAMAgIGpQIBAQQDAgEBMAwCAgarAgEBBAMCAQIwDAICBq4CAQEEAwIBADAMAgIGrwIBAQQDAgEAMAwCAgaxAgEBBAMCAQAwGwICBqcCAQEEEgwQMTAwMDAwMDM1MzM5MTQzNzAbAgIGqQIBAQQSDBAxMDAwMDAwMzUzMzkxNDM3MB8CAgaoAgEBBBYWFDIwMTctMTEtMjBUMDM6MDQ6MzVaMB8CAgaqAgEBBBYWFDIwMTctMTEtMjBUMDM6MDQ6MzVaMCYCAgamAgEBBB0MG3N1bm55Y2xhc3NfMV8xMjE2NjI2NTk2XzEwNTCCAWACARECAQEEggFWMYIBUjALAgIGrAIBAQQCFgAwCwICBq0CAQEEAgwAMAsCAgawAgEBBAIWADALAgIGsgIBAQQCDAAwCwICBrMCAQEEAgwAMAsCAga0AgEBBAIMADALAgIGtQIBAQQCDAAwCwICBrYCAQEEAgwAMAwCAgalAgEBBAMCAQEwDAICBqsCAQEEAwIBAjAMAgIGrgIBAQQDAgEAMAwCAgavAgEBBAMCAQAwDAICBrECAQEEAwIBADAbAgIGpwIBAQQSDBAxMDAwMDAwMzUzMzkyMTY4MBsCAgapAgEBBBIMEDEwMDAwMDAzNTMzOTIxNjgwHwICBqgCAQEEFhYUMjAxNy0xMS0yMFQwMzoxMjo0NFowHwICBqoCAQEEFhYUMjAxNy0xMS0yMFQwMzoxMjo0NFowJgICBqYCAQEEHQwbc3VubnljbGFzc18xXzEyMTY2MjY1OTZfMTA2MIIBYAIBEQIBAQSCAVYxggFSMAsCAgasAgEBBAIWADALAgIGrQIBAQQCDAAwCwICBrACAQEEAhYAMAsCAgayAgEBBAIMADALAgIGswIBAQQCDAAwCwICBrQCAQEEAgwAMAsCAga1AgEBBAIMADALAgIGtgIBAQQCDAAwDAICBqUCAQEEAwIBATAMAgIGqwIBAQQDAgECMAwCAgauAgEBBAMCAQAwDAICBq8CAQEEAwIBADAMAgIGsQIBAQQDAgEAMBsCAganAgEBBBIMEDEwMDAwMDAzNTMzOTMxNDkwGwICBqkCAQEEEgwQMTAwMDAwMDM1MzM5MzE0OTAfAgIGqAIBAQQWFhQyMDE3LTExLTIwVDAzOjE2OjUyWjAfAgIGqgIBAQQWFhQyMDE3LTExLTIwVDAzOjE2OjUyWjAmAgIGpgIBAQQdDBtzdW5ueWNsYXNzXzFfMTIxNjYyNjU5Nl8xMDcwggFgAgERAgEBBIIBVjGCAVIwCwICBqwCAQEEAhYAMAsCAgatAgEBBAIMADALAgIGsAIBAQQCFgAwCwICBrICAQEEAgwAMAsCAgazAgEBBAIMADALAgIGtAIBAQQCDAAwCwICBrUCAQEEAgwAMAsCAga2AgEBBAIMADAMAgIGpQIBAQQDAgEBMAwCAgarAgEBBAMCAQIwDAICBq4CAQEEAwIBADAMAgIGrwIBAQQDAgEAMAwCAgaxAgEBBAMCAQAwGwICBqcCAQEEEgwQMTAwMDAwMDM1MzM5NjI3OTAbAgIGqQIBAQQSDBAxMDAwMDAwMzUzMzk2Mjc5MB8CAgaoAgEBBBYWFDIwMTctMTEtMjBUMDM6Mjc6NTNaMB8CAgaqAgEBBBYWFDIwMTctMTEtMjBUMDM6Mjc6NTNaMCYCAgamAgEBBB0MG3N1bm55Y2xhc3NfMV8xMjE2NjI2NTk2XzEwNjCCAWACARECAQEEggFWMYIBUjALAgIGrAIBAQQCFgAwCwICBq0CAQEEAgwAMAsCAgawAgEBBAIWADALAgIGsgIBAQQCDAAwCwICBrMCAQEEAgwAMAsCAga0AgEBBAIMADALAgIGtQIBAQQCDAAwCwICBrYCAQEEAgwAMAwCAgalAgEBBAMCAQEwDAICBqsCAQEEAwIBAjAMAgIGrgIBAQQDAgEAMAwCAgavAgEBBAMCAQAwDAICBrECAQEEAwIBADAbAgIGpwIBAQQSDBAxMDAwMDAwMzUzNDEzOTEyMBsCAgapAgEBBBIMEDEwMDAwMDAzNTM0MTM5MTIwHwICBqgCAQEEFhYUMjAxNy0xMS0yMFQwNTo1Mjo0NVowHwICBqoCAQEEFhYUMjAxNy0xMS0yMFQwNTo1Mjo0NVowJgICBqYCAQEEHQwbc3VubnljbGFzc18xXzEyMTY2MjY1OTZfMTA2MIIBYAIBEQIBAQSCAVYxggFSMAsCAgasAgEBBAIWADALAgIGrQIBAQQCDAAwCwICBrACAQEEAhYAMAsCAgayAgEBBAIMADALAgIGswIBAQQCDAAwCwICBrQCAQEEAgwAMAsCAga1AgEBBAIMADALAgIGtgIBAQQCDAAwDAICBqUCAQEEAwIBATAMAgIGqwIBAQQDAgECMAwCAgauAgEBBAMCAQAwDAICBq8CAQEEAwIBADAMAgIGsQIBAQQDAgEAMBsCAganAgEBBBIMEDEwMDAwMDAzNjM1MjIzODEwGwICBqkCAQEEEgwQMTAwMDAwMDM2MzUyMjM4MTAfAgIGqAIBAQQWFhQyMDE4LTAxLTAzVDAxOjMyOjM0WjAfAgIGqgIBAQQWFhQyMDE4LTAxLTAzVDAxOjMyOjM0WjAmAgIGpgIBAQQdDBtzdW5ueWNsYXNzXzFfMTIxNjYyNjU5Nl8yNjUwggFgAgERAgEBBIIBVjGCAVIwCwICBqwCAQEEAhYAMAsCAgatAgEBBAIMADALAgIGsAIBAQQCFgAwCwICBrICAQEEAgwAMAsCAgazAgEBBAIMADALAgIGtAIBAQQCDAAwCwICBrUCAQEEAgwAMAsCAga2AgEBBAIMADAMAgIGpQIBAQQDAgEBMAwCAgarAgEBBAMCAQIwDAICBq4CAQEEAwIBADAMAgIGrwIBAQQDAgEAMAwCAgaxAgEBBAMCAQAwGwICBqcCAQEEEgwQMTAwMDAwMDM2NzkxMjg0MDAbAgIGqQIBAQQSDBAxMDAwMDAwMzY3OTEyODQwMB8CAgaoAgEBBBYWFDIwMTgtMDEtMTlUMDY6MzA6NTNaMB8CAgaqAgEBBBYWFDIwMTgtMDEtMTlUMDY6MzA6NTNaMCYCAgamAgEBBB0MG3N1bm55Y2xhc3NfMV8xMjE2NjI2NTk2XzEwNDCCAWACARECAQEEggFWMYIBUjALAgIGrAIBAQQCFgAwCwICBq0CAQEEAgwAMAsCAgawAgEBBAIWADALAgIGsgIBAQQCDAAwCwICBrMCAQEEAgwAMAsCAga0AgEBBAIMADALAgIGtQIBAQQCDAAwCwICBrYCAQEEAgwAMAwCAgalAgEBBAMCAQEwDAICBqsCAQEEAwIBAjAMAgIGrgIBAQQDAgEAMAwCAgavAgEBBAMCAQAwDAICBrECAQEEAwIBADAbAgIGpwIBAQQSDBAxMDAwMDAwMzY4NDM4NTM1MBsCAgapAgEBBBIMEDEwMDAwMDAzNjg0Mzg1MzUwHwICBqgCAQEEFhYUMjAxOC0wMS0yMlQwMToyODo1NFowHwICBqoCAQEEFhYUMjAxOC0wMS0yMlQwMToyODo1NFowJgICBqYCAQEEHQwbc3VubnljbGFzc18xXzEyMTY2MjY1OTZfMTY4MIIBYAIBEQIBAQSCAVYxggFSMAsCAgasAgEBBAIWADALAgIGrQIBAQQCDAAwCwICBrACAQEEAhYAMAsCAgayAgEBBAIMADALAgIGswIBAQQCDAAwCwICBrQCAQEEAgwAMAsCAga1AgEBBAIMADALAgIGtgIBAQQCDAAwDAICBqUCAQEEAwIBATAMAgIGqwIBAQQDAgECMAwCAgauAgEBBAMCAQAwDAICBq8CAQEEAwIBADAMAgIGsQIBAQQDAgEAMBsCAganAgEBBBIMEDEwMDAwMDAzNjg0Mzg1MzcwGwICBqkCAQEEEgwQMTAwMDAwMDM2ODQzODUzNzAfAgIGqAIBAQQWFhQyMDE4LTAxLTIyVDAxOjI5OjA4WjAfAgIGqgIBAQQWFhQyMDE4LTAxLTIyVDAxOjI5OjA4WjAmAgIGpgIBAQQdDBtzdW5ueWNsYXNzXzFfMTIxNjYyNjU5Nl8yNjMwggFgAgERAgEBBIIBVjGCAVIwCwICBqwCAQEEAhYAMAsCAgatAgEBBAIMADALAgIGsAIBAQQCFgAwCwICBrICAQEEAgwAMAsCAgazAgEBBAIMADALAgIGtAIBAQQCDAAwCwICBrUCAQEEAgwAMAsCAga2AgEBBAIMADAMAgIGpQIBAQQDAgEBMAwCAgarAgEBBAMCAQIwDAICBq4CAQEEAwIBADAMAgIGrwIBAQQDAgEAMAwCAgaxAgEBBAMCAQAwGwICBqcCAQEEEgwQMTAwMDAwMDM2OTQ3NzU2MzAbAgIGqQIBAQQSDBAxMDAwMDAwMzY5NDc3NTYzMB8CAgaoAgEBBBYWFDIwMTgtMDEtMjRUMDc6NDA6MjJaMB8CAgaqAgEBBBYWFDIwMTgtMDEtMjRUMDc6NDA6MjJaMCYCAgamAgEBBB0MG3N1bm55Y2xhc3NfMV8xMjE2NjI2NTk2XzQwNKCCDmUwggV8MIIEZKADAgECAggO61eH554JjTANBgkqhkiG9w0BAQUFADCBljELMAkGA1UEBhMCVVMxEzARBgNVBAoMCkFwcGxlIEluYy4xLDAqBgNVBAsMI0FwcGxlIFdvcmxkd2lkZSBEZXZlbG9wZXIgUmVsYXRpb25zMUQwQgYDVQQDDDtBcHBsZSBXb3JsZHdpZGUgRGV2ZWxvcGVyIFJlbGF0aW9ucyBDZXJ0aWZpY2F0aW9uIEF1dGhvcml0eTAeFw0xNTExMTMwMjE1MDlaFw0yMzAyMDcyMTQ4NDdaMIGJMTcwNQYDVQQDDC5NYWMgQXBwIFN0b3JlIGFuZCBpVHVuZXMgU3RvcmUgUmVjZWlwdCBTaWduaW5nMSwwKgYDVQQLDCNBcHBsZSBXb3JsZHdpZGUgRGV2ZWxvcGVyIFJlbGF0aW9uczETMBEGA1UECgwKQXBwbGUgSW5jLjELMAkGA1UEBhMCVVMwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQClz4H9JaKBW9aH7SPaMxyO4iPApcQmyz3Gn+xKDVWG\\/6QC15fKOVRtfX+yVBidxCxScY5ke4LOibpJ1gjltIhxzz9bRi7GxB24A6lYogQ+IXjV27fQjhKNg0xbKmg3k8LyvR7E0qEMSlhSqxLj7d0fmBWQNS3CzBLKjUiB91h4VGvojDE2H0oGDEdU8zeQuLKSiX1fpIVK4cCc4Lqku4KXY\\/Qrk8H9Pm\\/KwfU8qY9SGsAlCnYO3v6Z\\/v\\/Ca\\/VbXqxzUUkIVonMQ5DMjoEC0KCXtlyxoWlph5AQaCYmObgdEHOwCl3Fc9DfdjvYLdmIHuPsB8\\/ijtDT+iZVge\\/iA0kjAgMBAAGjggHXMIIB0zA\\/BggrBgEFBQcBAQQzMDEwLwYIKwYBBQUHMAGGI2h0dHA6Ly9vY3NwLmFwcGxlLmNvbS9vY3NwMDMtd3dkcjA0MB0GA1UdDgQWBBSRpJz8xHa3n6CK9E31jzZd7SsEhTAMBgNVHRMBAf8EAjAAMB8GA1UdIwQYMBaAFIgnFwmpthhgi+zruvZHWcVSVKO3MIIBHgYDVR0gBIIBFTCCAREwggENBgoqhkiG92NkBQYBMIH+MIHDBggrBgEFBQcCAjCBtgyBs1JlbGlhbmNlIG9uIHRoaXMgY2VydGlmaWNhdGUgYnkgYW55IHBhcnR5IGFzc3VtZXMgYWNjZXB0YW5jZSBvZiB0aGUgdGhlbiBhcHBsaWNhYmxlIHN0YW5kYXJkIHRlcm1zIGFuZCBjb25kaXRpb25zIG9mIHVzZSwgY2VydGlmaWNhdGUgcG9saWN5IGFuZCBjZXJ0aWZpY2F0aW9uIHByYWN0aWNlIHN0YXRlbWVudHMuMDYGCCsGAQUFBwIBFipodHRwOi8vd3d3LmFwcGxlLmNvbS9jZXJ0aWZpY2F0ZWF1dGhvcml0eS8wDgYDVR0PAQH\\/BAQDAgeAMBAGCiqGSIb3Y2QGCwEEAgUAMA0GCSqGSIb3DQEBBQUAA4IBAQANphvTLj3jWysHbkKWbNPojEMwgl\\/gXNGNvr0PvRr8JZLbjIXDgFnf4+LXLgUUrA3btrj+\\/DUufMutF2uOfx\\/kd7mxZ5W0E16mGYZ2+FogledjjA9z\\/Ojtxh+umfhlSFyg4Cg6wBA3LbmgBDkfc7nIBf3y3n8aKipuKwH8oCBc2et9J6Yz+PWY4L5E27FMZ\\/xuCk\\/J4gao0pfzp45rUaJahHVl0RYEYuPBX\\/UIqc9o2ZIAycGMs\\/iNAGS6WGDAfK+PdcppuVsq1h1obphC9UynNxmbzDscehlD86Ntv0hgBgw2kivs3hi1EdotI9CO\\/KBpnBcbnoB7OUdFMGEvxxOoMIIEIjCCAwqgAwIBAgIIAd68xDltoBAwDQYJKoZIhvcNAQEFBQAwYjELMAkGA1UEBhMCVVMxEzARBgNVBAoTCkFwcGxlIEluYy4xJjAkBgNVBAsTHUFwcGxlIENlcnRpZmljYXRpb24gQXV0aG9yaXR5MRYwFAYDVQQDEw1BcHBsZSBSb290IENBMB4XDTEzMDIwNzIxNDg0N1oXDTIzMDIwNzIxNDg0N1owgZYxCzAJBgNVBAYTAlVTMRMwEQYDVQQKDApBcHBsZSBJbmMuMSwwKgYDVQQLDCNBcHBsZSBXb3JsZHdpZGUgRGV2ZWxvcGVyIFJlbGF0aW9uczFEMEIGA1UEAww7QXBwbGUgV29ybGR3aWRlIERldmVsb3BlciBSZWxhdGlvbnMgQ2VydGlmaWNhdGlvbiBBdXRob3JpdHkwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDKOFSmy1aqyCQ5SOmM7uxfuH8mkbw0U3rOfGOAYXdkXqUHI7Y5\\/lAtFVZYcC1+xG7BSoU+L\\/DehBqhV8mvexj\\/avoVEkkVCBmsqtsqMu2WY2hSFT2Miuy\\/axiV4AOsAX2XBWfODoWVN2rtCbauZ81RZJ\\/GXNG8V25nNYB2NqSHgW44j9grFU57Jdhav06DwY3Sk9UacbVgnJ0zTlX5ElgMhrgWDcHld0WNUEi6Ky3klIXh6MSdxmilsKP8Z35wugJZS3dCkTm59c3hTO\\/AO0iMpuUhXf1qarunFjVg0uat80YpyejDi+l5wGphZxWy8P3laLxiX27Pmd3vG2P+kmWrAgMBAAGjgaYwgaMwHQYDVR0OBBYEFIgnFwmpthhgi+zruvZHWcVSVKO3MA8GA1UdEwEB\\/wQFMAMBAf8wHwYDVR0jBBgwFoAUK9BpR5R2Cf70a40uQKb3R01\\/CF4wLgYDVR0fBCcwJTAjoCGgH4YdaHR0cDovL2NybC5hcHBsZS5jb20vcm9vdC5jcmwwDgYDVR0PAQH\\/BAQDAgGGMBAGCiqGSIb3Y2QGAgEEAgUAMA0GCSqGSIb3DQEBBQUAA4IBAQBPz+9Zviz1smwvj+4ThzLoBTWobot9yWkMudkXvHcs1Gfi\\/ZptOllc34MBvbKuKmFysa\\/Nw0Uwj6ODDc4dR7Txk4qjdJukw5hyhzs+r0ULklS5MruQGFNrCk4QttkdUGwhgAqJTleMa1s8Pab93vcNIx0LSiaHP7qRkkykGRIZbVf1eliHe2iK5IaMSuviSRSqpd1VAKmuu0swruGgsbwpgOYJd+W+NKIByn\\/c4grmO7i77LpilfMFY0GCzQ87HUyVpNur+cmV6U\\/kTecmmYHpvPm0KdIBembhLoz2IYrF+Hjhga6\\/05Cdqa3zr\\/04GpZnMBxRpVzscYqCtGwPDBUfMIIEuzCCA6OgAwIBAgIBAjANBgkqhkiG9w0BAQUFADBiMQswCQYDVQQGEwJVUzETMBEGA1UEChMKQXBwbGUgSW5jLjEmMCQGA1UECxMdQXBwbGUgQ2VydGlmaWNhdGlvbiBBdXRob3JpdHkxFjAUBgNVBAMTDUFwcGxlIFJvb3QgQ0EwHhcNMDYwNDI1MjE0MDM2WhcNMzUwMjA5MjE0MDM2WjBiMQswCQYDVQQGEwJVUzETMBEGA1UEChMKQXBwbGUgSW5jLjEmMCQGA1UECxMdQXBwbGUgQ2VydGlmaWNhdGlvbiBBdXRob3JpdHkxFjAUBgNVBAMTDUFwcGxlIFJvb3QgQ0EwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDkkakJH5HbHkdQ6wXtXnmELes2oldMVeyLGYne+Uts9QerIjAC6Bg++FAJ039BqJj50cpmnCRrEdCju+QbKsMflZ56DKRHi1vUFjczy8QPTc4UadHJGXL1XQ7Vf1+b8iUDulWPTV0N8WQ1IxVLFVkds5T39pyez1C6wVhQZ48ItCD3y6wsIG9wtj8BMIy3Q88PnT3zK0koGsj+zrW5DtleHNbLPbU6rfQPDgCSC7EhFi501TwN22IWq6NxkkdTVcGvL0Gz+PvjcM3mo0xFfh9Ma1CWQYnEdGILEINBhzOKgbEwWOxaBDKMaLOPHd5lc\\/9nXmW8Sdh2nzMUZaF3lMktAgMBAAGjggF6MIIBdjAOBgNVHQ8BAf8EBAMCAQYwDwYDVR0TAQH\\/BAUwAwEB\\/zAdBgNVHQ4EFgQUK9BpR5R2Cf70a40uQKb3R01\\/CF4wHwYDVR0jBBgwFoAUK9BpR5R2Cf70a40uQKb3R01\\/CF4wggERBgNVHSAEggEIMIIBBDCCAQAGCSqGSIb3Y2QFATCB8jAqBggrBgEFBQcCARYeaHR0cHM6Ly93d3cuYXBwbGUuY29tL2FwcGxlY2EvMIHDBggrBgEFBQcCAjCBthqBs1JlbGlhbmNlIG9uIHRoaXMgY2VydGlmaWNhdGUgYnkgYW55IHBhcnR5IGFzc3VtZXMgYWNjZXB0YW5jZSBvZiB0aGUgdGhlbiBhcHBsaWNhYmxlIHN0YW5kYXJkIHRlcm1zIGFuZCBjb25kaXRpb25zIG9mIHVzZSwgY2VydGlmaWNhdGUgcG9saWN5IGFuZCBjZXJ0aWZpY2F0aW9uIHByYWN0aWNlIHN0YXRlbWVudHMuMA0GCSqGSIb3DQEBBQUAA4IBAQBcNplMLXi37Yyb3PN3m\\/J20ncwT8EfhYOFG5k9RzfyqZtAjizUsZAS2L70c5vu0mQPy3lPNNiiPvl4\\/2vIB+x9OYOLUyDTOMSxv5pPCmv\\/K\\/xZpwUJfBdAVhEedNO3iyM7R6PVbyTi69G3cN8PReEnyvFteO3ntRcXqNx+IjXKJdXZD9Zr1KIkIxH3oayPc4FgxhtbCS+SsvhESPBgOJ4V9T0mZyCKM2r3DYLP3uujL\\/lTaltkwGMzd\\/c6ByxW69oPIQ7aunMZT7XZNn\\/Bh1XZp5m5MkL72NVxnn6hUrcbvZNCJBIqxw8dtk2cXmPIS4AXUKqK1drk\\/NAJBzewdXUhMYIByzCCAccCAQEwgaMwgZYxCzAJBgNVBAYTAlVTMRMwEQYDVQQKDApBcHBsZSBJbmMuMSwwKgYDVQQLDCNBcHBsZSBXb3JsZHdpZGUgRGV2ZWxvcGVyIFJlbGF0aW9uczFEMEIGA1UEAww7QXBwbGUgV29ybGR3aWRlIERldmVsb3BlciBSZWxhdGlvbnMgQ2VydGlmaWNhdGlvbiBBdXRob3JpdHkCCA7rV4fnngmNMAkGBSsOAwIaBQAwDQYJKoZIhvcNAQEBBQAEggEABM3ZVGUcG9ZkDCj+lUIiZ43wUp1cYaGm+ao0wmqmim00PHg9cElSW+2KnIdHinXFhNGJ11VC4lqMTwgovbLrHLbHp7BMHxwMXT0j6+r+M5is2A6Fa7oQ1anuQS6Ing2gYv289I3CZtDhQapqW5ce+WL5iTqfXgPZH2KhW04efd0WtD2tpkodqCE2VI5Qir0Zj7Pd\\/02T0hYR7CPkkN9kAebSKbX8MXsfwsBo1QNXeJ47+VoAb2pc0SZwiFSZzIEALUCuHDwJufIQow4qOTegIvHcugXgzViRPqLoFc2ca07o9Ze8AykWLYXItee2AraVD1MIFWE6Bh7MkbShIJtoqA==\"}",
                    CourseID = "263",
                    FeeComboID = new Guid("6d3bfd00-f45c-4b11-ba00-0d03209228f1"),
                    UserID = "1235604062",
                    TotalMoney = 60,
                    IsDiscount = 1,
                    UserTicketID = new Guid("00000000-0000-0000-0000-000000000000"),
                    IsYX = 0,
                    AppID = "241ea176-fce7-4bd7-a65f-a7978aac1cd2",
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
                    order.SourceType = obj.IsYX;
                    int SanBoxTest = 0;
                    if (sc.isSanBox == "1")
                    {
                        SanBoxTest = 1;//表示沙盒订单
                    }
                    order.CreateDate = DateTime.Now;
                    order.TotalMoney = obj.TotalMoney;
                    order.UserID = obj.UserID;
                    order.IsDiscount = obj.IsDiscount;
                    AccountController accc = new AccountController();

                    #region 创建订单
                    try
                    {
                        UserManagement usermanager = new UserManagement();
                        string sql = string.Format(@"INSERT INTO dbo.TB_Order ( ID ,OrderID ,TotalMoney ,CreateDate ,State ,UserID ,PayWay ,FeeComboID ,CourseID ,IsDiscount ,SourceType)
                                                              VALUES  ( '{0}' , '{1}' , '{2}' , '{3}' , '{4}' , '{5}' , '{6}' , '{7}' , '{8}' , '{9}' , '{10}'  )",
                                                                      order.ID, order.OrderID, order.TotalMoney, order.CreateDate, order.State, order.UserID, order.PayWay, order.FeeComboID, order.CourseID, order.IsDiscount, order.SourceType);
                        var j = OrderSql(obj.AppID, sql);
                        if (j > 0)
                        {
                            #region 分库订单写入队列
                            listredis.LPush("Order2BaseDBQueue", order.ToJson());
                            #endregion
                            return GetResult(accc.QueryCombo(obj.UserID), "支付成功");
                        }
                        else
                        {
                            return GetErrorResult("苹果支付插入订单失败");
                        }
                        //o.Insert<TB_Order>(order);
                        //
                    }
                    catch (SqlException ex)
                    {
                        switch (ex.Number)
                        {
                            case 2627:  //重复插入
                                Log4Net.LogHelper.Error(ex, "苹果支付插入订单重复,order=" + JsonHelper.EncodeJson(order));
                                return GetResult(accc.QueryCombo(obj.UserID), "支付成功");
                            //return KVpi(order.OrderID, SanBoxTest);
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
        /// 获取购买记录
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="AppID">应用ID</param>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse PostOrderList([FromBody] KingRequest request)//string UserID, string AppID)
        {
            PostOrderList submitData = JsonHelper.DecodeJson<PostOrderList>(request.Data);
            var app = bm.Select<TB_APPManagement>(submitData.AppID);
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

            var vip = bm.Search<TB_UserMember>("UserID='" + submitData.UserID + "' and CourseID in (" + cids + ")", " StartDate");
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
        [HttpPost]
        public ApiResponse PostPayCount([FromBody] KingRequest request)//string FeeComboID)
        {
            PostPayCount submitData = JsonHelper.DecodeJson<PostPayCount>(request.Data);
            if (string.IsNullOrEmpty(submitData.FeeComboID))
            {
                GetErrorResult("套餐ID不能为空");
            }

            var list = bm.Search<TB_Order>("FeeComboID='" + submitData.FeeComboID + "' and State='0001'");
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

            #region 获取用户权限
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

        //}
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
                                on.TeacherUserID = Convert.ToInt32(tea.TchID);
                                on.TeacherUserName = tea.TchName;
                            }
                            on.SchoolName = "";
                            on.GradeID = classInfo.GradeID;
                            on.GradeName = "";
                            on.ClassID = new Guid(classInfo.ClassID);
                            on.ClassName = classInfo.ClassName;
                        });
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
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "同步到财富分账出错失败");
            }
            return true;
        }

        [HttpPost]
        public void ResultNotifyPage()
        {
            ResultNotify resultNotify = new ResultNotify();
            resultNotify.ProcessNotify();
        }

        /// <summary>
        /// 处理提醒过程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void ProcessNotify()
        {
            HttpContext context = HttpContext.Current;
            //string key;//填写自己的key
            //string partner;//填写自己的Partner
            //string gateway;//填写支付网关="https://www.alipay.com/cooperate/gateway.do"
            //string AccountID;
            string orderid = context.Request.QueryString["out_trade_no"];

            //SetParameters(_alikeycode, out partner, out gateway, out AccountID, orderid);

            AliPay.Util.AliPay ap = new AliPay.Util.AliPay();
            string notifyid = context.Request.Form["notify_id"];

            Verify v = new Verify("notify_verify", _alipartnerid, notifyid);
            ap.TradeFinished += new AliPay.Util.AliPay.ProcessNotifyEventHandler(ap_TradeFinished);
            ap.ProcessNotify(context, _alipayway, _alikeycode, v);
        }

        /// <summary>
        /// 处理支付宝支付支付验证失败后重新通知的订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ap_TradeFinished(object sender, NotifyEventArgs e)
        {
            HttpContext context = HttpContext.Current;
            if (e.Trade_Status == "TRADE_SUCCESS")//"TRADE_FINISHED"
            {
                context.Response.Clear();
                context.Response.Write("success");     //返回给支付宝消息，成功

            }
        }

        public ArrayList NewQueryCombo(string AppID, string UserID, string HasModule)
        {
            UserManagement usermanager = new UserManagement();
            ArrayList list = new ArrayList();
            try
            {
                #region 获取同步课堂权限
                TBService.WebServicePatch wp = new TBService.WebServicePatch();
                var tlist = wp.LoadSyncClassBind(UserID);
                if (tlist != null)
                {
                    foreach (var item in tlist)
                    {
                        list.Add(new
                        {
                            UserID = UserID,
                            CourseID = item.BookID ?? "",
                            EndDate = (DateTime.Now.AddMonths(12).ToUniversalTime().Ticks - 621355968000000000) / 10000000,
                            Months = 1
                        });
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "QueryCombo获取同步课堂权限出错，UserID=" + UserID);
            }
            finally
            {
                string sql = string.Format(@"SELECT DISTINCT
                                                    CourseID ,
                                                    SUM(Months) 'Months',
                                                    ModuleID
                                            FROM    dbo.TB_UserMember
                                            WHERE   UserID = '{0}'
                                            GROUP BY CourseID,ModuleID
                                            ORDER BY CourseID", UserID);
                DataSet ds = SelectOrderSql(AppID, sql);
                List<memberinfo> mi = JsonHelper.DataSetToIList<memberinfo>(ds, 0);
                string strsql;

                if (string.IsNullOrEmpty(HasModule))
                {
                    strsql = string.Format(@"SELECT  [ID]
                                                      ,[UserID]
                                                      ,[TbOrderID]
                                                      ,[Months]
                                                      ,[StartDate]
                                                      ,[EndDate]
                                                      ,[CourseID]
                                                      ,[Status]
                                                      ,[CreateTime]
                                                      ,[SourceType]
                                                      ,[ModuleID]
                                                  FROM [TB_UserMember] WHERE UserID='" + UserID + "' and (ModuleID is null or ModuleID='' or ModuleID='1') order By EndDate desc");
                }
                else
                {
                    strsql = string.Format(@"SELECT  [ID]
                                                      ,[UserID]
                                                      ,[TbOrderID]
                                                      ,[Months]
                                                      ,[StartDate]
                                                      ,[EndDate]
                                                      ,[CourseID]
                                                      ,[Status]
                                                      ,[CreateTime]
                                                      ,[SourceType]
                                                      ,[ModuleID]
                                                  FROM [TB_UserMember] WHERE UserID='" + UserID + "' order By EndDate desc");
                }

                DataSet datads = SelectOrderSql(AppID, strsql);
                List<TB_UserMember> umList = JsonHelper.DataSetToIList<TB_UserMember>(datads, 0);
                //var umList = usermanager.Search<TB_UserMember>("UserID='" + UserID + "'  order By EndDate desc");
                if (umList != null)
                {
                    if (mi != null)
                    {
                        foreach (var em in mi)
                        {
                            TB_UserMember tu = umList.Where(i => i.CourseID == em.CourseID &&　i.ModuleID==em.ModuleID ).OrderBy(i => i.CreateTime).FirstOrDefault();
                            if (tu != null)
                            {
                                string ModuleID;
                                if (string.IsNullOrEmpty(tu.ModuleID))
                                {
                                    ModuleID = "1";
                                }
                                else
                                {
                                    ModuleID = tu.ModuleID;
                                }
                                if (tu.CreateTime != null)
                                    if (tu.Months != null)
                                        list.Add(new
                                        {
                                            UserID = UserID,
                                            CourseID = tu.CourseID,
                                            ModuleID=ModuleID,
                                            EndDate = (tu.CreateTime.Value.AddMonths((int)em.Months).ToUniversalTime().Ticks - 621355968000000000) / 10000000,
                                            Months = em.Months
                                        });
                            }
                        }

                    }
                }
            }

            //if (list.Count == 0) return null;
            return list;
            //return "";
        }

        private static Thread BgThread { set; get; }
    }

    #region
    //public class PaySucessInfo
    //{
    //    public string OrderID { get; set; }
    //    public string TicketID { get; set; }
    //}

    //public class TempClass
    //{
    //    public string OrderID { get; set; }
    //    public IList<PayWay> PayWayList { get; set; }
    //}

    //class TempClass3
    //{
    //    public int State { get; set; }
    //    public string Msg { get; set; }
    //}
    //public class PayWay
    //{
    //    public int PayWayID { get; set; }
    //    public string PayWayName { get; set; }
    //    public string Description { get; set; }
    //}

    //public class teaInfo
    //{
    //    public string UserID { get; set; }
    //    public string UserName { get; set; }
    //    public string SchoolName { get; set; }
    //    public string TrueName { get; set; }
    //}

    //public class riUser
    //{
    //    public int SchoolID { get; set; }
    //}

    //public class Tb_OrderNet
    //{
    //    /// <summary>
    //    /// 订单编号（支付系统的）
    //    /// </summary>
    //    public string OrderID { get; set; }

    //    /// <summary>
    //    /// 订单时间
    //    /// </summary>
    //    public DateTime? OrderDate { get; set; }

    //    /// <summary>
    //    /// 区域编号(选填)
    //    /// </summary>
    //    public int? AreaID { get; set; }

    //    /// <summary>
    //    /// 区域路径
    //    /// </summary>
    //    public string AreaPath { get; set; }

    //    /// <summary>
    //    /// 学校编号(选填)
    //    /// </summary>
    //    public int? SchoolID { get; set; }

    //    /// <summary>
    //    /// 学校名称
    //    /// </summary>
    //    public string SchoolName { get; set; }

    //    /// <summary>
    //    /// 年级编号
    //    /// </summary>
    //    public int? GradeID { get; set; }

    //    /// <summary>
    //    /// 年级名称
    //    /// </summary>
    //    public string GradeName { get; set; }

    //    /// <summary>
    //    /// 班级编号（GUID）
    //    /// </summary>
    //    public Guid? ClassID { get; set; }

    //    /// <summary>
    //    /// 班级名称
    //    /// </summary>
    //    public string ClassName { get; set; }

    //    /// <summary>
    //    /// 产品编号（使用“,”分开多个产品编号，不允许为空）
    //    /// </summary>
    //    public string ProductNO { get; set; }

    //    /// <summary>
    //    /// 老师编号
    //    /// </summary>
    //    public int? TeacherUserID { get; set; }

    //    /// <summary>
    //    /// 老师名称
    //    /// </summary>
    //    public string TeacherUserName { get; set; }

    //    /// <summary>
    //    /// 购买用户编号
    //    /// </summary>
    //    public int? BuyUserID { get; set; }

    //    /// <summary>
    //    /// 购买用户手机号
    //    /// </summary>
    //    public string BuyUserPhone { get; set; }

    //    /// <summary>
    //    /// 支付金额
    //    /// </summary>
    //    public decimal? PayAmount { get; set; }

    //    /// <summary>
    //    /// 支付方式
    //    /// </summary>
    //    public string PayType { get; set; }

    //    /// <summary>
    //    /// 来源
    //    /// </summary>
    //    public int? Channel { get; set; }

    //    /// <summary>
    //    /// 用户购买IP地址（如果区域编号存在或者购买用户手机号存在，可以不填）
    //    /// </summary>
    //    public string UserClientIP { get; set; }
    //}

    //public class XmlUtil
    //{
    //    #region 反序列化
    //    /// <summary>
    //    /// 反序列化
    //    /// </summary>
    //    /// <param name="type">类型</param>
    //    /// <param name="xml">XML字符串</param>
    //    /// <returns></returns>
    //    public static object Deserialize(Type type, string xml)
    //    {
    //        try
    //        {
    //            using (StringReader sr = new StringReader(xml))
    //            {
    //                XmlSerializer xmldes = new XmlSerializer(type);
    //                return xmldes.Deserialize(sr);
    //            }
    //        }
    //        catch (Exception e)
    //        {

    //            return null;
    //        }
    //    }
    //    /// <summary>
    //    /// 反序列化
    //    /// </summary>
    //    /// <param name="type"></param>
    //    /// <param name="xml"></param>
    //    /// <returns></returns>
    //    public static object Deserialize(Type type, Stream stream)
    //    {
    //        XmlSerializer xmldes = new XmlSerializer(type);
    //        return xmldes.Deserialize(stream);
    //    }
    //    #endregion

    //    #region 序列化
    //    /// <summary>
    //    /// 序列化
    //    /// </summary>
    //    /// <param name="type">类型</param>
    //    /// <param name="obj">对象</param>
    //    /// <returns></returns>
    //    public static string Serializer(Type type, object obj)
    //    {
    //        MemoryStream Stream = new MemoryStream();
    //        XmlSerializer xml = new XmlSerializer(type);
    //        try
    //        {
    //            //序列化对象
    //            xml.Serialize(Stream, obj);
    //        }
    //        catch (InvalidOperationException)
    //        {
    //            throw;
    //        }
    //        Stream.Position = 0;
    //        StreamReader sr = new StreamReader(Stream);
    //        string str = sr.ReadToEnd();

    //        sr.Dispose();
    //        Stream.Dispose();

    //        return str;
    //    }

    //    #endregion
    //}

    //public class Temp1
    //{
    //    public List<TBFeeCombo> List { get; set; }

    //    public int Status { get; set; }
    //}

    //public class TBFeeCombo
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public decimal Discount { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Guid ID { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public DateTime? CreateDate { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public decimal? FeePrice { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public int? State { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string ImageUrl { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string ModifyUser { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string AppID { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public int? Month { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public int? Type { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string AppleID { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public int? ComboType { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string FeeName { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string CreateUser { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public DateTime? ModifyDate { get; set; }

    //    public int PayCount { get; set; }
    //    public string Time { get; set; }
    //}

    //public class Stateclass
    //{
    //    public bool Success { get; set; }
    //    public string isSanBox { get; set; }
    //}

    //public class GetOrderList
    //{
    //    public int PayWayID { get; set; }
    //    public string PayWay { get; set; }
    //    public string CourseID { get; set; }
    //    public string FeeComboID { get; set; }
    //    public string UserID { get; set; }
    //    public decimal TotalMoney { get; set; }
    //    public int channel { get; set; }

    //    public int IsYX { get; set; }
    //}
    #endregion
    public class PostOrderID
    {
        public int PayWayID { get; set; }
        public string PayWay { get; set; }
        public string CourseID { get; set; }
        public string FeeComboID { get; set; }
        public string UserID { get; set; }
        public decimal TotalMoney { get; set; }
        public string AppID { get; set; }
        public int channel { get; set; }
        public string packageName { get; set; }

        public string ModuleID { get; set; }
    }

    public class PostComboNew
    {
        public string AppID { get; set; }
        public string SyetemID { get; set; }
        public string UserID { get; set; }

        public string ModuleID { get; set; }
    }

    public class PostOrderList
    {
        public string UserID { set; get; }
        public string AppID { set; get; }
    }

    public class PostPayCount
    {
        public string FeeComboID { get; set; }
    }

    public class PostCombo
    {
        public string AppID { get; set; }
        public string SyetemID { get; set; }
    }

    public class PostPayList
    {
        public string AppID { get; set; }
    }

    public class PostPaySucessInfo
    {
        public string OrderID { get; set; }
        public string TicketID { get; set; }
        public string AppID { get; set; }
        public int PayWayID { get; set; }
        public string packageName { get; set; }

        public string HasModule { get; set; }
    }
}