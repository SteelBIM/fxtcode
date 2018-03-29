using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Common;

namespace FxtSpider.FxtApi.Model
{
    /// <summary>
    /// 楼盘下普通住宅中 面积段,建筑类型下案例个数
    /// </summary>
    public class FxtApi_ProjectJoinBuildingTypeJoinAreaTypeJoinPublicCaseCount
    {
        /// <summary>
        /// 楼盘ID
        /// </summary>
        public virtual int ProjectId
        {
            get;
            set;
        }
        /// <summary>
        /// 城市ID
        /// </summary>
        public virtual int CityId
        {
            get;
            set;
        }
        /// <summary>
        /// 月份日期(201202)
        /// </summary>
        public virtual string Date
        {
            get;
            set;
        }
        /// <summary>
        /// 建筑类型
        /// </summary>
        public virtual int BuildingTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 面积段
        /// </summary>
        public virtual int BuildingAreaTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 个数
        /// </summary>
        public virtual int Count
        {
            get;
            set;
        }

        public static FxtApi_ProjectJoinBuildingTypeJoinAreaTypeJoinPublicCaseCount ConvertToObj(string json)
        {
            return JsonHelp.ParseJSONjss<FxtApi_ProjectJoinBuildingTypeJoinAreaTypeJoinPublicCaseCount>(json);
        }
        public static List<FxtApi_ProjectJoinBuildingTypeJoinAreaTypeJoinPublicCaseCount> ConvertToObjList(string json)
        {
            return JsonHelp.ParseJSONList<FxtApi_ProjectJoinBuildingTypeJoinAreaTypeJoinPublicCaseCount>(json);
        }
    }
}
