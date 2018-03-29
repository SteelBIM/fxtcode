using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.DTODomain.NHibernate
{
    public class View_AllotFlowJoinProject
    {

        [JsonProperty(PropertyName = "id")]
        /// <summary>
        /// ID(任务表)
        /// </summary>
        public virtual long id
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "allotcityid")]
        /// <summary>
        /// 城市ID(任务表)
        /// </summary>
        public virtual int AllotCityId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "allotfxtcompanyid")]
        /// <summary>
        /// 公司ID(任务表)
        /// </summary>
        public virtual int AllotFxtCompanyId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "allotcreatetime")]
        /// <summary>
        /// 创建时间(任务表)
        /// </summary>
        public virtual DateTime AllotCreateTime
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "statedate")]
        /// <summary>
        /// 状态更新时间(任务表)
        /// </summary>
        public virtual DateTime? StateDate
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "statecode")]
        /// <summary>
        /// 状态Code(任务表)
        /// </summary>
        public virtual int StateCode
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "dattype")]
        /// <summary>
        /// 数据类型(任务表)
        /// </summary>
        public virtual int DatType
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "username")]
        /// <summary>
        /// 分配人(任务表)
        /// </summary>
        public virtual string UserName
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "usertruename")]
        /// <summary>
        /// 分配人(任务表)
        /// </summary>
        public virtual string UserTrueName
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "surveyusername")]
        /// <summary>
        /// 查勘员(任务表)
        /// </summary>
        public virtual string SurveyUserName
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "surveyusertruename")]
        /// <summary>
        /// 查勘员(任务表)
        /// </summary>
        public virtual string SurveyUserTrueName
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "projectid")]
        /// <summary>
        /// ID
        /// </summary>
        public virtual int ProjectId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "projectname")]
        /// <summary>
        /// 楼盘名称
        /// </summary>
        public virtual string ProjectName
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "subareaid")]
        /// <summary>
        /// 片区
        /// </summary>
        public virtual int? SubAreaId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "fieldno")]
        /// <summary>
        /// 宗地号
        /// </summary>
        public virtual string FieldNo
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "purposecode")]
        /// <summary>
        /// 主用途
        /// </summary>
        public virtual int PurposeCode
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "address")]
        /// <summary>
        /// 楼盘地址
        /// </summary>
        public virtual string Address
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "landarea")]
        /// <summary>
        /// 占地面积
        /// </summary>
        public virtual decimal? LandArea
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "startdate")]
        /// <summary>
        /// 土地起始日期
        /// </summary>
        public virtual DateTime? StartDate
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "usableyear")]
        /// <summary>
        /// 土地使用年限
        /// </summary>
        public virtual int? UsableYear
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "buildingarea")]
        /// <summary>
        /// 建筑面积
        /// </summary>
        public virtual decimal? BuildingArea
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "salablearea")]
        /// <summary>
        /// 可销售面积
        /// </summary>
        public virtual decimal? SalableArea
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "cubagerate")]
        /// <summary>
        /// 容积率
        /// </summary>
        public virtual decimal? CubageRate
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "greenrate")]
        /// <summary>
        /// 绿化率
        /// </summary>
        public virtual decimal? GreenRate
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "buildingdate")]
        /// <summary>
        /// 开工日期
        /// </summary>
        public virtual DateTime? BuildingDate
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "coverdate")]
        /// <summary>
        /// 封顶日期
        /// </summary>
        public virtual DateTime? CoverDate
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "saledate")]
        /// <summary>
        /// 开盘日期
        /// </summary>
        public virtual DateTime? SaleDate
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "joindate")]
        /// <summary>
        /// 入伙日期
        /// </summary>
        public virtual DateTime? JoinDate
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "enddate")]
        /// <summary>
        /// 竣工日期
        /// </summary>
        public virtual DateTime? EndDate
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "innersaledate")]
        /// <summary>
        /// 内部认购日期
        /// </summary>
        public virtual DateTime? InnerSaleDate
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "rightcode")]
        /// <summary>
        /// 产权形式
        /// </summary>
        public virtual int? RightCode
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "parkingnumber")]
        /// <summary>
        /// 车位数
        /// </summary>
        public virtual int? ParkingNumber
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "averageprice")]
        /// <summary>
        /// 项目均价
        /// </summary>
        public virtual decimal? AveragePrice
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "managertel")]
        /// <summary>
        /// 管理处电话
        /// </summary>
        public virtual string ManagerTel
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "managerprice")]
        /// <summary>
        /// 物业费
        /// </summary>
        public virtual string ManagerPrice
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "totalnum")]
        /// <summary>
        /// 总套数
        /// </summary>
        public virtual int? TotalNum
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "buildingnum")]
        /// <summary>
        /// 楼宇数
        /// </summary>
        public virtual int? BuildingNum
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "detail")]
        /// <summary>
        /// 项目概况
        /// </summary>
        public virtual string Detail
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "buildingtypecode")]
        /// <summary>
        /// 主建筑物类型
        /// </summary>
        public virtual int? BuildingTypeCode
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "updatedatetime")]
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public virtual DateTime? UpdateDateTime
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "officearea")]
        /// <summary>
        /// 办公面积
        /// </summary>
        public virtual decimal? OfficeArea
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "otherarea")]
        /// <summary>
        /// 其他用途面积
        /// </summary>
        public virtual decimal? OtherArea
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "planpurpose")]
        /// <summary>
        /// 土地规划用途
        /// </summary>
        public virtual string PlanPurpose
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "pricedate")]
        /// <summary>
        /// 价格调查时间
        /// </summary>
        public virtual DateTime? PriceDate
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "iscomplete")]
        /// <summary>
        /// 是否完成基础数据
        /// </summary>
        public virtual int? IsComplete
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "othername")]
        /// <summary>
        /// 别名
        /// </summary>
        public virtual string OtherName
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
        [JsonProperty(PropertyName = "weight")]
        /// <summary>
        /// RP修正系数
        /// </summary>
        public virtual decimal? Weight
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "businessarea")]
        /// <summary>
        /// 商业面积
        /// </summary>
        public virtual decimal? BusinessArea
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "industryarea")]
        /// <summary>
        /// 工业面积
        /// </summary>
        public virtual decimal? IndustryArea
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "isevalue")]
        /// <summary>
        /// 是否可估
        /// </summary>
        public virtual int? IsEValue
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "pinyin")]
        /// <summary>
        /// 拼音简写
        /// </summary>
        public virtual string PinYin
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "cityid")]
        /// <summary>
        /// 城市ID
        /// </summary>
        public virtual int CityID
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "areaid")]
        /// <summary>
        /// 行政区
        /// </summary>
        public virtual int AreaID
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
        /// 创建日期
        /// </summary>
        public virtual DateTime? CreateTime
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "arealineid")]
        /// <summary>
        /// 环线ID
        /// </summary>
        public virtual int? AreaLineId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "valid")]
        /// <summary>
        /// 是否有效
        /// </summary>
        public virtual int? Valid
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "saleprice")]
        /// <summary>
        /// 开盘均价
        /// </summary>
        public virtual decimal? SalePrice
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "fxtcompanyid")]
        /// <summary>
        /// 评估机构ID
        /// </summary>
        public virtual int? FxtCompanyId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "pinyinall")]
        /// <summary>
        /// 楼盘名称全拼
        /// </summary>
        public virtual string PinYinAll
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "x")]
        /// <summary>
        /// X
        /// </summary>
        public virtual decimal? X
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "y")]
        /// <summary>
        /// Y
        /// </summary>
        public virtual decimal? Y
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "xyscale")]
        /// <summary>
        /// 比例尺
        /// </summary>
        public virtual int? XYScale
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
        [JsonProperty(PropertyName = "isempty")]
        /// <summary>
        /// 是否空楼盘
        /// </summary>
        public virtual int? IsEmpty
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "totalid")]
        /// <summary>
        /// 总楼盘ID
        /// </summary>
        public virtual int? TotalId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "east")]
        /// <summary>
        /// 东
        /// </summary>
        public virtual string East
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "west")]
        /// <summary>
        /// 西
        /// </summary>
        public virtual string West
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "south")]
        /// <summary>
        /// 南
        /// </summary>
        public virtual string South
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "north")]
        /// <summary>
        /// 北
        /// </summary>
        public virtual string North
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "fxtprojectid")]
        /// <summary>
        /// FxtProjectId
        /// </summary>
        public virtual int? FxtProjectId
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
        [JsonProperty(PropertyName = "__hibernate_sort_row")]
        /// <summary>
        /// 排序
        /// </summary>
        public virtual long __hibernate_sort_row
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "parkingstatus")]
        /// <summary>
        /// 停车状况
        /// </summary>
        public virtual int? ParkingStatus
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "managerquality")]
        /// <summary>
        /// 物业管理质量
        /// </summary>
        public virtual int ManagerQuality
        {
            get;
            set;
        }
    }
}
