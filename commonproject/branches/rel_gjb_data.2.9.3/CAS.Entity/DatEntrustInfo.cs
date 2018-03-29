using System.Collections.Generic;
using CAS.Entity.BaseDAModels;
using System;

namespace CAS.Entity
{
    #region 博纳数据返回相关类
    /// <summary>
    /// 委估业务
    /// </summary>
    public class EntrustAppraise : BaseTO
    {
        /// <summary>
        /// 评估机构ID
        /// </summary>
        public int FxtCompanyId { get; set; }
        /// <summary>
        /// 业务流水号
        /// </summary>
        public long GJBEntrustId{ get; set; }
        /// <summary>
        /// 委托方户籍 1：本市户籍  2：非本市户籍
        /// </summary>
        public int EntrustCensusRegister { get; set; }
        /// <summary>
        /// 委托方电话
        /// </summary>
        public string EntrustPhone { get; set; }
        /// <summary>
        /// 委托方身份证
        /// </summary>
        public string EntrustIDNum { get; set; }
        /// <summary>
        /// 委托方联系人
        /// </summary>
        public string ClientContact { get; set; }
        /// <summary>
        /// 委托方联系人电话
        /// </summary>
        public string ClientContactPhone { get; set; }
        /// <summary>
        /// 购买类型 1：抵押2：按揭
        /// </summary>
        public int BuyingType { get; set; }
        /// <summary>
        /// 贷款银行
        /// </summary>
        public string LendingBank { get; set; }
        /// <summary>
        /// 按揭贷款是否有共同保证人 1：是 0：否
        /// </summary>
        public int Guarantor4Mortgage { get; set; }
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
        /// 银行申请状态 1：申请前 2：申请后
        /// </summary>
        public int ApplicationStatus { get; set; }
        /// <summary>
        /// 评估状态 1：已完成 0：未完成
        /// </summary>
        public int AppraiseStatus { get; set; }
        /// <summary>
        /// 委托人和产权人关系 1、本人 2、父母 3、中介 4、配偶  5、朋友
        /// </summary>
        public string EntrustAndPropertyOwnerRelation{ get; set; }
        /// <summary>
        /// 借款人是否为产权人 1是 0否
        /// </summary>
        public int BorrowerIsPropertyOwner { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public int Valid { get; set; }
        /// <summary>
        /// 评估目的
        /// </summary>
        public string EvaluationPurpose { get; set; }
        /// <summary>
        /// 中介公司
        /// </summary>
        public string RealEstateAgency { get; set; }
        /// <summary>
        /// 中介经纪人
        /// </summary>
        public string RealEstateBroker { get; set; }
        /// <summary>
        /// 业务状态 10013
        /// </summary>
        public int BusinessStateCode { get; set; }
        /// <summary>
        /// 委估对象
        /// </summary>
        public List<EntrustObject> EntrustObject { get; set; }
    }

    public class EntrustObject : BaseTO
    {
        /// <summary>
        /// 城市ID
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 区域ID
        /// </summary>
        public int? AreaId { get; set; }
        /// <summary>
        /// 委估对象ID
        /// </summary>
        public long GJBObjId { get; set; }
        /// <summary>
        /// 楼盘ID
        /// </summary>
        public long ProjectId { get; set; }
        /// <summary>
        /// 楼栋ID
        /// </summary>
        public long BuildingId { get; set; }
        /// <summary>
        /// 房号
        /// </summary>
        public long HouseId { get; set; }
        /// <summary>
        /// 城市Code
        /// </summary>
        public string CityCode { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 行政区Code
        /// </summary>
        public string AreaCode { get; set; }
        /// <summary>
        /// 行政区
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 楼面地价
        /// </summary>
        public decimal? LandValueInTermsOfPerUnitFloor { get; set; }
        /// <summary>
        /// 楼盘名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 楼栋名称
        /// </summary>
        public string BuildingName { get; set; }
        /// <summary>
        /// 建筑结构
        /// </summary>
        public string BuildingStructure { get; set; }
        /// <summary>
        /// 总楼层
        /// </summary>
        public string TotalFloor { get; set; }
        /// <summary>
        /// 房号
        /// </summary>
        public string HouseName { get; set; }
        /// <summary>
        /// 楼层
        /// </summary>
        public string Floor { get; set; }
        /// <summary>
        /// 房间数
        /// </summary>
        public int? RoomNum { get; set; }
        /// <summary>
        /// 阳台数
        /// </summary>
        public int? BalconyNum { get; set; }
        /// <summary>
        /// 建筑面积
        /// </summary>
        public string BuildingArea { get; set; }
        /// <summary>
        /// 土地面积
        /// </summary>
        public string LandArea { get; set; }
        /// <summary>
        /// 实用面积
        /// </summary>
        public string PracticalArea { get; set; }
        /// <summary>
        /// 装修
        /// </summary>
        public string Fitment { get; set; }
        /// <summary>
        /// 委估对象全称
        /// </summary>
        public string ObjectFullName { get; set; }
        /// <summary>
        /// 交易日期
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
        /// 成交价
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
        public string LandCertificateArea { get; set; }
        /// <summary>
        /// 土地证载地址
        /// </summary>
        public string LandCertificateAddress { get; set; }
        /// <summary>
        /// 是否首次购买 1是 0否
        /// </summary>
        public int? IsFirstBuy { get; set; }
        /// <summary>
        /// 查勘员
        /// </summary>
        public string Surveyor { get; set; }
        /// <summary>
        /// 是否到现场  1是 0否
        /// </summary>
        public int? IsSurvey { get; set; }
        /// <summary>
        /// 查勘开始时间
        /// </summary>
        public string SurveyBeginTime { get; set; }
        /// <summary>
        /// 查勘结束时间
        /// </summary>
        public string SurveyEndTime { get; set; }
        /// <summary>
        /// 云查勘照片列表
        /// </summary>
        public string PicturePathList { get; set; }
        /// <summary>
        /// 融资目的
        /// </summary>
        public string FinancingPurpose { get; set; }
        /// <summary>
        /// 使用情况
        /// </summary>
        public string Usage { get; set; }
        /// <summary>
        /// 装修价值
        /// </summary>
        public decimal? DecorationValue { get; set; }
        /// <summary>
        /// 公交线路数量
        /// </summary>
        public int? BusLineNum { get; set; }
        /// <summary>
        /// 房产位置
        /// </summary>
        public string HousingLocation { get; set; }
        /// <summary>
        /// 公共配套设施数量
        /// </summary>
        public int? PublicFacilitiesNum { get; set; }
        /// <summary>
        /// 自动估价价格
        /// </summary>
        public int? AutoPrice { get; set; }
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
        /// 评估总值
        /// </summary>
        public decimal? AppraiseTotalPrice { get; set; }
        /// <summary>
        /// 价值时点
        /// </summary>
        public DateTime? ValueDate { get; set; }
        /// <summary>
        /// 产权人联系人
        /// </summary>
        public string Contacts { get; set; }
        /// <summary>
        /// 与产权人关系
        /// </summary>
        public string Relation { get; set; }
        /// <summary>
        /// 产权人联系电话
        /// </summary>
        public string ContractPhone { get; set; }
        /// <summary>
        /// 婚姻状况
        /// </summary>
        public string MaritalStatus { get; set; }
        /// <summary>
        /// 有无子女
        /// </summary>
        public int? HasChildren { get; set; }
        /// <summary>
        /// 唯一住房
        /// </summary>
        public int? OnlyLivingRoom { get; set; }
        /// <summary>
        /// 朝向
        /// </summary>
        public string Front { get; set; }
        /// <summary>
        /// 景观
        /// </summary>
        public string Sight { get; set; }
        /// <summary>
        /// 土地使用权类型
        /// </summary>
        public string LandUseType { get; set; }
        /// <summary>
        /// 买房人名称
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
        public int BuyerCensusRegister { get; set; }
        /// <summary>
        /// 建筑年代
        /// </summary>
        public string BuildingDate { get; set; }
        /// <summary>
        /// 拟贷金额
        /// </summary>
        public decimal? DownPayment { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public int Valid { get; set; }
        /// <summary>
        /// 产权信息
        /// </summary>
        public List<OwnerPropertyInfo> PropertyInfo { get; set; }
        /// <summary>
        /// 买房人信息
        /// </summary>
        public List<BuyerInfo> BuyerInfo { get; set; }
    }

    public class OwnerPropertyInfo
    {
        /// <summary>
        /// 产权人名字
        /// </summary>
        public string PersonName { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string IDNum { get; set; }
        /// <summary>
        /// 所有权比例
        /// </summary>
        public string RightPercent { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 产权人户籍
        /// </summary>
        public int OwnerCensusRegister { get; set; }
        /// <summary>
        /// 产权人联系人
        /// </summary>
        public string Contacts { get; set; }
        /// <summary>
        /// 与产权人关系
        /// </summary>
        public string Relation { get; set; }
        /// <summary>
        /// 产权人联系电话
        /// </summary>
        public string ContractPhone { get; set; }
        /// <summary>
        /// 婚姻状况
        /// </summary>
        public string MaritalStatus { get; set; }
        /// <summary>
        /// 有无子女
        /// </summary>
        public int? HasChildren { get; set; }
        /// <summary>
        /// 建筑年代
        /// </summary>
        public string BuildingDate { get; set; }
    }

    public class BuyerInfo
    {
        /// <summary>
        /// 买房人名称
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
        public int BuyerCensusRegister { get; set; }
    }
    #endregion
}
