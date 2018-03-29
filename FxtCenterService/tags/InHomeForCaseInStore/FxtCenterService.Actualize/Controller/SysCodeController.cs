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
            int id = funinfo.Value<int>("id");
            List<CAS.Entity.SurveyDBEntity.SYSCode> list = SYSCodeBL.GetSYSCodeList(id);
            return list.ToJson();
        }

    }
}
