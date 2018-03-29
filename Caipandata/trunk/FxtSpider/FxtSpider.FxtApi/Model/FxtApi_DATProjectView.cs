using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Common;

namespace FxtSpider.FxtApi.Model
{
    public class FxtApi_DATProjectView
    {
        /// <summary>
        /// ID
        /// </summary>
        public virtual int ProjectId
        {
            get;
            set;
        }
        /// <summary>
        /// 楼盘名称
        /// </summary>
        public virtual string ProjectName
        {
            get;
            set;
        }
        /// <summary>
        /// CityID
        /// </summary>
        public virtual int CityID
        {
            get;
            set;
        }
        /// <summary>
        /// CityName
        /// </summary>
        public virtual string CityName
        {
            get;
            set;
        }
        /// <summary>
        /// 行政区
        /// </summary>
        public virtual int AreaID
        {
            get;
            set;
        }
        /// <summary>
        /// 行政区名称
        /// </summary>
        public virtual string AreaName
        {
            get;
            set;
        }
        public static FxtApi_DATProjectView ConvertToObj(string json)
        {
            return JsonHelp.ParseJSONjss<FxtApi_DATProjectView>(json);
        }
        public static List<FxtApi_DATProjectView> ConvertToObjList(string json)
        {
            return JsonHelp.ParseJSONList<FxtApi_DATProjectView>(json);
        }
    }
}
