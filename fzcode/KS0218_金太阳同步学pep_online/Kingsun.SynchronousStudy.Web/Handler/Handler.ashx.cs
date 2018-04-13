using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;

namespace Kingsun.SynchronousStudy.Web.Handler
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string formData = context.Request.Form["Form"];
                if (string.IsNullOrEmpty(formData))
                {
                    KingResponse response = new KingResponse();
                    response.Success = false;
                    response.ErrorMsg = "没有找到相应的数据包!";
                    response.RequestID = "";
                    response.Data = null;
                    context.Response.Write(JsonHelper.EncodeJson(response));
                    context.Response.End();
                    return;
                }
                KingForm form = JsonHelper.DecodeJson<KingForm>(formData);
                if (form == null)
                {
                    KingResponse response = new KingResponse();
                    response.Success = false;
                    response.ErrorMsg = "没有找到相应的数据包!";
                    response.RequestID = "";
                    response.Data = null;
                    context.Response.Write(JsonHelper.EncodeJson(response));
                    context.Response.End();
                    return;
                }
                string serviceID = form.SKEY;
                if (string.IsNullOrEmpty(serviceID))
                {
                    KingResponse response = new KingResponse();
                    response.Success = false;
                    response.ErrorMsg = "没有指定处理方法!";
                    response.RequestID = form.RID;
                    response.Data = null;
                    context.Response.Write(JsonHelper.EncodeJson(response));
                    context.Response.End();
                    return;
                }
                else
                {
                    Type objType = Type.GetType("Kingsun.SynchronousStudy.BLL." + serviceID.Trim() + ",Kingsun.SynchronousStudy.BLL");
                    if (objType != null)
                    {
                        string package = form.Pack;
                        string returnStr = null;

                        if (String.IsNullOrEmpty(package))
                        {
                            //返回错误信息
                            returnStr = KingResponse.GetErrorResponseString("无法找到参数包");
                        }
                        else
                        {
                            KingRequest request = KingRequest.DecodeRequest(package);
                            if (request == null)
                            {
                                returnStr = KingResponse.GetErrorResponseString("参数包解析失败");
                            }
                            else
                            {
                                BaseImplement obj = Activator.CreateInstance(objType) as BaseImplement;
                                KingResponse response;
                                if (obj != null)
                                {
                                    try
                                    {
                                        response = obj.ProcessRequest(request);
                                    }
                                    catch (Exception ex)
                                    {
                                        response = KingResponse.GetErrorResponse("服务接口内部错误，请联系管理员。" + ex.Message, request);
                                    }
                                }
                                else
                                {
                                    response = KingResponse.GetErrorResponse("无法实例化服务接口！", request);
                                }
                                returnStr = JsonHelper.EncodeJson(response);
                            }
                        }
                        context.Response.Write(returnStr);
                    }
                    else
                    {
                        KingResponse response = new KingResponse();
                        response.Success = false;
                        response.ErrorMsg = "无法确定处理程序!";
                        response.RequestID = form.RID;
                        response.Data = null;
                        context.Response.Write(JsonHelper.EncodeJson(response));
                        context.Response.End();
                    }
                }
            }
            catch (Exception e)
            {
                KingResponse response = new KingResponse();
                response.Success = false;
                response.ErrorMsg = "后台处理异常。" + e.Message;
                response.RequestID = "";
                response.Data = null;
                context.Response.Write(JsonHelper.EncodeJson(response));
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}