using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Common;

namespace FxtSpider.FxtApi.Model
{
    /// <summary>
    /// 楼盘下别墅中 别墅用途类型下案例个数
    /// </summary>
    public class FxtApi_ProjectJoinPurposeTypeJoinVillaCaseCount
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
        /// 用途
        /// </summary>
        public virtual int PurposeTypeCode
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

        public static FxtApi_ProjectJoinPurposeTypeJoinVillaCaseCount ConvertToObj(string json)
        {
            return JsonHelp.ParseJSONjss<FxtApi_ProjectJoinPurposeTypeJoinVillaCaseCount>(json);
        }
        public static List<FxtApi_ProjectJoinPurposeTypeJoinVillaCaseCount> ConvertToObjList(string json)
        {
            return JsonHelp.ParseJSONList<FxtApi_ProjectJoinPurposeTypeJoinVillaCaseCount>(json);
        }
    }
}
