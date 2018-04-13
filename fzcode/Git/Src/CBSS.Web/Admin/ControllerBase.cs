using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using CBSS.Account.Contract;
using CBSS.Framework.Web;
using CBSS.Framework.Contract;
using CBSS.Core.Log;
using CBSS.Tbx.Contract;
using CBSS.Cfgmanager.IBLL;
using CBSS.Tbx.IBLL;
using CBSS.Account.IBLL;
using CBSS.IBS.IBLL;
using CBSS.Account.Contract.ViewModel;
using CBSS.Core.Utility;
using CBSS.UserOrder.IBLL;
using CBSS.ResourcesManager.IBLL;

namespace CBSS.Web
{
    public abstract class ControllerBase : CBSS.Framework.Web.ControllerBase
    {

        protected Jurisdiction action = new Jurisdiction();
        public virtual IAccountService AccountService
        {
            get
            {
                return ServiceContext.Current.AccountService;
            }
        }

        public virtual ITbxService TbxService
        {
            get
            {
                return ServiceContext.Current.TbxService;
            }
        }

        public virtual ICfgmanagerService CfgmanagerService
        {
            get
            {
                return ServiceContext.Current.CfgmanagerService;
            }
        }

        public virtual IIBSService IBSService
        {
            get
            {
                return ServiceContext.Current.IBSService;
            }
        }
        public virtual IUserOrderService UserOrderService
        {
            get
            {
                return ServiceContext.Current.UserOrderService;
            }
        }

        public virtual IResourcesService ResourcesService
        {
            get
            {
                return ServiceContext.Current.ResourcesService;
            }
        }



        protected override void LogException(Exception exception,
            WebExceptionContext exceptionContext = null)
        {
            base.LogException(exception);

            var message = new
            {
                exception = exception.Message,
                exceptionContext = exceptionContext,
            };

            Log4NetHelper.Error(LoggerType.WebExceptionLog, message, exception);
        }

        public IDictionary<string, object> CurrentActionParameters { get; set; }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var vcontext = filterContext.HttpContext;
            //Log4NetHelper.Info(LoggerType.WebExceptionLog, vcontext.ToJson());
            if (Session["LoginInfo"] != null)
            {
                //Log4NetHelper.Info(LoggerType.WebExceptionLog, "session不为null");
                var logininfo = Session["LoginInfo"] as CBSS.Account.Contract.Sys_LoginInfo;
                List<v_action> actionlist = logininfo.BusinessPermissionString.ToList();
                string CurrentController = RouteData.Route.GetRouteData(this.HttpContext).Values["controller"].ToString();//当前Controller
                if (actionlist != null && actionlist.Count > 0)
                {
                    #region 获取权限
                    foreach (v_action item in actionlist)
                    {
                        if (item.actionurl == CurrentController)
                        {
                            if (item.actionname.Contains("View"))
                                action.View = true;
                            else if (item.actionname.Contains("Edit"))
                                action.Edit = true;
                            else if (item.actionname.Contains("Add"))
                                action.Add = true;
                            else if (item.actionname.Contains("Del"))
                                action.Del = true;
                            else if (item.actionname.Contains("Export"))
                                action.Export = true;
                            else if (item.actionname.Contains("Pullblack"))
                                action.Pullblack = true;
                            else if (item.actionname.Contains("Locking"))
                                action.Locking = true;
                            else if (item.actionname.Contains("Move"))
                                action.Move = true;
                            else if (item.actionname.Contains("Detailed"))
                                action.Detailed = true;
                            else if (item.actionname.Contains("Blacklist"))
                                action.Blacklist = true;
                            else if (item.actionname.Contains("Kont"))
                                action.Kont = true;
                            else if (item.actionname.Contains("Revoked"))
                                action.Revoked = true;
                            else if (item.actionname.Contains("Employee"))
                                action.Employee = true;
                            else if (item.actionname.Contains("Dept"))
                                action.Dept = true;
                            else if (item.actionname.Contains("Agent"))
                                action.Agent = true;
                            else if (item.actionname.Contains("Merge"))
                                action.Merge = true;
                            else if (item.actionname.Contains("Down"))
                                action.Down = true;
                            else if (item.actionname.Contains("Model_ImgLibrary"))
                                action.Model_ImgLibrary = true;
                            else if (item.actionname.Contains("Good_GoodPrice"))
                                action.Good_GoodPrice = true;
                            else if (item.actionname.Contains("Good_GoodModuleItem"))
                                action.Good_GoodModuleItem = true;
                            else if (item.actionname.Contains("App_AppVersion"))
                                action.App_AppVersion = true;
                            else if (item.actionname.Contains("App_AppModule"))
                                action.App_AppModule = true;
                            else if (item.actionname.Contains("App_Good"))
                                action.App_Good = true;
                            else if (item.actionname.Contains("App_AppSkinVersion"))
                                action.App_AppSkinVersion = true;
                            else if (item.actionname.Contains("MarketClassify_SyncModClassify"))
                                action.MarketClassify_SyncModClassify = true;
                            else if (item.actionname.Contains("MarketBook_Catalogs"))
                                action.MarketBook_Catalogs = true;
                            else if (item.actionname.Contains("MarketBook_SyncModBook"))
                                action.MarketBook_SyncModBook = true;
                            else if (item.actionname.Contains("ApiFunction_ApiDocument"))
                                action.ApiFunction_ApiDocument = true;
                            else if (item.actionname.Contains("SingleTable_FieldList"))
                                action.SingleTable_FieldList = true;
                            else if (item.actionname.Contains("Log4net"))
                                action.Log4net = true;
                        }
                    }
                    #endregion

                    //if (!action.View && CurrentController!= "Index")
                    //{
                    //    UrlHelper url = new UrlHelper(filterContext.RequestContext);
                    //    string result = string.Format("<script type='text/javascript'> window.top.location = '" + url.Content("~/Account/Auth/Login") + "';</script>");
                    //    filterContext.Result = new ContentResult() { Content = result };
                    //    return;
                    //}
                }
                filterContext.HttpContext.Session.Timeout = 20;
            }
            else
            {
                //Log4NetHelper.Info(LoggerType.WebExceptionLog, "session为null");
                //Authority 权限session
                UrlHelper url = new UrlHelper(filterContext.RequestContext);
                string result = string.Format("<script type='text/javascript'> window.top.location = '" + url.Content("~/Account/Auth/Login") + "';</script>");
                filterContext.Result = new ContentResult() { Content = result };
                return;
            }
        }

        /// <summary>
        /// 设置页面的索引
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public int setpageindex(int index, int size)
        {
            if (index == 0)
                index = index / size;
            else
                index = index / size + 1;
            return index;
        }
        /// <summary>
        /// 根据actionname判断权限是否存在
        /// </summary>
        /// <param name="actionname"></param>
        /// <returns></returns>
        public bool CheckActionName(string actionname)
        {
            try
            {
                if (Session["LoginInfo"] != null)
                {
                    var logininfo = Session["LoginInfo"] as CBSS.Account.Contract.Sys_LoginInfo;
                    List<v_action> actionlist = logininfo.BusinessPermissionString.ToList();
                    if (actionlist != null && actionlist.Count > 0)
                    {
                        var list = actionlist.Where<v_action>(a => a.actionname.Contains(actionname));
                        if (list != null && list.Count() > 0)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    Redirect("~/Account/Auth/Login");
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }



    /// <summary>
    /// 操作权限
    /// </summary>
    public class Jurisdiction
    {
        private bool _view = false;
        public bool View
        {
            get { return _view; }
            set { _view = value; }
        }
        private bool _add = false;
        public bool Add
        {
            get { return _add; }
            set { _add = value; }
        }
        private bool _edit = false;
        public bool Edit
        {
            get { return _edit; }
            set { _edit = value; }
        }
        private bool _del = false;
        public bool Del
        {
            get { return _del; }
            set { _del = value; }
        }
        private bool _export = false;
        /// <summary>
        /// 导出
        /// </summary>
        public bool Export
        {
            get { return _export; }
            set { _export = value; }
        }
        private bool _pullblack = false;
        /// <summary>
        /// 拉黑
        /// </summary>
        public bool Pullblack
        {
            get { return _pullblack; }
            set { _pullblack = value; }
        }
        private bool _locking = false;
        /// <summary>
        /// 锁定
        /// </summary>
        public bool Locking
        {
            get { return _locking; }
            set { _locking = value; }
        }
        private bool _move = false;
        /// <summary>
        /// 移动
        /// </summary>
        public bool Move
        {
            get { return _move; }
            set { _move = value; }
        }
        private bool _Merge = false;
        /// <summary>
        /// 合并 
        /// </summary>
        public bool Merge
        {
            get { return _Merge; }
            set { _Merge = value; }
        }
        private bool _detailed = false;
        /// <summary>
        /// 详细
        /// </summary>
        public bool Detailed
        {
            get { return _detailed; }
            set { _detailed = value; }
        }
        private bool _blacklist = false;
        /// <summary>
        /// 查看黑名单
        /// </summary>
        public bool Blacklist
        {
            get { return _blacklist; }
            set { _blacklist = value; }
        }
        private bool _kont = false;
        /// <summary>
        /// 结算
        /// </summary>
        public bool Kont
        {
            get { return _kont; }
            set { _kont = value; }
        }
        private bool _revoked = false;
        /// <summary>
        /// 结算撤销
        /// </summary>
        public bool Revoked
        {
            get { return _revoked; }
            set { _revoked = value; }
        }
        private bool _employee = false;

        public bool Employee
        {
            get { return _employee; }
            set { _employee = value; }
        }
        private bool _dept = false;

        public bool Dept
        {
            get { return _dept; }
            set { _dept = value; }
        }
        public bool _agent = false;

        public bool Agent
        {
            get { return _agent; }
            set { _agent = value; }
        }
        /// <summary>
        /// 下载权限
        /// </summary>
        public bool Down { get; set; }
        /// <summary>
        /// 模型图片库
        /// </summary>
        public bool Model_ImgLibrary { get; set; }

        /// <summary>
        /// 策略价格管理
        /// </summary>
        public bool Good_GoodPrice { get; set; }
        /// <summary>
        /// 策略配置模块
        /// </summary>
        public bool Good_GoodModuleItem { get; set; }
        /// <summary>
        /// 版本管理
        /// </summary>
        public bool App_AppVersion { get; set; }
        /// <summary>
        /// 配置应用模块
        /// </summary>
        public bool App_AppModule { get; set; }
        /// <summary>
        /// 配置商品策略
        /// </summary>
        public bool App_Good { get; set; }
        /// <summary>
        /// 皮肤管理
        /// </summary>
        public bool App_AppSkinVersion { get; set; }
        /// <summary>
        /// 同步MOD分类 
        /// </summary>
        public bool MarketClassify_SyncModClassify { get; set; }
        /// <summary>
        /// 同步MOD书籍
        /// </summary>
        public bool MarketBook_SyncModBook { get; set; }
        /// <summary>
        /// 书籍目录
        /// </summary>
        public bool MarketBook_Catalogs { get; set; }
        /// <summary>
        /// 接口文档
        /// </summary>
        public bool ApiFunction_ApiDocument { get; set; }
        /// <summary>
        /// 字段列表
        /// </summary>
        public bool SingleTable_FieldList { get; set; }
        /// <summary>
        /// 字段列表
        /// </summary>
        public bool Log4net { get; set; }
    }
}
