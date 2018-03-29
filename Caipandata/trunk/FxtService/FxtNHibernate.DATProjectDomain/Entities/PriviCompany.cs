using System;

/**
 * 作者: 曾智磊
 * 时间: 2014.4.16
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.DATProjectDomain.Entities
{
    //Privi_Company
    public class PriviCompany
    {

        /// <summary>
        /// 公司Id
        /// </summary>
        public virtual int CompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// 公司名称
        /// </summary>
        public virtual string CompanyName
        {
            get;
            set;
        }
        /// <summary>
        /// 英文名
        /// </summary>
        public virtual string EnglishName
        {
            get;
            set;
        }
        /// <summary>
        /// 别名
        /// </summary>
        public virtual string OtherName
        {
            get;
            set;
        }
        /// <summary>
        /// 城市
        /// </summary>
        public virtual int? FK_CityId
        {
            get;
            set;
        }
        /// <summary>
        /// 地址
        /// </summary>
        public virtual string Address
        {
            get;
            set;
        }
        /// <summary>
        /// 电话
        /// </summary>
        public virtual string Telephone
        {
            get;
            set;
        }
        /// <summary>
        /// 传真
        /// </summary>
        public virtual string Fax
        {
            get;
            set;
        }
        /// <summary>
        /// 公司网站
        /// </summary>
        public virtual string WebUrl
        {
            get;
            set;
        }
        /// <summary>
        /// 用户类型
        /// </summary>
        public virtual int? FK_CompanyTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// EMail
        /// </summary>
        public virtual string EMail
        {
            get;
            set;
        }
        /// <summary>
        /// 联系人
        /// </summary>
        public virtual string LinkMan
        {
            get;
            set;
        }
        /// <summary>
        /// 法人代表
        /// </summary>
        public virtual string LegalMan
        {
            get;
            set;
        }
        /// <summary>
        /// 用户类型：5006001公用用户，5006002各城市用户，5006003房讯通用户
        /// </summary>
        public virtual int FK_UserTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 创建者
        /// </summary>
        public virtual int? OwnerId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否有效
        /// </summary>
        public virtual int? CValid
        {
            get;
            set;
        }
        /// <summary>
        /// 拼音简称
        /// </summary>
        public virtual string PinYin
        {
            get;
            set;
        }
        /// <summary>
        /// 评估机构房地产评估资质
        /// </summary>
        public virtual int? HouseAptitudeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 评估机构土地评估资质
        /// </summary>
        public virtual int? LandAptitudeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 上级公司/机构
        /// </summary>
        public virtual int ParentId
        {
            get;
            set;
        }
        /// <summary>
        /// 组织机构代码证
        /// </summary>
        public virtual string CompanyCode
        {
            get;
            set;
        }
        /// <summary>
        /// logo地址
        /// </summary>
        public virtual string Logo
        {
            get;
            set;
        }
        /// <summary>
        /// 工商登记号
        /// </summary>
        public virtual string RegNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 工商登记日期
        /// </summary>
        public virtual DateTime? RegBeginDate
        {
            get;
            set;
        }
        /// <summary>
        /// 工商到期日期
        /// </summary>
        public virtual DateTime? RegEndDate
        {
            get;
            set;
        }
        /// <summary>
        /// 法人代码
        /// </summary>
        public virtual string LegalCode
        {
            get;
            set;
        }
        /// <summary>
        /// CreateDate
        /// </summary>
        public virtual DateTime? CreateDate
        {
            get;
            set;
        }
        /// <summary>
        /// 暂停
        /// </summary>
        public virtual int? Suspended
        {
            get;
            set;
        }
        /// <summary>
        /// 营业执照号码
        /// </summary>
        public virtual string BusinessNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 营业执照附件
        /// </summary>
        public virtual string BusinessFile
        {
            get;
            set;
        }

    }
}