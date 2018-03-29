using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Common;

namespace FxtSpider.FxtApi.Model
{
    public class FxtApi_ProjectJoinPurposeTypeJoinCaseCount
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
        /// 普通住宅个数
        /// </summary>
        public virtual int PurposePublicCount
        {
            get;
            set;
        }
        /// <summary>
        /// 别墅个数
        /// </summary>
        public virtual int PurposeVillaCount
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


        public static FxtApi_ProjectJoinPurposeTypeJoinCaseCount ConvertToObj(string json)
        {
            return JsonHelp.ParseJSONjss<FxtApi_ProjectJoinPurposeTypeJoinCaseCount>(json);
        }
        public static List<FxtApi_ProjectJoinPurposeTypeJoinCaseCount> ConvertToObjList(string json)
        {
            return JsonHelp.ParseJSONList<FxtApi_ProjectJoinPurposeTypeJoinCaseCount>(json);
        }
    }
}
