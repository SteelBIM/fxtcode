using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_AutoAssignSetting")]
    public class SYSAutoAssignSetting : BaseTO
    {
        private int _id;
        /// <summary>
        /// 主键
        /// </summary>
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _subcompanyid;
        /// <summary>
        /// 分支机构
        /// </summary>
        public int subcompanyid
        {
            get { return _subcompanyid; }
            set { _subcompanyid = value; }
        }
        private int _settingtype;
        /// <summary>
        /// 设置类别 2018
        /// </summary>
        public int settingtype
        {
            get { return _settingtype; }
            set { _settingtype = value; }
        }
        private string _entrusttypecode;
        /// <summary>
        /// 业务类型
        /// </summary>
        public string entrusttypecode
        {
            get { return _entrusttypecode; }
            set { _entrusttypecode = value; }
        }
        private string _entrusttype;
        public string entrusttype
        {
            get { return _entrusttype; }
            set { _entrusttype = value; }
        }
        private string _businesstypecode;
        /// <summary>
        /// 技术团队
        /// </summary>
        public string businesstypecode
        {
            get { return _businesstypecode; }
            set { _businesstypecode = value; }
        }
        private string _businesstype;
        public string businesstype
        {
            get { return _businesstype; }
            set { _businesstype = value; }
        }
        private string _reporttypecode;
        /// <summary>
        /// 报告类型
        /// </summary>
        public string reporttypecode
        {
            get { return _reporttypecode; }
            set { _reporttypecode = value; }
        }
        private string _reporttype;
        public string reporttype
        {
            get { return _reporttype; }
            set { _reporttype = value; }
        }
        private double _settinghours;
        /// <summary>
        /// 设置时间（小时）
        /// </summary>
        public double settinghours
        {
            get { return _settinghours; }
            set { _settinghours = value; }
        }
        private int _ruletype;
        /// <summary>
        /// 规则类型
        /// </summary>
        public int ruletype
        {
            get { return _ruletype; }
            set { _ruletype = value; }
        }
        private string _containusers;
        /// <summary>
        /// 指定人员
        /// </summary>
        public string containusers
        {
            get { return _containusers; }
            set { _containusers = value; }
        }
        private string _containusernames;
        /// <summary>
        /// 指定人员
        /// </summary>
        public string containusernames
        {
            get { return _containusernames; }
            set { _containusernames = value; }
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
        private int _isenable;
        /// <summary>
        /// 是否启用
        /// </summary>
        public int isenable
        {
            get { return _isenable; }
            set { _isenable = value; }
        }
        private int _valid;
        /// <summary>
        /// 是否删除
        /// </summary>
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        
        /// <summary>
        /// 分支机构
        /// </summary>
        [SQLReadOnly]
        public string subcompanyname { get; set; }
        /// <summary>
        /// 设置类别 中文
        /// </summary>
        [SQLReadOnly]
        public string settingtypetext { get; set; }
    }
}