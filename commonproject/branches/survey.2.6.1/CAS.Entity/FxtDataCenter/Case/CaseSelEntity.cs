using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtDataCenter.Case
{
    /// <summary>
    /// 案例列表数据类
    /// </summary>
    public class CaseSelEntity : BaseTO
    {
        /// <summary>
        /// 城市ID
        /// </summary>
        public int cityid { get; set; }
        /// <summary>
        /// 区域ID
        /// </summary>
        public int? areaid { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string areaname { get; set; }
        /// <summary>
        /// 楼盘ID
        /// </summary>
        public int? projectid { get; set; }
        /// <summary>
        ///楼盘名称
        /// </summary>
        public string projectname { get; set; }
        /// <summary>
        /// 案例ID
        /// </summary>
        public int? caseid { get; set; }
        /// <summary>
        /// 案例日期
        /// </summary>
        public DateTime casedate { get; set; }
        /// <summary>
        /// 案例类型标识id
        /// </summary>
        public int? casetypecode { get; set; }
        /// <summary>
        /// 案例类型名称
        /// </summary>
        public string casetypecodename { get; set; }
        /// <summary>
        /// 建筑面积
        /// </summary>
        public decimal? buildingarea { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? unitprice { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public decimal? totalprice { get; set; }
        /// <summary>
        /// 建筑类型标识码id
        /// </summary>
        public int? buildingtypecode { get; set; }
        /// <summary>
        /// 建筑类型名称
        /// </summary>
        public string buildingtypecodename { get; set; }
        /// <summary>
        /// 用途标识码id
        /// </summary>
        public int? purposecode { get; set; }
        /// <summary>
        /// 用途类型名称
        /// </summary>
        public string purposecodename { get; set; }
    }

    /// <summary>
    /// 下拉列表数据类
    /// </summary>
    public class CodeEntity : BaseTO
    {
        /// <summary>
        /// ID
        /// </summary>
        public int? id { get; set; }
        /// <summary>
        /// 码号
        /// </summary>
        public int? code { get; set; }
        /// <summary>
        /// 码名称
        /// </summary>
        public string codename { get; set; }
        /// <summary>
        /// 子码号
        /// </summary>
        public int? subcode { get; set; }
        /// <summary>
        /// 码类型
        /// </summary>
        public string codetype { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }

    /// <summary>
    /// 住宅案例最高单价、最低单价、平均单价
    /// </summary>
    public class CasePriceEntity : BaseTO
    {
        public decimal? maxunitprice { get; set; }

        public decimal? minunitprice { get; set; }

        public decimal? avgprice { get; set; }
    }
}
