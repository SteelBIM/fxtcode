using System;

/**
 * 作者: 曾智磊
 * 时间: 2014.4.16
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.DATProjectDomain.Entities
{
    //LNK_P_Company
    public class LNKPCompany
    {

        public virtual ProjectPKCompanyTypePKCity LNKPCompanyPX { get; set; }
        ///// <summary>
        ///// ProjectId
        ///// </summary>
        //public virtual int ProjectId
        //{
        //    get;
        //    set;
        //}
        ///// <summary>
        ///// CompanyId
        ///// </summary>
        //public virtual int CompanyId
        //{
        //    get;
        //    set;
        //}
        ///// <summary>
        ///// CompanyType
        ///// </summary>
        //public virtual int CompanyType
        //{
        //    get;
        //    set;
        //}
        ///// <summary>
        ///// CityId
        ///// </summary>
        //public virtual int CityId
        //{
        //    get;
        //    set;
        //}

    }

    public class ProjectPKCompanyTypePKCity
    {
        /// <summary>
        /// ProjectId
        /// </summary>
        public virtual int ProjectId
        {
            get;
            set;
        }
        /// <summary>
        /// CompanyId
        /// </summary>
        public virtual int CompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// CompanyType
        /// </summary>
        public virtual int CompanyType
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
        /// 判断两个对象是否相同，这个方法需要重写
        /// </summary>
        /// <param name="obj">进行比较的对象</param>
        /// <returns>真true或假false</returns>
        public override bool Equals(object obj)
        {
            if (obj is ProjectPKCompanyTypePKCity)
            {
                ProjectPKCompanyTypePKCity pk = obj as ProjectPKCompanyTypePKCity;
                if (this.ProjectId == pk.ProjectId&&this.CompanyId==pk.CompanyId
                     && this.CityId == pk.CityId && this.CompanyType == pk.CompanyType)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}