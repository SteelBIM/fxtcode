using System;

/**
 * 作者: 曾智磊
 * 时间: 2014.03.03
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.FxtTempDomain.Entities
{
    //SYS_Code
    public class SYSCode
    {

        /// <summary>
        /// ID
        /// </summary>
        public virtual int ID
        {
            get;
            set;
        }
        /// <summary>
        /// Code
        /// </summary>
        public virtual int Code
        {
            get;
            set;
        }
        /// <summary>
        /// CodeName
        /// </summary>
        public virtual string CodeName
        {
            get;
            set;
        }
        /// <summary>
        /// CodeType
        /// </summary>
        public virtual string CodeType
        {
            get;
            set;
        }
        /// <summary>
        /// Remark
        /// </summary>
        public virtual string Remark
        {
            get;
            set;
        }
        /// <summary>
        /// SubCode
        /// </summary>
        public virtual int? SubCode
        {
            get;
            set;
        }

    }
}