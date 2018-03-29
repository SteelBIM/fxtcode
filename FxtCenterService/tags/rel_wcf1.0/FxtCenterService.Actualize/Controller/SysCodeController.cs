using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Common;
using FxtCenterService.Logic;
using CAS.Entity;
using Newtonsoft.Json.Linq;
using CAS.Entity.SurveyDBEntity;

namespace FxtCenterService.Actualize
{
    public partial class DataController
    {   
        /// <summary>
        /// 根据编号获取syscode
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        public static string  GetSYSCodeList(JObject funinfo, UserCheck company)
        {
            int code = funinfo.Value<int>("code");
            List<CAS.Entity.SurveyDBEntity.SYSCode> list = SYSCodeBL.GetSYSCodeList(code);
            return list.ToJson();
        }

        /// <summary>
        /// 根据类型获取syscode 
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        public static string GetSYSCodeListByDictType(JObject funinfo, UserCheck company)
        {
            int type = funinfo.Value<int>("type");
            List<CAS.Entity.SurveyDBEntity.SYSCode> list = SYSCodeBL.GetSYSCodeListByDictType(type);
            return list.ToJson();
        }
    }
}
