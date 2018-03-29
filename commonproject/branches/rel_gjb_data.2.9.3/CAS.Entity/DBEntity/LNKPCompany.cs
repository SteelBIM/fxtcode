using System;
using CAS.Entity.BaseDAModels;
//创建人:曾智磊,日期:2014-06-26


namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.LNK_P_Company")]
    public class LNKPCompany : BaseTO
    {
        private int _projectid;
        [SQLField("projectid", EnumDBFieldUsage.PrimaryKey)]
        public int projectid
        {
            get { return _projectid; }
            set { _projectid = value; }
        }
        private int _companyid;
        [SQLField("companyid", EnumDBFieldUsage.PrimaryKey)]
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private int _companytype;
        [SQLField("companytype", EnumDBFieldUsage.PrimaryKey)]
        public int companytype
        {
            get { return _companytype; }
            set { _companytype = value; }
        }
        private int _cityid;
        [SQLField("cityid", EnumDBFieldUsage.PrimaryKey)]
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
    }

}
