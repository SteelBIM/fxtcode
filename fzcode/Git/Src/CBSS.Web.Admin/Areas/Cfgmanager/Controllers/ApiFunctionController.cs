using CBSS.Account.Contract.ViewModel;
using CBSS.Cfgmanager.BLL;
using CBSS.Cfgmanager.Contract;
using CBSS.Cfgmanager.Contract.DataModel;
using CBSS.Cfgmanager.Contract.ViewModel;
using CBSS.Core.Log;
using CBSS.Core.Utility;
using CBSS.Framework.Contract;
using CBSS.Framework.Contract.Enums;
using CBSS.Web.Admin.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBSS.Web.Admin.Areas.Cfgmanager.Controllers
{
    public class ApiFunctionController : Controller
    {
        //
        // GET: /ApiFunction/

        public ActionResult Index()
        {
            ViewData["SystemCode"] = new SelectList(WebControl.GetSelectList(typeof(SystemCodeEnum)), "Value", "Text");
            GetAppOperaMenu();
            return View();
        }

        public JsonResult GetApiFunctionPage(int pagesize, int pageindex, string ApiFunctionName, string SystemCode)
        {
            ApiFunctionRequest request = new ApiFunctionRequest();
            request.PageIndex = setpageindex(pageindex, pagesize);
            request.PageSize = pagesize;
            request.ApiFunctionName = ApiFunctionName;
            request.SystemCode = Convert.ToInt32(string.IsNullOrEmpty(SystemCode) ? "0" : SystemCode);
            ViewData["SystemCode"] = new SelectList(WebControl.GetSelectList(typeof(SystemCodeEnum)), "Value", "Text", request.SystemCode);
            int total = 0;
            var list = new CfgmanagerService().GetAllSys_ApiFunction(out total, request);
            return Json(new { total = total, rows = list });
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
        public void GetAppOperaMenu()
        {
            try
            {
                if (Session["LoginInfo"] != null)
                {
                    var logininfo = Session["LoginInfo"] as CBSS.Account.Contract.Sys_LoginInfo;
                    List<v_action> actionlist = logininfo.BusinessPermissionString.ToList();
                    if (actionlist != null && actionlist.Count > 0)
                    {
                        foreach (var item in actionlist)
                        {
                            if (item.actionurl == "ApiFunction")
                            {
                                if (item.actionname.Contains("Edit"))
                                    ViewBag.Edit = true;
                                else if (item.actionname.Contains("Add"))
                                    ViewBag.Add = true;
                                else if (item.actionname.Contains("Del"))
                                    ViewBag.Del = true;
                                else if (item.actionname.Contains("ApiFunction_ApiDocument"))
                                    ViewBag.ApiFunction_ApiDocument = true;
                            }
                        }
                    }
                }
                else
                {
                    Response.Redirect("~/Account/Auth/Login");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Create()
        {
            List<Sys_ApiFunctionParam> apiFunctionParams = new List<Sys_ApiFunctionParam>();

            Sys_ApiFunctionParam sys_ApiFunctionParam = new Sys_ApiFunctionParam();
            sys_ApiFunctionParam.ParameterFields = "Key";
            sys_ApiFunctionParam.ParameterExplain = "加密密钥";
            sys_ApiFunctionParam.ParameterType = "2";
            sys_ApiFunctionParam.IsAllowNull = 1;
            sys_ApiFunctionParam.Type = 0;
            apiFunctionParams.Add(sys_ApiFunctionParam);

            sys_ApiFunctionParam = new Sys_ApiFunctionParam();
            sys_ApiFunctionParam.ParameterFields = "Info";
            sys_ApiFunctionParam.ParameterExplain = "输入参数字符串";
            sys_ApiFunctionParam.ParameterType = "5";
            sys_ApiFunctionParam.IsAllowNull = 1;
            sys_ApiFunctionParam.Type = 0;
            apiFunctionParams.Add(sys_ApiFunctionParam);

            sys_ApiFunctionParam = new Sys_ApiFunctionParam();
            sys_ApiFunctionParam.ParameterFields = "FunName";
            sys_ApiFunctionParam.ParameterExplain = "接口名";
            sys_ApiFunctionParam.ParameterType = "2";
            sys_ApiFunctionParam.IsAllowNull = 1;
            sys_ApiFunctionParam.Type = 0;
            apiFunctionParams.Add(sys_ApiFunctionParam);

            sys_ApiFunctionParam = new Sys_ApiFunctionParam();
            sys_ApiFunctionParam.ParameterFields = "FunWay";
            sys_ApiFunctionParam.ParameterExplain = "返回方式：0正常请求，1加密，2压缩，3加密并压缩";
            sys_ApiFunctionParam.ParameterType = "1";
            sys_ApiFunctionParam.IsAllowNull = 1;
            sys_ApiFunctionParam.Type = 0;
            apiFunctionParams.Add(sys_ApiFunctionParam);

            sys_ApiFunctionParam = new Sys_ApiFunctionParam();
            sys_ApiFunctionParam.ParameterFields = "RequestID";
            sys_ApiFunctionParam.ParameterExplain = "请求ID";
            sys_ApiFunctionParam.ParameterType = "2";
            sys_ApiFunctionParam.IsAllowNull = 1;
            sys_ApiFunctionParam.Type = 1;
            apiFunctionParams.Add(sys_ApiFunctionParam);

            sys_ApiFunctionParam = new Sys_ApiFunctionParam();
            sys_ApiFunctionParam.ParameterFields = "Success";
            sys_ApiFunctionParam.ParameterExplain = "操作是否成功";
            sys_ApiFunctionParam.ParameterType = "3";
            sys_ApiFunctionParam.IsAllowNull = 1;
            sys_ApiFunctionParam.Type = 1;
            apiFunctionParams.Add(sys_ApiFunctionParam);

            sys_ApiFunctionParam = new Sys_ApiFunctionParam();
            sys_ApiFunctionParam.ParameterFields = "Data";
            sys_ApiFunctionParam.ParameterExplain = "业务数据";
            sys_ApiFunctionParam.ParameterType = "2";
            sys_ApiFunctionParam.IsAllowNull = 1;
            sys_ApiFunctionParam.Type = 1;
            apiFunctionParams.Add(sys_ApiFunctionParam);

            sys_ApiFunctionParam = new Sys_ApiFunctionParam();
            sys_ApiFunctionParam.ParameterFields = "ErrorCode";
            sys_ApiFunctionParam.ParameterExplain = "错误码";
            sys_ApiFunctionParam.ParameterType = "1";
            sys_ApiFunctionParam.IsAllowNull = 1;
            sys_ApiFunctionParam.Type = 1;
            apiFunctionParams.Add(sys_ApiFunctionParam);

            sys_ApiFunctionParam = new Sys_ApiFunctionParam();
            sys_ApiFunctionParam.ParameterFields = "ErrorMsg";
            sys_ApiFunctionParam.ParameterExplain = "错误信息";
            sys_ApiFunctionParam.ParameterType = "2";
            sys_ApiFunctionParam.IsAllowNull = 1;
            sys_ApiFunctionParam.Type = 1;
            apiFunctionParams.Add(sys_ApiFunctionParam);

            sys_ApiFunctionParam = new Sys_ApiFunctionParam();
            sys_ApiFunctionParam.ParameterFields = "SystemTime";
            sys_ApiFunctionParam.ParameterExplain = "接口返回时间";
            sys_ApiFunctionParam.ParameterType = "2";
            sys_ApiFunctionParam.IsAllowNull = 1;
            sys_ApiFunctionParam.Type = 1;
            apiFunctionParams.Add(sys_ApiFunctionParam);

            sys_ApiFunctionParam = new Sys_ApiFunctionParam();
            sys_ApiFunctionParam.ApiFunctionParamParentID = "Info";
            sys_ApiFunctionParam.ParameterFields = "PKey";
            sys_ApiFunctionParam.ParameterExplain = "解码公钥";
            sys_ApiFunctionParam.ParameterType = "2";
            sys_ApiFunctionParam.IsAllowNull = 1;
            sys_ApiFunctionParam.Type = 0;
            apiFunctionParams.Add(sys_ApiFunctionParam);

            sys_ApiFunctionParam = new Sys_ApiFunctionParam();
            sys_ApiFunctionParam.ApiFunctionParamParentID = "Info";
            sys_ApiFunctionParam.ParameterFields = "RTime";
            sys_ApiFunctionParam.ParameterExplain = "请求时间";
            sys_ApiFunctionParam.ParameterType = "2";
            sys_ApiFunctionParam.IsAllowNull = 1;
            sys_ApiFunctionParam.Type = 0;
            apiFunctionParams.Add(sys_ApiFunctionParam);

            ViewData["apiFunctionInfo"] = apiFunctionParams;

            ViewData["selApiFunctionWay"] = new SelectList(WebControl.GetSelectList(typeof(ApiFuncWayEnum)), "Value", "Text");
            ViewData["selSystemCode"] = new SelectList(WebControl.GetSelectList(typeof(SystemCodeEnum)), "Value", "Text");
            return View();
        }

        /// <summary>
        /// 提交接口信息
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Add(string jsonData)
        {
            try
            {
                if (jsonData != "[]")
                {
                    int ApiFunctionID = Convert.ToInt32(Request["ApiFunctionID"]);
                    string txtApiFunctionName = Request["txtApiFunctionName"];
                    string txtApiFunctionExplain = Request["txtApiFunctionExplain"];
                    string txtApiFunctionUrl = Request["txtApiFunctionUrl"];
                    int selApiFunctionWay = Convert.ToInt32(Request["selApiFunctionWay"]);
                    int selSystemCode = Convert.ToInt32(Request["selSystemCode"]);
                    string txtApiFunctionRemark = Request["txtApiFunctionRemark"];
                    Sys_ApiFunction apiFun = new Sys_ApiFunction()
                    {
                        ApiFunctionID = ApiFunctionID,
                        ApiFunctionName = txtApiFunctionName,
                        ApiFunctionExplain = txtApiFunctionExplain,
                        ApiFunctionUrl = txtApiFunctionUrl,
                        ApiFunctionWay = selApiFunctionWay,
                        ApiFunctionRemark = txtApiFunctionRemark,
                        SystemCode = selSystemCode,
                        CreateDate = DateTime.Now
                    };
                    var listParams = JsonConvert.DeserializeObject<List<Sys_ApiFunctionParam>>(jsonData);
                    if (listParams != null)
                    {
                        bool flag = new CfgmanagerService().SaveApiFunction(apiFun, listParams);
                        return Json(flag);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.WebExceptionLog, ex.Message, ex);
                return Json(false);
            }
            return Json(false);
        }

        public ActionResult Edit(int id)
        {
            var ApiFunctionModel = new CfgmanagerService().GetApiFunction(id);
            if (ApiFunctionModel != null)
            {
                var List = new CfgmanagerService().GetApiFunctionParam(id);
                ViewData["ApiFunctionParam"] = List;
            }
            ViewData["selApiFunctionWay"] = new SelectList(WebControl.GetSelectList(typeof(ApiFuncWayEnum)), "Value", "Text", ApiFunctionModel.ApiFunctionWay);
            ViewData["selSystemCode"] = new SelectList(WebControl.GetSelectList(typeof(SystemCodeEnum)), "Value", "Text", ApiFunctionModel.SystemCode);
            return View("Create", ApiFunctionModel);
        }

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            if (ids == null)
            {
                return RedirectToAction("Index");
            }
            bool flag = new CfgmanagerService().DeleteApiFunction(ids);
            return Json(flag);
        }
        [ValidateInput(false)]
        /// <summary>
        /// 生成接口文档
        /// </summary>
        /// <returns></returns>
        public ActionResult ApiDocument()
        {
            string ApiFunctionName = Request["ApiFunctionName"];
            int SystemCode = Convert.ToInt32(Request["SystemCode"] == "" ? "0" : Request["SystemCode"].ToString());
            List<Sys_ApiFunction> ApiFunIDList = new CfgmanagerService().GetApiFunctionList(ApiFunctionName, SystemCode);
            List<int> ApiFunIDStr = new List<int>();
            foreach (Sys_ApiFunction item in ApiFunIDList)
            {
                ApiFunIDStr.Add(item.ApiFunctionID);
            }
            string ids = string.Join(",", ApiFunIDStr);
            if (!string.IsNullOrEmpty(ids))
            {
                //string id = Request.RequestContext.RouteData.Values["id"].ToString();
                List<Sys_ApiFunction> apiFunList = new CfgmanagerService().GetApiFunctionList(ids);
                List<Sys_ApiFunctionParam> apiFunParamList = new CfgmanagerService().GetApiFunctionParamList(ids);
                List<Sys_ApiFunctionParam> RequestParamList = null; //请求参数 
                List<Sys_ApiFunctionParam> ResponseParamList = null; //返回参数 
                List<ApiDocModel> list = new List<ApiDocModel>();
                //IDictionary<int, string> WayEnums = EnumHelper.GetItemValueList<ApiFuncEnums.ApiFuncWayEnum>();
                //IDictionary<int, string> SystemEnums = EnumHelper.GetItemValueList<SystemEnum>();
                if (apiFunList != null && apiFunList.Count > 0)
                {
                    int index = 0;
                    foreach (var item in apiFunList)
                    {
                        ApiDocModel model = new ApiDocModel();
                        model.SystemCode = item.SystemCode;
                        model.SystemName = EnumHelper.GetEnumDesc<SystemCodeEnum>(Convert.ToInt32(item.SystemCode));
                        model.name = item.ApiFunctionExplain; //展示名称
                        model.ApiFunctionName = item.ApiFunctionName; //方法名称
                        model.url = item.ApiFunctionUrl;
                        model.hover = index == 0 ? true : false;
                        model.format = "json";
                        model.method = "Post";
                        model.apiway = EnumHelper.GetEnumDesc<ApiFuncWayEnum>(item.ApiFunctionWay);
                        model.ApiFunctionRemark = item.ApiFunctionRemark;
                        //请求参数 
                        RequestParamList = apiFunParamList.Where(a => a.ApiFunctionID == item.ApiFunctionID && a.Type == 0).ToList();
                        if (RequestParamList != null && RequestParamList.Count > 0)
                        {
                            string reqJson = "{";
                            List<InParamsModel> ListInParamsModel = new List<InParamsModel>();
                            var notChildList = RequestParamList.Where(a => string.IsNullOrEmpty(a.ApiFunctionParamParentID));
                            foreach (var m in notChildList)
                            {
                                reqJson += "'" + m.ParameterFields + "':";// "'" + m.ParameterValue + "',";
                                if (RequestParamList.Where(a => a.ApiFunctionParamParentID == m.ParameterFields).Count() > 0)//子集
                                {
                                    reqJson += m.ParameterType == "4" ? "[{" : "{";
                                    foreach (var n in RequestParamList.Where(a => a.ApiFunctionParamParentID == m.ParameterFields))
                                    {
                                        reqJson += "'" + n.ParameterFields + "':";// "'" + m.ParameterValue + "',";
                                        if (RequestParamList.Where(a => a.ApiFunctionParamParentID == n.ParameterFields).Count() > 0)//子集
                                        {
                                            reqJson += n.ParameterType == "4" ? "[{" : "{";
                                            foreach (var o in RequestParamList.Where(a => a.ApiFunctionParamParentID == n.ParameterFields))
                                            {
                                                reqJson += "'" + o.ParameterFields + "':" + "'" + o.ParameterValue + "',";
                                            }
                                            reqJson = reqJson.TrimEnd(',');
                                            reqJson += n.ParameterType == "4" ? "}]," : "},";
                                        }
                                        else
                                        {
                                            reqJson += "'" + n.ParameterValue + "',";
                                        }
                                    }
                                    reqJson = reqJson.TrimEnd(',');
                                    reqJson += m.ParameterType == "4" ? "}]," : "},";
                                }
                                else
                                {
                                    reqJson += "'" + m.ParameterValue + "',";
                                }
                                //ListInParamsModel.Add(new InParamsModel()
                                //{
                                //    name = m.ParameterFields,
                                //    value = m.ParameterType == "1" ? "int" : m.ParameterType == "2" ? "string" : m.ParameterType == "3" ? "bool" : "list",
                                //    desc = m.ParameterExplain,
                                //    must = m.IsAllowNull == 1 ? true : false
                                //});
                            }
                            reqJson = reqJson.TrimEnd(',');
                            reqJson += "}";
                            reqJson = reqJson.Replace('\'', '"');
                            model.demo = JsonConvertHelper.ToObject<object>(reqJson);
                            //请求参数说明
                            {
                                notChildList = notChildList.OrderBy(a => a.ParameterType);
                                foreach (var m in notChildList)
                                {
                                    if (RequestParamList.Where(a => a.ApiFunctionParamParentID == m.ParameterFields).Count() > 0)//子集
                                    {
                                        InParamsModel myModel = new InParamsModel()
                                        {
                                            name = m.ParameterFields,
                                            value = "json",
                                            desc = m.ParameterExplain,
                                            must = m.IsAllowNull == 1 ? true : false
                                        };
                                        if (!ListInParamsModel.Contains<InParamsModel>(myModel))
                                        {
                                            ListInParamsModel.Add(myModel);
                                        }
                                        foreach (var n in RequestParamList.Where(a => a.ApiFunctionParamParentID == m.ParameterFields))
                                        {
                                            if (RequestParamList.Where(a => a.ApiFunctionParamParentID == n.ParameterFields).Count() > 0)//子集
                                            {
                                                var myModel2 = new InParamsModel()
                                                {
                                                    name = n.ParameterFields,
                                                    value = "json",
                                                    desc = n.ParameterExplain,
                                                    must = n.IsAllowNull == 1 ? true : false
                                                };
                                                if (!ListInParamsModel.Contains<InParamsModel>(myModel2))
                                                {
                                                    ListInParamsModel.Add(myModel2);
                                                }
                                                foreach (var o in RequestParamList.Where(a => a.ApiFunctionParamParentID == n.ParameterFields))
                                                {
                                                    var oModel = new InParamsModel()
                                                    {
                                                        name = o.ParameterFields,
                                                        value = o.ParameterType == "1" ? "int" : o.ParameterType == "2" ? "string" : o.ParameterType == "3" ? "bool" : o.ParameterType == "4" ? "list" : o.ParameterType == "5" ? "class" : o.ParameterType == "6" ? "double" : o.ParameterType == "7" ? "strJson" : "string",
                                                        desc = o.ParameterExplain,
                                                        must = o.IsAllowNull == 1 ? true : false
                                                    };
                                                    if (!ListInParamsModel.Contains<InParamsModel>(oModel))
                                                    {
                                                        ListInParamsModel.Add(oModel);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var nModel = new InParamsModel()
                                                {
                                                    name = n.ParameterFields,
                                                    value = n.ParameterType == "1" ? "int" : n.ParameterType == "2" ? "string" : n.ParameterType == "3" ? "bool" : n.ParameterType == "4" ? "list" : n.ParameterType == "5" ? "class" : n.ParameterType == "6" ? "double" : n.ParameterType == "7" ? "strJson" : "string",
                                                    desc = n.ParameterExplain,
                                                    must = n.IsAllowNull == 1 ? true : false
                                                };
                                                if (!ListInParamsModel.Contains<InParamsModel>(nModel))
                                                {
                                                    ListInParamsModel.Add(nModel);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var mModel = new InParamsModel()
                                        {
                                            name = m.ParameterFields,
                                            value = m.ParameterType == "1" ? "int" : m.ParameterType == "2" ? "string" : m.ParameterType == "3" ? "bool" : m.ParameterType == "4" ? "list" : m.ParameterType == "5" ? "class" : m.ParameterType == "6" ? "double" : m.ParameterType == "7" ? "strJson" : "string",
                                            desc = m.ParameterExplain,
                                            must = m.IsAllowNull == 1 ? true : false
                                        };
                                        if (!ListInParamsModel.Contains<InParamsModel>(mModel))
                                        {
                                            ListInParamsModel.Add(mModel);
                                        }
                                    }
                                }
                            }
                            model.inparams = ListInParamsModel;
                        }
                        //返回参数 
                        ResponseParamList = apiFunParamList.Where(a => a.ApiFunctionID == item.ApiFunctionID && a.Type == 1).ToList();
                        if (ResponseParamList != null && ResponseParamList.Count > 0)
                        {
                            string reqJson = "{";
                            List<InParamsModel> ListInParamsModel = new List<InParamsModel>();
                            var notChildList = ResponseParamList.Where(a => string.IsNullOrEmpty(a.ApiFunctionParamParentID));
                            foreach (var m in notChildList)
                            {
                                reqJson += "'" + m.ParameterFields + "':";// "'" + m.ParameterValue + "',";
                                if (ResponseParamList.Where(a => a.ApiFunctionParamParentID == m.ParameterFields).Count() > 0)//子集
                                {
                                    reqJson += m.ParameterType == "4" ? "[{" : "{";
                                    foreach (var n in ResponseParamList.Where(a => a.ApiFunctionParamParentID == m.ParameterFields))
                                    {
                                        reqJson += "'" + n.ParameterFields + "':";// "'" + m.ParameterValue + "',";
                                        if (ResponseParamList.Where(a => a.ApiFunctionParamParentID == n.ParameterFields).Count() > 0)//子集
                                        {
                                            reqJson += n.ParameterType == "4" ? "[{" : "{";
                                            foreach (var o in ResponseParamList.Where(a => a.ApiFunctionParamParentID == n.ParameterFields))
                                            {
                                                reqJson += "'" + o.ParameterFields + "':" + "'" + o.ParameterValue + "',";
                                            }
                                            reqJson = reqJson.TrimEnd(',');
                                            reqJson += n.ParameterType == "4" ? "}]," : "},";
                                        }
                                        else
                                        {
                                            reqJson += "'" + n.ParameterValue + "',";
                                        }
                                    }
                                    reqJson = reqJson.TrimEnd(',');
                                    reqJson += m.ParameterType == "4" ? "}]," : "},";
                                }
                                else
                                {
                                    reqJson += "'" + m.ParameterValue + "',";
                                }
                                //ListInParamsModel.Add(new InParamsModel()
                                //{
                                //    name = m.ParameterFields,
                                //    value = m.ParameterType == "1" ? "int" : m.ParameterType == "2" ? "string" : m.ParameterType == "3" ? "bool" : "list",
                                //    desc = m.ParameterExplain,
                                //    must = m.IsAllowNull == 1 ? true : false
                                //});
                            }
                            reqJson = reqJson.TrimEnd(',');
                            reqJson += "}";
                            //reqJson = reqJson.Replace('\'','"');
                            model.output = JsonConvertHelper.ToObject<object>(reqJson);
                            //返回参数说明
                            {
                                notChildList = notChildList.OrderBy(a => a.ParameterType);
                                foreach (var m in notChildList)
                                {
                                    if (ResponseParamList.Where(a => a.ApiFunctionParamParentID == m.ParameterFields).Count() > 0)//子集
                                    {
                                        InParamsModel myModel = new InParamsModel()
                                        {
                                            name = m.ParameterFields,
                                            value = "json",
                                            desc = m.ParameterExplain,
                                            must = m.IsAllowNull == 1 ? true : false
                                        };
                                        if (!ListInParamsModel.Contains<InParamsModel>(myModel))
                                        {
                                            ListInParamsModel.Add(myModel);
                                        }
                                        foreach (var n in ResponseParamList.Where(a => a.ApiFunctionParamParentID == m.ParameterFields))
                                        {
                                            if (ResponseParamList.Where(a => a.ApiFunctionParamParentID == n.ParameterFields).Count() > 0)//子集
                                            {
                                                var myModel2 = new InParamsModel()
                                                {
                                                    name = n.ParameterFields,
                                                    value = "json",
                                                    desc = n.ParameterExplain,
                                                    must = n.IsAllowNull == 1 ? true : false
                                                };
                                                if (!ListInParamsModel.Contains<InParamsModel>(myModel2))
                                                {
                                                    ListInParamsModel.Add(myModel2);
                                                }
                                                foreach (var o in ResponseParamList.Where(a => a.ApiFunctionParamParentID == n.ParameterFields))
                                                {
                                                    var oModel = new InParamsModel()
                                                    {
                                                        name = o.ParameterFields,
                                                        value = o.ParameterType == "1" ? "int" : o.ParameterType == "2" ? "string" : o.ParameterType == "3" ? "bool" : o.ParameterType == "4" ? "list" : o.ParameterType == "5" ? "class" : o.ParameterType == "6" ? "double" : o.ParameterType == "7" ? "strJson" : "string",
                                                        desc = o.ParameterExplain,
                                                        must = o.IsAllowNull == 1 ? true : false
                                                    };
                                                    if (!ListInParamsModel.Contains<InParamsModel>(oModel))
                                                    {
                                                        ListInParamsModel.Add(oModel);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var nModel = new InParamsModel()
                                                {
                                                    name = n.ParameterFields,
                                                    value = n.ParameterType == "1" ? "int" : n.ParameterType == "2" ? "string" : n.ParameterType == "3" ? "bool" : n.ParameterType == "4" ? "list" : n.ParameterType == "5" ? "class" : n.ParameterType == "6" ? "double" : n.ParameterType == "7" ? "strJson" : "string",
                                                    desc = n.ParameterExplain,
                                                    must = n.IsAllowNull == 1 ? true : false
                                                };
                                                if (!ListInParamsModel.Contains<InParamsModel>(nModel))
                                                {
                                                    ListInParamsModel.Add(nModel);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var mModel = new InParamsModel()
                                        {
                                            name = m.ParameterFields,
                                            value = m.ParameterType == "1" ? "int" : m.ParameterType == "2" ? "string" : m.ParameterType == "3" ? "bool" : m.ParameterType == "4" ? "list" : m.ParameterType == "5" ? "class" : m.ParameterType == "6" ? "double" : m.ParameterType == "7" ? "strJson" : "string",
                                            desc = m.ParameterExplain,
                                            must = m.IsAllowNull == 1 ? true : false
                                        };
                                        if (!ListInParamsModel.Contains<InParamsModel>(mModel))
                                        {
                                            ListInParamsModel.Add(mModel);
                                        }
                                    }
                                }
                            }
                            model.outexplain = ListInParamsModel;
                        }

                        if (model.demo == null)
                        {
                            model.demo = new object();
                        }
                        if (model.inparams == null)
                        {
                            model.inparams = new List<InParamsModel>();
                        }
                        if (model.output == null)
                        {
                            model.output = new object();
                        }
                        if (model.outexplain == null)
                        {
                            model.outexplain = new List<InParamsModel>();
                        }
                        list.Add(model);
                        index++;
                    }
                }
                string json = JsonConvertHelper.ToJson<List<ApiDocModel>>(list);
                ViewData["apiFunJson"] = json;
            }
            return View();
        }
    }
    public class ApiFunctionParams
    {
        public string ApiFunctionParamParentID { get; set; }
        public string ParameterFields { get; set; }
        public string ParameterExplain { get; set; }
        public string ParameterValue { get; set; }
        public string ParameterType { get; set; }
        public string IsAllowNull { get; set; }
        public string Type { get; set; }
    }
}
