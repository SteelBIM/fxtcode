using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Report_BackUp_History")]
    public class DatReportBackUpHistory : BaseTO
    {
        private long _id;
        /// <summary>
        /// 自增ID
        /// </summary>
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _companyid;
        /// <summary>
        /// 公司ID
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private int _subcompanyid;
        /// <summary>
        /// 分公司
        /// </summary>
        public int subcompanyid
        {
            get { return _subcompanyid; }
            set { _subcompanyid = value; }
        }
        private int? _reporttype;
        /// <summary>
        /// 报告类型
        /// </summary>
        public int? reporttype
        {
            get { return _reporttype; }
            set { _reporttype = value; }
        }
        private string _reportno;
        /// <summary>
        /// 报告编号
        /// </summary>
        public string reportno
        {
            get { return _reportno; }
            set { _reportno = value; }
        }
        private string _projectname;
        /// <summary>
        /// 项目名称
        /// </summary>
        public string projectname
        {
            get { return _projectname; }
            set { _projectname = value; }
        }
        private string _clientname;
        /// <summary>
        /// 委托方
        /// </summary>
        public string clientname
        {
            get { return _clientname; }
            set { _clientname = value; }
        }
        private DateTime? _completedate;
        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? completedate
        {
            get { return _completedate; }
            set { _completedate = value; }
        }
        private int? _backupuserid;
        /// <summary>
        /// 归档人
        /// </summary>
        public int? backupuserid
        {
            get { return _backupuserid; }
            set { _backupuserid = value; }
        }
        private int? _backupdatauser;
        /// <summary>
        /// 归档资料负责人
        /// </summary>
        public int? backupdatauser
        {
            get { return _backupdatauser; }
            set { _backupdatauser = value; }
        }
        private DateTime? _backupdate;
        /// <summary>
        /// 归档时间
        /// </summary>
        public DateTime? backupdate
        {
            get { return _backupdate; }
            set { _backupdate = value; }
        }
        private int? _reportwriter;
        /// <summary>
        /// 报告撰写人
        /// </summary>
        public int? reportwriter
        {
            get { return _reportwriter; }
            set { _reportwriter = value; }
        }
        private string _backupcontent;
        /// <summary>
        /// 归档资料
        /// </summary>
        public string backupcontent
        {
            get { return _backupcontent; }
            set { _backupcontent = value; }
        }

        private DateTime _createdate;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private int _createuserid;
        /// <summary>
        /// 创建人
        /// </summary>
        public int createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private bool _valid;
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

        [SQLReadOnly]
        public string reporttypename { get; set; }
        [SQLReadOnly]
        public string backupusername { get; set; }
        [SQLReadOnly]
        public string backupdatausername { get; set; }
        [SQLReadOnly]
        public string writername { get; set; }
        [SQLReadOnly]
        public string subcompanyname { get; set; }
    }
}