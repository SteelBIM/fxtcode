using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity
{
    /// <summary>
    /// 外部api返回json实体
    /// </summary>
    public class OutJsonEntity
    {
        /// <summary>
        /// 返回ID
        /// 云查勘3.0 向第三方发起查勘，返回第三方查勘id
        /// </summary>
        public long objectid { get; set; }
    }

    /// <summary>
    /// 自动估价结果
    /// </summary>
    public class AutoPrice : BaseTO
    {
        /// <summary>
        /// 单价
        /// </summary>
        public decimal unitprice { get; set; }
        /// <summary>
        /// 项目均价(楼盘均价)
        /// </summary>
        public decimal eprice { get; set; }
        /// <summary>
        /// 楼栋均价
        /// </summary>
        public decimal beprice { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
        public decimal buildingarea { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public decimal totalprice { get; set; }
        /// <summary>
        /// 房号估价
        /// </summary>
        public decimal heprice { get; set; }
        /// <summary>
        /// 楼盘案例均价
        /// </summary>
        public decimal avgprice { get; set; }
        /// <summary>
        /// 根据样本楼盘价格计算出来的本楼盘的均价
        /// </summary>
        public decimal avgprice_other { get; set; }
        /// <summary>
        /// 案例数
        /// </summary>
        public int casecount { get; set; }
        /// <summary>
        /// 案例最大值
        /// </summary>
        public int casemax { get; set; }
        /// <summary>
        /// 案例最小值
        /// </summary>
        public int casemin { get; set; }

        /// <summary>
        /// 案例选取起始时间
        /// </summary>
        public string startdate { get; set; }

        /// <summary>
        /// 案例选取终结时间
        /// </summary>
        public string enddate { get; set; }
        /// <summary>
        /// 案例均价
        /// </summary>
        public int caseavg { get; set; }
        /// <summary>
        /// 楼层
        /// </summary>
        public string nominalfloor { get; set; }
        /// <summary>
        /// 户型
        /// </summary>
        public int? housetypecode { get; set; }
        /// <summary>
        /// 房屋用途
        /// </summary>
        public int? purposecode { get; set; }
        /// <summary>
        /// 建筑类型
        /// </summary>
        public int buildingtypecode { get; set; }
        /// <summary>
        /// 总楼层
        /// </summary>
        public int totalfloor { get; set; }
        /// <summary>
        /// 竣工时间
        /// </summary>
        public string builddate { get; set; }
         /// <summary>
        /// 地址
        /// </地址>
        public string address { get; set; }
         /// <summary>
        /// 户型结构
        /// </summary>
        public int structurecode { get; set; }
        /// <summary>
        /// 自动估价记录Id
        /// </summary>
        public int historyid { get; set; }

        /// <summary>
        /// 价格计算类型（银行使用）
        /// </summary>
        public int pricetype { get; set; }

        /// <summary>
        /// 楼盘
        /// </summary>
        public string projectname { get; set; }
        /// <summary>
        /// 楼栋
        /// </summary>
        public string buildingname { get; set; }
        /// <summary>
        /// 房号
        /// </summary>
        public string housename { get; set; }

        /// <summary>
        /// 楼盘细分类型均价(需要存储在银行数据库，确保每次显示一致）
        /// </summary>
        public List<DATProjectAvgPrice> avgpricelist { get; set; }
        /// <summary>
        /// 楼盘底层均价
        /// </summary>
        public decimal plprice { get; set; }
        /// <summary>
        /// 楼盘多层均价
        /// </summary>
        public decimal pmprice { get; set; }
        /// <summary>
        /// 楼盘小高层均价
        /// </summary>
        public decimal psprice { get; set; }
        /// <summary>
        /// 楼盘高层均价
        /// </summary>
        public decimal phprice { get; set; }
    }
    /// <summary>
    /// 押品复估价格结果
    /// 创建人:曾智磊，2014-08-06
    /// </summary>
    public class ReassessmentPrice
    {
        // "{{\"price\":{0},\"pricetype\":{1},\"projecteprice\":{2},\"buildingeprice\":{3},\"remark\":\"{4}\",
        //\"nowareaprice\":{5},\"lastareaprice\":{6}}}";
        /// <summary>
        /// 价格结果
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// 计算方式(//1:自动估价值，2:现复估值,0：估不出值)
        /// </summary>
        public int pricetype { get; set; }
        /// <summary>
        /// 楼盘均价
        /// </summary>
        public decimal projecteprice { get; set; }
        /// <summary>
        /// 楼栋均价
        /// </summary>
        public decimal buildingeprice { get; set; }
        /// <summary>
        /// 计算说明
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 当前月行政区楼盘均价
        /// </summary>
        public decimal nowareaprice { get; set; }
        /// <summary>
        /// 上个月行政区楼盘均价
        /// </summary>
        public decimal lastareaprice { get; set; }
        /// <summary>
        /// 楼盘主用途
        /// </summary>
        public int purposecode { get; set; }
    }
}
