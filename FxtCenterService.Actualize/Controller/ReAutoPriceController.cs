using CAS.Common;
using CAS.Entity;
using FxtCenterService.Logic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtCenterService.Actualize
{
    /// <summary>
    /// 复估
    /// </summary>
    public partial class DataController
    {
        /// <summary>
        /// 复估楼盘列表
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string ProjectListForReAutoPrice(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);

            if (search.PageIndex == 0)
            {
                search.PageIndex = 1;

                search.PageRecords = 15;

                search.Page = true;
            }

            search.FxtCompanyId = company.parentshowdatacompanyid;

            search.SysTypeCode = company.parentproducttypecode;

            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));

            search.AreaId = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));

            search.Key = funinfo.Value<string>("key");

            var data = ReAutoPriceBL.GetProjectList(search);

            return data.ToJson();
        }
    }
}
