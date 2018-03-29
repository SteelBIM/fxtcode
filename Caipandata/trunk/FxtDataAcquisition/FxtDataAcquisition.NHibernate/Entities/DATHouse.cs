using Newtonsoft.Json;
using System;

/**
 * 作者: 曾智磊
 * 时间: 2014.03.03
 * 摘要: 新建实体类
 * **/
namespace FxtDataAcquisition.NHibernate.Entities
{
    //DAT_House
    public class DATHouse
    {

        [JsonProperty(PropertyName = "houseid")]
        /// <summary>
        /// id
        /// </summary>
        public virtual int HouseId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "buildingid")]
        /// <summary>
        /// 楼栋
        /// </summary>
        public virtual int BuildingId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "housename")]
        /// <summary>
        /// 物业名称
        /// </summary>
        public virtual string HouseName
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "housetypecode")]
        /// <summary>
        /// 户型
        /// </summary>
        public virtual int? HouseTypeCode
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "floorno")]
        /// <summary>
        /// 起始楼层
        /// </summary>
        public virtual int FloorNo
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "endfloorno")]
        /// <summary>
        /// 结束楼层
        /// </summary>
        public virtual int? EndFloorNo
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "nominalfloor")]
        /// <summary>
        /// 名义层
        /// </summary>
        public virtual string NominalFloor
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "unitno")]
        /// <summary>
        /// 单元
        /// </summary>
        public virtual string UnitNo
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "buildarea")]
        /// <summary>
        /// 面积
        /// </summary>
        public virtual decimal? BuildArea
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "frontcode")]
        /// <summary>
        /// 朝向
        /// </summary>
        public virtual int? FrontCode
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "sightcode")]
        /// <summary>
        /// 景观
        /// </summary>
        public virtual int? SightCode
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "unitprice")]
        /// <summary>
        /// 单价
        /// </summary>
        public virtual decimal? UnitPrice
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "saleprice")]
        /// <summary>
        /// 总价
        /// </summary>
        public virtual decimal? SalePrice
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "weight")]
        /// <summary>
        /// 权重值
        /// </summary>
        public virtual decimal? Weight
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "photoname")]
        /// <summary>
        /// PhotoName
        /// </summary>
        public virtual string PhotoName
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "remark")]
        /// <summary>
        /// Remark
        /// </summary>
        public virtual string Remark
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "structurecode")]
        /// <summary>
        /// 户型结构
        /// </summary>
        public virtual int? StructureCode
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "totalprice")]
        /// <summary>
        /// TotalPrice
        /// </summary>
        public virtual decimal? TotalPrice
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "purposecode")]
        /// <summary>
        /// 用途
        /// </summary>
        public virtual int? PurposeCode
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "isevalue")]
        /// <summary>
        /// IsEValue
        /// </summary>
        public virtual int? IsEValue
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "cityid")]
        /// <summary>
        /// CityID
        /// </summary>
        public virtual int CityID
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "oldid")]
        /// <summary>
        /// OldId
        /// </summary>
        public virtual string OldId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "createtime")]
        /// <summary>
        /// CreateTime
        /// </summary>
        public virtual DateTime? CreateTime
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "valid")]
        /// <summary>
        /// Valid
        /// </summary>
        public virtual int? Valid
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "savedatetime")]
        /// <summary>
        /// SaveDateTime
        /// </summary>
        public virtual DateTime? SaveDateTime
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "saveuser")]
        /// <summary>
        /// SaveUser
        /// </summary>
        public virtual string SaveUser
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "fxtcompanyid")]
        /// <summary>
        /// FxtCompanyId
        /// </summary>
        public virtual int? FxtCompanyId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "isshowbuildingarea")]
        /// <summary>
        /// IsShowBuildingArea
        /// </summary>
        public virtual int? IsShowBuildingArea
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "innerbuildingarea")]
        /// <summary>
        /// InnerBuildingArea
        /// </summary>
        public virtual decimal? InnerBuildingArea
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "subhousetype")]
        /// <summary>
        /// 附属房屋类型
        /// </summary>
        public virtual int? SubHouseType
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "subhousearea")]
        /// <summary>
        /// 附属房屋面积
        /// </summary>
        public virtual decimal? SubHouseArea
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "creator")]
        /// <summary>
        /// Creator
        /// </summary>
        public virtual string Creator
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "fxthouseid")]
        /// <summary>
        /// FxtHouseId
        /// </summary>
        public virtual int? FxtHouseId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "status")]
        /// <summary>
        /// Status
        /// </summary>
        public virtual int? Status
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "vdcode")]
        /// <summary>
        /// 通风采光
        /// </summary>
        public virtual int? VDCode
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "noisecode")]
        /// <summary>
        /// 噪音情况
        /// </summary>
        public virtual int? NoiseCode
        {
            get;
            set;
        }
    }
}