using System;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

/***********************************************************
 * 功能：请求数据中心 序列化Json调用类
 *  
 * 创建：魏贝
 * 时间：2015/05
***********************************************************/

namespace FXT.VQ.UserService.Model
{
    [Serializable]
    public class SurveyApi
    {
        private AllInfo _info;
        private SecurityInfo _sInfo;
        public SurveyApi(string functionname)
        {
            _jss.MaxJsonLength = 10240000;
            _sInfo = new SecurityInfo(functionname);
            _info = new AllInfo(new UserInfo());
        }

        public SecurityInfo sinfo
        {
            get
            {
                return _sInfo;
            }
        }
        public AllInfo info
        {
            get
            {
                return _info;
            }
        }
        JavaScriptSerializer _jss = new JavaScriptSerializer();
        public string GetJsonString()
        {
            //string sinfo = _jss.Serialize(this._sInfo).Replace("\"", "'");
            //string info = _jss.Serialize(this._info).Replace("\"", "'");
            //return "{\"sinfo\":\"" + sinfo + "\",\"info\":\"" + info + "\"}";
            //modify \的问题  不能解析
            string sinfo = _jss.Serialize(this._sInfo);
            string info = _jss.Serialize(this._info);
            return new { sinfo = sinfo, info = info }.ToJsonString();
        }
    }

    /// <summary>
    /// 扩展类
    /// </summary>
    internal static class ExtensionHelper
    {
        /// <summary>
        /// 将对象转化成json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJsonString(this Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}