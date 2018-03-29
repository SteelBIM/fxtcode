using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtDataCenter.Price
{
    /// <summary>
    /// 数据中心—价格走势数据类
    /// 潘锦发-2015-04-14
    /// </summary>

    /// <summary>
    /// 价格走势区域列表
    /// </summary>
    public class PriceThendEntity:BaseTO
    {
        /// <summary>
        /// 城市ID
        /// </summary>
        public int cityid { get; set; }
        /// <summary>
        /// 区域ID
        /// </summary>
        public int areaid { get; set; }
        /// <summary>
        /// 环比
        /// </summary>
        public double linkratio  { get; set; }
        /// <summary>
        /// 同比
        /// </summary>
         public double yearbasis { get; set; }
        /// <summary>
        /// 平均价格
        /// </summary>
        public double avgprice { get; set; }
        /// <summary>
        /// 价格所属当月日期
        /// </summary>
        public DateTime avgpricedate { get; set; }
        /// <summary>
        /// 坐标x
        /// </summary>
        public decimal? x { get; set; }
        /// <summary>
        /// y
        /// </summary>
        public decimal? y { get; set; }
    }
    /// <summary>
    /// 价格监测区域列表
    /// </summary>
    public class PMAreaEntity : BaseTO
    {
        /// <summary>
        /// 区域ID
        /// </summary>
        public int? areaid { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string areaname { get; set; }
        /// <summary>
        /// 环比
        /// </summary>
        public double? centage { get; set; }
        /// <summary>
        /// 平均价格
        /// </summary>
        public decimal? avgprice { get; set; }
        /// <summary>
        /// x
        /// </summary>
        public decimal? x { get; set; }
        /// <summary>
        /// y
        /// </summary>
        public decimal? y { get; set; }
        /// <summary>
        /// 价格所属当月日期
        /// </summary>
        public string avgpricedate { get; set; }
    }
    /// <summary>
    /// 价格监测片区列表
    /// </summary>
    public class PMSubAreaEntity : BaseTO
    {
        /// <summary>
        /// 区域ID
        /// </summary>
        public int? subareaid { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string areaname { get; set; }
        /// <summary>
        /// 环比
        /// </summary>
        public double? centage { get; set; }
        /// <summary>
        /// 平均价格
        /// </summary>
        public decimal? avgprice { get; set; }
        /// <summary>
        /// x
        /// </summary>
        public decimal? x { get; set; }
        /// <summary>
        /// y
        /// </summary>
        public decimal? y { get; set; }
        /// <summary>
        /// 价格所属当月日期
        /// </summary>
        public string avgpricedate { get; set; }
    }
    /// <summary>
    /// 价格监测楼盘列表
    /// </summary>
    public class PMProjectEntity : BaseTO
    {
        /// <summary>
        /// 区域ID
        /// </summary>
        public int? projectid { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string projectname { get; set; }
        /// <summary>
        /// 环比
        /// </summary>
        public double? centage { get; set; }
        /// <summary>
        /// 平均价格
        /// </summary>
        public decimal? avgprice { get; set; }
        /// <summary>
        /// x
        /// </summary>
        public decimal? x { get; set; }
        /// <summary>
        /// y
        /// </summary>
        public decimal? y { get; set; }
        /// <summary>
        /// 价格所属当月日期
        /// </summary>
        public string avgpricedate { get; set; }
    }

    public class AreaEntity : BaseTO
    {
        public int cityid { get; set; }

        public int areaid { get; set; }

        public int orderid { get; set; }

        public string areaname { get; set; }

        public decimal? x { get; set; }

        public decimal? y { get; set; }

        public int recordcount { get; set; }

        public int CustomPrimaryKeyIdentify { get; set; }

        public bool IsSetCustomerFields { get; set; }

    }

    public class ProEntity : BaseTO
    {
        public int cityid { get; set; }

        public string cityname { get; set; }

        public string alias { get; set; }

        public int provinceid { get; set; }

        public string citycode { get; set; }

        public decimal? x { get; set; }

        public decimal? y { get; set; }

        public object citypy { get; set; }

        public int recordcount { get; set; }

        public int CustomPrimaryKeyIdentify { get; set; }

        public bool IsSetCustomerFields { get; set; }

    }

    public class PriceSelEntity : BaseTO
    {
        /// <summary>
        /// 价格面积id
        /// </summary>
        public int? areaTypeid { get; set; }
        /// <summary>
        ///  价格面积
        /// </summary>
        public string areaTypename { get; set; }
        /// <summary>
        ///  价格户型类型标识id
        /// </summary>
        public int? houseTypeid { get; set; }
        /// <summary>
        /// 价格户型类型名称
        /// </summary>
        public string houseTypename { get; set; }
        /// <summary>
        /// 建筑类型标识码id
        /// </summary>
        public int? buildingtypeid { get; set; }
        /// <summary>
        /// 建筑类型名称
        /// </summary>
        public string buildingtypename { get; set; }
        /// <summary>
        /// 价格日期ID
        /// </summary>
        public int? buildDateid { get; set; }
        /// <summary>
        /// 价格日期
        /// </summary>
        public string buildDatename { get; set; }
    }

    public class priceMonitorEntity : BaseTO
    {
        public int? unitprice { get; set; }
    }
    public class priceByTypeEntity : BaseTO
    {
        public string avgpricedate { get; set; }

        public decimal? avgprice { get; set; }

        public string circle { get; set; }

        public string same { get; set; }

        public decimal?  Exponential { get; set; }
    }
}
