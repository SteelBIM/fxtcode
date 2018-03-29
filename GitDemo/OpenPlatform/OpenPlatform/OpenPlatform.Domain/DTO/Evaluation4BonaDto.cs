using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Domain.DTO
{

    public class EntrustAppraise4BonaDto
    {
        public Evaluation4BonaDto EntrustAppraise { get; set; }
    }
    
    /// <summary>
    /// 委估业务
    /// </summary>
    public class Evaluation4BonaDto
    {

        /// <summary>
        /// 委估业务ID
        /// </summary>
        public long GjbEntrustId { get; set; }
        /// <summary>
        /// 委托方户籍
        /// </summary>
        public string EntrustCensusRegister { get; set; }

        /// <summary>
        /// 评估公司ID
        /// </summary>
        public int FxtCompanyId { get; set; }

        /// <summary>
        /// 委托方联系人
        /// </summary>
        public string ClientContact { get; set; }
        /// <summary>
        /// 委托方联系人电话
        /// </summary>
        public string ClientContactPhone { get; set; }

        /// <summary>
        /// 委托方省份证
        /// </summary>
        public string EntrustIDNum { get; set; }
        /// <summary>
        /// 委托方电话
        /// </summary>
        public string EntrustPhone { get; set; }
        /// <summary>
        /// 购买方式
        /// </summary>
        public string BuyingType { get; set; }
        /// <summary>
        /// 贷款银行
        /// </summary>
        public string LendingBank { get; set; }
        /// <summary>
        /// 按揭贷款是否有共同保证人
        /// </summary>
        public string Guarantor4Mortgage { get; set; }

        /// <summary>
        /// 评估机构
        /// </summary>
        public string AppraiseAgency { get; set; }
        /// <summary>
        /// 估价师
        /// </summary>
        public string Appraiser { get; set; }
        /// <summary>
        /// 业务分配人
        /// </summary>
        public string Assigner { get; set; }
        /// <summary>
        /// 银行申请状态
        /// </summary>
        public string ApplicationStatus { get; set; }
        /// <summary>
        /// 评估状态
        /// </summary>
        public string AppraiseStatus { get; set; }

        /// <summary>
        /// 委托人和产权人关系
        /// </summary>
        public string EntrustAndPropertyOwnerRelation { get; set; }
        /// <summary>
        /// 借款人是否为产权人
        /// </summary>
        public string BorrowerIsPropertyOwner { get; set; }

        /// <summary>
        /// 评估目的    
        /// </summary>
        public string AppraisePurpose { get; set; }

        /// <summary>
        /// 中介公司
        /// </summary>
        public string RealEstateAgency { get; set; }
        /// <summary>
        /// 中介经纪人
        /// </summary>
        public string RealEstateBroker { get; set; }

        /// <summary>
        ///  委估对象
        /// </summary>
        public List<EntrustObject4BonaDto> EntrustObject { get; set; }
        
    }

   /// <summary>
   /// 委估对象
   /// </summary>
   public class EntrustObject4BonaDto
   {

       /// <summary>
       /// 委估对象ID
       /// </summary>
       public long GjbObjId { get; set; }
       /// <summary>
       /// 城市CODE
       /// </summary>
       public string CityCode { get; set; }

       /// <summary>
       /// 城市名
       /// </summary>
       public string CityName { get; set; }

       /// <summary>
       /// 行政区code
       /// </summary>
       public string AreaCode { get; set; }

       /// <summary>
       /// 行政区
       /// </summary>
       public string AreaName { get; set; }

       /// <summary>
       /// 楼盘名称
       /// </summary>
       public string ProjectName { get; set; }
       /// <summary>
       /// 地址
       /// </summary>
       public string Address { get; set; }
       /// <summary>
       /// 楼面地价
       /// </summary>
       public decimal? LandValueInTermsOfPerUnitFloor { get; set; }

       /// <summary>
       /// 楼栋名称
       /// </summary>
       public string BuildingName { get; set; }
       /// <summary>
       /// 建筑结构
       /// </summary>
       public string BuildingStructure { get; set; }
       /// <summary>
       /// 建筑年代
       /// </summary>
       public string BuildingDate { get; set; }

       /// <summary>
       /// 总楼层
       /// </summary>
       public string TotalFloor { get; set; }

       /// <summary>
       /// 房号名称
       /// </summary>
       public string HouseName { get; set; }
       /// <summary>
       /// 楼层
       /// </summary>
       public string Floor { get; set; }
       /// <summary>
       /// 建筑面积
       /// </summary>
       public decimal? BuildingArea { get; set; }
       /// <summary>
       /// 房间数
       /// </summary>
       public string RoomNum { get; set; }
       /// <summary>
       /// 阳台数
       /// </summary>
       public string BalconyNum { get; set; }
       /// <summary>
       /// 土地面积
       /// </summary>
       public decimal? LandArea { get; set; }
       /// <summary>
       /// 实用面积
       /// </summary>
       public decimal? PracticalArea { get; set; }
       /// <summary>
       /// 装修
       /// </summary>
       public string Fitment { get; set; }
       /// <summary>
       /// 委估对象全称
       /// </summary>
       public string ObjectFullName { get; set; }
       /// <summary>
       /// 购买日期
       /// </summary>
       public DateTime? TranDate { get; set; }
       /// <summary>
       /// 房产证证载价格
       /// </summary>
       public decimal? PropertyCertificateRegistePrice { get; set; }
       /// <summary>
       /// 房产证注册日期
       /// </summary>
       public DateTime? PropertyCertificateRegisteDate { get; set; }
       /// <summary>
       /// 拟贷金额
       /// </summary>
       public decimal? PrepareLoanAmount { get; set; }

       /// <summary>
       /// 首付金额
       /// </summary>
       public decimal? DownPayment { get; set; }
       /// <summary>
       /// 交易价格
       /// </summary>
       public decimal? TranPrice { get; set; }
       /// <summary>
       /// 房产证号
       /// </summary>
       public string PropertyCertificateNum { get; set; }
       /// <summary>
       /// 土地所有权证注册日期
       /// </summary>
       public DateTime? LandCertificateDate { get; set; }
       /// <summary>
       /// 土地使用权面积
       /// </summary>
       public decimal? LandCertificateArea { get; set; }
       /// <summary>
       /// 土地证载地址
       /// </summary>
       public string LandCertificateAddress { get; set; }
       /// <summary>
       /// 是否首次购买
       /// </summary>
       public string IsFirstBuy { get; set; }
       /// <summary>
       /// 查勘员
       /// </summary>
       public string Surveyor { get; set; }
       /// <summary>
       /// 查勘开始时间
       /// </summary>
       public DateTime? SurveyBeginTime { get; set; }
       /// <summary>
       /// 查勘结束时间
       /// </summary>
       public DateTime? SurveyEndTime { get; set; }
       /// <summary>
       /// 是否到现场
       /// </summary>
       public string IsSurvey { get; set; }
       /// <summary>
       /// 云查勘照片列表
       /// </summary>
       public List<string> PicturePathList { get; set; }
       /// <summary>
       /// 融资目的
       /// </summary>
       public string FinancingPurpose { get; set; }
       /// <summary>
       /// 实用情况
       /// </summary>
       public string Usage { get; set; }
       /// <summary>
       /// 装修价值
       /// </summary>
       public decimal? DecorationValue { get; set; }
       /// <summary>
       /// 公交线路数量
       /// </summary>
       public string BusLineNum { get; set; }
       /// <summary>
       /// 房产位置
       /// </summary>
       public string HousingLocation { get; set; }
       /// <summary>
       /// 公共配套设施数量
       /// </summary>
       public string PublicFacilitiesNum { get; set; }


       /// <summary>
       /// 是否是唯一住房
       /// </summary>
       public string OnlyLivingRoom { get; set; }

       /// <summary>
       /// 层户数（原字段：Number）
       /// </summary>
       public string MembersOfLayer { get; set; }
       /// <summary>
       /// 竣工时间
       /// </summary>
       public string CompleteTime { get; set; }
       /// <summary>
       /// 楼龄
       /// </summary>
       public string SurveyHouseAge { get; set; }
       /// <summary>
       /// 外墙装修
       /// </summary>
       public string Wall { get; set; }
       /// <summary>
       /// 朝向
       /// </summary>
       public string Front { get; set; }
       /// <summary>
       /// 景观
       /// </summary>
       public string Sight { get; set; }

       /// <summary>
       /// 物管费(原字段：PropertyPrice)
       /// </summary>
       public string MaterialCost { get; set; }
       /// <summary>
       /// 小区规模 别墅栋数
       /// </summary>
       public string Villa { get; set; }
       /// <summary>
       /// 小区规模 普通住宅栋数
       /// </summary>
       public string AverageHouse { get; set; }
       /// <summary>
       /// 小区规模 非普通住宅栋数
       /// </summary>
       public string NotAverageHouse { get; set; }
       /// <summary>
       /// 绿化环境
       /// </summary>
       public string GreenEnvironment { get; set; }
       /// <summary>
       /// 空气质量
       /// </summary>
       public string AirQuality { get; set; }
       /// <summary>
       /// 土地使用权类型
       /// </summary>
       public string LandUseType { get; set; }

       /// <summary>
       /// 户型结构
       /// </summary>
       public string HouseStruct { get; set; }
       /// <summary>
       /// 厅数
       /// </summary>
       public string HallCount { get; set; }
       /// <summary>
       /// 卫生间数
       /// </summary>
       public string BathroomCount { get; set; }
       /// <summary>
       /// 是否有厨房
       /// </summary>
       public string HasKitchen { get; set; }
       /// <summary>
       /// 露台面积
       /// </summary>
       public decimal Terrace { get; set; }
       /// <summary>
       /// 单层阳台个数
       /// </summary>
       public string SingleBalcony { get; set; }
       /// <summary>
       /// 单层阳台面积
       /// </summary>
       public string SingleBalconyArea { get; set; }
       /// <summary>
       /// 高挑阳台个数
       /// </summary>
       public string TallBalcony { get; set; }
       /// <summary>
       /// 高挑阳台面积
       /// </summary>
       public string TallBalconyArea { get; set; }
       /// <summary>
       /// 天台面积
       /// </summary>
       public decimal Roof { get; set; }
       /// <summary>
       /// 入户花园面积
       /// </summary>
       public decimal Garden { get; set; }
       /// <summary>
       /// 结构成新率
       /// </summary>
       public string StructNewProbability { get; set; }
       /// <summary>
       /// 层高
       /// </summary>
       public string LayerHigh { get; set; }
       /// <summary>
       /// 通风采光
       /// </summary>
       public string Ventilation { get; set; }
       /// <summary>
       /// 噪声污染
       /// </summary>
       public string NoisePollution { get; set; }
       /// <summary>
       /// 装修成新率
       /// </summary>
       public string DecorationProbabilit { get; set; }
       /// <summary>
       /// 装修年代
       /// </summary>
       public string LvYear { get; set; }
       /// <summary>
       /// 客厅-天花
       /// </summary>
       public string ParlorCeiling { get; set; }
       /// <summary>
       /// 客厅-墙面
       /// </summary>
       public string ParlorWall { get; set; }
       /// <summary>
       /// 客厅-地面
       /// </summary>
       public string ParlorGround { get; set; }
       /// <summary>
       /// 卧室-天花
       /// </summary>
       public string BedroomCeiling { get; set; }
       /// <summary>
       /// 卧室-墙面
       /// </summary>
       public string BedroomWall { get; set; }
       /// <summary>
       /// 卧室-地面
       /// </summary>
       public string BedroomGround { get; set; }
       /// <summary>
       /// 厨房-天花
       /// </summary>
       public string KitchenCeiling { get; set; }
       /// <summary>
       /// 厨房-墙面
       /// </summary>
       public string KitchenWall { get; set; }
       /// <summary>
       /// 厨房-地面
       /// </summary>
       public string KitchenGround { get; set; }
       /// <summary>
       /// 厨房-工作台
       /// </summary>
       public string KitchenDesk { get; set; }
       /// <summary>
       /// 厨房-吊柜
       /// </summary>
       public string SurveyHouseKitchenCupboards { get; set; }
       /// <summary>
       /// 洗手间-天花
       /// </summary>
       public string ToiletsCeiling { get; set; }
       /// <summary>
       /// 洗手间-墙面
       /// </summary>
       public string ToiletsWall { get; set; }
       /// <summary>
       /// 洗手间-地面
       /// </summary>
       public string ToiletsGround { get; set; }
       /// <summary>
       /// 洗手间-卫生洁具
       /// </summary>
       public string ToiletsHealth { get; set; }
       /// <summary>
       /// 洗手间-浴具
       /// </summary>
       public string ToiletsBath { get; set; }
       /// <summary>
       /// 洗手间座便器
       /// </summary>
       public string Toilet { get; set; }
       /// <summary>
       /// 入户门
       /// </summary>
       public string BigDoor { get; set; }
       /// <summary>
       /// 内门
       /// </summary>
       public string InDoor { get; set; }
       /// <summary>
       /// 房门
       /// </summary>
       public string RoomDoor { get; set; }
       /// <summary>
       /// 窗
       /// </summary>
       public string Window { get; set; }
       /// <summary>
       /// 智能系统
       /// </summary>
       public string IntelligentSystems { get; set; }
       /// <summary>
       /// 烟感报警
       /// </summary>
       public string SmokeSystems { get; set; }
       /// <summary>
       /// 自动喷淋
       /// </summary>
       public string SpraySystems { get; set; }
       /// <summary>
       /// 管道燃气
       /// </summary>
       public string GasSystems { get; set; }
       /// <summary>
       /// 对讲系统
       /// </summary>
       public string IntercomSystems { get; set; }
       /// <summary>
       /// 宽带
       /// </summary>
       public string Broadband { get; set; }
       /// <summary>
       /// 有线电视
       /// </summary>
       public string Cabletelevision { get; set; }
       /// <summary>
       /// 电话
       /// </summary>
       public string Phone { get; set; }
       /// <summary>
       /// 暖气
       /// </summary>
       public string Heating { get; set; }
       /// <summary>
       /// 客梯
       /// </summary>
       public string ClientElevator { get; set; }
       /// <summary>
       /// 客梯品牌
       /// </summary>
       public string CabinBrand { get; set; }
       /// <summary>
       /// 消防梯品牌
       /// </summary>
       public string LadderBrand { get; set; }
       /// <summary>
       /// 地上车位
       /// </summary>
       public string IsUpCarLocation { get; set; }
       /// <summary>
       /// 地下车位
       /// </summary>
       public string IsDownCarLocation { get; set; }
       /// <summary>
       /// 车位是否充足
       /// </summary>
       public string IsCar { get; set; }
       /// <summary>
       /// 车户比
       /// </summary>
       public string CarOccupy { get; set; }
       /// <summary>
       /// 运动场所
       /// </summary>
       public string Movement { get; set; }
       /// <summary>
       /// 会所
       /// </summary>
       public string Club { get; set; }
       /// <summary>
       /// 社康中心
       /// </summary>
       public string HealthCente { get; set; }
       /// <summary>
       /// 邮局
       /// </summary>
       public string PostOffice { get; set; }
       /// <summary>
       /// 银行
       /// </summary>
       public string Bank { get; set; }
       /// <summary>
       /// 商场/菜市场
       /// </summary>
       public string Market { get; set; }
       /// <summary>
       /// 中学
       /// </summary>
       public string HighSchool { get; set; }
       /// <summary>
       /// 小学
       /// </summary>
       public string PrimarySchool { get; set; }
       /// <summary>
       /// 幼儿园
       /// </summary>
       public string Nursery { get; set; }
       /// <summary>
       /// 路名
       /// </summary>
       public string RoadName { get; set; }
       /// <summary>
       /// /路宽
       /// </summary>
       public string RoadWidht { get; set; }

       /// <summary>
       /// 交通便捷度
       /// </summary>
       public string TrafficConvenient { get; set; }
       /// <summary>
       /// 离公交站台距离
       /// </summary>
       public string BusDistance { get; set; }
       /// <summary>
       /// 地铁线路
       /// </summary>
       public string Metro { get; set; }
       /// <summary>
       /// 离地铁站距离
       /// </summary>
       public string SubwayDistance { get; set; }
       /// <summary>
       /// 道路及车流量
       /// </summary>
       public string RoadTrafficFlow { get; set; }

       /// <summary>
       /// 是否有交通管制
       /// </summary>
       public string TrafficManagement { get; set; }
       /// <summary>
       /// 周边住宅
       /// </summary>
       public string Side { get; set; }
       /// <summary>
       /// 小区及周边环境
       /// </summary>
       public string Environment { get; set; }
       /// <summary>
       /// 经度
       /// </summary>
       public float Loclat { get; set; }
       /// <summary>
       /// 纬度
       /// </summary>
       public float Loclng { get; set; }
       
       /// <summary>
       /// 委估对象价格
       /// </summary>
       public EntrustObjectPrice4BonaDto EntrustObjectPrice { get; set; }

       /// <summary>
       ///  产权信息
       /// </summary>
       public IList<PropertyInfo4BonaDto> PropertyInfo { get; set; }
      
       /// <summary>
       /// 买房人信息
       /// </summary>
       public IList<BuyerInfo4BonaDto> BuyerInfo { get; set; }
       
   }

    /// <summary>
    /// 委估对象价格
    /// </summary>
    public class EntrustObjectPrice4BonaDto
    {
        /// <summary>
        /// 自动估价
        /// </summary>
        public decimal? AutoPrice { get; set; }
        /// <summary>
        /// 主房单价
        /// </summary>
        public decimal? MainHouseUnitPrice { get; set; }
        /// <summary>
        /// 主房总价
        /// </summary>
        public decimal? MainHouseTotalPrice { get; set; }
        /// <summary>
        /// 附属房屋总价
        /// </summary>
        public decimal? OutbuildingTotalPrice { get; set; }
        /// <summary>
        /// 土地单价
        /// </summary>
        public decimal? LandUnitPrice { get; set; }
        /// <summary>
        /// 土地总价
        /// </summary>
        public decimal? LandTotalPrice { get; set; }
        /// <summary>
        /// 评估总价
        /// </summary>
        public decimal? AppraiseTotalPrice { get; set; }
        /// <summary>
        /// 价值时点
        /// </summary>
        public DateTime? ValueDate { get; set; }
    }

    /// <summary>
   /// 产权信息
   /// </summary>
   public class PropertyInfo4BonaDto
   {
       /// <summary>
       /// 产权人名称
       /// </summary>
       public string PersonName { get; set; }
       /// <summary>
       /// 身份证
       /// </summary>
       public string IdNum { get; set; }
       /// <summary>
       /// 所有权比例
       /// </summary>
       public float? RightPercent { get; set; }
       /// <summary>
       /// 电话
       /// </summary>       
       public string Phone { get; set; }
       /// <summary>
       /// 产权人联系人
       /// </summary>
       public string Contacts { get; set; }
       /// <summary>
       /// 与产权人关系
       /// </summary>
       public string Relation { get; set; }
       /// <summary>
       ///  产权人联系人电话
       /// </summary>
       public string ContractPhone { get; set; }
       /// <summary>
       /// 婚姻状况
       /// </summary>
       public string MaritalStatus { get; set; }
       /// <summary>
       /// 有无子女
       /// </summary>
       public string HasChildren { get; set; }
       /// <summary>
       /// 产权人户籍
       /// </summary>
       public string OwnerCensusRegister { get; set; }
   }

   /// <summary>
   /// 购房人信息
   /// </summary>
   public class BuyerInfo4BonaDto
   {
       /// <summary>
       /// 买房人信息
       /// </summary>
       public string BuyerName { get; set; }
       /// <summary>
       /// 买房人身份证
       /// </summary>
       public string BuyerIdNum { get; set; }
       /// <summary>
       /// 买房人电话
       /// </summary>
       public string BuyerPhone { get; set; }
       /// <summary>
       /// 买房人户籍
       /// </summary>
       public string BuyerCensusRegister { get; set; }
   }

}
