using CBSS.Account.Contract;
using CBSS.Core.Log;
using CBSS.Core.Utility;
using CBSS.Framework.Contract;
using CBSS.Framework.Contract.Enums;
using CBSS.Tbx.BLL;
using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using CBSS.Web.Admin.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBSS.Web.Admin.Areas.Tbx.Controllers
{

    public class ApiFunctionController : AdminControllerBase
    {
        //
        // GET: /ApiFunction/

        public ActionResult Index(ApiFunctionRequest request)
        {
            int totalcount = 0;
            PagedList<Sys_ApiFunction> list = new PagedList<Sys_ApiFunction>(apiServer.GetAllSys_ApiFunction(out totalcount, request), request.PageIndex, PageSize, totalcount);
            ViewData["ApiFuncWayEnum"] = EnumHelper.GetItemValueList<ApiFuncEnums.ApiFuncWayEnum>();
            ViewData["SystemEnum"] = EnumHelper.GetItemValueList<SystemEnum>();
            ViewData["SystemCode"] = request.SystemCode;
            return View(list);
        }
        public ActionResult Create()
        {
            ViewData["ApiFuncWayEnum"] = EnumHelper.GetItemValueList<ApiFuncEnums.ApiFuncWayEnum>();
            ViewData["SystemEnum"] = EnumHelper.GetItemValueList<SystemEnum>();
            return View();
        }
        ApiFunctionService apiServer = new ApiFunctionService();
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
                    string selSystemCode = Request["selSystemCode"];
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
                        bool flag = apiServer.SaveApiFunction(apiFun, listParams);
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
            var ApiFunctionModel = this.apiServer.GetApiFunction(id);
            if (ApiFunctionModel != null)
            {
                var List = this.apiServer.GetApiFunctionParam(id);
                ViewData["ApiFunctionParam"] = List;
            }
            ViewData["ApiFuncWayEnum"] = EnumHelper.GetItemValueList<ApiFuncEnums.ApiFuncWayEnum>();
            ViewData["SystemEnum"] = EnumHelper.GetItemValueList<SystemEnum>();
            return View("Create", ApiFunctionModel);
        }

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            if (ids == null)
            {
                return RedirectToAction("Index");
            }
            bool flag = this.apiServer.DeleteApiFunction(ids);
            return RedirectToAction("Index");
        }
        [ValidateInput(false)]
        /// <summary>
        /// 生成接口文档
        /// </summary>
        /// <returns></returns>
        public ActionResult ApiDocument()
        {
            string ApiFunctionName = Request["ApiFunctionName"];
            string SystemCode = Request["SystemCode"];
            List<Sys_ApiFunction> ApiFunIDList = apiServer.GetApiFunctionList(ApiFunctionName, SystemCode);
            List<int> ApiFunIDStr = new List<int>();
            foreach (Sys_ApiFunction item in ApiFunIDList)
            {
                ApiFunIDStr.Add(item.ApiFunctionID);
            }
            string ids = string.Join(",", ApiFunIDStr);
            if (!string.IsNullOrEmpty(ids))
            {
                //string id = Request.RequestContext.RouteData.Values["id"].ToString();
                List<Sys_ApiFunction> apiFunList = apiServer.GetApiFunctionList(ids);
                List<Sys_ApiFunctionParam> apiFunParamList = apiServer.GetApiFunctionParamList(ids);
                List<Sys_ApiFunctionParam> RequestParamList = null; //请求参数 
                List<Sys_ApiFunctionParam> ResponseParamList = null; //返回参数 
                List<ApiDocModel> list = new List<ApiDocModel>();
                IDictionary<int, string> WayEnums = EnumHelper.GetItemValueList<ApiFuncEnums.ApiFuncWayEnum>();
                IDictionary<int, string> SystemEnums = EnumHelper.GetItemValueList<SystemEnum>();
                if (apiFunList != null && apiFunList.Count > 0)
                {
                    int index = 0;
                    foreach (var item in apiFunList)
                    {
                        ApiDocModel model = new ApiDocModel();
                        model.SystemCode = item.SystemCode;
                        model.SystemName = SystemEnums[Convert.ToInt32(item.SystemCode)];
                        model.name = item.ApiFunctionExplain + "(" + item.ApiFunctionName + ")";
                        model.url = item.ApiFunctionUrl;
                        model.hover = index == 0 ? true : false;
                        model.format = "json";
                        model.method = "Post"; 
                        model.apiway = WayEnums[item.ApiFunctionWay];
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
                                    reqJson += "[{";
                                    foreach (var n in RequestParamList.Where(a => a.ApiFunctionParamParentID == m.ParameterFields))
                                    {
                                        reqJson += "'" + n.ParameterFields + "':";// "'" + m.ParameterValue + "',";
                                        if (RequestParamList.Where(a => a.ApiFunctionParamParentID == n.ParameterFields).Count() > 0)//子集
                                        {
                                            reqJson += "[{";
                                            foreach (var o in RequestParamList.Where(a => a.ApiFunctionParamParentID == n.ParameterFields))
                                            {
                                                reqJson += "'" + o.ParameterFields + "':" + "'" + o.ParameterValue + "',";
                                            }
                                            reqJson = reqJson.TrimEnd(',');
                                            reqJson += "}]";
                                        }
                                        else
                                        {
                                            reqJson += "'" + n.ParameterValue + "',";
                                        }
                                    }
                                    reqJson = reqJson.TrimEnd(',');
                                    reqJson += "}]";
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
                            model.demo = JsonConvertHelper.ToObject<object>(reqJson);
                            //请求参数说明
                            {
                                foreach (var a in RequestParamList)
                                {
                                    if (RequestParamList.Where(m => m.ApiFunctionParamParentID == a.ParameterFields).Count() > 0)//子集
                                    {
                                        ListInParamsModel.Add(new InParamsModel()
                                        {
                                            name = a.ParameterFields,
                                            value = "json",
                                            desc = a.ParameterExplain,
                                            must = a.IsAllowNull == 1 ? true : false
                                        });
                                        //foreach (var b in RequestParamList.Where(m => m.ApiFunctionParamParentID == a.ParameterFields))
                                        //{
                                        //    if (RequestParamList.Where(m => m.ApiFunctionParamParentID == b.ParameterFields).Count() > 0)//子集
                                        //    {
                                        //        ListInParamsModel.Add(new InParamsModel()
                                        //        {
                                        //            name = b.ParameterFields,
                                        //            value = "json",
                                        //            desc = b.ParameterExplain,
                                        //            must = b.IsAllowNull == 1 ? true : false
                                        //        }); 
                                        //    }
                                        //    else
                                        //    {
                                        //        ListInParamsModel.Add(new InParamsModel()
                                        //        {
                                        //            name = b.ParameterFields,
                                        //            value = b.ParameterType == "1" ? "int" : b.ParameterType == "2" ? "string" : b.ParameterType == "3" ? "bool" : "list",
                                        //            desc = b.ParameterExplain,
                                        //            must = b.IsAllowNull == 1 ? true : false
                                        //        });
                                        //    }
                                        //}
                                    }
                                    else
                                    {
                                        ListInParamsModel.Add(new InParamsModel()
                                        {
                                            name = a.ParameterFields,
                                            value = a.ParameterType == "1" ? "int" : a.ParameterType == "2" ? "string" : a.ParameterType == "3" ? "bool" : "list",
                                            desc = a.ParameterExplain,
                                            must = a.IsAllowNull == 1 ? true : false
                                        });
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
                                    reqJson += "[{";
                                    foreach (var n in ResponseParamList.Where(a => a.ApiFunctionParamParentID == m.ParameterFields))
                                    {
                                        reqJson += "'" + n.ParameterFields + "':";// "'" + m.ParameterValue + "',";
                                        if (ResponseParamList.Where(a => a.ApiFunctionParamParentID == n.ParameterFields).Count() > 0)//子集
                                        {
                                            reqJson += "[{";
                                            foreach (var o in ResponseParamList.Where(a => a.ApiFunctionParamParentID == n.ParameterFields))
                                            {
                                                reqJson += "'" + o.ParameterFields + "':" + "'" + o.ParameterValue + "',";
                                            }
                                            reqJson = reqJson.TrimEnd(',');
                                            reqJson += "}]";
                                        }
                                        else
                                        {
                                            reqJson += "'" + n.ParameterValue + "',";
                                        }
                                    }
                                    reqJson = reqJson.TrimEnd(',');
                                    reqJson += "}],";
                                }
                                else
                                {
                                    reqJson += "'" + m.ParameterValue + "',";
                                }
                            }
                            reqJson = reqJson.TrimEnd(',');
                            reqJson += "}";
                            model.output = JsonConvertHelper.ToObject<object>(reqJson);
                            //返回参数说明
                            {
                                foreach (var a in ResponseParamList)
                                {
                                    if (ResponseParamList.Where(m => m.ApiFunctionParamParentID == a.ParameterFields && a.Type == 1).Count() > 0)//子集
                                    {
                                        ListInParamsModel.Add(new InParamsModel()
                                        {
                                            name = a.ParameterFields,
                                            value = "json",
                                            desc = a.ParameterExplain,
                                            must = a.IsAllowNull == 1 ? true : false
                                        });
                                    }
                                    else
                                    {
                                        ListInParamsModel.Add(new InParamsModel()
                                        {
                                            name = a.ParameterFields,
                                            value = a.ParameterType == "1" ? "int" : a.ParameterType == "2" ? "string" : a.ParameterType == "3" ? "bool" : "list",
                                            desc = a.ParameterExplain,
                                            must = a.IsAllowNull == 1 ? true : false
                                        });
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
