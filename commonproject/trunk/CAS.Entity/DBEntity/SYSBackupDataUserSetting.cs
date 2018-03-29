using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_BackupDataUserSetting")]
    public class SYSBackupDataUserSetting : BaseTO
    {
        private int _buid;
        /// <summary>
        /// ID
        /// </summary>
        [SQLField("buid", EnumDBFieldUsage.PrimaryKey, true)]
        public int buid
        {
            get { return _buid; }
            set { _buid = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 评估机构ID
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private bool? _isallsubcompany;
        ///// <summary>
        ///// 是否全部分支机构
        ///// </summary>
        //public bool? isallsubcompany
        //{
        //    get { return _isallsubcompany; }
        //    set { _isallsubcompany = value; }
        //}
        private string _subcompanyid;
        /// <summary>
        /// 分支构Id
        /// </summary>
        public string subcompanyid
        {
            get { return _subcompanyid; }
            set { _subcompanyid = value; }
        }
        private string _subcompanytext;
        /// <summary>
        /// 分支构名称
        /// </summary>
        public string subcompanytext
        {
            get { return _subcompanytext; }
            set { _subcompanytext = value; }
        }
        private string _objecttypecode;
        /// <summary>
        /// 报告阶段
        /// </summary>
        public string objecttypecode
        {
            get { return _objecttypecode; }
            set { _objecttypecode = value; }
        }
        private string _objecttypetext;
        /// <summary>
        /// 报告阶段名称
        /// </summary>
        public string objecttypetext
        {
            get { return _objecttypetext; }
            set { _objecttypetext = value; }
        }
        //private bool? _isallreporttype;
        ///// <summary>
        ///// 是否全部分报告类型
        ///// </summary>
        //public bool? isallreporttype
        //{
        //    get { return _isallreporttype; }
        //    set { _isallreporttype = value; }
        //}
        private string _reporttypecode;
        /// <summary>
        /// 报型类型
        /// </summary>
        public string reporttypecode
        {
            get { return _reporttypecode; }
            set { _reporttypecode = value; }
        }
        private string _reporttypetext;
        /// <summary>
        /// 报型类型名称
        /// </summary>
        public string reporttypetext
        {
            get { return _reporttypetext; }
            set { _reporttypetext = value; }
        }
        private string _backupuserrole;
        /// <summary>
        /// 归档人角色 
        /// </summary>
        public string backupuserrole
        {
            get { return _backupuserrole; }
            set { _backupuserrole = value; }
        }
        private int? _backupuserid;
        /// <summary>
        /// 归档人ID
        /// </summary>
        public int? backupuserid
        {
            get { return _backupuserid; }
            set { _backupuserid = value; }
        }
        private string _backupdatauserrole;
        /// <summary>
        /// 归档资料负责人角色 
        /// </summary>
        public string backupdatauserrole
        {
            get { return _backupdatauserrole; }
            set { _backupdatauserrole = value; }
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
        private bool? _valid;
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool? valid
        {
            get { return _valid; }
            set { _valid = value; }
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
        private DateTime _createdate;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }

        private int _backupuserroleid;
        /// <summary>
        /// 归档人角色Id  1001001001--业务员，1001001002--报告撰写人，1001001003--其他
        /// </summary>
        public int backupuserroleid
        {
            get { return _backupuserroleid; }
            set { _backupuserroleid = value; }
        }
        private int _backupdatauserroleid;
        /// <summary>
        /// 归档资料负责人角色Id  1001001001--业务员，1001001002--报告撰写人，1001001003--其他
        /// </summary>
        public int backupdatauserroleid
        {
            get { return _backupdatauserroleid; }
            set { _backupdatauserroleid = value; }
        }
    }

}
