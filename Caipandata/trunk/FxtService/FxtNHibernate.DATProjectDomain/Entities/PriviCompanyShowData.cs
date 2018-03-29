using System;

/**
 * 作者: 曾智磊
 * 时间: 2014.03.13
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.DATProjectDomain.Entities{
	 	//SYS_SubArea
    public class PriviCompanyShowData
	{


        /// <summary>
        /// 评估机构之间的数据关系表
        /// </summary>
        public virtual long Id
        {
            get;
            set;
        }
        /// <summary>
        /// FxtCompanyId
        /// </summary>
        public virtual int FxtCompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// CityId
        /// </summary>
        public virtual int CityId
        {
            get;
            set;
        }
        /// <summary>
        /// 本机构可应用数据的机构ID，例如：25,101,102
        /// </summary>
        public virtual string ShowCompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// 应用类型，暂时未定
        /// </summary>
        public virtual int? TypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// CaseCompanyId
        /// </summary>
        public virtual string CaseCompanyId
        {
            get;
            set;
        }        
		   
		   
	}
}