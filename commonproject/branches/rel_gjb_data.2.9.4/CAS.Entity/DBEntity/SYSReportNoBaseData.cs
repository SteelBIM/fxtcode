using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity
{
    [Serializable]
    [TableAttribute("dbo.SYS_ReportNoBaseData")]
    public class SYSReportNoBaseData : BaseTO
    {
        private int _baseid;
        [SQLField("baseid", EnumDBFieldUsage.PrimaryKey)]
        public int baseid
        {
            get { return _baseid; }
            set { _baseid = value; }
        }
        private int _companyid;
        /// <summary>
        /// 公司ID
        /// </summary>
        [SQLField("companyid", EnumDBFieldUsage.PrimaryKey)]
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private string _columnvalue;
        /// <summary>
        /// 列的值
        /// </summary>
        public string columnvalue
        {
            get { return _columnvalue; }
            set { _columnvalue = value; }
        }
        private string _tablename;
        /// <summary>
        /// 关联表名
        /// </summary>
        public string tablename
        {
            get { return _tablename; }
            set { _tablename = value; }
        }
        private string _shortname;
        /// <summary>
        /// 列值对应的简写
        /// </summary>
        public string shortname
        {
            get { return _shortname; }
            set { _shortname = value; }
        }
        private string _description;
        /// <summary>
        /// 描述
        /// </summary>
        public string description
        {
            get { return _description; }
            set { _description = value; }
        }
        private int? _parentid;
        /// <summary>
        /// 所属父级id
        /// </summary>
        public int? parentid
        {
            get { return _parentid; }
            set { _parentid = value; }
        }
    }
}