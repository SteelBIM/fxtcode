using CAS.Common;
using FxtCenterService.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtCenterService.Logic
{
    public class DatProjectForCASBL
    {
        /// <summary>
        /// 获取楼盘列表ForCAS
        /// </summary>
        /// <param name="search"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="cityid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IEnumerable<object> GetProjecttListByKey(SearchBase search, int fxtcompanyid, int cityid, string key)
        {
            var autoproject = DatProjectDA.GetSearchProjectListByKey(search, fxtcompanyid, cityid, key).Select(o => new
            {
                projectid = EncryptHelper.ProjectIdEncrypt(o.projectid.ToString()),
                projectname = o.projectname,
                isevalue = o.isevalue,
                recordcount = o.recordcount,
                address = o.address,
                x = o.x,
                y = o.y,
                areaid = o.areaid,
                weight = o.weight
            });

            return autoproject;
        }
    }
}
