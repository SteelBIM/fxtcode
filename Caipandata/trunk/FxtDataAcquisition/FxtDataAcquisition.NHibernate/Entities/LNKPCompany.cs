using Newtonsoft.Json;
using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace FxtDataAcquisition.NHibernate.Entities
{
    public class ProjectPKCompanyTypePKCity
    {
        [JsonProperty(PropertyName = "projectid")]
        /// <summary>
        /// ProjectId
        /// </summary>
        public virtual int ProjectId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "companytype")]
        /// <summary>
        /// CompanyType
        /// </summary>
        public virtual int CompanyType
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "cityid")]
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
                if (this.ProjectId == pk.ProjectId
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
    //LNK_P_Company
    public class LNKPCompany
    {
        public virtual ProjectPKCompanyTypePKCity LNKPCompanyPX { get; set; }
        //[JsonProperty(PropertyName = "projectid")]
        ///// <summary>
        ///// ProjectId
        ///// </summary>
        //public virtual int ProjectId
        //{
        //    get;
        //    set;
        //}
        [JsonProperty(PropertyName = "companyname")]
        /// <summary>
        /// CompanyName
        /// </summary>
        public virtual string CompanyName
        {
            get;
            set;
        }
        //[JsonProperty(PropertyName = "companytype")]
        ///// <summary>
        ///// CompanyType
        ///// </summary>
        //public virtual int CompanyType
        //{
        //    get;
        //    set;
        //}
        //[JsonProperty(PropertyName = "cityid")]
        ///// <summary>
        ///// CityId
        ///// </summary>
        //public virtual int CityId
        //{
        //    get;
        //    set;
        //}

    }
}