using CAS.Entity.BaseDAModels;
using System;

/**
 * 作者: 李晓东
 * 时间: 2014-01-22
 * 摘要: 新建实体类
 *       修改人:李晓东  2014.05.21
 *       删除字段 OldLoanBalanceOldReassessment、OldReassessment
 * **/
namespace FxtNHibernate.FxtLoanDomain.Entities
{
    /// <summary>
    ///Data_Collateral
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.Data_Collateral")]
    public class DataCollateral:BaseTO
    {

        /// <summary>
        /// Id
        /// </summary>
        [SQLField("Id", EnumDBFieldUsage.PrimaryKey, true)]
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 押品编号
        /// </summary>
        public virtual string Number
        {
            get;
            set;
        }
       
        /// <summary>
        /// 银行ID
        /// </summary>
        public virtual int BankId
        {
            get;
            set;
        }
        /// <summary>
        /// 分行
        /// </summary>
        public virtual string Branch
        {
            get;
            set;
        }
        /// <summary>
        /// 押品类型(code)
        /// </summary>
        public virtual int PurposeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 押品类型(字符串)
        /// </summary>
        public virtual string PurposeName
        {
            get;
            set;
        }
        /// <summary>
        /// 押品名称
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 面积
        /// </summary>
        public virtual decimal BuildingArea
        {
            get;
            set;
        }
        /// <summary>
        /// 面积Code
        /// </summary>
        public virtual int BuildingAreaCode
        {
            get;
            set;
        }
        /// <summary>
        /// 押品地址
        /// </summary>
        public virtual string Address
        {
            get;
            set;
        }
        /// <summary>
        /// 银行分行所在城市ID
        /// </summary>
        public virtual int BranchCityId
        {
            get;
            set;
        }
        /// <summary>
        /// 楼盘Id
        /// </summary>
        public virtual int ProjectId
        {
            get;
            set;
        }
        /// <summary>
        /// 省份
        /// </summary>
        public virtual int ProvinceId
        {
            get;
            set;
        }
        /// <summary>
        /// 城市
        /// </summary>
        public virtual int CityId
        {
            get;
            set;
        }
        /// <summary>
        /// 行政区
        /// </summary>
        public virtual int AreaId
        {
            get;
            set;
        }
        /// <summary>
        /// 片区ID
        /// </summary>
        public virtual int? SubAreaId
        {
            get;
            set;
        }
        /// <summary>
        /// 楼栋ID
        /// </summary>
        public virtual int? BuildingId
        {
            get;
            set;
        }
        /// <summary>
        /// 房号ID
        /// </summary>
        public virtual int? RoomId
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
        /// 楼盘地址
        /// </summary>
        public virtual string ProjectAddress
        {
            get;
            set;
        }
        /// <summary>
        /// 分期
        /// </summary>
        public virtual string Installment
        {
            get;
            set;
        }
        /// <summary>
        /// 楼盘类型
        /// </summary>
        public virtual string ProjectType
        {
            get;
            set;
        }
        /// <summary>
        /// 路
        /// </summary>
        public virtual string Road
        {
            get;
            set;
        }
        /// <summary>
        /// 号
        /// </summary>
        public virtual string RoadNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 楼栋编号
        /// </summary>
        public virtual string BuildingNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 楼层
        /// </summary>
        public virtual string FloorNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 房号
        /// </summary>
        public virtual string RoomNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 业务员
        /// </summary>
        public virtual string Clerk
        {
            get;
            set;
        }
        /// <summary>
        /// 中介机构
        /// </summary>
        public virtual string Agency
        {
            get;
            set;
        }
        /// <summary>
        /// 首付
        /// </summary>
        public virtual decimal? FirstPay
        {
            get;
            set;
        }
        /// <summary>
        /// 月付
        /// </summary>
        public virtual decimal? MonthsPay
        {
            get;
            set;
        }
        /// <summary>
        /// 贷款日期
        /// </summary>
        public virtual DateTime? LoanDate
        {
            get;
            set;
        }
        /// <summary>
        /// 贷款年限
        /// </summary>
        public virtual decimal? LoanYear
        {
            get;
            set;
        }
        /// <summary>
        /// 贷款利率
        /// </summary>
        public virtual decimal? LoanInterestRate
        {
            get;
            set;
        }
        /// <summary>
        /// 贷款成数
        /// </summary>
        public virtual decimal? LoanCardinalNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 贷款额度
        /// </summary>
        public virtual decimal? LoanAmount
        {
            get;
            set;
        }        
        /// <summary>
        /// 贷款余额
        /// </summary>
        public virtual decimal? LoanBalance
        {
            get;
            set;
        }
        /// <summary>
        /// 贷款人
        /// </summary>
        public virtual string LoanPeople
        {
            get;
            set;
        }
        /// <summary>
        /// 性别
        /// </summary>
        public virtual int? LoanSex
        {
            get;
            set;
        }
        /// <summary>
        /// 年龄
        /// </summary>
        public virtual int? LoanAge
        {
            get;
            set;
        }
        /// <summary>
        /// 身份证
        /// </summary>
        public virtual string LoanCardID
        {
            get;
            set;
        }
        /// <summary>
        /// 户籍
        /// </summary>
        public virtual string LoanCensus
        {
            get;
            set;
        }
        /// <summary>
        /// 职业
        /// </summary>
        public virtual string LoanProfession
        {
            get;
            set;
        }
        /// <summary>
        /// 学历
        /// </summary>
        public virtual string LoanEducationalBackground
        {
            get;
            set;
        }
        /// <summary>
        /// 婚姻状况
        /// </summary>
        public virtual string LoanMarriage
        {
            get;
            set;
        }
        /// <summary>
        /// 薪水
        /// </summary>
        public virtual decimal? LoanSalary
        {
            get;
            set;
        }
        /// <summary>
        /// 信用等级
        /// </summary>
        public virtual string CreditRating
        {
            get;
            set;
        }
        /// <summary>
        /// 贷款人公司
        /// </summary>
        public virtual string LoanCompany
        {
            get;
            set;
        }
        /// <summary>
        /// 授信额度
        /// </summary>
        public virtual decimal? LineOfcredit
        {
            get;
            set;
        }
        /// <summary>
        /// 状态 1.匹配2.未匹配
        /// </summary>
        public virtual int? Status
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime? CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 0.正式库 1.临时库
        /// </summary>
        public virtual int? MatchStatus
        {
            get;
            set;
        }
         /// <summary>
        /// 原估值
        /// </summary>
        public virtual decimal OldRate
        {
            get;
            set;
        }
         /// <summary>
        /// 担保金额
        /// </summary>
        public virtual decimal GuaranteePrice
        {
            get;
            set;
        }

        /// <summary>
        /// 原抵押率
        /// </summary>
        public virtual decimal OldMortgageRates
        {
            get;
            set;
        }

        /// <summary>
        /// 所属文件ID
        /// </summary>
        public virtual int UploadFileId
        {
            get;
            set;
        }
        /// <summary>
        /// 银行所属项目ID
        /// </summary>
        public virtual int BankProjectId
        {
            get;
            set;
        }
    }
}