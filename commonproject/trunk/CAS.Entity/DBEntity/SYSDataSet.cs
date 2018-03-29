using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_DataSet")]
    public class SYSDataSet : BaseTO
    {
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
        private int _certvalidate;
        /// <summary>
        /// 估价师证书到期天数
        /// </summary>
        public int certvalidate
        {
            get { return _certvalidate; }
            set { _certvalidate = value; }
        }
        private string _certmanageruserids;
        /// <summary>
        /// 估价师证书管理人员
        /// </summary>
        public string certmanageruserids
        {
            get { return _certmanageruserids; }
            set { _certmanageruserids = value; }
        }
        private DateTime _createtime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createtime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        private int _createrid;
        /// <summary>
        /// 创建人
        /// </summary>
        public int createrid
        {
            get { return _createrid; }
            set { _createrid = value; }
        }
    }
}
