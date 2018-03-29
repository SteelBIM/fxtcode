using System;

/**
 * 作者: 曾智磊
 * 时间: 2014.4.16
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.DATProjectDomain.Entities
{
    //LNK_P_Appendage
    public class LNKPAppendage
    {

        /// <summary>
        /// id
        /// </summary>
        public virtual int id
        {
            get;
            set;
        }
        /// <summary>
        /// AppendageCode
        /// </summary>
        public virtual int AppendageCode
        {
            get;
            set;
        }
        /// <summary>
        /// ProjectId
        /// </summary>
        public virtual int ProjectId
        {
            get;
            set;
        }
        /// <summary>
        /// Area
        /// </summary>
        public virtual decimal? Area
        {
            get;
            set;
        }
        /// <summary>
        /// P_AName
        /// </summary>
        public virtual string P_AName
        {
            get;
            set;
        }
        /// <summary>
        /// IsInner
        /// </summary>
        public virtual bool IsInner
        {
            get;
            set;
        }
        /// <summary>
        /// CityId
        /// </summary>
        public virtual int? CityId
        {
            get;
            set;
        }
        /// <summary>
        /// 配套等级
        /// </summary>
        public virtual int? ClassCode
        {
            get;
            set;
        }

    }
}