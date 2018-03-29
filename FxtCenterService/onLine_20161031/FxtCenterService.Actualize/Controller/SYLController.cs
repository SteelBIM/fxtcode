using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Common;
using FxtCenterService.Logic;
using CAS.Entity;
using Newtonsoft.Json.Linq;
using CAS.Entity.DBEntity;
using OpenPlatform.Framework.FlowMonitor;

namespace FxtCenterService.Actualize
{
    public partial class DataController
    {
        /// <summary>
        /// 获取商业案例列表
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Case)]
        public static string GetSYLList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            int cityId = search.CityId;
            var list = SYLBL.GetSYLList(cityId);
            return list.ToJson();
        }
    }
}
