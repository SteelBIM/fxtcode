using FxtCommonLibrary.LibraryUtils;
using FxtNHibernate.DTODomain.APIActualizeDTO;
using FxtNHibernate.DTODomain.DATProjectDTO;
using FxtService.Common;
using FxtService.Contract.APIInterface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Web;
using System.Web.Mvc;

/**
 * 作者: 曾智磊
 * 时间:2014-03-03
 * 摘要:新建手机端API公共wcf入口APIActualize.FxtAPI契约(接口)实现
 * **/
namespace FxtService.Service.APIActualize
{
    public class FxtMobileAPI : IFxtMobileAPI
    {

        #region 公共入口
        public ResultData Entrance(string type, string name, string date, string code, string parameter)
        {
            //OperationContext context = OperationContext.Current;

            EntranceHelper eHelper = new EntranceHelper();
            if (!eHelper.GetCode(date).Equals(code))
            {
                return new ResultData { Type = 0, Message = "验证码错误" } ;//Utility.GetJson(0, "验证码错误!");
            }
            else
            {
                MatchClass mc = eHelper.GetMatchClass(type);
                if (mc == null)
                {
                    return new ResultData { Type = 0, Message = "未找到对象!" };
                }
                object objClass = System.Reflection.Assembly.Load(mc.Library).CreateInstance(mc.ClassName);
                if (objClass == null)
                {
                    return new ResultData { Type = 0, Message = "未找到对象!" };
                }
                MethodInfo method = objClass.GetType().GetMethod(name);
                if (method == null)
                {
                    return new ResultData { Type = 0, Message = "未找到对象!" };
                }
                try
                {
                    ParameterInfo[] parameterInfos = method.GetParameters();
                    object[] objvalue = null;
                    JObject jobject = new JObject();
                    if (!Utils.IsNullOrEmpty(parameter))
                    {
                        jobject = JObject.Parse(parameter);
                    }
                    if (jobject.Count != parameterInfos.Length)
                    {
                        return new ResultData { Type = 0, Message = "未找到对象!" };
                    }
                    objvalue = new object[parameterInfos.Length];
                    for (int i = 0; i < parameterInfos.Length; i++)
                    {
                        ParameterInfo parameterInfo = parameterInfos[i];
                        if (jobject[parameterInfo.Name] == null)
                        {
                            return new ResultData { Type = 0, Message = "未找到对象!" };
                        }
                        if (jobject[parameterInfo.Name].Type == JTokenType.Null)
                        {
                            objvalue[i] = null;
                            continue;
                        }
                        objvalue[i] = Utility.valueType(parameterInfo.ParameterType, jobject.Value<JValue>(parameterInfo.Name).Value); 
                    }
                    string resultJson = Convert.ToString(method.Invoke(objClass, objvalue));
                    JObject _jobject = JObject.Parse(resultJson);

                    int Type = _jobject.Value<int>("type");
                    string Message = _jobject.Value<string>("message");
                    object Data = _jobject.Value<object>("data");
                    int Count = _jobject.Value<int>("count");
                    string datajson = Utils.Serialize(Data);
                    //JObject _jobject2 = JObject.Parse(datajson);
                    ResultData obj = new ResultData { Type = Type, Message = Message, Data = "", Count = Count };
                    return obj;

                }
                catch (Exception exe)
                {
                    return new ResultData { Type = 0, Message = exe.Message };
                }
            }
        }

        //public ResultData GetFile(System.IO.Stream stream)
        //{
        //    stream.Dispose();
        //    return new ResultData { Count = 0, Data = "sdf", Message = "sdf", Type = 1 };
        //    try
        //    {
        //        using (StreamReader sr = new StreamReader(stream))
        //        {
        //            string str = sr.ReadToEnd();
        //            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;

        //    }

        //}

        #endregion

        public string aa(string b) {
            return b;
        }
    }
}
