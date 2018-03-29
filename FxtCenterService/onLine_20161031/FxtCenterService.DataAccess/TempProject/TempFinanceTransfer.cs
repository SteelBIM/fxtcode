using CAS.Entity.BaseDAModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtCenterService.DataAccess.TempProject
{
    [Serializable]
    [TableAttribute("dbo.DAT_PL_mortgage_Original")]
    public class TempFinanceTransfer : BaseTO
    {
        /// <summary>
        /// 押品id（系统自动生成）
        /// </summary>
        public string mortgageId { get; set; }
        /// <summary>
        /// 抵押物名称
        /// </summary>
        public string mortgageName { get; set; }
        /// <summary>
        /// 楼盘ID，对应Excel模版的楼盘编号
        /// </summary>
        public string projectId { get; set; }
        /// <summary>
        /// 数据中心楼盘ID
        /// </summary>
        public string fxtprojectId { get; set; }
        /// <summary>
        /// 楼盘名称
        /// </summary>
        public string projectName { get; set; }
        ///// <summary>
        ///// 楼盘地址
        ///// </summary>
        //public string address { get; set; }
        /// <summary>
        /// 楼栋名称
        /// </summary>
        public string buildingName { get; set; }
        /// <summary>
        /// 楼栋ID
        /// </summary>
        public int buildingId { get; set; }
        /// <summary>
        /// 数据中心楼栋ID
        /// </summary>
        public string fxtbuildingId { get; set; }
        /// <summary>
        /// 所在楼层
        /// </summary>
        public string floorNumber { get; set; }
        /// <summary>
        /// 总楼层
        /// </summary>
        public int totalFloor { get; set; }
        /// <summary>
        /// 房号名称
        /// </summary>
        public string houseName { get; set; }
        /// <summary>
        /// 房号ID
        /// </summary>
        public Int64 houseId { get; set; }
        ///// <summary>
        ///// 数据中心房号ID
        ///// </summary>
        //public Int64 fxthouseId { get; set; }
        /// <summary>
        /// 单价（复估价格）
        /// </summary>
        public decimal unitPrice { get; set; }
        /// <summary>
        /// 总价（复估价格）
        /// </summary>
        public decimal totalPrice { get; set; }
        /// <summary>
        /// 省份ID
        /// </summary>
        public int provinceId { get; set; }
        /// <summary>
        /// 省份名称
        /// </summary>
        public string provinceName { get; set; }
        /// <summary>
        /// 城市ID
        /// </summary>
        public int cityId { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        public string  cityName { get; set; }
        /// <summary>
        /// 区域ID
        /// </summary>
        public int areaId { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string areaName { get; set; }
        /// <summary>
        /// 路
        /// </summary>
        public string road { get; set; }
        /// <summary>
        /// 号
        /// </summary>
        public string no { get; set; }
        ///// <summary>
        ///// 标准化表备注
        ///// </summary>
        //public string remarkMortgage { get; set; }
        /// <summary>
        /// 押品名称
        /// </summary>
        public string collateralName { get; set; }
        /// <summary>
        /// 押品地址
        /// </summary>
        public string collateralAddress { get; set; }
        /// <summary>
        /// 押品面积
        /// </summary>
        public decimal collateralArea { get; set; }
        /// <summary>
        /// 押品编号
        /// </summary>
        public string collateralNo { get; set; }
        /// <summary>
        /// 公司Id
        /// </summary>
        public int companyId { get; set; }
        ///// <summary>
        ///// 省份Id
        ///// </summary>
        //public int provinceIdCollateral { get; set; }
        ///// <summary>
        ///// 省份名称
        ///// </summary>
        //public string provinceNameCollateral { get; set; }
        ///// <summary>
        ///// 城市Id
        ///// </summary>
        //public int cityIdCollateral { get; set; }
        ///// <summary>
        ///// 城市名称
        ///// </summary>
        //public string cityNameCollateral { get; set; }
        ///// <summary>
        ///// 业务所在区Id
        ///// </summary>
        //public int regionId { get; set; }
        ///// <summary>
        ///// 区名称
        ///// </summary>
        //public string regionName { get; set; }
        /// <summary>
        /// 押品类型
        /// </summary>
        public string collateralType { get; set; }
        /// <summary>
        /// 原始备注
        /// </summary>
        public string remarkCollateral { get; set; }
    }
}
